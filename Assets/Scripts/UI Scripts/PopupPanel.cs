using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainTextUI;
    [SerializeField] private TextMeshProUGUI subTextUI;

    [SerializeField] private string mainText;
    [SerializeField] private string subText;

    private void Awake()
    {
        SetMainText(mainText);
        SetSubText(subText);
    }
    private void SetMainText(string text)
    {
        mainTextUI.text = text;
    }

    private void SetSubText(string text)
    {
        subTextUI.text = text;
    }
}
