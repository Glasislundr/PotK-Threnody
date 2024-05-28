// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DuelElementHitEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DuelElementHitEffect
  {
    public int ID;
    public string original_effect_name;
    public int element_CommonElement;
    public string change_effect_name;

    public static DuelElementHitEffect Parse(MasterDataReader reader)
    {
      return new DuelElementHitEffect()
      {
        ID = reader.ReadInt(),
        original_effect_name = reader.ReadStringOrNull(true),
        element_CommonElement = reader.ReadInt(),
        change_effect_name = reader.ReadStringOrNull(true)
      };
    }

    public CommonElement element => (CommonElement) this.element_CommonElement;
  }
}
