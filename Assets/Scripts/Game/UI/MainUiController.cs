using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Helpers;
using Game.Managers;

namespace Game.UI
{
    public class MainUiController : MonoBehaviour
    {
        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private EndGameWindow _endGameWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private AboutMeWindow _aboutMeWindow;
        
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private TimeManager _timeManager;

        private string _playerName = "";

        public event Action BackToMainMenuEvent;
        public event Action StartGameEvent;
        public event Action EndGameEvent;

        void OnValidate()
        {
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
            if (!_scoreManager)
                _scoreManager = FindObjectOfType<ScoreManager>();
            if (!_timeManager)
                _timeManager = FindObjectOfType<TimeManager>();
        }

        void Start()
        {
            BackToMainMenu();
            _settingsWindow.SetPlayerName(_playerName);

            _startWindow.StartEvent += StartGame;
            _startWindow.ExitEvent += ExitHelper.Exit;
            _startWindow.SettingsEvent += OpenSettings;
            _startWindow.AboutMeEvent += OpenAboutMe;
            
            _endGameWindow.ReplayEvent += StartGame;
            _endGameWindow.BackToMenuEvent += BackToMainMenu;

            _settingsWindow.BackToMenuEvent += BackToMainMenu;
            _settingsWindow.ApplyEvent += SetPlayerName;

            _aboutMeWindow.BackToMenuEvent += BackToMainMenu;
            
            _timeManager.EndTimeEvent += EndGame;
            _timeManager.ChangeEvent += SetTime;
            _scoreManager.ChangeEvent += SetScore;
        }

        private void StartGame()
        {
            SetTime();
            _startWindow.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(true);
            _endGameWindow.gameObject.SetActive(false);
            
            StartGameEvent?.Invoke();
        }

        private void EndGame()
        {
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(true);

            _endGameWindow.SetScore(_scoreManager.Scores, _playerName);
            
            EndGameEvent?.Invoke();
        }

        private void BackToMainMenu()
        {
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(false);
            
            BackToMainMenuEvent?.Invoke();
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

        private void SetPlayerName(string playerName)
        {
            _playerName = playerName;
        }

        private void SetScore()
        {
            _gameScreen.SetScore(_scoreManager.Scores);
        }
        
        private void SetTime()
        {
            _gameScreen.SetTime(_timeManager.MaxTime - _timeManager.CurrentTime);
        }
    }
}