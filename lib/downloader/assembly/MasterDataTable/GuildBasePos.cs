// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildBasePos
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildBasePos
  {
    public int ID;
    public float baseXpos;
    public float baseYpos;

    public static GuildBasePos Parse(MasterDataReader reader)
    {
      return new GuildBasePos()
      {
        ID = reader.ReadInt(),
        baseXpos = reader.ReadFloat(),
        baseYpos = reader.ReadFloat()
      };
    }
  }
}
