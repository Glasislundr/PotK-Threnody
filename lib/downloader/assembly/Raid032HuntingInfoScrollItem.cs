// Decompiled with JetBrains decompiler
// Type: Raid032HuntingInfoScrollItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032HuntingInfoScrollItem : MonoBehaviour
{
  [SerializeField]
  private Transform iconParent;
  [SerializeField]
  private UILabel txt_boss_name;
  [SerializeField]
  private UILabel txt_num_lap;
  [SerializeField]
  private UILabel txt_num_time;
  private GuildRaid guildRaidMaster;
  private int lapNow;

  public IEnumerator InitAsync(
    WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories info,
    GameObject unitNormalIconPrefab)
  {
    PlayerUnit bossUnit = (PlayerUnit) null;
    yield return (object) this.LoadBossUnit(info.quest_s_id, info.loop_count, (Action<PlayerUnit>) (unit => bossUnit = unit));
    if (bossUnit == (PlayerUnit) null)
    {
      Debug.LogError((object) "bossUnit is null.");
    }
    else
    {
      this.txt_boss_name.SetTextLocalize(bossUnit.unit.name);
      this.txt_num_time.SetTextLocalize(string.Format("{0: MM/dd H:mm}", (object) info.defeated_at));
      yield return (object) this.SetSprite(unitNormalIconPrefab, bossUnit);
    }
  }

  private IEnumerator LoadBossUnit(int raid_quest_s_id, int loopCount, Action<PlayerUnit> complete)
  {
    if (!MasterData.GuildRaid.TryGetValue(raid_quest_s_id, out this.guildRaidMaster))
    {
      Debug.LogError((object) ("There is no MasterData in local [ID:" + (object) raid_quest_s_id + "]"));
    }
    else
    {
      yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[this.guildRaidMaster.stage_id]);
      this.lapNow = loopCount;
      int lap = this.guildRaidMaster.lap;
      if (this.lapNow <= lap)
        this.txt_num_lap.SetTextLocalize(this.lapNow);
      else
        this.txt_num_lap.SetTextLocalize("Ex" + (object) (this.lapNow - lap));
      complete(PlayerUnit.FromEnemy(this.guildRaidMaster.getBoss(), raidLoopCount: loopCount, raidID: raid_quest_s_id, isRaidBoss: true));
    }
  }

  private IEnumerator SetSprite(GameObject unitNormalIcon, PlayerUnit unit)
  {
    UnitIcon uniticon = unitNormalIcon.Clone(this.iconParent).GetComponent<UnitIcon>();
    uniticon.setBottom(unit);
    uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    IEnumerator e = uniticon.SetUnit(unit.unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    uniticon.BottomModeValue = UnitIconBase.GetBottomModeLevel(unit.unit, unit);
    uniticon.setLevelText(unit);
    ((Component) uniticon.RarityStar).gameObject.SetActive(false);
  }

  public void IbtnClick() => Raid032MyRankingScene.changeScene(this.guildRaidMaster);
}
