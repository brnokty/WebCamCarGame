using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Banner : MonoBehaviour
{
    public Renderer bannerRenderer;
    public TextMeshPro playerNameText;

    private void Start()
    {
        playerNameText.text = GameManager.Instance.playerName;
    }
}