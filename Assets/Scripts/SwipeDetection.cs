using System;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class SwipeDetection : MonoBehaviour
{
    private Dictionary<int, SwipeData> fingersInfo = new Dictionary<int, SwipeData>();
    private int fingerId;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private bool reversed = false;

    [SerializeField]
    private float minDistanceForSwipe = 40f;

    public struct SwipeData
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public SwipeDirection Direction;
    }

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        //Debug.Log(Input.touchCount);
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingersInfo.Add(touch.fingerId, new SwipeData()
                {
                    StartPosition = touch.position
                });
            }

            if (touch.phase == TouchPhase.Canceled)
            {
                fingersInfo.Remove(fingerId);
            }

            if (detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Ended)
            {
                fingerId = touch.fingerId;
                SwipeData temp = new SwipeData()
                {
                    StartPosition = fingersInfo[fingerId].StartPosition,
                    EndPosition = touch.position
                };
                fingersInfo[fingerId] = temp;
                DetectSwipe();
                fingersInfo.Remove(fingerId);
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = (reversed == false ? 1 : -1) * (fingersInfo[fingerId].EndPosition.y - fingersInfo[fingerId].StartPosition.y) > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = (reversed == false ? 1 : -1) * (fingersInfo[fingerId].EndPosition.x - fingersInfo[fingerId].StartPosition.x) > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
        }
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingersInfo[fingerId].StartPosition,
            EndPosition = fingersInfo[fingerId].EndPosition
        };
        OnSwipe(swipeData);
    }

    private bool SwipeDistanceCheckMet() => VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;

    private bool IsVerticalSwipe() => VerticalMovementDistance() > HorizontalMovementDistance();

    private float VerticalMovementDistance() => Mathf.Abs(fingersInfo[fingerId].EndPosition.y - fingersInfo[fingerId].StartPosition.y);

    private float HorizontalMovementDistance() => Mathf.Abs(fingersInfo[fingerId].EndPosition.x - fingersInfo[fingerId].StartPosition.x);
}