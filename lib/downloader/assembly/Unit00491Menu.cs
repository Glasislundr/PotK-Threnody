// Decompiled with JetBrains decompiler
// Type: Unit00491Menu
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
public class Unit00491Menu : UnitMenuBase
{
  private bool IsToucn = true;
  private Player player;
  private PlayerUnit[] playerUnits;
  private PlayerMaterialUnit[] playerMaterialUnits;
  private bool isEquip;
  private Unit00491Menu.Mode mode;
  private int unitCount;
  private GameObject SaveMemorySlotSelectPrefab;
  [SerializeField]
  private UISprite EvolutionSortSprite;
  [SerializeField]
  private UISprite TransSortSprite;

  public void EnableTouch() => this.Invoke("Touch", 1f);

  private void Touch() => this.IsToucn = true;

  public bool IsInit()
  {
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Count<PlayerUnit>() != this.unitCount;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  private void InitializeAllUnitInfosExtend()
  {
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      allUnitInfo.is_awakeUnti = allUnitInfo.playerUnit.unit.CanAwakeUnitFlag;
  }

  public IEnumerator Init(
    Player player,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    bool isEquip,
    Unit00491Menu.Mode mode)
  {
    Unit00491Menu unit00491Menu = this;
    unit00491Menu.mode = mode;
    unit00491Menu.player = player;
    unit00491Menu.playerUnits = playerUnits;
    unit00491Menu.playerMaterialUnits = playerMaterialUnits;
    unit00491Menu.isEquip = isEquip;
    IEnumerator e = unit00491Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00491Menu.unitCount = SMManager.Get<PlayerUnit[]>().Length;
    int nums = 0;
    if (mode == Unit00491Menu.Mode.Evolution)
    {
      unit00491Menu.SortSprite = unit00491Menu.EvolutionSortSprite;
      unit00491Menu.isDispOnlyNormalUnit = false;
      ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsEvolution || x.unit.CanAwakeUnitFlag)).ToArray<PlayerUnit>();
      PlayerMaterialUnit[] array = ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.unit.IsEvolution)).ToArray<PlayerMaterialUnit>();
      unit00491Menu.TxtPossessionUnit.SetTextLocalize(Consts.GetInstance().unit_004_9_1_evolution_possession_text);
      unit00491Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) null, (IEnumerable<PlayerMaterialUnit>) array, UnitSortAndFilter.SORT_TYPES.UnitID, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, isEquip, false, true, true, true, false, new Action(unit00491Menu.InitializeAllUnitInfosExtend), false, false);
      nums = array.Length;
    }
    else
    {
      unit00491Menu.SortSprite = unit00491Menu.TransSortSprite;
      if (PlayerUnitTransMigrateMemoryList.Current == null && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      {
        e = WebAPI.UnitListTransmigrateMemory().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Future<GameObject> prefabF = (Future<GameObject>) null;
      if (Object.op_Equality((Object) unit00491Menu.SaveMemorySlotSelectPrefab, (Object) null))
      {
        prefabF = Res.Prefabs.popup.popup_004_save_memory_slot_select__anim_popup01.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unit00491Menu.SaveMemorySlotSelectPrefab = prefabF.Result;
      }
      int?[] memoryIds = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
      unit00491Menu.isDispOnlyNormalUnit = true;
      PlayerUnit[] array = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.level >= x.unit.rarity.reincarnation_level || ((IEnumerable<int?>) memoryIds).Any<int?>((Func<int?, bool>) (y =>
      {
        int? nullable = y;
        int id = x.id;
        return nullable.GetValueOrDefault() == id & nullable.HasValue;
      })))).ToArray<PlayerUnit>();
      nums = array.Length;
      unit00491Menu.TxtPossessionUnit.SetTextLocalize(Consts.GetInstance().unit_004_9_1_trans_possession_text);
      unit00491Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) array, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit004912SortAndFilter, isEquip, false, true, true, true, false);
      unit00491Menu.IsRecord = true;
      prefabF = (Future<GameObject>) null;
    }
    e = unit00491Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00491Menu.TxtNumber.SetTextLocalize(string.Format("{0}", (object) nums));
    Singleton<PopupManager>.GetInstance().closeAll();
    unit00491Menu.lastReferenceUnitID = -1;
    unit00491Menu.InitializeEnd();
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
  }

  public override IEnumerator UpdateInfoAndScroll(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] materialUnits = null)
  {
    Unit00491Menu unit00491Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00491Menu.\u003C\u003En__0((PlayerUnit[]) null, materialUnits);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00491Menu.TxtNumber.SetTextLocalize(string.Format("{0}", (object) materialUnits.Length));
  }

  private void SelectUnitIcon(UnitIconBase unitIcon)
  {
    if (!this.IsToucn)
      return;
    if (unitIcon.PlayerUnit != (PlayerUnit) null)
    {
      this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
      if (this.mode == Unit00491Menu.Mode.Evolution)
      {
        if (!unitIcon.unit.CanAwakeUnitFlag)
          Unit00499Scene.changeScene(true, unitIcon.PlayerUnit, Unit00499Scene.Mode.Evolution);
        else if (unitIcon.unit.ID == 101414)
          Unit00499Scene.changeScene(true, unitIcon.PlayerUnit, Unit00499Scene.Mode.AwakeUnit);
        else
          Unit00499Scene.changeScene(true, unitIcon.PlayerUnit, Unit00499Scene.Mode.CommonAwakeUnit);
      }
      else
        Unit00499Scene.changeScene(true, unitIcon.PlayerUnit, Unit00499Scene.Mode.Transmigration);
    }
    else
      Debug.LogWarning((object) "PlayerUnit Null : Unit00491Menu");
    this.IsToucn = false;
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Unit00491Menu unit00491Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00491Menu.\u003C\u003En__1(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    unit00491Menu.allUnitIcons[unit_index].onClick = new Action<UnitIconBase>(unit00491Menu.\u003CCreateUnitIcon\u003Eb__19_0);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.allUnitIcons[unit_index].onClick = (Action<UnitIconBase>) (ui => this.SelectUnitIcon(ui));
  }

  public void IbtnRegression()
  {
    if (this.IsPushAndSet())
      return;
    Unit004RegressionScene.changeScene();
  }

  public void IbtnRecord()
  {
    this.StartCoroutine(Singleton<PopupManager>.GetInstance().open(this.SaveMemorySlotSelectPrefab).GetComponent<Unit00499SaveMemorySlotSelect>().Initialize((PlayerUnit) null, new Action(this.RecordEndUpdate)));
  }

  public void RecordEndUpdate()
  {
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    int?[] transmigrate_memory_player_unit_ids = SMManager.Get<PlayerTransmigrateMemoryPlayerUnitIds>().transmigrate_memory_player_unit_ids;
    this.playerUnits = ((IEnumerable<PlayerUnit>) source).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.level >= x.unit.rarity.reincarnation_level || ((IEnumerable<int?>) transmigrate_memory_player_unit_ids).Any<int?>((Func<int?, bool>) (y =>
    {
      int? nullable = y;
      int id = x.id;
      return nullable.GetValueOrDefault() == id & nullable.HasValue;
    })))).ToArray<PlayerUnit>();
    this.StartCoroutine(this.Init(this.player, this.playerUnits, playerMaterialUnits, this.isEquip, this.mode));
  }

  protected override void SortAndSetIcons(
    UnitSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isBattleFirst,
    bool isTowerEntry)
  {
    bool flag = Object.op_Inequality((Object) this.SortSprite, (Object) null);
    if (flag)
      this.UpdateSortSprite(type);
    if (this.allUnitIcons.Count < 1)
      return;
    if (flag)
    {
      this.displayUnitInfos = this.displayUnitInfos.SortBy(type, order, isBattleFirst, isTowerEntry).ToList<UnitIconInfo>();
      if (this.mode == Unit00491Menu.Mode.Trans)
      {
        int?[] memoryIds = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
        for (int i = memoryIds.Length - 1; i >= 0; i--)
        {
          if (memoryIds[i].HasValue)
            this.displayUnitInfos = this.displayUnitInfos.OrderByDescending<UnitIconInfo, bool>((Func<UnitIconInfo, bool>) (x =>
            {
              int? nullable = memoryIds[i];
              int id = x.playerUnit.id;
              return nullable.GetValueOrDefault() == id & nullable.HasValue;
            })).ToList<UnitIconInfo>();
        }
      }
    }
    this.scroll.Reset();
    this.allUnitIcons.ForEach((Action<UnitIconBase>) (x =>
    {
      ((Component) x).transform.parent = ((Component) this).transform;
      ((Component) x).gameObject.SetActive(false);
    }));
    for (int index = 0; index < Mathf.Min(this.IconMaxValue, this.displayUnitInfos.Count); ++index)
    {
      this.scroll.Add(((Component) this.allUnitIcons[index]).gameObject, this.IconWidth, this.IconHeight);
      ((Component) this.allUnitIcons[index]).gameObject.SetActive(true);
      if (this.allUnitIcons[index].unit != null && this.allUnitIcons[index].unit.IsMaterialUnit)
      {
        this.allUnitIcons[index].SetCounter(this.displayUnitInfos[index].count);
        this.allUnitIcons[index].SetSelectionCounter(this.displayUnitInfos[index].SelectedCount);
      }
    }
    int targetUnitID = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID;
    if (targetUnitID != -1)
    {
      this.scroll.CreateScrollPoint(this.IconHeight, this.displayUnitInfos.Count);
      int? nullable = this.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.removeButton)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).FirstIndexOrNull<int>((Func<int, bool>) (x => x == targetUnitID));
      if (nullable.HasValue)
      {
        this.scroll.ResolvePosition(nullable.Value, this.displayUnitInfos.Count<UnitIconInfo>());
      }
      else
      {
        int referenceUnitIndex = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex;
        if (referenceUnitIndex != -1 && referenceUnitIndex < this.displayUnitInfos.Count)
          this.scroll.ResolvePosition(referenceUnitIndex, this.displayUnitInfos.Count<UnitIconInfo>());
        else
          this.scroll.ResolvePosition(this.displayUnitInfos.Count<UnitIconInfo>() - 1, this.displayUnitInfos.Count<UnitIconInfo>());
      }
    }
    else
    {
      this.scroll.CreateScrollPoint(this.IconHeight, this.displayUnitInfos.Count);
      this.scroll.ResolvePosition();
    }
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
  }

  public override void IbtnBack() => base.IbtnBack();

  public enum Mode
  {
    Evolution,
    Trans,
  }
}
