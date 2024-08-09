using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenButton;
    List<Resolution> resolutions = new List<Resolution>();

    int resolutionNum;
    int optionNum = 0;

    void Start()
    {
        InitUI();
    }
    
    void InitUI()
    {
        foreach (Resolution res in Screen.resolutions)
        {
            float refreshRate = (float)res.refreshRateRatio.numerator / res.refreshRateRatio.denominator;
            if (Mathf.Approximately(refreshRate, 60f) && !resolutions.Contains(res))
                resolutions.Add(res);
        }
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach(Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + " x " + item.height;
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;

            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();

        fullScreenButton.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void OnFullScreenButtonToggled(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void OnApplyButtonClicked()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        //TO-DO: Save settings in JSON or PlayerPrefs
    }
}
