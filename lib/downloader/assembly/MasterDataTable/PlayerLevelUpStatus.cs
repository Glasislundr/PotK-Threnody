// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PlayerLevelUpStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PlayerLevelUpStatus
  {
    public int ID;
    public int ap;
    public int cost;
    public int friend;

    public static PlayerLevelUpStatus Parse(MasterDataReader reader)
    {
      return new PlayerLevelUpStatus()
      {
        ID = reader.ReadInt(),
        ap = reader.ReadInt(),
        cost = reader.ReadInt(),
        friend = reader.ReadInt()
      };
    }
  }
}
