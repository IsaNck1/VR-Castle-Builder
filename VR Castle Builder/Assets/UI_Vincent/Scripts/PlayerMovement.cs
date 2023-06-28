using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    private void Awake()
    {
        LoadGame();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.W) == true)
        {

            transform.position = new Vector3(0, 0, transform.position.z + 0.01f);

        }
        else if (Input.GetKey(KeyCode.S) == true)
        {
            transform.position = new Vector3(0, 0, transform.position.z - 0.01f);
        }

        if (Input.GetKey(KeyCode.A) == true)
        {

            transform.position = new Vector3(transform.position.x - 0.01f, 0, 0);

        }
        else if (Input.GetKey(KeyCode.D) == true)
        {
            transform.position = new Vector3(transform.position.x + 0.01f, 0, 0);

            GameManager manager = FindObjectOfType<GameManager>();
            manager.LoadNextLevel();
        }
    }

    /*    //Save
        if(Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
    }
    public void SaveGame()
    {
        Vector3 posToSave = transform.position;

        PlayerPrefs.SetFloat("xPos", posToSave.x);
        PlayerPrefs.SetFloat("yPos", posToSave.y);
        PlayerPrefs.SetFloat("zPos", posToSave.z);
    }*/

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
}