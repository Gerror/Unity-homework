using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;
using Game.Core;

namespace Game.Mechanics
{
    public class SpawnMechanics : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnObjectPrefab;
        [SerializeField] private List<Transform> _spawnerTransforms;

        private PrefabFactory _prefabFactory;
        private GameSettings _gameSettings;
        private int _spawnObjectCount;
        
        [Inject]
        private void Construct(PrefabFactory prefabFactory, GameSettings gameSettings)
        {
            _prefabFactory = prefabFactory;
            _gameSettings = gameSettings;
        }

        private void OnValidate()
        {
            if (_spawnerTransforms.Count != transform.childCount)
            {
                _spawnerTransforms = new List<Transform>();
                foreach (Transform spawner in transform)
                {
                    _spawnerTransforms.Add(spawner);
                }
            }
        }

        public void Respawn()
        {
            _spawnObjectCount = 0;
            List<GameObject> allSpawnObjects = new List<GameObject>();

            foreach (Transform spawner in _spawnerTransforms)
            {
                foreach (Transform spawnObject in spawner)
                {
                    allSpawnObjects.Add(spawnObject.gameObject);
                }
            }

            foreach (GameObject spawnObject in allSpawnObjects)
            {
                DestroyImmediate(spawnObject.gameObject);
            }

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            foreach (Transform spawner in _spawnerTransforms)
            {
                StartCoroutine(IntervalSpawnObject(spawner));
            }
        }

        private IEnumerator IntervalSpawnObject(Transform spawner)
        {
            SpawnObject(spawner);
            while (true)
            {
                yield return new WaitForSeconds(_gameSettings.SpawnInterval);
                SpawnObject(spawner);
            }
        }
        
        private void SpawnObject(Transform spawner)
        {
            if (_spawnObjectCount >= _gameSettings.MaxSpawnObjectCount)
                return;

            GameObject go = _prefabFactory.Spawn(_spawnObjectPrefab, spawner.position, Quaternion.identity, spawner);
            go.GetComponent<SpawnObjectMechanics>().BurstSpawnObjectEvent += BurstSpawnObject;

            Rigidbody2D goRigidbody = go.GetComponent<Rigidbody2D>();
            goRigidbody.velocity = Vector3.zero - spawner.position;
            _spawnObjectCount++;
        }

        private void BurstSpawnObject()
        {
            _spawnObjectCount--;
            SpawnObject(_spawnerTransforms[Random.Range(0, _spawnerTransforms.Count)]);
        }
    }
}