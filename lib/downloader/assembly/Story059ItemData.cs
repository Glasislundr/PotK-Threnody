// Decompiled with JetBrains decompiler
// Type: Story059ItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story059ItemData
{
  public int _episodeID;
  public Story0593ScrollItem _myStory0593ScrollItem;
  public Story059ItemData.PlayType playType;
  private string moviePath;
  private int storyPlaybackID;

  public EarthQuestEpisode episode => MasterData.EarthQuestEpisode[this._episodeID];

  public EarthQuestChapter chaptor => this.episode.chapter;

  public EarthQuestStoryPlayback storyPlayback
  {
    get
    {
      return MasterData.EarthQuestStoryPlayback.ContainsKey(this.storyPlaybackID) ? MasterData.EarthQuestStoryPlayback[this.storyPlaybackID] : (EarthQuestStoryPlayback) null;
    }
  }

  public string title
  {
    get => this.storyPlayback != null ? this.storyPlayback.title : this.episode.episode_name;
  }

  public Story059ItemData(
    int episodeID,
    Story059ItemData.PlayType type,
    string moviePath = null,
    int storyPlaybackID = 0)
  {
    this._episodeID = episodeID;
    this.playType = type;
    this.moviePath = moviePath;
    this.storyPlaybackID = storyPlaybackID;
  }

  private string GetMoviePath() => "windows/" + this.moviePath;

  public void Play()
  {
    if (this.playType != Story059ItemData.PlayType.MOVIE)
    {
      Singleton<EarthDataManager>.GetInstance().displayEpsodeData = this.episode;
      Singleton<EarthDataManager>.GetInstance().isStoryPlayBackMode = true;
    }
    if (this.playType == Story059ItemData.PlayType.MOVIE && !string.IsNullOrEmpty(this.moviePath))
      Singleton<CommonRoot>.GetInstance().StartCoroutine(this.PlayMovie(this.GetMoviePath()));
    else if (this.playType == Story059ItemData.PlayType.STORY_SCRIPT && MasterData.EarthQuestStoryPlayback.ContainsKey(this.storyPlaybackID))
    {
      Story0093Scene.changeScene009_3(true, this.storyPlayback.script_id);
    }
    else
    {
      if (this.playType != Story059ItemData.PlayType.STORY_SCRIPT || !this.episode.script.HasValue)
        return;
      Story0093Scene.changeScene009_3(true, this.episode.script.Value);
    }
  }

  private IEnumerator PlayMovie(string moviePath)
  {
    IEnumerator e = OnDemandDownload.waitLoadMovieResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviePath), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    string uriString = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPathForMovie(moviePath);
    string absoluteUri = new Uri(uriString).AbsoluteUri;
    string moviePath1 = uriString;
    this._myStory0593ScrollItem.ShowMainPanel(false);
    Singleton<NGSoundManager>.GetInstance().stopBGM();
    StatusBarHelper.SetVisibilityForIPhoneX(false);
    WindowsMovieController wmc = (Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject).GetComponent<WindowsMovieController>();
    wmc.ShowMovie(moviePath1);
    yield return (object) new WaitForEndOfFrame();
    while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
      yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1f);
    StatusBarHelper.SetVisibilityForIPhoneX(true);
    this._myStory0593ScrollItem.ShowMainPanel(true);
    e = Singleton<NGSceneManager>.GetInstance().sceneBase.PlayBGM();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
  }

  public enum PlayType
  {
    STORY_SCRIPT,
    MOVIE,
  }
}
