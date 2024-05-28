// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleCameraFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleCameraFilter
  {
    public int ID;
    public int stage_id_BattleStage;
    public string filter_resource_name;

    public static BattleCameraFilter Parse(MasterDataReader reader)
    {
      return new BattleCameraFilter()
      {
        ID = reader.ReadInt(),
        stage_id_BattleStage = reader.ReadInt(),
        filter_resource_name = reader.ReadString(true)
      };
    }

    public BattleStage stage_id
    {
      get
      {
        BattleStage stageId;
        if (!MasterData.BattleStage.TryGetValue(this.stage_id_BattleStage, out stageId))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.stage_id_BattleStage + "]"));
        return stageId;
      }
    }
  }
}
