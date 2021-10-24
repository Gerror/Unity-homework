using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private Text _scoreLabel;
        public event Action ReplayEvent;
        public event Action BackToMenuEvent;

        public void SetScore(float score, string playerName)
        {
            _scoreLabel.text = "Ваш счет: " + Math.Round(score, 1);
            if (playerName != "")
                _scoreLabel.text = playerName + ", " + _scoreLabel.text;
        }
        
        public void OnReplay()
        {
            ReplayEvent?.Invoke();
        }

        public void OnBackToMenu()
        {
            BackToMenuEvent?.Invoke();
        }
    }
}
