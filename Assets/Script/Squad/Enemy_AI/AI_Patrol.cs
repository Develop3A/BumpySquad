using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrol : AI_ChaseInRange {

    public enum Patrolmode { circle_mode,ordinal_mode}
    public Patrolmode Patrol_type = Patrolmode.circle_mode;

    public Transform[] waypoints;
    public float stop_distance;
    public float wait_on_waypoint_time;
    public float dis;
    int point_order = 0;

    public override void AI_Act()
    {
        base.AI_Act();
        isActive = true;

        switch (Patrol_type)
        {
            case Patrolmode.circle_mode:
                //Debug.Log("this is circle");
                StartCoroutine("Circle_Patrol");
                break;
            case Patrolmode.ordinal_mode:
                //Debug.Log("this is ordinal");
                StartCoroutine("Ordinal_Patrol");
                break;
        }
    }
    public override void AI_Act_off()
    {
        base.AI_Act_off();
    }

    IEnumerator Ordinal_Patrol()
    {
        bool ordinal_reach_point = false;
        bool updown = true;
        while (isActive)
        {
            if (!se._isActive)
            {
                AI_Act_off();
            }

            if (Check_In_Range())
            {
                se.Set_Nav_Destination(se.player_squad.transform.position);
            }
            else if (Vector3.Distance(transform.position, waypoints[point_order].position) < stop_distance)
            {//목표한 지점에 도달했는지를 체크
                //Debug.Log(point_order);
                ordinal_reach_point = true;
            }
            else
            {
                se.Set_Nav_Destination(waypoints[point_order].position);
            }


            yield return new WaitForSeconds(refresh_time);


            if (ordinal_reach_point) //목표한 지점에 도달한 경우 호출
            {
                if (updown)
                {
                    point_order++;
                    if (point_order == waypoints.Length)
                    {
                        point_order-= 2;
                        updown = false;
                    }
                }
                else
                {
                    point_order--;
                    if(point_order == -1)
                    {
                        point_order += 2;
                        updown = true;
                    }
                }
                ordinal_reach_point = false;
                yield return new WaitForSeconds(wait_on_waypoint_time);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Circle_Patrol()
    {
        bool circle_reach_point = false;
        while (isActive)
        {
            if (!se._isActive)
            {
                AI_Act_off();
            }

            if (Check_In_Range())
            {
                se.Set_Nav_Destination(se.player_squad.transform.position);
            }
            else if (Vector3.Distance(transform.position, waypoints[point_order].position) < stop_distance)
            {//목표한 지점에 도달했는지를 체크
                //Debug.Log(point_order);
                circle_reach_point = true;
            }
            else
            {
                se.Set_Nav_Destination(waypoints[point_order].position);
            }


            yield return new WaitForSeconds(refresh_time);
            

            if (circle_reach_point) //목표한 지점에 도달한 경우 호출
            {
                point_order++;
                if (point_order == waypoints.Length) point_order = 0;
                circle_reach_point = false;
                yield return new WaitForSeconds(wait_on_waypoint_time);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
