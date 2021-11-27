using System.Collections;
using System.Collections.Generic;
using Game.Configs;
using UnityEngine;
using View;
using View.Components;

namespace Game
{
    public class AccelerationManager
    {
        private readonly NetworkManager _networkManager;
        private readonly GameConfig _config;
        private readonly GameplayView _gameplayView;

        public AccelerationManager(NetworkManager networkManager, GameConfig config, GameplayView gameplayView)
        {
            _gameplayView = gameplayView;
            _networkManager = networkManager;
            _config = config;
        }

        public void StartAccelerationManager()
        {
            SpawnAccelerationEffect();
        }

        private void SpawnAccelerationEffect()
        {
            var points = _gameplayView.AccelerationSpawnPoints;
            var spawnPoint = points[Random.Range(0, points.Length)];
            
            float probability = Random.Range(0f, 1f);
            string prefabPath =
                probability > 0.5f ? _config.AccelerationPrefab.Path : _config.DecelerationPrefab.Path;

            AccelerationComponent accelerationComponent = _networkManager.CreateAccelerationEffect(prefabPath, spawnPoint.position, spawnPoint.rotation);
            accelerationComponent.TakeEffect += SpawnAccelerationEffect;
        }
    }
}