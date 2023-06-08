using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingTypeSelectUI : MonoBehaviour
{
    private Dictionary<BuildingTypeSO, Transform> buildingBtnDictionary;

    [SerializeField] private List<BuildingTypeSO> buildingTypeSOList;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private GameObject buildingBtnTemplate;

    private void Awake()
    {
        buildingBtnDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;
        foreach (BuildingTypeSO buildingTypeSO in buildingTypeSOList)
        {
            GameObject newButton = Instantiate(buildingBtnTemplate, transform);
            newButton.SetActive(true);
            newButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 180, 0);

            Image buttonImage = newButton.transform.Find("image").GetComponent<Image>();
            buttonImage.sprite = buildingTypeSO.sprite;

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() =>
            {
                buildingManager.SetActiveBuildingType(buildingTypeSO);
                UpdateSelectedVisual();
            });

            Transform selectedVisual = newButton.transform.Find("selected");
            buildingBtnDictionary[buildingTypeSO] = selectedVisual;

            index++;
        }

        buildingBtnTemplate.SetActive(false);
    }

    private void Start()
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (Transform selectedVisual in buildingBtnDictionary.Values)
        {
            selectedVisual.gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = buildingManager.GetActiveBuildingType();
        buildingBtnDictionary[activeBuildingType].gameObject.SetActive(true);
    }
}


/*
public class BuildingTypeSelectUI : MonoBehaviour
{

    private Dictionary<BuildingTypeSO, Transform> buildingBtnDictionary;

    [SerializeField] private List<BuildingTypeSO> buildingTypeSOList;
    [SerializeField] private BuildingManager buildingManager;

    private void Awake()
    {
        Transform buildingBtnTemplate = transform.Find("BuildingBtnTemplate");
        buildingBtnTemplate.gameObject.SetActive(false);

        buildingBtnDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;
        foreach (BuildingTypeSO buildingTypeSO in buildingTypeSOList)
        {
                
            Transform buildingBtnTransform = Instantiate(buildingBtnTemplate, transform);
            buildingBtnTransform.gameObject.SetActive(true);
            buildingBtnTransform.GetComponent<RectTransform>().anchoredPosition +=  new Vector2(index * 180, 0);
            buildingBtnTemplate.Find("image").GetComponent<UnityEngine.UI.Image>().sprite = buildingTypeSO.sprite;       

            buildingBtnTransform.GetComponent<Button>().onClick.AddListener(() => {
                buildingManager.SetActiveBuildingType(buildingTypeSO);
                UpdateSelectedVisual();
            });
            
            buildingBtnDictionary[buildingTypeSO] = buildingBtnTransform;

            index++;
        }
    }

    private void Start() {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual() {
        foreach (BuildingTypeSO buildingTypeSO in buildingBtnDictionary.Keys) {
            buildingBtnDictionary[buildingTypeSO].Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activebuildingType = buildingManager.GetActiveBuildingType();
        buildingBtnDictionary[activebuildingType].Find("selected").gameObject.SetActive(true);
    }
}
*/