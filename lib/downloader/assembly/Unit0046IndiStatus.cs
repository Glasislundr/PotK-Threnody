// Decompiled with JetBrains decompiler
// Type: Unit0046IndiStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Unit0046IndiStatus : MonoBehaviour
{
  public UILabel HP;
  public UILabel PAt;
  public UILabel MAt;
  public UILabel PDe;
  public UILabel MDe;
  private Color GrayColor = new Color(0.6f, 0.6f, 0.6f);

  public void SetText(PlayerUnit pUnit)
  {
    Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(pUnit);
    this.HP.SetTextLocalize(nonBattleParameter.Hp.ToString());
    this.PAt.SetTextLocalize(nonBattleParameter.PhysicalAttack.ToString());
    this.MAt.SetTextLocalize(nonBattleParameter.MagicAttack.ToString());
    UnitTypeEnum[] validUnitTypes = pUnit.unit.validUnitTypes;
    if (!((IEnumerable<UnitTypeEnum>) validUnitTypes).Contains<UnitTypeEnum>(UnitTypeEnum.maki))
      ((UIWidget) this.MAt).color = this.GrayColor;
    else if (!((IEnumerable<UnitTypeEnum>) validUnitTypes).Contains<UnitTypeEnum>(UnitTypeEnum.kouki))
      ((UIWidget) this.PAt).color = this.GrayColor;
    this.PDe.SetTextLocalize(nonBattleParameter.PhysicalDefense.ToString());
    this.MDe.SetTextLocalize(nonBattleParameter.MagicDefense.ToString());
  }

  public void RemoveText()
  {
    ((IEnumerable<UILabel>) ((Component) this).GetComponentsInChildren<UILabel>()).ForEach<UILabel>((Action<UILabel>) (x => x.SetText("-")));
  }
}
