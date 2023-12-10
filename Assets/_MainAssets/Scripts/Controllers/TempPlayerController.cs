using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class TempPlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 1;
        [SerializeField] private Transform handsocket;
        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float minThrowForce = 1;
        [SerializeField] private float maxThrowForce = 10;

        private Vector2 directionInput;
        private PalloController heldPallo;
        private float throwChargeTime = 0;

        private bool IsHoldingBall => heldPallo;

        private void Update()
        {
            PlayerMovement();
            PlayerRotation();
            BallThrow();
        }

        private void PlayerMovement()
        {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.y = Input.GetAxis("Vertical");

            transform.position += new Vector3(directionInput.x, 0, directionInput.y) * speed * Time.deltaTime;
        }
        private void PlayerRotation()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), Vector3.up);
            }
        }
        private void BallThrow()
        {
            if (!IsHoldingBall)
                return;

            if (throwChargeTime >= maxChargeTime || Input.GetKeyUp(KeyCode.Mouse0))
            {
                heldPallo.Throw(transform.forward * (minThrowForce + (Mathf.Min(throwChargeTime, maxChargeTime) * (maxThrowForce - minThrowForce) / maxChargeTime)));
                heldPallo = null;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                throwChargeTime += Time.deltaTime;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            heldPallo = other.GetComponent<PalloController>();
            if (IsHoldingBall)
            {
                throwChargeTime = 0;
                heldPallo.Hold(handsocket);
            }
        }

    }

}