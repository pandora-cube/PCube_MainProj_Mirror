using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interactable Object", menuName = "Scriptable Object/Interactable Object", order = int.MaxValue)]
public class InteractableObject : ScriptableObject
{
    [SerializeField] private string objectName = "";
    public string ObjectName { get { return objectName; } }

    [SerializeField] private bool human_canInteract = true;
    public bool Human_CanInteract { get { return human_canInteract; } }
    [SerializeField] private bool ghost_canInterect = false;
    public bool Ghost_CanInterect { get {  return ghost_canInterect; } }

    [SerializeField] private AudioClip soundEffect;
    public bool SoundEffect { get { return soundEffect; } }

}
