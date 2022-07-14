using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject pendingObject;

    private Vector3 pos;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
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
}
