// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartAwake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/Awake")]
public class PopupRecommendPartAwake : PopupRecommendPart
{
  [SerializeField]
  private Color colorEnable_ = Color.white;
  [SerializeField]
  private Color colorDisable_ = Color.gray;
  [SerializeField]
  private GameObject objImpossible_;
  [SerializeField]
  private GameObject objTarget_;
  [SerializeField]
  private GameObject objPossible_;
  [SerializeField]
  private GameObject objComplete_;
  private const int POSSIBLE_RARITY = 5;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    GameObject[] gameObjectArray = new GameObject[4]
    {
      this.objImpossible_,
      this.objTarget_,
      this.objPossible_,
      this.objComplete_
    };
    PopupRecommendPartAwake.Awake awake = PopupRecommendPartAwake.Awake.Impossible;
    UnitUnit unit = playerUnit.unit;
    if (unit.awake_unit_flag)
    {
      awake = PopupRecommendPartAwake.Awake.Complete;
    }
    else
    {
      bool flag = false;
      foreach (UnitUnit unitUnit in ((IEnumerable<UnitEvolutionPattern>) UnitEvolutionPattern.getGenealogy(unit.ID)).Select<UnitEvolutionPattern, UnitUnit>((Func<UnitEvolutionPattern, UnitUnit>) (x => x.unit)))
      {
        if (unitUnit.CanAwakeUnitFlag)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        awake = unit.rarity.index == 5 ? PopupRecommendPartAwake.Awake.Possible : PopupRecommendPartAwake.Awake.Target;
    }
    for (int index = 0; index < gameObjectArray.Length; ++index)
    {
      if (!Object.op_Equality((Object) gameObjectArray[index], (Object) null))
      {
        Color color = awake == (PopupRecommendPartAwake.Awake) index ? this.colorEnable_ : this.colorDisable_;
        foreach (UIWidget componentsInChild in gameObjectArray[index].GetComponentsInChildren<UIWidget>(true))
          componentsInChild.color = color;
      }
    }
    yield break;
  }

  private enum Awake
  {
    Impossible,
    Target,
    Possible,
    Complete,
  }
}
