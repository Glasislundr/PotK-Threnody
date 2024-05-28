// Decompiled with JetBrains decompiler
// Type: Raid032BattleResultBossStatus
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
public class Raid032BattleResultBossStatus : MonoBehaviour
{
  [SerializeField]
  private UILabel raidBossLv;
  [SerializeField]
  private UILabel raidBossName;
  [SerializeField]
  private UILabel nowHpLabel;
  [SerializeField]
  private UILabel maxHpLabel;
  [SerializeField]
  private NGTweenGaugeScale hpGauge;
  [SerializeField]
  private NGTweenGaugeScale delayedHpGauge;
  [SerializeField]
  private CreateIconObject killRewardAnchor;
  [SerializeField]
  private CreateIconObject[] damageRewardAnchors;
  [SerializeField]
  private GameObject[] damageRewardBase;
  private bool isSimulation;
  private int maxHp;
  private int nowHp;
  private bool isEndless;
  private bool isGaugeAnimating;
  private List<Raid032BattleResultBossStatus.RewardIconEntity> rewardIconEntities;
  private const int PER_TEN_THOUDSAND_MAX = 10000;

  public bool isSurvive { get; private set; }

  public IEnumerator Init(
    GuildRaid masterData,
    BattleInfo info,
    WebAPI.Response.GuildraidBattleFinish result,
    PlayerUnit bossUnit)
  {
    this.isSimulation = info.isSimulation;
    this.isEndless = false;
    int num1 = 0;
    KeyValuePair<int, GuildRaid> keyValuePair = MasterData.GuildRaid.Where<KeyValuePair<int, GuildRaid>>((Func<KeyValuePair<int, GuildRaid>, bool>) (x => x.Value.period_id == masterData.period_id)).OrderByDescending<KeyValuePair<int, GuildRaid>, int>((Func<KeyValuePair<int, GuildRaid>, int>) (x => x.Value.lap)).FirstOrDefault<KeyValuePair<int, GuildRaid>>();
    if (keyValuePair.Value != null)
      num1 = keyValuePair.Value.lap;
    this.isEndless = result.loop_count > num1;
    this.raidBossLv.SetTextLocalize(bossUnit.level);
    this.raidBossName.SetTextLocalize(bossUnit.unit.name);
    this.maxHp = bossUnit.total_hp;
    this.nowHp = this.maxHp - result.boss_total_damage;
    int num2 = Mathf.Min(this.maxHp, this.nowHp + result.boss_damage);
    this.nowHp = Mathf.Max(this.nowHp, 0);
    this.SetHp(this.maxHp, num2);
    this.delayedHpGauge.setValue(num2, this.maxHp, false);
    this.isSurvive = this.nowHp > 0;
    this.rewardIconEntities = new List<Raid032BattleResultBossStatus.RewardIconEntity>();
    int boss_total_hp_ratio_before = result.already_defeated ? this.nowHp : num2;
    boss_total_hp_ratio_before = (int) ((double) boss_total_hp_ratio_before / (double) this.maxHp * 10000.0);
    yield return (object) this.SetDamageRewards(masterData.getDamageRewardsList(), masterData.getDamageRewardRatiosList(), boss_total_hp_ratio_before);
    if (!this.isEndless)
      yield return (object) this.SetKillRewards(masterData.getKillRewardsList(), masterData.getDamageRewardRatiosList(), boss_total_hp_ratio_before);
    else
      yield return (object) this.SetKillRewards(masterData.getRaidEndlessKillRewardsList(), masterData.getDamageRewardRatiosList(), boss_total_hp_ratio_before);
  }

  public IEnumerator AnimateHpGauge()
  {
    float duration = 1f;
    this.isGaugeAnimating = true;
    this.hpGauge.setValue(this.nowHp, this.maxHp, delay: 0.0f, duration: duration);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.SetHp(this.maxHp, this.nowHp);
    this.delayedHpGauge.setValue(this.nowHp, this.maxHp, delay: 0.0f, duration: duration);
    this.isGaugeAnimating = false;
  }

  public void SetAfterHpAndGaugeImmediate()
  {
    this.SetHp(this.maxHp, this.nowHp);
    this.hpGauge.setValue(this.nowHp, this.maxHp, false);
    this.delayedHpGauge.setValue(this.nowHp, this.maxHp, false);
  }

  private IEnumerator SetDamageRewards(
    List<GuildRaid.RaidReward> rewards,
    List<int> damageRatios,
    int hpRatio)
  {
    for (int index = 0; index < this.damageRewardBase.Length; ++index)
    {
      this.damageRewardBase[index].SetActive(!this.isEndless);
      ((Component) this.damageRewardAnchors[index]).gameObject.SetActive(!this.isEndless);
    }
    for (int i = 0; i < this.damageRewardAnchors.Length; ++i)
    {
      MasterDataTable.CommonRewardType type = rewards[i].Type;
      int id = rewards[i].Id;
      int quantity = rewards[i].Quantity;
      bool isGotten = damageRatios[i] <= 10000 - hpRatio;
      yield return (object) this.damageRewardAnchors[i].CreateThumbnail(type, id, quantity, false, false);
      IconPrefabBase component = this.damageRewardAnchors[i].GetIcon().GetComponent<IconPrefabBase>();
      component.Gray = isGotten;
      GameObject gameObject = ((Component) ((Component) this.damageRewardAnchors[i]).transform.Find("slc_Get")).gameObject;
      if (Object.op_Inequality((Object) gameObject, (Object) null))
        gameObject.SetActive(isGotten);
      this.rewardIconEntities.Add(new Raid032BattleResultBossStatus.RewardIconEntity(component, damageRatios[i], gameObject));
    }
  }

  private IEnumerator SetKillRewards(
    List<GuildRaid.RaidReward> rewards,
    List<int> damageRatios,
    int hpRatio)
  {
    bool isGotten = 10000 <= 10000 - hpRatio;
    if (rewards.Count > 1)
    {
      yield return (object) this.killRewardAnchor.CreateThumbnail(MasterDataTable.CommonRewardType.emblem, 0, visibleBottom: false, isButton: false);
    }
    else
    {
      GuildRaid.RaidReward raidReward = rewards.First<GuildRaid.RaidReward>();
      yield return (object) this.killRewardAnchor.CreateThumbnail(raidReward.Type, raidReward.Id, visibleBottom: false, isButton: false);
    }
    IconPrefabBase component = this.killRewardAnchor.GetIcon().GetComponent<IconPrefabBase>();
    component.Gray = isGotten;
    GameObject gameObject = ((Component) ((Component) this.killRewardAnchor).transform.Find("slc_Get")).gameObject;
    if (Object.op_Inequality((Object) gameObject, (Object) null))
      gameObject.SetActive(isGotten);
    this.rewardIconEntities.Add(new Raid032BattleResultBossStatus.RewardIconEntity(component, 10000, gameObject));
  }

  private void Update() => this.UpdateIfGaugeAnimating();

  private void UpdateIfGaugeAnimating()
  {
    if (!this.isGaugeAnimating)
      return;
    this.nowHpLabel.SetTextLocalize((int) ((double) ((Component) this.hpGauge).transform.localScale.x * (double) this.maxHp));
    if (this.isSimulation)
      return;
    for (int index = 0; index < this.rewardIconEntities.Count; ++index)
    {
      int animatingHpRatio = (int) ((double) ((Component) this.hpGauge).transform.localScale.x * 10000.0);
      this.rewardIconEntities[index].UpdateGotten(animatingHpRatio);
    }
  }

  private void SetHp(int max, int now)
  {
    this.maxHpLabel.SetTextLocalize("/" + max.ToString());
    this.nowHpLabel.SetTextLocalize(now.ToString());
    this.hpGauge.setValue(now, max, false);
  }

  private class RewardIconEntity
  {
    private IconPrefabBase iconPrefabBase;
    private int damageRatio;
    private GameObject marker;

    public RewardIconEntity(IconPrefabBase iconPrefabBase, int damageRatio, GameObject marker)
    {
      this.iconPrefabBase = iconPrefabBase;
      this.damageRatio = damageRatio;
      this.marker = marker;
    }

    public void UpdateGotten(int animatingHpRatio)
    {
      if (this.iconPrefabBase.Gray)
        return;
      bool flag = this.damageRatio <= 10000 - animatingHpRatio;
      this.iconPrefabBase.Gray = flag;
      this.marker.SetActive(flag);
    }
  }
}
