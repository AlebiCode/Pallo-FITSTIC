using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ParallaxComponent {
    public GameObject go;
    [Tooltip("Negative number is left, positive is right")]
    [Range(-500,500)]public float xMovement;

}
public class ParallaxManager : MonoBehaviour
{
    [SerializeField] float moveTime;
    [SerializeField] List<ParallaxComponent> list;


    public void Move(Direction dir) {

        if (dir == Direction.Left) {
            foreach (var parallax in list) {
                RectTransform rt = parallax.go.GetComponent<RectTransform>();
                rt.DOAnchorPosX(rt.anchoredPosition.x - parallax.xMovement, moveTime);
            }
        }else if (dir == Direction.Right) {
            foreach (var parallax in list) {
                RectTransform rt = parallax.go.GetComponent<RectTransform>();
                rt.DOAnchorPosX(rt.anchoredPosition.x + parallax.xMovement, moveTime);
            }
        }
    }

   


}
