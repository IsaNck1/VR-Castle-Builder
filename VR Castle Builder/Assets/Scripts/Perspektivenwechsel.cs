using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspektivenwechsel : MonoBehaviour
{

    public GameObject terrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            if (terrain != null) terrain.SetActive(!terrain.activeSelf);
        }
    }
}
