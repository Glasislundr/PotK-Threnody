// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraLL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraLL
  {
    public int ID;
    public string name;
    public int? description;
    public string background_image_name;
    public bool enabled_header;
    public int? banner_image_id;

    public static QuestExtraLL Parse(MasterDataReader reader)
    {
      return new QuestExtraLL()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadIntOrNull(),
        background_image_name = reader.ReadString(true),
        enabled_header = reader.ReadBool(),
        banner_image_id = reader.ReadIntOrNull()
      };
    }
  }
}
