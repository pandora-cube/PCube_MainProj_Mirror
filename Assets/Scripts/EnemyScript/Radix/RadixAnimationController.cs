using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadixAnimationController : EnemyBaseAnimationController<RadixAnimationController.RadixAnimationStates>
{
    public enum RadixAnimationStates
    {
        radixIdle,
        radixEmerged,
        radixMove,
        radixAttack
    }

    void Start()
    {
        ChangeAnimationState(RadixAnimationStates.radixIdle);
    }
}
