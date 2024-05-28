// Decompiled with JetBrains decompiler
// Type: SM.TowerQuestEntryCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerQuestEntryCondition : KeyCompare
  {
    public int id;
    public int tower_id;

    public TowerQuestEntryCondition()
    {
    }

    public TowerQuestEntryCondition(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.id = (int) (long) json[nameof (id)];
      this.tower_id = (int) (long) json[nameof (tower_id)];
    }
  }
}
