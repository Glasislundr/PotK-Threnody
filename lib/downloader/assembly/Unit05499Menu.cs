// Decompiled with JetBrains decompiler
// Type: Unit05499Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit05499Menu : Unit00499Menu
{
  [SerializeField]
  private Unit00499UnitStatus afterUnit;

  public IEnumerator Initialize(PlayerUnit beforeUnit, PlayerUnit[] targetUnits)
  {
    IEnumerator e = this.Init(beforeUnit, targetUnits, Unit00499Scene.Mode.EarthEvolution, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void SetStatusText()
  {
    this.beforeUnit.SetStatusTextEarth(this.playerUnit);
    this.afterUnit.SetStatusTextEarth(this.targetPlayerUnit);
  }

  protected override IEnumerator Evolution()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit05499Menu unit05499Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Tuple<PlayerUnit, PlayerUnit> tuple = Singleton<EarthDataManager>.GetInstance().EvolutionUnit(unit05499Menu.baseUnit.id, unit05499Menu.selectEvolutionPatternId, unit05499Menu.materialUnitIds);
    Consts instance = Consts.GetInstance();
    if (tuple == null)
    {
      unit05499Menu.StartCoroutine(PopupCommon.Show(instance.UNIT_054_9_9_ERROR_TITLE, instance.UNIT_054_9_9_ERROR_BODY));
      return false;
    }
    Singleton<EarthDataManager>.GetInstance().Save();
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (GameObject linkEvolutionUnit in unit05499Menu.linkEvolutionUnits)
    {
      UnitIcon componentInChildren = ((Component) linkEvolutionUnit.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
      if (componentInChildren.PlayerUnit != (PlayerUnit) null)
        playerUnitList.Add(componentInChildren.PlayerUnit);
    }
    unit00497Scene.ChangeScene(false, new PrincesEvolutionParam()
    {
      materiaqlUnits = playerUnitList,
      is_new = false,
      baseUnit = tuple.Item1,
      resultUnit = tuple.Item2,
      fromEarth = true,
      mode = Unit00499Scene.Mode.EarthEvolution
    });
    Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_9_9");
    return false;
  }

  public override bool CheckEnabledButton(int money)
  {
    this.comShortage.SetActive(false);
    bool flag = false;
    if (!this.isUnit)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(1);
    }
    if (!this.isLevel)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(2);
    }
    if (!this.isFavorite)
    {
      flag = true;
      ((IEnumerable<GameObject>) this.comShortages).ToggleOnce(3);
    }
    this.comShortage.SetActive(flag);
    return this.isUnit && this.isLevel && this.isFavorite;
  }

  public override void IbtnBack() => base.IbtnBack();
}
