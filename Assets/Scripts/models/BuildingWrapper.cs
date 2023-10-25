using System;
using System.Collections.Generic;
using UnityEngine;

// This class represents a Building in Unity
[Serializable]
public class Building
{
    public string name;
    public float posX;
    public float posY;
    public float posZ;
    public float width;
    public float height;
}

public class BuildingWrapper : MonoBehaviour
{
    public Building buildingData;

}
