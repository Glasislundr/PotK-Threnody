// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillDuelClipEventEffectDataPreload
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillDuelClipEventEffectDataPreload
  {
    public int ID;
    public int duel_effect_id;
    public string clipeventeffectdata_file_name;

    public static BattleskillDuelClipEventEffectDataPreload Parse(MasterDataReader reader)
    {
      return new BattleskillDuelClipEventEffectDataPreload()
      {
        ID = reader.ReadInt(),
        duel_effect_id = reader.ReadInt(),
        clipeventeffectdata_file_name = reader.ReadString(true)
      };
    }
  }
}
