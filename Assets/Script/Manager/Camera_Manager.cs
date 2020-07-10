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
        if (target)
        {
            Set_Active(true);
        }
        else
        {
            Debug.LogWarning(gameObject.name + "의 target이 설정되어있지 않습니다.");
        }
    }

    public void Set_Active(bool value)
    {
        if (value == isActive) return;
        if(value)
        {
            isActive = true;
            StartCoroutine("Active");
        }
        else
        {
            isActive = false;
        }
    }

    IEnumerator Active()
    {
        while(isActive)
        {
            Vector3 t_pos = new Vector3(target.position.x + Sum_pos.x, target.position.y + Sum_pos.y, target.position.z + Sum_pos.z);
            if (moving) transform.position = Vector3.Lerp(transform.position, t_pos, move_speed);
            if (rotating) transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rot_speed);

            yield return new WaitForEndOfFrame();
        }
    }


}
