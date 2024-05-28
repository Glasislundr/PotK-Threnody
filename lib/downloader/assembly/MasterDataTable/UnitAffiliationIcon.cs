// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitAffiliationIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitAffiliationIcon
  {
    public int ID;
    public string english_name;
    public int unit_group_head_UnitGroupHead;
    public int group_refer_id;
    public int priority;
    public string file_name;

    public static UnitAffiliationIcon Parse(MasterDataReader reader)
    {
      return new UnitAffiliationIcon()
      {
        ID = reader.ReadInt(),
        english_name = reader.ReadString(true),
        unit_group_head_UnitGroupHead = reader.ReadInt(),
        group_refer_id = reader.ReadInt(),
        priority = reader.ReadInt(),
        file_name = reader.ReadString(true)
      };
    }

    public UnitGroupHead unit_group_head => (UnitGroupHead) this.unit_group_head_UnitGroupHead;
  }
}
