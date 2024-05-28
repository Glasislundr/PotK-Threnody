// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ColosseumTotalVictoryReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ColosseumTotalVictoryReward
  {
    public int ID;
    public int victory_value;
    public string reward_type;
    public int reward_id;
    public int reward_value;
    public string reward_title;
    public string reward_description;

    public static ColosseumTotalVictoryReward Parse(MasterDataReader reader)
    {
      return new ColosseumTotalVictoryReward()
      {
        ID = reader.ReadInt(),
        victory_value = reader.ReadInt(),
        reward_type = reader.ReadString(true),
        reward_id = reader.ReadInt(),
        reward_value = reader.ReadInt(),
        reward_title = reader.ReadString(true),
        reward_description = reader.ReadString(true)
      };
    }
  }
}
