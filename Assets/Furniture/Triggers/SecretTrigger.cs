using UnityEngine;

public class SecretTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddTorque(1, 0, 0, ForceMode.Impulse);
        }
    }
}
