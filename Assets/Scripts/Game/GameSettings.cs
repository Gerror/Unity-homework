using System;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private enum Difficalty
    {
        Easy,
        Average,
        Hard
    }

    [SerializeField] private Difficalty _difficalty;

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
        return _timeToThinkAI[(int) _difficalty];
    }

    public float GetZombieSpeed()
    {
        return _zombieSpeed[(int) _difficalty];
    }

    public float GetZombieDamage()
    {
        return _zombieDamage[(int) _difficalty];
    }

    public float GetZombieAttackTime()
    {
        return _zombieAttackTime[(int) _difficalty];
    }
}
