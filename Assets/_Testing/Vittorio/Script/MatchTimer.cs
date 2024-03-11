using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace vittorio {

    [System.Serializable]
    public class MatchTimer : MonoBehaviour {
        public static MatchTimer Instance { get; private set; }

        public UnityEvent OnStandardTimeEnd = new();
        public UnityEvent OnOvertimeEnd = new();

        [SerializeField] float normalTimerSeconds;
        [SerializeField] float OvertimeTimerSeconds;

        private float timeRemaining;

        private void Start()
        {
            StartTimer();
        }

        public void StartTimer()
        {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown() {

            timeRemaining = normalTimerSeconds;

            InGameUiManager.Instance.UpdateTimer(timeRemaining);

            //float minutes = Mathf.FloorToInt(timeRemaining / 60);
            //float seconds = Mathf.FloorToInt(timeRemaining % 60);
            //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            while (timeRemaining > 0) {
                
                yield return new WaitForSeconds(1);
                timeRemaining -= 1;
                InGameUiManager.Instance.UpdateTimer(timeRemaining);
                //minutes = Mathf.FloorToInt(timeRemaining / 60);
                //seconds = Mathf.FloorToInt(timeRemaining % 60);
                //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            }

            yield return new WaitForSeconds(1);

            OnStandardTimeEnd?.Invoke();

            timeRemaining = OvertimeTimerSeconds;

            while (timeRemaining > 0) {

                yield return new WaitForSeconds(1);
                timeRemaining -= 1;
                //minutes = Mathf.FloorToInt(timeRemaining / 60);
                //seconds = Mathf.FloorToInt(timeRemaining % 60);
                //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                InGameUiManager.Instance.UpdateTimer(timeRemaining);
            }

            yield return new WaitForSeconds(1);

            OnOvertimeEnd?.Invoke();

        }

        private void OnDestroy() {
            StopAllCoroutines();
        }
    }
}
