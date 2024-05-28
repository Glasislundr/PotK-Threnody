// Decompiled with JetBrains decompiler
// Type: BattleUI05PunitiveExpeditionRewardMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05PunitiveExpeditionRewardMenu : ResultMenuBase
{
  private GameObject rewardReceivePopup;
  [SerializeField]
  private GameObject hantingEvent;
  private bool toNext;
  private int allPlayerPoint;
  private int playerPoint;
  private int[] rewardIds;
  private int[] guildRewardIds;
  private bool isGuild;

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.rewardReceivePopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_002_hunting_reward_receive__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.rewardReceivePopup = prefabF.Result;
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
    this.allPlayerPoint = !this.isGuild ? eventTopInfo.all_player_point : eventTopInfo.guild_point;
    this.playerPoint = eventTopInfo.player_point;
    this.rewardIds = eventTopInfo.get_reward_ids;
    this.guildRewardIds = eventTopInfo.get_guild_reward_ids;
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result, int index)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.toNext = false;
    this.isGuild = result.events[index].IsGuild();
    this.allPlayerPoint = !this.isGuild ? result.events[index].all_player_point : result.events[index].guild_point;
    this.playerPoint = result.events[index].player_point;
    this.rewardIds = result.events[index].get_reward_ids;
    this.guildRewardIds = result.events[index].get_guild_rward_ids;
  }

  public override IEnumerator Run()
  {
    BattleUI05PunitiveExpeditionRewardMenu expeditionRewardMenu = this;
    expeditionRewardMenu.hantingEvent.SetActive(false);
    GameObject popup = expeditionRewardMenu.rewardReceivePopup.Clone();
    Popup002HuntingRewardReceiveMenu script = popup.GetComponent<Popup002HuntingRewardReceiveMenu>();
    popup.SetActive(false);
    IEnumerator e = script.Init(expeditionRewardMenu.allPlayerPoint, expeditionRewardMenu.playerPoint, expeditionRewardMenu.rewardIds, expeditionRewardMenu.guildRewardIds, expeditionRewardMenu.isGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    script.ResetScrollPosition();
    // ISSUE: reference to a compiler-generated method
    script.SetTapCallBack(new Action(expeditionRewardMenu.\u003CRun\u003Eb__11_0));
    while (!expeditionRewardMenu.toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.onTapToNext();
      }
      yield return (object) null;
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
