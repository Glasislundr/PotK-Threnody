// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearReisouChaosCreation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearReisouChaosCreation
  {
    public int ID;
    public int chaos_ID_GearGear;
    public int cost_sand;
    public int cost_medal;

    public static GearReisouChaosCreation Parse(MasterDataReader reader)
    {
      return new GearReisouChaosCreation()
      {
        ID = reader.ReadInt(),
        chaos_ID_GearGear = reader.ReadInt(),
        cost_sand = reader.ReadInt(),
        cost_medal = reader.ReadInt()
      };
    }

    public GearGear chaos_ID
    {
      get
      {
        GearGear chaosId;
        if (!MasterData.GearGear.TryGetValue(this.chaos_ID_GearGear, out chaosId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.chaos_ID_GearGear + "]"));
        return chaosId;
      }
    }
  }
}
