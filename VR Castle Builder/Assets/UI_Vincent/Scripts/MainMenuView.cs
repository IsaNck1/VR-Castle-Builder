using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MainMenuView : MonoBehaviour
{
    public void EndGame()
    {

        SceneManager.LoadScene(0);
    }
}
