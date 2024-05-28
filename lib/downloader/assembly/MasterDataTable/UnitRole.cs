// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitRole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitRole
  {
    public int ID;
    public int icon;
    public string name;
    public string description;

    public static UnitRole Parse(MasterDataReader reader)
    {
      return new UnitRole()
      {
        ID = reader.ReadInt(),
        icon = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true)
      };
    }
  }
}
