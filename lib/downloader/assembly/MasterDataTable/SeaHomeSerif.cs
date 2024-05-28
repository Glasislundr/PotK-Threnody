// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaHomeSerif
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaHomeSerif
  {
    public int ID;
    public int? same_character_id_UnitUnit;
    public int? character_id;
    public int? time_zone_SeaHomeTimeZone;
    public int trust_provision_SeaHomeTrustProvisions;
    public int weiht;
    public string serif;
    public string face;
    public string voice_cue_name;
    public bool is_rare;

    public static SeaHomeSerif Parse(MasterDataReader reader)
    {
      return new SeaHomeSerif()
      {
        ID = reader.ReadInt(),
        same_character_id_UnitUnit = reader.ReadIntOrNull(),
        character_id = reader.ReadIntOrNull(),
        time_zone_SeaHomeTimeZone = reader.ReadIntOrNull(),
        trust_provision_SeaHomeTrustProvisions = reader.ReadInt(),
        weiht = reader.ReadInt(),
        serif = reader.ReadString(true),
        face = reader.ReadString(true),
        voice_cue_name = reader.ReadString(true),
        is_rare = reader.ReadBool()
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

    public SeaHomeTimeZone time_zone
    {
      get
      {
        if (!this.time_zone_SeaHomeTimeZone.HasValue)
          return (SeaHomeTimeZone) null;
        SeaHomeTimeZone timeZone;
        if (!MasterData.SeaHomeTimeZone.TryGetValue(this.time_zone_SeaHomeTimeZone.Value, out timeZone))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeTimeZone[" + (object) this.time_zone_SeaHomeTimeZone.Value + "]"));
        return timeZone;
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
