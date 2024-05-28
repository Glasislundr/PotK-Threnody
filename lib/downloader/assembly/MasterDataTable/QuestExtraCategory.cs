// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraCategory
  {
    public int ID;
    public string name;

    public static QuestExtraCategory Parse(MasterDataReader reader)
    {
      return new QuestExtraCategory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }
  }
}
