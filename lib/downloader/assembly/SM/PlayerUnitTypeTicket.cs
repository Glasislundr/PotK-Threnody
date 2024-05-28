// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitTypeTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitTypeTicket : KeyCompare
  {
    public int ticket_id;
    public int quantity;

    public PlayerUnitTypeTicket()
    {
    }

    public PlayerUnitTypeTicket(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.ticket_id = (int) (long) json[nameof (ticket_id)];
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
