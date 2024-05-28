// Decompiled with JetBrains decompiler
// Type: SM.EventBattleFinishDestroy_enemys
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class EventBattleFinishDestroy_enemys : KeyCompare
  {
    public int bonus_point;
    public int point;
    public int unit_id;
    public int destroy_count;

    public EventBattleFinishDestroy_enemys()
    {
    }

    public EventBattleFinishDestroy_enemys(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.bonus_point = (int) (long) json[nameof (bonus_point)];
      this.point = (int) (long) json[nameof (point)];
      this.unit_id = (int) (long) json[nameof (unit_id)];
      this.destroy_count = (int) (long) json[nameof (destroy_count)];
    }
  }
}
