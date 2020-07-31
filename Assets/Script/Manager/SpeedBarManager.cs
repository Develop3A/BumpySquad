using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarManager : MonoBehaviour {

    public static SpeedBarManager sbm;

    void Awake()
    {
        sbm = this;
    }

    public void Refresh(float value)
    {
        GetComponent<Image>().fillAmount = value;
        if(value == 1)
        {
            GetComponent<Animator>().SetBool("idle", false);
            GetComponent<Animator>().SetTrigger("twinkle");
        }
        else
        {
            GetComponent<Animator>().SetBool("idle",true);
        }
    }

}
