// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillEquipListMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004ExtraskillEquipListMenu : Unit004SelectExtraSkillListMenuBase
{
  private readonly int SCROLL_BOTTOM_FULL = 100;
  private readonly int SCROLL_BAR_BOTTOM_FULL = 6;
  private readonly int SCROLL_BOTTOM_SELL = 300;
  private readonly int SCROLL_BAR_BOTTOM_SELL = 168;
  private readonly int SCROLL_BOTTOM_INFO = 430;
  private readonly int SCROLL_BAR_BOTTOM_INFO = 230;
  private readonly int SCROLL_BOTTOM_COMPOSITE = 810;
  private readonly int SCROLL_BAR_BOTTOM_COMPOSITE = 420;
  private readonly float SKILL_INFO_Y_CORRECT = -176f;
  [SerializeField]
  private UIWidget scrollBarWidget;
  [SerializeField]
  private GameObject[] bottomInfos;
  [SerializeField]
  private Unit004ExtraskillInfo extraSkillInfo;
  private Dictionary<int, bool> extraSkillFavoriteDic;
  private bool isSelectItemAutoGray;
  private GameObject extraSkillEquipUnitConfirm1DlgPrefab;
  private GameObject extraSkillEquipUnitConfirm2DlgPrefab;
  private GameObject skillEquipedByOthersPrefab;
  private PlayerUnit targetUnit;
  private InventoryExtraSkill selectSkill;
  private UIWidget scrollBottom;
  private UIWidget scrollTop;
  private Unit004ExtraskillEquipListMenu.BottomInfoType bottomInfoType;
  private bool isUpdateInfoData;

  public EditAwakeSkillParam EditParam { get; set; }

  public PlayerUnit TargetUnit
  {
    set => this.targetUnit = value;
    get => this.targetUnit;
  }

  public override IEnumerator Init()
  {
    Unit004ExtraskillEquipListMenu extraskillEquipListMenu = this;
    if (extraskillEquipListMenu.targetUnit.equippedExtraSkill != null)
      extraskillEquipListMenu.equip_unit_id = extraskillEquipListMenu.targetUnit.id;
    if (extraskillEquipListMenu.EditParam != null)
      extraskillEquipListMenu.extraSkillInfo.setEnableFavoriteSwitch(false);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = extraskillEquipListMenu.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void SelectItemProc(InventoryExtraSkill skill)
  {
    if (skill == null || skill.removeButton)
    {
      this.RemoveExtraSkill(this.targetUnit);
      this.onIbtnClose();
    }
    else
    {
      this.selectSkill = this.InventoryExtraSkills.FirstOrDefault<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x.skill != null && x.skill.id == skill.skill.id));
      bool flag = false;
      PlayerAwakeSkill equippedExtraSkill = this.targetUnit.equippedExtraSkill;
      if (!skill.removeButton && equippedExtraSkill != null && equippedExtraSkill.id == skill.skill.id)
        flag = true;
      switch (this.bottomInfoType)
      {
        case Unit004ExtraskillEquipListMenu.BottomInfoType.LIST:
          this.ClearSelectItem();
          if (!this.IsSelectItem(skill))
            this.AddSelectItem(skill, false);
          this.ChangeBottomType(Unit004ExtraskillEquipListMenu.BottomInfoType.SKILL_INFO);
          this.SelectMode = Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2;
          this.SelectMax = 2;
          this.isSelectItemAutoGray = false;
          this.StartCoroutine(this.extraSkillInfo.InitSkillInfo((Unit004SelectExtraSkillListMenuBase) this, this.selectSkill, Unit004ExtraskillInfo.StartTweenID, false));
          this.extraSkillInfo.setEnableIbtnDecision(!flag);
          break;
        case Unit004ExtraskillEquipListMenu.BottomInfoType.SKILL_INFO:
          this.ClearSelectItem();
          if (!this.IsSelectItem(skill))
            this.AddSelectItem(skill, false);
          this.SelectMode = Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2;
          this.SelectMax = 2;
          this.isSelectItemAutoGray = false;
          this.StartCoroutine(this.extraSkillInfo.InitSkillInfo((Unit004SelectExtraSkillListMenuBase) this, this.selectSkill, Unit004ExtraskillInfo.None, false));
          this.extraSkillInfo.setEnableIbtnDecision(!flag);
          break;
      }
      this.AllItemIconUpdate();
    }
  }

  public override Persist<Persist.ExtraSkillSortAndFilterInfo> GetPersist()
  {
    if (Persist.unit004ExtraSkillEquipListSortAndFilter.Exists && Persist.unit004ExtraSkillEquipListSortAndFilter.Data.filter.Count < 25)
    {
      int num = 25 - Persist.unit004ExtraSkillEquipListSortAndFilter.Data.filter.Count;
      for (int index = 0; index < num; ++index)
        Persist.unit004ExtraSkillEquipListSortAndFilter.Data.filter.Add(true);
      Persist.unit004ExtraSkillEquipListSortAndFilter.Flush();
    }
    return Persist.unit004ExtraSkillEquipListSortAndFilter;
  }

  protected override List<PlayerAwakeSkill> GetExtraSkills()
  {
    return ((IEnumerable<PlayerAwakeSkill>) this.targetUnit.unit.GetAllEquipableAwakeSkills(this.EditParam?.skills)).ToList<PlayerAwakeSkill>();
  }

  protected override IEnumerator InitExtension()
  {
    Unit004ExtraskillEquipListMenu extraskillEquipListMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = extraskillEquipListMenu.\u003C\u003En__1();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if ((extraskillEquipListMenu.TargetUnit.equip_awake_skill_ids == null ? 0 : (extraskillEquipListMenu.TargetUnit.equip_awake_skill_ids.Length == 0 ? 0 : (extraskillEquipListMenu.TargetUnit.equip_awake_skill_ids[0].HasValue ? 1 : 0))) != 0)
      extraskillEquipListMenu.InventoryExtraSkills.Insert(0, new InventoryExtraSkill());
    Future<GameObject> popupPrefabF;
    if (extraskillEquipListMenu.EditParam == null)
    {
      if (Object.op_Equality((Object) extraskillEquipListMenu.skillEquipedByOthersPrefab, (Object) null))
      {
        popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equiped_by_others__anim_popup01").Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        extraskillEquipListMenu.skillEquipedByOthersPrefab = popupPrefabF.Result;
        popupPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) extraskillEquipListMenu.extraSkillEquipUnitConfirm1DlgPrefab, (Object) null))
      {
        popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equip_confirm1__anim_popup01").Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        extraskillEquipListMenu.extraSkillEquipUnitConfirm1DlgPrefab = popupPrefabF.Result;
        popupPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) extraskillEquipListMenu.extraSkillEquipUnitConfirm2DlgPrefab, (Object) null))
      {
        popupPrefabF = new ResourceObject("Prefabs/popup/popup_004_extraskill_equip_confirm2__anim_popup01").Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        extraskillEquipListMenu.extraSkillEquipUnitConfirm2DlgPrefab = popupPrefabF.Result;
        popupPrefabF = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) extraskillEquipListMenu.unitIconPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      extraskillEquipListMenu.unitIconPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) extraskillEquipListMenu.skillGenrePrefab, (Object) null))
    {
      popupPrefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      extraskillEquipListMenu.skillGenrePrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (extraskillEquipListMenu.extraSkillFavoriteDic == null)
      extraskillEquipListMenu.extraSkillFavoriteDic = new Dictionary<int, bool>();
  }

  public override void Sort(
    ExtraSkillSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    base.Sort(type, order);
    if (Object.op_Inequality((Object) this.scroll.BottomObject, (Object) null))
      this.scrollBottom = this.scroll.BottomObject.GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) this.scroll.TopObject, (Object) null))
      this.scrollTop = this.scroll.TopObject.GetComponent<UIWidget>();
    this.ChangeBottomType(this.bottomInfoType, true);
  }

  private void ChangeBottomType(Unit004ExtraskillEquipListMenu.BottomInfoType type, bool force = false)
  {
    if (Object.op_Equality((Object) this.scrollBottom, (Object) null) || Object.op_Equality((Object) this.scrollBarWidget, (Object) null) || !(type != this.bottomInfoType | force))
      return;
    this.bottomInfoType = type;
    int num = (this.DisplaySkills.Count - 1 < 0 ? 0 : this.DisplaySkills.Count - 1) / this.iconColumnValue * this.iconHeight;
    switch (this.bottomInfoType)
    {
      case Unit004ExtraskillEquipListMenu.BottomInfoType.LIST:
        if (num > ExtraSkillIcon.Height)
          this.scrollBottom.height = this.SCROLL_BOTTOM_FULL;
        ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_FULL;
        break;
      case Unit004ExtraskillEquipListMenu.BottomInfoType.SKILL_INFO:
        ((IEnumerable<GameObject>) this.bottomInfos).ToggleOnceEx(1);
        if (num > ExtraSkillIcon.Height)
          this.scrollBottom.height = this.SCROLL_BOTTOM_INFO;
        ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_INFO;
        break;
    }
    this.scroll.scrollView.UpdatePosition();
  }

  private IEnumerator ShowEquipedByOthersDig(InventoryExtraSkill awakeSKill)
  {
    Unit004ExtraskillEquipListMenu extraskillEquipListMenu = this;
    GameObject prefab = extraskillEquipListMenu.skillEquipedByOthersPrefab.Clone();
    prefab.SetActive(false);
    IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipedByOthers>().Init(awakeSKill.skill, awakeSKill.equipUnit, extraskillEquipListMenu.targetUnit, new Action<PlayerAwakeSkill, PlayerUnit, PlayerUnit>(extraskillEquipListMenu.SwapEquipExtraSkill));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
  }

  private IEnumerator ShowEquipUnitConfirm1Dlg(InventoryExtraSkill awakeSKill)
  {
    Unit004ExtraskillEquipListMenu extraskillEquipListMenu = this;
    GameObject prefab = extraskillEquipListMenu.extraSkillEquipUnitConfirm1DlgPrefab.Clone();
    prefab.SetActive(false);
    IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipConfirm1Menu>().Init(awakeSKill.skill, extraskillEquipListMenu.targetUnit, new Action<PlayerAwakeSkill, PlayerUnit>(extraskillEquipListMenu.EquipExtraSkill));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
  }

  private IEnumerator ShowEquipUnitConfirm2Dlg(PlayerAwakeSkill awakeSKill)
  {
    Unit004ExtraskillEquipListMenu extraskillEquipListMenu = this;
    if (extraskillEquipListMenu.targetUnit.equippedExtraSkill == null)
    {
      extraskillEquipListMenu.ChangeExtraSkill(extraskillEquipListMenu.targetUnit.equippedExtraSkill, awakeSKill, extraskillEquipListMenu.targetUnit);
    }
    else
    {
      GameObject prefab = extraskillEquipListMenu.extraSkillEquipUnitConfirm2DlgPrefab.Clone();
      prefab.SetActive(false);
      IEnumerator e = prefab.GetComponent<Popup004ExtraSkillEquipConfirm2Menu>().Init(extraskillEquipListMenu.targetUnit.equippedExtraSkill, awakeSKill, extraskillEquipListMenu.targetUnit, new Action<PlayerAwakeSkill, PlayerAwakeSkill, PlayerUnit>(extraskillEquipListMenu.ChangeExtraSkill));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      prefab.SetActive(true);
      prefab = (GameObject) null;
    }
  }

  private IEnumerator changeFavorite(Action endAct)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.AwakeSkillFavorite> fav = WebAPI.AwakeSkillFavorite(this.extraSkillFavoriteDic.Where<KeyValuePair<int, bool>>((Func<KeyValuePair<int, bool>, bool>) (x => x.Value)).Select<KeyValuePair<int, bool>, int>((Func<KeyValuePair<int, bool>, int>) (x => x.Key)).ToArray<int>(), this.extraSkillFavoriteDic.Where<KeyValuePair<int, bool>>((Func<KeyValuePair<int, bool>, bool>) (x => !x.Value)).Select<KeyValuePair<int, bool>, int>((Func<KeyValuePair<int, bool>, int>) (x => x.Key)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }));
    IEnumerator e1 = fav.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (fav.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      this.extraSkillFavoriteDic.Clear();
      endAct();
    }
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.extraSkillFavoriteDic != null && this.extraSkillFavoriteDic.Count<KeyValuePair<int, bool>>() > 0)
      this.StartCoroutine(this.changeFavorite((Action) (() => this.backScene())));
    else
      this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  private void RemoveExtraSkill(PlayerUnit unit)
  {
    if (this.EditParam != null)
      this.onSetCustomDeckAwakeSkill(0);
    else
      this.StartCoroutine(new ExtraskillUtil().RemoveExtraSkillAsync(unit, new Action(((Unit004ExtraSkillListMenuBase) this).IbtnBack)));
  }

  private void EquipExtraSkill(PlayerAwakeSkill skill, PlayerUnit unit)
  {
    this.StartCoroutine(new ExtraskillUtil().EquipExtraSkillAsync(skill, unit, new Action(((Unit004ExtraSkillListMenuBase) this).IbtnBack)));
  }

  private void ChangeExtraSkill(
    PlayerAwakeSkill beforeSkill,
    PlayerAwakeSkill afterSkill,
    PlayerUnit unit)
  {
    this.StartCoroutine(new ExtraskillUtil().ChangeEquipExtraSkillAsync(beforeSkill, afterSkill, unit, new Action(((Unit004ExtraSkillListMenuBase) this).IbtnBack)));
  }

  private void SwapEquipExtraSkill(
    PlayerAwakeSkill skill,
    PlayerUnit nowEquipUnit,
    PlayerUnit unit)
  {
    this.StartCoroutine(this.ShowEquipUnitConfirm2Dlg(skill));
  }

  public void onIbtnClose()
  {
    this.extraSkillInfo.StartTween(Unit004ExtraskillInfo.EndTweenID);
    this.ChangeBottomType(Unit004ExtraskillEquipListMenu.BottomInfoType.LIST);
  }

  public override void changeExtraSkillFavorite(InventoryExtraSkill skill, bool favorite)
  {
    if (this.extraSkillFavoriteDic.ContainsKey(skill.skill.id))
    {
      int num = this.extraSkillFavoriteDic[skill.skill.id] ? 1 : 0;
      if (((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.id == skill.skill.id)).favorite == favorite)
        this.extraSkillFavoriteDic.Remove(skill.skill.id);
    }
    else if (((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.id == skill.skill.id)).favorite != favorite)
      this.extraSkillFavoriteDic.Add(skill.skill.id, favorite);
    skill.favorite = favorite;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (!this.Filter[11])
      this.CreateExtraSkillIcon();
    else
      this.UpdateInventoryExtraSkillList();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  protected override void UpdateInventoryExtraSkillList()
  {
    List<InventoryExtraSkill> list = this.InventoryExtraSkills.Where<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => !x.removeButton)).ToList<InventoryExtraSkill>();
    if (list != null && list.Count<InventoryExtraSkill>() > 0)
    {
      PlayerAwakeSkill[] source = SMManager.Get<PlayerAwakeSkill[]>();
      foreach (InventoryExtraSkill inventoryExtraSkill in list)
      {
        InventoryExtraSkill invItem = inventoryExtraSkill;
        PlayerAwakeSkill skill = ((IEnumerable<PlayerAwakeSkill>) source).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.id == invItem.uniqueID));
        if (skill != null)
          this.UpdateInventoryExtraSkillList(invItem, skill, this.isUpdateInfoData ? skill.favorite : invItem.favorite);
      }
    }
    this.SelectItemList.ForEachIndex<InventoryExtraSkill>((Action<InventoryExtraSkill, int>) ((x, idx) =>
    {
      x.select = true;
      if (this.isSelectItemAutoGray)
        x.Gray = true;
      if (this.SelectMode != Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num)
        return;
      x.index = idx + 1;
    }));
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  public override void onClickDecision()
  {
    if (this.EditParam != null)
      this.onSetCustomDeckAwakeSkill(this.selectSkill?.skill?.id ?? 0);
    else if (this.selectSkill.equipUnit != (PlayerUnit) null && this.selectSkill.equipUnit != this.targetUnit)
      this.StartCoroutine(this.ShowEquipedByOthersDig(this.selectSkill));
    else if (this.targetUnit.equippedExtraSkill != null)
      this.StartCoroutine(this.ShowEquipUnitConfirm2Dlg(this.selectSkill.skill));
    else
      this.StartCoroutine(this.ShowEquipUnitConfirm1Dlg(this.selectSkill));
  }

  private void onSetCustomDeckAwakeSkill(int skillId)
  {
    this.IsPush = true;
    this.EditParam.onSetSkill(this.EditParam.index, skillId);
    this.backScene();
  }

  protected override InventoryExtraSkill CreateInventoryExtraSkill(PlayerAwakeSkill skill)
  {
    if (this.EditParam == null)
      return new InventoryExtraSkill(skill);
    PlayerUnit equipped;
    this.EditParam.dicReference.TryGetValue(skill.id, out equipped);
    return new InventoryExtraSkill(skill, equipped);
  }

  protected override void UpdateInventoryExtraSkillList(
    InventoryExtraSkill invItem,
    PlayerAwakeSkill skill,
    bool favorite)
  {
    if (this.EditParam == null)
    {
      invItem.Init(skill);
      invItem.favorite = favorite;
    }
    else
    {
      PlayerUnit equipped;
      this.EditParam.dicReference.TryGetValue(skill.id, out equipped);
      invItem.Init(skill, equipped);
      invItem.favorite = favorite;
    }
  }

  private enum BottomInfoType
  {
    LIST,
    SKILL_INFO,
  }
}
