using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrol : AI_ChaseInRange {

    public enum Patrolmode { circle_mode,ordinal_mode}
    public Patrolmode Patrol_type = Patrolmode.ordinal_mode;

    public Transform[] waypoints;
    public float stop_distance;
    public float wait_on_waypoint_time;
    int point_order = 0;

    public override void AI_Act()
    {
        if (isActive)
        {
            Debug.LogError("is Already start Ai's Acting!");
            return;
        }
        else
        {
            isActive = true;

            switch(Patrol_type)
            {
                case Patrolmode.circle_mode:
                    StartCoroutine("Circle_Patrol");
                    break;
                case Patrolmode.ordinal_mode:
                    StartCoroutine("Ordinal_Patrol");
                    break;
            }
        }
    }
    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }


    IEnumerator Circle_Patrol()
    {
        bool reach_point = false;
        while (isActive)
        {
            if (se._isActive) { }

            if (Check_In_Range())
            {
                se.Set_Nav_Destination(se.player_squad.transform.position);
            }
            else if (Vector3.Distance(transform.position, waypoints[point_order].position) < stop_distance)
            {//목표한 지점에 도달했는지를 체크
                Debug.Log(point_order);
                reach_point = true;
            }
            else
            {
                se.Set_Nav_Destination(waypoints[point_order].position);
            }


            yield return new WaitForSeconds(refresh_time);


            if (reach_point) //목표한 지점에 도달한 경우 호출
            {
                point_order++;
                if (point_order == waypoints.Length) point_order = 0;
                reach_point = false;
                yield return new WaitForSeconds(wait_on_waypoint_time);
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
