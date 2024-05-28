// Decompiled with JetBrains decompiler
// Type: SM.BattleEndGain_trust_info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEndGain_trust_info : KeyCompare
  {
    public int player_unit_id;
    public GainTrustResult gain_trust_result;

    public BattleEndGain_trust_info()
    {
    }

    public BattleEndGain_trust_info(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_unit_id = (int) (long) json[nameof (player_unit_id)];
      this.gain_trust_result = json[nameof (gain_trust_result)] == null ? (GainTrustResult) null : new GainTrustResult((Dictionary<string, object>) json[nameof (gain_trust_result)]);
    }
  }
}
