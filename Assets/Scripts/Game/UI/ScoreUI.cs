using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text _text; 
        private SpawnMechanics _spawnMechanics;

        private float _currentScore;

        void Start()
        {
            _currentScore = 0;
            _spawnMechanics = FindObjectOfType<SpawnMechanics>();
            _spawnMechanics.BurstSpawnObjectEvent += BurstSpawnObject;
            
            SetText();
        }

        private void BurstSpawnObject(float result)
        {
            _currentScore += result;
            SetText();           
        }

        private void SetText()
        {
            _text.text = "SCORE: " + _currentScore;
        }
    }
}