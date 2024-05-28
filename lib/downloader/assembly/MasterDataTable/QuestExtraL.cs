// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestExtraL
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestExtraL
  {
    public int ID;
    public string name;
    public int? description;
    public string background_image_name;
    public int category_QuestExtraCategory;
    public int? quest_ll_QuestExtraLL;
    public bool enabled_header;
    public int? banner_image_id;

    public static QuestExtraL Parse(MasterDataReader reader)
    {
      return new QuestExtraL()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadIntOrNull(),
        background_image_name = reader.ReadString(true),
        category_QuestExtraCategory = reader.ReadInt(),
        quest_ll_QuestExtraLL = reader.ReadIntOrNull(),
        enabled_header = reader.ReadBool(),
        banner_image_id = reader.ReadIntOrNull()
      };
    }

    public QuestExtraCategory category
    {
      get
      {
        QuestExtraCategory category;
        if (!MasterData.QuestExtraCategory.TryGetValue(this.category_QuestExtraCategory, out category))
          Debug.LogError((object) ("Key not Found: MasterData.QuestExtraCategory[" + (object) this.category_QuestExtraCategory + "]"));
        return category;
      }
    }

    public QuestExtraLL quest_ll
    {
      get
      {
        if (!this.quest_ll_QuestExtraLL.HasValue)
          return (QuestExtraLL) null;
        QuestExtraLL questLl;
        if (!MasterData.QuestExtraLL.TryGetValue(this.quest_ll_QuestExtraLL.Value, out questLl))
          Debug.LogError((object) ("Key not Found: MasterData.QuestExtraLL[" + (object) this.quest_ll_QuestExtraLL.Value + "]"));
        return questLl;
      }
    }
  }
}
