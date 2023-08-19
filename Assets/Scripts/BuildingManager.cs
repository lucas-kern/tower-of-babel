using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject pendingObject;
    public GameObject TowerPrefab;
    public GameObject GoldMinePrefab;
    public GameObject GoldStoragePrefab;

    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    public float gridSize;
    bool gridOn = true;
    [SerializeField] private Toggle gridToggle;
    public NetworkController networkController;
    public Transform buildingParent;

    private Dictionary<string, GameObject> BuildingPrefabMap = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        // Populate the dictionary with building names and prefabs
        BuildingPrefabMap.Add("Tower", TowerPrefab);
        BuildingPrefabMap.Add("GoldMine", GoldMinePrefab);
        BuildingPrefabMap.Add("GoldStorage", GoldStoragePrefab);
        
        // Check if you have valid user data
        User userData = UserDataHolder.Instance.UserData;

        if (userData == null)
        {
            // No valid user data, return to login scene
            SceneManager.LoadScene("LoginScene"); // Replace "LoginScene" with your actual login scene name
            return;
        }

        // Load base objects based on userData
        LoadBaseObjects(userData.Base);
    }

    // Update is called once per frame
    void Update()
    {
        if(pendingObject != null)
        {
            if(gridOn)
            {
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z)
                );
            }
            else{ pendingObject.transform.position = pos; }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }
    }

    public void PlaceObject()
    {
        pendingObject = null;
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
    }

    public void ToggleGrid()
    {
        if(gridToggle.isOn)
        {
            gridOn = true;
        }
        else{ gridOn = false; }
    }
    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if(xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    private void LoadBaseObjects(Base baseData)
    {
        foreach (Building building in baseData.buildings)
        {
            // Instantiate a GameObject for each building type
            GameObject buildingPrefab = GetBuildingPrefab(building.name);

            // Instantiate the building at the specified position
            Vector3 position = new Vector3(building.posX, building.posY, building.posZ);
            GameObject instantiatedBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);

            // Attach to the building parent (you need to have a reference to the parent GameObject)
            instantiatedBuilding.transform.SetParent(buildingParent);
        }
    }


    // Instance method to get the prefab based on the building name
    public GameObject GetBuildingPrefab(string buildingName)
    {
        if (BuildingPrefabMap.TryGetValue(buildingName, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"Prefab not found for building: {buildingName}");
            return null;
        }
    }
}
