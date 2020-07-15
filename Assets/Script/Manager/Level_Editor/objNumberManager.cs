using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objNumberManager : MonoBehaviour {

    private static objNumberManager obj;

    public static objNumberManager instance
    {
        get
        {
            if (obj == null)
            {
                var Instance = FindObjectOfType<objNumberManager>();
                if (Instance != null)
                {
                    obj = Instance;
                }
                else
                {
                    Debug.LogError("no obj mng");
                }
            }
            return obj;
        }
        set
        {
            obj = value;
        }
    }

    List<GameObject> obj_list = new List<GameObject>();
    public GameObject[] Rock;
    public GameObject[] TallRock;

    void Awake()
    {
        Initialize();
    }

    public GameObject Find_Prefab(string obj_name)
    {
        if (obj_list.Count < 1) Initialize();
        foreach (GameObject g in obj_list)
        {
            //Debug.Log(g.name);
            if (g.name == obj_name)
            {
                return g;
            }
        }

        Debug.LogWarning(gameObject.name+": "+obj_name + "을 찾는데에 실패하였습니다.");

        return null;
    }
    public GameObject Search_Prefab(string obj_name)
    {
        if (obj_list.Count < 1) Initialize();
        foreach (GameObject g in obj_list)
        {
            //Debug.Log(g.name);
            if (g.name == obj_name)
            {
                return g;
            }
        }
        return null;
    }

    public void Initialize()
    {
        obj_list.Clear();
        Add_list(Rock);
        Add_list(TallRock);
    }

    public void Add_list(GameObject[] objs)
    {
        foreach(GameObject g in objs)
        {
            obj_list.Add(g);
        }
    }
}
