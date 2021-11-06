using System;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class TimeManager : MonoBehaviour
    {
        private bool _timerIsStart = false;
        private const int _timeTick = 1;

        public event Action ChangeEvent;

        public int CurrentTime { get; private set; }

        public void StartTimer()
        {
            if (_timerIsStart)
                return;

            StartCoroutine(Timer());
            
            _timerIsStart = true;
        }
        
        public void StopTimer()
        {
            StopAllCoroutines();
            _timerIsStart = false;
            CurrentTime = 0;
        }
        
        private void AddTime(int value)
        {
            CurrentTime += value;
            ChangeEvent?.Invoke();
        }
        
        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeTick);
                AddTime(_timeTick);
            }
        }
    }
}