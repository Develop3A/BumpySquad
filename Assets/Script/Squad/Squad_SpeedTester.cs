using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_SpeedTester : Squad {

    public override void Set_Active(bool value)
    {
        if (value)
        {
            isActive = true;
            StartCoroutine("Active");
            //Debug.Log(gameObject.name + " active");
        }
        else
        {
            isActive = false;
        }
    }
    public override void Ready()
    {
        rigid = GetComponent<Rigidbody>();
        //Set_Active(true);
    }
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "rock")
        {
           // Debug.Log(rigid.velocity);
            //Debug.Log("back");
            rigid.velocity = -rigid.velocity;
            //Debug.Log(rigid.velocity);
        }
    }

    protected override IEnumerator Active()
    {
        while (isActive)
        {
            /*
            Sum_speed(accel);
            float yv = rigid.velocity.y;
            Vector3 vel = transform.forward * speed * Application.targetFrameRate * Time.deltaTime;
            rigid.velocity = new Vector3(vel.x, yv, vel.z);
            */
            rigid.AddForce(transform.forward * accel * Time.deltaTime * Application.targetFrameRate, ForceMode.Acceleration);

            if (isCurving)
            {
                if (curve_isright && !isCurve_delay) transform.Rotate(Vector3.up * rotation_speed);
                else if (!isCurve_delay) transform.Rotate(Vector3.up * -rotation_speed);
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

}
