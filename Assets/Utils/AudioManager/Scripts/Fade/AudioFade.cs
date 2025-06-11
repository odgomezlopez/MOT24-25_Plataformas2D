using UnityEngine;

namespace AudioManager
{

    /// <summary>
    /// Attach this component to a GameObject that has an AudioSource.
    /// It provides fade-in/out functionality on that AudioSource,
    /// </summary>
    /// 
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioFade : MonoBehaviour
    {
        [Header("Basic Fade Configuration")]
        [SerializeField] public bool fadeInOnStart = false;
        [ConditionalHide("fadeInOnStart")]
        [SerializeField, Min(0f)] public float defaultFadeInDuration = 1f;

        [SerializeField] public bool fadeOutOnEnd = false;
        [ConditionalHide("fadeOutnOnEnd")]
        [SerializeField, Min(0f)] public float defaultFadeOutDuration = 1f;

        [Header("Fade Target")]
        public float targetVolume = 1f;

        //Internal parameter
        private bool fading = false;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (!audioSource)
            {
                Debug.LogError("[AudioFade] No AudioSource found on this GameObject.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            fading = false;
            if (fadeInOnStart && defaultFadeInDuration > 0f && !fading)
            {
                fading = true;

                audioSource.volume = 0f;
                audioSource.Play();
                AudioFadeUtility.FadeIn(this, audioSource, defaultFadeInDuration, targetVolume, () => { fading = false; });
            }
        }

        private void Update()
        {
            if (fadeOutOnEnd && audioSource.isPlaying && !audioSource.loop && !fading)
            {
                float remainingTime = audioSource.clip.length - audioSource.time;
                if (remainingTime <= defaultFadeOutDuration)
                {
                    fading = true;
                    AudioFadeUtility.FadeOut(this, audioSource, defaultFadeOutDuration, () =>
                    {
                        fading = false;
                        audioSource.Stop();
                    });
                }
            }
        }

    }
}