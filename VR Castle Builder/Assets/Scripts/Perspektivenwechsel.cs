using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Perspektivenwechsel : MonoBehaviour
{
    public GameObject terrain;
	public GameObject tisch;
	public GameObject scene;
	public GameObject player;
	public bool innenAnsicht = true; // Nur bauen wenn innenAnsicht false

	public float outerScale = 0.05f;
	public float outerPositionY = 1.0f;
	public float entfernungZumTisch = 1.5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("k"))
		{
			ToggleActivate();
		}
	}

	public void Toggle()
    {
			for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
			{
				Hand hand = Player.instance.hands[handIndex];
				if (hand != null)
				{
					ToggleActivate();
				}
			}
	}

	public void ToggleActivate()
	{

		innenAnsicht = !innenAnsicht;
		if (terrain != null) terrain.SetActive(innenAnsicht);
		if (tisch != null) tisch.SetActive(!innenAnsicht);
		if (scene != null && player != null)
		{
			if (innenAnsicht)
			{
				// Teleportiere Burg an die Position des Spielers
				scene.transform.localPosition = new Vector3(0, 0, 0);
				scene.transform.localScale = new Vector3(1, 1, 1);
			}
			else
			{
				// Schrumpfe Burg und platziere sie vor Spieler
				scene.transform.localPosition = 
					player.transform.position + 
					entfernungZumTisch * player.transform.forward + 
					new Vector3(0, outerPositionY, 0);
				scene.transform.localEulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
				scene.transform.localScale = new Vector3(outerScale, outerScale, outerScale);
			}
		}
	}
}
