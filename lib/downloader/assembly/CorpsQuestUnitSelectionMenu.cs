// Decompiled with JetBrains decompiler
// Type: CorpsQuestUnitSelectionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeckOrganization;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/UnitSelectionMenu")]
public class CorpsQuestUnitSelectionMenu : UnitSelectMenuBase
{
  [SerializeField]
  private UIButton btnDecide;
  [SerializeField]
  private GameObject dirRule;
  [SerializeField]
  private UILabel txtRule;
  [SerializeField]
  private UISprite slc_box_Unit_Formation;
  private const int c_slc_box_Unit_Formation_sizeW = 720;
  private const int c_slc_box_Unit_Formation_sizeH = 261;
  private const int c_slc_box_Unit_Formation_sizeH_norule = 208;
  private GameObject goHpGauge;
  private PlayerUnit[] playerUnits;
  private List<PlayerUnit> selectedUnits;
  private bool IsAutoSelectProcess;
  private CorpsUtil.SequenceType sequenceType;
  private bool isCheckSelectionMode;
  private PlayerCorps playerCorps;
  private CorpsSetting setting_;
  protected Func<PlayerUnit, bool> ruleChecker;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();

  private PlayerUnit[] getRegulationUnits()
  {
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
    {
      if (playerUnit.unit.IsNormalUnit && playerUnit.total_level >= this.setting_.min_unit_level && (this.ruleChecker == null || this.ruleChecker(playerUnit)))
        playerUnitList.Add(playerUnit);
    }
    return playerUnitList.ToArray();
  }

  private IEnumerator AutoSelectAsync()
  {
    CorpsQuestUnitSelectionMenu unitSelectionMenu = this;
    unitSelectionMenu.IsAutoSelectProcess = true;
    CorpsCreator deckCreator = new CorpsCreator(unitSelectionMenu.getRegulationUnits(), unitSelectionMenu.SelectMax);
    IEnumerator e = deckCreator.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.selectedUnits = deckCreator.result_;
    e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.UpdateInfomation();
  }

  private IEnumerator ExecUpdateInfoAndScroll()
  {
    CorpsQuestUnitSelectionMenu unitSelectionMenu = this;
    IEnumerator e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.UpdateInfomation();
  }

  public IEnumerator OnStartSceneAsync(
    PlayerCorps corps,
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestUnitSelectionMenu unitSelectionMenu = this;
    MasterData.CorpsSetting.TryGetValue(corps.corps_id, out unitSelectionMenu.setting_);
    unitSelectionMenu.ruleChecker = (Func<PlayerUnit, bool>) null;
    if (unitSelectionMenu.setting_.rule_no > 0)
    {
      unitSelectionMenu.ruleChecker = BattleUnitRule.createChecker(unitSelectionMenu.setting_.rule_no);
      unitSelectionMenu.dirRule.SetActive(true);
      unitSelectionMenu.txtRule.SetTextLocalize(unitSelectionMenu.setting_.rule_detail);
      ((UIWidget) unitSelectionMenu.slc_box_Unit_Formation).SetDimensions(720, 261);
    }
    else
    {
      unitSelectionMenu.dirRule.SetActive(false);
      ((UIWidget) unitSelectionMenu.slc_box_Unit_Formation).SetDimensions(720, 208);
    }
    unitSelectionMenu.SelectMax = unitSelectionMenu.setting_.max_unit_count;
    unitSelectionMenu.playerUnits = unitSelectionMenu.getRegulationUnits();
    unitSelectionMenu.selectedUnits = new List<PlayerUnit>();
    unitSelectionMenu.playerCorps = corps;
    unitSelectionMenu.sequenceType = type;
    IEnumerator e = unitSelectionMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.SetIconType(UnitMenuBase.IconType.NormalWithHpGauge);
    unitSelectionMenu.InitializeInfo((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.corpsUnitListSortAndFilter, false, false, false, true, false, new Action(unitSelectionMenu.InitializeAllUnitInfosExtend));
    e = unitSelectionMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitSelectionMenu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) 0, (object) unitSelectionMenu.SelectMax));
    if (unitSelectionMenu.lastReferenceUnitID != -1)
    {
      unitSelectionMenu.InitializeEnd();
      yield return (object) null;
      unitSelectionMenu.resolveScrollPosition(unitSelectionMenu.lastReferenceUnitID);
      yield return (object) null;
      unitSelectionMenu.setLastReference();
    }
    ((UIButtonColor) unitSelectionMenu.btnDecide).isEnabled = unitSelectionMenu.selectedUnits.Count > 0;
    unitSelectionMenu.isCheckSelectionMode = true;
  }

  public IEnumerator OnBackSceneAsync(CorpsUtil.UnitSelectionMode mode, CorpsUtil.SequenceType type)
  {
    CorpsQuestUnitSelectionMenu unitSelectionMenu = this;
    unitSelectionMenu.playerUnits = unitSelectionMenu.getRegulationUnits();
    IEnumerator e = unitSelectionMenu.UpdateInfoAndScroll(((IEnumerable<PlayerUnit>) unitSelectionMenu.playerUnits).ToArray<PlayerUnit>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(CorpsUtil.UnitSelectionMode mode)
  {
    if (!this.isInitialize)
      this.InitializeEnd();
    if (!this.isCheckSelectionMode)
      return;
    this.isCheckSelectionMode = false;
    if (mode != CorpsUtil.UnitSelectionMode.Auto)
      return;
    this.StartCoroutine(this.AutoSelectAsync());
  }

  public override void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(SMManager.Get<PlayerItem[]>().AllSupplies(), this.playerCorps.supplies)).ToList<SupplyItem>();
    this.SaveDeck = this.SupplyItems.Copy();
    Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      removeButton = false,
      limitedOnly = true,
      mode = Quest00210Scene.Mode.Corps
    }, this.selectedUnitIcons.Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>(), this.playerCorps, this.sequenceType);
  }

  public override void IbtnClearS()
  {
    this.selectedUnits.Clear();
    base.IbtnClearS();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onClickUnitlIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  public override void UpdateInfomation()
  {
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) this.selectedUnitIcons.Count, (object) this.SelectMax));
    ((UIButtonColor) this.btnDecide).isEnabled = this.selectedUnitIcons.Count > 0;
  }

  public void InitializeAllUnitInfosExtend()
  {
    if (!this.IsAutoSelectProcess)
    {
      this.selectedUnitIcons.Clear();
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        UnitIconInfo info = allUnitInfo;
        int? nullable = this.selectedUnits.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    else
    {
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        UnitIconInfo info = allUnitInfo;
        int? nullable = this.selectedUnits.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          this.selectedUnitIcons.Add(info);
        }
      }
      this.IsAutoSelectProcess = false;
    }
  }

  protected override IEnumerator CreateUnitIconBase(GameObject prefab)
  {
    CorpsQuestUnitSelectionMenu unitSelectionMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unitSelectionMenu.\u003C\u003En__0(prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) unitSelectionMenu.goHpGauge, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitSelectionMenu.goHpGauge = f.Result;
      f = (Future<GameObject>) null;
    }
    for (int index = 0; index < unitSelectionMenu.allUnitIcons.Count; ++index)
    {
      UnitIcon allUnitIcon = (UnitIcon) unitSelectionMenu.allUnitIcons[index];
      unitSelectionMenu.goHpGauge.Clone(allUnitIcon.hp_gauge.transform);
      allUnitIcon.HpGauge.TweenHpGauge.setValue(100, 100, false);
    }
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (unitIcon.Gray && !unitIcon.Selected)
      return;
    base.Select(unitIcon);
    if (unitIcon.Selected)
      this.selectedUnits.Add(unitIcon.PlayerUnit);
    else
      this.selectedUnits.Remove(unitIcon.PlayerUnit);
    ((UIButtonColor) this.btnDecide).isEnabled = this.selectedUnits.Count > 0;
  }

  protected override void Sort(SortInfo info)
  {
    base.Sort(info);
    this.StartCoroutine(this.ExecUpdateInfoAndScroll());
  }
}
