using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "AudioClipSO", menuName = "AudioSO/Audio Clip")]
public class AudioClipSO : ScriptableObject
{
    public AudioCategory category;
    public AudioClip[] clips;

    [Header("Volume Settings")]
    [SerializeField,Range(0, 2f)] private float volume = 1f;
    [SerializeField] private bool randomizeVolume = false;
    [ConditionalHide("randomizeVolume")]
    [SerializeField, Min(0f)] private float volumeVariation = 0.1f;

    [Header("Pitch Settings")]
    [SerializeField, Range(0.5f, 2f)] private float pitch = 1f;
    [SerializeField] private bool randomizePitch = false;
    [ConditionalHide("randomizePitch")]
    [SerializeField, Min(0f)] private float pitchVariation = 0.1f;

    [Header("Fade (seconds)")]
    [SerializeField, Min(0f)] private float fadeIn = 0f;
    [SerializeField, Min(0f)] private float fadeOut = 0f;

    AudioSource playingAtSource;

    private void OnValidate()
    {
        volume = Mathf.Clamp(volume, 0f, 2f);
        volumeVariation = Mathf.Clamp(volumeVariation, 0f, 0.5f);
        pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        pitchVariation = Mathf.Clamp(pitchVariation, 0f, 0.5f);
        fadeIn = Mathf.Max(0f, fadeIn);
        fadeOut = Mathf.Max(0f, fadeOut);
    }

    #region Public Access
    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"AudioClipSO '{name}': No audio clips assigned.");
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }

    public float GetAdjustedVolume()
    {
        if (randomizeVolume)
        {
            float rand = Random.Range(-volumeVariation, volumeVariation);
            return Mathf.Clamp01(volume + rand);
        }
        return volume;
    }

    public float GetAdjustedPitch()
    {
        if (randomizePitch)
        {
            float rand = Random.Range(-pitchVariation, pitchVariation);
            float newPitch = pitch + rand;
            return Mathf.Clamp(newPitch, 0.1f, 3f);
        }
        return pitch;
    }

    //  Expose read‑only properties for runtime helpers
    public AudioCategory Category => category;
    public float FadeIn => fadeIn;
    public float FadeOut => fadeOut;
    #endregion

    #region Play methods using the AudioManager.
    //Fix me. Separar y poner esta lógica en una extensión.
    public void Play()
    {
        Play(default);
    }

    public void Play(Vector3 position)
    {
        if(!AudioManager.Instance)
        {
            Debug.LogWarning("No AudioManager in the scene.");
            return;
        }

        AudioClip clip = GetRandomClip();
        if (!clip) return;

        float adjustedVolume = GetAdjustedVolume();
        float adjustedPitch = GetAdjustedPitch();

        playingAtSource = AudioManager.Instance.Play(category, clip, adjustedVolume, adjustedPitch, fadeIn, fadeOut, position);
    }

    public void Pause()
    {
        if(playingAtSource) playingAtSource.Pause();
    }

    public void Resume()
    {
        if (playingAtSource) playingAtSource.UnPause();
    }

    public void Stop()
    {
        if (playingAtSource) playingAtSource.Stop();
        playingAtSource = null;
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioClipSO))]
public class AudioClipSettingsEditor : Editor
{
    private AudioSource previewSource;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Reference to AudioClipSO
        AudioClipSO settings = (AudioClipSO)target;

        // Play preview button
        EditorGUILayout.Space();
        if (GUILayout.Button("Play Random Clip"))
            PlayPreview(settings);
        if (GUILayout.Button("Stop"))
            StopPreview();

        // Apply changes
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
            EditorUtility.SetDirty(settings);
    }

    private void PlayPreview(AudioClipSO settings)
    {
        // Stop any existing audio
        if (previewSource != null && previewSource.isPlaying)
            previewSource.Stop();

        // Create an AudioSource if it doesn't exist
        if (previewSource == null)
        {
            GameObject previewGO = new GameObject("AudioClipPreview", typeof(AudioSource));
            previewSource = previewGO.GetComponent<AudioSource>();
            previewSource.hideFlags = HideFlags.HideAndDontSave;
        }

        // Set random clip, volume, and pitch
        previewSource.volume = settings.GetAdjustedVolume();
        previewSource.clip = settings.GetRandomClip();
        previewSource.pitch = settings.GetAdjustedPitch();
        previewSource.Play();
    }

    private void StopPreview()
    {
        // Stop the preview audio if it is playing
        if (previewSource != null && previewSource.isPlaying)
            previewSource.Stop();
    }

    private void OnDisable()
    {
        // Clean up the preview AudioSource
        if (previewSource != null)
            DestroyImmediate(previewSource.gameObject);
    }
}
#endif