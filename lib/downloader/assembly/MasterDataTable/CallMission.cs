// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallMission
  {
    public int ID;
    public int mission_type_id;
    public string item_category_id;
    public int number_times;
    public string prerequisite_mission_id;

    public static CallMission Parse(MasterDataReader reader)
    {
      return new CallMission()
      {
        ID = reader.ReadInt(),
        mission_type_id = reader.ReadInt(),
        item_category_id = reader.ReadString(true),
        number_times = reader.ReadInt(),
        prerequisite_mission_id = reader.ReadString(true)
      };
    }
  }
}
