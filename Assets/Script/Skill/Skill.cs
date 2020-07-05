using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour {

    public int Skill_Number;
    protected Squad squad;
    protected int skill_sequence;
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
