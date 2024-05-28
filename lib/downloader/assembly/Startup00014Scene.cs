// Decompiled with JetBrains decompiler
// Type: Startup00014Scene
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
public class Startup00014Scene : NGSceneBase
{
  private List<PlayerLoginBonus> skipList = new List<PlayerLoginBonus>();
  [SerializeField]
  private Transform middle;
  private GameObject loginBonusPrefab;
  private GameObject loginBonusMonthlyPrefab;
  private GameObject loginBonusMakeupPrefab;
  private bool doExploreLoginCalc;
  private DateTime exploreSyncTime;

  public bool GoHelp { set; get; }

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("startup000_14", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    ExploreDataManager explore = Singleton<ExploreDataManager>.GetInstance();
    IEnumerator e;
    if (!explore.IsNotRegisteredDeck())
    {
      this.doExploreLoginCalc = true;
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.exploreSyncTime = ServerTime.NowAppTimeAddDelta();
      yield return (object) explore.LoadSuspendData(true);
      explore.StartLoginCalc();
    }
    Future<GameObject> prefabF;
    if (Object.op_Equality((Object) this.loginBonusPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.startup000_14.loginBonus.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loginBonusPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.loginBonusMonthlyPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/startup000_14/loginBonus_monthly_old").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loginBonusMonthlyPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.loginBonusMakeupPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/startup000_14/loginBonus_monthly").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loginBonusMakeupPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    foreach (PlayerLoginBonus loginBonuse in Singleton<NGGameDataManager>.GetInstance().loginBonuses)
    {
      PlayerLoginBonus loginbonus = loginBonuse;
      if (loginbonus.loginbonus.draw_type != LoginbonusDrawType.popup)
      {
        int loginTotalDay = loginbonus.login_days;
        LoginbonusReward loginBonusReward = ((IEnumerable<LoginbonusReward>) MasterData.LoginbonusRewardList).Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.loginbonus_LoginbonusLoginbonus == loginbonus._loginbonus)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<LoginbonusReward>().FirstOrDefault<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.number == loginTotalDay));
        if (loginBonusReward != null && loginBonusReward.character_id > 999)
        {
          UnitUnit[] array = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == loginBonusReward.character_id)).ToArray<UnitUnit>();
          if (array.Length != 0)
          {
            IEnumerator e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) array, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            Debug.LogError((object) ("ログインボーナスID" + (object) loginbonus.loginbonus.ID + "の" + (object) loginTotalDay + "日目のUnitIDの" + (object) loginBonusReward.character_id + "がマスターデータにありません"));
            this.skipList.Add(loginbonus);
          }
        }
      }
    }
    yield return (object) null;
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    this.StartCoroutine(this.LoginBonusLoop((IEnumerable<PlayerLoginBonus>) Singleton<NGGameDataManager>.GetInstance().loginBonuses));
  }

  public override IEnumerator onEndSceneAsync()
  {
    if (this.doExploreLoginCalc)
    {
      this.doExploreLoginCalc = false;
      ExploreDataManager explore = Singleton<ExploreDataManager>.GetInstance();
      while (explore.IsResuming)
        yield return (object) null;
      if (explore.LoginCalcDirty)
      {
        if (!explore.IsRankingAcceptanceFinish)
          yield return (object) explore.ResumeExplore(this.exploreSyncTime);
        explore.LoginReportInfo.SetBeforePlayerStatus();
        bool saveFailed = false;
        yield return (object) explore.SaveSuspendData((Action) (() => saveFailed = true), true);
        if (saveFailed)
          explore.LoginCalcDirty = false;
        else
          explore.LoginReportInfo.SetAfterPlayerStatus();
        explore = (ExploreDataManager) null;
      }
    }
  }

  public IEnumerator LoginBonusLoop(IEnumerable<PlayerLoginBonus> loginBonusList)
  {
    IOrderedEnumerable<PlayerLoginBonus> second1 = loginBonusList.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.monthly_by_day)).OrderBy<PlayerLoginBonus, int>((Func<PlayerLoginBonus, int>) (x => x.loginbonus.ID));
    IOrderedEnumerable<PlayerLoginBonus> second2 = loginBonusList.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.monthly)).OrderBy<PlayerLoginBonus, int>((Func<PlayerLoginBonus, int>) (x => x.loginbonus.ID));
    PlayerLoginBonus[] array = loginBonusList.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type != LoginbonusDrawType.monthly && x.loginbonus.draw_type != LoginbonusDrawType.monthly_by_day && x.loginbonus.draw_type != LoginbonusDrawType.popup)).OrderBy<PlayerLoginBonus, int>((Func<PlayerLoginBonus, int>) (x => x.loginbonus.ID)).Concat<PlayerLoginBonus>((IEnumerable<PlayerLoginBonus>) second2).Concat<PlayerLoginBonus>((IEnumerable<PlayerLoginBonus>) second1).ToArray<PlayerLoginBonus>();
    List<PlayerLoginBonus> removeList = new List<PlayerLoginBonus>();
    PlayerLoginBonus[] playerLoginBonusArray = array;
    for (int index = 0; index < playerLoginBonusArray.Length; ++index)
    {
      PlayerLoginBonus loginBonus = playerLoginBonusArray[index];
      if (!this.skipList.Exists((Predicate<PlayerLoginBonus>) (x => x.loginbonus.ID == loginBonus.loginbonus.ID)))
      {
        GameObject prefab;
        switch (loginBonus.loginbonus.draw_type)
        {
          case LoginbonusDrawType.monthly:
            prefab = this.loginBonusMonthlyPrefab;
            break;
          case LoginbonusDrawType.monthly_by_day:
            prefab = this.loginBonusMakeupPrefab;
            break;
          default:
            prefab = this.loginBonusPrefab;
            break;
        }
        IEnumerator e = this.LoginBonus(prefab, loginBonus);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      removeList.Add(loginBonus);
    }
    playerLoginBonusArray = (PlayerLoginBonus[]) null;
    foreach (PlayerLoginBonus playerLoginBonus in removeList)
      Singleton<NGGameDataManager>.GetInstance().loginBonuses.Remove(playerLoginBonus);
    if (this.GoHelp)
      Help0152Scene.ChangeScene(false, MasterData.HelpCategory[34]);
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      Sea030HomeScene.ChangeScene(false);
    }
    else
      MypageScene.ChangeScene();
  }

  public IEnumerator LoginBonus(GameObject prefab, PlayerLoginBonus loginBonus)
  {
    Startup00014Scene startup00014Scene = this;
    GameObject obj = prefab.Clone(startup00014Scene.middle);
    obj.GetComponent<Startup00014Menu>().Parent = startup00014Scene;
    IEnumerator e = obj.GetComponent<Startup00014Menu>().InitSceneAsync(loginBonus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (Object.op_Inequality((Object) obj, (Object) null))
      yield return (object) null;
  }
}
