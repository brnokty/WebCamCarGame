using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private TMP_Text orderNumber;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text raceTime;


    public void SetLine(string order, string name, string time)
    {
        gameObject.SetActive(true);
        orderNumber.text = order;
        playerName.text = name;
        raceTime.text = time;
    }
}