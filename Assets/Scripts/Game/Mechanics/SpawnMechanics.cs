using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Mechanics
{
    public class SpawnMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnObjectPrefab;
        [SerializeField] private float _borderMultiplier = 0.8f;
        [Min(1)] [SerializeField] private int _maxSpawnObject = 10;

        private Camera _mainCamera;
        private int _spawnObjectCount;
        
        public event Action<float> BurstSpawnObjectEvent;

        private void OnValidate()
        {
            if (_borderMultiplier < 0.5f)
                _borderMultiplier = 0.5f;
        }

        void Start()
        {
            _mainCamera = Camera.main;
            _spawnObjectCount = 0;
            SpawnObjects(Random.Range(1, _maxSpawnObject + 1));
        }

        private void SpawnObjects(int count)
        {
            for (int i = 0; i < count; i++) {
                SpawnObject();    
            }

            _spawnObjectCount += count;
        }

        private void SpawnObject()
        {
            float x = Random.Range(Screen.width * (1.0f - _borderMultiplier), Screen.width * _borderMultiplier);
            float y = Random.Range(Screen.height * (1.0f - _borderMultiplier), Screen.height * _borderMultiplier);
            Vector3 screenPosition = new Vector3(x, y, 0f);
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
            
            GameObject go = Object.Instantiate(_spawnObjectPrefab, worldPosition, Quaternion.identity);
            go.transform.SetParent(gameObject.transform);
            go.GetComponent<SpawnObjectMechanics>().BurstSpawnObjectEvent += BurstSpawnObject;
        }

        private void BurstSpawnObject(float result)
        {
            _spawnObjectCount--;
            SpawnObjects(Random.Range(1, _maxSpawnObject - _spawnObjectCount + 1));
            BurstSpawnObjectEvent?.Invoke(result);
        }
    }
}