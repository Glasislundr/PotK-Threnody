// Decompiled with JetBrains decompiler
// Type: UnitWanted
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
public class UnitWanted : MonoBehaviour
{
  [SerializeField]
  private GameObject slcUnitWantedBaseNon;
  [SerializeField]
  private GameObject slcUnitWantedBase;
  [SerializeField]
  private UILabel txtTargetMaxPoint;
  [SerializeField]
  private CreateIconObject dynUnitThum;
  private EnemyTopInfo[] infos;
  private Action<EnemyTopInfo[]> tapAction;

  public IEnumerator Init(EnemyTopInfo[] infos, Action<EnemyTopInfo[]> action)
  {
    if (infos == null)
    {
      this.slcUnitWantedBaseNon.SetActive(true);
      this.slcUnitWantedBase.SetActive(false);
    }
    else
    {
      this.tapAction = action;
      this.infos = infos;
      UnitUnit unitUnit = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == infos[0].unit_id)).FirstOrDefault<UnitUnit>();
      int num1 = ((IEnumerable<EnemyTopInfo>) infos).Min<EnemyTopInfo>((Func<EnemyTopInfo, int>) (x => x.min_point));
      int num2 = ((IEnumerable<EnemyTopInfo>) infos).Max<EnemyTopInfo>((Func<EnemyTopInfo, int>) (x => x.min_point));
      if (unitUnit == null)
      {
        this.slcUnitWantedBaseNon.SetActive(true);
        this.slcUnitWantedBase.SetActive(false);
      }
      else
      {
        this.slcUnitWantedBaseNon.SetActive(false);
        this.slcUnitWantedBase.SetActive(true);
        if (num1 == num2)
          this.txtTargetMaxPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00230_TARGET_POINT, (IDictionary) new Hashtable()
          {
            {
              (object) "point",
              (object) num1
            }
          }));
        else
          this.txtTargetMaxPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00230_TARGET_POINT_MULTI, (IDictionary) new Hashtable()
          {
            {
              (object) "point",
              (object) num1
            }
          }));
        IEnumerator e = this.dynUnitThum.CreateThumbnail(MasterDataTable.CommonRewardType.unit, unitUnit.ID, visibleBottom: false, isButton: false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  public void DispDetail()
  {
    if (this.tapAction == null)
      return;
    this.tapAction(this.infos);
  }
}
