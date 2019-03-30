using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Vuforia;

// Define the functions which can be called from the .dll.
internal static class OpenCVInterop
{
    //const string library = "OpenCVPalmRecognition";
    const string library = "UnityOpenCVSample";

    [DllImport("OpenCVPalmRecognition")]
    internal static extern int Init(ref int outCameraWidth, ref int outCameraHeight);

    [DllImport("OpenCVPalmRecognition")]
    internal static extern int Close();

    [DllImport("OpenCVPalmRecognition")]
    internal static extern int SetScale(int downscale);

    //[DllImport("OpenCVPalmRecognition")]
    //internal unsafe static extern void Detect(CvCircle* outFaces, int maxOutFacesCount, ref int outDetectedFacesCount);
    [DllImport("OpenCVPalmRecognition")] 
    internal unsafe static extern void DetectInMat(CvCircle* outFaces, Color32[] frame, int maxOutFacesCount, ref int outDetectedFacesCount);
}

// Define the structure to be sequential and with the correct byte size(3 ints = 4 bytes* 3 = 12 bytes)
[StructLayout(LayoutKind.Sequential, Size = 12)]
public struct CvCircle
{
    public int X, Y, Radius;
}

public class OpenCVDetection : MonoBehaviour {
    
    public static List<Vector2> NormalizedFacePositions { get; private set; }
    public static Vector2 CameraResolution;

    /// <summary>
    /// Downscale factor to speed up detection.
    /// </summary>
    private const int DetectionDownScale = 1;

    private bool _ready;
    private int _maxFaceDetectCount = 5;
    private CvCircle[] _faces;
    private Image.PIXEL_FORMAT _mPixelFormat = Image.PIXEL_FORMAT.UNKNOWN_FORMAT;

    void Awake()
    {
        NormalizedFacePositions = new List<Vector2>();

        int camWidth = 0, camHeight = 0;
        int result = OpenCVInterop.Init(ref camWidth, ref camHeight);
        if (result < 0)
        {
            if (result == -1)
            {
                Debug.LogWarningFormat("[{0}] Failed to find cascades definition.", GetType());
            }
            else if (result == -2)
            {
                Debug.LogWarningFormat("[{0}] Failed to open camera stream.", GetType());
            }

            return;
        }
        CameraResolution = new Vector2(camWidth, camHeight);
        _faces = new CvCircle[_maxFaceDetectCount];
        OpenCVInterop.SetScale(DetectionDownScale);
        _ready = true;
        //if (CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true))
        //    Debug.Log("Set");

        #if UNITY_EDITOR
                _mPixelFormat = Image.PIXEL_FORMAT.GRAYSCALE;        //Need Grayscale for Editor
        #else
                _mPixelFormat = Image.PIXEL_FORMAT.RGB888;               //Need RGB888 for mobile
        #endif
                VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);

    }

    private void OnVuforiaStarted()
    {
        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(_mPixelFormat, true);
        Debug.Log("FormatSet : " + isFrameFormatSet + " to " + _mPixelFormat);
    }

    void OnApplicationQuit()
    {
        if (_ready)
        {
            OpenCVInterop.Close();
        }
    }

    void Update()
    {
        if (!_ready)
            return;

        int detectedFaceCount = 0;
        unsafe
        {
            fixed (CvCircle* outFaces = _faces)
            {
                if (CameraDevice.Instance != null && _mPixelFormat == Image.PIXEL_FORMAT.RGB888)
                {
                    Debug.Log("You crashing now?");
                    Image img = CameraDevice.Instance.GetCameraImage(_mPixelFormat);
                    Debug.Log("Maybe here?");
                    if (img != null)
                    {
                        //Color32[] mat = new Color32(red, green, blue, new byte());
                        Color32[] mat = GetBytesToColors(img);
                        //.Pixels);
                        OpenCVInterop.DetectInMat(outFaces, mat, _maxFaceDetectCount, ref detectedFaceCount);
                    }
                    else
                        Debug.Log("Image not found");
                }
                //else
                //    Debug.Log("Camera not loaded");
            }
        }

        NormalizedFacePositions.Clear();
        for (int i = 0; i < detectedFaceCount; i++)
        {
            NormalizedFacePositions.Add(new Vector2((_faces[i].X * DetectionDownScale) / CameraResolution.x, 1f - ((_faces[i].Y * DetectionDownScale) / CameraResolution.y)));
        }
    }

    public Color32[] GetBytesToColors(Image img)
    {
        byte[] pixels = img.Pixels;
        int stride = img.Stride;
        int row = img.Height;
        int col = img.Width;
        //Color32[] mat = GetColorArray();

        Color32[] temp = new Color32[pixels.Length / 4];
        //for (int i = 0; i < pixels.Length / 4; i++)
        //{
        //    temp[i] = new Color32(pixels[3 * (row * stride + col)], pixels[3 * (row * stride + col) + 1], pixels[3 * (row * stride + col) + 2], pixels[3 * (row * stride + col) + 2]);

        //}

        return GetColorArray(pixels);
        //byte red = pixels[3 * (row * stride + col)];
        //byte green = pixels[3 * (row * stride + col) + 1];
        //byte blue = pixels[3 * (row * stride + col) + 2];
    }

    public Color32[] GetColorArray(byte[] rgb888Data)
    {
        Color32[] temp = new Color32[rgb888Data.Length / 4];
        for (int i = 0; i < rgb888Data.Length / 4; i++)
        {
            temp[i] = new Color32(rgb888Data[i * 4 + 2], rgb888Data[i * 4 + 1], rgb888Data[i * 4], rgb888Data[i * 4 + 3]);
        }

        return temp;
    }

    //public Image getImage()
    //{
    //    VuforiaLocalizer.CloseableFrame frame = null;
    //    try
    //    {
    //        frame = vuforia.getFrameQueue().take();
    //        long numImages = frame.getNumImages();
    //        Image rgbImage = null;
    //        for (int i = 0; i < numImages; i++)
    //        {
    //            Image img = frame.getImage(i);
    //            int fmt = img.getFormat();
    //            if (fmt == PIXEL_FORMAT.RGB565)
    //            {
    //                rgbImage = frame.getImage(i);
    //                break;
    //            }
    //        }
    //        return rgbImage;
    //    }
    //    catch (InterruptedException exc)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        if (frame != null) frame.close();
    //    }

    //}
}
