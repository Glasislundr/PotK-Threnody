// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestCommonSpecialColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestCommonSpecialColor
  {
    public int ID;
    public string color_code;

    public static QuestCommonSpecialColor Parse(MasterDataReader reader)
    {
      return new QuestCommonSpecialColor()
      {
        ID = reader.ReadInt(),
        color_code = reader.ReadString(true)
      };
    }
  }
}
