// Decompiled with JetBrains decompiler
// Type: MasterDataTable.RecoveryItemAPHeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class RecoveryItemAPHeal
  {
    public int ID;
    public string name;
    public int recovery_amount;
    public string description;

    public static RecoveryItemAPHeal Parse(MasterDataReader reader)
    {
      return new RecoveryItemAPHeal()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        recovery_amount = reader.ReadInt(),
        description = reader.ReadString(true)
      };
    }
  }
}
