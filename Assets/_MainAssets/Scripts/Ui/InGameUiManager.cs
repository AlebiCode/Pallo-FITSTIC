using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager Instance;

    [System.Serializable]
    private struct PlayerSection
    {
        public GameObject deadIcon;
        public Image staminaBar;
        public Transform lifesParent;
    }

    [SerializeField] private GameObject lifeIconPrefab;
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
        playerSections[player].deadIcon.SetActive(hpPercent > 0);
    }

    public void SetPlayerLifes(int player, int value)
    {
        for (int i = value; i < playerSections[player].lifesParent.childCount; i++)
        {
            Destroy(playerSections[player].lifesParent.GetChild(0).gameObject);
        }
        for (int i = playerSections[player].lifesParent.childCount; i < value; i++)
        {
            Instantiate(lifeIconPrefab, playerSections[player].lifesParent);
        }
    }

}
