// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitGenderText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitGenderText
  {
    public int ID;
    public string name;

    public static UnitGenderText Parse(MasterDataReader reader)
    {
      return new UnitGenderText()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }

    public static string GetText(UnitGender gender)
    {
      UnitGenderText unitGenderText = (UnitGenderText) null;
      return MasterData.UnitGenderText.TryGetValue((int) gender, out unitGenderText) ? unitGenderText.name : string.Empty;
    }
  }
}
