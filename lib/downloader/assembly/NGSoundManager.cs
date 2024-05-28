// Decompiled with JetBrains decompiler
// Type: NGSoundManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CriWare;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class NGSoundManager : Singleton<NGSoundManager>
{
  private const string CriSelectorName = "Type";
  [SerializeField]
  private int maxBGM = 3;
  [SerializeField]
  private int maxSE = 10;
  [SerializeField]
  private int maxVoice = 3;
  [SerializeField]
  private int maxStreamBgm = 10;
  [SerializeField]
  private int maxStreamVoice = 10;
  [SerializeField]
  private float[] busLevel = new float[8];
  private readonly string beforeBgmName = "OpBgmCueSheet";
  private readonly string beforeSeName = "OpCueSheet";
  private readonly string afterBgmName = "BgmCueSheet";
  private readonly string afterSeName = "SECueSheet";
  private readonly string bgmCueSheetName = "BgmCueSheet";
  private readonly string seCueSheetName = "SECueSheet";
  private List<string> bgmCueSheetNameList = new List<string>();
  private List<string> voiceCueSheetName = new List<string>();
  private string bgmCueSheetNameTemp;
  private CriAtomExAcb bgmAcb;
  private List<CriAtomExAcb> seAcbList = new List<CriAtomExAcb>();
  private readonly float[] defaultBusLevel = new float[8]
  {
    1f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f,
    0.0f
  };
  private NGSoundManager.InitializeState initializeState;
  private bool isInitialize;
  private NGSoundManager.BgmPlayer[] bgmPlayer;
  private CriAtomExPlayback?[] bgmPlayback;
  private CriAtomExPlayer[] sePlayer;
  private CriAtomExPlayer[] voicePlayer;
  private bool isTitleScene;
  private GameObject audioObject;
  public NGSoundManager.SoundVolume volume;

  public bool IsTitleScene
  {
    set => this.isTitleScene = value;
    get => this.isTitleScene;
  }

  public string platform => "windows";

  protected override void Initialize()
  {
  }

  protected override void Finlaize()
  {
    if (!this.isInitialize)
      return;
    ((IEnumerable<NGSoundManager.BgmPlayer>) this.bgmPlayer).ForEach<NGSoundManager.BgmPlayer>((Action<NGSoundManager.BgmPlayer>) (v => ((CriDisposable) v.player).Dispose()));
    ((IEnumerable<CriAtomExPlayback?>) this.bgmPlayback).ForEach<CriAtomExPlayback?>((Action<CriAtomExPlayback?>) (v => v = new CriAtomExPlayback?()));
    ((IEnumerable<CriAtomExPlayer>) this.sePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v => ((CriDisposable) v).Dispose()));
    ((IEnumerable<CriAtomExPlayer>) this.voicePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v => ((CriDisposable) v).Dispose()));
    this.isInitialize = false;
  }

  private void OnDestroy() => this.Finlaize();

  public void CheckInitialize(bool forcingCompleted = false)
  {
    if (!this.isInitialize)
    {
      this.audioObject = new GameObject("AudioObject");
      this.audioObject.transform.position = ((Component) this).transform.position;
      this.audioObject.transform.parent = ((Component) this).transform;
      this.bgmPlayer = new NGSoundManager.BgmPlayer[this.maxBGM];
      this.bgmPlayback = new CriAtomExPlayback?[this.maxBGM];
      for (int index = 0; index < this.bgmPlayer.Length; ++index)
      {
        this.bgmPlayer[index] = new NGSoundManager.BgmPlayer();
        this.bgmPlayer[index].player.AttachFader();
        this.bgmPlayback[index] = new CriAtomExPlayback?();
      }
      this.sePlayer = new CriAtomExPlayer[this.maxSE];
      for (int index = 0; index < this.sePlayer.Length; ++index)
      {
        this.sePlayer[index] = new CriAtomExPlayer();
        this.sePlayer[index].AttachFader();
        this.sePlayer[index].SetFadeInTime(0);
      }
      this.voicePlayer = new CriAtomExPlayer[this.maxVoice];
      for (int index = 0; index < this.voicePlayer.Length; ++index)
      {
        this.voicePlayer[index] = new CriAtomExPlayer();
        this.voicePlayer[index].AttachFader();
        this.voicePlayer[index].SetFadeInTime(0);
      }
      this.getVolume().bgmVolume = Persist.volume.Data.Bgm;
      this.getVolume().seVolume = Persist.volume.Data.Se;
      this.getVolume().voiceVolume = Persist.volume.Data.Voice;
      this.UpdateVolumeLevel();
      this.volume.modified = false;
      this.isInitialize = true;
    }
    if (!this.isTitleScene && this.initializeState == NGSoundManager.InitializeState.DLC_AFTER || !forcingCompleted && this.initializeState == NGSoundManager.InitializeState.DLC_BEFORE && (!ResourceDownloader.Completed || ResourceDownloader.Error != null) || this.isTitleScene && this.initializeState == NGSoundManager.InitializeState.DLC_BEFORE)
      return;
    try
    {
      if (!this.isTitleScene && ((!ResourceDownloader.Completed ? 0 : (ResourceDownloader.Error == null ? 1 : 0)) | (forcingCompleted ? 1 : 0)) != 0)
      {
        this.StopBgm(time: 0.0f);
        this.StopSe(time: 0.0f);
        this.StopVoice(time: 0.0f);
        this.bgmAcb = (CriAtomExAcb) null;
        this.seAcbList.Clear();
        CriAtom.RemoveCueSheet(this.bgmCueSheetName);
        CriAtom.RemoveCueSheet(this.seCueSheetName);
        CriAtomEx.UnregisterAcf();
        CriAtomEx.RegisterAcf((CriFsBinder) null, Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath("dlc/punk_music"));
        this.DetachDspBusSetting();
        CriAtom.AddCueSheet("AA", Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterBgmName + "_acb"), Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterBgmName + "_awb"), (CriFsBinder) null);
        CriAtom.AddCueSheet("BB", Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterSeName + "_acb"), Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterSeName + "_awb"), (CriFsBinder) null);
        CriAtom.AddCueSheet("BB2", Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterSeName + "_2_acb"), Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + this.afterSeName + "_2_awb"), (CriFsBinder) null);
        this.bgmAcb = CriAtom.GetAcb("AA");
        this.bgmCueSheetNameTemp = "AA";
        this.seAcbList.Add(CriAtom.GetAcb("BB"));
        this.seAcbList.Add(CriAtom.GetAcb("BB2"));
        this.initializeState = NGSoundManager.InitializeState.DLC_AFTER;
      }
      else
      {
        CriAtomEx.UnregisterAcf();
        CriAtomEx.RegisterAcf((CriFsBinder) null, Path.Combine(Common.streamingAssetsPath, "punk_music.acf"));
        this.seAcbList.Clear();
        CriAtom.AddCueSheet(this.bgmCueSheetName, this.beforeBgmName + ".acb", this.beforeBgmName + ".awb", (CriFsBinder) null);
        CriAtom.AddCueSheet(this.seCueSheetName, this.beforeSeName + ".acb", (string) null, (CriFsBinder) null);
        this.bgmAcb = CriAtom.GetAcb(this.bgmCueSheetName);
        this.bgmCueSheetNameTemp = this.bgmCueSheetName;
        this.seAcbList.Add(CriAtom.GetAcb(this.seCueSheetName));
        this.initializeState = NGSoundManager.InitializeState.DLC_BEFORE;
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
  }

  private void Update()
  {
    if (!this.isInitialize || !this.volume.modified)
      return;
    this.UpdateVolumeLevel();
    NGUITools.soundVolume = this.volume.mute ? 0.0f : this.volume.seVolume;
    this.volume.modified = false;
  }

  public void UpdateVolumeLevel()
  {
    ((IEnumerable<NGSoundManager.BgmPlayer>) this.bgmPlayer).ForEach<NGSoundManager.BgmPlayer>((Action<NGSoundManager.BgmPlayer>) (v =>
    {
      v.player.SetVolume(this.volume.mute ? 0.0f : this.volume.bgmVolume);
      v.player.UpdateAll();
    }));
    ((IEnumerable<CriAtomExPlayer>) this.sePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v =>
    {
      v.SetVolume(this.volume.mute ? 0.0f : this.volume.seVolume);
      v.UpdateAll();
    }));
    ((IEnumerable<CriAtomExPlayer>) this.voicePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v =>
    {
      v.SetVolume(this.volume.mute ? 0.0f : this.volume.voiceVolume);
      v.UpdateAll();
    }));
  }

  private IEnumerator PlayBGM(
    string clip,
    int channel = 0,
    float delay = 0.0f,
    float fadeInTime = 0.5f,
    float fadeOutTime = 0.5f)
  {
    NGSoundManager ngSoundManager = this;
    yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromBgm(clip), false);
    ngSoundManager.CheckInitialize();
    if (ngSoundManager.initializeState != NGSoundManager.InitializeState.DLC_BEFORE || ngSoundManager.bgmAcb.Exists(clip))
    {
      if (channel == -1)
      {
        int? nullable = ((IEnumerable<NGSoundManager.BgmPlayer>) ngSoundManager.bgmPlayer).FirstIndexOrNull<NGSoundManager.BgmPlayer>((Func<NGSoundManager.BgmPlayer, bool>) (v => v.player.GetStatus() == null || v.player.GetStatus() == 3));
        if (!nullable.HasValue)
          yield break;
        else
          channel = nullable.Value;
      }
      NGSoundManager.BgmPlayer bgmPlayer = ngSoundManager.bgmPlayer[channel];
      if (!(bgmPlayer.nowName == clip))
      {
        bgmPlayer.nowName = clip;
        bgmPlayer.nowCueName = ngSoundManager.bgmCueSheetNameTemp;
        bgmPlayer.aisac = 0.0f;
        bgmPlayer.player.SetFadeInTime((int) ((double) fadeInTime * 1000.0));
        bgmPlayer.player.SetFadeOutTime((int) ((double) fadeOutTime * 1000.0));
        bgmPlayer.player.SetCue(ngSoundManager.bgmAcb, clip);
        bgmPlayer.player.SetAisacControl(0U, bgmPlayer.aisac);
        ngSoundManager.bgmPlayback[channel] = new CriAtomExPlayback?(bgmPlayer.player.Start());
        if ((double) delay > 0.0)
        {
          bgmPlayer.player.Pause();
          ngSoundManager.StartCoroutine(ngSoundManager.delayPlay(bgmPlayer.player, delay));
        }
      }
    }
  }

  public int PlayBgm(string clip, int channel = 0, float delay = 0.0f, float fadeInTime = 0.5f, float fadeOutTime = 0.5f)
  {
    this.CheckInitialize();
    if (this.initializeState == NGSoundManager.InitializeState.DLC_BEFORE && !this.bgmAcb.Exists(clip))
      return -1;
    if (channel == -1)
    {
      int? nullable = ((IEnumerable<NGSoundManager.BgmPlayer>) this.bgmPlayer).FirstIndexOrNull<NGSoundManager.BgmPlayer>((Func<NGSoundManager.BgmPlayer, bool>) (v => v.player.GetStatus() == null || v.player.GetStatus() == 3));
      if (!nullable.HasValue)
        return -1;
      channel = nullable.Value;
    }
    NGSoundManager.BgmPlayer bgmPlayer = this.bgmPlayer[channel];
    if (bgmPlayer.nowName == clip)
      return -1;
    bgmPlayer.nowName = clip;
    bgmPlayer.nowCueName = this.bgmCueSheetNameTemp;
    bgmPlayer.aisac = 0.0f;
    bgmPlayer.player.SetFadeInTime((int) ((double) fadeInTime * 1000.0));
    bgmPlayer.player.SetFadeOutTime((int) ((double) fadeOutTime * 1000.0));
    bgmPlayer.player.SetCue(this.bgmAcb, clip);
    bgmPlayer.player.SetAisacControl(0U, bgmPlayer.aisac);
    this.bgmPlayback[channel] = new CriAtomExPlayback?(bgmPlayer.player.Start());
    if ((double) delay > 0.0)
    {
      bgmPlayer.player.Pause();
      this.StartCoroutine(this.delayPlay(bgmPlayer.player, delay));
    }
    return channel;
  }

  private IEnumerator PlayBGMFile(
    string file,
    string clip,
    int channel = 0,
    float delay = 0.0f,
    float fadeInTime = 0.5f,
    float fadeOutTime = 0.5f)
  {
    NGSoundManager ngSoundManager = this;
    yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromBgm(file), false);
    ngSoundManager.CheckInitialize();
    if (ngSoundManager.checkSoundData(ngSoundManager.bgmCueSheetNameList, ngSoundManager.maxStreamBgm, file))
    {
      if (channel == -1)
      {
        int? nullable = ((IEnumerable<NGSoundManager.BgmPlayer>) ngSoundManager.bgmPlayer).FirstIndexOrNull<NGSoundManager.BgmPlayer>((Func<NGSoundManager.BgmPlayer, bool>) (v => v.player.GetStatus() == null || v.player.GetStatus() == 3));
        if (!nullable.HasValue)
          yield break;
        else
          channel = nullable.Value;
      }
      NGSoundManager.BgmPlayer bgmPlayer = ngSoundManager.bgmPlayer[channel];
      if (!(bgmPlayer.nowName == clip))
      {
        bgmPlayer.nowName = clip;
        bgmPlayer.nowCueName = file;
        bgmPlayer.aisac = 0.0f;
        bgmPlayer.player.SetFadeInTime((int) ((double) fadeInTime * 1000.0));
        bgmPlayer.player.SetFadeOutTime((int) ((double) fadeOutTime * 1000.0));
        CriAtomExAcb acb = CriAtom.GetAcb(file);
        bgmPlayer.player.SetCue(acb, clip);
        bgmPlayer.player.SetAisacControl(0U, bgmPlayer.aisac);
        ngSoundManager.bgmPlayback[channel] = new CriAtomExPlayback?(bgmPlayer.player.Start());
        if ((double) delay > 0.0)
        {
          bgmPlayer.player.Pause();
          ngSoundManager.StartCoroutine(ngSoundManager.delayPlay(bgmPlayer.player, delay));
        }
      }
    }
  }

  public int PlayBgmFile(
    string file,
    string clip,
    int channel = 0,
    float delay = 0.0f,
    float fadeInTime = 0.5f,
    float fadeOutTime = 0.5f)
  {
    if (file == "" || file == this.afterBgmName)
      return this.PlayBgm(clip, channel, delay, fadeInTime, fadeOutTime);
    this.StartCoroutine(this.PlayBGMFile(file, clip, channel, delay, fadeInTime, fadeOutTime));
    return channel;
  }

  public void SetNextBGMBlock(int channel, int blockID)
  {
    if (channel < 0 || channel >= this.bgmPlayback.Length)
      return;
    CriAtomExPlayback? nullable = this.bgmPlayback[channel];
    if (!nullable.HasValue)
      return;
    CriAtomExPlayback criAtomExPlayback = nullable.Value;
    if (((CriAtomExPlayback) ref criAtomExPlayback).GetCurrentBlockIndex() == -1)
      return;
    criAtomExPlayback = nullable.Value;
    ((CriAtomExPlayback) ref criAtomExPlayback).SetNextBlockIndex(blockID);
  }

  private IEnumerator Fade(
    CriAtomExPlayer player,
    float v0,
    float v1,
    float duration,
    float delay = 0.0f)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    for (float currentTime = 0.0f; (double) duration > (double) currentTime; currentTime += Time.deltaTime)
    {
      player.SetVolume(Mathf.Lerp(v0, v1, currentTime / duration));
      yield return (object) null;
    }
    if ((double) v1 <= 0.0)
      player.Stop();
  }

  public void StopBgm(int channel = -1, float time = 0.5f)
  {
    if (!this.isInitialize)
      return;
    if (channel < 0)
    {
      ((IEnumerable<NGSoundManager.BgmPlayer>) this.bgmPlayer).ForEach<NGSoundManager.BgmPlayer>((Action<NGSoundManager.BgmPlayer>) (v => v.Stop(time)));
      ((IEnumerable<CriAtomExPlayback?>) this.bgmPlayback).ForEach<CriAtomExPlayback?>((Action<CriAtomExPlayback?>) (v => v = new CriAtomExPlayback?()));
    }
    else
    {
      if (channel >= this.bgmPlayer.Length)
        return;
      this.bgmPlayer[channel].Stop(time);
      this.bgmPlayback[channel] = new CriAtomExPlayback?();
    }
  }

  public void PauseBgm(int channel = -1)
  {
    if (!this.isInitialize)
      return;
    if (channel == -1)
    {
      for (int index = 0; index < this.bgmPlayer.Length; ++index)
        this.bgmPlayer[index].player.Pause();
    }
    else
    {
      if (channel >= this.bgmPlayer.Length)
        return;
      this.bgmPlayer[channel].player.Pause();
    }
  }

  public void ResumeBgm(int channel = -1, bool forceStop = false)
  {
    if (!this.isInitialize)
      return;
    if (channel == -1)
    {
      for (int index = 0; index < this.bgmPlayer.Length; ++index)
      {
        CriAtomExPlayer player = this.bgmPlayer[index].player;
        player.Pause(false);
        if (forceStop)
          player.Stop();
        else if ((double) this.getVolume().bgmVolume > 0.0)
          this.StartCoroutine(this.Fade(player, 0.0f, this.getVolume().bgmVolume, 0.5f));
      }
    }
    else
    {
      if (channel >= this.bgmPlayer.Length)
        return;
      CriAtomExPlayer player = this.bgmPlayer[channel].player;
      player.Pause(false);
      if (forceStop)
      {
        player.Stop();
      }
      else
      {
        if ((double) this.getVolume().bgmVolume <= 0.0)
          return;
        this.StartCoroutine(this.Fade(player, 0.0f, this.getVolume().bgmVolume, 0.5f));
      }
    }
  }

  public bool IsBgmPlaying(int channel)
  {
    if (!this.isInitialize || channel < 0 || channel >= this.bgmPlayer.Length)
      return false;
    CriAtomExPlayer player = this.bgmPlayer[channel].player;
    return player.GetStatus() == 1 || player.GetStatus() == 2;
  }

  public string GetBgmName(int channel = 0)
  {
    return !this.isInitialize || channel < 0 || channel >= this.bgmPlayer.Length ? "" : this.bgmPlayer[channel].nowName;
  }

  public string GetCueName(int channel = 0)
  {
    return !this.isInitialize || channel < 0 || channel >= this.bgmPlayer.Length ? "" : this.bgmPlayer[channel].nowCueName;
  }

  public int PlaySe(string clip, bool isLoop = false, float delay = 0.0f, int channel = -1)
  {
    this.CheckInitialize();
    for (int i = 0; i < this.seAcbList.Count; i++)
    {
      if (this.seAcbList[i].Exists(clip))
        return this.PlaySe(isLoop, delay, channel, true, (Action<CriAtomExPlayer>) (player => player.SetCue(this.seAcbList[i], clip)));
    }
    return -1;
  }

  private int PlaySe(
    bool isLoop,
    float delay,
    int channel,
    bool busEffect,
    Action<CriAtomExPlayer> action)
  {
    channel = Mathf.Clamp(channel, -1, this.maxSE - 1);
    if (channel == -1)
    {
      int? nullable = ((IEnumerable<CriAtomExPlayer>) this.sePlayer).FirstIndexOrNull<CriAtomExPlayer>((Func<CriAtomExPlayer, bool>) (v => v.GetStatus() == null || v.GetStatus() == 3));
      if (!nullable.HasValue)
        return -1;
      channel = nullable.Value;
    }
    CriAtomExPlayer player = this.sePlayer[channel];
    player.StopWithoutReleaseTime();
    this.SetBusSendLevel(busEffect, player);
    player.Loop(isLoop);
    action(player);
    if ((double) delay <= 0.0)
    {
      player.Start();
    }
    else
    {
      player.Prepare();
      this.StartCoroutine(this.delayPlay(player, delay));
    }
    return channel;
  }

  public void StopSe(int channel = -1, float time = 0.5f)
  {
    if (!this.isInitialize)
      return;
    this.stop(this.sePlayer, channel, time);
  }

  public void PauseSe(int channel = -1)
  {
    if (!this.isInitialize)
      return;
    if (channel == -1)
    {
      for (int channel1 = 0; channel1 < this.sePlayer.Length; ++channel1)
        this.pause(this.sePlayer, channel1);
    }
    else
    {
      if (channel >= this.sePlayer.Length)
        return;
      this.pause(this.sePlayer, channel);
    }
  }

  public void ResumeSe(int channel = -1)
  {
    if (!this.isInitialize)
      return;
    if (channel == -1)
    {
      for (int channel1 = 0; channel1 < this.sePlayer.Length; ++channel1)
        this.resume(this.sePlayer, channel1);
    }
    else
    {
      if (channel >= this.sePlayer.Length)
        return;
      this.resume(this.sePlayer, channel);
    }
  }

  public bool IsSePlaying(int channel)
  {
    if (!this.isInitialize || channel < 0 || channel >= this.sePlayer.Length)
      return false;
    CriAtomExPlayer criAtomExPlayer = this.sePlayer[channel];
    return criAtomExPlayer.GetStatus() == 1 || criAtomExPlayer.GetStatus() == 2;
  }

  public int playVoiceByStringID(
    UnitVoicePattern voicePattern,
    string cuename,
    int channel = -1,
    float delay = 0.0f)
  {
    return voicePattern == null ? -1 : this.PlayVoice(voicePattern.file_name, cuename, voicePattern.SelectorLabel, channel, delay);
  }

  public int playVoiceByID(UnitVoicePattern voicePattern, int id, int channel = -1, float delay = 0.0f)
  {
    return voicePattern == null ? -1 : this.PlayVoice(voicePattern.file_name, id, voicePattern.SelectorLabel, channel, delay);
  }

  public int PlayVoice(
    string fileName,
    string name,
    string selectorLabel,
    int channel = -1,
    float delay = 0.0f)
  {
    string id = fileName;
    if (fileName == "VO_9999" && name.Contains("durin"))
      id = "VO_9999_durin";
    return this.PlayVoice(id, delay, channel, true, selectorLabel, (Action<CriAtomExPlayer, CriAtomExAcb>) ((player, acb) => player.SetCue(acb, name)));
  }

  public int PlayVoice(string fileName, int id, string selectorLabel, int channel = -1, float delay = 0.0f)
  {
    return this.PlayVoice(fileName, delay, channel, true, selectorLabel, (Action<CriAtomExPlayer, CriAtomExAcb>) ((player, acb) => player.SetCue(acb, id)));
  }

  private int PlayVoice(
    string id,
    float delay,
    int channel,
    bool busEffect,
    string selectorLabel,
    Action<CriAtomExPlayer, CriAtomExAcb> action)
  {
    if (!this.isInitialize || !this.checkSoundData(this.voiceCueSheetName, this.maxStreamVoice, id))
      return -1;
    channel = Mathf.Clamp(channel, -1, this.maxVoice - 1);
    if (channel == -1)
    {
      int? nullable = ((IEnumerable<CriAtomExPlayer>) this.voicePlayer).FirstIndexOrNull<CriAtomExPlayer>((Func<CriAtomExPlayer, bool>) (v => v.GetStatus() == null || v.GetStatus() == 3));
      if (!nullable.HasValue)
        return -1;
      channel = nullable.Value;
    }
    CriAtomExPlayer player = this.voicePlayer[channel];
    CriAtomExAcb acb = CriAtom.GetAcb(id);
    if (acb == null)
      return -1;
    player.StopWithoutReleaseTime();
    this.SetBusSendLevel(busEffect, player);
    player.Loop(false);
    action(player, acb);
    if (!string.IsNullOrEmpty(selectorLabel))
      player.SetSelectorLabel("Type", selectorLabel);
    else
      player.ClearSelectorLabels();
    if ((double) delay <= 0.0)
    {
      player.Start();
    }
    else
    {
      player.Prepare();
      this.StartCoroutine(this.delayPlay(player, delay));
    }
    return channel;
  }

  public void StopVoice(int channel = -1, float time = 0.5f)
  {
    if (!this.isInitialize)
      return;
    this.stop(this.voicePlayer, channel, time);
  }

  public int PlayVoicePriorityFirst(UnitUnit unit, int id, int channel, float delay = 0.0f)
  {
    return !this.IsVoicePlaying(channel) ? this.playVoiceByID(unit.unitVoicePattern, id, channel, delay) : -1;
  }

  public bool IsVoicePlaying(int channel)
  {
    if (!this.isInitialize || channel < 0 || channel >= this.voicePlayer.Length)
      return false;
    CriAtomExPlayer criAtomExPlayer = this.voicePlayer[channel];
    return criAtomExPlayer.GetStatus() == 1 || criAtomExPlayer.GetStatus() == 2;
  }

  public bool IsVoiceStopAll()
  {
    if (!this.isInitialize)
      return true;
    foreach (CriAtomExPlayer criAtomExPlayer in this.voicePlayer)
    {
      if (criAtomExPlayer.GetStatus() == 1 || criAtomExPlayer.GetStatus() == 2)
        return false;
    }
    return true;
  }

  public bool LoadVoiceData(string fileName)
  {
    return this.checkSoundData(this.voiceCueSheetName, this.maxStreamVoice, fileName);
  }

  public void StopAll(float time = 0.5f)
  {
    this.StopBgm(time: time);
    this.StopSe(time: time);
    this.StopVoice(time: time);
  }

  private IEnumerator delayPlay(CriAtomExPlayer player, float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    player.Resume((CriAtomEx.ResumeMode) 0);
  }

  private void OnApplicationPause(bool enable)
  {
    if (!this.isInitialize)
      return;
    this.PauseSources(this.bgmPlayer, enable);
    this.PauseSources(this.sePlayer, enable);
    this.PauseSources(this.voicePlayer, enable);
  }

  private void PauseSources(CriAtomExPlayer[] sources, bool enable)
  {
    ((IEnumerable<CriAtomExPlayer>) sources).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v => v.Pause(enable)));
  }

  private void PauseSources(NGSoundManager.BgmPlayer[] sources, bool enable)
  {
    ((IEnumerable<NGSoundManager.BgmPlayer>) sources).ForEach<NGSoundManager.BgmPlayer>((Action<NGSoundManager.BgmPlayer>) (v => v.player.Pause(enable)));
  }

  public NGSoundManager.SoundVolume getVolume() => this.volume;

  private void stop(CriAtomExPlayer[] playerList, int channel, float time)
  {
    if (channel < 0)
    {
      ((IEnumerable<CriAtomExPlayer>) playerList).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v =>
      {
        v.SetFadeOutTime((int) ((double) time * 1000.0));
        v.Stop((double) time == 0.0);
      }));
    }
    else
    {
      if (channel >= playerList.Length)
        return;
      playerList[channel].SetFadeOutTime((int) ((double) time * 1000.0));
      playerList[channel].Stop((double) time == 0.0);
    }
  }

  private void pause(CriAtomExPlayer[] playerList, int channel) => playerList[channel].Pause();

  private IEnumerator pauseCorutine(CriAtomExPlayer player)
  {
    player.Pause(true);
    yield break;
  }

  private void resume(CriAtomExPlayer[] playerList, int channel)
  {
    float v1 = 0.0f;
    if (playerList == this.sePlayer)
      v1 = this.getVolume().seVolume;
    else if (playerList == this.voicePlayer)
      v1 = this.getVolume().voiceVolume;
    CriAtomExPlayer player = playerList[channel];
    player.Pause(false);
    this.StartCoroutine(this.Fade(player, 0.0f, v1, 0.5f));
  }

  private bool IsFile(string s)
  {
    return !string.IsNullOrEmpty(s) && new FileInfo(Path.Combine(Common.streamingAssetsPath, s)).Exists;
  }

  public bool LoadedCueSheet(string cue_name) => CriAtom.GetCueSheet(cue_name) != null;

  private bool checkSoundData(List<string> fileList, int maxValue, string file)
  {
    if (fileList.FirstIndexOrNull<string>((Func<string, bool>) (v => v == file)).HasValue)
    {
      fileList.Remove(file);
      fileList.Add(file);
      return true;
    }
    string str = file;
    if (!Singleton<ResourceManager>.GetInstance().Contains(this.platform + "/" + str + "_acb") || !Singleton<ResourceManager>.GetInstance().Contains(this.platform + "/" + str + "_acb"))
      return false;
    string s1 = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + str + "_acb");
    string s2 = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPath(this.platform + "/" + str + "_awb");
    if (!this.IsFile(s1) || !this.IsFile(s2))
      return false;
    if (fileList.Count >= maxValue)
    {
      CriAtom.RemoveCueSheet(fileList[0]);
      fileList.RemoveAt(0);
    }
    fileList.Add(file);
    CriAtom.AddCueSheet(str, s1, s2, (CriFsBinder) null);
    return true;
  }

  public void SetVolumeBgm(float volume)
  {
    ((IEnumerable<NGSoundManager.BgmPlayer>) this.bgmPlayer).ForEach<NGSoundManager.BgmPlayer>((Action<NGSoundManager.BgmPlayer>) (v =>
    {
      v.player.SetVolume(volume);
      v.player.UpdateAll();
    }));
  }

  public void SetVolumeSe(float volume)
  {
    ((IEnumerable<CriAtomExPlayer>) this.sePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v =>
    {
      v.SetVolume(volume);
      v.UpdateAll();
    }));
  }

  public void SetVolumeVoice(float volume)
  {
    ((IEnumerable<CriAtomExPlayer>) this.voicePlayer).ForEach<CriAtomExPlayer>((Action<CriAtomExPlayer>) (v =>
    {
      v.SetVolume(volume);
      v.UpdateAll();
    }));
  }

  public CriAtomSource playBGM(string clip, float startTime = 0.1f)
  {
    this.PlayBgm(clip);
    return (CriAtomSource) null;
  }

  public void playBGM(AudioClip clip, float startTime = 0.1f) => this.PlayBgm(((Object) clip).name);

  private IEnumerator BgmCrossFade(
    CriAtomSource audio,
    float v0,
    float v1,
    float duration,
    float delay = 0.0f)
  {
    yield break;
  }

  private IEnumerator BgmCrossFade(NGSoundManager.BgmPlayer player, float aisac, float duration)
  {
    float currentTime = 0.0f;
    float v0 = player.aisac;
    float v1 = aisac;
    while ((double) duration > (double) currentTime)
    {
      currentTime += Time.deltaTime;
      player.aisac = Mathf.Lerp(v0, v1, currentTime / duration);
      player.player.SetAisacControl(0U, player.aisac);
      player.player.UpdateAll();
      yield return (object) null;
    }
    player.aisac = v1;
    player.player.SetAisacControl(0U, player.aisac);
  }

  public void crossFadeCurrentBGM(float duration, float aisac)
  {
    this.StartCoroutine(this.BgmCrossFade(this.bgmPlayer[0], aisac, duration));
  }

  public IEnumerator crossFadeCurrentBGMAsync(float duration, float aisac)
  {
    yield return (object) this.BgmCrossFade(this.bgmPlayer[0], aisac, duration);
  }

  public void playBGMWithCrossFade(AudioClip clip, float duration, float startTime = 0.1f)
  {
    this.playBGMWithCrossFade(((Object) clip).name, duration, startTime);
  }

  public void playBGMWithCrossFade(string clip, float duration, float startTime = 0.1f)
  {
    this.PlayBgm(clip, fadeInTime: duration, fadeOutTime: duration);
  }

  public void playBGMWithCombineTimeFade(AudioClip clip, float duration, float startTime = 0.1f)
  {
    this.playBGMWithCombineTimeFade(((Object) clip).name, duration, startTime);
  }

  public void playBGMWithCombineTimeFade(string clip, float duration, float startTime = 0.1f)
  {
  }

  public void stopBGM() => this.StopBgm();

  public void stopBGMWithFadeOut(float duration) => this.StopBgm(time: duration);

  public CriAtomSource getNextBGMAudioSource() => (CriAtomSource) null;

  public void pauseBGM(int channel = -1) => this.PauseBgm(channel);

  public void resumeBGM(int channel = -1) => this.ResumeBgm(channel);

  public int playSE(AudioClip clip, bool isLoop = false, float delay = 0.0f)
  {
    return this.PlaySe(((Object) clip).name, isLoop, delay);
  }

  public int playSE(string clip, bool isLoop = false, float delay = 0.0f, int seChannel = -1)
  {
    return this.PlaySe(clip, isLoop, delay, seChannel);
  }

  public void stopSE(int channel = -1) => this.StopSe();

  public void fadeOutSE(int channel = -1, float duration = 1f) => this.StopSe(channel, duration);

  public void pauseSE(int channel = -1) => this.PauseSe(channel);

  public void resumeSE(int channel = -1) => this.ResumeSe(channel);

  public int playVoice(AudioClip clip, int channel = -1) => 0;

  public int playVoice(string clip, int channel = -1) => 0;

  public int playVoiceByID(
    string fileName,
    int id,
    int channel = -1,
    float delay = 0.0f,
    string selectorLabel = null)
  {
    return this.PlayVoice(fileName, id, selectorLabel, channel, delay);
  }

  public int playVoiceByStringID(
    string fileName,
    string cuename,
    int channel = -1,
    float delay = 0.0f,
    string selectorLabel = null)
  {
    return this.PlayVoice(fileName, cuename, selectorLabel, channel, delay);
  }

  public void stopVoice(int channel = -1, float time = 0.5f) => this.StopVoice(channel, time);

  public CriAtomSource getVoiceAudioSource(int channel) => (CriAtomSource) null;

  public void OpeningStart() => this.CheckInitialize();

  public void OpeningEnd() => this.CheckInitialize();

  public void checkIfDLCEnd() => this.CheckInitialize();

  public CriAtomSource getMainBGMAudioSource() => (CriAtomSource) null;

  public void AttachDspBusSetting(string settingName, float[] _busLevel)
  {
    CriAtomEx.DetachDspBusSetting();
    CriAtomEx.AttachDspBusSetting(settingName);
    this.busLevel = _busLevel;
  }

  public void DetachDspBusSetting()
  {
    CriAtomEx.DetachDspBusSetting();
    CriAtomEx.AttachDspBusSetting("default");
    this.busLevel = this.defaultBusLevel;
  }

  private void SetBusSendLevel(bool flag, CriAtomExPlayer player)
  {
    for (int index = 0; index < 8; ++index)
    {
      if ((double) this.busLevel[index] > 0.0)
      {
        player.SetBusSendLevel(index, 0.0f);
        player.SetBusSendLevelOffset(index, this.busLevel[index]);
      }
    }
  }

  public bool ExistsCueID(string name, int id)
  {
    CriAtomCueSheet cueSheet = CriAtom.GetCueSheet(name);
    return cueSheet != null && cueSheet.acb.Exists(id);
  }

  public bool IsEffectiveCueID(string name, int id)
  {
    CriAtomCueSheet cueSheet = CriAtom.GetCueSheet(name);
    CriAtomEx.CueInfo cueInfo;
    return cueSheet != null && cueSheet.acb.Exists(id) && cueSheet.acb.GetCueInfo(id, ref cueInfo) && cueInfo.length > 20L;
  }

  private enum InitializeState
  {
    NONE,
    DLC_BEFORE,
    DLC_AFTER,
  }

  private class BgmPlayer
  {
    public CriAtomExPlayer player = new CriAtomExPlayer();
    public string nowName = "";
    public string nowCueName = "";
    public float aisac;

    public void Stop(float time)
    {
      this.player.SetFadeOutTime((int) ((double) time * 1000.0));
      this.player.Stop((double) time == 0.0);
      this.nowName = "";
      this.nowCueName = "";
      this.aisac = 0.0f;
    }
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
    private float mVoiceVolume = 0.8f;
    [SerializeField]
    private bool mMute;
    public bool modified;

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
