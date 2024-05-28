// Decompiled with JetBrains decompiler
// Type: Sea030HomeScene
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
public class Sea030HomeScene : NGSceneBase
{
  private bool isLoginOrInfo;
  private bool playStory;
  private Sea030HomeMenu sea030Menu;
  private string homeMapBGMFile;
  private string homeMapBGMCue;

  public static void ChangeScene(bool isStack, bool isDuringDate = false)
  {
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_home", (isStack ? 1 : 0) != 0, (object) isDuringDate);
  }

  public IEnumerator onStartSceneAsync(bool isDuringDate)
  {
    Sea030HomeScene sea030HomeScene = this;
    sea030HomeScene.isLoginOrInfo = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = !isDuringDate ? 0 : 3;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    sea030HomeScene.sea030Menu = sea030HomeScene.sea030Menu ?? sea030HomeScene.menuBase as Sea030HomeMenu;
    IEnumerator e = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!isDuringDate && Singleton<NGGameDataManager>.GetInstance().InfoOrLoginBonusJump())
    {
      sea030HomeScene.isLoginOrInfo = true;
    }
    else
    {
      if (!isDuringDate)
      {
        Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), sea030HomeScene.sceneName), sea030HomeScene.sceneName);
        sea030HomeScene.playStory = Persist.eventStoryPlay.Data.PlayEventScript(sea030HomeScene.sceneName, 0);
      }
      if (Object.op_Inequality((Object) sea030HomeScene.sea030Menu, (Object) null))
      {
        e = sea030HomeScene.sea030Menu.onStartSceneAsync(isDuringDate);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      sea030HomeScene.bgmFile = (string) null;
      sea030HomeScene.bgmName = (string) null;
      if (!sea030HomeScene.sea030Menu.ExistDateFlows())
      {
        sea030HomeScene.bgmFile = sea030HomeScene.homeMapBGMFile;
        sea030HomeScene.bgmName = sea030HomeScene.homeMapBGMCue;
      }
    }
  }

  public IEnumerator onBackSceneAsync(bool isDuringDate)
  {
    Sea030HomeScene sea030HomeScene = this;
    sea030HomeScene.isLoginOrInfo = false;
    sea030HomeScene.playStory = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = !sea030HomeScene.sea030Menu.ExistDateFlows() ? 0 : 3;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (Object.op_Inequality((Object) sea030HomeScene.sea030Menu, (Object) null))
    {
      IEnumerator e = sea030HomeScene.sea030Menu.onBackSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    sea030HomeScene.bgmFile = (string) null;
    sea030HomeScene.bgmName = (string) null;
    if (!sea030HomeScene.sea030Menu.ExistDateFlows())
    {
      sea030HomeScene.bgmFile = sea030HomeScene.homeMapBGMFile;
      sea030HomeScene.bgmName = sea030HomeScene.homeMapBGMCue;
    }
  }

  public void onStartScene(bool isDuringDate)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (!Singleton<NGGameDataManager>.GetInstance().successStartSceneAsyncProxyImpl)
      return;
    if (!this.isLoginOrInfo && !this.sea030Menu.ExistDateFlows() && !this.playStory)
    {
      bool flag = false;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      if (Singleton<NGGameDataManager>.GetInstance().loginBonuses != null)
      {
        List<PlayerLoginBonus> list = Singleton<NGGameDataManager>.GetInstance().loginBonuses.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.popup)).ToList<PlayerLoginBonus>();
        if (list.Count > 0)
        {
          this.sea030Menu.openningNoticePrefab = true;
          this.sea030Menu.LoginPopupStart(list, list.Count<PlayerLoginBonus>(), new Action(this.ShowAdvice));
          flag = true;
        }
      }
      if (!flag)
      {
        List<LevelRewardSchemaMixin> playerLevelRewards = Singleton<NGGameDataManager>.GetInstance().playerLevelRewards;
        if (playerLevelRewards != null && playerLevelRewards.Count > 0)
        {
          this.sea030Menu.openningNoticePrefab = true;
          this.sea030Menu.LevelUpPopupStart(playerLevelRewards, new Action(this.ShowAdvice));
          flag = true;
        }
      }
      if (!flag)
        this.ShowAdvice();
    }
    this.sea030Menu.onStartScene();
  }

  private void ShowAdvice()
  {
    this.sea030Menu.openningNoticePrefab = false;
    if (Singleton<TutorialRoot>.GetInstance().ShowAdvice("sea030_home_tutorial", finishCallback: (Action) (() => this.StartCoroutine(this.sea030Menu.OpenSeaHomeTutorial()))))
      return;
    this.StartCoroutine(this.sea030Menu.OpenSeaHomeTutorial());
  }

  public void onBackScene(bool isDuringDate)
  {
    if (!this.sea030Menu.ExistDateFlows() && !this.sea030Menu.isReturnSelectSpot && !this.sea030Menu.isSelectedSpot)
    {
      bool flag = false;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      if (Singleton<NGGameDataManager>.GetInstance().loginBonuses != null)
      {
        List<PlayerLoginBonus> list = Singleton<NGGameDataManager>.GetInstance().loginBonuses.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.popup)).ToList<PlayerLoginBonus>();
        if (list.Count > 0)
        {
          this.sea030Menu.LoginPopupStart(list, list.Count<PlayerLoginBonus>(), new Action(this.ShowAdvice));
          flag = true;
        }
      }
      if (!flag)
      {
        List<LevelRewardSchemaMixin> playerLevelRewards = Singleton<NGGameDataManager>.GetInstance().playerLevelRewards;
        if (playerLevelRewards != null && playerLevelRewards.Count > 0)
        {
          this.sea030Menu.LevelUpPopupStart(playerLevelRewards, new Action(this.ShowAdvice));
          flag = true;
        }
      }
      if (!flag)
        this.ShowAdvice();
    }
    this.sea030Menu.onStartScene();
  }

  public override void onEndScene()
  {
    Singleton<NGSceneManager>.GetInstance().SetCurrentSceneArgs((object) false);
    this.sea030Menu.onEndScene();
  }

  public void SetBgm(string cueSheet, string cueName)
  {
    this.homeMapBGMFile = cueSheet;
    this.homeMapBGMCue = cueName;
  }

  public void PlayBgm()
  {
    Singleton<NGSoundManager>.GetInstance().PlayBgmFile(this.bgmFile, this.bgmName);
  }
}
