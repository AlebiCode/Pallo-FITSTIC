using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class Player : MonoBehaviour
    {

        public StateMachine myStateMachine;
        // Start is called before the first frame update
        void Start()
        {
            myStateMachine = new StateMachine();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

