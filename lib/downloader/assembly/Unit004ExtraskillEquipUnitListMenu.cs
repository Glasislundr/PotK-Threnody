// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillEquipUnitListMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillEquipUnitListMenu : UnitMenuBase
{
  private GameObject extraSkillEquipUnitConfirm1DlgPrefab;
  private GameObject extraSkillEquipUnitConfirm2DlgPrefab;
  private GameObject skillEquipedByOthersPrefab;
  private PlayerUnit nowEquiptUnit;
  private PlayerAwakeSkill targetSkill;

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public void createAllUnitInfoExtendFunc()
  {
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (x =>
    {
      x.equip = false;
      if (!(x.playerUnit != (PlayerUnit) null) || x.playerUnit.equip_awake_skill_ids == null || ((IEnumerable<int?>) x.playerUnit.equip_awake_skill_ids).Count<int?>() <= 0 || !x.playerUnit.equip_awake_skill_ids[0].HasValue)
        return;
      x.equip = true;
    }));
  }

  public IEnumerator Init(PlayerUnit unit, PlayerAwakeSkill skill)
  {
    Unit004ExtraskillEquipUnitListMenu equipUnitListMenu = this;
    equipUnitListMenu.nowEquiptUnit = unit;
    equipUnitListMenu.targetSkill = skill;
    equipUnitListMenu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = equipUnitListMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> popupPrefabF;
    if (Object.op_Equality((Object) equipUnitListMenu.skillEquipedByOthersPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equiped_by_others__anim_popup01").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      equipUnitListMenu.skillEquipedByOthersPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) equipUnitListMenu.extraSkillEquipUnitConfirm1DlgPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equip_confirm1__anim_popup01").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      equipUnitListMenu.extraSkillEquipUnitConfirm1DlgPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) equipUnitListMenu.extraSkillEquipUnitConfirm2DlgPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equip_confirm2__anim_popup01").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      equipUnitListMenu.extraSkillEquipUnitConfirm2DlgPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    PlayerUnit[] playerUnitArray = equipUnitListMenu.SetEquipUnits(SMManager.Get<PlayerUnit[]>(), equipUnitListMenu.targetSkill);
    if (equipUnitListMenu.targetSkill.EqupmentUnit != (PlayerUnit) null)
      equipUnitListMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnitArray, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004ExtraSkillEquipUnitListSortAndFilter, true, equipUnitListMenu.nowEquiptUnit != (PlayerUnit) null, false, true, false, new Action(equipUnitListMenu.createAllUnitInfoExtendFunc));
    else
      equipUnitListMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnitArray, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004ExtraSkillEquipUnitListSortAndFilter, true, equipUnitListMenu.nowEquiptUnit != (PlayerUnit) null, false, true, false, new Action(equipUnitListMenu.createAllUnitInfoExtendFunc));
    e = equipUnitListMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    equipUnitListMenu.InitializeEnd();
  }

  private PlayerUnit[] SetEquipUnits(PlayerUnit[] units, PlayerAwakeSkill skill)
  {
    return ((IEnumerable<PlayerUnit>) units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.can_equip_awake_skill && x.unit.CanEquipAwakeSkill(skill))).ToArray<PlayerUnit>();
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.SetUnitIconAction(info_index, unit_index);
  }

  private IEnumerator ShowEquipedByOthersDig(PlayerUnit equipUnit)
  {
    Unit004ExtraskillEquipUnitListMenu equipUnitListMenu = this;
    GameObject prefab = equipUnitListMenu.skillEquipedByOthersPrefab.Clone();
    prefab.SetActive(false);
    IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipedByOthers>().Init(equipUnitListMenu.targetSkill, equipUnitListMenu.nowEquiptUnit, equipUnit, new Action<PlayerAwakeSkill, PlayerUnit, PlayerUnit>(equipUnitListMenu.SwapEquipExtraSkill));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
  }

  private IEnumerator ShowEquipUnitConfirm1Dlg(PlayerUnit unit)
  {
    Unit004ExtraskillEquipUnitListMenu equipUnitListMenu = this;
    GameObject prefab = equipUnitListMenu.extraSkillEquipUnitConfirm1DlgPrefab.Clone();
    prefab.SetActive(false);
    IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipConfirm1Menu>().Init(equipUnitListMenu.targetSkill, unit, new Action<PlayerAwakeSkill, PlayerUnit>(equipUnitListMenu.EquipExtraSkill));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
  }

  private IEnumerator ShowEquipUnitConfirm2Dlg(PlayerAwakeSkill awakeSKill, PlayerUnit targetUnit)
  {
    Unit004ExtraskillEquipUnitListMenu equipUnitListMenu = this;
    if (targetUnit.equippedExtraSkill == null)
    {
      equipUnitListMenu.ChangeExtraSkill(equipUnitListMenu.nowEquiptUnit.equippedExtraSkill, awakeSKill, targetUnit);
    }
    else
    {
      GameObject prefab = equipUnitListMenu.extraSkillEquipUnitConfirm2DlgPrefab.Clone();
      prefab.SetActive(false);
      IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipConfirm2Menu>().Init(targetUnit.equippedExtraSkill, awakeSKill, targetUnit, new Action<PlayerAwakeSkill, PlayerAwakeSkill, PlayerUnit>(equipUnitListMenu.ChangeExtraSkill));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      prefab.SetActive(true);
      prefab = (GameObject) null;
    }
  }

  private void SetUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    if (displayUnitInfo.removeButton)
    {
      unitIcon.onClick = (Action<UnitIconBase>) (iconBase => this.RemoveExtraSkill(this.nowEquiptUnit));
    }
    else
    {
      if (this.nowEquiptUnit != (PlayerUnit) null && this.nowEquiptUnit.id == displayUnitInfo.playerUnit.id)
        displayUnitInfo.button_enable = false;
      unitIcon.onClick = (Action<UnitIconBase>) (ui =>
      {
        if (!(unitIcon.PlayerUnit != (PlayerUnit) null))
          return;
        if (this.nowEquiptUnit != (PlayerUnit) null)
        {
          if (this.nowEquiptUnit.id == unitIcon.PlayerUnit.id)
            return;
          this.StartCoroutine(this.ShowEquipedByOthersDig(unitIcon.PlayerUnit));
        }
        else
          this.StartCoroutine(this.ShowEquipUnitConfirm1Dlg(unitIcon.PlayerUnit));
      });
      EventDelegate.Set(unitIcon.Button.onLongPress, (EventDelegate.Callback) (() => { }));
    }
    if (displayUnitInfo.button_enable)
      return;
    unitIcon.onClick = (Action<UnitIconBase>) (_ => { });
    unitIcon.Gray = true;
  }

  private void RemoveExtraSkill(PlayerUnit unit)
  {
    this.StartCoroutine(new ExtraskillUtil().RemoveExtraSkillAsync(unit, new Action(((UnitMenuBase) this).IbtnBack)));
  }

  private void EquipExtraSkill(PlayerAwakeSkill skill, PlayerUnit unit)
  {
    this.StartCoroutine(new ExtraskillUtil().EquipExtraSkillAsync(skill, unit, new Action(((UnitMenuBase) this).IbtnBack)));
  }

  private void ChangeExtraSkill(
    PlayerAwakeSkill beforeSkill,
    PlayerAwakeSkill afterSkill,
    PlayerUnit unit)
  {
    this.StartCoroutine(new ExtraskillUtil().ChangeEquipExtraSkillAsync(beforeSkill, afterSkill, unit, new Action(((UnitMenuBase) this).IbtnBack)));
  }

  private void SwapEquipExtraSkill(
    PlayerAwakeSkill skill,
    PlayerUnit nowEquipUnit,
    PlayerUnit targetUnit)
  {
    ExtraskillUtil extraskillUtil = new ExtraskillUtil();
    this.StartCoroutine(this.ShowEquipUnitConfirm2Dlg(skill, targetUnit));
  }
}
