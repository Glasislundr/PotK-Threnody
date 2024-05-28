// Decompiled with JetBrains decompiler
// Type: Guild02871Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild02871Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel sceneTitle;
  [SerializeField]
  private NGxScrollMasonry Scroll;

  public IEnumerator InitializeAsync()
  {
    this.sceneTitle.SetTextLocalize(Consts.GetInstance().GUILD_BANK_HOWTO_TITLE);
    GuildBankHowto guildBankHowto = ((IEnumerable<GuildBankHowto>) MasterData.GuildBankHowtoList).FirstOrDefault<GuildBankHowto>((Func<GuildBankHowto, bool>) (x => x.kind == 1));
    GuildBankHowto[] array = ((IEnumerable<GuildBankHowto>) MasterData.GuildBankHowtoList).Where<GuildBankHowto>((Func<GuildBankHowto, bool>) (x => x.kind >= 2)).ToArray<GuildBankHowto>();
    IEnumerator e = DetailController.Init(this.Scroll, guildBankHowto == null ? string.Empty : guildBankHowto.body, array);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (instance.backScene())
      return;
    instance.destroyCurrentScene();
    instance.clearStack();
    Guild0287Scene.ChangeScene(false);
  }
}
