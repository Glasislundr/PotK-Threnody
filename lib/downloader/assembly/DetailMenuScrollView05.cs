// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollView05
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using UnityEngine;

#nullable disable
public class DetailMenuScrollView05 : DetailMenuScrollViewBase
{
  [SerializeField]
  protected UILabel TxtIntroduction;

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    this.Set(playerUnit.unit);
    return true;
  }

  public void Set(UnitUnit unit) => this.TxtIntroduction.SetTextLocalize(unit.description);
}
