using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorPicker : MonoBehaviour
{
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;

    public TextMeshProUGUI _debugtxt;

    RectTransform rect;
    Camera cam;

    Texture2D _colortexture;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main;

        _colortexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    private void Update()
    {
        if(!RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition)) return;

        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out delta);

        string debug = "MousePosition=" + Input.mousePosition;
        debug += "<br>delta=" + delta;

        float width = rect.rect.width;
        float height = rect.rect.height;
        delta += new Vector2(width * .5f, height * .5f);
        debug += "<br>offset delta=" + delta;


        float x = Mathf.Clamp(delta.x / width, 0, 1);
        float y = Mathf.Clamp(delta.y / height, 0, 1);
        debug += "<br>x=" + x + " y=" +y;


        int texX = Mathf.RoundToInt(x * _colortexture.width);
        int texY = Mathf.RoundToInt(y * _colortexture.height);
        debug += "<br>texX=" + texX + " texY=" + texY;

        Color color = _colortexture.GetPixel(texX, texY);

        _debugtxt.text = debug;
        _debugtxt.color = color;

        OnColorPreview?.Invoke(color);

        if(Input.GetMouseButtonDown(0))
            OnColorSelect?.Invoke(color);
    }
}
