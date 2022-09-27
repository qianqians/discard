using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISoundLayer
{
    void Play(string sound_name);
    void Stop();
    void SetVolume(float volume);
    void Destroy();
}