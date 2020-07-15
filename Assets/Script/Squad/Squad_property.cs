using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_property : MonoBehaviour {
    
    public enum Squad_Category { Full_squad,Solo_squad }
    [Header("Squad Option")]
    public Squad_Category Squad_Type = Squad_Category.Full_squad;
    [Space(10)]
    [Header("분대의 최대속도 관련")]
    public float max_speed;
    //public float max_curve_speed_persecond;
    //public float max_dash_speed_persecond;
    //public float max_contact_speed_persecond;
    [Range(0, 1)] public float mire_speed_ratio;

    [Header("분대의 가속도 관련")]
    public float accel;
    //public float curve_decel_;

    //[Header("분대의 접촉판정 관련")]
    protected float yellowboxsize;
    protected float plus_z;

    protected Transform rot_target;
    [Header("커브 관련")]
    public float curve_delay_time;
    public bool isCurve_delay;

    //[Header("지속시간 관련")]
    protected float sturn_duration;
    
}
