using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Swipe : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private SwipeType swipeType;
    [SerializeField]
    private float swipeThreshold = 0.05f;

    private SwipeType currentSwipeType = 0;

    public void OnDrag(PointerEventData eventData)
    {
        var calcPosition = eventData.pressPosition - eventData.position;

        var calcX = calcPosition.x / Screen.width;
        var calcY = calcPosition.y / Screen.height;

        float targetX = calcX > 0 ? -50f : 50f;

        if (Mathf.Abs(calcX) > swipeThreshold)
        {
            if (calcX > 0) // RIGHT
            {
                currentSwipeType = SwipeType.LEFT;
            }
            else // LEFT
            {
                currentSwipeType = SwipeType.RIGHT;
            }
        }
        else
        {
            currentSwipeType = 0;
        }

        GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(Vector3.zero, new Vector3(targetX, 0, 0), Mathf.Abs(calcX) * 20f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        switch (currentSwipeType)
        {
            case SwipeType.LEFT:
                Debug.LogError("LEFT SWIPE");
                break;
            case SwipeType.RIGHT:
                Debug.LogError("RIGHT SWIPE");
                break;
            case SwipeType.UP:
                Debug.LogError("UP SWIPE");
                break;
            default:
                Debug.LogError("Nothing");
                GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                break;
        }

        currentSwipeType = 0;
    }
}
