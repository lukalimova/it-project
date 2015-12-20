using UnityEngine;
using System.Collections;

public class MiniGame : MonoBehaviour
{

    public GameObject Button;
    public GameObject[] Mechanisms;

    void OnTriggerEnter(Collider other)
    {
        bool flag = true;
        //Button.transform.localPosition += new Vector3(0, 0, 0.3f);
        Button.GetComponent<Renderer>().material.color = new Color(1, 0.098f, 0.098f);
        GetComponent<Collider>().enabled = false;

        Application.LoadLevel(4);
    }
}
