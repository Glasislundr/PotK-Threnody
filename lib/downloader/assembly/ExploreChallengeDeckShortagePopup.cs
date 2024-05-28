// Decompiled with JetBrains decompiler
// Type: ExploreChallengeDeckShortagePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class ExploreChallengeDeckShortagePopup : BackButtonMenuBase
{
  public virtual void IbtnBack() => Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();

  public virtual void IbtnTeamSetting()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Explore033DeckEditScene.ChangeSceneChallengeDeckEdit();
  }

  public void IbtnNo() => this.IbtnBack();

  public override void onBackButton() => this.IbtnNo();
}
