using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace vittorio {
    public class GameplayManager : MonoBehaviour {
        public UnityEvent OnStartGame = new();
        public UnityEvent OnOvertimeStart = new();
        public UnityEvent OnEndGame = new();
        [SerializeField] Text timeText;



    }
}