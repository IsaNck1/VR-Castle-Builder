using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspektivenwechsel : MonoBehaviour
{

    public GameObject terrain;
	public GameObject tisch;
	public GameObject scene;
	public GameObject player;
	public bool innenAnsicht = true;

	public float outerScale = 0.05f;
	public float outerPositionY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
			innenAnsicht = !innenAnsicht;
            if (terrain != null) terrain.SetActive(innenAnsicht);
			if (tisch != null) tisch.SetActive(!innenAnsicht);
			if (scene != null && player != null) {
				if(innenAnsicht) {
					// Teleportiere Burg an die Position des Spielers
					scene.transform.localPosition = player.transform.position;
					scene.transform.localScale = new Vector3(1,1,1);
				} else {
					// Schrumpfe Burg und platziere sie vorm Spieler
					scene.transform.localPosition = player.transform.position + player.transform.forward + new Vector3(0,outerPositionY,0);
					scene.transform.localEulerAngles = new Vector3(0,player.transform.eulerAngles.y,0);
					scene.transform.localScale = new Vector3(outerScale,outerScale,outerScale);
				}
			}
        }
    }
}
