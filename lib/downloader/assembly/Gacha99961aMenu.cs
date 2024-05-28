// Decompiled with JetBrains decompiler
// Type: Gacha99961aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha99961aMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtPopuptitle;

  public Player player_data_ { get; set; }

  public bool isStackScene { get; set; } = true;

  public void SetText(int have_bugu, int max_have_bugu, Player player_data)
  {
    this.player_data_ = player_data;
    this.TxtDescription01.SetText(Consts.Format(Consts.GetInstance().GACHA_99961MENU_DESCRIPTION01));
    this.TxtDescription02.SetText(Consts.Format(Consts.GetInstance().GACHA_99961MENU_DESCRIPTION02));
    this.TxtDescription03.SetText(Consts.Format(Consts.GetInstance().GACHA_99961MENU_DESCRIPTION03, (IDictionary) new Hashtable()
    {
      {
        (object) "havebugu",
        (object) have_bugu.ToString().ToConverter()
      },
      {
        (object) "maxhavebugu",
        (object) max_have_bugu.ToString().ToConverter()
      }
    }));
    this.TxtPopuptitle.SetText(Consts.Format(Consts.GetInstance().GACHA_99961MENU_DESCRIPTION04));
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupExtend()
  {
    this.player_data_ = SMManager.Get<Player>();
    if (this.player_data_.CheckLimitMaxItem())
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_12_1());
    else
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._007_16());
  }

  public void IbtnPopupIntegrate()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Bugu0053Scene.changeScene(this.isStackScene);
  }

  public void IbtnPopupSell()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Bugu00525Scene.ChangeScene(this.isStackScene, Bugu00525Scene.Mode.Weapon);
  }

  public void IbtnPopupDrilling()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Bugu00526Scene.ChangeScene(this.isStackScene);
  }
}
