// Decompiled with JetBrains decompiler
// Type: Title0241SortPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class Title0241SortPopup : DisplayOrderSortPopup
{
  public void Init(Action sortAction)
  {
    this.Init(Persist.emblemSortCategory.Data.category, sortAction);
  }

  public override void IbtnOK()
  {
    this.SaveData();
    base.IbtnOK();
  }

  public override void SaveData()
  {
    base.SaveData();
    Persist.emblemSortCategory.Data.category = this.nowCategory;
    Persist.emblemSortCategory.Flush();
  }
}
