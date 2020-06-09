using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public Transform hp_parent;
    public GameObject hp;

    void Awake()
    {
        gm = this;
    }

    public void Gen_hp_text(Soldier target_s)
    {
        GameObject g = Instantiate(hp, hp_parent);
        g.GetComponent<TextFollow>().s = target_s;
    }




}
