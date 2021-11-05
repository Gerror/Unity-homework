using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Managers;
using UnityEngine;

namespace Game.Mechanics
{
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private MainUiController _mainUiController;
        [SerializeField] private SpawnMechanics _spawnMechanics;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private TimeManager _timeManager;
        
        void OnValidate()
        {
            if (!_spawnMechanics)
                _spawnMechanics = FindObjectOfType<SpawnMechanics>();
            if (!_scoreManager)
                _scoreManager = FindObjectOfType<ScoreManager>();
            if (!_timeManager)
                _timeManager = FindObjectOfType<TimeManager>();
            if (!_mainUiController)
                _mainUiController = FindObjectOfType<MainUiController>();
        }

        void Start()
        {
            _spawnMechanics.BurstSpawnObjectEvent += BurstSpawnObject;

            _timeManager.EndTimeEvent += EndGame;

            _mainUiController.BackToMainMenuEvent += DeactivateSpawnMechanics;
            _mainUiController.EndGameEvent += EndGame;
            _mainUiController.StartGameEvent += StartGame;
        }
        
        private void BurstSpawnObject(float result)
        {
            _scoreManager.AddScore(result);
        }

        private void DeactivateSpawnMechanics()
        {
            _spawnMechanics.gameObject.SetActive(false);
        }

        private void StartGame()
        {
            _spawnMechanics.gameObject.SetActive(true);
            _spawnMechanics.Respawn();
            Time.timeScale = 1;
            _timeManager.StartTimer();
        }

        private void EndGame()
        {
            _spawnMechanics.gameObject.SetActive(false);

            Time.timeScale = 0;
            _scoreManager.ResetScore();
        }
    }
}
