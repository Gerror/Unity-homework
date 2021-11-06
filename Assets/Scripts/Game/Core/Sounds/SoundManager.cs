using UnityEngine;

namespace Game.Core.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public SoundObject SoundObjectPrefab;
        
        public SoundObject CreateSoundObject()
        {
            var go = Instantiate(SoundObjectPrefab);
            return go.GetComponent<SoundObject>();
        }
    }
}