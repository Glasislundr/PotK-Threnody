// Decompiled with JetBrains decompiler
// Type: Quest0529Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0529Menu : UnitSelectMenuBase
{
  [SerializeField]
  private UILabel PossessionUnit;
  private EarthCharacter[] selectPlayerUnitsCache;
  private GameObject supplyIcon;
  [SerializeField]
  private Transform[] SuppleIconPositions;
  private bool isEventQuest;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();

  public IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.supplyIcon, (Object) null))
    {
      Future<GameObject> supplyIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      IEnumerator e = supplyIconF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.supplyIcon = supplyIconF.Result;
      supplyIconF = (Future<GameObject>) null;
    }
  }

  private void InitializeAllUnitInfosEx(EarthCharacter[] selectEarthPlayerUnits)
  {
    bool flag = this.selectedUnitIcons.Count<UnitIconInfo>() == 0;
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      info.for_battle = false;
      if (flag)
      {
        info.select = -1;
        info.gray = false;
        EarthCharacter earthCharacter = ((IEnumerable<EarthCharacter>) selectEarthPlayerUnits).FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.GetPlayerUnit().id == info.playerUnit.id));
        if (earthCharacter != null)
        {
          info.gray = true;
          info.select = !this.isEventQuest ? earthCharacter.battleIndex : earthCharacter.eventQuestBattleIndex;
          this.selectedUnitIcons.Add(info);
        }
      }
      else
      {
        info.select = -1;
        info.gray = false;
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.gray = true;
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(false);
  }

  protected void CreateSelectUnitList(bool updateIndex = true)
  {
    this.selectedUnitIcons.Clear();
    this.selectedUnitIcons = this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.select >= 0)).OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ToList<UnitIconInfo>();
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      if (updateIndex)
        u.select = n;
      this.UnitInfoUpdate(u, true, u.select);
    }));
  }

  private void UpdateSelectUnitText()
  {
    this.PossessionUnit.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_054_6_8_Possession, (IDictionary) new Hashtable()
    {
      {
        (object) "select",
        (object) this.selectedUnitIcons.Count<UnitIconInfo>()
      },
      {
        (object) "possession",
        (object) this.SelectMax
      }
    }));
  }

  public IEnumerator Initialize(PlayerUnit[] playerUnits, int maxNum)
  {
    Quest0529Menu quest0529Menu = this;
    quest0529Menu.SetIconType(UnitMenuBase.IconType.EarthNormal);
    quest0529Menu.isEventQuest = false;
    if (maxNum == -1)
    {
      quest0529Menu.SelectMax = Singleton<EarthDataManager>.GetInstance().questProgress.MaximumNumberOfSorties;
      quest0529Menu.selectPlayerUnitsCache = ((IEnumerable<EarthCharacter>) Singleton<EarthDataManager>.GetInstance().characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.battleIndex != 0)).ToArray<EarthCharacter>();
    }
    else
    {
      quest0529Menu.isEventQuest = true;
      quest0529Menu.SelectMax = maxNum;
      quest0529Menu.selectPlayerUnitsCache = ((IEnumerable<EarthCharacter>) Singleton<EarthDataManager>.GetInstance().characters).Where<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.eventQuestBattleIndex != 0)).ToArray<EarthCharacter>();
    }
    IEnumerator e = quest0529Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    quest0529Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, (Persist<Persist.UnitSortAndFilterInfo>) null, false, false, false, false, false, new Action(quest0529Menu.\u003CInitialize\u003Eb__11_2));
    e = quest0529Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0529Menu.UpdateSelectUnitText();
    quest0529Menu.InitializeEnd();
  }

  private void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    int num = !this.isEventQuest ? Singleton<EarthDataManager>.GetInstance().questProgress.forcedSortieCharacterMaxPosition : 1;
    if (this.selectedUnitIcons.Count<UnitIconInfo>() >= this.SelectMax || num >= this.SelectMax)
      unitIcon.Gray = !info.gray;
    else
      unitIcon.Gray = info.gray;
    if (info.select != -1)
      return;
    this.Deselect(unitIcon);
    unitIcon.HiddenEarthUnitNumberIcon(false);
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo info = this.displayUnitInfos[info_index];
    info.gray = false;
    if (info.select >= 0)
    {
      info.gray = true;
      info.icon.HideCheckIcon();
      info.icon.DispEarthUnitNumberIcon(info.select, info.select == 1, false);
    }
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui =>
    {
      if (!this.isEventQuest && info.select >= 1 && info.select <= Singleton<EarthDataManager>.GetInstance().questProgress.forcedSortieCharacterMaxPosition)
        return;
      this.Select(ui);
    });
    ((UnitIcon) allUnitIcon).SetEarthButtonDetalEvent(info.playerUnit, this.getUnits());
    if (info.button_enable)
    {
      ((Behaviour) allUnitIcon.Button).enabled = true;
    }
    else
    {
      ((Behaviour) allUnitIcon.Button).enabled = false;
      info.gray = true;
    }
    if (this.selectedUnitIcons.Count >= this.SelectMax)
      allUnitIcon.Gray = !info.gray;
    else
      this.IconGraySetting(allUnitIcon, info);
  }

  public override void UpdateSelectIcon()
  {
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(u.playerUnit);
      if (unitInfoDisplay == null || !Object.op_Inequality((Object) unitInfoDisplay.icon, (Object) null))
        return;
      unitInfoDisplay.gray = true;
      unitInfoDisplay.icon.Select(unitInfoDisplay.select);
      unitInfoDisplay.icon.HideCheckIcon();
      unitInfoDisplay.icon.DispEarthUnitNumberIcon(unitInfoDisplay.select, unitInfoDisplay.select == 1, false);
    }));
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (Object.op_Inequality((Object) displayUnitInfo.icon, (Object) null))
        this.IconGraySetting(displayUnitInfo.icon, displayUnitInfo);
    }
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (unitIcon.Selected)
    {
      this.Deselect(unitIcon);
      unitIcon.HiddenEarthUnitNumberIcon(false);
      this.UpdateSelectIcon();
      this.UpdateSelectUnitText();
    }
    else
    {
      int min = !this.isEventQuest ? Singleton<EarthDataManager>.GetInstance().questProgress.forcedSortieCharacterMaxPosition : 1;
      if (this.selectedUnitIcons.Count >= this.SelectMax || min >= this.SelectMax)
        return;
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(unitIcon.PlayerUnit);
      if (unitInfoDisplay != null)
      {
        unitIcon.HideCheckIcon();
        unitIcon.DispEarthUnitNumberIcon(unitInfoDisplay.select, unitInfoDisplay.select == 1, false);
        unitIcon.Select(this.GetMinSelectIndex(min));
        this.UnitInfoUpdate(unitInfoDisplay, true, unitIcon.SelIndex);
      }
      UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIcon.PlayerUnit);
      if (unitInfoAll != null)
      {
        this.UnitInfoUpdate(unitInfoAll, true, unitIcon.SelIndex);
        this.selectedUnitIcons.Add(unitInfoAll);
      }
      this.UpdateSelectIcon();
      this.UpdateSelectUnitText();
    }
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  public IEnumerator DispSupplyDeck(PlayerItem[] supplys)
  {
    IEnumerator e;
    for (int i = 0; i < Consts.GetInstance().ITEM_EXTEND_VALUE; ++i)
    {
      if (i < supplys.Length)
      {
        e = this.CreateSupplyIcon(supplys[i], i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.CreateSupplyIcon((PlayerItem) null, i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator CreateSupplyIcon(PlayerItem supply, int pos)
  {
    ItemIcon itemIcon = ((Component) this.SuppleIconPositions[pos]).GetComponentInChildren<ItemIcon>();
    if (Object.op_Equality((Object) itemIcon, (Object) null))
      itemIcon = this.supplyIcon.Clone(this.SuppleIconPositions[pos]).GetComponent<ItemIcon>();
    if (supply == (PlayerItem) null)
    {
      itemIcon.SetEmpty(true);
    }
    else
    {
      itemIcon.SetEmpty(false);
      IEnumerator e = itemIcon.InitByPlayerItem(supply);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    EarthCharacter[] characters = Singleton<EarthDataManager>.GetInstance().characters;
    foreach (EarthCharacter earthCharacter in characters)
    {
      if (this.isEventQuest)
        earthCharacter.SetEventQuestBattleIndex(0);
      else
        earthCharacter.SetBattleIndex(0);
    }
    foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
    {
      UnitIconInfo unit = selectedUnitIcon;
      EarthCharacter earthCharacter = ((IEnumerable<EarthCharacter>) characters).FirstOrDefault<EarthCharacter>((Func<EarthCharacter, bool>) (x => x.GetPlayerUnit().id == unit.playerUnit.id));
      if (earthCharacter != null)
      {
        if (this.isEventQuest)
          earthCharacter.SetEventQuestBattleIndex(unit.select);
        else
          earthCharacter.SetBattleIndex(unit.select);
      }
    }
    if (!this.isEventQuest)
      Singleton<EarthDataManager>.GetInstance().Save();
    this.backScene();
  }

  public void IbtnClear()
  {
    if (this.IsPushAndSet())
      return;
    this.selectedUnitIcons.Clear();
    this.InitializeAllUnitInfosEx(this.selectPlayerUnitsCache);
    this.UpdateSelectIcon();
    this.UpdateSelectUnitText();
    this.IsPush = false;
  }

  public override void IbtnBack() => base.IbtnBack();

  public void IbtnChange()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllSupplies()).ToList<PlayerItem>().ToArray(), ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllBattleSupplies()).ToArray<PlayerItem>())).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = false,
      mode = Quest00210Scene.Mode.Earth
    });
  }
}
