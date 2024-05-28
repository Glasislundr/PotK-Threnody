// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpRankingKind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpRankingKind
  {
    public int ID;
    public int class_kind_PvpClassKind;
    public string name;

    public static PvpRankingKind Parse(MasterDataReader reader)
    {
      return new PvpRankingKind()
      {
        ID = reader.ReadInt(),
        class_kind_PvpClassKind = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }

    public PvpClassKind class_kind
    {
      get
      {
        PvpClassKind classKind;
        if (!MasterData.PvpClassKind.TryGetValue(this.class_kind_PvpClassKind, out classKind))
          Debug.LogError((object) ("Key not Found: MasterData.PvpClassKind[" + (object) this.class_kind_PvpClassKind + "]"));
        return classKind;
      }
    }
  }
}
