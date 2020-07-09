using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_KeepDistance : Enemy_AI {
    
    public float target_distance;
    public float goback_distance;
    Transform back_postion;


    public override void Ready()
    {
        base.Ready();
        back_postion = new GameObject().transform;
        back_postion.rotation = transform.rotation;
        back_postion.parent = transform;
        back_postion.position = transform.localPosition + transform.forward * -goback_distance;
    }
    public override void AI_Act()
    {
        base.AI_Act();
        isActive = true;
        StartCoroutine("Keep_Distance");
    }
    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }

    IEnumerator Keep_Distance()
    {
        bool dis_maintaining = false;
        Vector3 pos = Vector3.zero;
        while (isActive)
        {
            if(dis_maintaining && Vector3.Distance(se.transform.position,pos) < 0.5f)
            {
                dis_maintaining = false;
            }
            else if(dis_maintaining)
            {
            }
            else if(Vector3.Distance(se.player_squad.transform.position, se.transform.position)<target_distance)
            {
                dis_maintaining = true;
                pos = back_postion.position;
                se.Set_Nav_Destination(pos);
            }
            else
            {
                se.Set_Nav_Destination(se.player_squad.transform.position);
            }

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
