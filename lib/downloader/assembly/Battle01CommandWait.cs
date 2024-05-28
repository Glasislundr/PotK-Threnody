// Decompiled with JetBrains decompiler
// Type: Battle01CommandWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandWait : BattleMonoBehaviour, IButtonEnableBeheviour
{
  private BattleUIController controller;
  private UIButton button;
  private static Action onClickAction;

  public bool buttonEnable
  {
    set => ((UIButtonColor) this.button).isEnabled = value;
  }

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandWait behaviour = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    behaviour.button = ((Component) behaviour).GetComponent<UIButton>();
    EventDelegate.Set(behaviour.button.onClick, new EventDelegate((MonoBehaviour) behaviour, "onClick"));
    behaviour.controller = behaviour.battleManager.getManager<NGBattleUIManager>().controller;
    behaviour.controller.setButtonBehaviour((IButtonEnableBeheviour) behaviour);
    return false;
  }

  public void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel)
      return;
    this.controller.uiWait();
    if (Battle01CommandWait.onClickAction == null)
      return;
    Battle01CommandWait.onClickAction();
  }

  public static void SetOnClickAction(Action action) => Battle01CommandWait.onClickAction = action;
}
