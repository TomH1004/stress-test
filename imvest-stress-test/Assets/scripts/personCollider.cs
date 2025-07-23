using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;


public class personCollider : MonoBehaviour {


    private string subID2_file = "C:/AI4Coping/ForestBathing/IMVEST-Stress-Test/ExperimentData/PersonColliderData.txt";
    public int participant_id;
    public string subID2;
    public int trialnr;
    public string equation;
    public bool computer_correct;

    private recording rec_script;

    private float length_experiment = 240f;
    private bool english = false;
    private bool deutsch = true;

    public GameObject tile;

    #region Dimensions

    public float room_dim_z = 3f;
    public float room_dim_x = 5f;

    public int tiles_x_nr = 5;
    public int tiles_z_nr = 5;

    private float tile_x_scale;
    private float tile_z_scale;

    #endregion

    public int deviceIndexController = 1;

    public List<GameObject> tiles = new List<GameObject>();

    public ArrayList visible = new ArrayList();


    public Transform cam;

    float triggerTimeout = 0f;
    bool Touching = false;


    public GameObject text_touching;

    public GameObject text_task;

    public GameObject text_performance;
    public GameObject text_displaytime;

    //public Transform steam_ref;
    //public Camera steam_ref;
    public OVRCameraRig overCameraRig;

    private bool active = false;

    private bool button_press_X;
    private bool button_press_T;

    public AudioSource scream;
    //AudioNew
    public AudioSource Bartok;

    public GameObject Controller_right;

    public int step = 0;
    float Timestamp;
    public GameObject text_time;

    public GameObject text_perf_other;
    private StreamReader theReader;

    #region GameVars

    private int level = 0; //0: before training //1: training //2: experiment //3:after experiement  SHOULDNT THIS BE THE STEP?? THIS ONE ISNT USED

    public int subject_id;
    public float display_time = 5f; //was 1.5f
    public float wrong_answers = 0;
    public float correct_answers = 0;
    public float perf = 100;
    public List<float> runingPerf = new List<float>();
    public float runingP;

    public bool falling;

    public float ITI = 1f; // inter trial interval
    public float ITIpvar = 0.5f; //percentage of the ITI for variance

    public int right2wrongP = 5; //percentage of right answers that will be considered as wrong (from 0 to 100).

    public int number_falls = 0; //Number of falls


    #endregion

    // Use this for initialization
    void Start()
    {
        subject_id = UnityEngine.Random.Range(10000, 99999);

        
        trialnr = 0;
        tile_exploding = "null";
        //read sub ID 2
        // theReader = new StreamReader(subID2_file, Encoding.Default);
        // subID2 = theReader.ReadLine();
        subID2 = "test";

        rec_script = this.gameObject.GetComponent<recording>();

        makeTiles();
        // deviceIndexController = (int)Controller_right.GetComponent<SteamVR_TrackedObject>().index;
        //deloted print("DEVICEID: " + deviceIndexController);
        step = 0;

        if (english)
            text_performance.GetComponent<TextMesh>().text = "Your performance:  %";
        else if (deutsch)
            text_performance.GetComponent<TextMesh>().text = "Ihre Leistung:  %";
        else 
            text_performance.GetComponent<TextMesh>().text = "Votre performance:  %";

        if (english)
            text_perf_other.GetComponent<TextMesh>().text = "Other players average: 73%";
        else if (deutsch)
            text_perf_other.GetComponent<TextMesh>().text = "Andere Spieler im Durchschnitt: 73%";
        else
            text_perf_other.GetComponent<TextMesh>().text = "Moyennes des autres joueurs: 73%";

    }

    void makeTiles()
    {
        tile_x_scale = (room_dim_x / tiles_x_nr) / 10f;
        tile_z_scale = (room_dim_z / tiles_z_nr) / 10f;

        int count = 0;

        //3.50m x 6.00 meter = 7 * 12 tiles = 84 TILES
        for (int i = 0; i < tiles_x_nr; i++)
        {
            for (int j = 0; j < tiles_z_nr; j++)
            {

                count++;
                

                Vector3 scale = new Vector3(tile_x_scale * 0.99f, 1f, tile_z_scale * 0.99f);

                Vector3 pos = new Vector3((i) - room_dim_x / 2 + 0.5f, 0f, (j) - room_dim_z / 2 + 0.5f);



                GameObject new_tile = Instantiate(tile, pos, Quaternion.identity);
                new_tile.name = "tile_" + count.ToString();
             
                new_tile.transform.localScale = scale;
                tiles.Add(new_tile);
                visible.Add(true);
            }
        }
    }

    void Destroy_all_tiles_and_make_new() {

        while (tiles.Count > 0)
        {
            GameObject tile2destroy = tiles[0];
            tiles.Remove(tile2destroy);
            tile2destroy.SetActive(false);
            Destroy(tile2destroy);
        }

        makeTiles();
    }

    public string tile_exploding;

    public IEnumerator new_task()
    {
        Debug.Log(display_time);

        rec_script.start_recording();

        trialnr++;

        //difficult level 1

        int no1 = UnityEngine.Random.Range(0, 100);
        int no2 = UnityEngine.Random.Range(0, 100);
        //int no2 = no1;

        button_press_X = false;
        button_press_T = false;

        int cor_res = no1 - no2;

        int rand = UnityEngine.Random.Range(-10, 10);
        //if (rand == 0) rand = 7;
        int fake_res = no1 - no2 + rand; 
        

        int play_cor = UnityEngine.Random.Range(0, 2);

        //deloted print("corr: " + play_cor.ToString());

        tile_exploding = "null";

        //computer correct
        if (play_cor == 1) {
            computer_correct = true;

            equation = no1.ToString() + " - " + no2.ToString() + " = " + cor_res.ToString();

            text_task.GetComponent<TextMesh>().text = equation;
            text_task.SetActive(true);

            yield return new WaitForSeconds(display_time);

            if (button_press_T)
            {
                if (english)
                    text_task.GetComponent<TextMesh>().text = "Correct answer!";               
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Richtige Antwort!";               
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse correcte!";

                correct_answers++;
                runingPerf.Add(1);
            }

            else if (button_press_X)
            {
                if (english)
                    text_task.GetComponent<TextMesh>().text = "Wrong answer!";
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Falsche Antwort!";
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse incorrecte!";

                wrong_answers++;
                runingPerf.Add(0);
                if (step == 2)
                {
                    int rando = UnityEngine.Random.Range(0, tiles.Count);
                    tile_exploding = tiles[rando].name;
                    StartCoroutine(tiles[rando].GetComponent<tile>().action());
                }
            }

            else
            {
                if (english)
                    text_task.GetComponent<TextMesh>().text = "Too late!";
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Zu spät!";
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse incorrecte!";

                wrong_answers++;
                runingPerf.Add(0);
                if (step == 2)
                {
                    int rando = UnityEngine.Random.Range(0, tiles.Count);
                    tile_exploding = tiles[rando].name;
                    StartCoroutine(tiles[rando].GetComponent<tile>().action());
                }
            }

        }

        //computer false
        if (play_cor == 0)
        {
            computer_correct = false;

            equation = no1.ToString() + " - " + no2.ToString() + " = " + fake_res.ToString();

            text_task.GetComponent<TextMesh>().text = equation;
                      

            text_task.SetActive(true);

            yield return new WaitForSeconds(display_time);

            if (button_press_T)
            {
                if (english)
                    text_task.GetComponent<TextMesh>().text = "Wrong answer!";
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Falsche Antwort!";
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse incorrecte!";

                wrong_answers++;
                runingPerf.Add(0);
                if (step == 2) {
                    int rando = UnityEngine.Random.Range(0, tiles.Count);
                    tile_exploding = tiles[rando].name;
                    StartCoroutine(tiles[rando].GetComponent<tile>().action());
                }

            }
            else if(button_press_X)
            {

                if (english)
                    text_task.GetComponent<TextMesh>().text = "Correct answer!";
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Richtige Antwort!";
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse correcte!";

                correct_answers++;
                runingPerf.Add(1);
            }

            else
            {
                if (english)
                    text_task.GetComponent<TextMesh>().text = "Too late!";
                else if (deutsch)
                    text_task.GetComponent<TextMesh>().text = "Zu spät!";
                else
                    text_task.GetComponent<TextMesh>().text = "Réponse incorrecte!";

                wrong_answers++;
                runingPerf.Add(0);
                if (step == 2)
                {
                    int rando = UnityEngine.Random.Range(0, tiles.Count);
                    tile_exploding = tiles[rando].name;
                    StartCoroutine(tiles[rando].GetComponent<tile>().action());
                }

            }

        }

        //yield return new WaitForSeconds(display_time);

        yield return new WaitForSeconds(0.8f);

        //perf = (((float)correct_answers / ((float)wrong_answers + (float)correct_answers))*100f)*2-100f;
        perf = (((float)correct_answers / ((float)wrong_answers + (float)correct_answers)) * 100f);

        // Calculates running performance based on the last 10 items.
        int runingPerfNumber = 5;
        if(runingPerf.Count < runingPerfNumber)
        {
            runingP = runingPerf.Average();
                    }
        else
        {
            runingP = runingPerf.Skip(runingPerf.Count - runingPerfNumber).ToList().Average();
                    }

        //if (perf < 0) {
        //    if(english)text_performance.GetComponent<TextMesh>().text = "your performance: 0 %";
        //    else text_performance.GetComponent<TextMesh>().text = "votre performance: 0 %";
        //}

        if (1 == 2) { }

        else
        {
            //perf = runingP*100; //Activate for switch for displaing running average instead of average
            if (english)
                text_performance.GetComponent<TextMesh>().text = "Your performance: " + perf.ToString("n0") + "%";
            else if (deutsch)
                text_performance.GetComponent<TextMesh>().text = "Ihre Leistung: " + perf.ToString("n0") + "%";
            else
                text_performance.GetComponent<TextMesh>().text = "Votre performance: " + perf.ToString("n0") + "%";
        }



        //Can be used to visualize current display time, for testing. NB! Must activate Mesh Render for object -> Camera (head) "displaytime"
        //text_displaytime.GetComponent<TextMesh>().text = "Your time: " + display_time.ToString("n"); 

        //if (perf > 20f && display_time>0.8f) { display_time = display_time - 0.07f;}
        //if (perf < 0f) { display_time = display_time + 0.07f; }

        if (runingP*100 > 60f && display_time > 0.8f) {display_time = display_time - 0.1f;} // runingP instead of perf. was - 0.07f
        if (runingP*100 < 55f || display_time > 3f) { display_time = display_time + 0.1f;}

        //Changes display color for Performance, Red if below 73%, green if above
        if(perf > 73f)
            text_performance.GetComponent<TextMesh>().color = Color.green;
        else
            text_performance.GetComponent<TextMesh>().color = Color.red;
        //if ((length_experiment - (Time.time - Timestamp)) < 60f) display_time = 0.8f;

        print("DISPLAY: " + display_time);

        text_task.SetActive(false);

        //yield return new WaitForSeconds(display_time);
        //Inter Trial Interval

        float ITIvar = UnityEngine.Random.Range(-ITI * ITIpvar, ITI * ITIpvar);
        if (step != 2)
        {
            ITIvar = 0;
        }
        yield return new WaitForSeconds(ITI + ITIvar);


        //yield return null;
        if ((length_experiment - (Time.time - Timestamp)) > 0f) {

            rec_script.stop_recording();
            rec_script.write_file();
            while (!rec_script.done_writing) { yield return null; }

            StartCoroutine(new_task());
        }
        else
        {
            if (english)
                text_task.GetComponent<TextMesh>().text = "End";
            else if (deutsch)
                text_task.GetComponent<TextMesh>().text = "Ende";
            else
                text_task.GetComponent<TextMesh>().text = "Fin";
            text_task.SetActive(true);
        }

      

    }


    public bool training = false;

    void Update()
    {

        //deloted print("Step: " + step);
        //deloted print("DEVICE: " + deviceIndexController);

        //deviceIndexController = (int)Controller_right.GetComponent<SteamVR_TrackedObject>().index;

        text_time.GetComponent<TextMesh>().text = (length_experiment - (Time.time-Timestamp)).ToString("n0");

        transform.position = new Vector3(cam.position.x, transform.position.y, cam.position.z);

        // if (SteamVR_Controller.Input(deviceIndexController).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            //deloted print("Trigger pressed.");
            button_press_X = true;
            //StartCoroutine(tiles[Random.Range(0, tiles.Count)].GetComponent<tile>().action());
            Debug.Log("X pressed");
        }

        // if (SteamVR_Controller.Input(deviceIndexController).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger))

        {
            //deloted print("Trigger pressed.");
            button_press_T = true;
            Debug.Log("Trigger pressed");
            //StartCoroutine(tiles[Random.Range(0, tiles.Count)].GetComponent<tile>().action());
        }

        // if (step==0 && (Input.GetKeyUp("space")|| SteamVR_Controller.Input(deviceIndexController).GetPressUp(SteamVR_Controller.ButtonMask.Grip)))
        if (step==0 && (Input.GetKeyUp("space")))  //|| OVRInput.GetDown(OVRInput.RawButton.Y)))
        {
            step = -1;
            StopAllCoroutines();
            StartCoroutine("start_Training");
        }

        // if (step == 1 && (Input.GetKeyUp("space") || SteamVR_Controller.Input(deviceIndexController).GetPressUp(SteamVR_Controller.ButtonMask.Grip)))
        if (step==1 && (Input.GetKeyUp("space")))  //|| OVRInput.GetDown(OVRInput.RawButton.Y)))
        {
            step = -1;
            StopAllCoroutines();
            StartCoroutine("start_Experiment");

        }



        if (triggerTimeout > 0)
        {
            triggerTimeout -= Time.deltaTime;

            if (triggerTimeout <= 0)
            {
                triggerTimeout = 0f;
                Touching = false;
                collision_obj = "null";
            }

        }

        if (Touching) { text_touching.SetActive(true);  }
 
        else
        {
            text_touching.SetActive(false);

            //print("not touching");

            //if (!fall_active && step==2)
            if (!fall_active && step==2 && !Touching)
            {
                StartCoroutine(fall());
            }

        }
    }

    public bool fall_active = false;

    public IEnumerator start_Training()
    {
        if(english)
            text_task.GetComponent<TextMesh>().text = "Training will start!";
        else if (deutsch)
            text_task.GetComponent<TextMesh>().text = "Das Training beginnt!";
        else
            text_task.GetComponent<TextMesh>().text = "Début de l'entraînement!";

        text_task.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        text_task.SetActive(false);
        step = 1;
        training = true;
        Timestamp = Time.time;
        StartCoroutine("new_task"); 
    }

    public IEnumerator start_Experiment()
    {
        step = 2;
        display_time = 2; //1.5f
        perf = 100; //restart performance when experiment starts
        training = false;

        //AudioNew
        if (!Bartok.isPlaying) Bartok.Play();
        
        if (english)
            text_task.GetComponent<TextMesh>().text = "Starting Experiment.";
        else if (deutsch)
            text_task.GetComponent<TextMesh>().text = "Experiment starten.";
        else
            text_task.GetComponent<TextMesh>().text = "Début de l'expérience";

        text_task.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        text_task.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Timestamp = Time.time;
        StartCoroutine("new_task");
    }


    public IEnumerator fall()
    {
        Debug.Log("START Couroutine Fall");
        fall_active = true;
        number_falls++; //add one to falls

        if (!scream.isPlaying) scream.Play();

        float vel = 0;

        //while (steam_ref.main.transform.position.y > -10f)
        while (overCameraRig.transform.position.y > -10f)
        {
            vel = 0.001f + vel * 1.2f;
            //steam_ref.main.transform.position = new Vector3(0f, -vel, 0f);
            overCameraRig.transform.position = new Vector3(0f, -vel, 0f);
            yield return new WaitForSeconds(0.016f);
        }

        

        scream.Stop();
        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();


        yield return new WaitForSeconds(0.01f);
        Destroy_all_tiles_and_make_new();
        yield return new WaitForSeconds(0.01f);

        //steam_ref.main.transform.position = new Vector3(0f,0f,0f);
        overCameraRig.transform.position = new Vector3(0f, 0f, 0f);
        //yield return new WaitForSeconds(10f);

        fall_active = false;
        Debug.Log("STOP Couroutine Fall");
  
    }


    void OnTriggerStay(Collider col)
    {
        //print("trigger");
        collision_obj = col.name;
        Touching = true;
        triggerTimeout = 0.1f; //(or whatever is appropriate for a timeout value for your project)
    }

    public String collision_obj;

    void OnCollisionStay() {
        //print("Touching");
        //Touching = true;
        //triggerTimeout = 0.1f; //(or whatever is appropriate for a timeout value for your project)
    }



}
