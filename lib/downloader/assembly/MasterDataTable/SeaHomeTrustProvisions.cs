// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaHomeTrustProvisions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaHomeTrustProvisions
  {
    public int ID;
    public string name;
    public int min_trust;
    public int max_trust;

    public static SeaHomeTrustProvisions Parse(MasterDataReader reader)
    {
      return new SeaHomeTrustProvisions()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        min_trust = reader.ReadInt(),
        max_trust = reader.ReadInt()
      };
    }

    public bool HasUnit() => this.min_trust >= 0 && this.max_trust >= 0;

    public bool WithIn(int trust) => this.min_trust <= trust && this.max_trust >= trust;
  }
}
