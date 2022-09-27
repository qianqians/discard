using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class SoundLayerEffect : ISoundLayer
{
    SoundServer mSoundServer;
    Queue<AudioSource> mSoundPlayers = new Queue<AudioSource>();

    public SoundLayerEffect(SoundServer sound_server, int track_number)
    {
        mSoundServer = sound_server;
        mSoundPlayers = new Queue<AudioSource>(track_number);
        while (track_number > 0)
        {
            mSoundPlayers.Enqueue(mSoundServer.CreateSoundPlayer());
            track_number--;
        }
    }

    public void Play(string sound_name)
    {
        AudioSource current_player = mSoundPlayers.Dequeue();
        mSoundServer.Play(current_player, sound_name, false);
        mSoundPlayers.Enqueue(current_player);
    }

    public void Stop()
    {
        foreach (var sound_player in mSoundPlayers)
        {
            mSoundServer.Stop(sound_player);
        }
    }

    public void SetVolume(float volume)
    {
        foreach (var sound_player in mSoundPlayers)
        {
            sound_player.volume = volume;
        }
    }

    public void Destroy()
    {
        foreach (var sound_player in mSoundPlayers)
        {
            mSoundServer.DestroySoundPlayer(sound_player);
        }
    }
}
