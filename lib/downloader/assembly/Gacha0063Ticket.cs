// Decompiled with JetBrains decompiler
// Type: Gacha0063Ticket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha0063Ticket : Gacha0063hindicator
{
  [SerializeField]
  private UI2DSprite TopImg;
  [SerializeField]
  private UI2DSprite TitleImg;
  [SerializeField]
  private UILabel GachaTickets;
  [SerializeField]
  private UILabel GachaTerm;
  [SerializeField]
  private UIButton m_detailButton;
  private GachaModuleGacha Gacha;
  private const float INTERVAL = 30f;
  private float Timer = 30f;

  public override void InitGachaModuleGacha(
    Gacha0063Menu gacha0063Menu,
    GachaModule gachaModule,
    DateTime serverTime,
    UIScrollView scrollView,
    int prefabCount)
  {
    this.Menu = gacha0063Menu;
    this.GachaModule = gachaModule;
    this.singleGachaButton.Init("", gachaModule.gacha[0], this.Menu, 4, gachaModule.number, gachaModule);
  }

  public override IEnumerator Set(GameObject detailPopup)
  {
    Gacha0063Ticket gacha0063Ticket = this;
    if (gacha0063Ticket.GachaModule.front_image_url != "")
    {
      IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(gacha0063Ticket.GachaModule.front_image_url, gacha0063Ticket.TopImg);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) gacha0063Ticket.TopImg).gameObject.SetActive(true);
    }
    else
      Debug.LogError((object) "メイン画像URLなし");
  }
}
