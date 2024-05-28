// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpMatchingType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpMatchingType
  {
    public int ID;
    public string room_key;

    public static PvpMatchingType Parse(MasterDataReader reader)
    {
      return new PvpMatchingType()
      {
        ID = reader.ReadInt(),
        room_key = reader.ReadString(true)
      };
    }
  }
}
