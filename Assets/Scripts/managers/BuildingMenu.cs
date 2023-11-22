using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingButtonPrefab;
    public BuildingManager buildingManager;
    public Transform contentTransform;

    private void Start()
    {
    }

    public void OpenBuildingMenu()
    {
        PopulateMenu();
        gameObject.SetActive(true);
    }

    public void CloseBuildingMenu()
    {
        gameObject.SetActive(false);
    }
    private void PopulateMenu()
    {
        foreach (var buildingName in buildingManager.BuildingPrefabMap.Keys)
        {
            GameObject buttonObj = Instantiate(buildingButtonPrefab, contentTransform);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = buildingName;
            button.onClick.AddListener(() => buildingManager.SelectObject(buildingName));
        }
    }
}