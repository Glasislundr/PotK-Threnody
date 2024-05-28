// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthDesertCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthDesertCharacter
  {
    public int ID;
    public int desert_logic_EarthJoinLogicType;
    public string desert_logic_arg;
    public int character_id;

    public static EarthDesertCharacter Parse(MasterDataReader reader)
    {
      return new EarthDesertCharacter()
      {
        ID = reader.ReadInt(),
        desert_logic_EarthJoinLogicType = reader.ReadInt(),
        desert_logic_arg = reader.ReadStringOrNull(true),
        character_id = reader.ReadInt()
      };
    }

    public EarthJoinLogicType desert_logic
    {
      get => (EarthJoinLogicType) this.desert_logic_EarthJoinLogicType;
    }
  }
}
