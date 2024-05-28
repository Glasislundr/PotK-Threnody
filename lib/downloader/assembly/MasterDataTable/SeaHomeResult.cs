// Decompiled with JetBrains decompiler
// Type: MasterDataTable.SeaHomeResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class SeaHomeResult
  {
    public int ID;
    public string name;
    public bool effect_on;
    public string effect_trigger;
    public bool gauge_effect_on;
    public string gauge_effect_trigger;

    public static SeaHomeResult Parse(MasterDataReader reader)
    {
      return new SeaHomeResult()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        effect_on = reader.ReadBool(),
        effect_trigger = reader.ReadString(true),
        gauge_effect_on = reader.ReadBool(),
        gauge_effect_trigger = reader.ReadString(true)
      };
    }
  }
}
