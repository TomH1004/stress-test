using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class recording : MonoBehaviour {


    String filePath;

    private rec_struct rec;
    public int active = 0;
    private bool is_recording = false;
    List<rec_struct> recordings = new List<rec_struct>();
    public bool record_mvn;

    // objects to record
    public Transform controller_1;
    //public Transform controller_2;
    public Transform hmd;
    public GameObject moven;
    //public Transform hand_1;
    //public Transform hand_2;

    public bool done_writing; 


    //// struct
  
    private personCollider col_script;
    private int deviceIndexController;


    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "ExperimentData/");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        col_script = this.gameObject.GetComponent<personCollider>();
        done_writing = true;



    }

    public void print_list(){print("print list.");}


    public void start_recording()
    {
        recordings.Clear();
        print("start recording.");
        is_recording = true;
    }

    public void stop_recording()
    {
        print("stop recording.");
        is_recording = false;
    }

	
	// Update is called once per frame
	void Update () {

        //if(Input.GetKeyUp(KeyCode.P)){ print_list(); }

        if (is_recording)
        {
            
  
            rec = new rec_struct();

            rec.subjectID = col_script.subject_id;

            rec.subID2 = col_script.subID2;

            rec.trialID = col_script.trialnr;
            rec.stepID = col_script.step;
            rec.time_stamp = DateTime.Now.ToString("dd MM yyyy tt hh mm ss ffff");

            rec.head_rot = hmd.eulerAngles;
            rec.head_pos = hmd.position;

            rec.controller_1_rot = controller_1.eulerAngles;
            rec.controller_1_pos = controller_1.position;

            // rec.press_trigger = SteamVR_Controller.Input(col_script.deviceIndexController).GetPress(SteamVR_Controller.ButtonMask.Trigger);
            
            rec.equation = col_script.equation;

            rec.computer_correct = col_script.computer_correct;
            rec.subject_correct = int.Parse(col_script.correct_answers.ToString());
            rec.subject_wrong = int.Parse(col_script.wrong_answers.ToString());

            rec.falling = col_script.fall_active;
            rec.collision_obj = col_script.collision_obj;
            rec.display_time = col_script.display_time;
            rec.performance = col_script.perf;

            rec.tile_exploding = col_script.tile_exploding;
            rec.training = col_script.training;
            //rec.controller_2_rot = controller_2.rotation;
            //rec.controller_2_pos = controller_2.position;

            //rec.hand_1_rot = hand_1.rotation;
            //rec.hand_1_pos = hand_1.position;
            // rec.hand_2_rot = hand_2.rotation;
            //rec.hand_2_pos = hand_2.position;

            //rec.balanceBoard = Wii.GetBalanceBoard(0);

            recordings.Add(rec);

            // moven suit

            //Transform[] joints = moven.GetComponentsInChildren<Transform>();

            //for (int joint_nr = 0; joint_nr < joints.GetLength(0); joint_nr++)
            //{
            //    rec.moven_pos[joint_nr] = joints[joint_nr].position;
            //    rec.moven_quat[joint_nr] = joints[joint_nr].rotation;

            //}

        }
    }

    public void write_file()
    {
        print("write file");
        done_writing = false;
        String fileName;
        if (recordings[0].training) fileName = filePath + recordings[0].subID2 + "_stress_training_" + recordings[0].subjectID.ToString() + "_" + recordings[0].trialID.ToString() + ".txt";
        else fileName = filePath + recordings[0].subID2 + "_stress_" + recordings[0].subjectID.ToString() + "_" + recordings[0].trialID.ToString() + ".txt";
        var sr = File.CreateText(fileName);

        //header
        sr.WriteLine("subjectID2, subjectID, trialID, stepID, training, time_stamp, head_pos, head_euler, contr_pos, contr_euler, press_trigger, equation, display_time, performance, computer_correct, subject_correct, subject_wrong, subject_falling, tile_exploding, collision_object");


        //body
        for (int i = 0; i < recordings.Count; i++)
        {
            sr.WriteLine(

                recordings[i].subID2 + " ; " +
                recordings[i].subjectID.ToString() + " ; " +
                recordings[i].trialID.ToString() + " ; " +
                recordings[i].stepID.ToString() + " , " +
                recordings[i].training.ToString() + " , " +
                recordings[i].time_stamp + " ; " +
                recordings[i].head_pos.ToString("n6") + " ; " +
                recordings[i].head_rot.ToString("n6") + " ; " +
                recordings[i].controller_1_pos.ToString("n6") + " ; " +
                recordings[i].controller_1_rot.ToString("n6") + " ; " +
                recordings[i].press_trigger.ToString() + " ; " +
                recordings[i].equation + " ; " +
                recordings[i].display_time.ToString("n6") + " ; " +
                recordings[i].performance.ToString("n6") + " ; " +
                recordings[i].computer_correct.ToString() + " ; " +
                recordings[i].subject_correct.ToString() + " ; " +
                recordings[i].subject_wrong.ToString() + " ; " +
                recordings[i].falling.ToString() + " ; " +
                recordings[i].tile_exploding + " ; " +
                recordings[i].collision_obj
            );
        }

        sr.Close();

        //print("write file moven");

        //fileName = filePath + recordings[0].subjectID.ToString() + "_" + recordings[0].trialID.ToString() + "_mvn.txt";

        //sr = File.CreateText(fileName);

        //Transform[] joints = moven.GetComponentsInChildren<Transform>();
        //String[] names = new String[joints.GetLength(0)];

        //String header = "";    

        ////header
        //for (int joint_nr = 0; joint_nr < joints.GetLength(0); joint_nr++)
        //{
        //    header = header + "POS_" + joints[joint_nr].ToString() + " ;  QUAT_" + joints[joint_nr].ToString() + " ; ";
        //}

        //sr.WriteLine(header);


        //for (int i = 0; i < recordings.Count; i++)
        //{
        //    String new_line = "";
        //    // data
        //    for (int joint_nr = 0; joint_nr < joints.GetLength(0); joint_nr++)
        //    {

        //        new_line = new_line + recordings[i].moven_pos[joint_nr].ToString("n5") + " ; " + recordings[i].moven_quat[joint_nr].ToString("n5") + " ; ";
        //    }

        //    sr.WriteLine(new_line);


        //}

        //sr.Close();

        print("done writing!");

        done_writing = true;

        //this.gameObject.GetComponent< >().step = 9;

    }
}
