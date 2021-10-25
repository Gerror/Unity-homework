using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SpawnObjectMechanics : MonoBehaviour
    {
        [Min(1)] [SerializeField] private int _maxSize = 2;
        [Min(1)] [SerializeField] private float _maxLifeTime = 5.0f;
        [SerializeField] private float fine = -1f;
        [Min(1)] [SerializeField] private float maxReward = 1f;
        
        private float _spawnTime;
        private float _currentDeltaTime;

        public event Action<float> BurstSpawnObjectEvent;

        private void OnValidate()
        {
            if (fine > -1f)
                fine = -1f;
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
                BurstSpawnObjectEvent?.Invoke(fine);
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            var reward = maxReward * (1f - (_currentDeltaTime / _maxLifeTime));
            BurstSpawnObjectEvent?.Invoke(reward);
            Destroy(gameObject);
        }
    }
}