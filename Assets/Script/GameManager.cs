using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public Transform hp_parent;
    public GameObject hp;

    public Squad_Player squad_player;
    public Cooltime_timer Dash, Turnback;

    void Awake()
    {
        gm = this;
    }
    void Start()
    {
        squad_player = GameObject.FindObjectOfType<Squad_Player>();
    }

    public void Gen_hp_text(Soldier target_s)
    {
        GameObject g = Instantiate(hp, hp_parent);
        g.GetComponent<TextFollow>().s = target_s;
    }

    public void Use_skill(int value)
    {

        switch (value)
        {
            case 0:
                squad_player.StartCoroutine("Use_Dash");
                break;
            case 1:
                squad_player.StartCoroutine("Use_Turnback");
                break;

        }
    }
    public void Start_Cooltime_tiemr(int value)
    {
        switch (value)
        {
            case 0:
                Dash.Start_Cooltime();
                break;
            case 1:
                Turnback.Start_Cooltime();
                break;

        }

    }



}
