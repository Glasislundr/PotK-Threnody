// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearMaterialQuestInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearMaterialQuestInfo
  {
    public int ID;
    public int gear_id;
    public string detail_desc1;
    public string detail_desc2;
    public string detail_desc3;

    public static GearMaterialQuestInfo Parse(MasterDataReader reader)
    {
      return new GearMaterialQuestInfo()
      {
        ID = reader.ReadInt(),
        gear_id = reader.ReadInt(),
        detail_desc1 = reader.ReadString(true),
        detail_desc2 = reader.ReadString(true),
        detail_desc3 = reader.ReadString(true)
      };
    }
  }
}
