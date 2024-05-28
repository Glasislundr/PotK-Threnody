// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuEnd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public class MapEdit031MenuEnd : MapEditMenuBase
{
  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.End;

  protected override IEnumerator initializeAsync()
  {
    yield break;
  }

  protected override void onEnable()
  {
    this.ui3DEvent_.isEnabled_ = false;
    this.StartCoroutine(this.doConfirmExit());
  }

  protected override void onDisable()
  {
  }

  public override void onBackButton()
  {
  }

  private IEnumerator doConfirmExit()
  {
    MapEdit031MenuEnd mapEdit031MenuEnd = this;
    bool bWait = true;
    bool bOk = false;
    Consts instance = Consts.GetInstance();
    ModalWindow.ShowYesNo(instance.MAPEDIT_031_TITLE_CONFIRM_EXIT, instance.MAPEDIT_031_MESSAGE_CONFIRM_EXIT, (Action) (() =>
    {
      bWait = false;
      bOk = true;
    }), (Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
    if (bOk)
      mapEdit031MenuEnd.topMenu_.onExitOK();
    else
      mapEdit031MenuEnd.topMenu_.onExitNG();
  }
}
