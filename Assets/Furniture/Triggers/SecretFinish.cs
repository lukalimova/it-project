using UnityEngine;

public class SecretFinish : MonoBehaviour
{
    public GameObject habr;
    public GameObject room;
    public GameObject cam;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Rigidbody>().useGravity = true;
            other.GetComponent<PlayerController>().enabled = false;
                
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY |
                                            RigidbodyConstraints.FreezeRotationZ;
            other.GetComponent<Rigidbody>().AddTorque(-1, 0, 0, ForceMode.Impulse);

            habr.transform.localScale = Vector3.one;
            room.SetActive(false);
            cam.GetComponent<Camera>().far = 1000;
        }
    }
}
