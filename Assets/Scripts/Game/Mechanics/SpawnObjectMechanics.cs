using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SpawnObjectMechanics : MonoBehaviour
    {
        [SerializeField] private int _maxSize = 2;
        [SerializeField] private float _maxLifeTime = 5.0f;
        [SerializeField] private float fine = -1f;
        [SerializeField] private float maxReward = 1f;
        
        private float _spawnTime;
        private float _currentDeltaTime;

        public event Action<float> BurstSpawnObject;

        private void OnValidate()
        {
            if (_maxSize < 1)
                _maxSize = 1;
            if (maxReward < 1)
                maxReward = 1;
            if (fine > -1f)
                fine = -1f;
            if (_maxSize < 1)
                _maxSize = 1;
            if (_maxLifeTime < 1)
                _maxLifeTime = 1;
        }

        void Start()
        {
            _spawnTime = Time.time;
            gameObject.transform.localScale = Vector3.one;
        }

        private void Update()
        {
            _currentDeltaTime = Time.time - _spawnTime;
            
            gameObject.transform.localScale = 
                _maxSize * (_currentDeltaTime / _maxLifeTime) * Vector3.one + Vector3.one;
            
            if (_currentDeltaTime >= _maxLifeTime)
            {
                BurstSpawnObject?.Invoke(fine);
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            var reward = maxReward * (1f - (_currentDeltaTime / _maxLifeTime));
            BurstSpawnObject?.Invoke(reward);
            Destroy(gameObject);
        }
    }
}