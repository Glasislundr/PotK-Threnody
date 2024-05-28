// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaPresentAffinity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaPresentAffinity
  {
    public int ID;
    public string name;
    public float coefficient;
    public int? home_result_SeaHomeResult;

    public static SeaPresentAffinity Parse(MasterDataReader reader)
    {
      return new SeaPresentAffinity()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        coefficient = reader.ReadFloat(),
        home_result_SeaHomeResult = reader.ReadIntOrNull()
      };
    }

    public SeaHomeResult home_result
    {
      get
      {
        if (!this.home_result_SeaHomeResult.HasValue)
          return (SeaHomeResult) null;
        SeaHomeResult homeResult;
        if (!MasterData.SeaHomeResult.TryGetValue(this.home_result_SeaHomeResult.Value, out homeResult))
          Debug.LogError((object) ("Key not Found: MasterData.SeaHomeResult[" + (object) this.home_result_SeaHomeResult.Value + "]"));
        return homeResult;
      }
    }
  }
}
