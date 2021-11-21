using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

namespace Game
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _floor;
        [SerializeField] private LevelConfig _levelConfig;
        
        public IReadOnlyList<Vector3> Points => _levelConfig.Points;

        [MenuItem("CONTEXT/LevelMap/Load map")]
        private static void LoadMap(MenuCommand command)
        {
            ClearMap(command);
            
            var levelMap = command.context as LevelMap;
            if (levelMap == null)
                return;

            foreach (var p in levelMap._levelConfig.Points.Distinct())
            {
                var prefab = PrefabUtility.InstantiatePrefab(levelMap._prefab, levelMap._root) as GameObject;
                prefab.transform.position = p;
            }

            Transform floorTransform = levelMap._floor.transform; 
            floorTransform.gameObject.SetActive(true);
            floorTransform.localScale = new Vector3(
                levelMap._levelConfig.width, 
                1, 
                levelMap._levelConfig.height);

            floorTransform.position = new Vector3(
                levelMap._levelConfig.width / 2f,
                -0.5f,
                levelMap._levelConfig.height / 2f);
            
            NavMeshBuilder.BuildNavMesh();
        }
        
        [MenuItem("CONTEXT/LevelMap/Clear map")]
        private static void ClearMap(MenuCommand command)
        {
            var levelMap = command.context as LevelMap;
            if (levelMap == null)
                return;
            
            var count = levelMap._root.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(levelMap._root.GetChild(i).gameObject);
            }
            levelMap._floor.gameObject.SetActive(false);
            
            NavMeshBuilder.ClearAllNavMeshes();
        }
    }
}