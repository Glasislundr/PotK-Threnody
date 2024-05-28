// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaDateResultStaging
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaDateResultStaging
  {
    public int ID;
    public string name;
    public float? trust_min;
    public float? trust_max;
    public int home_result_SeaHomeResult;

    public static SeaDateResultStaging Parse(MasterDataReader reader)
    {
      return new SeaDateResultStaging()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        trust_min = reader.ReadFloatOrNull(),
        trust_max = reader.ReadFloatOrNull(),
        home_result_SeaHomeResult = reader.ReadInt()
      };
    }

    public SeaHomeResult home_result
    {
      get
      {
        SeaHomeResult homeResult;
        if (!MasterData.SeaHomeResult.TryGetValue(this.home_result_SeaHomeResult, out homeResult))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeResult[" + (object) this.home_result_SeaHomeResult + "]"));
        return homeResult;
      }
    }

    public bool WithIn(float trust)
    {
      if (this.trust_min.HasValue)
      {
        if (this.trust_min.HasValue)
        {
          float? trustMin = this.trust_min;
          float num = trust;
          if ((double) trustMin.GetValueOrDefault() <= (double) num & trustMin.HasValue)
            goto label_3;
        }
        return false;
      }
label_3:
      if (!this.trust_max.HasValue)
        return true;
      if (!this.trust_max.HasValue)
        return false;
      float? trustMax = this.trust_max;
      float num1 = trust;
      return (double) trustMax.GetValueOrDefault() >= (double) num1 & trustMax.HasValue;
    }
  }
}
