// Decompiled with JetBrains decompiler
// Type: GuildBattleEntryPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public class GuildBattleEntryPopup : BackButtonMenuBase
{
  public void onClickedEntry()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.coConfirmEntry());
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private IEnumerator coConfirmEntry()
  {
    GuildBattleEntryPopup battleEntryPopup = this;
    bool bwait = true;
    bool bok = false;
    ModalWindow.ShowYesNo(Consts.GetInstance().GUILD_MAP_MATING_ENTRY_TITLE, Consts.GetInstance().GUILD_MAP_MATING_ENTRY_MESSAGE, (Action) (() =>
    {
      bok = true;
      bwait = false;
    }), (Action) (() => bwait = false));
    while (bwait)
      yield return (object) null;
    if (!bok)
    {
      battleEntryPopup.IsPush = false;
    }
    else
    {
      Future<WebAPI.Response.GvgMatchingEntry> mating = WebAPI.GvgMatchingEntry();
      IEnumerator e = mating.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (mating.Result == null)
        battleEntryPopup.IsPush = false;
      else
        Singleton<PopupManager>.GetInstance().dismiss();
    }
  }
}
