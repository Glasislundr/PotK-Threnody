// Decompiled with JetBrains decompiler
// Type: Unit00441Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using EquipmentRules;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00441Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtAttackLeft;
  [SerializeField]
  protected UILabel TxtMagicAttackLeft;
  [SerializeField]
  protected UILabel TxtDefenseLeft;
  [SerializeField]
  protected UILabel TxtMagicDefenseLeft;
  [SerializeField]
  protected UILabel TxtDexterityLeft;
  [SerializeField]
  protected UILabel TxtCriticalLeft;
  [SerializeField]
  protected UILabel TxtEvasionLeft;
  [SerializeField]
  protected UILabel TxtWaitLeft;
  [SerializeField]
  private GameObject[] RangeLeftObjects;
  [SerializeField]
  protected UILabel TxtRangeMinLeft;
  [SerializeField]
  protected UILabel TxtRangeMaxLeft;
  [SerializeField]
  protected UILabel TxtRangeLeft;
  [SerializeField]
  private GameObject NoneRangeLeft;
  [SerializeField]
  protected UILabel TxtAttackRight;
  [SerializeField]
  protected UILabel TxtMagicAttackRight;
  [SerializeField]
  protected UILabel TxtDefenseRight;
  [SerializeField]
  protected UILabel TxtMagicDefenseRight;
  [SerializeField]
  protected UILabel TxtDexterityRight;
  [SerializeField]
  protected UILabel TxtCriticalRight;
  [SerializeField]
  protected UILabel TxtEvasionRight;
  [SerializeField]
  protected UILabel TxtWaitRight;
  [SerializeField]
  private GameObject[] RangeRightObjects;
  [SerializeField]
  protected UILabel TxtRangeMinRight;
  [SerializeField]
  protected UILabel TxtRangeMaxRight;
  [SerializeField]
  protected UILabel TxtRangeRight;
  [SerializeField]
  private GameObject NoneRangeRight;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeHp;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeAttack;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeMagicAttack;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeDefense;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeMental;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeSpeed;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeTechnique;
  [SerializeField]
  private Popup00441AddEffect StatusGaugeLucky;
  [SerializeField]
  protected GameObject BeforeGear;
  [SerializeField]
  protected GameObject AfterGear;
  [SerializeField]
  protected GameObject BeforeSkill_One_Base;
  [SerializeField]
  protected UIWidget[] BeforeSkill_One;
  [SerializeField]
  protected UIButton[] BeforeSkill_One_Button;
  [SerializeField]
  protected GameObject AfterSkill_One_Base;
  [SerializeField]
  protected UIWidget[] AfterSkill_One;
  [SerializeField]
  protected UIButton[] AfterSkill_One_Button;
  [SerializeField]
  protected GameObject BeforeSkill_Two_Base;
  [SerializeField]
  protected UIWidget[] BeforeSkill_Two;
  [SerializeField]
  protected UIButton[] BeforeSkill_Two_Button;
  [SerializeField]
  protected GameObject AfterSkill_Two_Base;
  [SerializeField]
  protected UIWidget[] AfterSkill_Two;
  [SerializeField]
  protected UIButton[] AfterSkill_Two_Button;
  private PlayerUnit basePlayerUnit;
  private PlayerItem afterGearItem;
  private PlayerUnit changeUnit;
  private int changeGearIndex;
  private int? swapIndex;
  private bool isEarthMode;
  [SerializeField]
  private GameObject SkillDialog;
  [SerializeField]
  private GameObject floatingSkillDialog;
  private Action<GearGearSkill> showSkillDialog;
  private Action onDecide;
  private const int INDEX_EXTENSION = 3;

  private Color GetStrColor(int before, int after)
  {
    Color white = Color.white;
    if (before < after)
    {
      // ISSUE: explicit constructor call
      ((Color) ref white).\u002Ector(0.0f, 0.8627451f, 0.117647059f);
    }
    else if (before > after)
    {
      // ISSUE: explicit constructor call
      ((Color) ref white).\u002Ector(0.980392158f, 0.0f, 0.0f);
    }
    return white;
  }

  private IEnumerator setTexture(Future<Sprite> src, UI2DSprite to)
  {
    return src.Then<Sprite>((Func<Sprite, Sprite>) (sprite => to.sprite2D = sprite)).Wait();
  }

  protected void SetSkillDialogEvent(UIButton button, GearGearSkill skill_data)
  {
    if (this.showSkillDialog == null)
    {
      Battle0171111Event[] componentsInChildren = this.SkillDialog.Clone(this.floatingSkillDialog.transform).GetComponentsInChildren<Battle0171111Event>(true);
      Battle0171111Event dialog = componentsInChildren.Length != 0 ? componentsInChildren[0] : (Battle0171111Event) null;
      if (Object.op_Equality((Object) dialog, (Object) null))
        return;
      ((Component) ((Component) dialog).transform.parent).gameObject.SetActive(false);
      this.showSkillDialog = (Action<GearGearSkill>) (skill =>
      {
        dialog.setData(skill.skill);
        dialog.setSkillLv(skill.skill_level, skill.skill.upper_level);
        dialog.Show();
      });
    }
    EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.showSkillDialog(skill_data)));
  }

  public IEnumerator SetGear(
    PlayerUnit removeUnit,
    PlayerUnit baseUnit,
    PlayerItem beforeGear,
    PlayerItem afterGear,
    GameObject beforeGearIcon,
    GameObject afterGearIcon,
    GameObject[] beforeSkillIcons,
    GameObject[] afterSkillIcons,
    int index,
    bool isEarth = false,
    Action eventDecide = null,
    bool isSwap = false,
    PlayerItem[] customReisous = null)
  {
    this.isEarthMode = isEarth;
    this.changeUnit = removeUnit;
    this.changeGearIndex = index;
    this.onDecide = eventDecide;
    this.basePlayerUnit = baseUnit;
    this.afterGearItem = afterGear;
    PlayerItem beforeReisou = beforeGear == (PlayerItem) null ? (PlayerItem) null : beforeGear.equipReisou;
    PlayerItem afterReisou = afterGear == (PlayerItem) null ? (PlayerItem) null : afterGear.equipReisou;
    if (Object.op_Equality((Object) this.SkillDialog, (Object) null))
    {
      Future<GameObject> loader = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SkillDialog = loader.Result;
      loader = (Future<GameObject>) null;
    }
    PlayerItem[] self = new PlayerItem[3]
    {
      baseUnit.equippedGear,
      baseUnit.equippedGear2,
      baseUnit.equippedGear3
    };
    int after = BattleFuncs.calcEquippedGearWeight((GearGear) null, self[0], self[1], self[2]);
    PlayerItem[] playerItemArray = new PlayerItem[3]
    {
      baseUnit.equippedReisou,
      baseUnit.equippedReisou2,
      baseUnit.equippedReisou3
    };
    Judgement.NonBattleParameter nonBattleParameter1 = Judgement.NonBattleParameter.FromPlayerUnitWithPlayerGear(baseUnit, true, self[0], self[1], playerItemArray[0], playerItemArray[1], playerItemArray[2]);
    this.swapIndex = isSwap ? ((IEnumerable<PlayerItem>) self).FirstIndexOrNull<PlayerItem>((Func<PlayerItem, bool>) (x =>
    {
      int id1 = afterGear.id;
      int? id2 = x?.id;
      int valueOrDefault = id2.GetValueOrDefault();
      return id1 == valueOrDefault & id2.HasValue;
    })) : new int?();
    switch (this.changeGearIndex)
    {
      case 2:
        self[1] = afterGear;
        playerItemArray[1] = isSwap || customReisous == null ? afterReisou : Reisou.checkPossibleEquipped(self[1], customReisous[1]);
        break;
      case 3:
        self[2] = afterGear;
        playerItemArray[2] = isSwap || customReisous == null ? afterReisou : Reisou.checkPossibleEquipped(self[2], customReisous[2]);
        break;
      default:
        self[0] = afterGear;
        playerItemArray[0] = isSwap || customReisous == null ? afterReisou : Reisou.checkPossibleEquipped(self[0], customReisous[0]);
        break;
    }
    if (isSwap)
    {
      self[this.swapIndex.Value] = beforeGear;
      playerItemArray[this.swapIndex.Value] = beforeReisou;
    }
    Judgement.NonBattleParameter nonBattleParameter2 = Judgement.NonBattleParameter.FromPlayerUnitWithPlayerGear(baseUnit, true, self[0], self[1], playerItemArray[0], playerItemArray[1], playerItemArray[2]);
    int minRange1 = baseUnit.initial_gear.min_range;
    int maxRange1 = baseUnit.initial_gear.max_range;
    int minRange2 = baseUnit.initial_gear.min_range;
    int maxRange2 = baseUnit.initial_gear.max_range;
    int num1 = BattleFuncs.calcEquippedGearWeight((GearGear) null, self[0], self[1], self[2]);
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    int num7 = 0;
    int num8 = 0;
    int num9 = 0;
    bool flag = this.changeGearIndex == 3;
    beforeGearIcon.gameObject.SetParent(this.BeforeGear);
    beforeGearIcon.SetActive(false);
    beforeGearIcon.SetActive(true);
    if (beforeGear != (PlayerItem) null)
    {
      UIWidget[] uiWidgetArray = this.BeforeSkill_One;
      UIButton[] uiButtonArray = this.BeforeSkill_One_Button;
      this.BeforeSkill_One_Base.SetActive(true);
      this.BeforeSkill_Two_Base.SetActive(false);
      if (beforeSkillIcons != null && beforeSkillIcons.Length != 0)
      {
        if (beforeSkillIcons.Length > 1)
        {
          this.BeforeSkill_One_Base.SetActive(false);
          this.BeforeSkill_Two_Base.SetActive(true);
          uiWidgetArray = this.BeforeSkill_Two;
          uiButtonArray = this.BeforeSkill_Two_Button;
        }
        for (int index1 = 0; index1 < uiWidgetArray.Length; ++index1)
        {
          beforeSkillIcons[index1].GetComponent<BattleSkillIcon>().SetDepth(uiWidgetArray[index1].depth + 1);
          beforeSkillIcons[index1].SetParent(((Component) uiWidgetArray[index1]).gameObject);
          this.SetSkillDialogEvent(uiButtonArray[index1], beforeGear.skills[index1]);
          beforeSkillIcons[index1].SetActive(false);
          beforeSkillIcons[index1].SetActive(true);
        }
      }
      if (beforeGear.gear.kind.isNonWeapon)
      {
        if (minRange1 < beforeGear.gear.min_range)
          minRange1 = beforeGear.gear.min_range;
        if (maxRange1 < beforeGear.gear.max_range)
          maxRange1 = beforeGear.gear.max_range;
      }
      else
      {
        minRange1 = beforeGear.gear.min_range;
        maxRange1 = beforeGear.gear.max_range;
      }
    }
    this.TxtAttackLeft.SetTextLocalize(nonBattleParameter1.PhysicalAttack.ToString());
    this.TxtMagicAttackLeft.SetTextLocalize(nonBattleParameter1.MagicAttack.ToString());
    this.TxtCriticalLeft.SetTextLocalize(nonBattleParameter1.Critical.ToString());
    this.TxtDefenseLeft.SetTextLocalize(nonBattleParameter1.PhysicalDefense.ToString());
    this.TxtDexterityLeft.SetTextLocalize(nonBattleParameter1.Hit.ToString());
    this.TxtEvasionLeft.SetTextLocalize(nonBattleParameter1.Evasion.ToString());
    this.TxtMagicDefenseLeft.SetTextLocalize(nonBattleParameter1.MagicDefense.ToString());
    ((IEnumerable<GameObject>) this.RangeLeftObjects).SetActives(!flag);
    this.NoneRangeLeft.SetActive(flag);
    if (!flag)
    {
      this.TxtRangeMinLeft.SetTextLocalize(string.Format("{0}", (object) minRange1));
      this.TxtRangeMaxLeft.SetTextLocalize(string.Format("{0}", (object) maxRange1));
    }
    this.TxtWaitLeft.SetTextLocalize(after.ToString());
    afterGearIcon.gameObject.SetParent(this.AfterGear);
    afterGearIcon.SetActive(false);
    afterGearIcon.SetActive(true);
    if (afterGear != (PlayerItem) null)
    {
      UIWidget[] uiWidgetArray = this.AfterSkill_One;
      UIButton[] uiButtonArray = this.AfterSkill_One_Button;
      this.AfterSkill_One_Base.SetActive(true);
      this.AfterSkill_Two_Base.SetActive(false);
      if (afterSkillIcons != null && afterSkillIcons.Length != 0)
      {
        if (afterSkillIcons.Length > 1)
        {
          this.AfterSkill_One_Base.SetActive(false);
          this.AfterSkill_Two_Base.SetActive(true);
          uiWidgetArray = this.AfterSkill_Two;
          uiButtonArray = this.AfterSkill_Two_Button;
        }
        for (int index2 = 0; index2 < uiWidgetArray.Length; ++index2)
        {
          afterSkillIcons[index2].GetComponent<BattleSkillIcon>().SetDepth(uiWidgetArray[index2].depth + 1);
          afterSkillIcons[index2].gameObject.SetParent(((Component) uiWidgetArray[index2]).gameObject);
          this.SetSkillDialogEvent(uiButtonArray[index2], afterGear.skills[index2]);
          afterSkillIcons[index2].SetActive(false);
          afterSkillIcons[index2].SetActive(true);
        }
      }
      if (this.changeGearIndex != 3)
      {
        num2 = afterGear.hp_incremental;
        num3 = afterGear.strength_incremental;
        num4 = afterGear.intelligence_incremental;
        num5 = afterGear.vitality_incremental;
        num6 = afterGear.mind_incremental;
        num7 = afterGear.agility_incremental;
        num8 = afterGear.dexterity_incremental;
        num9 = afterGear.lucky_incremental;
      }
      if (afterGear.gear.kind.isNonWeapon)
      {
        if (minRange2 < afterGear.gear.min_range)
          minRange2 = afterGear.gear.min_range;
        if (maxRange2 < afterGear.gear.max_range)
          maxRange2 = afterGear.gear.max_range;
      }
      else
      {
        minRange2 = afterGear.gear.min_range;
        maxRange2 = afterGear.gear.max_range;
      }
    }
    this.TxtAttackRight.SetTextLocalize(nonBattleParameter2.PhysicalAttack.ToString());
    ((UIWidget) this.TxtAttackRight).color = this.GetStrColor(nonBattleParameter1.PhysicalAttack, nonBattleParameter2.PhysicalAttack);
    this.TxtMagicAttackRight.SetTextLocalize(nonBattleParameter2.MagicAttack.ToString());
    ((UIWidget) this.TxtMagicAttackRight).color = this.GetStrColor(nonBattleParameter1.MagicAttack, nonBattleParameter2.MagicAttack);
    this.TxtCriticalRight.SetTextLocalize(nonBattleParameter2.Critical.ToString());
    ((UIWidget) this.TxtCriticalRight).color = this.GetStrColor(nonBattleParameter1.Critical, nonBattleParameter2.Critical);
    this.TxtDefenseRight.SetTextLocalize(nonBattleParameter2.PhysicalDefense.ToString());
    ((UIWidget) this.TxtDefenseRight).color = this.GetStrColor(nonBattleParameter1.PhysicalDefense, nonBattleParameter2.PhysicalDefense);
    this.TxtDexterityRight.SetTextLocalize(nonBattleParameter2.Hit.ToString());
    ((UIWidget) this.TxtDexterityRight).color = this.GetStrColor(nonBattleParameter1.Hit, nonBattleParameter2.Hit);
    this.TxtEvasionRight.SetTextLocalize(nonBattleParameter2.Evasion.ToString());
    ((UIWidget) this.TxtEvasionRight).color = this.GetStrColor(nonBattleParameter1.Evasion, nonBattleParameter2.Evasion);
    this.TxtMagicDefenseRight.SetTextLocalize(nonBattleParameter2.MagicDefense.ToString());
    ((UIWidget) this.TxtMagicDefenseRight).color = this.GetStrColor(nonBattleParameter1.MagicDefense, nonBattleParameter2.MagicDefense);
    ((IEnumerable<GameObject>) this.RangeRightObjects).SetActives(!flag);
    this.NoneRangeRight.SetActive(flag);
    if (!flag)
    {
      this.TxtRangeMinRight.SetTextLocalize(string.Format("{0}", (object) minRange2));
      ((UIWidget) this.TxtRangeMinRight).color = this.GetStrColor(minRange1, minRange2);
      this.TxtRangeMaxRight.SetTextLocalize(string.Format("{0}", (object) maxRange2));
      ((UIWidget) this.TxtRangeMaxRight).color = this.GetStrColor(maxRange1, maxRange2);
    }
    this.TxtWaitRight.SetTextLocalize(num1);
    ((UIWidget) this.TxtWaitRight).color = this.GetStrColor(num1, after);
    this.StatusGaugeHp.Init(num2);
    this.StatusGaugeAttack.Init(num3);
    this.StatusGaugeMagicAttack.Init(num4);
    this.StatusGaugeDefense.Init(num5);
    this.StatusGaugeMental.Init(num6);
    this.StatusGaugeSpeed.Init(num7);
    this.StatusGaugeTechnique.Init(num8);
    this.StatusGaugeLucky.Init(num9);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupDecide()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onDecide != null)
      this.onDecide();
    else
      this.StartCoroutine(this.Equip());
  }

  private IEnumerator Equip()
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    bool isSuccess = true;
    PlayerItem afterGearItem = this.afterGearItem;
    int afterGearID = (object) afterGearItem != null ? afterGearItem.id : 0;
    IEnumerator e1;
    if (this.isEarthMode)
    {
      if (this.changeUnit != (PlayerUnit) null)
      {
        e1 = RequestDispatcher.EquipGear(this.changeGearIndex, new int?(0), this.changeUnit.id, (Action<WebAPI.Response.UserError>) (e =>
        {
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          isSuccess = false;
          Singleton<NGSceneManager>.GetInstance().backScene();
        }), this.isEarthMode);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (!isSuccess)
          yield break;
      }
      e1 = RequestDispatcher.EquipGear(this.changeGearIndex, new int?(afterGearID), this.basePlayerUnit.id, (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        isSuccess = false;
        Singleton<NGSceneManager>.GetInstance().backScene();
      }), this.isEarthMode);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (!isSuccess)
        yield break;
    }
    else
    {
      int[] numArray = new int[3];
      PlayerItem equippedGear = this.basePlayerUnit.equippedGear;
      numArray[0] = (object) equippedGear != null ? equippedGear.id : 0;
      PlayerItem equippedGear2 = this.basePlayerUnit.equippedGear2;
      numArray[1] = (object) equippedGear2 != null ? equippedGear2.id : 0;
      PlayerItem equippedGear3 = this.basePlayerUnit.equippedGear3;
      numArray[2] = (object) equippedGear3 != null ? equippedGear3.id : 0;
      int[] player_gear_ids = numArray;
      if (this.swapIndex.HasValue)
      {
        int index1 = this.changeGearIndex - 1;
        int index2 = this.swapIndex.Value;
        int num = player_gear_ids[index1];
        player_gear_ids[index1] = player_gear_ids[index2];
        player_gear_ids[index2] = num;
      }
      else
      {
        int index = this.changeGearIndex - 1;
        player_gear_ids[index] = afterGearID;
      }
      e1 = WebAPI.UnitBulkEquip(player_gear_ids, this.basePlayerUnit.id, (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (e != null)
          WebAPI.DefaultUserErrorCallback(e);
        isSuccess = false;
        Singleton<NGSceneManager>.GetInstance().backScene();
      })).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (!isSuccess)
        yield break;
    }
    this.basePlayerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == this.basePlayerUnit.id));
    switch (GuildUtil.gvgPopupState)
    {
      case GuildUtil.GvGPopupState.None:
        this.BackScene();
        break;
      case GuildUtil.GvGPopupState.AtkTeam:
        e1 = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => this.BackScene()));
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        break;
      case GuildUtil.GvGPopupState.DefTeam:
        e1 = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, (Action) (() => this.BackScene()));
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        break;
      default:
        this.BackScene();
        break;
    }
  }

  private void BackScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<NGSceneManager>.GetInstance().backScene();
  }
}
