using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace Game.Core
{
    public class PlayingForTime : MonoBehaviour, IGameManager
    {
        private TimeManager _timeManager;
        private GameSettings _gameSettings;
        
        public event Action EndGameEvent;
        public event Action StartGameEvent;

        [Inject]
        private void Construct(TimeManager timeManager, GameSettings gameSettings)
        {
            _timeManager = timeManager;
            _gameSettings = gameSettings;
        }
        
        public int GetAllGameTime()
        {
            return _gameSettings.MaxTime;
        }

        public int GetTime()
        {
            return _gameSettings.MaxTime - _timeManager.CurrentTime;
        }

        public void StartGame()
        {
            _timeManager.ChangeEvent += CheckTime;
            _timeManager.StartTimer();
            StartGameEvent?.Invoke();
        }

        private void EndGame()
        {
            EndGameEvent?.Invoke();
            _timeManager.StopTimer();
            _timeManager.ChangeEvent -= CheckTime;
        }

        private void CheckTime()
        {
            if (_timeManager.CurrentTime >= _gameSettings.MaxTime)
            {
                EndGame();
            }
        }
    }
}