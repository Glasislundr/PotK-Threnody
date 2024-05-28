// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TowerBattleStageClear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TowerBattleStageClear
  {
    public int ID;
    public int reward_group_id;
    public int stage_id;
    public int lap;
    public int entity_type_CommonRewardType;
    public int reward_id;
    public int is_direct;
    public string reward_name;
    public string reward_message;

    public static TowerBattleStageClear Parse(MasterDataReader reader)
    {
      return new TowerBattleStageClear()
      {
        ID = reader.ReadInt(),
        reward_group_id = reader.ReadInt(),
        stage_id = reader.ReadInt(),
        lap = reader.ReadInt(),
        entity_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        is_direct = reader.ReadInt(),
        reward_name = reader.ReadStringOrNull(true),
        reward_message = reader.ReadStringOrNull(true)
      };
    }

    public CommonRewardType entity_type => (CommonRewardType) this.entity_type_CommonRewardType;
  }
}
