using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5_Ghost : TutoEnterTrigger
{
    [SerializeField] private PlayerGhostHealthManager playerGhostHealthManager;
    [SerializeField] private TutorialMirror mirror;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thisTutoPlaying && collision.CompareTag("Player"))
        {
            PlayTutorial(); // 대화 시작
            ApplyToGhostTimeLimit(); 
        }
    }

    void ApplyToGhostTimeLimit()
    {
        playerGhostHealthManager.SetGhostTimeLimit(60f); // ghost 제한 60초로 설정
        Debug.Log("20second start");
        mirror.tuto6 = true; // tuto6 가능하도록 설정
        StartCoroutine(CheckTuto6StatusAfterDelay(20f)); // 20초 동안 거울을 사용하지 않으면 사망
    }

    private IEnumerator CheckTuto6StatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (mirror.tuto6)
        {
            Debug.Log("tuto6 die");
            playerGhostHealthManager.Die(); // 20초 동안 거울을 사용하지 않으면 사망
        }
        else Debug.Log("무사히 넘어갓네요");
    }

}
