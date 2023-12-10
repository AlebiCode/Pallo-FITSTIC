using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{

    public class PalloController : MonoBehaviour
    {
        private enum BallStates { held, thrown }

        [SerializeField] private new SphereCollider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private LayerMask collisionLayermask;

        private BallStates ballState = BallStates.thrown;
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
        private bool IsHeld => ballState == BallStates.held;

        private Vector3 velocity;
        private RaycastHit spherecastInfo;

        private const float DECELERATION_TIME = 1.5f;
        private float decelerationTimer = 0;

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
        }
        private void OnWallCollision()
        {
            //eh... migliorabile
            velocity = Vector3.Reflect(velocity, spherecastInfo.normal);
            decelerationTimer = 0;
        }

        private void UpdateVelocity()
        {
            if (decelerationTimer >= DECELERATION_TIME)
            {
                velocity = Vector3.MoveTowards(velocity, Vector3.zero, 5 * Time.deltaTime);
            }
            else
            {
                decelerationTimer += Time.deltaTime;
            }
        }

        public void Hold(Transform socket)
        {
            ballState = BallStates.held;
            transform.SetParent(socket);
            transform.localPosition = Vector3.zero;

        }
        public void Throw(Vector3 velocity)
        {
            ballState = BallStates.thrown;
            transform.SetParent(null);
            decelerationTimer = 0;
            this.velocity = velocity;
        }

    }

}