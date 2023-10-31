using System;
using System.Collections.Generic;

// This class represents a Base in Unity
public class Base
{
    public string ID;
    public string owner;
    public Dictionary<string, List<Building>> buildings; // Map of buildings to the type
    public List<List<Building>> grid; // Representation of the map
}