// Decompiled with JetBrains decompiler
// Type: BattleUI05PunitiveExpeditionNextRewardMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05PunitiveExpeditionNextRewardMenu : ResultMenuBase
{
  private GameObject nextRewardPopup;
  private bool toNext;
  private int guildPoint;
  private int allPlayerPoint;
  private int playerPoint;
  private int periodId;
  private bool isGuild;

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.nextRewardPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_002_hunting_next_reward__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.nextRewardPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Init(WebAPI.Response.EventTop eventTopInfo)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.toNext = false;
    this.isGuild = eventTopInfo.IsGuild();
    this.guildPoint = eventTopInfo.guild_point;
    this.allPlayerPoint = eventTopInfo.all_player_point;
    this.playerPoint = eventTopInfo.player_point;
    this.periodId = eventTopInfo.period_id;
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result, int index)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.toNext = false;
    this.isGuild = result.events[index].IsGuild();
    this.guildPoint = result.events[index].guild_point;
    this.allPlayerPoint = result.events[index].all_player_point;
    this.playerPoint = result.events[index].player_point;
    this.periodId = result.events[index].period_id;
  }

  public override IEnumerator Run()
  {
    BattleUI05PunitiveExpeditionNextRewardMenu expeditionNextRewardMenu = this;
    GameObject popup = expeditionNextRewardMenu.nextRewardPopup.Clone();
    Popup002HuntingNextRewardReceiveMenu script = popup.GetComponent<Popup002HuntingNextRewardReceiveMenu>();
    popup.SetActive(false);
    IEnumerator e = script.Init(expeditionNextRewardMenu.periodId, expeditionNextRewardMenu.allPlayerPoint, expeditionNextRewardMenu.playerPoint, expeditionNextRewardMenu.guildPoint, expeditionNextRewardMenu.isGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    script.ResetScrollPosition();
    // ISSUE: reference to a compiler-generated method
    script.SetTapCallBack(new Action(expeditionNextRewardMenu.\u003CRun\u003Eb__10_0));
    while (!expeditionNextRewardMenu.toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.onTapToNext();
      }
      yield return (object) null;
    }
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
