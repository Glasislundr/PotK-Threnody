// Decompiled with JetBrains decompiler
// Type: Popup0171816Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Popup0171816Menu : BattleBackButtonMenuBase
{
  public void IbtnYes()
  {
    this.env.core.isAutoBattle.value = true;
    this.battleManager.popupCloseAll();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo() => this.battleManager.popupDismiss();
}
