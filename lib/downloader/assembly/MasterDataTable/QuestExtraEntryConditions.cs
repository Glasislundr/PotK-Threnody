// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraEntryConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraEntryConditions
  {
    public int ID;
    public int banner_category_QuestExtraBannerCategory;
    public int quest_id;
    public int value;
    public string text;

    public static QuestExtraEntryConditions Parse(MasterDataReader reader)
    {
      return new QuestExtraEntryConditions()
      {
        ID = reader.ReadInt(),
        banner_category_QuestExtraBannerCategory = reader.ReadInt(),
        quest_id = reader.ReadInt(),
        value = reader.ReadInt(),
        text = reader.ReadString(true)
      };
    }

    public QuestExtraBannerCategory banner_category
    {
      get => (QuestExtraBannerCategory) this.banner_category_QuestExtraBannerCategory;
    }
  }
}
