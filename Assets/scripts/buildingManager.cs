using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildingManager : MonoBehaviour
{
    private bool mapLoaded;
    public GameObject[] objects;
    private GameObject pendingObject;

    private Vector3 pos;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        if(!mapLoaded)
        {
            PlaceObjects();
            mapLoaded = true;
        }
        if(pendingObject != null)
        {
            pendingObject.transform.position = pos;
            if(Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }
    }

    void PlaceObject()
    {
        pendingObject = null;
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        appManager.instance.StartCoroutine("GetData", "/bases");
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
    }

    public void PlaceObjects()
    {
        int index = 2;
        appManager.instance.StartCoroutine("GetData", "/bases");
        Vector3 pos2 = new Vector3(appManager.instance.tower.sphere[0],0.5f, appManager.instance.tower.sphere[1]);
        Instantiate(objects[index], pos2, transform.rotation);
        pos2 = new Vector3(appManager.instance.tower.cube[0],0.5f, appManager.instance.tower.cube[1]);
        Instantiate(objects[0], pos2, transform.rotation);
        pos2 = new Vector3(appManager.instance.tower.cylinder[0],0.5f, appManager.instance.tower.cylinder[1]);
        Instantiate(objects[1], pos2, transform.rotation);
    }
}
