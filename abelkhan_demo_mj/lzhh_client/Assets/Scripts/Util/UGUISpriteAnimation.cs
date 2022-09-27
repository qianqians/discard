using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour
{
	private Image ImageSource;
	private int mCurFrame = 0;
	private float mDelta = 0;

	public float FPS = 5;
	public List<Sprite> SpriteFrames;
	public bool IsPlaying = false;
	public bool Foward = true;
	public bool AutoPlay = false;
	public bool Loop = false;

    /// <summary>
    /// 一个动画的最大帧数
    /// </summary>
    private int maxMovieCount = 32;
    public string resPath;
    public string mcName;
    public string soundName;
    AudioSource mSoundPlayer;
    // public Action PlayOver;
    public int FrameCount
	{
		get
		{
			return SpriteFrames.Count;
		}
	}

	void Awake()
	{
		ImageSource = GetComponent<Image>();
	}

	void Start()
	{
        Sprite tempSpr;
        string path;
		if (AutoPlay)
		{
			Play();
		}
		else
		{
			IsPlaying = false;
		}
        if (SpriteFrames == null)
        {
            SpriteFrames = new List<Sprite>();          
        }
        for (int i = 0; i < maxMovieCount; i++)
        {
            path = resPath + mcName + "_" + i.ToString();
            tempSpr = Resources.Load(path, typeof(Sprite)) as Sprite;
            if (tempSpr == null)
            {
                break;
            }
            SpriteFrames.Add(tempSpr);         
        }

        if (soundName != "")
        {
            mSoundPlayer = this.gameObject.AddComponent<AudioSource>();
            mSoundPlayer.loop = true;
            mSoundPlayer.clip = LoadSound(soundName);
        }
    }

    private AudioClip LoadSound(string sound_name)
    {
        return Resources.Load<AudioClip>("movieClipSound/" + sound_name);
    }

    public void OnDestroy()
    {
        if (mSoundPlayer !=null)
        {
            mSoundPlayer.Stop();
            Destroy(mSoundPlayer);
        }
    }

	private void SetSprite(int idx)
	{
       // Sprite
		ImageSource.sprite = SpriteFrames[idx];
		ImageSource.SetNativeSize();
	}

	public void Play()
	{
		IsPlaying = true;
		Foward = true;
        if (mSoundPlayer!=null)
        {
            mSoundPlayer.Play();
        }
	}

	public void PlayReverse()
	{
		IsPlaying = true;
		Foward = false;
	}

	void Update()
	{
		if (!IsPlaying || 0 == FrameCount)
		{
			return;
		}

		mDelta += Time.deltaTime;
		if (mDelta > 1 / FPS)
		{
			mDelta = 0;
			if(Foward)
			{
				mCurFrame++;
			}
			else
			{
				mCurFrame--;
			}

			if (mCurFrame >= FrameCount)
			{
				if (Loop)
				{
					mCurFrame = 0;
				}
				else
				{
					IsPlaying = false;
                    Destroy(this.gameObject);
                    return;
				}
			}
			else if (mCurFrame<0)
			{
				if (Loop)
				{
					mCurFrame = FrameCount-1;
				}
				else
				{
					IsPlaying = false;
					return;
				}          
			}

			SetSprite(mCurFrame);
		}
	}

	public void Pause()
	{
		IsPlaying = false;
	}

	public void Resume()
	{
		if (!IsPlaying)
		{
			IsPlaying = true;
		}
	}

	public void Stop()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		IsPlaying = false;
	}

	public void Rewind()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		Play();
	}
}