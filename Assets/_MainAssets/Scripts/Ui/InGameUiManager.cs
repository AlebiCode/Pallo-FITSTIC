using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager Instance;

    [System.Serializable]
    private struct PlayerSection
    {
        public GameObject deadIcon;
        public Image staminaBar;
    }

    [SerializeField] private TMP_Text timer;
    [SerializeField] private PlayerSection[] playerSections;

    private void Awake()
    {
        if(Instance)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void UpdateTimer(float secondsRemaining)
    {
        int minutes = (int)(secondsRemaining / 60);
        int seconds = (int)(secondsRemaining - (minutes * 60));
        timer.text = minutes.ToString("00") + '+' + seconds.ToString("00");
    }

    public void UpdateHpBar(int player, float hpPercent)
    {
        playerSections[player].staminaBar.transform.localScale = new Vector3(hpPercent, 1, 1);
    }

    public void EnableDeathIcon(int player, bool value = true)
    {
        playerSections[player].deadIcon.SetActive(value);
    }

}
