using Game.UI;
using Game.Core;
using Game.Core.Sounds;
using UnityEngine;
using Zenject;

namespace Game.Mechanics
{
    public class MainMechanics : MonoBehaviour
    {
        [SerializeField] private MainUiController _mainUiController;
        [SerializeField] private SpawnMechanics _spawnMechanics;

        [SerializeField] private AudioClip _backgroundAudio;
        
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        private TimeManager _timeManager;

        [Inject]
        private void Construct(ScoreManager scoreManager, SoundManager soundManager, TimeManager timeManager)
        {
            _scoreManager = scoreManager;
            _soundManager = soundManager;
            _timeManager = timeManager;
        }
        
        void OnValidate()
        {
            if (!_spawnMechanics)
                _spawnMechanics = FindObjectOfType<SpawnMechanics>();
            if (!_mainUiController)
                _mainUiController = FindObjectOfType<MainUiController>();
        }

        void Start()
        {
            _timeManager.EndTimeEvent += EndGame;

            _mainUiController.BackToMainMenuEvent += DeactivateSpawnMechanics;
            _mainUiController.EndGameEvent += EndGame;
            _mainUiController.StartGameEvent += StartGame;
            
            _soundManager.CreateSoundObject().Play(_backgroundAudio, transform.position, true, 0.1f);    
        }

        private void DeactivateSpawnMechanics()
        {
            _spawnMechanics.gameObject.SetActive(false);
        }

        private void StartGame()
        {
            _spawnMechanics.gameObject.SetActive(true);
            _spawnMechanics.Respawn();
            Time.timeScale = 1;
            _timeManager.StartTimer();
        }

        private void EndGame()
        {
            _spawnMechanics.gameObject.SetActive(false);

            Time.timeScale = 0;
            _scoreManager.ResetScore();
        }
    }
}
