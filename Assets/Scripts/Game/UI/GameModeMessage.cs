using UnityEngine;
using System;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameModeMessage : MonoBehaviour
    {
        [SerializeField] private Text _message;

        public event Action OkEvent;
        
        public void SetMessage(string message)
        {
            _message.text = message;
        }

        public void Ok()
        {
            OkEvent?.Invoke();
            OkEvent = null;
        }
    }
}