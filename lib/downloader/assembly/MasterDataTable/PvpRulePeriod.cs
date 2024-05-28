// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpRulePeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpRulePeriod
  {
    public int ID;
    public int rule_no;
    public string rule_detail;

    public static PvpRulePeriod Parse(MasterDataReader reader)
    {
      return new PvpRulePeriod()
      {
        ID = reader.ReadInt(),
        rule_no = reader.ReadInt(),
        rule_detail = reader.ReadStringOrNull(true)
      };
    }
  }
}
