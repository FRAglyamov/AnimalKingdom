using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
    public RectTransform panel;
    public GameObject[] pages;
    public RectTransform center;
    public float speed = 25f;
    public int centerPageNum = 1;

    private float[] distance;
    private bool dragging = false;
    private int pageDist;
    private int minPageNum;

    void Start()
    {
        int pagesLength = pages.Length;
        distance = new float[pagesLength];

        pageDist = (int)Mathf.Abs(pages[1].GetComponent<RectTransform>().anchoredPosition.x - pages[0].GetComponent<RectTransform>().anchoredPosition.x);
        //Debug.Log(pageDist);
    }

    void Update()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - pages[i].transform.position.x);
        }

        minPageNum = Array.IndexOf(distance, Mathf.Min(distance));
        //Debug.Log("Min Page: " + minPageNum);

        if (!dragging)
        {
            LerpToPage((minPageNum - centerPageNum) * -pageDist);
#if Unit_Editor && Debug
            Debug.Log("Lerp to page: " + (minPageNum - centerPageNum));
            Debug.Log("Lerp to pos: " + (minPageNum - centerPageNum) * -pageDist);
#endif
        }

    }

    void LerpToPage(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPos;
    }

    public void StartDrag() => dragging = true;

    public void EndDrag() => dragging = false;
}