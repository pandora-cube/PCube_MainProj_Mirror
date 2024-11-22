using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHeirarchyController : MonoBehaviour
{
    Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject topMenu;
    [SerializeField] private GameObject previousTopMenu;
    [SerializeField] private bool isOnMainMenu;
    [SerializeField] PlayerInput playerInput;

    void Start()
    {
        playerInput.SwitchCurrentActionMap("PlayerActions");
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Started) return;

        //player is in-game or in menu
        if (menuStack.Count == 0)
        {
            menuStack.Push(topMenu);
            menuStack.Peek().SetActive(true);
            if (!isOnMainMenu)
            {
                Time.timeScale = 0f;
                playerInput.SwitchCurrentActionMap("UI Actions");
            }
        }
        else
        {
            //turn off current menu
            menuStack.Peek().SetActive(false);
            menuStack.Pop();

            //turn on the parent menu if there is any
            if (menuStack.Count > 0) menuStack.Peek().SetActive(true);
            else if (menuStack.Count == 0 && !isOnMainMenu)
            {
                Time.timeScale = 1f;
                playerInput.SwitchCurrentActionMap("PlayerActions");
            }
            if (menuStack.Count == 1 && isOnMainMenu)
            {
                menuStack.Clear();
                topMenu = previousTopMenu;
            }
        }
    }

    public void AddToStack(GameObject obj)
    {
        menuStack.Push(obj);
        Debug.Log(menuStack.Count);
    }

    public void SetTopMenu(GameObject obj)
    {
        previousTopMenu = topMenu;
        topMenu = obj;
        ClearStack();
        AddToStack(obj);
    }

    //when leaving submenu by clicking on button
    public void PopStack()
    {
        menuStack.Pop();
        Debug.Log(menuStack.Count);
        if (menuStack.Count == 1 && isOnMainMenu)
        {
            menuStack.Clear();
            topMenu = previousTopMenu;
        }
    }

    //menu stack needs to be cleared when leaving menu by clicking on button
    public void ClearStack()
    {
        menuStack.Clear();
        playerInput.SwitchCurrentActionMap("PlayerActions");
    }
}
