using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortaitSmall : MonoBehaviour
{
    [SerializeField] public CharacterSO character;
    [SerializeField] public List<GameObject> playerIcons;
    [SerializeField] public Image previewImage;

    private void Awake() {
        previewImage.sprite = character.smallPreviewImage;
    }
}
