// Decompiled with JetBrains decompiler
// Type: MasterDataTable.HelpHelp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class HelpHelp
  {
    public int ID;
    public int priority;
    public int category_HelpCategory;
    public string subcategory_name;
    public string title;
    public string description;
    public string image_name;

    public static HelpHelp Parse(MasterDataReader reader)
    {
      return new HelpHelp()
      {
        ID = reader.ReadInt(),
        priority = reader.ReadInt(),
        category_HelpCategory = reader.ReadInt(),
        subcategory_name = reader.ReadString(true),
        title = reader.ReadString(true),
        description = reader.ReadString(true),
        image_name = reader.ReadString(true)
      };
    }

    public HelpCategory category
    {
      get
      {
        HelpCategory category;
        if (!MasterData.HelpCategory.TryGetValue(this.category_HelpCategory, out category))
          Debug.LogError((object) ("Key not Found: MasterData.HelpCategory[" + (object) this.category_HelpCategory + "]"));
        return category;
      }
    }
  }
}
