// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageEnemyReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStageEnemyReward
  {
    public int ID;
    public int exp;
    public int money;
    public int drop_id;

    public static BattleStageEnemyReward Parse(MasterDataReader reader)
    {
      return new BattleStageEnemyReward()
      {
        ID = reader.ReadInt(),
        exp = reader.ReadInt(),
        money = reader.ReadInt(),
        drop_id = reader.ReadInt()
      };
    }
  }
}
