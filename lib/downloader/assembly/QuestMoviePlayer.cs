// Decompiled with JetBrains decompiler
// Type: QuestMoviePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class QuestMoviePlayer : MonoBehaviour
{
  private List<string> windowsMoviePath = new List<string>();
  private int index;
  private Action action;
  [SerializeField]
  private GameObject MainPanel;

  public bool isPlayMovie(int s_id) => this.checkSid(s_id);

  public void Attach(int s_id, Action act)
  {
    this.action = act;
    this.StartCoroutine(this.Play());
  }

  public void Attach(string fileName, bool enabledSkip, Action act)
  {
    this.action = act;
    this.windowsMoviePath.Add(string.Format("windows/{0}", (object) fileName));
    this.StartCoroutine(this.Play(enabledSkip));
  }

  private bool checkSid(int id)
  {
    IEnumerable<QuestMovieQuest> source = ((IEnumerable<QuestMovieQuest>) MasterData.QuestMovieQuestList).Where<QuestMovieQuest>((Func<QuestMovieQuest, bool>) (x => x.quest_s_id == id));
    if (source == null)
      return false;
    this.windowsMoviePath = source.Select<QuestMovieQuest, string>((Func<QuestMovieQuest, string>) (x => x.movie_path.windows_path)).ToList<string>();
    return true;
  }

  private void ChangeScene(bool activeFlag)
  {
    if (this.action == null)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = activeFlag;
    this.action();
  }

  private string moviePath()
  {
    return this.index >= this.windowsMoviePath.Count ? "" : this.windowsMoviePath[this.index++];
  }

  public IEnumerator Play(bool enabledSkip = true)
  {
    this.HideMainPanel();
    StatusBarHelper.SetVisibilityForIPhoneX(false);
    Singleton<NGSoundManager>.GetInstance().StopAll();
    bool flag = Singleton<CommonRoot>.GetInstance().isLoading;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    while (true)
    {
      string moviepath = this.moviePath();
      if (!(moviepath == ""))
      {
        IEnumerator e = OnDemandDownload.waitLoadMovieResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviepath), false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        string str = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPathForMovie(moviepath);
        string absoluteUri = new Uri(str).AbsoluteUri;
        GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject;
        gameObject.GetComponentInChildren<MovieInputController>().enableSkip = enabledSkip;
        WindowsMovieController wmc = gameObject.GetComponent<WindowsMovieController>();
        wmc.ShowMovie(str);
        yield return (object) new WaitForEndOfFrame();
        while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
          yield return (object) new WaitForEndOfFrame();
        yield return (object) null;
        moviepath = (string) null;
        wmc = (WindowsMovieController) null;
      }
      else
        break;
    }
    yield return (object) new WaitForEndOfFrame();
    this.ChangeScene(flag);
  }

  public void HideMainPanel()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(false);
    if (Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<Transform>(), (Object) null))
      ((Component) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<Transform>()).gameObject.SetActive(false);
    this.MainPanel.SetActive(false);
    ((Component) this).gameObject.SetActive(true);
  }

  public void ShowMainPanel() => this.MainPanel.SetActive(true);
}
