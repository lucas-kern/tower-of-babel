using UnityEngine;

[System.Serializable]
public class towerBase
{
    public int id;
    public string name;
    public int[] sphere;
    public int[] cube;
    public int[] cylinder;

    public static towerBase CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<towerBase>(jsonString);
    }

    public static string toString(towerBase tower)
    {
        return string.Format("{0}, {1}, {2}, {3}", tower.name, tower.sphere, tower.cube, tower.cylinder);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}