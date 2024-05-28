// Decompiled with JetBrains decompiler
// Type: CustomDeck.EditReisouParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;

#nullable disable
namespace CustomDeck
{
  public class EditReisouParam : EditBaseParam
  {
    public PlayerItem baseGear { get; private set; }

    public int slotNo { get; private set; }

    public PlayerItem[] reisous { get; private set; }

    public Dictionary<int, Tuple<PlayerUnit, PlayerItem>> dicReference { get; private set; }

    public Action<int, int, int> onSetReisou { get; private set; }

    public EditReisouParam(
      PlayerCustomDeck d,
      PlayerItem g,
      Dictionary<int, Tuple<PlayerUnit, PlayerItem>> dic,
      int paramIndex,
      int slotNumber,
      PlayerItem[] aReisous,
      Action<int, int, int> eventSetReisou)
    {
      this.deck = d;
      this.baseGear = g;
      this.dicReference = dic;
      this.index = paramIndex;
      this.slotNo = slotNumber;
      this.reisous = aReisous;
      this.onSetReisou = eventSetReisou;
    }
  }
}
