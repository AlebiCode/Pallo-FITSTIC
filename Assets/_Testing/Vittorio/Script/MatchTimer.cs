using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace vittorio {
    public class MatchTimer : MonoBehaviour {
        public static MatchTimer Instance { get; private set; }

        public UnityEvent OnStandardTimeEnd = new();
        public UnityEvent OnOvertimeEnd = new();

        [SerializeField] float normalTimerSeconds;
        [SerializeField] float OvertimeTimerSeconds;
        [SerializeField] TMP_Text timeText;

        private float timeRemaining;

        private void Awake() {

            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }

        }
        void Start() {
            timeText.text = "";
            StartCoroutine(Countdown());
        }

        // Update is called once per frame
        void Update() {

        }

        private IEnumerator Countdown() {

            timeRemaining = normalTimerSeconds;

            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            while (timeRemaining > 0) {
                
                yield return new WaitForSeconds(1);
                timeRemaining -= 1;
                minutes = Mathf.FloorToInt(timeRemaining / 60);
                seconds = Mathf.FloorToInt(timeRemaining % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            }

            yield return new WaitForSeconds(1);

            OnStandardTimeEnd?.Invoke();

            timeRemaining = OvertimeTimerSeconds;

            while (timeRemaining > 0) {

                yield return new WaitForSeconds(1);
                timeRemaining -= 1;
                minutes = Mathf.FloorToInt(timeRemaining / 60);
                seconds = Mathf.FloorToInt(timeRemaining % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            }

            yield return new WaitForSeconds(1);

            OnOvertimeEnd?.Invoke();

        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }
}
