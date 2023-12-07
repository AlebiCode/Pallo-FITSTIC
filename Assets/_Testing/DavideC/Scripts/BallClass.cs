using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DavideCTest
{
    
    public class BallClass : MonoBehaviour
    {

        [SerializeField] private float ballSpeed;

        [SerializeField] private PlayerTestControl playerInPossession = null;
        public PlayerTestControl PlayerInPossession { get; set; }

        [SerializeField] private bool isBallInPossession = false;
        public bool IsBallInPossession { get; set; }
        
        private void OnCollisionEnter(Collision other)
        {
            if (IsBallInPossession == false && other.gameObject.GetComponent<PlayerTestControl>() != null)
            {
                var playerGrabbing = other.gameObject.GetComponent<PlayerTestControl>();

                if (playerGrabbing.IsPlayerInBallPossession == false)
                {
                    IsBallInPossession = true;
                    PlayerInPossession = playerGrabbing;

                    playerGrabbing.IsPlayerInBallPossession = true;
                    playerGrabbing.ActiveBall = this;

                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    this.transform.parent = playerGrabbing.transform;

                }
            }
        }

        private void Update()
        {
            if (PlayerInPossession != null)
            {
                if (PlayerInPossession.IsPlayerInBallPossession)
                {
                    this.transform.position = new Vector3(PlayerInPossession.transform.position.x, PlayerInPossession.transform.position.y, PlayerInPossession.transform.position.z + 1);
                }
            }
        }

        public void UnparentFromPlayer()
        {
            this.transform.parent = transform.parent.transform.parent;
        }

        private void FixedUpdate()
        {
            
        }
    }
}
