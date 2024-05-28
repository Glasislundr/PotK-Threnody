// Decompiled with JetBrains decompiler
// Type: PopupBattleEditSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Sortie/BattleEditSelect")]
public class PopupBattleEditSelect : BackButtonPopupBase
{
  [SerializeField]
  private UIButton btnToCustom_;
  private Action onSelectNormal_;
  private Action onSelectCustom_;
  private Action onClose_;

  public static void show(Action eventSelectNormal, Action eventSelectCustom, Action eventClose = null)
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupBattleEditSelect.doShow(eventSelectNormal, eventSelectCustom, eventClose));
  }

  public static IEnumerator doShow(
    Action eventSelectNormal,
    Action eventSelectCustom,
    Action eventClose)
  {
    PopupManager pm = Singleton<PopupManager>.GetInstance();
    pm.open((GameObject) null, isViewBack: false);
    Future<GameObject> loader = new ResourceObject("Prefabs/battle/Popup_BattleEdit_Select").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pm.dismiss();
    GameObject result = loader.Result;
    loader = (Future<GameObject>) null;
    pm.open(result).GetComponent<PopupBattleEditSelect>().initialize(eventSelectNormal, eventSelectCustom, eventClose);
  }

  private void initialize(Action eventSelectNormal, Action eventSelectCustom, Action eventClose)
  {
    this.setTopObject(((Component) this).gameObject);
    this.onSelectNormal_ = eventSelectNormal;
    this.onSelectCustom_ = eventSelectCustom;
    this.onClose_ = eventClose;
  }

  private void Start()
  {
    if (Persist.customDeckTutorial.Data.isUnlocked && Util.checkUnlockedPlayerLevel(Player.Current.level))
      return;
    ((UIButtonColor) this.btnToCustom_).isEnabled = false;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.commonClose();
    Action onClose = this.onClose_;
    if (onClose == null)
      return;
    onClose();
  }

  public void onClickedToNormal()
  {
    if (this.IsPushAndSet())
      return;
    this.commonClose();
    this.onSelectNormal_();
  }

  public void onClickedToCustom()
  {
    if (this.IsPushAndSet())
      return;
    this.commonClose();
    this.onSelectCustom_();
  }

  private void commonClose() => Singleton<PopupManager>.GetInstance().dismiss();
}
