// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GachaTutorialFixedEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GachaTutorialFixedEntity
  {
    public int ID;
    public int _gacha_id;
    public int _deck_id;
    public int _fix_count;
    public string name;

    public static GachaTutorialFixedEntity Parse(MasterDataReader reader)
    {
      return new GachaTutorialFixedEntity()
      {
        ID = reader.ReadInt(),
        _gacha_id = reader.ReadInt(),
        _deck_id = reader.ReadInt(),
        _fix_count = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }
  }
}
