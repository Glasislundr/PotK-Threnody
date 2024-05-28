// Decompiled with JetBrains decompiler
// Type: Battle02StatusScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle02StatusScrollParts : BattleMonoBehaviour
{
  [SerializeField]
  private SelectParts mColumnHeader;
  [SerializeField]
  private NGxScroll scrollParts;
  [SerializeField]
  public GameObject[] chilObject;
  private int forcsId = -1;

  public void initParts(GameObject prefab, int forcsId)
  {
    if (this.forcsId == forcsId)
      return;
    this.forcsId = forcsId;
    ((IEnumerable<GameObject>) this.chilObject).ForEach<GameObject>((Action<GameObject>) (o => o.SetActive(true)));
    List<BL.Unit> unitList;
    switch (forcsId)
    {
      case 0:
        unitList = this.env.core.playerUnits.value;
        break;
      case 1:
        unitList = this.env.core.enemyUnits.value;
        break;
      case 2:
        unitList = this.env.core.neutralUnits.value;
        break;
      default:
        return;
    }
    this.scrollParts.Clear();
    foreach (BL.Unit unit in unitList)
    {
      if (unit.playerUnit.spawn_turn <= this.env.core.phaseState.turnCount)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(prefab);
        Battle02MenuBase component = gameObject.GetComponent<Battle02MenuBase>();
        component.setUnit(unit);
        component.UpdateData();
        this.scrollParts.Add(gameObject);
      }
    }
    ((Component) this).transform.localPosition = new Vector3(((Component) this).transform.localPosition.x + 100000f, ((Component) this).transform.localPosition.y, ((Component) this).transform.localPosition.z);
    this.chilObject[0].transform.localPosition = new Vector3(this.chilObject[0].transform.localPosition.x - 100000f, this.chilObject[0].transform.localPosition.y, this.chilObject[0].transform.localPosition.z);
    ((Component) this).gameObject.SetActive(true);
    ((Component) ((Component) this).transform.parent).gameObject.SetActive(true);
    this.scrollParts.ResolvePosition();
    this.mColumnHeader.setValueNonTween(forcsId);
  }
}
