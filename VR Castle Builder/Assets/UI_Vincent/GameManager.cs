using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevelIndex = 0;

    public void LoadNextLevel()
    {

        Debug.Log("Called next level");

        currentLevelIndex += 1;
        SceneManager.LoadScene(currentLevelIndex);
    }
}
