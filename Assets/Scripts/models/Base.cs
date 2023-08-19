using System;

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

// This class represents a Base in Unity
public class Base
{
    public string ID;
    public string owner;
    public Building[] buildings; // Array of buildings
}