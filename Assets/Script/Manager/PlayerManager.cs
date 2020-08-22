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
        if (sp)
        {
            switch (skill_number)
            {
                case -1:
                    break;
                case 0:
                    sp.gameObject.AddComponent<Skill_Dash>();
                    //엑셀을 참조하든 어쩌든 수치수정가능하게
                    sp.skills[ct.Skill_Order] = sp.GetComponent<Skill_Dash>();
                    sp.skills[ct.Skill_Order].Skill_Order = ct.Skill_Order;
                    break;
                case 1:
                    sp.gameObject.AddComponent<Skill_ShakeOff>();
                    //엑셀을 참조하든 어쩌든 수치수정가능하게
                    sp.skills[ct.Skill_Order] = sp.GetComponent<Skill_ShakeOff>();
                    sp.skills[ct.Skill_Order].Skill_Order = ct.Skill_Order;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("스킬 : 플레이어 스쿼드를 찾을 수 없습니다");
        }
    }
    public void Use_skill(int value)
    {
        //UI에서 스킬 조작을 인식함.
        sp.skills[value].Use();
    }

    public void Set_Cooltime_timer(Cooltime_timer ct, float cooltime)
    {
        ct.Set_Cooltime(cooltime);
    }
}
