using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour {

    public static UserDataManager UDM;

    public int userskillA, userskillB;
    
    void Awake()
    {
        UDM = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        StartCoroutine("GameInitialize");
    }

    IEnumerator GameInitialize()
    {
        PlayerManager.pm.Set_skill(userskillA, PlayerManager.pm.timers[0]);
        PlayerManager.pm.Set_skill(userskillB, PlayerManager.pm.timers[1]);
        yield return null;
    }




}
