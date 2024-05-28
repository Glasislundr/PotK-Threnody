// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultRewardPopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032BattleResultRewardPopupMenu : MonoBehaviour, IRaidResultMenu
{
  private GameObject rewardPopupPrefab;
  private RaidDamageRewardPopupSequence damageRewardPopupSeq;
  private RaidKillRewardPopupSequence killRewardPopupSeq;
  private WebAPI.Response.GuildraidBattleFinish response;

  public bool isSkip { get; set; }

  public IEnumerator Init(
    GuildRaid masterData,
    BattleInfo info,
    WebAPI.Response.GuildraidBattleFinish response,
    PlayerUnit bossUnit)
  {
    this.response = response;
    if (response.damage_rewards != null && response.damage_rewards.Length != 0 && !response.already_defeated)
    {
      this.damageRewardPopupSeq = new RaidDamageRewardPopupSequence();
      yield return (object) this.damageRewardPopupSeq.Init(response.damage_rewards);
    }
    if (response.defeat_rewards != null && response.defeat_rewards.Length != 0 && !response.already_defeated)
    {
      this.killRewardPopupSeq = new RaidKillRewardPopupSequence();
      yield return (object) this.killRewardPopupSeq.Init(masterData.ID, response.defeat_rewards);
    }
  }

  public IEnumerator Run()
  {
    if (this.damageRewardPopupSeq != null)
      yield return (object) this.damageRewardPopupSeq.Run();
    if (this.killRewardPopupSeq != null)
      yield return (object) this.killRewardPopupSeq.Run();
  }

  public IEnumerator OnFinish()
  {
    yield break;
  }
}
