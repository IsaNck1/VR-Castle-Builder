using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickSave : MonoBehaviour
{
    private bool hasButtonBeenClicked = false;

    public void OnButtonClick()
    {
        if (!hasButtonBeenClicked)
        {

            SaveGame();

            // Führe den Code aus, der durch GetKeyDown ersetzt werden soll
            Debug.Log("Speichern erfolgreich!");
            hasButtonBeenClicked = true;
            ResetButtonClickedState();

        }

    }

    public void SaveGame()
    {
        Vector3 posToSave = transform.position;

        PlayerPrefs.SetFloat("xPos", posToSave.x);
        PlayerPrefs.SetFloat("yPos", posToSave.y);
        PlayerPrefs.SetFloat("zPos", posToSave.z);
    }

    private void ResetButtonClickedState()
    {
        // Setze die Variable zurück
        hasButtonBeenClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
