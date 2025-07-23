using UnityEngine;
using System.Collections;
using System;

public class rec_struct
{

    //public float time_stamp;
    //public float time_delta;

    public int subjectID;
    public string subID2;
    public int trialID;
    public int stepID;

    public String time_stamp;// = DateTime.Now.ToString("dd MM yyyy tt hh mm ss ffff"); //System.DateTime.Now.ToString(); 

    public Vector3 head_rot; // = GameObject.Find("CenterEyeAnchor").transform.rotation;
    public Vector3 head_pos;// = GameObject.Find("CenterEyeAnchor").transform.position;

    public Vector3 controller_1_rot; // = VRPN.vrpnTrackerQuat("PPT0@localhost", 0);
    public Vector3 controller_1_pos; // = VRPN.vrpnTrackerPos("PPT0@localhost", 0);

    public bool press_trigger;
    public bool press_ring;

    public string equation;

    public bool computer_correct;
    public int subject_correct;
    public int subject_wrong;

    public bool falling;
    public string collision_obj;

    public float display_time;
    public float performance;

    public string tile_exploding;

    public bool training;


    //public Quaternion controller_2_rot;// = VRPN.vrpnTrackerQuat("PPT0@localhost", 4);
    //public Vector3 controller_2_pos;// = VRPN.vrpnTrackerPos("PPT0@localhost", 4);

    //public Vector3 hand_1_pos; // = VRPN.vrpnTrackerPos("PPT0@localhost", 0);
    //public Quaternion hand_1_rot;// = VRPN.vrpnTrackerQuat("PPT0@localhost", 4);

    //public Vector3 hand_2_pos; // = VRPN.vrpnTrackerPos("PPT0@localhost", 0);
    //public Quaternion hand_2_rot;// = VRPN.vrpnTrackerQuat("PPT0@localhost", 4);


    //public Vector4 balanceBoard; //= Wii.GetBalanceBoard(0);

    //public Quaternion[] moven_quat = new Quaternion [57];
    //public Vector3[] moven_pos = new Vector3 [57];

}
