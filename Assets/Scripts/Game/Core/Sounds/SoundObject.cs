using UnityEngine;

namespace Game.Core.Sounds
{
    public class SoundObject : MonoBehaviour
    {
        private AudioSource _source;
        
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }
        
        public void Play(AudioClip clip, Vector3 position, bool loop = false, float volume = 1f)
        {
            transform.position = position;

            Play(clip, loop, volume);
        }

        public void Play(AudioClip clip, bool loop = false, float volume = 1f)
        {
            _source.clip = clip;
            _source.volume = volume;
            _source.loop = loop;

            _source.Play();
        }
        
        private void Update()
        {
            if (!_source.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
}