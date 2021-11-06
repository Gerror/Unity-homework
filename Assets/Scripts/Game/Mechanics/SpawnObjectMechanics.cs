using System;
using Game.Core.Sounds;
using UnityEngine;
using Zenject;
using Game.Core;

namespace Game.Mechanics
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SpawnObjectMechanics : MonoBehaviour
    {
        [Min(1)] [SerializeField] private int _maxSize = 2;
        [Min(1)] [SerializeField] private float _maxLifeTime = 5.0f;
        [Min(1)] [SerializeField] private float maxReward = 1f;

        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private AudioClip _burstClip;
        
        private SoundManager _soundManager;
        private ScoreManager _scoreManager;
        private PrefabFactory _prefabFactory;
        
        private float _spawnTime;
        private float _currentDeltaTime;

        public event Action BurstSpawnObjectEvent;
        
        [Inject]
        private void Construct(SoundManager soundManager, ScoreManager scoreManager, PrefabFactory prefabFactory)
        {
            _soundManager = soundManager;
            _scoreManager = scoreManager;
            _prefabFactory = prefabFactory;
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
                Burst();
            }
        }

        public void OnClick()
        {
            var reward = maxReward * (1f - (_currentDeltaTime / _maxLifeTime));
            _scoreManager.AddScore(reward);
            Burst();
        }

        private void Burst()
        {
            GameObject explosion = _prefabFactory.Spawn(
                _explosionPrefab,
                transform.position,
                _explosionPrefab.transform.rotation,
                gameObject.transform.parent);
            explosion.transform.localScale = gameObject.transform.localScale;
            _soundManager.CreateSoundObject().Play(_burstClip, transform.position, false, 0.25f);
            BurstSpawnObjectEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}