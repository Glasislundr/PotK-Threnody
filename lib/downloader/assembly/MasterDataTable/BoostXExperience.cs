// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BoostXExperience
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BoostXExperience
  {
    public int ID;
    public int period_BoostPeriod;
    public float scale;

    public static BoostXExperience Parse(MasterDataReader reader)
    {
      return new BoostXExperience()
      {
        ID = reader.ReadInt(),
        period_BoostPeriod = reader.ReadInt(),
        scale = reader.ReadFloat()
      };
    }

    public BoostPeriod period
    {
      get
      {
        BoostPeriod period;
        if (!MasterData.BoostPeriod.TryGetValue(this.period_BoostPeriod, out period))
          Debug.LogError((object) ("Key not Found: MasterData.BoostPeriod[" + (object) this.period_BoostPeriod + "]"));
        return period;
      }
    }
  }
}
