using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LorenzoCastelli { 

public class GameLogic : MonoBehaviour
{

    private static GameLogic instance;
    public enum GAMESTATES {
        START = 0,
        PLAYING = 1,
        TIMEUP = 2,
        ENDGAME = 3
    }

        public TextMeshProUGUI TM;
    public float playTime;
    public GAMESTATES gameState = GAMESTATES.START;

    private void Awake() {
        if (instance) {
            Destroy(this);
        } else {
            instance = this;
        }
        
    }

    private void Start() {
        EnterStart();
    }

private void EnterStart() {
            TM.text = "00";
            Debug.Log("Changing to " +  GAMESTATES.PLAYING + " from " + gameState);
            EnterPlaying();
    }

    private void EnterPlaying() {
            //EFFETTO ENTRATA INIZIO PARTITA
            //InitPlayers()
            TM.text = playTime.ToString();
        gameState = GAMESTATES.PLAYING;
    }

    private void EnterTimeUp() {
        gameState = GAMESTATES.TIMEUP;
        //EFFETTO ENTRATA OVERTIME
    }

    private void EnterEndGame() {
        //EFFETTO ENTRATA FINE PARTITA
    }
    private void UpdateOnGameLogic() {
        
    }

    private void UpdateStart() {
        //ChangeGameLogic(GAMESTATES.PLAYING);
    }

    private void UpdatePlaying() {
        playTime -= Time.deltaTime;
            TM.text = ((int)playTime).ToString();
            if (playTime <= 0) {
            EnterTimeUp();
        }
        //EFFETTI DA MANTENERE DURANTE PARTITA
    }

    private void UpdateTimeUp() {
        //ROBE DA FAR FARE QUANDO IN OVERTIME
    }

    private void UpdateEndGame() {
        //ROBE DA FAR FARE QUANDO IN FINE PARTITA
    }



    // Update is called once per frame
    void Update()
    {
            switch (gameState) {
                case GAMESTATES.START:
                    UpdateStart();
                    break;
                case GAMESTATES.PLAYING:
                    UpdatePlaying();
                    break;
                case GAMESTATES.TIMEUP:
                    UpdateTimeUp();
                    break;
                case GAMESTATES.ENDGAME:
                    UpdateEndGame();
                    break;
            }
        }
    }





}