using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

/// <summary>
/// Lightweight expandable pool that hands out reusable <see cref="AudioSource"/> components.
/// </summary>
public class AudioSourcePool
{
    private readonly List<AudioSource> _pool = new();
    private readonly AudioManager _audioManager;
    private readonly AudioGroupManager _audioGroupManager;


    private int _counter;


    public AudioSourcePool(AudioManager audioManager, AudioGroupManager audioGroupManager)
    {
        _audioManager = audioManager;
        _audioGroupManager = audioGroupManager;

        BuildInitialPool(Mathf.Max(1, _audioGroupManager.SourcePoolInitSize));
    }


    public AudioSource GetFirst()
    {
        return _pool[0];
    }
    /// <summary>Fetches an idle source or creates a new one if all are busy.</summary>
    public AudioSource GetFirstAvailable()
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
        string srcName = _audioGroupManager.NamePrefix;
        if (_audioGroupManager.Type == AudioType.MultipleSource) srcName += _counter++;

        AudioSource src;

        //Comprobamos si ya existe la pista de audio
        Transform found = _audioManager.transform.Find(srcName);
        if (!found)
        {
            GameObject go = new(srcName);
            go.transform.SetParent(_audioManager.transform);
            src = go.AddComponent<AudioSource>();
        }
        else
        {
            src = found.GetComponent<AudioSource>();
        }

        PrepareSource(src);
        return src;
    }

    private void PrepareSource(AudioSource src)
    {
        //Default values
        src.playOnAwake = false;
        src.spatialBlend = 0f; // TODO. Change in a 3D game
        src.outputAudioMixerGroup = _audioGroupManager.MixerGroup;
        src.loop = _audioGroupManager.Loop;   // Loop behaviour determined per‑use.

        src.volume = 1f;
        src.pitch = 1f;

        //Set the audioBlend
        src.spatialBlend = (_audioGroupManager.Mode == AudioMode.Audio2D) ? 0f : 1f;
    }
}