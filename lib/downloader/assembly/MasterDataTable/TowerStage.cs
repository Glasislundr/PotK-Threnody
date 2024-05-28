// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TowerStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TowerStage
  {
    public int ID;
    public int tower_id;
    public int floor;
    public int stage_id;
    public int back_ground_id_TowerCommonBackground;

    public static TowerStage Parse(MasterDataReader reader)
    {
      return new TowerStage()
      {
        ID = reader.ReadInt(),
        tower_id = reader.ReadInt(),
        floor = reader.ReadInt(),
        stage_id = reader.ReadInt(),
        back_ground_id_TowerCommonBackground = reader.ReadInt()
      };
    }

    public TowerCommonBackground back_ground_id
    {
      get
      {
        TowerCommonBackground backGroundId;
        if (!MasterData.TowerCommonBackground.TryGetValue(this.back_ground_id_TowerCommonBackground, out backGroundId))
          Debug.LogError((object) ("Key not Found: MasterData.TowerCommonBackground[" + (object) this.back_ground_id_TowerCommonBackground + "]"));
        return backGroundId;
      }
    }

    public string GetBackgroundPath()
    {
      return !string.IsNullOrEmpty(this.back_ground_id.background_name) ? string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) this.back_ground_id.background_name) : Consts.GetInstance().DEFULAT_BACKGROUND;
    }
  }
}
