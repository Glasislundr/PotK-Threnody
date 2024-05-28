﻿// Decompiled with JetBrains decompiler
// Type: SM.PlayerTalkMessageCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerTalkMessageCondition : KeyCompare
  {
    public int? condition_type_id;
    public int? condition_id;

    public PlayerTalkMessageCondition()
    {
    }

    public PlayerTalkMessageCondition(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (condition_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (condition_type_id)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.condition_type_id = nullable2;
      int? nullable3;
      if (json[nameof (condition_id)] != null)
      {
        nullable1 = (long?) json[nameof (condition_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.condition_id = nullable3;
    }
  }
}
