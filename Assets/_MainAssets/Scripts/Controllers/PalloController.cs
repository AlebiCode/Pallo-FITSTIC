using System;
using System.Collections;
using System.Collections.Generic;
using LorenzoCastelli;
using UnityEngine;

namespace Controllers
{

    public class PalloController : MonoBehaviour
    {
        //TODO:
        //come funziona il rallentamento?
        //il check per essere afferrata da un giocatore non è nel player ma nella palla (così uso lo spherecast e evito i clip che possono accadere con ontriggerenter)
        //ragionamento fisica: P=m*v. Ptot=p1+p2. [before impact] Ptot=m1*v1+m2*v2, v2=0, Ptot=p1. [after impact] p1=p2, m1*v1=m2

        private const float GRAVITY = 9.81f;
        public static readonly float[] SPEED_TIERS = { 4.5f, 7.5f, 10f, 12f };

        private enum BallStates { held, thrown, bouncing }

        [Header("Components")]
        [SerializeField] private new SphereCollider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [Header("Settings")]
        [SerializeField] private LayerMask collisionLayermask;
        [Header("Ball Info")]
        [SerializeField] private BallStates ballState = BallStates.thrown;
        [SerializeField] private int speedTier;

        private Vector3 velocity;
        private RaycastHit spherecastInfo;
        private RaycastHit groundHitInfo;

        public bool IsHeld => ballState == BallStates.held;
        private float HorizontalVelocityMagnitude => new Vector2(velocity.x, velocity.y).magnitude;
        private BallStates BallState
        {
            get { return ballState; }
            set { 
                ballState = value;
                switch (ballState)
                {
                    case BallStates.thrown:
                    case BallStates.bouncing:
                        enabled = true;
                        break;
                    case BallStates.held:
                        enabled = false;
                        break;
                }
            }
        }


        private void Update()
        {
            Move();
            UpdateVelocity();
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
                if(Vector3.Angle(Vector3.up, spherecastInfo.normal) <= 45)
                    OnGroundCollision();
                else
                    OnWallCollision();
            }
        }
        private void OnWallCollision()
        {
            //eh... migliorabile
            //Debug.Log("Bounced against " + spherecastInfo.transform.name);
            velocity = Vector3.Reflect(velocity, new Vector3(spherecastInfo.normal.x, 0, spherecastInfo.normal.z)).normalized * velocity.magnitude;
        }
        private void OnGroundCollision()
        {
            BallState = BallStates.bouncing;
            velocity.y = 2.5f;

            if (groundHitInfo.rigidbody)
            {
                groundHitInfo.rigidbody.AddForceAtPosition(Vector3.down * velocity.y, groundHitInfo.point, ForceMode.Impulse);
            }
        }

        private void UpdateVelocity()
        {
            //decellera se sta rimbalzando a terra
            if (BallState == BallStates.bouncing)
            {
                float decelSpeed = 10f;
                Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
                horizontalVelocity = horizontalVelocity.normalized * (horizontalVelocity.magnitude - decelSpeed * Time.deltaTime);
                velocity.x = horizontalVelocity.x;
                velocity.z = horizontalVelocity.y;
            }

            //gravità
            velocity.y = velocity.y - (BallState == BallStates.bouncing ? GRAVITY : GRAVITY/2) * Time.deltaTime;
        }

        public void Hold(Transform socket)
        {
            BallState = BallStates.held;
            transform.SetParent(socket);
            //collider.enabled = false;
            transform.localPosition = Vector3.zero;
        }
        public void Throw(Vector3 velocity)
        {
            BallState = BallStates.thrown;
            transform.SetParent(null);
            collider.enabled = true;
            this.velocity = velocity;

            speedTier = 1;
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
        private int CalculateSpeedTier(float horizontalMagnitude)
        {
            for (int i = SPEED_TIERS.Length - 1; i >= 0; i--)
            {
                if (horizontalMagnitude >= SPEED_TIERS[i])
                {
                    return i;
                }
            }
            return 0;
        }

    }

}