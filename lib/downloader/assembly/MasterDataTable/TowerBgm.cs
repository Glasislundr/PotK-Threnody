// Decompiled with JetBrains decompiler
// Type: MasterDataTable.TowerBgm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class TowerBgm
  {
    public int ID;
    public int period_id;
    public string bgm_name;
    public string bgm_file;
    public int floor;

    public static TowerBgm Parse(MasterDataReader reader)
    {
      return new TowerBgm()
      {
        ID = reader.ReadInt(),
        period_id = reader.ReadInt(),
        bgm_name = reader.ReadStringOrNull(true),
        bgm_file = reader.ReadStringOrNull(true),
        floor = reader.ReadInt()
      };
    }
  }
}
