// Decompiled with JetBrains decompiler
// Type: Unit004SelectExtraSkillListMenuBase
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
public class Unit004SelectExtraSkillListMenuBase : Unit004ExtraSkillListMenuBase
{
  [SerializeField]
  protected Unit004SelectExtraSkillListMenuBase.SelectModeEnum SelectMode;
  [SerializeField]
  protected bool EnableFavorite;
  [SerializeField]
  protected bool EnableForBattle;
  [SerializeField]
  private int selectMax;
  protected List<InventoryExtraSkill> SelectItemList = new List<InventoryExtraSkill>();
  protected GameObject unitIconPrefab;
  protected GameObject skillGenrePrefab;
  protected int equip_unit_id = -1;

  public int SelectMax
  {
    get => this.selectMax;
    set => this.selectMax = value;
  }

  public GameObject UnitIconPrefab => this.unitIconPrefab;

  public GameObject SkillGenrePrefab => this.skillGenrePrefab;

  protected virtual void SelectItemProc(InventoryExtraSkill skill)
  {
  }

  protected virtual bool IsGrayIcon(InventoryExtraSkill item)
  {
    if (item.equipUnit != (PlayerUnit) null && item.equipUnit.id == this.equip_unit_id || this.DisableTouchIcon(item))
      return true;
    return this.SelectItemList.Count >= this.selectMax ? !item.Gray : item.Gray;
  }

  protected virtual bool DisableTouchIcon(InventoryExtraSkill invSkill)
  {
    if (invSkill == null)
      return !this.EnableForBattle || !this.EnableFavorite;
    if (!this.EnableForBattle && invSkill.forBattle)
      return true;
    return !this.EnableFavorite && invSkill.favorite;
  }

  protected override void UpdateInventoryExtraSkillList(
    InventoryExtraSkill invItem,
    PlayerAwakeSkill skill,
    bool favorite)
  {
    invItem.Init(skill);
    invItem.favorite = favorite;
    invItem.select = false;
    invItem.Gray = false;
    if (!this.DisableTouchIcon(invItem))
      return;
    this.RemoveSelectItem(invItem);
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
          this.UpdateInventoryExtraSkillList(invItem, skill, skill.favorite);
      }
    }
    this.SelectItemList.ForEachIndex<InventoryExtraSkill>((Action<InventoryExtraSkill, int>) ((x, idx) =>
    {
      x.select = true;
      x.Gray = true;
      if (this.SelectMode != Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num)
        return;
      x.index = idx + 1;
    }));
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryIdx, int allItemIdx)
  {
    ExtraSkillIcon extraSkillIcon = this.AllExtraSkillIcon[allItemIdx];
    InventoryExtraSkill displaySkill = this.DisplaySkills[inventoryIdx];
    extraSkillIcon.ClickAction = (Action<InventoryExtraSkill>) (skill => this.SelectItemProc(skill));
    extraSkillIcon.ForBattle = displaySkill.forBattle;
    extraSkillIcon.Favorite = displaySkill.favorite;
    extraSkillIcon.Gray = this.IsGrayIcon(displaySkill);
    if (this.DisableTouchIcon(displaySkill))
    {
      extraSkillIcon.ClickAction = (Action<InventoryExtraSkill>) (_ => { });
      extraSkillIcon.Gray = true;
    }
    if (displaySkill.select)
    {
      switch (this.SelectMode)
      {
        case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num:
          extraSkillIcon.Select(displaySkill.index - 1);
          break;
        case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check:
          extraSkillIcon.SelectByCheckIcon();
          break;
        case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2:
          extraSkillIcon.SelectByCheck2Icon();
          break;
      }
    }
    else
      extraSkillIcon.Deselect();
  }

  protected override IEnumerator InitExtension()
  {
    this.SelectItemList.Clear();
    yield break;
  }

  protected override void AllItemIconUpdate()
  {
    foreach (ExtraSkillIcon extraSkillIcon in this.AllExtraSkillIcon)
    {
      ExtraSkillIcon skillIcon = extraSkillIcon;
      InventoryExtraSkill inventoryExtraSkill = this.InventoryExtraSkills.FirstOrDefault<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x == skillIcon.InvExtraSkill));
      if (inventoryExtraSkill != null)
      {
        if (inventoryExtraSkill.select)
        {
          switch (this.SelectMode)
          {
            case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num:
              skillIcon.Select(inventoryExtraSkill.index - 1);
              break;
            case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check:
              skillIcon.SelectByCheckIcon();
              break;
            case Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Check2:
              skillIcon.SelectByCheck2Icon();
              break;
          }
          skillIcon.Gray = this.IsGrayIcon(inventoryExtraSkill);
        }
        else
        {
          skillIcon.Deselect();
          skillIcon.Gray = this.IsGrayIcon(inventoryExtraSkill);
        }
      }
    }
  }

  public override IEnumerator Init()
  {
    IEnumerator e = base.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void UpdateSelectItemIndex()
  {
    if (this.SelectMode != Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num)
      return;
    int count = this.SelectItemList.Count;
    for (int index = 0; index < count; ++index)
      this.SelectItemList[index].index = index + 1;
  }

  public bool IsSelectItem(InventoryExtraSkill invItem)
  {
    return this.SelectItemList.Any<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x == invItem));
  }

  protected void AddSelectItem(InventoryExtraSkill invItem, bool isGray = true)
  {
    if (invItem == null || this.SelectItemList.Any<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x == invItem)))
      return;
    invItem.select = true;
    invItem.Gray = isGray;
    invItem.index = 0;
    if (this.SelectMode == Unit004SelectExtraSkillListMenuBase.SelectModeEnum.Num)
      invItem.index = this.SelectItemList.Count<InventoryExtraSkill>() + 1;
    this.SelectItemList.Add(invItem);
  }

  public void RemoveSelectItem(InventoryExtraSkill invItem)
  {
    if (invItem == null || !this.SelectItemList.Any<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x == invItem)))
      return;
    invItem.select = false;
    invItem.Gray = false;
    invItem.index = 0;
    this.SelectItemList.Remove(invItem);
  }

  public void RemoveSelectItem(int idx)
  {
    this.RemoveSelectItem(this.InventoryExtraSkills.FirstOrDefault<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => x.index == idx)));
  }

  public void ClearSelectItem()
  {
    foreach (InventoryExtraSkill selectItem in this.SelectItemList)
    {
      selectItem.select = false;
      selectItem.Gray = false;
      selectItem.index = 0;
    }
    this.SelectItemList.Clear();
    this.DisplayIconAndBottomInfoUpdate();
  }

  public virtual void IbtnClear()
  {
    if (this.IsPush)
      return;
    this.ClearSelectItem();
  }

  public void UpdateSelectItemIndexWithInfo()
  {
    this.UpdateSelectItemIndex();
    this.DisplayIconAndBottomInfoUpdate();
  }

  public virtual void changeSceneExtraSkillEquipment(
    PlayerUnit targetUnit,
    PlayerAwakeSkill targetSkill)
  {
  }

  public virtual void changeExtraSkillFavorite(InventoryExtraSkill skill, bool favorite)
  {
  }

  public virtual void onClickDecision()
  {
  }

  protected enum SelectModeEnum
  {
    Num,
    Check,
    Check2,
  }
}
