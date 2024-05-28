// Decompiled with JetBrains decompiler
// Type: Guide01122bMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class Guide01122bMenu : Unit00493Menu
{
  public void SetNumber(UnitUnit unit)
  {
    this.TxtOwnednumber.SetTextLocalize("NO." + (unit.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public override void IbtnBack() => this.backScene();
}
