﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_property : MonoBehaviour {
    

    [Header("Squad Option")]
    [Header("분대의 최대속도 관련")]
    public float max_straight_speed_persecond;
    //public float max_curve_speed_persecond;
    //public float max_dash_speed_persecond;
    public float max_contact_speed_persecond;
    [Range(0, 1)] public float max_mire_speed_ratio;

    [Header("분대의 가속도 관련")]
    public float accel_;
    //public float curve_decel_;

    [Header("분대의 접촉판정 관련")]
    public float yellowboxsize;
    public float plus_z;

    [Header("각도 관련")]
    public Transform rot_target;
    public float curve_delay_time;
    public bool isCurve_delay;

    [Header("지속시간 관련")]
    protected float sturn_duration;
    
}
