// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BeginnerNaviTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BeginnerNaviTitle
  {
    public int ID;
    public int category_BeginnerNaviCategory;
    public string title;
    public int priority;

    public BeginnerNaviDetail detail
    {
      get
      {
        return ((IEnumerable<BeginnerNaviDetail>) MasterData.BeginnerNaviDetailList).Single<BeginnerNaviDetail>((Func<BeginnerNaviDetail, bool>) (x => x.title.ID == this.ID));
      }
    }

    public static BeginnerNaviTitle Parse(MasterDataReader reader)
    {
      return new BeginnerNaviTitle()
      {
        ID = reader.ReadInt(),
        category_BeginnerNaviCategory = reader.ReadInt(),
        title = reader.ReadString(true),
        priority = reader.ReadInt()
      };
    }

    public BeginnerNaviCategory category
    {
      get
      {
        BeginnerNaviCategory category;
        if (!MasterData.BeginnerNaviCategory.TryGetValue(this.category_BeginnerNaviCategory, out category))
          Debug.LogError((object) ("Key not Found: MasterData.BeginnerNaviCategory[" + (object) this.category_BeginnerNaviCategory + "]"));
        return category;
      }
    }
  }
}
