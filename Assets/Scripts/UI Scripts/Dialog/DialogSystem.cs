using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Speaker { Player, Glasya }

public struct Dialog 
{
    public int id;
    public string dialogText;
    public Speaker speaker;

    public Dialog(int id, string dialogText, Speaker speaker)
    {
        this.id = id;
        this.dialogText = dialogText;
        this.speaker = speaker;
    }
}

public class DialogSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textUI;
    [SerializeField] GameObject[] SpeakerUI;

    private List<Dialog> dialog_Lists = new List<Dialog>();

    private string currentText;
    private int currentID;
    private Speaker currentSpeaker;
    private Speaker prevSpeaker;
    private int currentIndex = 0;

    private bool isActive = false;

    private void Start()
    {
        AddTextList(0, "Hello Wolrd Hello Wolrd Hello Wolrd Hello Wolrd", Speaker.Glasya);
        AddTextList(1, "My name is James", Speaker.Player);
        AddTextList(2, "You died", Speaker.Glasya);

        StartCoroutine(DialogProgress());
    }

    void AddTextList(int id, string dialogText, Speaker speaker)
    {
        Debug.Log(speaker);
        dialog_Lists.Add(new Dialog(id, dialogText, speaker));
    }

    IEnumerator DialogProgress()
    {
        while (currentIndex < dialog_Lists.Count)
        {
            Debug.Log(currentIndex + currentSpeaker);
            currentText = dialog_Lists[currentIndex].dialogText;
            currentID = dialog_Lists[currentIndex].id;
            currentSpeaker = dialog_Lists[currentIndex].speaker;

            SetActiveDialogUI();

            yield return StartCoroutine(DialogTypingEffect());

            prevSpeaker = currentSpeaker;
            if (!isActive) currentIndex++;
        }

        SpeakerUI[(int)currentSpeaker].SetActive(false);
        yield return null;
    }

    IEnumerator DialogTypingEffect()
    {
        string showText = "";
        isActive = true;
        for (int i = 0; i< currentText.Length; i++)
        {
            showText += currentText[i];
            textUI[(int)currentSpeaker].text = showText;

            if (Input.GetMouseButton(0))
            {
                textUI[(int)currentSpeaker].text = currentText;
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        while (true)
        {
            if (Input.GetMouseButton(0)) break;
            yield return new WaitForSeconds(0.1f);
        }
        isActive = false;
    }

    void SetActiveDialogUI()
    {
        if (prevSpeaker != currentSpeaker)
        {
            Debug.Log(currentSpeaker);
            SpeakerUI[(int)prevSpeaker].SetActive(false);
            SpeakerUI[(int)currentSpeaker].SetActive(true);
        }
    }
}
