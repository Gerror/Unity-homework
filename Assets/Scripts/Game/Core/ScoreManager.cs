using System;
using UnityEngine;

namespace Game.Core
{
    public class ScoreManager : MonoBehaviour
    {
        public event Action ChangeEvent;

        public float Scores { get; private set; }

        public void ResetScore()
        {
            Scores = 0f;
            ChangeEvent?.Invoke();
        }
            
        public void AddScore(float value)
        {
            Scores += value;
            ChangeEvent?.Invoke();
        }
    }
}