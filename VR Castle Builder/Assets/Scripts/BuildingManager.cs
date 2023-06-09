using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    //singleton to access the manager from other scripts
    public static BuildingManager current;
    //buildings prefabs
    /*public GameObject prefab1;
    public GameObject prefab2;
    */
    [SerializeField] private BuildingTypeSO activebuildingType;
    //variables    
    private bool _mouseState;
    private GameObject currentObj;
    public Vector3 screenSpace;
    public Vector3 offset;


    #region Unity methods
    private void Awake()
    {
        //currentObj = null;
    }

    private void Update()
    {   
        //Drag & Drop Objects
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            currentObj = GetClickedMovableObject (out hitInfo);
            if (currentObj != null) 
            {
                _mouseState = true;
                screenSpace = Camera.main.WorldToScreenPoint (currentObj.transform.position);
                offset = currentObj.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            }
        }
        if (Input.GetMouseButtonUp (0)) 
        {
            _mouseState = false;
        }
        if (_mouseState) 
        {
            var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;
            currentObj.transform.position = curPosition;
        }
        
        //Rotate Objects
        if (Input.GetKeyDown(KeyCode.V))
        {   
            if(currentObj != null)
            {
            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            // Die Position des Zielobjekts um 90 Grad im Uhrzeigersinn drehen
            currentObj.transform.Rotate(rotation.eulerAngles, Space.World);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(currentObj != null)
            {
                Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
                currentObj.transform.Rotate(rotation.eulerAngles, Space.World);
            }
        }
        
        //Spawn activebuildingType
        if (Input.GetKeyDown(KeyCode.T))
        {
            Perspektivenwechsel p = GameObject.Find("Perspektivenwechsel").GetComponent<Perspektivenwechsel>();
            if (p.innenAnsicht) { p.Toggle(); }

            Vector3 mouseWorldPosition = GetMouseWorldPosition(5f);
            Transform instantiatedObject = Instantiate(activebuildingType.prefab, mouseWorldPosition, Quaternion.identity);
            instantiatedObject.SetParent(GameObject.Find("Scene").transform, false);

            if (p.innenAnsicht) { p.Toggle(); }
        }
    }

        //current pos
        //Input fields
        //spawn object
        //prevent null errors
        //rotate object 
        //check placement
        //cancel placement

    #endregion

    #region Utils

    GameObject GetClickedMovableObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.collider.gameObject;

            // Überprüfe, ob das Zielobjekt das "PlaceableObject" Skript hat
            if (target.GetComponent<PlaceableObject>() == null)
            {
                // Zielobjekt hat das "PlaceableObject" Skript nicht, setze es auf null
                target = null;
            }
        }
        return target;
    }

    public static Vector3 GetMouseWorldPosition(float distanceFromCamera)
    {
        Vector3 mousePosition = Input.mousePosition; // Position des Mauszeigers im Bildschirmraum
        mousePosition.z = distanceFromCamera; // Setze die z-Koordinate entsprechend des Abstands zur Kamera
        return Camera.main.ScreenToWorldPoint(mousePosition); // Wandelt den Bildschirmraumpunkt in eine Weltposition um
    }

    #endregion
    
    #region Building Placement
    public void SetActiveBuildingType(BuildingTypeSO buildingTypeSO) {
        activebuildingType = buildingTypeSO;
    }

    public BuildingTypeSO GetActiveBuildingType() {
        return activebuildingType;        
    }
    #endregion
}
