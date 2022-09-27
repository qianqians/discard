using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class SoundManager
{
    SoundServer mSoundServer = new SoundServer();
    Dictionary<ESoundLayer, ISoundLayer> mSoundLayers = new Dictionary<ESoundLayer, ISoundLayer>();
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }

    public void Create()
    {
        mSoundServer.Create("sound");
        mSoundLayers.Add(ESoundLayer.Background, new SoundLayerBackground(mSoundServer));
        mSoundLayers.Add(ESoundLayer.Effect, new SoundLayerEffect(mSoundServer, 5));
        mSoundLayers.Add(ESoundLayer.EffectUI, new SoundLayerUI(mSoundServer, 3));
    }

    public void SetVolume(ESoundLayer layer, float volume)
    {
        mSoundLayers[layer].SetVolume(volume);
    }

    public void Play(ESoundLayer layer, string sound_name)
    {
        if (string.IsNullOrEmpty(sound_name))
        {
            return;
        }
        mSoundLayers[layer].Play(sound_name);
    }

    public void StopPointSound(ESoundLayer layer, string sound_name)
    {
        mSoundLayers[layer].Destroy();
    }

    public void Stop(ESoundLayer layer)
    {
        mSoundLayers[layer].Stop();
    }

    public void Destroy()
    {
        foreach (var sound_layer in mSoundLayers)
        {
            sound_layer.Value.Destroy();
        }
        mSoundLayers.Clear();
    }
}