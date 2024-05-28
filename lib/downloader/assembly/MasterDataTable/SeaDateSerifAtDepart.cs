// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaDateSerifAtDepart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaDateSerifAtDepart
  {
    public int ID;
    public int? same_character_id_UnitUnit;
    public int? character_id;
    public int trust_provision_SeaHomeTrustProvisions;
    public string serif;
    public string face;
    public string voice_cue_name;

    public static SeaDateSerifAtDepart Parse(MasterDataReader reader)
    {
      return new SeaDateSerifAtDepart()
      {
        ID = reader.ReadInt(),
        same_character_id_UnitUnit = reader.ReadIntOrNull(),
        character_id = reader.ReadIntOrNull(),
        trust_provision_SeaHomeTrustProvisions = reader.ReadInt(),
        serif = reader.ReadString(true),
        face = reader.ReadString(true),
        voice_cue_name = reader.ReadString(true)
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

    public SeaHomeTrustProvisions trust_provision
    {
      get
      {
        SeaHomeTrustProvisions trustProvision;
        if (!MasterData.SeaHomeTrustProvisions.TryGetValue(this.trust_provision_SeaHomeTrustProvisions, out trustProvision))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeTrustProvisions[" + (object) this.trust_provision_SeaHomeTrustProvisions + "]"));
        return trustProvision;
      }
    }
  }
}
