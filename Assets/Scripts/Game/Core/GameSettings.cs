using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameSettings : MonoBehaviour
    {
        [Header("Общие настройки")] 
        [Min(8)] public int MaxSpawnObjectCount = 8;
        [Min(5f)] public float SpawnInterval = 5f;
        
        [Header("Режим игры на количество очков")]
        [Min(15)] public float MaxScore = 15f;
        
        [Header("Режим игры на время")]
        [Min(15)] public int MaxTime = 15;
    }
}