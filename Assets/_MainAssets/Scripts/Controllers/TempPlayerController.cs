using System.Collections;
using System.Collections.Generic;
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

        private int currentHp;
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

        [SerializeField] private Transform handsocket;
        //[SerializeField] private float minThrowForce = 1;

        private Vector2 directionInput;
        private PalloController heldPallo;
        private float throwChargeTime = 0;

        private bool IsHoldingBall => heldPallo;
        private float MinThrowForce => PalloController.TIER_2_SPEED;

        private void Awake()
        {
            currentHp = maxHealthPoints;
        }
        private void Update()
        {
            PlayerMovement();
            PlayerRotation();
            BallThrow();
        }

        private void FixedUpdate()
        {
            
        }
        private void PlayerMovement()
        {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.y = Input.GetAxis("Vertical");

            if(transform.position.y<2)
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

            Debug.Log("Player HP = " + CurrentHp);

            if (CurrentHp <= 0)
            {
                this.KillPlayer();
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