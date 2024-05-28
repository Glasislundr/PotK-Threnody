// Decompiled with JetBrains decompiler
// Type: EquipmentRules.Reisou
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
namespace EquipmentRules
{
  public static class Reisou
  {
    public static PlayerItem checkPossibleEquipped(PlayerItem gear, PlayerItem reisou)
    {
      int? kindGearKind1 = gear?.gear.kind_GearKind;
      int? kindGearKind2 = reisou?.gear.kind_GearKind;
      return !(kindGearKind1.GetValueOrDefault() == kindGearKind2.GetValueOrDefault() & kindGearKind1.HasValue == kindGearKind2.HasValue) ? (PlayerItem) null : reisou;
    }
  }
}
