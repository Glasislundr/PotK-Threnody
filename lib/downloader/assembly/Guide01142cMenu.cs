// Decompiled with JetBrains decompiler
// Type: Guide01142cMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using UnityEngine;

#nullable disable
public class Guide01142cMenu : Bugu00561Menu
{
  [SerializeField]
  protected UILabel TxtNumber;
  [SerializeField]
  protected GameObject dirNumber;

  public void SetNumber(ItemInfo item, bool isDispNumber)
  {
    this.dirNumber.SetActive(isDispNumber);
    this.TxtNumber.SetTextLocalize("NO." + (item.gear.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public void SetNumber(GearGear gear, bool isDispNumber)
  {
    this.dirNumber.SetActive(isDispNumber);
    this.TxtNumber.SetTextLocalize("NO." + (gear.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
