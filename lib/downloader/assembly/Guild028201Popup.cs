// Decompiled with JetBrains decompiler
// Type: Guild028201Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Guild028201Popup : MonoBehaviour
{
  private Guild02811Menu menu2811;
  private Guild02812Menu menu2812;
  private GuildUtil.MenuType aMenu;

  public void Initialize(Guild02811Menu guild02811menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2811;
    this.menu2811 = guild02811menu;
  }

  public void Initialize(Guild02812Menu guild02812menu)
  {
    this.aMenu = GuildUtil.MenuType.menu2812;
    this.menu2812 = guild02812menu;
  }

  public void onButtonOk()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    switch (this.aMenu)
    {
      case GuildUtil.MenuType.menu2811:
        this.menu2811.onButtonSetting();
        break;
      case GuildUtil.MenuType.menu2812:
        this.menu2812.onButtonSetting();
        break;
    }
  }
}
