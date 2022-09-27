using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class SoundServer
{
    GameObject mPlayer;
    string mSoundFileRootPath;

    public void Create(string sound_file_root_path)
    {
     //   Debug.Log(" SoundPlayer +++++++++++++++++++++++++++");
        mSoundFileRootPath = sound_file_root_path;
        mPlayer = new GameObject("SoundPlayer");
        mPlayer.transform.parent = GameObject.Find("DontDestroyOnLoad").transform;
    }

    AudioClip LoadSound(string sound_name)
    {
        return Resources.Load<AudioClip>(mSoundFileRootPath + "/" + sound_name);
    }

    public AudioSource CreateSoundPlayer()
    {
        return mPlayer.AddComponent<AudioSource>();
    }

    public void Play(AudioSource sound_player, string sound_name, bool is_loop)
    {
        if (sound_player == null) return;    
        //sound_player.Stop();
        sound_player.loop = is_loop;
        sound_player.clip = LoadSound(sound_name);
        sound_player.Play();
    }

    public void Pause(AudioSource sound_player)
    {
        if (sound_player == null) return;
        sound_player.Pause();
    }

    public void Stop(AudioSource sound_player)
    {
        if (sound_player == null) return;
        sound_player.Stop();
    }

    public void DestroySoundPlayer(AudioSource sound_player)
    {
        if (sound_player == null) return;
        sound_player.Stop();
        UnityEngine.Object.Destroy(sound_player);
    }
}
