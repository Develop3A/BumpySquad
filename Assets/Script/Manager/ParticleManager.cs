using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public static ParticleManager pm;

    public GameObject[] hit_particles;

    void Awake()
    {
        pm = this;
    }

    public void Gen_hit_particle(Transform t,int num)
    {
        GameObject g = Instantiate(hit_particles[num], t.position, Quaternion.identity, t);
        Destroy(g, 0.5f);
    }
}
