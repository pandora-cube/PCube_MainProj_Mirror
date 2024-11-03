using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaoreAnimationController : EnemyBaseAnimationController<FleaoreAnimationController.FleaoreAnimationStates>
{
    public enum FleaoreAnimationStates
    {
        fleaoreIdle,
        fleaoreAttack
    }

    void Start()
    {
        ChangeAnimationState(FleaoreAnimationStates.fleaoreIdle);
    }
}
