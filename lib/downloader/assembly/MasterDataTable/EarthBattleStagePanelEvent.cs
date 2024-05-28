// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthBattleStagePanelEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthBattleStagePanelEvent
  {
    public int ID;
    public int stage_id;
    public int set_group;
    public int group_appearance;
    public int position_x;
    public int position_y;
    public int drop_table_id;

    public static EarthBattleStagePanelEvent Parse(MasterDataReader reader)
    {
      return new EarthBattleStagePanelEvent()
      {
        ID = reader.ReadInt(),
        stage_id = reader.ReadInt(),
        set_group = reader.ReadInt(),
        group_appearance = reader.ReadInt(),
        position_x = reader.ReadInt(),
        position_y = reader.ReadInt(),
        drop_table_id = reader.ReadInt()
      };
    }
  }
}
