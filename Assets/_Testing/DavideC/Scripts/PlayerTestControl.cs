using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavideCTest
{
    public class PlayerTestControl : MonoBehaviour
    {
        [SerializeField] private Rigidbody myRb;
        [SerializeField] private float speed = 1;
        [SerializeField, Range(0f, 50f)] private float throwingForceModule = 20f;
        [SerializeField] private Vector3 throwingDirection;
        [SerializeField] private Transform BallSocket;

        private Vector3 finalThrowingForce;
        public Vector3 FinalThrowingForce => throwingDirection * throwingForceModule;

        //possesed ball
        [SerializeField] private BallClass activeBall;
        public BallClass ActiveBall { get; set; }

        [SerializeField] private bool isPlayerInBallPossession = false;
        public bool IsPlayerInBallPossession {get; set; } /*= false;*/

        private Vector2 directionInput;

        private void Awake()
        {
            throwingDirection = Vector3.forward;
        }

        private void ManageBallPossession()
        {

        }

        private void Update()
        {
            PlayerMovement();
            PlayerActions();
        }

        
        private void PlayerActions()
        { 
            if(Input.GetKeyDown(KeyCode.LeftShift) && IsPlayerInBallPossession) 
            {
                ActiveBall.GetComponent<Rigidbody>().AddForce(FinalThrowingForce, ForceMode.Impulse);

                ActiveBall.UnparentFromPlayer();

                ActiveBall.IsBallInPossession = false;
                ActiveBall.PlayerInPossession = null;

                IsPlayerInBallPossession = false;
                ActiveBall = null;

            }
        
        }

        private void PlayerMovement()
        {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.y = Input.GetAxis("Vertical");

        }
        private void FixedUpdate()
        {
            myRb.velocity += new Vector3(directionInput.x, 0, directionInput.y) * speed * Time.deltaTime;
        }
    }
}
