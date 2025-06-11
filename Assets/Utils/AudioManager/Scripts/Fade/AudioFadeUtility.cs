using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioManager
{
    public static class AudioFadeUtility
    {
        private static Dictionary<AudioSource, Coroutine> fadeCoroutines = new Dictionary<AudioSource, Coroutine>();


        #region Public Fade Methods
        public static void FadeIn(MonoBehaviour context, AudioSource source, float fadeTime, float targetVolume = 1f, System.Action onComplete = null)
        {
            StartFade(context, source, 0f, targetVolume, fadeTime, onComplete);
        }

        public static void FadeOut(MonoBehaviour context, AudioSource source, float fadeTime, System.Action onComplete = null)
        {
            StartFade(context, source, source.volume, 0f, fadeTime, onComplete);
        }

        public static void FadeTo(MonoBehaviour context, AudioSource source, float fadeTime, float targetVolume = 1f, System.Action onComplete = null)
        {
            StartFade(context, source, source.volume, targetVolume, fadeTime, onComplete);
        }
        #endregion

        #region Private Fade Methods
        private static void StartFade(MonoBehaviour context, AudioSource source, float fromVolume, float toVolume, float fadeTime, System.Action onComplete = null)
        {
            if (fadeCoroutines.ContainsKey(source))
            {
                context.StopCoroutine(fadeCoroutines[source]);
                fadeCoroutines.Remove(source);
            }

            Coroutine fadeCoroutine = context.StartCoroutine(FadeRoutine(source, fromVolume, toVolume, fadeTime, () =>
            {
                fadeCoroutines.Remove(source);
                onComplete?.Invoke();
            }));
            fadeCoroutines[source] = fadeCoroutine;
        }

        private static IEnumerator FadeRoutine(AudioSource source, float fromVolume, float toVolume, float fadeTime, System.Action onComplete)
        {
            float elapsed = 0f;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                source.volume = Mathf.Lerp(fromVolume, toVolume, elapsed / fadeTime);
                yield return null;
            }
            source.volume = toVolume;
            onComplete?.Invoke();
        }
        #endregion

    }
}