// Decompiled with JetBrains decompiler
// Type: SM.GuildLevelBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildLevelBonus : KeyCompare
  {
    public int item;
    public int player_exp;
    public int unit_exp;
    public bool campaign_flag;
    public int money;

    public GuildLevelBonus()
    {
    }

    public GuildLevelBonus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.item = (int) (long) json[nameof (item)];
      this.player_exp = (int) (long) json[nameof (player_exp)];
      this.unit_exp = (int) (long) json[nameof (unit_exp)];
      this.campaign_flag = (bool) json[nameof (campaign_flag)];
      this.money = (int) (long) json[nameof (money)];
    }
  }
}
