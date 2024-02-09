using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MainMenuUI/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite smallPreviewImage;
    public Sprite largePreviewImage;
}
