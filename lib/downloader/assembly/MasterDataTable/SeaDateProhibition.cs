// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaDateProhibition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaDateProhibition
  {
    public int ID;
    public int? same_character_id_UnitUnit;

    public static SeaDateProhibition Parse(MasterDataReader reader)
    {
      return new SeaDateProhibition()
      {
        ID = reader.ReadInt(),
        same_character_id_UnitUnit = reader.ReadIntOrNull()
      };
    }

    public UnitUnit same_character_id
    {
      get
      {
        if (!this.same_character_id_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit sameCharacterId;
        if (!MasterData.UnitUnit.TryGetValue(this.same_character_id_UnitUnit.Value, out sameCharacterId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.same_character_id_UnitUnit.Value + "]"));
        return sameCharacterId;
      }
    }
  }
}
