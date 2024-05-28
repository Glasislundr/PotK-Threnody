// Decompiled with JetBrains decompiler
// Type: APRecovery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class APRecovery : BackButtonMenuBase
{
  [SerializeField]
  private UIButton ibtnNo;
  [SerializeField]
  public APRecoveryScrollContainer APRecoveryScrollContainer;
  public GameObject APRecoveryListPrefab;
  [SerializeField]
  public GameObject TxtDescription01;
  [SerializeField]
  public GameObject TxtDescription02;
  [SerializeField]
  private UILabel txtPlayerAP;
  private Action btnAct;
  private string strPlayerAP = "[ffff00]AP:{0}/{1}";

  public IEnumerator Init(
    bool isAPShortage,
    PlayerRecoveryItem[] apRecoveryItems,
    Action questChangeScene)
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    APRecovery apRecovery = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Player player = SMManager.Get<Player>();
    int num2 = player.ap + player.ap_overflow;
    apRecovery.txtPlayerAP.SetTextLocalize(string.Format(apRecovery.strPlayerAP, (object) num2, (object) player.ap_max));
    apRecovery.APRecoveryScrollContainer.Initialize(apRecovery.APRecoveryListPrefab, apRecoveryItems);
    apRecovery.SetBtnAct(questChangeScene);
    if (isAPShortage)
    {
      apRecovery.TxtDescription01.SetActive(false);
      apRecovery.TxtDescription02.SetActive(true);
    }
    apRecovery.StartCoroutine(apRecovery.DisplayAPRecoveryitemList());
    return false;
  }

  private IEnumerator DisplayAPRecoveryitemList()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    this.APRecoveryScrollContainer.SetVisible(false);
    APRecoveryScrollContainer targetContainer = this.APRecoveryScrollContainer;
    ((Component) targetContainer).GetComponent<APRecoveryScrollContainer>().SetBtnAct(this.btnAct);
    IEnumerator e = targetContainer.Create();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    targetContainer.SetVisible(true);
    targetContainer.scrollView.SetDragAmount(0.0f, ((UIProgressBar) targetContainer.scrollBar).value, true);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void SetBtnAct(Action questChangeScene) => this.btnAct = questChangeScene;
}
