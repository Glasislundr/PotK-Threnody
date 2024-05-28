// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsSetting
  {
    public int ID;
    public string battlefield_name;
    public int max_unit_count;
    public int min_unit_level;
    public int max_supply_count;
    public string background_file;
    public string bgm_name;
    public string bgm_file;
    public int rule_no;
    public string rule_detail;

    public static CorpsSetting Parse(MasterDataReader reader)
    {
      return new CorpsSetting()
      {
        ID = reader.ReadInt(),
        battlefield_name = reader.ReadStringOrNull(true),
        max_unit_count = reader.ReadInt(),
        min_unit_level = reader.ReadInt(),
        max_supply_count = reader.ReadInt(),
        background_file = reader.ReadStringOrNull(true),
        bgm_name = reader.ReadStringOrNull(true),
        bgm_file = reader.ReadStringOrNull(true),
        rule_no = reader.ReadInt(),
        rule_detail = reader.ReadStringOrNull(true)
      };
    }
  }
}
