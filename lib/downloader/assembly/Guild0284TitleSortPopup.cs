// Decompiled with JetBrains decompiler
// Type: Guild0284TitleSortPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class Guild0284TitleSortPopup : DisplayOrderSortPopup
{
  public void Init(Action sortAction) => this.Init(GuildUtil.getTitleSortCategory(), sortAction);

  public override void IbtnOK()
  {
    this.SaveData();
    base.IbtnOK();
  }

  public override void SaveData()
  {
    base.SaveData();
    GuildUtil.setTitleSortCategory(this.nowCategory);
    Persist.guildSetting.Flush();
  }
}
