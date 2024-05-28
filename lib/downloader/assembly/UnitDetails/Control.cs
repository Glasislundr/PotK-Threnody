// Decompiled with JetBrains decompiler
// Type: UnitDetails.Control
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnitDetails
{
  [Flags]
  public enum Control
  {
    Zero = 0,
    Limited = 1,
    Guest = 2,
    Memory = 4,
    Material = 8,
    ReincarnationType = 16, // 0x00000010
    OverkillersUnit = 32, // 0x00000020
    OverkillersBase = 64, // 0x00000040
    OverkillersSlot = 128, // 0x00000080
    OverkillersEdit = 256, // 0x00000100
    OverkillersMove = 512, // 0x00000200
    SelfAbility = 1024, // 0x00000400
    ToutaPlusNoEnable = 2048, // 0x00000800
    MyGvgAtkDeck = 4096, // 0x00001000
    MyGvgDefDeck = 8192, // 0x00002000
    MyGvgDeck = MyGvgDefDeck | MyGvgAtkDeck, // 0x00003000
    Pickup = 16384, // 0x00004000
    CustomDeck = 32768, // 0x00008000
    NotUpdate = 65536, // 0x00010000
  }
}
