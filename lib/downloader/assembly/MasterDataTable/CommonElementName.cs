// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CommonElementName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CommonElementName
  {
    private static readonly Dictionary<string, string> convTable_ = new Dictionary<string, string>()
    {
      {
        "火",
        "炎"
      }
    };
    public int ID;
    public string name;

    public static string GetName(int commonElementId)
    {
      CommonElementName commonElementName;
      string name;
      if (MasterData.CommonElementName.TryGetValue(commonElementId, out commonElementName))
      {
        if (!CommonElementName.convTable_.TryGetValue(commonElementName.name, out name))
          name = commonElementName.name;
      }
      else
        name = string.Empty;
      return name;
    }

    public static CommonElementName Parse(MasterDataReader reader)
    {
      return new CommonElementName()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }
  }
}
