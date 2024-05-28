// Decompiled with JetBrains decompiler
// Type: Tower029RestartPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029RestartPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel lblTitle;
  [SerializeField]
  private UILabel lblDesc;
  [SerializeField]
  private UILabel lblPosession;
  [SerializeField]
  private UILabel lblPossesionValue;
  private GameObject unitSelectionPopup;
  private Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> actionUnitSelection;
  private TowerProgress progress;
  private int requiredCoin;
  [SerializeField]
  private GameObject commercialObj;
  [SerializeField]
  private UISprite popupBaseSprite;
  [SerializeField]
  private GameObject popupButtonObj;

  public void Initialize(
    GameObject popup,
    TowerProgress progress,
    Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> action,
    int coin)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.progress = progress;
    this.requiredCoin = coin;
    this.unitSelectionPopup = popup;
    this.actionUnitSelection = action;
    this.lblTitle.SetTextLocalize(Consts.GetInstance().TOWER_POPUP_RESTART_TITLE);
    this.lblDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_POPUP_RESTART_DESC, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (coin),
        (object) coin
      }
    }));
    this.lblPosession.SetTextLocalize(Consts.GetInstance().POPUP_TOWER_UNIT_RECOVERY_STONE_POSESSION);
    this.lblPossesionValue.SetTextLocalize(Player.Current.coin);
    if (this.requiredCoin > 0)
      return;
    this.commercialObj.SetActive(false);
    ((UIWidget) this.popupBaseSprite).SetDimensions(((UIWidget) this.popupBaseSprite).width, 360);
  }

  public void onYesBtn()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (Player.Current.coin < this.requiredCoin)
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_SHORTAGE_COIN_RECOVERY_TITLE, Consts.Format(Consts.GetInstance().TOWER_POPUP_SHORTAGE_COIN_RESTART_DESC, (IDictionary) new Hashtable()
      {
        {
          (object) "coin",
          (object) TowerUtil.RestartCoinNum
        }
      }), (Action) (() => { }));
    }
    else
    {
      GameObject prefab = this.unitSelectionPopup.Clone();
      prefab.SetActive(false);
      prefab.GetComponent<Tower029UnitRestartSelectionRecoveryPopup>().Initialize(this.progress, this.actionUnitSelection, TowerUtil.SequenceType.Restart);
      prefab.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
