// Decompiled with JetBrains decompiler
// Type: ExploreDeckEditPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class ExploreDeckEditPopup : BackButtonMenuBase
{
  [SerializeField]
  private ExploreDeckIndicator mExploreDeckIndicator;
  [SerializeField]
  private ExploreDeckIndicator mChallengaDeckIndicator;

  public IEnumerator InitializeAsync()
  {
    ExploreDataManager data = Singleton<ExploreDataManager>.GetInstance();
    data.SetReopenPopupStateDeckEdit();
    IEnumerator e = this.mExploreDeckIndicator.InitializeAsync(data.GetExploreDeck(), data.GetWinRate());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerChallenge playerChallenge = SMManager.Get<PlayerChallenge>();
    int winRate = data.GetWinRate(playerChallenge.win_count, playerChallenge.lose_count);
    e = this.mChallengaDeckIndicator.InitializeAsync(data.GetChallengeDeck(), winRate);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onEditExploreDeckButton()
  {
    if (this.IsPushAndSet())
      return;
    Explore033DeckEditScene.ChangeSceneExploreDeckEdit();
  }

  public void onEditChallengeDeckButton()
  {
    if (this.IsPushAndSet())
      return;
    Explore033DeckEditScene.ChangeSceneChallengeDeckEdit();
  }

  public void onCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<ExploreDataManager>.GetInstance().InitReopenPopupState();
  }

  public override void onBackButton() => this.onCloseButton();
}
