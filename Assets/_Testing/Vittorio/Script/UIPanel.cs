using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction { Left, Right, Up, Down, Null }
[RequireComponent(typeof(RectTransform))]
public class UIPanel : MonoBehaviour
{
    
    RectTransform rectTransform;
    int height;
    int width;


    [SerializeField] float moveTime;
    [SerializeField] Direction spawnFrom;

    private void OnEnable() {

        if(rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        switch(spawnFrom) {
        case Direction.Left:
            rectTransform.anchoredPosition = new Vector2(-width, 0);
            MoveRight(false);
            break;

        case Direction.Right:
            rectTransform.anchoredPosition = new Vector2(width, 0);
            MoveLeft(false); break;

        case Direction.Up:
            rectTransform.anchoredPosition = new Vector2(0, height);
            MoveDown(false); break;

        case Direction.Down:
            rectTransform.anchoredPosition = new Vector2(0,-height);
            MoveUp(false); break;
        
        }
    }

    public void MoveUp() {
        MoveUp(true);
    }
    public void MoveDown() {
        MoveDown(true);
    }
    public void MoveLeft() {
        MoveLeft(true);
    }
    public void MoveRight() {
        MoveRight(true);
    }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        width = Screen.currentResolution.width;
        height = Screen.currentResolution.height;
    }
 
    private void MoveUp(bool disableOnComplete) {

        if ( disableOnComplete) {
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + height, moveTime).OnComplete(delegate () { gameObject.SetActive(false); });
        } else {
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + height, moveTime);
        }
        
    }

    private void MoveDown(bool disableOnComplete) {
        if ( disableOnComplete) {
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - height, moveTime).OnComplete(delegate () { gameObject.SetActive(false); });
        } else {
            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - height, moveTime);
        }
        
    }

    private void MoveLeft(bool disableOnComplete) {
        if (disableOnComplete)
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - width, moveTime).OnComplete(delegate () { gameObject.SetActive(false); });
        else
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - width, moveTime);
    }

    private void MoveRight(bool disableOnComplete) {
        if (disableOnComplete)
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + width, moveTime).OnComplete(delegate () { gameObject.SetActive(false); });
        else
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + width, moveTime);
    }

    public void MoveToScreen() {
        rectTransform.DOAnchorPos(new Vector2(0,0), moveTime);
    }


}
