// Decompiled with JetBrains decompiler
// Type: Unit004ExtraskillListMenu
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
public class Unit004ExtraskillListMenu : Unit004SelectExtraSkillListMenuBase
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
  private InventoryExtraSkill selectSkill;
  private UIWidget scrollBottom;
  private UIWidget scrollTop;
  private Unit004ExtraskillListMenu.BottomInfoType bottomInfoType;
  private bool isUpdateInfoData;

  protected override void SelectItemProc(InventoryExtraSkill skill)
  {
    this.selectSkill = this.InventoryExtraSkills.FirstOrDefault<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x.skill.id == skill.skill.id));
    switch (this.bottomInfoType)
    {
      case Unit004ExtraskillListMenu.BottomInfoType.LIST:
        this.ClearSelectItem();
        if (!this.IsSelectItem(skill))
          this.AddSelectItem(skill, false);
        this.ChangeBottomType(Unit004ExtraskillListMenu.BottomInfoType.SKILL_INFO);
        this.SelectMode = Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2;
        this.SelectMax = 2;
        this.isSelectItemAutoGray = false;
        this.StartCoroutine(this.extraSkillInfo.InitSkillInfo((Unit004SelectExtraSkillListMenuBase) this, this.selectSkill, Unit004ExtraskillInfo.StartTweenID));
        break;
      case Unit004ExtraskillListMenu.BottomInfoType.SKILL_INFO:
        this.ClearSelectItem();
        if (!this.IsSelectItem(skill))
          this.AddSelectItem(skill, false);
        this.SelectMode = Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2;
        this.SelectMax = 2;
        this.isSelectItemAutoGray = false;
        this.StartCoroutine(this.extraSkillInfo.InitSkillInfo((Unit004SelectExtraSkillListMenuBase) this, this.selectSkill, Unit004ExtraskillInfo.None));
        break;
    }
    this.AllItemIconUpdate();
  }

  public override Persist<Persist.ExtraSkillSortAndFilterInfo> GetPersist()
  {
    if (Persist.unit004ExtraSkillSortAndFilter.Exists && Persist.unit004ExtraSkillSortAndFilter.Data.filter.Count < 25)
    {
      int num = 25 - Persist.unit004ExtraSkillSortAndFilter.Data.filter.Count;
      for (int index = 0; index < num; ++index)
        Persist.unit004ExtraSkillSortAndFilter.Data.filter.Add(true);
      Persist.unit004ExtraSkillSortAndFilter.Flush();
    }
    return Persist.unit004ExtraSkillSortAndFilter;
  }

  protected override List<PlayerAwakeSkill> GetExtraSkills()
  {
    return ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).ToList<PlayerAwakeSkill>();
  }

  protected override void BottomInfoUpdate()
  {
  }

  public IEnumerator onBacSceneAsync()
  {
    Unit004ExtraskillListMenu menu = this;
    IEnumerator e = menu.extraSkillInfo.InitSkillInfo((Unit004SelectExtraSkillListMenuBase) menu, menu.selectSkill, -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override IEnumerator InitExtension()
  {
    Unit004ExtraskillListMenu extraskillListMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = extraskillListMenu.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> prefabF;
    if (Object.op_Equality((Object) extraskillListMenu.unitIconPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      extraskillListMenu.unitIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) extraskillListMenu.skillGenrePrefab, (Object) null))
    {
      prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      extraskillListMenu.skillGenrePrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (extraskillListMenu.extraSkillFavoriteDic == null)
      extraskillListMenu.extraSkillFavoriteDic = new Dictionary<int, bool>();
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

  private void ChangeBottomType(Unit004ExtraskillListMenu.BottomInfoType type, bool force = false)
  {
    if (Object.op_Equality((Object) this.scrollBottom, (Object) null) || Object.op_Equality((Object) this.scrollBarWidget, (Object) null))
      return;
    if (type != this.bottomInfoType | force)
    {
      this.bottomInfoType = type;
      int num = (this.DisplaySkills.Count - 1 < 0 ? 0 : this.DisplaySkills.Count - 1) / this.iconColumnValue * this.iconHeight;
      switch (this.bottomInfoType)
      {
        case Unit004ExtraskillListMenu.BottomInfoType.LIST:
          if (num > ExtraSkillIcon.Height)
            this.scrollBottom.height = this.SCROLL_BOTTOM_FULL;
          ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_FULL;
          break;
        case Unit004ExtraskillListMenu.BottomInfoType.SKILL_INFO:
          ((IEnumerable<GameObject>) this.bottomInfos).ToggleOnceEx(1);
          if (num > ExtraSkillIcon.Height)
            this.scrollBottom.height = this.SCROLL_BOTTOM_INFO;
          ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_INFO;
          break;
        case Unit004ExtraskillListMenu.BottomInfoType.SELL:
          ((IEnumerable<GameObject>) this.bottomInfos).ToggleOnceEx(2);
          if (num > ExtraSkillIcon.Height)
            this.scrollBottom.height = this.SCROLL_BOTTOM_SELL;
          ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_SELL;
          break;
        case Unit004ExtraskillListMenu.BottomInfoType.SKILL_COMPOSITE:
          ((IEnumerable<GameObject>) this.bottomInfos).ToggleOnceEx(1);
          if (num > ExtraSkillIcon.Height)
            this.scrollBottom.height = this.SCROLL_BOTTOM_COMPOSITE;
          ((UIRect) this.scrollBarWidget).bottomAnchor.absolute = this.SCROLL_BAR_BOTTOM_COMPOSITE;
          if (type != this.bottomInfoType)
          {
            Vector3 position = this.bottomInfos[1].transform.position;
            this.bottomInfos[1].transform.localPosition = new Vector3(position.x, 0.0f, position.z);
            break;
          }
          break;
      }
    }
    this.scroll.scrollView.UpdatePosition();
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

  public void onIbtnSell() => this.ChangeBottomType(Unit004ExtraskillListMenu.BottomInfoType.SELL);

  public void onIbtnComposite()
  {
    this.bottomInfoType = Unit004ExtraskillListMenu.BottomInfoType.SKILL_COMPOSITE;
  }

  public void onIbtnClose() => this.extraSkillInfo.StartTween(Unit004ExtraskillInfo.EndTweenID);

  public void CloseTweenFinish()
  {
    if (this.selectSkill != null && Object.op_Inequality((Object) this.selectSkill.icon, (Object) null))
      this.selectSkill.icon.Deselect();
    else
      this.AllDeselect();
    this.ChangeBottomType(Unit004ExtraskillListMenu.BottomInfoType.LIST);
  }

  public override void changeSceneExtraSkillEquipment(
    PlayerUnit targetUnit,
    PlayerAwakeSkill targetSkill)
  {
    this.StartCoroutine(this.changeFavorite((Action) (() => Unit004ExtraskillEquipUnitListScene.changeScene(true, targetUnit, targetSkill))));
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
    skill.skill.favorite = favorite;
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

  private enum BottomInfoType
  {
    LIST,
    SKILL_INFO,
    SELL,
    SKILL_COMPOSITE,
  }
}
