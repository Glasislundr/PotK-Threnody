// Decompiled with JetBrains decompiler
// Type: SM.GainTrustResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GainTrustResult : KeyCompare
  {
    public bool is_equip_awake_skill_release;
    public bool has_new_player_awake_skill;

    public GainTrustResult()
    {
    }

    public GainTrustResult(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_equip_awake_skill_release = (bool) json[nameof (is_equip_awake_skill_release)];
      this.has_new_player_awake_skill = (bool) json[nameof (has_new_player_awake_skill)];
    }
  }
}
