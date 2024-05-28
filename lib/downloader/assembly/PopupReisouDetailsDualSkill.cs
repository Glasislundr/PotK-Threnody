// Decompiled with JetBrains decompiler
// Type: PopupReisouDetailsDualSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopupReisouDetailsDualSkill : PopupReisouDetails
{
  [SerializeField]
  private PopupReisouDetailsDualSkill.ReisouUI holyReisou;
  [SerializeField]
  private PopupReisouDetailsDualSkill.ReisouUI chaosReisou;

  public override IEnumerator Init(
    GameCore.ItemInfo item,
    PlayerItem playerItem = null,
    bool isDispRank = true,
    Action removeCallback = null,
    bool isRemovePossible = false,
    bool isFusionPossible = false,
    bool isDrillingPossible = false,
    PlayerItem customGearBase = null)
  {
    PopupReisouDetailsDualSkill detailsDualSkill = this;
    detailsDualSkill.item = item;
    detailsDualSkill.weaponItem = (PlayerItem) null;
    detailsDualSkill.removeCallback = removeCallback;
    bool limited = customGearBase != (PlayerItem) null;
    if (item.playerItem == (PlayerItem) null)
      limited = true;
    PlayerItem playerItemTemp = !(item.playerItem != (PlayerItem) null) ? playerItem : item.playerItem;
    detailsDualSkill.equipGear = customGearBase;
    if (detailsDualSkill.equipGear == (PlayerItem) null && playerItemTemp.id != 0)
      detailsDualSkill.equipGear = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.equipped_reisou_player_gear_id == playerItemTemp.id && x.isWeapon()));
    GearReisouFusion recipe = playerItemTemp.GetReisouFusionMineRecipe();
    PlayerItem holyReisouItemTemp = new PlayerItem(recipe.holy_ID, MasterDataTable.CommonRewardType.gear);
    PlayerItem chaosReisouItemTemp = new PlayerItem(recipe.chaos_ID, MasterDataTable.CommonRewardType.gear);
    if (isDispRank)
    {
      PlayerMythologyGearStatus mythologyGearStatus = playerItemTemp.GetPlayerMythologyGearStatus();
      holyReisouItemTemp.gear_level = mythologyGearStatus.holy_gear_level;
      holyReisouItemTemp.gear_level_limit = mythologyGearStatus.holy_gear_level_limit;
      holyReisouItemTemp.gear_exp = mythologyGearStatus.holy_gear_exp;
      holyReisouItemTemp.gear_exp_next = mythologyGearStatus.holy_gear_exp_next;
      chaosReisouItemTemp.gear_level = mythologyGearStatus.chaos_gear_level;
      chaosReisouItemTemp.gear_level_limit = mythologyGearStatus.chaos_gear_level_limit;
      chaosReisouItemTemp.gear_exp = mythologyGearStatus.chaos_gear_exp;
      chaosReisouItemTemp.gear_exp_next = mythologyGearStatus.chaos_gear_exp_next;
    }
    Future<GameObject> prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_01").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    detailsDualSkill.prefabReisouSkillDetail01 = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_02").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    detailsDualSkill.prefabReisouSkillDetail02 = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_03").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    detailsDualSkill.prefabReisouSkillDetail03 = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    detailsDualSkill.iconGearKind.Init(item.gear.kind);
    detailsDualSkill.txtReisouName.SetTextLocalize(item.name);
    RarityIcon.SetRarity(item.gear, detailsDualSkill.slcRarityStar);
    detailsDualSkill.setRankUI(holyReisouItemTemp, detailsDualSkill.holyReisou, isDispRank);
    detailsDualSkill.setRankUI(chaosReisouItemTemp, detailsDualSkill.chaosReisou, isDispRank);
    yield return (object) detailsDualSkill.setHolyParamSkillDetail(detailsDualSkill.holyReisou.grid, holyReisouItemTemp);
    yield return (object) detailsDualSkill.setChaosParamSkillDetailForMythology(detailsDualSkill.chaosReisou.grid, recipe.holy_ID, chaosReisouItemTemp);
    yield return (object) detailsDualSkill.setSkillDetail(detailsDualSkill.holyReisou.grid, recipe.holy_ID, holyReisouItemTemp.gear_level);
    yield return (object) detailsDualSkill.setSkillDetail(detailsDualSkill.chaosReisou.grid, recipe.chaos_ID, chaosReisouItemTemp.gear_level);
    if (limited || removeCallback == null || !isRemovePossible)
    {
      ((UIButtonColor) detailsDualSkill.btnRemove).isEnabled = false;
    }
    else
    {
      int id = item.playerItem.id;
      foreach (PlayerItem playerItem1 in SMManager.Get<PlayerItem[]>())
      {
        if (playerItem1.isWeapon() && playerItem1.equipped_reisou_player_gear_id == id)
        {
          detailsDualSkill.weaponItem = playerItem1;
          break;
        }
      }
      ((UIButtonColor) detailsDualSkill.btnRemove).isEnabled = detailsDualSkill.weaponItem != (PlayerItem) null;
    }
    if (limited || !isDrillingPossible)
    {
      if (Object.op_Inequality((Object) detailsDualSkill.btnDrilling, (Object) null))
        ((UIButtonColor) detailsDualSkill.btnDrilling).isEnabled = false;
      if (Object.op_Inequality((Object) detailsDualSkill.btnDrillingDisabled, (Object) null))
        ((UIButtonColor) detailsDualSkill.btnDrillingDisabled).isEnabled = false;
    }
    else
    {
      PlayerMythologyGearStatus mythologyGearStatus = playerItemTemp.GetPlayerMythologyGearStatus();
      bool flag = (mythologyGearStatus.holy_gear_level < mythologyGearStatus.holy_gear_level_limit ? 0 : (mythologyGearStatus.chaos_gear_level >= mythologyGearStatus.chaos_gear_level_limit ? 1 : 0)) == 0 & isDrillingPossible;
      ((UIButtonColor) detailsDualSkill.btnDrilling).isEnabled = flag;
      ((UIButtonColor) detailsDualSkill.btnDrillingDisabled).isEnabled = false;
    }
  }

  private void setRankUI(
    PlayerItem playerItem,
    PopupReisouDetailsDualSkill.ReisouUI reisouUI,
    bool isDispRank)
  {
    if (!isDispRank)
    {
      playerItem.GetReisouRankLimit();
      reisouUI.txtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) 1, (object) this.item.gearLevelLimit));
      ((Component) reisouUI.slcReisouGauge).gameObject.SetActive(false);
    }
    else
    {
      reisouUI.txtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK.F((object) playerItem.gear_level, (object) playerItem.gear_level_limit));
      float num = (float) ((UIWidget) reisouUI.slcReisouGauge).width * ((float) playerItem.gear_exp / (float) (playerItem.gear_exp_next + playerItem.gear_exp));
      if ((double) num == 0.0 || playerItem.gear_exp_next + playerItem.gear_exp == 0)
      {
        ((Component) reisouUI.slcReisouGauge).gameObject.SetActive(false);
      }
      else
      {
        ((Component) reisouUI.slcReisouGauge).gameObject.SetActive(true);
        ((UIWidget) reisouUI.slcReisouGauge).width = (int) num;
      }
    }
  }

  public override void scrollResetPosition()
  {
    this.holyReisou.grid.Reposition();
    this.holyReisou.scrollview.ResetPosition();
    this.chaosReisou.grid.Reposition();
    this.chaosReisou.scrollview.ResetPosition();
    ((Component) this.chaosReisou.scrollview).gameObject.SetActive(false);
    this.chaosReisou.btnTab.SetColor(Color.gray);
  }

  protected IEnumerator setChaosParamSkillDetailForMythology(
    UIGrid _grid,
    GearGear holyReisou,
    PlayerItem playerItem)
  {
    PopupReisouDetailsDualSkill detailsDualSkill = this;
    if (holyReisou != null)
    {
      Queue<string> paramTextQueue = new Queue<string>();
      detailsDualSkill.SetChaosParamSkillStringsForMythology(paramTextQueue, playerItem, playerItem.reisouRankIncr, holyReisou);
      if (paramTextQueue.Count > 0)
      {
        int page = (int) Math.Ceiling((double) paramTextQueue.Count / 3.0);
        for (int i = 0; i < page; ++i)
        {
          List<string> paramTextList = new List<string>();
          for (int index = 0; index < 3 && paramTextQueue.Count > 0; ++index)
            paramTextList.Add(paramTextQueue.Dequeue());
          yield return (object) detailsDualSkill.prefabReisouSkillDetail03.Clone(((Component) _grid).transform).GetComponent<ReisouSkillDetail_02>().Init(detailsDualSkill.chaosReisou.scrollview, paramTextList);
        }
      }
    }
  }

  protected void SetChaosParamSkillStringsForMythology(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    ReisouRankIncr rankIncr,
    GearGear holyReisou)
  {
    if (rankIncr.hp_incremental > 0 && rankIncr.hp_incremental != 100)
    {
      double num = (double) rankIncr.hp_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HP_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.strength_incremental > 0 && rankIncr.strength_incremental != 100)
    {
      double num = (double) rankIncr.strength_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_POWER_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.intelligence_incremental > 0 && rankIncr.intelligence_incremental != 100)
    {
      double num = (double) rankIncr.intelligence_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAGIC_POWER_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.vitality_incremental > 0 && rankIncr.vitality_incremental != 100)
    {
      double num = (double) rankIncr.vitality_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_VITALITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.mind_incremental > 0 && rankIncr.mind_incremental != 100)
    {
      double num = (double) rankIncr.mind_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MIND_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.agility_incremental > 0 && rankIncr.agility_incremental != 100)
    {
      double num = (double) rankIncr.agility_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_AGILITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.dexterity_incremental > 0 && rankIncr.dexterity_incremental != 100)
    {
      double num = (double) rankIncr.dexterity_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_DEXTERITY_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.lucky_incremental > 0 && rankIncr.lucky_incremental != 100)
    {
      double num = (double) rankIncr.lucky_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_LUCK_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num);
      paramTextQueue.Enqueue(str);
    }
    int num1 = 0;
    if (playerItem.gear.attack_type == GearAttackType.physical)
      num1 = rankIncr.power;
    if (num1 > 0 && num1 != 100)
    {
      double num2 = (double) num1 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_POW_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num2);
      paramTextQueue.Enqueue(str);
    }
    int num3 = 0;
    if (playerItem.gear.attack_type == GearAttackType.magic)
      num3 = rankIncr.power;
    if (num3 > 0 && num3 != 100)
    {
      double num4 = (double) num3 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_POW_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num4);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.physical_defense > 0 && rankIncr.physical_defense != 100)
    {
      double num5 = (double) rankIncr.physical_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_DEF_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num5);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.magic_defense > 0 && rankIncr.magic_defense != 100)
    {
      double num6 = (double) rankIncr.magic_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_DEF_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num6);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.hit > 0 && rankIncr.hit != 100)
    {
      double num7 = (double) rankIncr.hit / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HIT_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num7);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.critical > 0 && rankIncr.critical != 100)
    {
      double num8 = (double) rankIncr.critical / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_CRITICAL_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num8);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.evasion <= 0 || rankIncr.evasion == 100)
      return;
    double num9 = (double) rankIncr.evasion / 100.0;
    string str1 = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_EVASION_FOR_MYTHOLOGY.F((object) holyReisou.name, (object) num9);
    paramTextQueue.Enqueue(str1);
  }

  public void onBtnHolyReisou()
  {
    if (this.IsPushAndSet())
      return;
    ((Component) this.holyReisou.scrollview).gameObject.SetActive(true);
    ((Component) this.chaosReisou.scrollview).gameObject.SetActive(false);
    this.holyReisou.btnTab.SetColor(Color.white);
    this.chaosReisou.btnTab.SetColor(Color.gray);
    this.StartCoroutine(this.IsPushOff());
  }

  public void onBtnChaosReisou()
  {
    if (this.IsPushAndSet())
      return;
    ((Component) this.holyReisou.scrollview).gameObject.SetActive(false);
    ((Component) this.chaosReisou.scrollview).gameObject.SetActive(true);
    this.holyReisou.btnTab.SetColor(Color.gray);
    this.chaosReisou.btnTab.SetColor(Color.white);
    this.StartCoroutine(this.IsPushOff());
  }

  [Serializable]
  private struct ReisouUI
  {
    [SerializeField]
    public UILabel txtReisouRank;
    [SerializeField]
    public UISprite slcReisouGauge;
    [SerializeField]
    public UIScrollView scrollview;
    [SerializeField]
    public UIGrid grid;
    [SerializeField]
    public SpreadColorButton btnTab;
  }
}
