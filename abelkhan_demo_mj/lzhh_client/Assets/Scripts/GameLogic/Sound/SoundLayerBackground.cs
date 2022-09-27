using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class SoundLayerBackground : ISoundLayer
{
    SoundServer mSoundServer;
    AudioSource mSoundPlayer;

    public SoundLayerBackground(SoundServer sound_server)
    {
        mSoundServer = sound_server;
        mSoundPlayer = mSoundServer.CreateSoundPlayer();
    }

    public void Play(string sound_name)
    { 
        if (IsSameSound(sound_name)) return;
        mSoundServer.Play(mSoundPlayer, sound_name, true);
    }

    public void Stop()
    {
        mSoundServer.Stop(mSoundPlayer);
    }

    public void SetVolume(float volume)
    {
        mSoundPlayer.volume = volume;
    }

    public void Destroy()
    {
        mSoundServer.DestroySoundPlayer(mSoundPlayer);
    }

    bool IsSameSound(string sound_name)
    {
        return mSoundPlayer.clip != null && mSoundPlayer.clip.name == sound_name;
    }
}