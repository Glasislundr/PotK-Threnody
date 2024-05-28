// Decompiled with JetBrains decompiler
// Type: SM.PlayerItemHistory
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
  public class PlayerItemHistory : KeyCompare
  {
    public int _entity_type;
    public int entity_id;

    public MasterDataTable.CommonRewardType entity_type => (MasterDataTable.CommonRewardType) this._entity_type;

    public GearGear gear
    {
      get
      {
        return this.entity_type == MasterDataTable.CommonRewardType.gear ? MasterData.GearGear[this.entity_id] : (GearGear) null;
      }
    }

    public SupplySupply item
    {
      get
      {
        return this.entity_type == MasterDataTable.CommonRewardType.supply ? MasterData.SupplySupply[this.entity_id] : (SupplySupply) null;
      }
    }

    public PlayerItemHistory()
    {
    }

    public PlayerItemHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._entity_type = (int) (long) json[nameof (_entity_type)];
      this.entity_id = (int) (long) json[nameof (entity_id)];
    }
  }
}
