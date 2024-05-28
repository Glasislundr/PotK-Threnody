// Decompiled with JetBrains decompiler
// Type: PopupReisouDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class PopupReisouDetails : BackButtonPopupBase
{
  [SerializeField]
  protected GearKindIcon iconGearKind;
  [SerializeField]
  protected UILabel txtReisouName;
  [SerializeField]
  protected UI2DSprite slcRarityStar;
  [SerializeField]
  private UILabel txtReisouRank;
  [SerializeField]
  private UISprite slcReisouGauge;
  [SerializeField]
  private GameObject slcReisouGaugeBase;
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  protected SpreadColorButton btnRemove;
  [SerializeField]
  protected SpreadColorButton btnDrilling;
  [SerializeField]
  protected UIButton btnDrillingDisabled;
  [SerializeField]
  private SpreadColorButton btnFusion;
  protected GameObject prefabReisouSkillDetail01;
  protected GameObject prefabReisouSkillDetail02;
  protected GameObject prefabReisouSkillDetail03;
  protected GameObject reisouRemovePopupPrefab;
  protected GameObject prefabPopupReisouTargetWeapon;
  protected const int holyParamSkillDispCntMax = 12;
  protected const int chaosParamSkillDispCntMax = 3;
  protected Action removeCallback;
  protected GameCore.ItemInfo item;
  protected PlayerItem weaponItem;
  protected PlayerItem equipGear;

  public virtual IEnumerator Init(
    GameCore.ItemInfo item,
    PlayerItem playerItemDummy = null,
    bool isDispRank = true,
    Action removeCallback = null,
    bool isRemovePossible = false,
    bool isFusionPossible = false,
    bool isDrillingPossible = false,
    PlayerItem customGearBase = null)
  {
    this.item = item;
    this.weaponItem = (PlayerItem) null;
    this.removeCallback = removeCallback;
    bool limited = customGearBase != (PlayerItem) null;
    if (item.playerItem == (PlayerItem) null)
      limited = true;
    PlayerItem playerItemTemp = playerItemDummy;
    if (playerItemTemp == (PlayerItem) null)
      playerItemTemp = item.playerItem;
    this.equipGear = customGearBase;
    if (this.equipGear == (PlayerItem) null && playerItemTemp.id != 0)
      this.equipGear = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.equipped_reisou_player_gear_id == playerItemTemp.id && x.isWeapon()));
    Future<GameObject> prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_01").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.prefabReisouSkillDetail01 = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    if (item.gear.isHolyReisou())
    {
      prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_02").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabReisouSkillDetail02 = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (item.gear.isChaosReisou())
    {
      prefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouSkillDetail_03").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabReisouSkillDetail03 = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    this.iconGearKind.Init(item.gear.kind);
    this.txtReisouName.SetTextLocalize(item.name);
    RarityIcon.SetRarity(item.gear, this.slcRarityStar);
    if (!isDispRank)
    {
      if (playerItemTemp != (PlayerItem) null)
      {
        int reisouRankLimit = playerItemTemp.GetReisouRankLimit();
        this.txtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK_FOR_DETAILS.F((object) 1, (object) reisouRankLimit));
      }
      else
        ((Component) this.txtReisouRank).gameObject.SetActive(false);
      ((Component) this.slcReisouGauge).gameObject.SetActive(false);
    }
    else
    {
      this.txtReisouRank.SetTextLocalize(Consts.GetInstance().UNIT_00443_REISOU_RANK_FOR_DETAILS.F((object) playerItemTemp.gear_level, (object) playerItemTemp.gear_level_limit));
      float num = (float) ((UIWidget) this.slcReisouGauge).width * ((float) playerItemTemp.gear_exp / (float) (playerItemTemp.gear_exp_next + playerItemTemp.gear_exp));
      if ((double) num == 0.0 || playerItemTemp.gear_exp_next + playerItemTemp.gear_exp == 0)
      {
        ((Component) this.slcReisouGauge).gameObject.SetActive(false);
      }
      else
      {
        ((Component) this.slcReisouGauge).gameObject.SetActive(true);
        ((UIWidget) this.slcReisouGauge).width = (int) num;
      }
    }
    if (item.gear.isHolyReisou())
      yield return (object) this.setHolyParamSkillDetail(this.grid, playerItemTemp);
    else if (item.gear.isChaosReisou())
      yield return (object) this.setChaosParamSkillDetail(this.grid, playerItemTemp);
    yield return (object) this.setSkillDetail(this.grid, item.gear, item.gearLevel);
    if (limited)
    {
      ((UIButtonColor) this.btnRemove).isEnabled = false;
      if (Object.op_Inequality((Object) this.btnDrilling, (Object) null))
      {
        ((UIButtonColor) this.btnDrilling).isEnabled = false;
        ((Collider) ((Component) this.btnDrilling).GetComponent<BoxCollider>()).enabled = false;
      }
      if (Object.op_Inequality((Object) this.btnDrillingDisabled, (Object) null))
        ((UIButtonColor) this.btnDrillingDisabled).isEnabled = false;
      if (Object.op_Inequality((Object) this.btnFusion, (Object) null))
        ((UIButtonColor) this.btnFusion).isEnabled = false;
    }
    else
    {
      if (Object.op_Inequality((Object) this.btnDrilling, (Object) null))
      {
        playerItemTemp.isLevelMax();
        int num = isDrillingPossible ? 1 : 0;
        if (Object.op_Inequality((Object) this.btnDrilling, (Object) null))
        {
          ((UIButtonColor) this.btnDrilling).isEnabled = !playerItemTemp.isLevelMax() & isDrillingPossible;
          ((Collider) ((Component) this.btnDrilling).GetComponent<BoxCollider>()).enabled = !playerItemTemp.isLevelMax() & isDrillingPossible;
        }
        if (Object.op_Inequality((Object) this.btnDrillingDisabled, (Object) null))
          ((UIButtonColor) this.btnDrillingDisabled).isEnabled = false;
      }
      if (removeCallback == null)
      {
        ((UIButtonColor) this.btnRemove).isEnabled = false;
        if (Object.op_Inequality((Object) this.btnFusion, (Object) null))
        {
          if (isFusionPossible)
            ((UIButtonColor) this.btnFusion).isEnabled = item.playerItem.isReisouFusionPossible(SMManager.Get<PlayerItem[]>());
          else
            ((UIButtonColor) this.btnFusion).isEnabled = false;
        }
      }
      else
      {
        int id = item.playerItem.id;
        foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
        {
          if (playerItem.isWeapon() && playerItem.equipped_reisou_player_gear_id == id)
          {
            this.weaponItem = playerItem;
            break;
          }
        }
        ((UIButtonColor) this.btnRemove).isEnabled = this.weaponItem != (PlayerItem) null & isRemovePossible;
        if (Object.op_Inequality((Object) this.btnFusion, (Object) null))
        {
          if (isFusionPossible)
            ((UIButtonColor) this.btnFusion).isEnabled = item.playerItem.isReisouFusionPossible(SMManager.Get<PlayerItem[]>());
          else
            ((UIButtonColor) this.btnFusion).isEnabled = false;
        }
      }
    }
  }

  protected IEnumerator setHolyParamSkillDetail(UIGrid _grid, PlayerItem playerItem)
  {
    Judgement.GearParameter gearParam = Judgement.GearParameter.FromPlayerGear(playerItem);
    Queue<string> paramTextQueue = new Queue<string>();
    this.SetHolyParamSkillStrings(paramTextQueue, playerItem, gearParam);
    if (paramTextQueue.Count > 0)
    {
      int page = paramTextQueue.Count / 12 + 1;
      for (int i = 0; i < page; ++i)
      {
        List<string> paramTextList = new List<string>();
        for (int index = 0; index < 12 && paramTextQueue.Count > 0; ++index)
          paramTextList.Add(paramTextQueue.Dequeue());
        yield return (object) this.prefabReisouSkillDetail02.Clone(((Component) _grid).transform).GetComponent<ReisouSkillDetail_02>().Init(this.scrollview, paramTextList);
      }
    }
  }

  protected void SetHolyParamSkillStrings(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    Judgement.GearParameter gearParam)
  {
    if (gearParam.Hp > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_HP.F((object) gearParam.Hp);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Strength > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_POWER.F((object) gearParam.Strength);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Intelligence > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAGIC_POWER.F((object) gearParam.Intelligence);
      paramTextQueue.Enqueue(str);
    }
    int vitalityIncremental = playerItem.vitality_incremental;
    if (vitalityIncremental > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_VITALITY.F((object) vitalityIncremental);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Mind > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MIND.F((object) gearParam.Mind);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Agility > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_AGILITY.F((object) gearParam.Agility);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Dexterity > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_DEXTERITY.F((object) gearParam.Dexterity);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Luck > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_LUCK.F((object) gearParam.Luck);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.PhysicalPower > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_PHY_POW.F((object) gearParam.PhysicalPower);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.MagicalPower > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAG_POW.F((object) gearParam.MagicalPower);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.PhysicalDefense > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_PHY_DEF.F((object) gearParam.PhysicalDefense);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.MagicDefense > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_MAG_DEF.F((object) gearParam.MagicDefense);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Hit > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_HIT.F((object) gearParam.Hit);
      paramTextQueue.Enqueue(str);
    }
    if (gearParam.Critical > 0)
    {
      string str = Consts.GetInstance().UNIT_0044_HOLY_REISOU_CRITICAL.F((object) gearParam.Critical);
      paramTextQueue.Enqueue(str);
    }
    int evasion = playerItem.evasion;
    if (evasion <= 0)
      return;
    string str1 = Consts.GetInstance().UNIT_0044_HOLY_REISOU_EVASION.F((object) evasion);
    paramTextQueue.Enqueue(str1);
  }

  protected IEnumerator setChaosParamSkillDetail(UIGrid _grid, PlayerItem playerItem)
  {
    Queue<string> paramTextQueue = new Queue<string>();
    this.SetChaosParamSkillStrings(paramTextQueue, playerItem, playerItem.reisouRankIncr);
    if (paramTextQueue.Count > 0)
    {
      int page = (int) Math.Ceiling((double) paramTextQueue.Count / 3.0);
      for (int i = 0; i < page; ++i)
      {
        List<string> paramTextList = new List<string>();
        for (int index = 0; index < 3 && paramTextQueue.Count > 0; ++index)
          paramTextList.Add(paramTextQueue.Dequeue());
        yield return (object) this.prefabReisouSkillDetail03.Clone(((Component) _grid).transform).GetComponent<ReisouSkillDetail_02>().Init(this.scrollview, paramTextList);
      }
    }
  }

  protected void SetChaosParamSkillStrings(
    Queue<string> paramTextQueue,
    PlayerItem playerItem,
    ReisouRankIncr rankIncr)
  {
    if (rankIncr.hp_incremental > 0 && rankIncr.hp_incremental != 100)
    {
      double num = (double) rankIncr.hp_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HP.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.strength_incremental > 0 && rankIncr.strength_incremental != 100)
    {
      double num = (double) rankIncr.strength_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_POWER.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.intelligence_incremental > 0 && rankIncr.intelligence_incremental != 100)
    {
      double num = (double) rankIncr.intelligence_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAGIC_POWER.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.vitality_incremental > 0 && rankIncr.vitality_incremental != 100)
    {
      double num = (double) rankIncr.vitality_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_VITALITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.mind_incremental > 0 && rankIncr.mind_incremental != 100)
    {
      double num = (double) rankIncr.mind_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MIND.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.agility_incremental > 0 && rankIncr.agility_incremental != 100)
    {
      double num = (double) rankIncr.agility_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_AGILITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.dexterity_incremental > 0 && rankIncr.dexterity_incremental != 100)
    {
      double num = (double) rankIncr.dexterity_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_DEXTERITY.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.lucky_incremental > 0 && rankIncr.lucky_incremental != 100)
    {
      double num = (double) rankIncr.lucky_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_LUCK.F((object) num);
      paramTextQueue.Enqueue(str);
    }
    int num1 = 0;
    if (playerItem.gear.attack_type == GearAttackType.physical)
      num1 = rankIncr.power;
    if (num1 > 0 && num1 != 100)
    {
      double num2 = (double) num1 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_POW.F((object) num2);
      paramTextQueue.Enqueue(str);
    }
    int num3 = 0;
    if (playerItem.gear.attack_type == GearAttackType.magic)
      num3 = rankIncr.power;
    if (num3 > 0 && num3 != 100)
    {
      double num4 = (double) num3 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_POW.F((object) num4);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.physical_defense > 0 && rankIncr.physical_defense != 100)
    {
      double num5 = (double) rankIncr.physical_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_DEF.F((object) num5);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.magic_defense > 0 && rankIncr.magic_defense != 100)
    {
      double num6 = (double) rankIncr.magic_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_DEF.F((object) num6);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.hit > 0 && rankIncr.hit != 100)
    {
      double num7 = (double) rankIncr.hit / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HIT.F((object) num7);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.critical > 0 && rankIncr.critical != 100)
    {
      double num8 = (double) rankIncr.critical / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_CRITICAL.F((object) num8);
      paramTextQueue.Enqueue(str);
    }
    if (rankIncr.evasion <= 0 || rankIncr.evasion == 100)
      return;
    double num9 = (double) rankIncr.evasion / 100.0;
    string str1 = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_EVASION.F((object) num9);
    paramTextQueue.Enqueue(str1);
  }

  protected IEnumerator setSkillDetail(UIGrid _grid, GearGear gear, int gearLevel)
  {
    PopupReisouDetails basePopup = this;
    List<PopupSkillDetails.Param> objList = new List<PopupSkillDetails.Param>();
    List<GearReisouSkill> skillList = new List<GearReisouSkill>();
    List<GearReisouSkill> releaseSkillList = new List<GearReisouSkill>();
    foreach (List<GearReisouSkill> rememberReisouSkill in gear.rememberReisouSkills)
    {
      foreach (GearReisouSkill gearReisouSkill in rememberReisouSkill)
      {
        skillList.Add(gearReisouSkill);
        PopupSkillDetails.Param obj = new PopupSkillDetails.Param(gearReisouSkill.skill, UnitParameter.SkillGroup.Reisou, new int?(gearReisouSkill.skill_level));
        objList.Add(obj);
        bool flag = false;
        if (gearReisouSkill.awake_weapon_group != 0)
        {
          if (!(basePopup.equipGear == (PlayerItem) null) && PlayerItem.isReisouSkillAwakeWeaponGroup(basePopup.equipGear.entity_id, gearReisouSkill.awake_weapon_group))
            flag = true;
          else
            continue;
        }
        if (gearReisouSkill.release_rank <= gearLevel && gearReisouSkill.awake_weapon_group == 0 | flag)
          releaseSkillList.Add(gearReisouSkill);
      }
    }
    PopupSkillDetails.Param[] skillParamsArray = objList.ToArray();
    for (int i = 0; i < skillList.Count; ++i)
    {
      GearReisouSkill skill = skillList[i];
      bool is_release = false;
      foreach (GearReisouSkill gearReisouSkill in releaseSkillList)
      {
        if (gearReisouSkill.ID == skill.ID)
        {
          is_release = true;
          break;
        }
      }
      yield return (object) basePopup.prefabReisouSkillDetail01.Clone(((Component) _grid).transform).GetComponent<ReisouSkillDetail_01>().Init(basePopup.scrollview, skill, gearLevel, skillParamsArray, is_release, basePopup.equipGear, new Action<GearReisouSkill>(basePopup.OpenPopupReisouTargetWeapon), (BackButtonMenuBase) basePopup);
    }
  }

  public virtual void scrollResetPosition()
  {
    this.grid.Reposition();
    this.scrollview.ResetPosition();
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onBtnRemove()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenReisouRemovePopupAsync());
  }

  public IEnumerator OpenReisouRemovePopupAsync()
  {
    PopupReisouDetails popupReisouDetails = this;
    IEnumerator e;
    if (Object.op_Equality((Object) popupReisouDetails.reisouRemovePopupPrefab, (Object) null))
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_remove").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popupReisouDetails.reisouRemovePopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    GameObject popup = popupReisouDetails.reisouRemovePopupPrefab.Clone();
    PopupReisouRemove component = popup.GetComponent<PopupReisouRemove>();
    popup.SetActive(false);
    e = component.Init(popupReisouDetails.weaponItem, popupReisouDetails.removeCallback);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    yield return (object) null;
    popupReisouDetails.StartCoroutine(popupReisouDetails.IsPushOff());
  }

  public void onBtnDrilling()
  {
    if (this.IsPushAndSet())
      return;
    ReisouRenseiScene.changeScene(true, this.item.playerItem);
    Singleton<NGGameDataManager>.GetInstance().isReisouScene = true;
  }

  public void onBtnDrillingDisabled()
  {
  }

  public void OpenPopupReisouTargetWeapon(GearReisouSkill skill)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenPopupReisouTargetWeaponAsync(skill));
  }

  public IEnumerator OpenPopupReisouTargetWeaponAsync(GearReisouSkill skill)
  {
    PopupReisouDetails basePopup = this;
    IEnumerator e;
    if (Object.op_Equality((Object) basePopup.prefabPopupReisouTargetWeapon, (Object) null))
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_target_weapon").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      basePopup.prefabPopupReisouTargetWeapon = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    GameObject popup = basePopup.prefabPopupReisouTargetWeapon.Clone();
    PopupReisouTargetWeapon component = popup.GetComponent<PopupReisouTargetWeapon>();
    popup.SetActive(false);
    e = component.Init(skill, (BackButtonMenuBase) basePopup);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    yield return (object) null;
    basePopup.StartCoroutine(basePopup.IsPushOff());
  }

  public void onBtnFusion()
  {
    if (this.IsPushAndSet())
      return;
    Bugu005ReisouFusionMaterialScene.ChangeScene(true, this.item.playerItem);
  }
}
