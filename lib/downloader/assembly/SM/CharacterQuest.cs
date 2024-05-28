// Decompiled with JetBrains decompiler
// Type: SM.CharacterQuest
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
  public class CharacterQuest : KeyCompare
  {
    public bool is_disable;
    public int _unit_id;
    public bool is_playable;

    public UnitUnit unit_id
    {
      get
      {
        if (MasterData.UnitUnit.ContainsKey(this._unit_id))
          return MasterData.UnitUnit[this._unit_id];
        Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this._unit_id + "]"));
        return (UnitUnit) null;
      }
    }

    public CharacterQuest()
    {
    }

    public CharacterQuest(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_disable = (bool) json[nameof (is_disable)];
      this._unit_id = (int) (long) json[nameof (unit_id)];
      this.is_playable = (bool) json[nameof (is_playable)];
    }
  }
}
