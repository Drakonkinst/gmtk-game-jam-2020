using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Steerable
{
    public override void DoBehavior() {
        steering.Wander();
    }
}
