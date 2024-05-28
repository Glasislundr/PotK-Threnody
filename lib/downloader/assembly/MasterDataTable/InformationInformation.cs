// Decompiled with JetBrains decompiler
// Type: MasterDataTable.InformationInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class InformationInformation
  {
    public int ID;
    public int category_InformationCategory;
    public int sub_category_InformationSubCategory;
    public string title;
    public string title_image;
    public string description1;
    public string description_image1;
    public string description_image_url1;
    public string description2;
    public string description_image2;
    public string description_image_url2;
    public string description3;
    public DateTime published_at;
    public DateTime? start_at;
    public DateTime? end_at;

    public static InformationInformation Parse(MasterDataReader reader)
    {
      return new InformationInformation()
      {
        ID = reader.ReadInt(),
        category_InformationCategory = reader.ReadInt(),
        sub_category_InformationSubCategory = reader.ReadInt(),
        title = reader.ReadString(true),
        title_image = reader.ReadString(true),
        description1 = reader.ReadString(true),
        description_image1 = reader.ReadString(true),
        description_image_url1 = reader.ReadString(true),
        description2 = reader.ReadString(true),
        description_image2 = reader.ReadString(true),
        description_image_url2 = reader.ReadString(true),
        description3 = reader.ReadString(true),
        published_at = reader.ReadDateTime(),
        start_at = reader.ReadDateTimeOrNull(),
        end_at = reader.ReadDateTimeOrNull()
      };
    }

    public InformationCategory category
    {
      get
      {
        InformationCategory category;
        if (!MasterData.InformationCategory.TryGetValue(this.category_InformationCategory, out category))
          Debug.LogError((object) ("Key not Found: MasterData.InformationCategory[" + (object) this.category_InformationCategory + "]"));
        return category;
      }
    }

    public InformationSubCategory sub_category
    {
      get
      {
        InformationSubCategory subCategory;
        if (!MasterData.InformationSubCategory.TryGetValue(this.sub_category_InformationSubCategory, out subCategory))
          Debug.LogError((object) ("Key not Found: MasterData.InformationSubCategory[" + (object) this.sub_category_InformationSubCategory + "]"));
        return subCategory;
      }
    }
  }
}
