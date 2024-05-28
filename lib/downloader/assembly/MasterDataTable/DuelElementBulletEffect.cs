// Decompiled with JetBrains decompiler
// Type: MasterDataTable.DuelElementBulletEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class DuelElementBulletEffect
  {
    public int ID;
    public string bullet_prefab_name;
    public int element_CommonElement;
    public string effect_name;

    public static DuelElementBulletEffect Parse(MasterDataReader reader)
    {
      return new DuelElementBulletEffect()
      {
        ID = reader.ReadInt(),
        bullet_prefab_name = reader.ReadStringOrNull(true),
        element_CommonElement = reader.ReadInt(),
        effect_name = reader.ReadStringOrNull(true)
      };
    }

    public CommonElement element => (CommonElement) this.element_CommonElement;
  }
}
