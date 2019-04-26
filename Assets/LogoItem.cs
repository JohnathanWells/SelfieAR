using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LogoItem : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image image;
    public float scalingFactor;
    bool currentlyOn;

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void OnDrag(PointerEventData pointer)
    {
        transform.position = new Vector2(pointer.position.x, pointer.position.y);
    }

    public void OnPointerEnter(PointerEventData pointer)
    {
        currentlyOn = true;
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        currentlyOn = false;
    }

    public void ModifySize(float byFactor)
    {
        image.rectTransform.sizeDelta += image.rectTransform.sizeDelta.normalized * scalingFactor * byFactor;
    }

    void Update()
    {
        if (currentlyOn)
        {
            image.rectTransform.sizeDelta += image.rectTransform.sizeDelta.normalized * scalingFactor * Input.GetAxis("Mouse ScrollWheel");
        }
    }
}
