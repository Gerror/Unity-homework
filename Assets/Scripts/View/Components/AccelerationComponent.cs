using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View.Components
{
    public class AccelerationComponent : MonoBehaviour
    {
        [SerializeField] private float _effectMultiply = 1.5f;
        [SerializeField] private float _effectLifeTime = 3f;

        private Collider _collider;
        private MeshRenderer _meshRenderer;

        public event Action TakeEffect;
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private IEnumerator Effect(PlayerController playerController)
        {
            playerController.Speed *= _effectMultiply;
            
            yield return new WaitForSeconds(_effectLifeTime);
            
            playerController.Speed /= _effectMultiply;
            
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                _collider.enabled = false;
                _meshRenderer.enabled = false;
                TakeEffect?.Invoke();
                StartCoroutine(Effect(playerController));
            }
        }
    }
}