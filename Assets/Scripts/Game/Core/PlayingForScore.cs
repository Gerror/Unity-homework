using System;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class PlayingForScore : MonoBehaviour, IGameManager
    {
        private ScoreManager _scoreManager;
        private TimeManager _timeManager;
        private GameSettings _gameSettings;
        
        public event Action EndGameEvent;
        public event Action StartGameEvent;
        
        [Inject]
        private void Construct(ScoreManager scoreManager, TimeManager timeManager, GameSettings gameSettings)
        {
            _timeManager = timeManager;
            _scoreManager = scoreManager;
            _gameSettings = gameSettings;
        }

        public int GetAllGameTime()
        {
            return GetTime();
        }

        public int GetTime()
        {
            return _timeManager.CurrentTime;
        }

        public void StartGame()
        {
            _scoreManager.ChangeEvent += CheckScore;
            _timeManager.StartTimer();
            StartGameEvent?.Invoke();
        }

        private void EndGame()
        {
            EndGameEvent?.Invoke();
            _timeManager.StopTimer();
            _scoreManager.ChangeEvent -= CheckScore;
        }

        private void CheckScore()
        {
            if (_scoreManager.Scores >= _gameSettings.MaxScore)
            {
                EndGame();
            }
        }
    }
}