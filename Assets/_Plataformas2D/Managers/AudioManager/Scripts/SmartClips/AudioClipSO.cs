using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipSO", menuName = "Audio/Audio Clip SO")]
public class AudioClipSO : ScriptableObject
{
    public AudioClip[] clips;

    [Header("Volume Settings")]
    [Range(0, 1)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;

    [Header("Randomization")]
    public bool randomizeVolume = false;
    [Range(0, 0.5f)] public float volumeVariation = 0.1f;
    public bool randomizePitch = false;
    [Range(0, 0.5f)] public float pitchVariation = 0.1f;

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
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioClipSO))]
public class AudioClipSettingsEditor : Editor
{
    private AudioSource previewSource;

    public override void OnInspectorGUI()
    {
        // Reference to AudioClipSO
        AudioClipSO settings = (AudioClipSO)target;

        // Display multiple audio clips
        SerializedProperty clipsProperty = serializedObject.FindProperty("clips");
        EditorGUILayout.PropertyField(clipsProperty, new GUIContent("Audio Clips"), true);

        // Volume and pitch settings
        settings.volume = EditorGUILayout.Slider("Volume", settings.volume, 0, 1);
        settings.pitch = EditorGUILayout.Slider("Pitch", settings.pitch, 0.5f, 2f);

        // Randomization settings
        EditorGUILayout.Space();
        settings.randomizeVolume = EditorGUILayout.Toggle("Randomize Volume", settings.randomizeVolume);
        if (settings.randomizeVolume)
            settings.volumeVariation = EditorGUILayout.Slider("Volume Variation", settings.volumeVariation, 0, 0.5f);

        settings.randomizePitch = EditorGUILayout.Toggle("Randomize Pitch", settings.randomizePitch);
        if (settings.randomizePitch)
            settings.pitchVariation = EditorGUILayout.Slider("Pitch Variation", settings.pitchVariation, 0, 0.5f);

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
        previewSource.clip = settings.GetRandomClip();
        previewSource.volume = settings.GetAdjustedVolume();
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