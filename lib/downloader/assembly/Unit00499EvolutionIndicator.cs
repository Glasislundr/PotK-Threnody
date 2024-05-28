// Decompiled with JetBrains decompiler
// Type: Unit00499EvolutionIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Unit00499EvolutionIndicator : MonoBehaviour
{
  public UILabel[] linkEvolutionUnitsPossessionLabel;
  public GameObject[] linkEvolutionUnits;
  public Unit00499EvolutionIndicator.ButtonIcon[] linkSelectableUnits;
  public UILabel TxtZeny;
  public UILabel TxtZenyNeed;

  [Serializable]
  public class ButtonIcon
  {
    public GameObject top_;
    public LongPressButton button_;
  }
}
