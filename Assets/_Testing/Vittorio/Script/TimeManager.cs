using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace vittorio {

    public class Timer : MonoBehaviour {

        public static Timer Instance { get; private set; }

        public UnityEvent OnStart = new();
        public UnityEvent OnFinished = new();
        
        [SerializeField] Text timerText;

        private void Awake() {
            SetInstance();
        }

        IEnumerator StartCountdown() {
            OnStart?.Invoke();
            yield return new WaitForSeconds(1);
        }

        private void SetInstance() {
            if (Instance == null) {
                Instance = this;
                return;
            }
            Destroy(gameObject);
        }
    }
}
