// Decompiled with JetBrains decompiler
// Type: Battle01PVPEnemyUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01PVPEnemyUnits : NGBattleMenuBase
{
  [SerializeField]
  private GameObject[] nodes;
  private GameObject popupInfoPrefab;

  protected override IEnumerator Start_Battle()
  {
    yield break;
  }

  public IEnumerator setupUnits(List<BL.Unit> units, GameObject prefab)
  {
    Future<GameObject> popupInfoPrefabF = new ResourceObject("Prefabs/battleUI_03/Battle030627_UI_PlayerStatus_2").Load<GameObject>();
    IEnumerator e = popupInfoPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupInfoPrefab = popupInfoPrefabF.Result;
    int i = 0;
    foreach (BL.Unit unit in units)
    {
      if (this.nodes.Length > i)
      {
        e = this.cloneUnitThum(unit, prefab, this.nodes[i++].transform);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        break;
    }
  }

  private IEnumerator cloneUnitThum(BL.Unit unit, GameObject prefab, Transform parent)
  {
    UnitIcon up = prefab.Clone(parent).GetComponent<UnitIcon>();
    IEnumerator e = up.SetUnit(unit.unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    up.setLevelText(unit.lv.ToString());
    up.BottomModeValue = UnitIconBase.GetBottomModeLevel(unit.unit, (PlayerUnit) null);
    up.ShowBottomInfosLevelOnly();
    up.onClick = (Action<UnitIconBase>) (x => this.OpenPopupInfo(unit));
  }

  public void OpenPopupInfo(BL.Unit unit)
  {
    Singleton<PopupManager>.GetInstance().open(this.popupInfoPrefab).GetComponent<BattleUI01UnitInformation>().InitFromPVP(unit, false);
  }
}
