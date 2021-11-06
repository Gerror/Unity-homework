using System;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class TimeManager : MonoBehaviour
    {
        [Min(15)] [SerializeField] private int _maxTime = 60;
        private bool _timerIsStart = false;
        private const int _timeTick = 1;

        public event Action ChangeEvent;
        public event Action EndTimeEvent;

        public int CurrentTime { get; private set; }

        public int MaxTime
        {
            get => _maxTime;
        }

        public void StartTimer()
        {
            if (_timerIsStart)
                return;

            StartCoroutine(Timer());
            
            _timerIsStart = true;
        }
        
        public void AddTime(int value)
        {
            CurrentTime += value;
            ChangeEvent?.Invoke();
            
            if (CurrentTime >= _maxTime)
            {
                EndTimeEvent?.Invoke();
                ResetTime();
            }
        }
        
        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeTick);
                AddTime(_timeTick);
            }
        }
        
        private void ResetTime()
        {
            CurrentTime = 0;
            _timerIsStart = false;
            StopAllCoroutines();
        }
    }
}