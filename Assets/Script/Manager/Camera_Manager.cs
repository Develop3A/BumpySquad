using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour {

    public Transform target;
    public Vector3 Sum_pos;
    public Vector3 asd;
    public bool moving = true, rotating = true;
    public float move_speed, rot_speed;
    public bool isActive;

    void Awake()
    {
        Set_Active(true);
    }

    public void Set_Active(bool value)
    {
        if(value)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }

    void Update()
    {
        Vector3 t_pos = new Vector3(target.position.x + Sum_pos.x, target.position.y + Sum_pos.y, target.position.z + Sum_pos.z);
        if(moving)transform.position = Vector3.Lerp(transform.position, t_pos, move_speed);
        if(rotating)transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rot_speed);
    }


}
