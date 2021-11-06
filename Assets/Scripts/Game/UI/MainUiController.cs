using System;
using UnityEngine;
using Game.Helpers;
using Game.Core;
using Game.Core.Sounds;
using Zenject;

namespace Game.UI
{
    public class MainUiController : MonoBehaviour
    {
        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private WelcomeScreen _welcomeScreen;
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private EndGameWindow _endGameWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private AboutMeWindow _aboutMeWindow;

        [SerializeField] private AudioClip _clickAudio;
        
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        private TimeManager _timeManager;

        private string _playerName = "";

        public event Action BackToMainMenuEvent;
        public event Action StartGameEvent;
        public event Action EndGameEvent;
        
        [Inject]
        private void Construct(ScoreManager scoreManager, SoundManager soundManager, TimeManager timeManager)
        {
            _scoreManager = scoreManager;
            _soundManager = soundManager;
            _timeManager = timeManager;
        }

        void OnValidate()
        {
            if (!_startWindow)
                _startWindow = FindObjectOfType<StartWindow>();
            if (!_gameScreen)
                _gameScreen = FindObjectOfType<GameScreen>();
            if (!_welcomeScreen)
                _welcomeScreen = FindObjectOfType<WelcomeScreen>();
            if (!_endGameWindow)
                _endGameWindow = FindObjectOfType<EndGameWindow>();
            if (!_settingsWindow)
                _settingsWindow = FindObjectOfType<SettingsWindow>();
            if (!_aboutMeWindow)
                _aboutMeWindow = FindObjectOfType<AboutMeWindow>();
        }

        void Start()
        {
            WelcomeMessage();
            _settingsWindow.SetPlayerName(_playerName);

            _welcomeScreen.AnimationEndEvent += BackToMainMenu;
            _welcomeScreen.AnimationEndEvent += WelcomeMessageEnd;

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

        private void WelcomeMessage()
        {
            _welcomeScreen.gameObject.SetActive(true);
            _startWindow.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(false);
        }

        private void WelcomeMessageEnd()
        {
            _welcomeScreen.gameObject.SetActive(false);
        }
        
        private void StartGame()
        {
            PlayClickAudio();
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
            PlayClickAudio();
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(false);
            
            BackToMainMenuEvent?.Invoke();
        }

        private void OpenSettings()
        {
            PlayClickAudio();
            _settingsWindow.SetPlayerName(_playerName);
            _startWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(true);
        }

        private void OpenAboutMe()
        {
            PlayClickAudio();
            _startWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(true);
        }

        private void SetPlayerName(string playerName)
        {
            PlayClickAudio();
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
        
        private void PlayClickAudio()
        {
            _soundManager.CreateSoundObject().Play(_clickAudio, transform.position, false, 0.25f);
        }
    }
}