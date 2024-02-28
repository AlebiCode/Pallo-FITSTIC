using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Handles the small selectable character portrait
/// </summary>
public class CharacterPortraitSmall : MonoBehaviour
{
    [SerializeField] public CharacterSO character;
    [SerializeField] public List<GameObject> playerIcons;
    [SerializeField] private Image previewImage;


    private void Awake() {
        previewImage.sprite = character.smallPreviewImage;
    }

    public void EnablePlayerIcon(UIPlayer player) {
        playerIcons[player.PlayerNum].SetActive(true);
    }

    public void DisablePlayerIcon(UIPlayer player) {
        playerIcons[player.PlayerNum].SetActive(false);
    }


}
