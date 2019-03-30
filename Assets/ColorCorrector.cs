using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ColorCorrector : MonoBehaviour {

    public CelebrityManager celebrityManager;
    public Camera camera;
    public Renderer input;
    public Renderer output;
    public Color averageColor;

	void Awake()
    {
        //averageColor = averageColour(input.material.mainTexture as Texture2D);
        //averageColor = averageColour(input.transform.GetComponent<SpriteRenderer>().sprite.);
        //output.material.color = averageColor;
        //ColorCorrect();
    }

    public void ColorCorrect()
    {
        input = camera.GetComponentInChildren<Renderer>();
        output = celebrityManager.GetCurrentlySelectedCelebrity().GetComponentInChildren<VideoPlayer>().targetMaterialRenderer;
        averageColor = averageColour(input.material.mainTexture as Texture2D);
        output.material.color = averageColor;
    }

    Color averageColour(Texture2D texture)
    {
        float red = 0;
        float green = 0;
        float blue = 0;

        int count = 0;
        Color pixel;

        int textureWidth = texture.width;
        int textureHeight= texture.height;

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                pixel = texture.GetPixel(x, y);

                red += pixel.r;
                green += pixel.g;
                blue += pixel.b;

                count++;
            }
        }

        red /= count;
        green /= count;
        blue /= count;

        Debug.Log(new Color(red, green, blue));
        return new Color(red, green, blue);
    }
}
