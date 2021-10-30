using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using Game.UI;
using UnityEngine;

namespace Game.Mechanics
{
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private SpawnMechanics _spawnMechanics;

        [SerializeField] private StartWindow _startWindow;

        [SerializeField] private GameScreen _gameScreen;

        [SerializeField] private EndGameWindow _endGameWindow;

        [SerializeField] private SettingsWindow _settingsWindow;

        [SerializeField] private AboutMeWindow _aboutMeWindow;

        [Min(15)] [SerializeField] private int _maxTime = 60;

        private string _playerName = "";
        private int _currentTime = 0;
        private float _currentScore = 0f;

        void OnValidate()
        {
            if (!_spawnMechanics)
                _spawnMechanics = FindObjectOfType<SpawnMechanics>();
            if (!_startWindow)
                _startWindow = FindObjectOfType<StartWindow>();
            if (!_gameScreen)
                _gameScreen = FindObjectOfType<GameScreen>();
            if (!_endGameWindow)
                _endGameWindow = FindObjectOfType<EndGameWindow>();
            if (!_settingsWindow)
                _settingsWindow = FindObjectOfType<SettingsWindow>();
            if (!_aboutMeWindow)
                _aboutMeWindow = FindObjectOfType<AboutMeWindow>();
        }

        void Start()
        {
            BackToMainMenu();
            
            _spawnMechanics.BurstSpawnObjectEvent += BurstSpawnObject;
            
            _startWindow.StartEvent += StartGame;
            _startWindow.ExitEvent += ExitHelper.Exit;
            _startWindow.SettingsEvent += OpenSettings;
            _startWindow.AboutMeEvent += OpenAboutMe;

            _gameScreen.TimeTickEvent += TimeTickEvent;

            _endGameWindow.ReplayEvent += StartGame;
            _endGameWindow.BackToMenuEvent += BackToMainMenu;

            _settingsWindow.BackToMenuEvent += BackToMainMenu;
            _settingsWindow.ApplyEvent += SetPlayerName;
            _settingsWindow.SetPlayerName(_playerName);

            _aboutMeWindow.BackToMenuEvent += BackToMainMenu;
        }
        
        private void BurstSpawnObject(float result)
        {
            _currentScore += result;
            _gameScreen.SetScore(_currentScore);           
        }

        private void BackToMainMenu()
        {
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _spawnMechanics.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(false);
        }

        private void StartGame()
        {
            StopAllCoroutines();
            _startWindow.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(true);
            _spawnMechanics.gameObject.SetActive(true);
            _endGameWindow.gameObject.SetActive(false);
            
            _spawnMechanics.Respawn();
            
            Time.timeScale = 1;
        }

        private void OpenSettings() 
        {
            _settingsWindow.SetPlayerName(_playerName);
            _startWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(true);
        }

        private void OpenAboutMe()
        {
            _startWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(true);
        }

        private void EndGame()
        {
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _spawnMechanics.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(true);
            
            _endGameWindow.SetScore(_currentScore, _playerName);
            
            Time.timeScale = 0;
            _currentTime = 0;
            _currentScore = 0;
        }

        private void TimeTickEvent()
        {
            _gameScreen.SetTime(_maxTime - _currentTime);
            _currentTime += 1;
            if (_currentTime > _maxTime)
            {
                EndGame();
            }
        }

        private void SetPlayerName(string playerName)
        {
            _playerName = playerName;
        }
    }
}
