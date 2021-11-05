using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
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