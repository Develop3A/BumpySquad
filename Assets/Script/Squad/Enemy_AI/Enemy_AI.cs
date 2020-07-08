using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    protected Squad_Enemy se;
    protected bool isActive = false;
    public float refresh_time = 0.1f;

    public  virtual void Ready()
    {
        se = GetComponent<Squad_Enemy>();
    }

    public virtual void AI_Act()
    {
        if (isActive) return;
        Debug.Log(gameObject.name + " AI_Act");
    }
    public virtual void AI_Act_off()
    {
        isActive = false;
        StopAllCoroutines();
    }


}
