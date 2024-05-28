// Decompiled with JetBrains decompiler
// Type: Quest99951Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using UnityEngine;

#nullable disable
public class Quest99951Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtPopupdescripton01;
  [SerializeField]
  protected UILabel TxtPopupdescripton02;
  [SerializeField]
  protected UILabel TxtPopupdescripton03;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UIButton btnCom;
  protected Player player_data_;

  public Action<PopupUtility.SceneTo> onChangedScene { get; set; }

  public bool isStackScene { get; set; } = true;

  public virtual void SetText(int have_unit, int max_have_unit, Player player_data)
  {
    this.TxtPopupdescripton03.SetTextLocalize(Consts.GetInstance().GACHA_0065MENU_DESCRIPTION02 + "：[ff0000]" + have_unit.ToLocalizeNumberText() + "[-]/[ff0000]" + max_have_unit.ToLocalizeNumberText() + "[-]");
  }

  public void IbtnPopupCom()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Action<PopupUtility.SceneTo> onChangedScene = this.onChangedScene;
    if (onChangedScene != null)
      onChangedScene(PopupUtility.SceneTo.UnitTraining);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Unit004UnitTrainingListScene.changeScene(this.isStackScene);
  }

  public void IbtnPopupDisjoint()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Action<PopupUtility.SceneTo> onChangedScene = this.onChangedScene;
    if (onChangedScene != null)
      onChangedScene(PopupUtility.SceneTo.UnitSale);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Unit00468Scene.changeScene00410(this.isStackScene, Unit00410Menu.FromType.AlertUnitOver);
  }

  public void IbtnPopupStorage()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Action<PopupUtility.SceneTo> onChangedScene = this.onChangedScene;
    if (onChangedScene != null)
      onChangedScene(PopupUtility.SceneTo.UnitStorage);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Unit004StorageInScene.changeScene(this.isStackScene, true);
  }

  public void IbtnPopupExtend()
  {
    this.player_data_ = SMManager.Get<Player>();
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.player_data_.CheckLimitMaxUnit())
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_11_1());
    else
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._007_14(0.0f));
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
