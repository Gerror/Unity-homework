using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private ZombieMap _zombieMap;
        
        [SerializeField] private GameObject _winBlock;
        [SerializeField] private GameObject _gameOverBlock;

        private bool _isGameOver = false;
        private bool _playerWin = false;

        public bool IsGameOver
        {
            get
            {
                return _isGameOver;
            }
        }
        
        public bool PlayerWin
        {
            get
            {
                return _playerWin;
            }
        }
        
        private void Update()
        {
            if (!_zombieMap.AlivePositions().Any())
            {
                _winBlock.SetActive(true);
                _isGameOver = true;
                _playerWin = true;
                return;
            }

            if (_player.Hitpoints <= 0)
            {
                _gameOverBlock.SetActive(true);
                _isGameOver = true;
                _playerWin = false;
                return;
            }
            
            _player.Hitpoints -= Time.deltaTime;
        }
    }
}