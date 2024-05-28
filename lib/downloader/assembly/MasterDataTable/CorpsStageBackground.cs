// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsStageBackground
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsStageBackground
  {
    public int ID;
    public string background_name;
    public float offset_x;
    public float offset_y;
    public float scale;

    public static CorpsStageBackground Parse(MasterDataReader reader)
    {
      return new CorpsStageBackground()
      {
        ID = reader.ReadInt(),
        background_name = reader.ReadStringOrNull(true),
        offset_x = reader.ReadFloat(),
        offset_y = reader.ReadFloat(),
        scale = reader.ReadFloat()
      };
    }
  }
}
