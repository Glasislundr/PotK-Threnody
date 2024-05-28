// Decompiled with JetBrains decompiler
// Type: SM.PlayerMaterialUnit
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
  public class PlayerMaterialUnit : KeyCompare
  {
    public string player_id;
    public int id;
    public int _unit;
    public int quantity;

    public static PlayerMaterialUnit CreateByUnitId(int id, int quantity)
    {
      PlayerMaterialUnit byUnitId = new PlayerMaterialUnit();
      byUnitId._hasKey = true;
      int num1;
      int num2 = num1 = 1;
      byUnitId.id = num1;
      byUnitId._key = (object) num2;
      byUnitId._unit = id;
      byUnitId.player_id = "1";
      byUnitId.quantity = quantity;
      return byUnitId;
    }

    public UnitUnit unit
    {
      get
      {
        if (MasterData.UnitUnit.ContainsKey(this._unit))
          return MasterData.UnitUnit[this._unit];
        Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this._unit + "]"));
        return (UnitUnit) null;
      }
    }

    public PlayerMaterialUnit()
    {
    }

    public PlayerMaterialUnit(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.player_id = (string) json[nameof (player_id)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this._unit = (int) (long) json[nameof (unit)];
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
