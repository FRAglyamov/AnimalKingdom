using UnityEngine;

public class SwipeLogger : MonoBehaviour
{
    private void Awake()
    {
        SwipeDetection.OnSwipe += SwipeDetection_OnSwipe;
    }

    private void SwipeDetection_OnSwipe(SwipeDetection.SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);
        Debug.Log(data.StartPosition);
        Debug.Log(data.EndPosition);
    }
}
