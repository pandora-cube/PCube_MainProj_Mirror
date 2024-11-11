using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHeirarchyController : MonoBehaviour
{
    Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject topMenu;
    bool isPaused;

    void Awake()
    {
        isPaused = false;
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Started) return;

        //player is in-game or in menu
        if (menuStack.Count == 0)
        {
            if (topMenu == null) return;
            menuStack.Push(topMenu);
            menuStack.Peek().SetActive(true);
        }
        else
        {
            //turn off current menu
            menuStack.Peek().SetActive(false);
            menuStack.Pop();

            //turn on the parent menu if there is any
            if (menuStack.Count > 0) menuStack.Peek().SetActive(true);
        }
    }

    public void AddToStack(GameObject obj)
    {
        menuStack.Push(obj);
    }

    //when leaving submenu by clicking on button
    public void PopStack()
    {
        menuStack.Pop();
    }

    //menu stack needs to be cleared when leaving menu by clicking on button
    public void ClearStack()
    {
        menuStack.Clear();
    }
}
