// Decompiled with JetBrains decompiler
// Type: CustomDeck.EditOverkillersParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;

#nullable disable
namespace CustomDeck
{
  public class EditOverkillersParam : EditBaseParam
  {
    public int slotNo { get; private set; }

    public PlayerUnit selected { get; private set; }

    public PlayerUnit[] units { get; private set; }

    public Action<int, int, int> onSetOverkillers { get; private set; }

    public EditOverkillersParam(
      PlayerCustomDeck d,
      PlayerUnit pu,
      Dictionary<int, PlayerUnit> dic,
      int paramIndex,
      int slotNumber,
      PlayerUnit selectedUnit,
      PlayerUnit[] aUnits,
      Action<int, int, int> eventSetOverkillers)
    {
      this.deck = d;
      this.baseUnit = pu;
      this.dicReference = dic;
      this.index = paramIndex;
      this.slotNo = slotNumber;
      this.selected = selectedUnit;
      this.units = aUnits;
      this.onSetOverkillers = eventSetOverkillers;
    }
  }
}
