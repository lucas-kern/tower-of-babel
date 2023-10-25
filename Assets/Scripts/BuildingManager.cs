using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                

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
    private Base baseData;
    private User userData;
    public TextMeshProUGUI informationMessageText;

    private Dictionary<string, GameObject> BuildingPrefabMap = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        // Populate the dictionary with building names and prefabs
        BuildingPrefabMap.Add("tower", TowerPrefab);
        BuildingPrefabMap.Add("goldmine", GoldMinePrefab);
        BuildingPrefabMap.Add("goldstorage", GoldStoragePrefab);
        
        // Check if you have valid user data
        userData = UserDataHolder.Instance.UserData;

        if (userData == null)
        {
            // No valid user data, return to login scene
            SceneManager.LoadScene("LoginScene"); // Replace "LoginScene" with your actual login scene name
            return;
        }
        baseData = userData.Base;

        // Load base objects based on userData
        LoadBaseObjects(baseData);
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

// TODO update this to send to the backend and save the data 
    public async void PlaceObject()
    {
        if (pendingObject != null)
        {
                // Access the Building script component attached to the prefab
            BuildingWrapper buildingComponent = pendingObject.GetComponent<BuildingWrapper>();
            if (buildingComponent != null)
            {
                // Access the name property from the Building component
                string prefabName = buildingComponent.buildingData.name;

                // Obtain the top left corner for the backend
                Renderer objectRenderer = pendingObject.GetComponent<Renderer>();
                Vector3 objectSize = objectRenderer.bounds.size;
                Vector3 objectCenter = pendingObject.transform.position;
                Vector3 topLeftCorner = objectCenter - new Vector3(objectSize.x / 2, 0, objectSize.z / 2);

                // Set the position properties
                buildingComponent.buildingData.posX = topLeftCorner.x;
                buildingComponent.buildingData.posY = topLeftCorner.y;
                buildingComponent.buildingData.posZ = topLeftCorner.z;
                buildingComponent.buildingData.width = objectSize.x;
                buildingComponent.buildingData.height = objectSize.z;

                // Send the updated base data to the backend
                Base updatedBase = await networkController.PlaceBuilding(buildingComponent.buildingData);

                if (updatedBase != null)
                {
                    // Add the new building to the base data
                    baseData = updatedBase;

                    pendingObject.transform.SetParent(buildingParent);

                    // The base data was successfully updated on the backend, so the object can be placed
                    Debug.Log("Base data sent to the backend successfully!");

                    // Set pendingObject to null and destroy the GameObject
                    pendingObject = null;
                    Destroy(pendingObject);
                }
                else
                {
                    informationMessageText.text = "Building cannot be placed";
                    StartCoroutine(ResetInformationText(3.0f));
                    // Handle the case where the base data failed to save
                    Debug.LogError("Failed to send base data to the backend. Object placement cancelled.");
                }
            }
            else
            {
                Debug.LogError("Building component not found on the prefab.");
            }
        }
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
        foreach (var buildingType in baseData.buildings)
        {
            string buildingName = buildingType.Key;
            List<Building> buildingsOfType = buildingType.Value;

            foreach (Building building in buildingsOfType)
            {
                // Instantiate a GameObject for each building type
                GameObject buildingPrefab = GetBuildingPrefab(buildingName);

                // Calculate the center position based on the top-left corner
                Vector3 topLeftCorner = new Vector3(building.posX, building.posY, building.posZ);
                Vector3 objectSize = buildingPrefab.GetComponent<Renderer>().bounds.size;
                Vector3 objectCenter = topLeftCorner + new Vector3(objectSize.x / 2, 0, objectSize.z / 2);

                // Instantiate the building at the specified position
                // Vector3 position = new Vector3(building.posX, building.posY, building.posZ);
                GameObject instantiatedBuilding = Instantiate(buildingPrefab, objectCenter, Quaternion.identity);

                // Attach to the building parent (you need to have a reference to the parent GameObject)
                instantiatedBuilding.transform.SetParent(buildingParent);
            }
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

        // Coroutine to reset the text to empty after a delay
    private IEnumerator ResetInformationText(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (informationMessageText != null)
        {
            informationMessageText.text = "";
        }
    }
}
