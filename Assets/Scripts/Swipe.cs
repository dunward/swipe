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

        float targetX = calcX > 0 ? -100f : 100f;
        float angleZ = calcX > 0 ? 5f : -5f;

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
        GetComponent<RectTransform>().eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, angleZ), Mathf.Abs(calcX) * 20f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        switch (currentSwipeType)
        {
            case SwipeType.LEFT:
                Debug.LogError("LEFT SWIPE");
                StartCoroutine(SwipeMove(-100, 5));
                break;
            case SwipeType.RIGHT:
                Debug.LogError("RIGHT SWIPE");
                StartCoroutine(SwipeMove(100, -5));
                break;
            case SwipeType.UP:
                Debug.LogError("UP SWIPE");
                break;
            default:
                Debug.LogError("Nothing");
                GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                GetComponent<RectTransform>().eulerAngles = Vector3.zero;
                break;
        }

        currentSwipeType = 0;
    }

    private IEnumerator SwipeMove(float startPosition, float startAngle)
    {
        float timer = 0f;
        
        while (timer <= 1f)
        {
            timer += Time.deltaTime * 5;

            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(new Vector3(startPosition, 0, 0), new Vector3(startPosition * 20, 0, 0), timer);
            GetComponent<RectTransform>().eulerAngles = Vector3.Lerp(new Vector3(0, 0, startAngle), new Vector3(0, 0, startAngle * 5), timer);

            yield return null;
        }

        Destroy(gameObject);
    }
}
