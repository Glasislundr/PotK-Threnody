// Decompiled with JetBrains decompiler
// Type: CRISoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CRISoundManager : Singleton<CRISoundManager>
{
  public int maxBGM = 3;
  public int maxSE = 3;
  public int maxVoice = 3;
  private float curAisac;
  public CRISoundManager.SoundVolume volume;
  private int mainBGMChannel;
  private CriAtomSource[] bgmSource;
  private CriAtomSource[] seSources;
  private CriAtomSource[] voiceSources;
  private GameObject audioObject;
  private string bgmCueSheetName = "BgmCueSheet";
  private string seCueSheetName = "SECueSheet";
  private string voiceCueSheetName = "VO_1001";
  private int reserveVoiceAudioChannel = -1;

  protected override void Initialize()
  {
    this.bgmSource = new CriAtomSource[this.maxBGM];
    this.seSources = new CriAtomSource[this.maxSE];
    this.voiceSources = new CriAtomSource[this.maxVoice];
    this.createAudioObject();
  }

  private void createAudioObject()
  {
    CriAtom.AddCueSheet("SECueSheet", "SECueSheet.acb", "", (CriFsBinder) null);
    CriAtom.AddCueSheet("VO_1001", "VO_1001.acb", "", (CriFsBinder) null);
    CriAtom.AddCueSheet("BgmCueSheet", "BgmCueSheet.acb", "", (CriFsBinder) null);
    this.audioObject = new GameObject("AudioObject");
    this.audioObject.transform.position = ((Component) this).transform.position;
    this.audioObject.transform.parent = ((Component) this).transform;
    for (int index = 0; index < this.bgmSource.Length; ++index)
    {
      this.bgmSource[index] = this.audioObject.AddComponent<CriAtomSource>();
      this.bgmSource[index].cueSheet = this.bgmCueSheetName;
    }
    for (int index = 0; index < this.seSources.Length; ++index)
    {
      this.seSources[index] = this.audioObject.AddComponent<CriAtomSource>();
      this.seSources[index].loop = false;
      this.seSources[index].cueSheet = this.seCueSheetName;
    }
    for (int index = 0; index < this.voiceSources.Length; ++index)
    {
      this.voiceSources[index] = this.audioObject.AddComponent<CriAtomSource>();
      this.voiceSources[index].loop = false;
      this.voiceSources[index].cueSheet = this.voiceCueSheetName;
    }
  }

  private void Update()
  {
    if (!this.volume.modified)
      return;
    CriAtomExCategory.Mute("BGM", this.volume.mute);
    CriAtomExCategory.SetVolume("BGM", this.volume.bgmVolume);
    CriAtomExCategory.Mute("SE", this.volume.mute);
    CriAtomExCategory.SetVolume("SE", this.volume.seVolume);
    CriAtomExCategory.Mute("VOICE", this.volume.mute);
    CriAtomExCategory.SetVolume("VOICE", this.volume.voiceVolume);
    NGUITools.soundVolume = this.volume.mute ? 0.0f : this.volume.seVolume;
    this.volume.modified = false;
  }

  public CriAtomSource getMainBGMAudioSource() => this.bgmSource[this.mainBGMChannel];

  public CriAtomSource getPrevBGMAudioSource()
  {
    int index = this.mainBGMChannel - 1;
    if (index < 0)
      index = this.maxBGM - 1;
    return this.bgmSource[index];
  }

  public CriAtomSource getNextBGMAudioSource()
  {
    return this.bgmSource[(this.mainBGMChannel + 1) % this.maxBGM];
  }

  public void playBGM(string clip, float startTime = 0.1f)
  {
    if (clip == this.bgmSource[this.mainBGMChannel].cueName)
      return;
    CriAtomSource audio = this.bgmSource[this.mainBGMChannel];
    audio.cueName = clip;
    audio.startTime = 0;
    float delay = startTime;
    this.StartCoroutine(this.delayPlay(audio, delay));
  }

  private IEnumerator Fade(CriAtomSource audio, float v0, float v1, float duration, float delay = 0.0f)
  {
    if ((double) delay != 0.0)
    {
      audio.SetAisacControl(0U, this.curAisac);
      yield return (object) new WaitForSeconds(delay);
    }
    for (float currentTime = 0.0f; (double) duration > (double) currentTime; currentTime += Time.deltaTime)
    {
      audio.volume = Mathf.Lerp(v0, v1, currentTime / duration);
      yield return (object) null;
    }
    if ((double) v1 == 0.0)
    {
      audio.Stop();
      audio.cueName = (string) null;
    }
  }

  private IEnumerator BgmCrossFade(
    CriAtomSource audio,
    float v0,
    float v1,
    float duration,
    float delay = 0.0f)
  {
    if ((double) delay != 0.0)
      yield return (object) new WaitForSeconds(delay);
    for (float currentTime = 0.0f; (double) duration > (double) currentTime; currentTime += Time.deltaTime)
    {
      audio.SetAisacControl(0U, Mathf.Lerp(v0, v1, currentTime / duration));
      yield return (object) null;
    }
  }

  public void corssFadeCurrentBGM(float duration, float startTime)
  {
    CriAtomSource audio = this.bgmSource[this.mainBGMChannel];
    float curAisac = this.curAisac;
    this.curAisac = (double) curAisac != 0.0 ? 0.0f : 1f;
    this.StartCoroutine(this.BgmCrossFade(audio, curAisac, this.curAisac, duration, startTime));
  }

  public void playBGMWithCrossFade(AudioClip clip, float duration, float startTime = 0.1f)
  {
    if (((Object) clip).name == this.bgmSource[this.mainBGMChannel].cueName)
      return;
    float delay = startTime;
    CriAtomSource audio1 = this.bgmSource[this.mainBGMChannel];
    if (this.bgmSource[this.mainBGMChannel].status == 2)
    {
      this.StartCoroutine(this.Fade(audio1, audio1.volume, 0.0f, duration, delay));
      this.mainBGMChannel = (this.mainBGMChannel + 1) % this.maxBGM;
    }
    CriAtomSource audio2 = this.bgmSource[this.mainBGMChannel];
    audio2.volume = 0.0f;
    audio2.cueName = ((Object) clip).name;
    audio2.startTime = 0;
    audio2.Play();
    this.curAisac = 0.0f;
    audio2.SetAisacControl(0U, this.curAisac);
    this.StartCoroutine(this.Fade(audio2, audio2.volume, this.volume.bgmVolume, duration, delay));
  }

  public void playBGMWithCrossFade(string clip, float duration, float startTime = 0.1f)
  {
    if (clip == this.bgmSource[this.mainBGMChannel].cueName)
      return;
    float delay = startTime;
    CriAtomSource audio1 = this.bgmSource[this.mainBGMChannel];
    if (this.bgmSource[this.mainBGMChannel].status == 2)
    {
      this.StartCoroutine(this.Fade(audio1, audio1.volume, 0.0f, duration, delay));
      this.mainBGMChannel = (this.mainBGMChannel + 1) % this.maxBGM;
    }
    CriAtomSource audio2 = this.bgmSource[this.mainBGMChannel];
    audio2.volume = 0.0f;
    audio2.cueName = clip;
    audio2.startTime = 0;
    this.StartCoroutine(this.delayPlay(audio2, delay));
    this.StartCoroutine(this.Fade(audio2, audio2.volume, this.volume.bgmVolume, duration, delay));
  }

  public void playBGMWithCombineTimeFade(AudioClip clip, float duration, float startTime = 0.1f)
  {
    if (((Object) clip).name == this.bgmSource[this.mainBGMChannel].cueName)
      return;
    float delay = startTime;
    long time = this.bgmSource[this.mainBGMChannel].time;
    Debug.Log((object) (" === CRISoundManager# playBGMWithCombineTimeFade current time = " + time.ToString()));
    Debug.Log((object) (" === CRISoundManager# playBGMWithCombineTimeFade dspStartTime = " + (AudioSettings.dspTime + (double) delay).ToString()));
    CriAtomSource audio1 = this.bgmSource[this.mainBGMChannel];
    if (audio1.status == 2)
    {
      this.StartCoroutine(this.Fade(audio1, audio1.volume, 0.0f, duration, delay));
      this.mainBGMChannel = (this.mainBGMChannel + 1) % this.maxBGM;
    }
    CriAtomSource audio2 = this.bgmSource[this.mainBGMChannel];
    audio2.cueName = ((Object) clip).name;
    audio2.startTime = (int) time;
    audio2.volume = 0.0f;
    audio2.Play();
    this.StartCoroutine(this.Fade(audio2, audio2.volume, this.volume.bgmVolume, duration, delay));
  }

  public void playBGMWithCombineTimeFade(string clip, float duration, float startTime = 0.1f)
  {
    if (clip == this.bgmSource[this.mainBGMChannel].cueName)
      return;
    float delay = startTime;
    long time = this.bgmSource[this.mainBGMChannel].time;
    Debug.Log((object) (" === CRISoundManager# playBGMWithCombineTimeFade(str) current time = " + time.ToString()));
    Debug.Log((object) (" === CRISoundManager# playBGMWithCombineTimeFade(str) dspStartTime = " + (AudioSettings.dspTime + (double) delay).ToString()));
    CriAtomSource audio1 = this.bgmSource[this.mainBGMChannel];
    if (audio1.status == 2)
    {
      this.StartCoroutine(this.Fade(audio1, audio1.volume, 0.0f, duration, delay));
      this.mainBGMChannel = (this.mainBGMChannel + 1) % this.maxBGM;
    }
    CriAtomSource audio2 = this.bgmSource[this.mainBGMChannel];
    audio2.cueName = clip;
    audio2.startTime = (int) time;
    audio2.volume = 0.0f;
    CriAtomExPlayback criAtomExPlayback = audio2.Play();
    CriAtomEx.FormatInfo formatInfo;
    ((CriAtomExPlayback) ref criAtomExPlayback).GetFormatInfo(ref formatInfo);
    Debug.Log((object) (" === CRISoundManager# playBGMWithCombineTimeFade(str) loopOffset = " + (object) formatInfo.loopOffset + " len = " + (object) formatInfo.loopLength));
    this.bgmSource[this.mainBGMChannel].SetAisacControl(0U, 0.0f);
    this.StartCoroutine(this.Fade(audio2, audio2.volume, this.volume.bgmVolume, duration, delay));
  }

  public void stopBGM()
  {
    foreach (CriAtomSource criAtomSource in this.bgmSource)
    {
      if (criAtomSource.status == 2)
      {
        criAtomSource.Stop();
        criAtomSource.cueName = (string) null;
      }
    }
  }

  public void stopBGMWithFadeOut(float duration)
  {
    foreach (CriAtomSource audio in this.bgmSource)
    {
      if (audio.status == 2)
        this.StartCoroutine(this.Fade(audio, audio.volume, 0.0f, duration));
      else
        audio.cueName = (string) null;
    }
  }

  public int playSE(AudioClip clip, bool isLoop = false)
  {
    int num = 0;
    CriAtomSource criAtomSource = (CriAtomSource) null;
    foreach (CriAtomSource seSource in this.seSources)
    {
      if (seSource.status != 2)
      {
        criAtomSource = seSource;
        break;
      }
      ++num;
    }
    if (Object.op_Inequality((Object) criAtomSource, (Object) null))
    {
      criAtomSource.cueName = ((Object) clip).name;
      if (isLoop)
        criAtomSource.loop = true;
      criAtomSource.Play();
    }
    else
      num = -1;
    return num;
  }

  public int playSE(string clip, bool isLoop = false)
  {
    int num = 0;
    CriAtomSource criAtomSource = (CriAtomSource) null;
    foreach (CriAtomSource seSource in this.seSources)
    {
      if (seSource.status != 2)
      {
        criAtomSource = seSource;
        break;
      }
      ++num;
    }
    if (Object.op_Inequality((Object) criAtomSource, (Object) null))
    {
      criAtomSource.cueName = clip;
      if (isLoop)
        criAtomSource.loop = true;
      criAtomSource.Play();
    }
    else
      num = -1;
    return num;
  }

  public int playSE(int clip, bool isLoop = false)
  {
    int num = 0;
    CriAtomSource criAtomSource = (CriAtomSource) null;
    foreach (CriAtomSource seSource in this.seSources)
    {
      if (seSource.status != 2)
      {
        criAtomSource = seSource;
        break;
      }
      ++num;
    }
    if (Object.op_Inequality((Object) criAtomSource, (Object) null))
    {
      if (isLoop)
        criAtomSource.loop = true;
      criAtomSource.Play(clip);
    }
    else
      num = -1;
    return num;
  }

  public void stopSE(int channel = -1)
  {
    if (channel == -1)
    {
      foreach (CriAtomSource seSource in this.seSources)
      {
        if (seSource.status == 2)
          seSource.Stop();
      }
    }
    else
    {
      if (this.seSources[channel].status != 2)
        return;
      this.seSources[channel].Stop();
    }
  }

  public int getVoiceAudioChannel()
  {
    int voiceAudioChannel;
    for (voiceAudioChannel = 0; voiceAudioChannel < this.voiceSources.Length; ++voiceAudioChannel)
    {
      if (voiceAudioChannel != this.reserveVoiceAudioChannel && this.voiceSources[voiceAudioChannel].status != 2)
        goto label_5;
    }
    voiceAudioChannel = -1;
label_5:
    this.reserveVoiceAudioChannel = voiceAudioChannel;
    return voiceAudioChannel;
  }

  public void cleanupReserveAudioChannel(int channel = -1)
  {
    if (channel != -1 && channel != this.reserveVoiceAudioChannel)
      return;
    this.reserveVoiceAudioChannel = -1;
  }

  public int playVoice(AudioClip clip, int channel = -1)
  {
    if (channel == -1)
      channel = this.getVoiceAudioChannel();
    if (channel == this.reserveVoiceAudioChannel)
      this.reserveVoiceAudioChannel = -1;
    if (channel != -1)
    {
      this.voiceSources[channel].cueName = ((Object) clip).name;
      this.voiceSources[channel].Play();
    }
    return channel;
  }

  public int playVoice(string clip, int channel = -1)
  {
    if (channel == -1)
      channel = this.getVoiceAudioChannel();
    if (channel == this.reserveVoiceAudioChannel)
      this.reserveVoiceAudioChannel = -1;
    if (channel != -1)
    {
      this.voiceSources[channel].cueName = clip;
      this.voiceSources[channel].Play();
    }
    return channel;
  }

  public int playVoiceByID(int id, int character_id, int channel = -1)
  {
    if (channel == -1)
      channel = this.getVoiceAudioChannel();
    if (channel == this.reserveVoiceAudioChannel)
      this.reserveVoiceAudioChannel = -1;
    if (channel != -1)
    {
      this.voiceSources[channel].cueSheet = "VO_" + character_id.ToString();
      this.voiceSources[channel].Play(id);
    }
    return channel;
  }

  public void stopVoice(int channel = -1)
  {
    if (channel == -1)
    {
      foreach (CriAtomSource voiceSource in this.voiceSources)
      {
        if (voiceSource.status == 2)
          voiceSource.Stop();
      }
    }
    else
    {
      if (this.voiceSources[channel].status != 2)
        return;
      this.voiceSources[channel].Stop();
    }
  }

  public CriAtomSource getVoiceAudioSource(int channel)
  {
    return channel == -1 || channel >= this.voiceSources.Length ? (CriAtomSource) null : this.voiceSources[channel];
  }

  public CRISoundManager.SoundVolume getVolume() => this.volume;

  private IEnumerator delayPlay(CriAtomSource audio, float delay)
  {
    if ((double) delay != 0.0)
      yield return (object) new WaitForSeconds(delay);
    audio.Play();
  }

  [Serializable]
  public class SoundVolume
  {
    [SerializeField]
    [Range(0.0f, 1f)]
    private float mBgmVolume = 0.5f;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float mSeVolume = 0.5f;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float mVoiceVolume = 0.5f;
    [SerializeField]
    private bool mMute;
    public bool modified = true;

    public float bgmVolume
    {
      get => this.mBgmVolume;
      set
      {
        this.mBgmVolume = value;
        this.modified = true;
      }
    }

    public float seVolume
    {
      get => this.mSeVolume;
      set
      {
        this.mSeVolume = value;
        this.modified = true;
      }
    }

    public float voiceVolume
    {
      get => this.mVoiceVolume;
      set
      {
        this.mVoiceVolume = value;
        this.modified = true;
      }
    }

    public bool mute
    {
      get => this.mMute;
      set
      {
        this.mMute = value;
        this.modified = true;
      }
    }
  }
}
