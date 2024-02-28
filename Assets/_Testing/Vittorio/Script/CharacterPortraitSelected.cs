using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortraitSelected : MonoBehaviour
{
    private CharacterSO character;
    [SerializeField] Image image;

    private void Awake() {
        image.sprite = null;
    }

    private void OnDisable() {
        character = null;
    }

    public void SetSprite(Sprite sprite) {
        image.sprite=sprite;
    }

}
