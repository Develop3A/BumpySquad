using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour {

    public int Skill_Number;
    public int Skill_Order;
    protected Squad squad;
    public float cooltime;

    void Awake()
    {
        First();
    }

    protected virtual void First()
    {
        squad = GetComponent<Squad>();
    }

    public virtual void Use()
    {

    }

}
