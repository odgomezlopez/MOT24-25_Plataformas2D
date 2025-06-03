#region ───────────── AudioSourcePool ─────────────
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Lightweight expandable pool that hands out reusable <see cref="AudioSource"/> components.
/// </summary>
public class AudioSourcePool
{
    private readonly List<AudioSource> _pool = new();
    private readonly AudioManager _audioManager;
    private readonly AudioMixerGroup _mixerGroup;
    private readonly string _namePrefix;
    private readonly bool _loop;


    private int _counter;


    public AudioSourcePool(AudioManager audioManager, AudioMixerGroup mixerGroup,
                           string namePrefix, bool loop, int initialSize = 6)
    {
        _audioManager = audioManager;
        _mixerGroup = mixerGroup;
        _namePrefix = namePrefix;
        _loop = loop;

        BuildInitialPool(Mathf.Max(1, initialSize));
    }


    public AudioSource GetFirst()
    {
        return _pool[0];
    }
    /// <summary>Fetches an idle source or creates a new one if all are busy.</summary>
    public AudioSource GetAvailable()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            AudioSource src = _pool[i];
            if (!src.isPlaying) return src;
        }

        // None available – expand pool lazily.
        AudioSource newSrc = CreateNewSource();
        _pool.Add(newSrc);
        return newSrc;
    }

    /// <summary>Returns all sources currently playing.</summary>
    public List<AudioSource> ActiveSources => _pool.Where(s => s.isPlaying).ToList();

    public List<AudioSource> AllSources => _pool.ToList();


    /// <summary>Resets a source so it is ready for reuse.</summary>
    public void Release(AudioSource src)
    {
        if (src == null) return;
        src.Stop();
        src.clip = null;
        src.volume = 1f;
        src.pitch = 1f;
    }

    //─────────────────────────────────────────────────────────────────────────
    private void BuildInitialPool(int size)
    {
        for (int i = 0; i < size; i++)
            _pool.Add(CreateNewSource());
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = new($"{_namePrefix}_{_counter++}");
        go.transform.SetParent(_audioManager.transform);
        AudioSource src = go.AddComponent<AudioSource>();

        PrepareSource(src);
        return src;
    }

    private void PrepareSource(AudioSource src)
    {
        //Default values
        src.playOnAwake = false;
        src.spatialBlend = 0f; // TODO. Change in a 3D game
        src.outputAudioMixerGroup = _mixerGroup;
        src.loop = _loop;   // Loop behaviour determined per‑use.
        src.volume = 1f;
        src.pitch = 1f;
    }

    //private void SetupAudioSource(ref AudioSource source, string sourceName, AudioMixerGroup group)
    //{
    //    if (source != null) return;

    //    Transform found = audioManager.transform.Find(sourceName);
    //    if (!found)
    //    {
    //        GameObject go = new GameObject(sourceName);
    //        go.transform.SetParent(audioManager.transform);
    //        source = go.AddComponent<AudioSource>();
    //    }
    //    else
    //    {
    //        source = found.GetComponent<AudioSource>();
    //    }

    //    source.spatialBlend = 0f;
    //    source.playOnAwake = false;
    //    source.outputAudioMixerGroup = group;
    //    source.loop = loop;
    //}
}
#endregion