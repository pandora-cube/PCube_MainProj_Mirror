using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoints : MonoBehaviour
{
    public Transform[] savePoints;
    [SerializeField] private Transform normalPlayer;
    int currentStage = 0;

    void Start()
    {
        PlayerRespawn();
    }

    public void PlayerRespawn()
    {
        currentStage = ProgressData.Instance.playerData.currentStage - 1;
        normalPlayer.position = new Vector3(savePoints[currentStage].position.x, savePoints[currentStage].position.y, normalPlayer.position.z);
    }
}