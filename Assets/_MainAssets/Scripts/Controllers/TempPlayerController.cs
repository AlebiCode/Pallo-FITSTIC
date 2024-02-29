using AleBorghesi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class TempPlayerController : MonoBehaviour
    {
        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float maxThrowForce = 10;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 1;
        [SerializeField] private int maxHealthPoints = 100;

        [SerializeField] private IKTesting ikControl;
        [SerializeField] private PalloTrigger parryHitbox;
        [SerializeField] private Transform handsocket;
        [SerializeField] private PlayerAnimation playerAnimation;

        public bool playerIsBeingRejected = false;

        public int currentHp;
        public int CurrentHp 
        {
            get { return currentHp; }
            
            set
            {
                if(value < 0)
                    currentHp = 0;
                else
                    currentHp = value;
            } 
        }

        private Vector3 directionInput;
        private Vector3 relativeDir;
        private PalloController heldPallo;
        private float throwChargeTime = 0;

        private bool IsHoldingBall => heldPallo;
        private float MinThrowForce => PalloController.SPEED_TIERS[0];

        private void Awake()
        {
            currentHp = maxHealthPoints;
        }
        private void Start()
        {
            GetComponent<PalloTrigger>().AddOnEnterListener(PalloContact);
            parryHitbox.AddOnEnterListener(GrabPallo);
        }
        private void Update()
        {
            PlayerMovement();
            PlayerRotation();
            float angle = Vector3.SignedAngle(transform.forward, relativeDir, Vector3.up);
            ikControl.SetLowerBodyRotation(angle);
            BallThrow();

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                playerAnimation.PlayAnimation(PlayerAnimation.takeIntro);
                parryHitbox.SetActivationStatus(true);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                playerAnimation.PlayAnimation(IsHoldingBall ? PlayerAnimation.takeOutroBall : PlayerAnimation.takeOutroNoBall);
                parryHitbox.SetActivationStatus(false);
            }
        }

       
        private void PlayerMovement()
        {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.z = Input.GetAxis("Vertical");

            transform.position += new Vector3(directionInput.x, 0, directionInput.z) * speed * Time.deltaTime;

            //playerAnimation.SetLegLayerWeight(directionInput.magnitude);
            relativeDir = transform.InverseTransformDirection(directionInput);
            playerAnimation.LegMovementParameters(relativeDir.x, relativeDir.z);
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
                heldPallo.Throw(transform.forward * (MinThrowForce + (Mathf.Min(throwChargeTime, maxChargeTime) * (maxThrowForce - MinThrowForce) / maxChargeTime)) + Vector3.up * 1.2f);
                heldPallo = null;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                throwChargeTime += Time.deltaTime;
            }
        }

        public void KillPlayer()
        {
            Debug.Log("player is killed!");
            //rimuovi destroy e togli una vita
            Destroy(this?.gameObject);
        }

        public void TakeDamage(int amount)
        {
            CurrentHp -= amount;

            Debug.Log("Took damage. Player HP = " + CurrentHp);

            if (CurrentHp <= 0)
            {
                this.KillPlayer();
            }
        }

        public void PalloContact(PalloController palloController)
        {
            if(palloController.BallState == PalloController.BallStates.thrown)
                TakeDamage(10);
            else
                GrabPallo(palloController);
        }
        public void GrabPallo(PalloController palloController)
        {
            heldPallo = palloController;
            throwChargeTime = 0;
            heldPallo.Hold(handsocket);
        }

        private float velocityChange = 0;
        RaycastHit castInfo;
        private void PlayerAirborneControl()
        {        
            Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x, Vector3.down, out castInfo, transform.position.y - ( GetComponent<Collider>().bounds.extents.y + 0.1f ), 1 << 0);

            if (playerIsBeingRejected)
            {
                if (castInfo.collider)
                {
                    StopAirbornePlayer();

                    Debug.Log("player stopped");
                }
                else
                {
                    SlowPlayerVerticalSpeed();

                    Debug.Log("player being slowed");
                }              
            }
        }
        public void SlowPlayerVerticalSpeed()
        {
            velocityChange += 0.5f * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x, -velocityChange, rb.velocity.z);
        }
        public void StopAirbornePlayer()
        {
            playerIsBeingRejected = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        public void SetPlayerToRejectState()
        {
            playerIsBeingRejected = true;
            velocityChange = 0f;
        }

        private void FixedUpdate()
        {
            PlayerAirborneControl();
        }
    }
}