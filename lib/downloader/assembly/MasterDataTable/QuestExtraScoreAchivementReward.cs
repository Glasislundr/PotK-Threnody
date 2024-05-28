// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraScoreAchivementReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraScoreAchivementReward
  {
    public int ID;
    public string display_text;
    public string image_name;
    public int alignement;

    public static QuestExtraScoreAchivementReward Parse(MasterDataReader reader)
    {
      return new QuestExtraScoreAchivementReward()
      {
        ID = reader.ReadInt(),
        display_text = reader.ReadString(true),
        image_name = reader.ReadString(true),
        alignement = reader.ReadInt()
      };
    }
  }
}
