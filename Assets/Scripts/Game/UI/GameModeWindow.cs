using System;
using UnityEngine;

namespace Game.UI
{
    public class GameModeWindow : MonoBehaviour
    {
        public event Action PlayingForTimeEvent;
        public event Action PlayingForScoreEvent;
        public event Action BackToMenuEvent;

        public void PlayingForTime()
        {
            PlayingForTimeEvent?.Invoke();
        }

        public void PlayingForScore()
        {
            PlayingForScoreEvent?.Invoke();
        }

        public void BackToMenu()
        {
            BackToMenuEvent?.Invoke();
        }
    }
}