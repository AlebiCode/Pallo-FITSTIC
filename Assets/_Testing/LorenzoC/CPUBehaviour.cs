using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LorenzoCastelli {

    public class CPUBehaviour : MonoBehaviour {

        public int attackRange;
        public PlayerData player;
        public CPUStateMachine stateMachine;
        [SerializeField] private NEWPLAYERSTATES state;
        private NavMeshAgent agent;
        /*[SerializeField] private EnemyStatistics stats;
        [SerializeField] private Animator animator;
        [SerializeField] private Attack attack;
        [SerializeField] public Weapon weapon;*/

        public Action<int> animationEvent;
        public Action OnAttackFinished;

        /*[SerializeField] List<GameObject> patrolNodes = new List<GameObject>();
        private List<Vector3> patrolList;*/

        #region Getter e Setter
        //public List<Vector3> PatrolNodes => patrolList; //fa schifo but bear with me

        //public Animator Animator => animator;

        public NEWPLAYERSTATES State { get => state; set => state = value; }
        public NavMeshAgent Agent { get => agent; set => agent = value; }

        #endregion



        private void Awake() {
            Agent = GetComponent<NavMeshAgent>();
            Dictionary<NEWPLAYERSTATES, CPUState> dict = new() {
                { NEWPLAYERSTATES.IDLE, new CPU_Idle(this) },
                { NEWPLAYERSTATES.GOINGFORBALL, new CPU_GoingForBall(this) },
                { NEWPLAYERSTATES.ATTACKING, new CPU_Attacking(this) },
                { NEWPLAYERSTATES.BACKINGOFF, new CPU_BackingOff(this) },
                { NEWPLAYERSTATES.BACKINGOFF, new CPU_Death(this) }
            };


            stateMachine = new CPUStateMachine(this, dict);
        }



        private void Start() {

            stateMachine.ChangeState(NEWPLAYERSTATES.IDLE);
        }

        private void Update() {
            stateMachine.UpdateState();

        }

        //for testing purposes
        public void Attack() {

            stateMachine.ChangeState(NEWPLAYERSTATES.ATTACKING);
        }

        private void BackOff() {
            stateMachine.ChangeState(NEWPLAYERSTATES.BACKINGOFF);
        }


        /*public void SendEvent(int eventNumber) {
            animationEvent?.Invoke(eventNumber);
        }

        public void FinishAttack() {
            OnAttackFinished?.Invoke();
        }*/


    }
}