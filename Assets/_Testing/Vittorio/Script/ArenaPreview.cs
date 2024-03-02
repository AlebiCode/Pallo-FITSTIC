using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaPreview : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;


    public void SetPreview(ArenaSO arena) {
        this.image.sprite = arena.largePreviewImage;
        this.text.text = arena.arenaName;
    }
}
