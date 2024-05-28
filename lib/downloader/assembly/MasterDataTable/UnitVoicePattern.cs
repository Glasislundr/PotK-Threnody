// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitVoicePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitVoicePattern
  {
    public int ID;
    public string dead_message;
    public string file_name;
    public int selector_label;
    private string selectorLabel = string.Empty;

    public static UnitVoicePattern Parse(MasterDataReader reader)
    {
      return new UnitVoicePattern()
      {
        ID = reader.ReadInt(),
        dead_message = reader.ReadString(true),
        file_name = reader.ReadString(true),
        selector_label = reader.ReadInt()
      };
    }

    public string SelectorLabel
    {
      get
      {
        if (this.selector_label == 0)
          return (string) null;
        if (string.IsNullOrEmpty(this.selectorLabel))
          this.selectorLabel = this.selector_label.ToString();
        return this.selectorLabel;
      }
    }
  }
}
