// Decompiled with JetBrains decompiler
// Type: CustomDeck.EditGearParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;

#nullable disable
namespace CustomDeck
{
  public class EditGearParam : EditBaseParam
  {
    public int slotNo { get; private set; }

    public PlayerItem[] gears { get; private set; }

    public int[] reisouIds { get; private set; }

    public Action<int, int, int> onSetGear { get; private set; }

    public EditGearParam(
      PlayerCustomDeck d,
      PlayerUnit pu,
      Dictionary<int, PlayerUnit> dic,
      int paramIndex,
      int slotNumber,
      PlayerItem[] aGears,
      int[] reisou_ids,
      Action<int, int, int> eventSetGear)
    {
      this.deck = d;
      this.baseUnit = pu;
      this.dicReference = dic;
      this.index = paramIndex;
      this.slotNo = slotNumber;
      this.gears = aGears;
      this.reisouIds = reisou_ids;
      this.onSetGear = eventSetGear;
    }
  }
}
