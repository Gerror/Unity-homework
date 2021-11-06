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
        
        [SerializeField] private GameObject _playingForTimePrefab;
        [SerializeField] private GameObject _playingForScorePrefab;

        [SerializeField] private AudioClip _backgroundAudio;

        private PrefabFactory _prefabFactory;
        private ScoreManager _scoreManager;
        private SoundManager _soundManager;
        private IGameManager _gameManager;
        private GameObject _gameManagerGo;

        [Inject]
        private void Construct(ScoreManager scoreManager, SoundManager soundManager, PrefabFactory prefabFactory)
        {
            _scoreManager = scoreManager;
            _soundManager = soundManager;
            _prefabFactory = prefabFactory;
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
            _mainUiController.PlayingForTimeEvent += PlayingForTimeMode;
            _mainUiController.PlayingForScoreEvent += PlayingForScoreMode;
            _mainUiController.BackToMenuEvent += DestroyGameManager;
            _mainUiController.ReplayEvent += StartGame;

            _soundManager.CreateSoundObject().Play(_backgroundAudio, transform.position, true, 0.1f);    
        }

        private void PlayingForTimeMode()
        {
            PlayingMode(_playingForTimePrefab);
        }

        private void PlayingForScoreMode()
        {
            PlayingMode(_playingForScorePrefab);
        }

        private void PlayingMode(GameObject playingModePrefab)
        {
            _gameManagerGo = _prefabFactory.Spawn(playingModePrefab, 
                Vector3.zero, Quaternion.identity, null);
            SetGameManager();
            StartGame();
        }

        private void SetGameManager()
        {
            _gameManager = _gameManagerGo.GetComponent<IGameManager>();

            _mainUiController.GameManager = _gameManager;
            
            _gameManager.EndGameEvent += EndGame;
        }
        
        private void DestroyGameManager()
        {
            Destroy(_gameManagerGo);
        }

        private void StartGame()
        {
            _spawnMechanics.gameObject.SetActive(true);
            _spawnMechanics.Respawn();
            _gameManager.StartGame();
            Time.timeScale = 1;
        }

        private void EndGame()
        {
            _spawnMechanics.gameObject.SetActive(false);

            Time.timeScale = 0;
            _scoreManager.ResetScore();
        }
    }
}
