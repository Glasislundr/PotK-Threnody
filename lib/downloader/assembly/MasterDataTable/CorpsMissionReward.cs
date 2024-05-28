// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsMissionReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsMissionReward
  {
    public int ID;
    public int setting_id;
    public int priority;
    public string name;
    public int entity_type_CommonRewardType;
    public int entity_id;
    public int quantity;
    public string reward_name;

    public static CorpsMissionReward Parse(MasterDataReader reader)
    {
      return new CorpsMissionReward()
      {
        ID = reader.ReadInt(),
        setting_id = reader.ReadInt(),
        priority = reader.ReadInt(),
        name = reader.ReadString(true),
        entity_type_CommonRewardType = reader.ReadInt(),
        entity_id = reader.ReadInt(),
        quantity = reader.ReadInt(),
        reward_name = reader.ReadStringOrNull(true)
      };
    }

    public CommonRewardType entity_type => (CommonRewardType) this.entity_type_CommonRewardType;
  }
}
