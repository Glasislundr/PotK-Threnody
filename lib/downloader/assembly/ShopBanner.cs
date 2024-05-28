// Decompiled with JetBrains decompiler
// Type: ShopBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopBanner : NGMenuBase
{
  private ShopTopMenu menu;
  private string bannerTimeText;
  [SerializeField]
  private int ShopId;
  public int BannerId;
  public string Name;
  [SerializeField]
  private GameObject NewIcon;
  public UIButton Button;
  public Transform limitedParent;

  public void Init(ShopTopMenu menu, int bannerId, string name)
  {
    this.menu = menu;
    this.BannerId = bannerId;
    this.Name = name;
    this.UpdateNewIcon();
  }

  public void SetBannerTime(DateTime? EndTime)
  {
    this.bannerTimeText = (string) null;
    if (!EndTime.HasValue)
      return;
    this.bannerTimeText = string.Format(Consts.GetInstance().GACHA_0065MENU_COMMERCIAL_LIMIT, (object) EndTime.Value.ToString("MM/dd HH:mm"));
  }

  public void UpdateNewIcon()
  {
    if (!Object.op_Implicit((Object) this.NewIcon))
      return;
    if (ShopCommon.IsNewLimitedShopBanner(ShopCommon.LoginTime, this.BannerId))
      this.NewIcon.SetActive(true);
    else
      this.NewIcon.SetActive(false);
  }

  public void IbtnLimitedBanner()
  {
    if (this.menu.IsBannerSpringPanelSlideing())
      return;
    this.StartCoroutine(this.FadeOut());
  }

  private IEnumerator FadeOut()
  {
    yield return (object) this.menu.FadeOut();
    ShopItemListScene.ChangeScene(this.ShopId, this.BannerId, this.Name, this.bannerTimeText);
  }
}
