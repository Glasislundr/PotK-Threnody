// Decompiled with JetBrains decompiler
// Type: APRecoveryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class APRecoveryList : MonoBehaviour
{
  private Action btnAct;
  [SerializeField]
  private CreateIconObject dirItemIcon;
  [SerializeField]
  private UILabel txtItemName;
  [SerializeField]
  private UILabel txtIntroduciton;
  [SerializeField]
  private UILabel txtProssessionValue;
  [SerializeField]
  private SpreadColorButton ibtnButtonRecovery;
  private RecoveryItemAPHeal masterItem;
  private int playerItemQuantity;
  private APRecoveryList.MODE mode;
  private string strApRecoveryAmount = "AP回復量 [ffff00]";
  private string strApRecovery = "AP回復";
  private string strApFull = "APは全快しています。";

  public IEnumerator Init(
    APRecoveryList.MODE mode,
    RecoveryItemAPHeal item,
    int quantity,
    Action questChangeScene)
  {
    this.mode = mode;
    this.SetBtnAct(questChangeScene);
    Player player = SMManager.Get<Player>();
    int nowAP = player.ap + player.ap_overflow;
    IEnumerator e;
    switch (this.mode)
    {
      case APRecoveryList.MODE.APRecoveryItem:
        this.InitItem(item, quantity, nowAP);
        e = this.dirItemIcon.CreateThumbnail(MasterDataTable.CommonRewardType.recovery_item, item.ID, visibleBottom: false, isButton: false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case APRecoveryList.MODE.APRecoveryStone:
        this.InitStone(quantity, nowAP);
        e = this.dirItemIcon.CreateThumbnail(MasterDataTable.CommonRewardType.coin, 1, visibleBottom: false, isButton: false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
    yield return (object) null;
  }

  public void InitItem(RecoveryItemAPHeal item, int quantity, int nowAP)
  {
    this.masterItem = item;
    this.playerItemQuantity = quantity;
    this.txtItemName.SetTextLocalize(item.name);
    this.txtIntroduciton.SetTextLocalize(this.strApRecoveryAmount + (object) item.recovery_amount);
    this.txtProssessionValue.SetTextLocalize(this.playerItemQuantity);
    int num = (Player.GetApOverChargeLimit() - nowAP) / item.recovery_amount;
    if (this.playerItemQuantity > 0 && num > 0)
      return;
    ((UIButtonColor) this.ibtnButtonRecovery).isEnabled = false;
  }

  public void InitStone(int playerStoneQuantity, int nowAP)
  {
    this.txtItemName.SetTextLocalize(Consts.GetInstance().UNIQUE_ICON_KISEKI);
    this.txtIntroduciton.SetTextLocalize(this.strApRecoveryAmount + Consts.GetInstance().AP_RECOVERY_AMOUNT_STONE);
    this.txtProssessionValue.SetTextLocalize(playerStoneQuantity);
    int num = (Player.GetApOverChargeLimit() - nowAP) / int.Parse(Consts.GetInstance().AP_RECOVERY_AMOUNT_STONE);
    if (playerStoneQuantity > 0 && num > 0)
      return;
    ((UIButtonColor) this.ibtnButtonRecovery).isEnabled = false;
  }

  public void SetBtnAct(Action questChangeScene) => this.btnAct = questChangeScene;

  public bool IsPush { get; set; }

  public void OnRecoveryButton()
  {
    Player player = SMManager.Get<Player>();
    if (player.ap + player.ap_overflow >= Player.GetApOverChargeLimit())
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      ModalWindow.Show(this.strApRecovery, this.strApFull, (Action) (() => this.IsPush = false));
    }
    else if (this.mode == APRecoveryList.MODE.APRecoveryStone)
      this.StartCoroutine(this.popup00712());
    else
      this.StartCoroutine(this.popupUseAPRecoveryitem(this.masterItem, this.playerItemQuantity));
  }

  private IEnumerator popupAPFull()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Future<GameObject> popupF = Res.Prefabs.popup.popup_999_11_1_1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupF.Result);
  }

  private IEnumerator popupUseAPRecoveryitem(RecoveryItemAPHeal master_item, int item_quantity)
  {
    APRecoveryList apRecoveryList = this;
    Future<GameObject> popupF = new ResourceObject("Prefabs/popup/popup_Use_AP_Recovery_Item").Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UseAPRecoveryItem component = Singleton<PopupManager>.GetInstance().open(popupF.Result).GetComponent<UseAPRecoveryItem>();
    component.SetBtnAct(apRecoveryList.btnAct);
    apRecoveryList.StartCoroutine(component.Init(master_item, item_quantity));
  }

  private IEnumerator popup00712()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_12__popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Shop00712Menu component = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Shop00712Menu>();
    component.setUserData();
    component.SetBtnAction(this.btnAct);
  }

  public enum MODE
  {
    APRecoveryItem = 1,
    APRecoveryStone = 2,
  }
}
