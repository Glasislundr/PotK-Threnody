// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraScoreRankingReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraScoreRankingReward
  {
    public int ID;
    public int campaign_id;
    public string display_text;
    public string image_name;
    public int alignment;
    public int group_id;

    public static QuestExtraScoreRankingReward Parse(MasterDataReader reader)
    {
      return new QuestExtraScoreRankingReward()
      {
        ID = reader.ReadInt(),
        campaign_id = reader.ReadInt(),
        display_text = reader.ReadString(true),
        image_name = reader.ReadString(true),
        alignment = reader.ReadInt(),
        group_id = reader.ReadInt()
      };
    }
  }
}
