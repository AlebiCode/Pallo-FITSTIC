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

        private Vector2 directionInput;

        private void Update()
        {
            PlayerMovement();
        }

        private void PlayerMovement()
        {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.y = Input.GetAxis("Vertical");

            rb.velocity += new Vector3(directionInput.x, 0, directionInput.y) * speed * Time.deltaTime;
        }

    }

}