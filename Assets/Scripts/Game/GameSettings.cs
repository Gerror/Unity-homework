using System;
using UnityEngine;

namespace Game
{
    public enum Difficalty
    {
        Easy,
        Average,
        Hard
    }

    public class GameSettings : MonoBehaviour
    {

        public Difficalty Difficalty;

        [SerializeField] private float[] _timeToThinkAI;
        [SerializeField] private float[] _zombieSpeed;
        [SerializeField] private float[] _zombieDamage;
        [SerializeField] private float[] _zombieAttackTime;

        private void OnValidate()
        {
            ValidateFloatArray(ref _timeToThinkAI);
            ValidateFloatArray(ref _zombieSpeed);
            ValidateFloatArray(ref _zombieDamage);
            ValidateFloatArray(ref _zombieAttackTime);
        }

        private void ValidateFloatArray(ref float[] floatArray)
        {
            if (floatArray.Length != Enum.GetNames(typeof(Difficalty)).Length)
                floatArray = new float[Enum.GetNames(typeof(Difficalty)).Length];
        }

        public float GetTimeToThinkAI()
        {
            return _timeToThinkAI[(int) Difficalty];
        }

        public float GetZombieSpeed()
        {
            return _zombieSpeed[(int) Difficalty];
        }

        public float GetZombieDamage()
        {
            return _zombieDamage[(int) Difficalty];
        }

        public float GetZombieAttackTime()
        {
            return _zombieAttackTime[(int) Difficalty];
        }
    }
}
