using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRes", menuName = "Levels/LevelObject", order = 1)]
public class LevelObject : ScriptableObject
{
    public Vector3 DogSpawn;
    public Vector3 VacheSpawn;
    public GameObject level;
}
