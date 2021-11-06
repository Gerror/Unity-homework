using System;
using Game.Core;
using UnityEngine;

namespace Game.Core
{
    public interface IGameManager
    {
        public event Action EndGameEvent;
        public event Action StartGameEvent;

        public int GetTime();
        public int GetAllGameTime();
        public void StartGame();
    }
}
