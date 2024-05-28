// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitCameraPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitCameraPattern
  {
    public int ID;
    public float camera_x;
    public float camera_y;
    public float camera_z;
    public float angle_of_view;
    public float unit_x;
    public float unit_y;
    public float unit_z;

    public static UnitCameraPattern Parse(MasterDataReader reader)
    {
      return new UnitCameraPattern()
      {
        ID = reader.ReadInt(),
        camera_x = reader.ReadFloat(),
        camera_y = reader.ReadFloat(),
        camera_z = reader.ReadFloat(),
        angle_of_view = reader.ReadFloat(),
        unit_x = reader.ReadFloat(),
        unit_y = reader.ReadFloat(),
        unit_z = reader.ReadFloat()
      };
    }
  }
}
