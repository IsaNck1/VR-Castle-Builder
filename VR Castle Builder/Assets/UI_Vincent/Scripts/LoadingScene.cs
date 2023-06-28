using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    private void Awake()
    {
        LoadGame();
    }

    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("xPos"))
        {
            Vector3 loadedPos = new Vector3();

            loadedPos.x = PlayerPrefs.GetFloat("xPos");
            loadedPos.y = PlayerPrefs.GetFloat("yPos");
            loadedPos.z = PlayerPrefs.GetFloat("zPos");

            transform.position = loadedPos;

            Debug.Log("Load Position");
        }
    }

            IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

                yield return null;
        }
    }
}
