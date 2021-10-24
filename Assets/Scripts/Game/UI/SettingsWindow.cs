using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI 
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private InputField _playerNameInputField;
        
        public event Action<string> ApplyEvent;
        public event Action BackToMenuEvent;

        public void OnApply()
        {
            ApplyEvent?.Invoke(_playerNameInputField.text);
        }

        public void OnBackToMenu()
        {
            BackToMenuEvent?.Invoke();
        }

        public void SetPlayerName(string playerName)
        {
            _playerNameInputField.text = playerName;
        }
    }
}