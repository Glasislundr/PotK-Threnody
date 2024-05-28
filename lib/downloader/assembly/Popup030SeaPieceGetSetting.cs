// Decompiled with JetBrains decompiler
// Type: Popup030SeaPieceGetSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Popup030SeaPieceGetSetting : MonoBehaviour
{
  [SerializeField]
  private UIGrid grid;
  private GameObject unitIconPrefab;
  private Action callback;

  public IEnumerator Init(List<PieceGetResult> getPiece, Action callback)
  {
    IEnumerator e = this.LoadResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.callback = callback;
    List<UnitUnit> unitUnitList = new List<UnitUnit>();
    foreach (PieceGetResult pieceGetResult in getPiece)
    {
      PieceGetResult piece = pieceGetResult;
      UnitUnit unitUnit = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).FirstOrDefault<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == piece.same_character_id));
      unitUnitList.Add(unitUnit);
    }
    if (unitUnitList != null)
    {
      foreach (UnitUnit unit in unitUnitList)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(this.unitIconPrefab);
        gameObject.gameObject.SetParent(((Component) ((Component) this.grid).transform).gameObject);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
        e = unitIcon.SetUnit(unit, unit.GetElement(), false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unitIcon.BottomModeValue = UnitIconBase.BottomMode.AwakeUnit;
        unitIcon.SetSeaPiece(true);
        unitIcon = (UnitIcon) null;
      }
      this.grid.Reposition();
    }
  }

  private IEnumerator LoadResource()
  {
    Future<GameObject> unitIconPrefabF = Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = unitIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconPrefab = unitIconPrefabF.Result;
  }

  public void OnButtonOK() => this.callback();
}
