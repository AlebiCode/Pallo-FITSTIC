using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "MainMenuUI/Arena")]
public class ArenaSO : ScriptableObject
{
    public int sceneNumber;
    public string arenaName;
    public string arenaDescription;
    public Sprite smallPreviewImage;
    public Sprite largePreviewImage;
}
