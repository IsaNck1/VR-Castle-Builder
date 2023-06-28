using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public GameObject PauseMenu;

    private void Awake()
    {
        PauseMenu.SetActive(false);
    }
    private void PauseGame()
    {
        PauseMenu.SetActive(true);

        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;

        PauseMenu.SetActive(false);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
    public void SaveGame()
    {
        Vector3 posToSave = transform.position;

        PlayerPrefs.SetFloat("xPos", posToSave.x);
        PlayerPrefs.SetFloat("yPos", posToSave.y);
        PlayerPrefs.SetFloat("zPos", posToSave.z);

        Debug.Log("Save Game");



        Vector3 loadedPos = new Vector3();

        loadedPos.x = PlayerPrefs.GetFloat("xPos");
        loadedPos.y = PlayerPrefs.GetFloat("yPos");
        loadedPos.z = PlayerPrefs.GetFloat("zPos");

        transform.position = loadedPos;

        Debug.Log("Load Position");


    }



    private bool hasButtonBeenClicked = false;

        public void OnButtonClick()
        {
            if (!hasButtonBeenClicked)
            {
                
                    if (PauseMenu.activeSelf)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }

                    // Führe den Code aus, der durch GetKeyDown ersetzt werden soll
                    Debug.Log("Button wurde geklickt!");
                    hasButtonBeenClicked = true;
                    ResetButtonClickedState();

            }

        }
    private void ResetButtonClickedState()
    {
        // Setze die Variable zurück
        hasButtonBeenClicked = false;
    }
}
