using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics
{
    public class ExplosionMechanics : MonoBehaviour
    {
        private ParticleSystem _explosionParticleSystem;

        private void Start()
        {
            _explosionParticleSystem = GetComponent<ParticleSystem>();
            float lifetime = _explosionParticleSystem.main.duration;
            StartCoroutine(Explose(lifetime));
        }

        private IEnumerator Explose(float time)
        {
            yield return new WaitForSeconds(time);
            
            Destroy(gameObject);
        }
    }
}