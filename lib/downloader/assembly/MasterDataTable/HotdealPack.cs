// Decompiled with JetBrains decompiler
// Type: MasterDataTable.HotdealPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class HotdealPack
  {
    public int ID;
    public string modal_resource_name;
    public string icon_resource_name;
    public string icon_top_text_resource_name;
    public string icon_bottom_text_resource_name;
    public string icon_shadow_text_resource_name;

    public static HotdealPack Parse(MasterDataReader reader)
    {
      return new HotdealPack()
      {
        ID = reader.ReadInt(),
        modal_resource_name = reader.ReadString(true),
        icon_resource_name = reader.ReadString(true),
        icon_top_text_resource_name = reader.ReadString(true),
        icon_bottom_text_resource_name = reader.ReadString(true),
        icon_shadow_text_resource_name = reader.ReadString(true)
      };
    }
  }
}
