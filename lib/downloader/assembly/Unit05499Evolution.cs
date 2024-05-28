// Decompiled with JetBrains decompiler
// Type: Unit05499Evolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit05499Evolution : Unit00499Evolution
{
  protected override IEnumerator CreateIndicator(
    int evolutionPatternId,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits)
  {
    Unit05499Evolution unit05499Evolution = this;
    Unit00499EvolutionIndicator component = unit05499Evolution.indicator.instantiateParts(unit05499Evolution.indicatorPrefab).GetComponent<Unit00499EvolutionIndicator>();
    ((Component) component.TxtZeny).gameObject.SetActive(false);
    ((Component) component.TxtZenyNeed).gameObject.SetActive(false);
    unit05499Evolution.linkEvolutionUnitsDict[evolutionPatternId] = component.linkEvolutionUnits;
    IEnumerator e = unit05499Evolution.menu.InitEvolutionUnits(playerUnits, playerMaterialUnits, unit05499Evolution.unitIconPrefab, component, evolutionPatternId, unit05499Evolution.evolutionMaterialDict[evolutionPatternId], true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
