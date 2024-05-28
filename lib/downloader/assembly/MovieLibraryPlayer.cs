// Decompiled with JetBrains decompiler
// Type: MovieLibraryPlayer
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
public class MovieLibraryPlayer : MonoBehaviour
{
  [SerializeField]
  private GameObject MainPanel;
  [SerializeField]
  private UIWidget dir_LeftFit;
  [SerializeField]
  private UIWidget dir_RightFit;
  [NonSerialized]
  public Action endAction;
  [SerializeField]
  private UIWidget dir_Title;
  [SerializeField]
  private UISprite slc_title;
  [SerializeField]
  private UILabel txt_Title;
  [SerializeField]
  private UISprite ibtn_Back;
  [SerializeField]
  private UISprite ibtn_Back_txt;

  public void AttachOpMovie() => this.StartCoroutine(this.Play("windows/opmovie_3"));

  public void AttachTutorialMovie1() => this.StartCoroutine(this.Play("windows/Shortmovie_001"));

  public void AttachTutorialMovie2() => this.StartCoroutine(this.Play("windows/Shortmovie_003"));

  public void AttachStoryQuestMovie(int id)
  {
    this.StartCoroutine(this.Play(this.GetQuestMoviePaht(id)));
  }

  private string GetQuestMoviePaht(int id)
  {
    QuestMovieQuest questMovieQuest = ((IEnumerable<QuestMovieQuest>) MasterData.QuestMovieQuestList).Where<QuestMovieQuest>((Func<QuestMovieQuest, bool>) (x => x.movie_path_QuestMoviePath == id)).FirstOrDefault<QuestMovieQuest>();
    return questMovieQuest == null ? "" : questMovieQuest.movie_path.windows_path;
  }

  private void Hide()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = false;
    ((Component) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<Transform>()).gameObject.SetActive(false);
    this.MainPanel.SetActive(false);
  }

  private void Show()
  {
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
    ((Component) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<Transform>()).gameObject.SetActive(true);
    this.MainPanel.SetActive(true);
  }

  private IEnumerator Play(string moviePath)
  {
    IEnumerator e = OnDemandDownload.waitLoadMovieResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviePath), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    string uriString = Singleton<ResourceManager>.GetInstance().ResolveStreamingAssetsPathForMovie(moviePath);
    string absoluteUri = new Uri(uriString).AbsoluteUri;
    string moviePath1 = uriString;
    this.Hide();
    StatusBarHelper.SetVisibilityForIPhoneX(false);
    Singleton<NGSoundManager>.GetInstance().stopBGM();
    WindowsMovieController wmc = (Object.Instantiate(Resources.Load("Prefabs/WindowsMovie")) as GameObject).GetComponent<WindowsMovieController>();
    wmc.ShowMovie(moviePath1);
    yield return (object) new WaitForEndOfFrame();
    while (((CriManaMovieMaterial) wmc.movieScreen).player.status != null)
      yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1f);
    e = Singleton<NGSceneManager>.GetInstance().sceneBase.PlayBGM();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    StatusBarHelper.SetVisibilityForIPhoneX(true);
    this.Show();
    yield return (object) null;
    if (this.endAction != null)
      this.endAction();
  }
}
