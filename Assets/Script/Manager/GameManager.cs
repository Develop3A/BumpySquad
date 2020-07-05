using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;
    public List<Squad> squads;
    
    void Awake()
    {
        gm = this;

        StartCoroutine("Init_game");
    }

    public void Squad_List_Refresh()
    {
        squads.Clear();
        Squad[] ss = FindObjectsOfType<Squad>();

        foreach (Squad s in ss)
            squads.Add(s);
    }
    public void Squad_Ready()
    {
        foreach(Squad s in squads)
        {
            s.Ready();
        }
    }
    public void Squad_SetActive(bool value)
    {
        foreach(Squad s in squads)
        {
            s.Set_Active(value);
        }
    }

    IEnumerator Init_game()
    {
        Squad_List_Refresh();
        //Debug.Log("Refresh");
        yield return new WaitForSeconds(0.5f);
        Squad_Ready();
        //Debug.Log("Ready");
        yield return new WaitForSeconds(0.5f);
        Squad_SetActive(true);


        yield return null;
    }


    



}
