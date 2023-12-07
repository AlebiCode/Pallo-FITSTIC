using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LorenzoCastelli {


public class PalloHandler : MonoBehaviour
{

    public float minSpeed;
    public float maxSpeed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Couldn't locate Rigidbody for " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (rb!= null) {
            if ((rb.velocity.x <= minSpeed) && (rb.velocity.z <= minSpeed)){
                rb.AddRelativeForce(new Vector3(Random.Range(0.1f, 4), 0, Random.Range(0.1f, 4)),ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
            rb.AddRelativeForce(new Vector3(-transform.position.x/10, 0, transform.position.z/10), ForceMode.Force);
        }
    }

    private void LateUpdate() {
        CheckForOutOfBounds();
    }

    private void CheckForOutOfBounds() {
        if (transform.position.y <= -1) {
            //METTERE RESPAWN DELLA PALLA
        }
    }
}

}
