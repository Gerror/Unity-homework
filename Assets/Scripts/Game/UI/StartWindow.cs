using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class StartWindow : MonoBehaviour
    {
        public event Action StartEvent;
        public event Action ExitEvent;
        public event Action AboutMeEvent;
        public event Action SettingsEvent;

        public void OnStart()
        {
            StartEvent?.Invoke();
        }

        public void OnExitGame()
        {
            ExitEvent?.Invoke();
        }

        public void OnSettings()
        {
            SettingsEvent?.Invoke();
        }

        public void OnAboutMe()
        {
            AboutMeEvent?.Invoke();
        }
    }
}
