// Decompiled with JetBrains decompiler
// Type: Quest999ReisouMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Quest999ReisouMenu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtPopupdescripton01;
  [SerializeField]
  protected UILabel TxtPopupdescripton02;
  [SerializeField]
  protected UILabel TxtPopupdescripton03;
  [SerializeField]
  protected UILabel TxtTitle;

  public void SetText(int have, int max_have)
  {
    this.TxtPopupdescripton01.SetText("[ff0000]" + Consts.GetInstance().QUEST_999_REISOU_MENU_SET_TEXT_01);
    this.TxtPopupdescripton02.SetText(Consts.GetInstance().QUEST_999_REISOU_MENU_SET_TEXT_02);
    this.TxtPopupdescripton03.SetText(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION02 + "：[ff0000]" + have.ToString().ToConverter() + "[-]/[ff0000]" + max_have.ToString().ToConverter() + "[-]");
    this.TxtTitle.SetText(Consts.GetInstance().QUEST_999_REISOU_MENU_SET_TEXT_03);
  }

  public void IbtnPopupDrilling()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Bugu00526Scene.ChangeScene(true);
  }

  public void IbtnPopupSell()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Bugu00525Scene.ChangeScene(true, Bugu00525Scene.Mode.Reisou);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnBack();
}
