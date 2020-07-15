using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Chase : Enemy_AI {
    
    public override void AI_Act()
    {
        base.AI_Act();
        isActive = true;
        StartCoroutine("Chase");
    }

    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }

    IEnumerator Chase()
    {
        while (isActive)
        {
            if(!se._isActive)
            {
                AI_Act_off();
            }

            se.Set_Nav_Destination(se.player_squad.transform.position);
            yield return new WaitForSeconds(refresh_time);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 1);
    }
}
