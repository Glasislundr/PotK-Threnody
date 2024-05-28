// Decompiled with JetBrains decompiler
// Type: SM.GuildBaseBonusEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildBaseBonusEffect : KeyCompare
  {
    public int _skill;
    public int id;

    public BattleskillSkill skill
    {
      get
      {
        if (MasterData.BattleskillSkill.ContainsKey(this._skill))
          return MasterData.BattleskillSkill[this._skill];
        Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this._skill + "]"));
        return (BattleskillSkill) null;
      }
    }

    public GuildBaseBonusEffect()
    {
    }

    public GuildBaseBonusEffect(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._skill = (int) (long) json[nameof (skill)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
