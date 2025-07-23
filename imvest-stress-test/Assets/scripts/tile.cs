using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile : MonoBehaviour
{

    public int step;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator action()
    {

        print("corroutine active: " + name);


        transform.GetComponent<AudioSource>().Play();

        //GetComponent<MeshRenderer>.material. .color = Color.red;
        for(int i = 0; i<2; i++)
        {

            transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red); 
            yield return new WaitForSeconds(0.25f);
            transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.clear);
            yield return new WaitForSeconds(0.25f);
        }

        transform.GetComponent<AudioSource>().Stop();

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;


        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        GameObject.Find("person_collider").GetComponent<personCollider>().tiles.Remove(this.gameObject);

        Destroy(this.gameObject);

    }
}
