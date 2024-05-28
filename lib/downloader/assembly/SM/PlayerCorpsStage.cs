// Decompiled with JetBrains decompiler
// Type: SM.PlayerCorpsStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCorpsStage : KeyCompare
  {
    public int stage_id;
    public bool is_first;
    public CorpsEnemyStatus[] enemies;

    public PlayerCorpsStage()
    {
    }

    public PlayerCorpsStage(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.stage_id = (int) (long) json[nameof (stage_id)];
      this.is_first = (bool) json[nameof (is_first)];
      List<CorpsEnemyStatus> corpsEnemyStatusList = new List<CorpsEnemyStatus>();
      foreach (object json1 in (List<object>) json[nameof (enemies)])
        corpsEnemyStatusList.Add(json1 == null ? (CorpsEnemyStatus) null : new CorpsEnemyStatus((Dictionary<string, object>) json1));
      this.enemies = corpsEnemyStatusList.ToArray();
    }
  }
}
