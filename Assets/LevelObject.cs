using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRes", menuName = "Levels/LevelObject", order = 1)]
public class LevelObject : ScriptableObject
{
    public Vector3 DogSpawn;
    public List<Vector3> VacheSpawn;
    public int nbrVaches;
    public GameObject level;
    public Vector3 signPos;

}
