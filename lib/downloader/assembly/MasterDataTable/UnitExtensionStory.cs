// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitExtensionStory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitExtensionStory
  {
    public int ID;
    public int unit;
    public int job_metamor_id;
    public int face_x;
    public int face_y;
    public float story_texture_scale;
    public int story_texture_x;
    public int story_texture_y;

    public static UnitExtensionStory Parse(MasterDataReader reader)
    {
      return new UnitExtensionStory()
      {
        ID = reader.ReadInt(),
        unit = reader.ReadInt(),
        job_metamor_id = reader.ReadInt(),
        face_x = reader.ReadInt(),
        face_y = reader.ReadInt(),
        story_texture_scale = reader.ReadFloat(),
        story_texture_x = reader.ReadInt(),
        story_texture_y = reader.ReadInt()
      };
    }
  }
}
