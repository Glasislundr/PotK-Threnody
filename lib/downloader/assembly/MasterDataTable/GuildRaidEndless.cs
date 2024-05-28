// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildRaidEndless
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildRaidEndless
  {
    public int ID;
    public int hp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int agility;
    public int dexterity;
    public int lucky;

    public static GuildRaidEndless Parse(MasterDataReader reader)
    {
      return new GuildRaidEndless()
      {
        ID = reader.ReadInt(),
        hp = reader.ReadInt(),
        strength = reader.ReadInt(),
        vitality = reader.ReadInt(),
        intelligence = reader.ReadInt(),
        mind = reader.ReadInt(),
        agility = reader.ReadInt(),
        dexterity = reader.ReadInt(),
        lucky = reader.ReadInt()
      };
    }
  }
}
