// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SlotS001MedalReelIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SlotS001MedalReelIcon
  {
    public int ID;
    public string name;
    public string description;
    public int medal_flg;
    public string file_name;

    public static SlotS001MedalReelIcon Parse(MasterDataReader reader)
    {
      return new SlotS001MedalReelIcon()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        medal_flg = reader.ReadInt(),
        file_name = reader.ReadString(true)
      };
    }
  }
}
