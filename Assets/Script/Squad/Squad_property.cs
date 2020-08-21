using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_property : MonoBehaviour {
    
    public enum Squad_Category { Full_squad,Solo_squad }
    [Header("Squad Option")]
    public Squad_Category Squad_Type = Squad_Category.Full_squad;
    [Space(10)]
    [Header("분대의 속도 관련")]
    public float maxSpeedReachTime;
    public float maxSpeed;
    public float minSpeed;
    public float accel;
    public float turnDecelSpeed;
    public float rotateDecelTime;
    public float rotateDecelSpeed;
    public bool collisionPower;
    [Range(0, 1)] public float mire_speed_ratio;

    [Header("커브 관련")]
    public float rotate_push_time;
    public bool is_rotate_able;
    protected bool input_rotate;

    //[Header("지속시간 관련")]
    protected float sturn_duration;
    protected float yellowboxsize = 4.0f;
    protected float plus_z;
    protected Transform rot_target;

}
