using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Stay : Enemy_AI {

    public override void AI_Act()
    {
        base.AI_Act();
        isActive = true;
    }

    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 1);
    }
}
