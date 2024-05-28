// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CallCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CallCharacter
  {
    public int ID;
    public int same_character_id;
    public string first_person;
    public string master_name;
    public int like_item_id;
    public int call_skill_id;
    public string blue_date_place;
    public int call_script_id;
    public DateTime? start_at;

    public static CallCharacter Parse(MasterDataReader reader)
    {
      return new CallCharacter()
      {
        ID = reader.ReadInt(),
        same_character_id = reader.ReadInt(),
        first_person = reader.ReadString(true),
        master_name = reader.ReadString(true),
        like_item_id = reader.ReadInt(),
        call_skill_id = reader.ReadInt(),
        blue_date_place = reader.ReadString(true),
        call_script_id = reader.ReadInt(),
        start_at = reader.ReadDateTimeOrNull()
      };
    }
  }
}
