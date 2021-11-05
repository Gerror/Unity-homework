using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.UI
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Text _scoreLabel;
        [SerializeField] private Text _timeLabel;
        
        public void SetScore(float score)
        {
            _scoreLabel.text = "Очки: " + Math.Round(score, 1);
        }

        public void SetTime(int time)
        {
            string minutes = (time / 60).ToString();
            string seconds = (time % 60).ToString();
            if ((time / 60) / 10 == 0)
                minutes = "0" + minutes;
            if ((time % 60) / 10 == 0)
                seconds = "0" + seconds;
            _timeLabel.text = "Время: " + minutes + ":" + seconds;
        }
    }
}