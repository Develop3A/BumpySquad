using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquad_Generator : Squad_Generator
{
    protected override void Ready()
    {
        if (Soldiers_parent == null) Soldiers_parent = FindObjectOfType<Squad_Player>().transform;
        Gen_Squad();
    }
}
