// Decompiled with JetBrains decompiler
// Type: Startup00014Monthly
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
public class Startup00014Monthly : Startup00014Renzoku
{
  protected override void Init()
  {
    this.DestroyTrash();
    this.rewardReset = false;
    this.rewardLast = false;
    this.loginTotalDay = this.loginBonus.login_days;
    this.loginBonusRewardList = ((IEnumerable<LoginbonusReward>) MasterData.LoginbonusRewardList).Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.loginbonus == this.loginBonus.loginbonus)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<LoginbonusReward>();
    this.loginBonusReward = this.loginBonusRewardList.Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.number == this.loginTotalDay)).First<LoginbonusReward>();
    this.maxRewardValue = this.loginBonusRewardList.Count;
    this.unitID = this.loginBonusReward.character_id;
    this.story = this.unitID < 100000;
    if (this.story)
    {
      this.charaID = this.unitID;
    }
    else
    {
      this.unitData = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == this.unitID)).First<UnitUnit>();
      this.charaID = this.unitData.character.ID;
    }
    this.drawIconValue = this.loginBonus.loginbonus.draw_reward_num;
    this.listIcons = this.positionList[0].positionList;
    this.stampValue = this.loginTotalDay % this.drawIconValue;
    if (this.stampValue == 0)
      this.stampValue = this.drawIconValue;
    if (this.stampValue != 0)
      return;
    this.stampValue = 1;
  }

  public override IEnumerator InitSceneAsync(PlayerLoginBonus LB)
  {
    Startup00014Monthly startup00014Monthly = this;
    startup00014Monthly.GetNextMaskTop.SetActive(false);
    startup00014Monthly.GetNextMaskLeft.SetActive(false);
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    startup00014Monthly.next.SetActive(false);
    startup00014Monthly.loginBonus = LB;
    startup00014Monthly.Init();
    IEnumerator e = startup00014Monthly.LoadResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.InitNaviChara();
    startup00014Monthly.naviChara.GetComponent<UIWidget>().depth = 100;
    // ISSUE: reference to a compiler-generated method
    int receiveCount = startup00014Monthly.loginBonusRewardList.Where<LoginbonusReward>(new Func<LoginbonusReward, bool>(startup00014Monthly.\u003CInitSceneAsync\u003Eb__1_0)).ToList<LoginbonusReward>().Count;
    int num1 = startup00014Monthly.maxRewardValue / startup00014Monthly.drawIconValue;
    int num2 = receiveCount / startup00014Monthly.drawIconValue;
    int startIndex = num2 * startup00014Monthly.drawIconValue;
    List<int> number = startup00014Monthly.loginBonusRewardList.Select<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToList<int>();
    if (receiveCount % startup00014Monthly.drawIconValue == 0 && receiveCount != 0)
    {
      int index = startIndex;
      startup00014Monthly.rewardReset = true;
      startIndex -= startup00014Monthly.drawIconValue;
      if (num2 % num1 == 0 && num2 != 0)
      {
        index = 0;
        if (!startup00014Monthly.loginBonus.loginbonus.is_loop)
          startup00014Monthly.rewardLast = true;
      }
      if (!startup00014Monthly.rewardLast)
      {
        e = startup00014Monthly.newItemList.Initialize(startup00014Monthly.listIcons, startup00014Monthly.loginBonusRewardList, number[index]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        startup00014Monthly.newItemList.ListEnable(false);
      }
    }
    e = startup00014Monthly.itemList.Initialize(startup00014Monthly.listIcons, startup00014Monthly.loginBonusRewardList, number[startIndex]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.SetRewardObject(receiveCount);
    startup00014Monthly.TxtNavi.SetTextLocalize(startup00014Monthly.loginBonusReward.reward_message);
    startup00014Monthly.OldStamp();
    yield return (object) new WaitForSeconds(0.1f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    ((Component) startup00014Monthly).gameObject.SetActive(true);
    startup00014Monthly.GetNextMaskTop.SetActive(true);
    startup00014Monthly.GetNextMaskLeft.SetActive(true);
    startup00014Monthly.fade.onFinished = new List<EventDelegate>();
    startup00014Monthly.fade.AddOnFinished(new EventDelegate.Callback(((Startup00014Renzoku) startup00014Monthly).AnimationStart));
    startup00014Monthly.fade.PlayForward();
  }

  protected override IEnumerator LoadResource()
  {
    Startup00014Monthly startup00014Monthly = this;
    Future<GameObject> animeF = Res.Animations.longin_bonus.LoginAnimationRoot.Load<GameObject>();
    IEnumerator e = animeF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.loginAnime = animeF.Result;
    Future<GameObject> finishF = Res.Prefabs.startup000_14.slc_Stamp_Finished.Load<GameObject>();
    e = finishF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.finishStamp = finishF.Result;
    Future<GameObject> todayF = Res.Prefabs.startup000_14.slc_Stamp_Today.Load<GameObject>();
    e = todayF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.todayStamp = todayF.Result;
    Future<GameObject> stampFrameF = Res.Prefabs.startup000_14.stampFrame.Load<GameObject>();
    e = stampFrameF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.stampFrame = stampFrameF.Result;
    Future<GameObject> naviF = (Future<GameObject>) null;
    Future<Sprite> spriteF = (Future<Sprite>) null;
    if (startup00014Monthly.story)
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMobUnit(startup00014Monthly.charaID), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = MobUnits.LoadStory(startup00014Monthly.charaID);
      spriteF = MobUnits.LoadSpriteLarge(startup00014Monthly.charaID);
    }
    else
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) new List<UnitUnit>()
      {
        startup00014Monthly.unitData
      }, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = startup00014Monthly.unitData.LoadStory();
      spriteF = startup00014Monthly.unitData.LoadSpriteLarge();
    }
    e = naviF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.naviChara = naviF.Result.Clone(((Component) startup00014Monthly.charaContainer).transform);
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.mainSprite = spriteF.Result;
    Future<Sprite> futureC = Res.GUI._009_3_sozai.mask_Chara_C.Load<Sprite>();
    e = futureC.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    startup00014Monthly.maskTexture = futureC.Result.texture;
  }
}
