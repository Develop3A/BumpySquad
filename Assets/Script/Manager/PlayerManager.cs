using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager pm;

    public Transform hp_parent;
    public GameObject hp;

    public Squad_Player sp;
    public Cooltime_timer[] timers;

    void Awake()
    {
        pm = this;
        sp = FindObjectOfType<Squad_Player>();
    }

    public void Gen_hp_text(Soldier target_s)
    {
        GameObject g = Instantiate(hp, hp_parent);
        g.GetComponent<TextFollow>().s = target_s;
    }

    public void Set_skill(int skill_number, Cooltime_timer ct)
    {
        //게임시작 이전에 UDM에서 호출하여 플레이어의 스킬을 설정함.
        switch (skill_number)
        {
            case -1:
                break;
            case 0:
                sp.gameObject.AddComponent<Skill_Dash>();
                //엑셀을 참조하든 어쩌든 수치수정가능하게
                sp.skills[ct.Skill_Order] = sp.GetComponent<Skill_Dash>();
                ct.cool_time = sp.skills[ct.Skill_Order].cooltime;
                break;
            case 1:
                sp.gameObject.AddComponent<Skill_Turnback>();
                //엑셀을 참조하든 어쩌든 수치수정가능하게
                sp.skills[ct.Skill_Order] = sp.GetComponent<Skill_Turnback>();
                ct.cool_time = sp.skills[ct.Skill_Order].cooltime;
                break;
        }
    }
    public void Use_skill(int value)
    {
        //UI에서 스킬 조작을 인식함.
        if (timers[value].present > 0) return;
        sp.skills[value].Use();
        Start_Cooltime_timer(value);
    }

    public void Set_Cooltime_timer(Cooltime_timer ct, float cooltime)
    {
        ct.cool_time = cooltime;
    }
    public void Start_Cooltime_timer(int value)
    {
        timers[value].Start_Cooltime();
    }
}
