using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Enum que representa las categorías de audio.
/// </summary>
public enum AudioCategory
{
    Background,
    Music,
    Dialogue,
    SFX
}

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References

    [SerializeField] private VolumeSettings volumeSettings;    // Configuración de volúmenes
    [SerializeField] private AudioDictionary audioDictionary;  // Diccionario opcional para lookup por "key"

    [Header("Audio Sources (separados para cada categoría)")]
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;
    // Nota: Para SFX en 3D solemos usar PlaySoundAtPoint (no requiere AudioSource dedicado)

    #endregion


    #region Unity Lifecycle

    private void Start()
    {
        volumeSettings?.LoadVolumeSettings();

        // Aseguramos que cada AudioSource existe; si no, lo creamos
        SetupAudioSource(ref backgroundAudioSource, "BackgroundAudioSource");
        SetupAudioSource(ref musicAudioSource, "MusicAudioSource");
        SetupAudioSource(ref dialogueAudioSource, "DialogueAudioSource");
    }

    private void OnDestroy()
    {
        volumeSettings?.SaveVolumeSettings();
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Reproduce un AudioClipReference en la categoría indicada. 
    /// Si la referencia contiene un AudioClipSO, se aplica randomización.
    /// </summary>
    public void PlayAudio(AudioCategory category, AudioClipReference reference, Vector3 position = default)
    {
        if (reference == null) return;

        if (reference.HasAudioClipSO)
        {
            PlayAudio(category, reference.ClipSO, position);
        }
        else
        {
            // Reproducción directa de AudioClip
            if (!reference.Clip) return;
            PlayAudioInternal(category, reference.Clip, 1f, 1f, position);
        }
    }

    /// <summary>
    /// Reproduce un AudioClip directo en la categoría indicada.
    /// </summary>
    public void PlayAudio(AudioCategory category, AudioClip clip, Vector3 position = default)
    {
        if (clip == null) return;
        PlayAudioInternal(category, clip, 1f, 1f, position);
    }

    /// <summary>
    /// Reproduce un AudioClipSO directo en la categoría indicada.
    /// </summary>
    public void PlayAudio(AudioCategory category, AudioClipSO clipSO, Vector3 position = default)
    {
        if (clipSO == null) return;
        AudioClip clip = clipSO.GetRandomClip();
        if (!clip) return;

        float adjustedVolume = clipSO.GetAdjustedVolume();
        float adjustedPitch = clipSO.GetAdjustedPitch();
        PlayAudioInternal(category, clip, adjustedVolume, adjustedPitch, position);
    }

    /// <summary>
    /// Reproduce un clip (o AudioClipSO) buscado por "key" en el AudioDictionary, 
    /// dentro de la categoría dada.
    /// </summary>
    public void PlayAudio(AudioCategory category, string key, Vector3 position = default)
    {
        if (audioDictionary == null) return;

        AudioClipReference reference = GetClipReferenceByCategory(category, key);
        if (reference == null) return;

        PlayAudio(category, reference, position);
    }

    /// <summary>
    /// Método estático para reproducir un clip 3D "one-shot" 
    /// (no requiere un AudioSource permanente en escena).
    /// </summary>
    public static void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume, float pitch, AudioMixerGroup mixerGroup = null)
    {
        if (!clip) return;

        GameObject tempGO = new GameObject("TempAudio_OneShot");
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        if (mixerGroup != null)
            tempSource.outputAudioMixerGroup = mixerGroup;

        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.pitch = pitch;
        tempSource.Play();

        // Destruye el GO cuando termine el clip
        Object.Destroy(tempGO, clip.length / Mathf.Abs(pitch));
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Crear o asignar un AudioSource hijo en tiempo de ejecución, 
    /// si no existe ya.
    /// </summary>
    private void SetupAudioSource(ref AudioSource source, string sourceName)
    {
        if (source != null) return;

        Transform found = transform.Find(sourceName);
        if (!found)
        {
            GameObject go = new GameObject(sourceName);
            go.transform.SetParent(transform);
            source = go.AddComponent<AudioSource>();
        }
        else
        {
            source = found.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Lógica interna que decide cómo reproducir el clip según la categoría 
    /// (loop, 3D one-shot, etc.).
    /// </summary>
    private void PlayAudioInternal(AudioCategory category, AudioClip clip, float clipVolume, float clipPitch, Vector3 position)
    {
        if (!volumeSettings) return;

        VolumeControl volumeControl = GetVolumeControlByCategory(category);
        float finalVolume = volumeControl.Volume * clipVolume;  // Ajusta según VolSettings

        switch (category)
        {
            case AudioCategory.Background:
                // Se asume que el audio de fondo va en loop
                PlaySound(clip, backgroundAudioSource, volumeControl, true, finalVolume, clipPitch);
                break;

            case AudioCategory.Music:
                // Música en loop
                PlaySound(clip, musicAudioSource, volumeControl, true, finalVolume, clipPitch);
                break;

            case AudioCategory.Dialogue:
                // Diálogo usualmente no va en loop
                PlaySound(clip, dialogueAudioSource, volumeControl, false, finalVolume, clipPitch);
                break;

            case AudioCategory.SFX:
                // SFX en 3D a la posición dada
                Vector3 pos = position == default ? Camera.main.transform.position : position;
                PlaySoundAtPoint(clip, pos, finalVolume, clipPitch, volumeControl.Group);
                break;
        }
    }

    /// <summary>
    /// Asigna al AudioSource el clip y parámetros para reproducir en loop o no.
    /// </summary>
    private void PlaySound(AudioClip clip, AudioSource source, VolumeControl volumeControl, bool loop, float overrideVolume, float pitch)
    {
        if (!clip || !source) return;

        source.clip = clip;
        source.outputAudioMixerGroup = volumeControl.Group;
        source.volume = overrideVolume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();
    }
    #endregion

    #region AudioCategories Helpers 
    /// <summary>
    /// Devuelve el VolumeControl adecuado según la categoría.
    /// </summary>
    private VolumeControl GetVolumeControlByCategory(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.Background: return volumeSettings.background;
            case AudioCategory.Music: return volumeSettings.music;
            case AudioCategory.Dialogue: return volumeSettings.dialogue;
            case AudioCategory.SFX: return volumeSettings.sfx;
            default: return volumeSettings.master; // fallback
        }
    }

    /// <summary>
    /// Recupera el AudioClipReference desde el AudioDictionary para la categoría y key dada.
    /// </summary>
    private AudioClipReference GetClipReferenceByCategory(AudioCategory category, string key)
    {
        switch (category)
        {
            case AudioCategory.Background: return audioDictionary.GetBackgroundClipReference(key);
            case AudioCategory.Music: return audioDictionary.GetMusicClipReference(key);
            case AudioCategory.Dialogue: return audioDictionary.GetDialogueClipReference(key);
            case AudioCategory.SFX: return audioDictionary.GetSfxClipReference(key);
            default: return null;
        }
    }

    #endregion
}
