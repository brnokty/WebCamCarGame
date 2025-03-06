using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamPanel : Panel
{
    public RawImage displayImage;

    public void SetImageTexture(WebCamTexture texture)
    {
        displayImage.texture = texture;
    }
}