using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    protected Squad_Enemy se;
    protected bool isActive = false;
    public float refresh_time = 0.5f;

    public  virtual void Ready()
    {
        se = GetComponent<Squad_Enemy>();
    }

    public virtual void AI_Act()
    {
        if (isActive)
        {
            //Debug.LogError("is Already start Ai's Acting!");
            return;
        }
    }
    public virtual void AI_Act_off()
    {
        isActive = false;
        StopAllCoroutines();
    }


}
