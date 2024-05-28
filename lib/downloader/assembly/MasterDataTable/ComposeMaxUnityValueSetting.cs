// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ComposeMaxUnityValueSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ComposeMaxUnityValueSetting
  {
    public int ID;
    public string hp_compose_add_max;
    public string strength_compose_add_max;
    public string vitality_compose_add_max;
    public string intelligence_compose_add_max;
    public string mind_compose_add_max;
    public string agility_compose_add_max;
    public string dexterity_compose_add_max;
    public string lucky_compose_add_max;
    public string name;
    public string description;

    public static ComposeMaxUnityValueSetting Parse(MasterDataReader reader)
    {
      return new ComposeMaxUnityValueSetting()
      {
        ID = reader.ReadInt(),
        hp_compose_add_max = reader.ReadStringOrNull(true),
        strength_compose_add_max = reader.ReadStringOrNull(true),
        vitality_compose_add_max = reader.ReadStringOrNull(true),
        intelligence_compose_add_max = reader.ReadStringOrNull(true),
        mind_compose_add_max = reader.ReadStringOrNull(true),
        agility_compose_add_max = reader.ReadStringOrNull(true),
        dexterity_compose_add_max = reader.ReadStringOrNull(true),
        lucky_compose_add_max = reader.ReadStringOrNull(true),
        name = reader.ReadString(true),
        description = reader.ReadString(true)
      };
    }
  }
}
