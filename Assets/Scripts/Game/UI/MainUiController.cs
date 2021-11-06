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
        [SerializeField] private GameModeWindow _gameModeWindow;
        [SerializeField] private GameModeMessage _gameModeMessage;

        [SerializeField] private AudioClip _clickAudio;
        
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        private TimeManager _timeManager;
        private GameSettings _gameSettings;
        private IGameManager _gameManager;

        private string _playerName = "";

        public event Action PlayingForTimeEvent;
        public event Action PlayingForScoreEvent;
        public event Action BackToMenuEvent;
        public event Action ReplayEvent;

        public IGameManager GameManager
        {
            set
            {
                _gameManager = value;
                _gameManager.EndGameEvent += EndGame;
                _gameManager.StartGameEvent += StartGame;
            }
        }

        [Inject]
        private void Construct(ScoreManager scoreManager, SoundManager soundManager, 
            TimeManager timeManager, GameSettings gameSettings)
        {
            _scoreManager = scoreManager;
            _soundManager = soundManager;
            _timeManager = timeManager;
            _gameSettings = gameSettings;
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
            if (!_gameModeWindow)
                _gameModeWindow = FindObjectOfType<GameModeWindow>();
            if (!_gameModeMessage)
                _gameModeMessage = FindObjectOfType<GameModeMessage>();
        }

        void Start()
        {
            WelcomeMessage();
            _settingsWindow.SetPlayerName(_playerName);

            _welcomeScreen.AnimationEndEvent += BackToMainMenu;
            _welcomeScreen.AnimationEndEvent += WelcomeMessageEnd;

            _startWindow.StartEvent += SelectGameMode;
            _startWindow.ExitEvent += ExitHelper.Exit;
            _startWindow.SettingsEvent += OpenSettings;
            _startWindow.AboutMeEvent += OpenAboutMe;
            
            _endGameWindow.ReplayEvent += Replay;
            _endGameWindow.BackToMenuEvent += BackToMainMenu;

            _settingsWindow.BackToMenuEvent += BackToMainMenu;
            _settingsWindow.ApplyEvent += SetPlayerName;

            _aboutMeWindow.BackToMenuEvent += BackToMainMenu;

            _gameModeWindow.BackToMenuEvent += BackToMainMenu;
            _gameModeWindow.PlayingForTimeEvent += PlayingForTime;
            _gameModeWindow.PlayingForScoreEvent += PlayingForScore;
            
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
            _gameModeWindow.gameObject.SetActive(false);
        }

        private void WelcomeMessageEnd()
        {
            _welcomeScreen.gameObject.SetActive(false);
        }

        private void SelectGameMode()
        {
            PlayClickAudio();
            _startWindow.gameObject.SetActive(false);
            _gameModeWindow.gameObject.SetActive(true);
        }
        
        private void StartGame()
        {
            PlayClickAudio();
            SetTime();
            _gameScreen.gameObject.SetActive(true);
            _endGameWindow.gameObject.SetActive(false);
            _gameModeWindow.gameObject.SetActive(false);
        }

        public void Replay()
        {
            PlayClickAudio();
            ReplayEvent?.Invoke();
        }

        private void EndGame()
        {
            _startWindow.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(true);

            _endGameWindow.SetScore(_scoreManager.Scores, _playerName, _gameManager.GetAllGameTime());
        }

        private void BackToMainMenu()
        {
            PlayClickAudio();
            _startWindow.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _endGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _aboutMeWindow.gameObject.SetActive(false);
            _gameModeWindow.gameObject.SetActive(false);
            
            BackToMenuEvent?.Invoke();
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
        
        private void PlayingForTime()
        {
            OpenGameModeMessage("Игра на время: " +
                                "За " + _gameSettings.MaxTime + " сек. нужно набрать как можно больше очков");
            _gameModeMessage.OkEvent += PlayingForTimeInvoke;
        }

        private void PlayingForTimeInvoke()
        {
            _gameModeMessage.gameObject.SetActive(false);
            PlayingForTimeEvent?.Invoke();
        }

        private void PlayingForScore()
        {
            OpenGameModeMessage("Игра на количество очков: " +
                                "Нужно набрать минимум " + _gameSettings.MaxScore + " очков как можно быстрее");
            _gameModeMessage.OkEvent += PlayingForScoreInvoke;
        }
        
        private void PlayingForScoreInvoke()
        {
            _gameModeMessage.gameObject.SetActive(false);
            PlayingForScoreEvent?.Invoke();
        }

        private void OpenGameModeMessage(string message)
        {
            _gameModeMessage.gameObject.SetActive(true);
            _gameModeWindow.gameObject.SetActive(false);
            
            _gameModeMessage.SetMessage(message);
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
            _gameScreen.SetTime(_gameManager.GetTime());
        }
        
        private void PlayClickAudio()
        {
            _soundManager.CreateSoundObject().Play(_clickAudio, transform.position, false, 0.25f);
        }
    }
}