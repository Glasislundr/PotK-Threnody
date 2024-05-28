// Decompiled with JetBrains decompiler
// Type: Story0099Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0099Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private MovieLibraryPlayer movieObj;
  [SerializeField]
  private UIButton btnBack_;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public bool onStartMovie()
  {
    if (this.IsPushAndSet())
      return false;
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(true);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    if (Object.op_Inequality((Object) this.btnBack_, (Object) null))
      ((UIButtonColor) this.btnBack_).isEnabled = false;
    this.movieObj.endAction = (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      Singleton<CommonRoot>.GetInstance().setDisableFooterColor(false);
      if (Object.op_Inequality((Object) this.btnBack_, (Object) null))
        ((UIButtonColor) this.btnBack_).isEnabled = true;
      this.IsPush = false;
    });
    return true;
  }

  public IEnumerator PlayOpeningMovie()
  {
    yield return (object) new WaitForSeconds(1f);
    this.movieObj.AttachOpMovie();
  }

  public IEnumerator PlayTutorialMovie1()
  {
    yield return (object) new WaitForSeconds(1f);
    this.movieObj.AttachTutorialMovie1();
  }

  public IEnumerator PlayTutorialMovie2()
  {
    yield return (object) new WaitForSeconds(1f);
    this.movieObj.AttachTutorialMovie2();
  }

  public IEnumerator PlayStoryQuestMovie(int id)
  {
    yield return (object) new WaitForSeconds(1f);
    this.movieObj.AttachStoryQuestMovie(id);
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  public IEnumerator InitScene()
  {
    Story0099Menu menu = this;
    Future<GameObject> prefabScrollPartsF = Res.Prefabs.story009_9.story0099scroll.Load<GameObject>();
    IEnumerator e = prefabScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabScrollPartsF.Result;
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    QuestMovieQuest[] questMovieQuestList = MasterData.QuestMovieQuestList;
    menu.ScrollContainer.Clear();
    menu.ScrollInit(menu, Consts.GetInstance().STORY_0099MOVIE_LIST_OPMOVIE, Story0099Scroll.MovieType.OPENING, result);
    foreach (QuestMovieQuest questMovieQuest in questMovieQuestList)
    {
      QuestMovieQuest movie = questMovieQuest;
      PlayerStoryQuestS playerStoryQuestS = ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.ID == movie.quest_s_id)).FirstOrDefault<PlayerStoryQuestS>();
      if (playerStoryQuestS != null && !playerStoryQuestS.is_new)
        menu.ScrollInit(menu, movie.movie_path_QuestMoviePath, movie.movie_path.title, result);
    }
    menu.ScrollContainer.ResolvePosition();
  }

  public void ScrollInit(
    Story0099Menu menu,
    string title,
    Story0099Scroll.MovieType type,
    GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.GetComponent<Story0099Scroll>().Init(menu, title, type, 0);
    this.ScrollContainer.Add(gameObject);
  }

  public void ScrollInit(Story0099Menu menu, int movieID, string title, GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.GetComponent<Story0099Scroll>().Init(menu, title, Story0099Scroll.MovieType.STORY, movieID);
    this.ScrollContainer.Add(gameObject);
  }
}
