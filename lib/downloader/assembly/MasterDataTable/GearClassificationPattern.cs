// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearClassificationPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearClassificationPattern
  {
    public int ID;
    public int kind_GearKind;
    public string name;
    public int attack_classification_GearAttackClassification;

    public static GearClassificationPattern Parse(MasterDataReader reader)
    {
      return new GearClassificationPattern()
      {
        ID = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        name = reader.ReadString(true),
        attack_classification_GearAttackClassification = reader.ReadInt()
      };
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }

    public GearAttackClassification attack_classification
    {
      get => (GearAttackClassification) this.attack_classification_GearAttackClassification;
    }
  }
}
