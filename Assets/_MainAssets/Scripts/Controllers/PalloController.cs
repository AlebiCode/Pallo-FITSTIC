using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{

    public class PalloController : MonoBehaviour
    {
        //TODO:
        //come funziona il rallentamento?
        //il check per essere afferrata da un giocatore non è nel player ma nella palla (così uso lo spherecast e evito i clip che possono accadere con ontriggerenter)
        //ragionamento fisica: P=m*v. Ptot=p1+p2. [before impact] Ptot=m1*v1+m2*v2, v2=0, Ptot=p1. [after impact] p1=p2, m1*v1=m2

        public const float TIER_1_SPEED = 4.5f;
        public const float TIER_2_SPEED = 7.5f;
        private const float GRAVITY = 9.81f;

        private enum SpeedTiers { tier0, tier1, tier2 }
        private enum BallStates { held, thrown }

        [Header("Components")]
        [SerializeField] private new SphereCollider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask collisionLayermask;

        [SerializeField] private BallStates ballState = BallStates.thrown;
        /*private BallStates BallState
        {
            get { return ballState; }
            set
            {
                if(ballState != value)
                    switch (value)
                    {   
                        case BallStates.held:
                            break;
                        case BallStates.thrown:
                            break;
                    }
                ballState = value;
            }
        }*/
        [SerializeField] private SpeedTiers speedTier = SpeedTiers.tier0;

        private Vector3 velocity;
        private RaycastHit spherecastInfo;

        private float decelerationTimer = 0;
        private bool IsHeld => ballState == BallStates.held;
        private float HorizontalVelocityMagnitude => new Vector2(velocity.x, velocity.y).magnitude;

        private void Update()
        {
            if (ballState == BallStates.thrown)
            {
                Move();
                UpdateVelocity();
            }
        }

        private void Move()
        {
            CollisionChecks();
            transform.position += velocity * Time.deltaTime;
        }
        private void CollisionChecks()
        {
            Physics.SphereCast(transform.position, collider.radius, velocity, out spherecastInfo, velocity.magnitude * Time.deltaTime, collisionLayermask);
            if (spherecastInfo.collider)
            {
                //da aggingere le varie casistiche
                OnWallCollision();
            }
            if (Physics.Raycast(transform.position, Vector3.down, collider.radius + velocity.y * Time.deltaTime, collisionLayermask))
            {
                OnGroundCollision();
            }
        }
        private void OnWallCollision()
        {
            //eh... migliorabile
            Debug.Log("Bounced against " + spherecastInfo.transform.name);
            velocity = Vector3.Reflect(velocity, new Vector3(spherecastInfo.normal.x, 0, spherecastInfo.normal.z)).normalized * velocity.magnitude;
            decelerationTimer = 0;
        }
        private void OnGroundCollision()
        {
            speedTier = SpeedTiers.tier0;
            velocity.y = 2.5f;
        }

        private void UpdateVelocity()
        {
            float decelSpeed = (int)speedTier > 0 ? 0f :10f;
            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
            float newMagnitude = horizontalVelocity.magnitude - decelSpeed * Time.deltaTime;

            velocity.y = velocity.y - ((int)speedTier > 0 ? GRAVITY/2 : GRAVITY) * Time.deltaTime;
            horizontalVelocity = horizontalVelocity.normalized * newMagnitude;
            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.y;
            //UpdateSpeedTier(newMagnitude);
        }

        public void Hold(Transform socket)
        {
            ballState = BallStates.held;
            transform.SetParent(socket);
            enabled = false;
            transform.localPosition = Vector3.zero;
        }
        public void Throw(Vector3 velocity)
        {
            ballState = BallStates.thrown;
            transform.SetParent(null);
            enabled = true;
            decelerationTimer = 0;
            this.velocity = velocity;

            speedTier = SpeedTiers.tier1;
            UpdateSpeedTier();
        }

        private void UpdateSpeedTier()
        {
            UpdateSpeedTier(HorizontalVelocityMagnitude);
        }
        private void UpdateSpeedTier(float horizontalMagnitude)
        {
            speedTier = CalculateSpeedTier(horizontalMagnitude);
        }
        private SpeedTiers CalculateSpeedTier(float horizontalMagnitude)
        {
            if (horizontalMagnitude >= TIER_2_SPEED)
                return SpeedTiers.tier2;
            else if (horizontalMagnitude >= TIER_1_SPEED)
                return SpeedTiers.tier1;
            else
                return SpeedTiers.tier0;
        }

    }

}