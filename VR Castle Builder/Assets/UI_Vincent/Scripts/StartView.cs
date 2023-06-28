using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class StartView : MonoBehaviour
{
    public void StartGame()
    {

        SceneManager.LoadScene(1);
    }
}
