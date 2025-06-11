using System.Collections.Generic;
using UnityEngine;

namespace AudioManager
{
    public class GlobalAudioDicts : MonoBehaviourSingleton<GlobalAudioDicts>
    {
        [SerializeField, Expandable] private List<AudioDictionarySO> dicts;

        //FindFirst
        public AudioClipSO Has(string key)
        {
            foreach (AudioDictionarySO d in dicts)
            {
                AudioClipSO clipSO = d.GetClip(key);
                if (clipSO != null) return clipSO;
            }
            return null;
        }

        public void Play(string key)
        {
            AudioClipSO clipSO = Has(key);
            if (clipSO != null) clipSO.Play();
        }
    }
}