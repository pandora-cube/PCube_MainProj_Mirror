using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTreePassage : MonoBehaviour
{
    public enum Available { open, closed }
    public Available passageState;
    public Inventory invetory => Inventory.instance;
}
