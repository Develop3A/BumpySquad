using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ChaseInRange : Enemy_AI {

    public float chase_range = 6;

    public override void AI_Act()
    {
        base.AI_Act();
        isActive = true;
        StartCoroutine("Chase_In_Range");
    }
    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }


    IEnumerator Chase_In_Range()
    {
        while(isActive)
        {
            if (!se._isActive) { }

            if(Check_In_Range())
            {
                se.Set_Nav_Destination(se.player_squad.transform.position);
                yield return new WaitForSeconds(refresh_time);
            }
            else
            {
                se.Set_Nav_Destination(se.transform.position);
                yield return new WaitForSeconds(refresh_time);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    public bool Check_In_Range()
    {
        //chase_range안에 플레이어가 있는지만 체크하는 메소드입니다.
        if (Vector3.Distance(se.transform.position, se.player_squad.transform.position) < chase_range)
            return true;
        else
            return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero,chase_range);
    }
}
