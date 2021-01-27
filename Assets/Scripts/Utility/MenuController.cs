using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject slider;
    public GameObject pages;

    public void LoadGameScene(int sceneId)
    {
        StartCoroutine(LoadAsynchronously(sceneId));
    }

    IEnumerator LoadAsynchronously(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        slider.SetActive(true);
        pages.SetActive(false);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.GetComponent<Slider>().value = progress;

            yield return null;
        }
    }
}