// Decompiled with JetBrains decompiler
// Type: Guide01142indicator1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class Guide01142indicator1 : Unit00443indicator
{
  [SerializeField]
  private GameObject NumPossession;
  [SerializeField]
  private UILabel txtUnit;

  public override void Init(GearGear gear)
  {
    this.SetParam(gear);
    this.SetSkillDeteilEvent(gear);
    this.SetWeaponAttack(gear);
    this.SetWeaponElement(gear);
    this.Weapon.Init(gear.kind_GearKind);
  }

  public override void SetUnit(int num)
  {
    if (!Object.op_Inequality((Object) this.NumPossession, (Object) null))
      return;
    this.txtUnit.SetTextLocalize(num);
    this.NumPossession.SetActive(true);
  }
}
