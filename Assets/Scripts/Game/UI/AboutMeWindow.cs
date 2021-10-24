using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class AboutMeWindow : MonoBehaviour
    {
        public event Action BackToMenuEvent;

        public void OnBackToMenu()
        {
            BackToMenuEvent?.Invoke();
        }
    }
}