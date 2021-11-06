using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EndGameWindow : MonoBehaviour
    {
        [SerializeField] private Text _scoreLabel;
        public event Action ReplayEvent;
        public event Action BackToMenuEvent;

        public void SetScore(float score, string playerName, int time)
        {
            _scoreLabel.text = time + " сек. Ваш счет " + Math.Round(score, 1) + " очков!";
            if (playerName != "")
                _scoreLabel.text = playerName + ", за " + _scoreLabel.text;
            else
                _scoreLabel.text = "За " + _scoreLabel.text;
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
