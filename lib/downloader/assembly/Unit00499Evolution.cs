// Decompiled with JetBrains decompiler
// Type: Unit00499Evolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00499Evolution : NGMenuBase
{
  public Unit00499Menu menu;
  public NGHorizontalScrollParts indicator;
  public GameObject unitIconPrefab;
  protected GameObject indicatorPrefab;
  private bool fromEarth;
  public int selectIndicator;
  private float positionIndicator;
  private bool realityEvolutionButton;
  private List<int> orderList;
  protected Dictionary<int, UnitUnit[]> evolutionMaterialDict;
  protected Dictionary<int, GameObject[]> linkEvolutionUnitsDict;
  public PlayerUnit basePlayUnit;
  public PlayerUnit[] afterPlayerUnits;
  private const int SINGLE_PATTERN_EVOLUTION = 1;

  public bool isMovingIndicator { get; private set; }

  private bool isEvolutionButtonEnabled => this.realityEvolutionButton && !this.isMovingIndicator;

  private IEnumerator WaitScrollSe()
  {
    this.indicator.SeEnable = true;
    yield return (object) null;
  }

  private void Update()
  {
    this.isMovingIndicator = false;
    int selected = this.indicator.selected;
    if (this.orderList == null || selected >= this.orderList.Count || selected < 0)
      return;
    if (this.orderList.Count > 1 && (double) this.positionIndicator != (double) this.indicator.scrollView.transform.localPosition.x)
    {
      this.isMovingIndicator = true;
      this.positionIndicator = this.indicator.scrollView.transform.localPosition.x;
    }
    if (this.orderList[selected] != this.selectIndicator)
    {
      this.selectIndicator = this.orderList[selected];
      this.menu.HideMaterialQuestInfo();
      this.StopCoroutine("processByswipeIndicator");
      this.standbyEvolutionButton();
      this.StartCoroutine("processByswipeIndicator", (object) selected);
    }
    else
    {
      if (((UIButtonColor) this.menu.evolutionBtn).isEnabled == this.isEvolutionButtonEnabled)
        return;
      ((UIButtonColor) this.menu.evolutionBtn).isEnabled = this.isEvolutionButtonEnabled;
    }
  }

  private IEnumerator processByswipeIndicator(int selectedIdx)
  {
    this.menu.selectEvolutionPatternId = this.selectIndicator;
    this.menu.materialUnitIds = this.menu.getEvolutionMaterialSelectedUnitIds(this.selectIndicator);
    this.menu.materialMaterialUnitIds = this.menu.getEvolutionMaterialSelectedMaterialIds(this.selectIndicator);
    this.menu.linkEvolutionUnits = this.linkEvolutionUnitsDict[this.selectIndicator];
    IEnumerator e = this.menu.InitPlayer(this.basePlayUnit, this.afterPlayerUnits[selectedIdx], this.unitIconPrefab, this.fromEarth);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CheckEvolutionPossible();
  }

  public IEnumerator Init(PlayerUnit basePlayUnit, PlayerUnit[] afterPlayerUnits, bool fromEarth = false)
  {
    Unit00499Evolution unit00499Evolution = this;
    unit00499Evolution.basePlayUnit = basePlayUnit;
    unit00499Evolution.afterPlayerUnits = afterPlayerUnits;
    unit00499Evolution.evolutionMaterialDict = basePlayUnit.unit.EvolutionUnits;
    unit00499Evolution.linkEvolutionUnitsDict = new Dictionary<int, GameObject[]>();
    unit00499Evolution.fromEarth = fromEarth;
    unit00499Evolution.isMovingIndicator = false;
    unit00499Evolution.standbyEvolutionButton();
    unit00499Evolution.orderList = unit00499Evolution.SetMaterialUnitList();
    // ISSUE: reference to a compiler-generated method
    int lastIndex = unit00499Evolution.orderList.FindIndex(new Predicate<int>(unit00499Evolution.\u003CInit\u003Eb__23_0));
    if (lastIndex < 0)
    {
      unit00499Evolution.selectIndicator = unit00499Evolution.orderList.First<int>();
      lastIndex = 0;
    }
    IEnumerator e = unit00499Evolution.createPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00499Evolution.indicator.SeEnable = false;
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    unit00499Evolution.indicator.destroyParts(false);
    foreach (int order in unit00499Evolution.orderList)
    {
      e = unit00499Evolution.CreateIndicator(order, playerUnits, playerMaterialUnits);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unit00499Evolution.indicator.resetScrollView();
    unit00499Evolution.indicator.setItemPositionQuick(lastIndex);
    unit00499Evolution.positionIndicator = unit00499Evolution.indicator.scrollView.transform.localPosition.x;
    e = unit00499Evolution.processByswipeIndicator(lastIndex);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) unit00499Evolution.indicator.dot).gameObject.SetActive(true);
    if (unit00499Evolution.orderList.Count<int>() == 1)
      ((Component) unit00499Evolution.indicator.dot).gameObject.SetActive(false);
    unit00499Evolution.StartCoroutine(unit00499Evolution.WaitScrollSe());
  }

  private void standbyEvolutionButton()
  {
    ((UIButtonColor) this.menu.evolutionBtn).isEnabled = false;
    this.realityEvolutionButton = false;
  }

  private List<int> SetMaterialUnitList()
  {
    List<int> intList = new List<int>();
    foreach (int key in this.basePlayUnit.unit.EvolutionUnits.Keys)
      intList.Add(key);
    return intList;
  }

  private IEnumerator createPrefab()
  {
    Future<GameObject> indicatorPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.indicatorPrefab, (Object) null))
    {
      indicatorPrefabF = Res.Prefabs.unit004_9_9.dir_Evolution.Load<GameObject>();
      e = indicatorPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.indicatorPrefab = indicatorPrefabF.Result;
      indicatorPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      indicatorPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = indicatorPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = indicatorPrefabF.Result;
      indicatorPrefabF = (Future<GameObject>) null;
    }
  }

  protected virtual IEnumerator CreateIndicator(
    int evolutionPatternId,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits)
  {
    Unit00499EvolutionIndicator component = this.indicator.instantiateParts(this.indicatorPrefab).GetComponent<Unit00499EvolutionIndicator>();
    this.linkEvolutionUnitsDict[evolutionPatternId] = component.linkEvolutionUnits;
    Player player = SMManager.Get<Player>();
    UnitEvolutionPattern evolutionPattern = ((IEnumerable<UnitEvolutionPattern>) this.basePlayUnit.unit.EvolutionPattern).First<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (p => p.ID == evolutionPatternId));
    ((UIWidget) component.TxtZeny).color = player.money >= (long) evolutionPattern.money ? Color.white : Color.red;
    component.TxtZeny.SetTextLocalize(evolutionPattern.money.ToString());
    IEnumerator e = this.menu.InitEvolutionUnits(playerUnits, playerMaterialUnits, this.unitIconPrefab, component, evolutionPatternId, this.evolutionMaterialDict[evolutionPatternId]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void CheckEvolutionPossible()
  {
    UnitEvolutionPattern evolutionPattern = ((IEnumerable<UnitEvolutionPattern>) this.basePlayUnit.unit.EvolutionPattern).First<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (p => p.ID == this.selectIndicator));
    if (this.basePlayUnit.level < evolutionPattern.threshold_level && this.basePlayUnit.unit.IsNormalUnit)
      this.menu.isLevel = false;
    if (this.basePlayUnit.unit.CanAwakeUnitFlag && this.basePlayUnit.unit.ID != 101414 && (double) this.basePlayUnit.unityTotal < (double) evolutionPattern.threshold_unity_value)
      this.menu.isPlusValue = false;
    this.menu.updateCheckEnableButton(this.selectIndicator);
    this.realityEvolutionButton = this.menu.CheckEnabledButton(evolutionPattern.money);
    if (this.menu.SceneMode == Unit00499Scene.Mode.Evolution || this.menu.SceneMode == Unit00499Scene.Mode.EarthEvolution)
      ((UIButtonColor) this.menu.evolutionBtn).isEnabled = this.isEvolutionButtonEnabled;
    else if (this.menu.SceneMode == Unit00499Scene.Mode.AwakeUnit)
    {
      ((UIButtonColor) this.menu.awakeBtn).isEnabled = this.isEvolutionButtonEnabled;
    }
    else
    {
      if (this.menu.SceneMode != Unit00499Scene.Mode.CommonAwakeUnit)
        return;
      ((UIButtonColor) this.menu.awakeBtn).isEnabled = this.isEvolutionButtonEnabled;
    }
  }
}
