using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public List<Vector3> Points;
    public int width;
    public int height;
}
