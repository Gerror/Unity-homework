using System;
using UnityEngine;

namespace Game.UI
{
    public class WelcomeScreen : MonoBehaviour
    {
        public event Action AnimationEndEvent;
        
        private void AnimationEnd()
        {
            AnimationEndEvent?.Invoke();
        }
    }
}