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

        private float currentTimer;

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

            currentTimer = normalTimerSeconds;

            float minutes = Mathf.FloorToInt(currentTimer / 60);
            float seconds = Mathf.FloorToInt(currentTimer % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            while (currentTimer > 0) {
                print(currentTimer); 
                yield return new WaitForSeconds(1);
                currentTimer -= 1;
                minutes = Mathf.FloorToInt(currentTimer / 60);
                seconds = Mathf.FloorToInt(currentTimer % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            }

            yield return new WaitForSeconds(1);

            OnStandardTimeEnd?.Invoke();

            currentTimer = OvertimeTimerSeconds;

            while (currentTimer > 0) {

                yield return new WaitForSeconds(1);
                currentTimer -= 1;
                minutes = Mathf.FloorToInt(currentTimer / 60);
                seconds = Mathf.FloorToInt(currentTimer % 60);
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
