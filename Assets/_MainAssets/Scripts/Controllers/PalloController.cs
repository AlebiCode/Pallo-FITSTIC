using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using LorenzoCastelli;  
using StateMachine;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace Controllers
{
    public class PalloController : MonoBehaviour
    {
        //TODO:
        //come funziona il rallentamento?
        //il check per essere afferrata da un giocatore non è nel player ma nella palla (così uso lo spherecast e evito i clip che possono accadere con ontriggerenter)
        //ragionamento fisica: P=m*v. Ptot=p1+p2. [before impact] Ptot=m1*v1+m2*v2, v2=0, Ptot=p1. [after impact] p1=p2, m1*v1=m2

        private const int OVERLAP_SPHERE_BUFFER_SIZE = 3;
        private const float RETHROW_POWERUP_WINDOW = 1;

        private const float GRAVITY = 9.81f;
        public static readonly float[] SPEED_TIERS = { 4.5f, 7.5f, 10f, 12f };

        public enum BallStates { respawning, held, thrown, bouncing }

        PlayerData playerHolding = null;

        [Header("Components")]
        [SerializeField] private new SphereCollider collider;
        [SerializeField] private new Rigidbody rigidbody;
        [Header("Settings")]
        [SerializeField] private LayerMask collisionLayermask;
        [Header("Ball Info")]
        [SerializeField] private BallStates ballState = BallStates.thrown;
        [SerializeField] private int speedTier;

        private UnityEvent<BallStates> onBallStateChange = new UnityEvent<BallStates>();
        private UnityEvent<int> onSpeedTierChange = new UnityEvent<int>();

        private Vector3 velocity;
        private RaycastHit spherecastInfo;
        private Collider[] overlapSphereBuffer = new Collider[OVERLAP_SPHERE_BUFFER_SIZE];


        public UnityEvent<BallStates> OnStateChange => onBallStateChange;
        public UnityEvent<int> OnSpeedTierChange => onSpeedTierChange;

        public int SpeedTier => speedTier;
        public bool IsHeld => ballState == BallStates.held;
        public PlayerData PlayerHoldingIt { get { return playerHolding; } set { playerHolding = value; } }
        public bool CollisionsActive => enabled;
        public Vector3 Velocity => velocity;
        public BallStates BallState
        {
            get { return ballState; }
            set { 
                ballState = value;
                switch (ballState)
                {
                    case BallStates.respawning:
                    case BallStates.thrown:
                    case BallStates.bouncing:
                        enabled = true;
                        break;
                    case BallStates.held:
                        enabled = false;
                        break;
                }
                onBallStateChange.Invoke(value);
            }
        }


        private void Update()
        {
            Move();
            UpdateVelocity();
        }
        private void Move()
        {
            //if(IsHeld) return;    Non serve perchè se è held non viene chiamato update (component disabled)
            CollisionChecks();
            transform.position += velocity * Time.deltaTime;
        }
        private void CollisionChecks()
        {
            /*int colliderIndex = -1 + Physics.OverlapSphereNonAlloc(transform.position, collider.radius, overlapSphereBuffer, collisionLayermask, QueryTriggerInteraction.Collide);
            while (colliderIndex >= 0)
            {
                Debug.Log("Overlap " + overlapSphereBuffer[colliderIndex].name);
                PalloTriggerCheck(overlapSphereBuffer[colliderIndex]);
                colliderIndex--;
            }
            if (IsHeld)
                return;*/
            Physics.SphereCast(transform.position, collider.radius, velocity, out spherecastInfo, velocity.magnitude * Time.deltaTime, collisionLayermask, QueryTriggerInteraction.Collide);
            if (spherecastInfo.collider)
            {
                PalloTriggerCheck(spherecastInfo.collider.GetComponent<PalloTrigger>());
                if (!spherecastInfo.collider.isTrigger)
                {
                    if (Vector3.Angle(Vector3.up, spherecastInfo.normal) <= 45)
                        OnGroundCollision();
                    else
                        OnWallCollision();
                }
            }
        }
        private PalloTrigger lastPalloTrigger;
        private void PalloTriggerCheck(PalloTrigger newPalloTrigger)
        {
            if (newPalloTrigger != lastPalloTrigger)
            {
                lastPalloTrigger?.CallPalloTriggerExit(this);
                lastPalloTrigger = newPalloTrigger;
                newPalloTrigger?.CallPalloTriggerEnter(this);
            }

        }
        private void OnWallCollision()
        {
            AddInpulseToCollision(velocity);
            ReflectHorizontalVelocity();
        }
        private void ReflectHorizontalVelocity()
        {
            velocity = Vector3.Reflect(velocity, new Vector3(spherecastInfo.normal.x, 0, spherecastInfo.normal.z)).normalized * velocity.magnitude;
        }
        private void OnGroundCollision()
        {
            GroundBounce();
            AddInpulseToCollision(Vector3.down * velocity.y);
        }
        private void GroundBounce()
        {
            BallState = BallStates.bouncing;
            velocity.y = 2.5f;
        }
        private void AddInpulseToCollision(Vector3 force)
        {
            if (spherecastInfo.rigidbody)
            {
                spherecastInfo.rigidbody.AddForceAtPosition(force, spherecastInfo.point, ForceMode.Impulse);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collisionLayermask == (collisionLayermask | (1 << other.gameObject.layer)))
            {
                PalloTriggerCheck(other.GetComponent<PalloTrigger>());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (collisionLayermask == (collisionLayermask | (1 << other.gameObject.layer)))
            {
                PalloTriggerCheck(null);
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
        public void Throw(Vector3 speed, int speedTier = 0)
        {
            BallState = BallStates.thrown;
            transform.SetParent(null);
            collider.enabled = true;
            this.velocity = speed;// * SPEED_TIERS[speedTier];

            this.speedTier = speedTier;
            onSpeedTierChange.Invoke(speedTier);    //da fare per bene
            playerHolding = null;
            //UpdateSpeedTier();
        }
        public void Drop()
        {
            BallState = BallStates.bouncing;
            transform.SetParent(null);
            collider.enabled = true;
            playerHolding = null;
            //UpdateSpeedTier();
        }

        public void Respawn(Vector3 respawnForce)
        {
            velocity = respawnForce;
            BallState = BallStates.respawning;
        }

        /*
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
        */
    }

}