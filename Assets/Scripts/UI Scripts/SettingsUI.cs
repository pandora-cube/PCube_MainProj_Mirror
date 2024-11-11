using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public GameObject topPauseMenu;
    public GameObject[] subMenus;
    public bool isSubMenuActivated = false;
    private int activatedSubMenuIdx = 0;

    public bool isSettingsMenuActivated;

    void Awake()
    {
        isSubMenuActivated = false;
        activatedSubMenuIdx = 0;
    }

    private void FindActivatedSubMenu()
    {
        //iterate through the array of submenus
        //if submenu is active, set the bool as true and keep the index of activated sub menu.
        for (int i = 0; i < subMenus.Length; ++i)
        {
            if (!subMenus[i].activeSelf) continue;

            isSubMenuActivated = true;
            activatedSubMenuIdx = i;
        }
    }

    private void CloseActivatedSubMenu()
    {
        subMenus[activatedSubMenuIdx].SetActive(false); //turn off the submenu
        topPauseMenu.SetActive(true); //turn on the top pause menu
    }

}
