using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniTutorial : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] Item item;
    private Inventory inventory => Inventory.instance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && item == null)
        {
            textUI.gameObject.SetActive(true);
            textUI.text = text;
            Invoke(nameof(OnOFFUI), 3f);
        }
        else if (inventory != null && collision.CompareTag("Player") && !inventory.FindItem(item))
        {
            textUI.gameObject.SetActive(true);
            textUI.text = text;
            Invoke(nameof(OnOFFUI), 3f);
        }
    }

    void OnOFFUI()
    {
        textUI.gameObject.SetActive(false);
    }
}
