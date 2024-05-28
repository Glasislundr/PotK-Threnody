// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaPresentPresentResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaPresentPresentResult
  {
    public int ID;
    public int? same_character_id_UnitUnit;
    public int? character_id;
    public int affinity_SeaPresentAffinity;
    public string serif;
    public string face;
    public string voice_cue_name;

    public static SeaPresentPresentResult Parse(MasterDataReader reader)
    {
      return new SeaPresentPresentResult()
      {
        ID = reader.ReadInt(),
        same_character_id_UnitUnit = reader.ReadIntOrNull(),
        character_id = reader.ReadIntOrNull(),
        affinity_SeaPresentAffinity = reader.ReadInt(),
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

    public SeaPresentAffinity affinity
    {
      get
      {
        SeaPresentAffinity affinity;
        if (!MasterData.SeaPresentAffinity.TryGetValue(this.affinity_SeaPresentAffinity, out affinity))
          Debug.LogError((object) ("Key not Found: MasterData.SeaPresentAffinity[" + (object) this.affinity_SeaPresentAffinity + "]"));
        return affinity;
      }
    }
  }
}
