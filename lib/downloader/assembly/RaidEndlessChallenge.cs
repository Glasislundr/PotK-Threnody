// Decompiled with JetBrains decompiler
// Type: RaidEndlessChallenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RaidEndlessChallenge : MonoBehaviour
{
  public UIButton chanllengeBtn;
  public UIButton closeBtn;
  private RaidTopMenu menu;

  public void Init(int period_id, RaidTopMenu topMenu)
  {
    ((Behaviour) this.chanllengeBtn).enabled = true;
    ((Behaviour) this.closeBtn).enabled = true;
    this.chanllengeBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.StartCoroutine(this.RaidEndlessEntry(period_id)))));
    this.closeBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onBackButton())));
    this.menu = topMenu;
  }

  private IEnumerator RaidEndlessEntry(int period_id)
  {
    RaidEndlessChallenge endlessChallenge = this;
    ((Behaviour) endlessChallenge.chanllengeBtn).enabled = false;
    ((Behaviour) endlessChallenge.closeBtn).enabled = false;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = WebAPI.GuildraidRaidEndlessEntry(period_id, new Action<WebAPI.Response.UserError>(endlessChallenge.\u003CRaidEndlessEntry\u003Eb__4_0)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Persist.guildRaidProgress.Data.lastPeriodId = period_id;
    Persist.guildRaidProgress.Data.lastLap = endlessChallenge.menu.topFloorNum - 1;
    Persist.guildRaidProgress.Data.lastOrder = 1;
    ((UIRect) ((Component) endlessChallenge).gameObject.GetComponent<UIWidget>()).alpha = 1f / 1000f;
    yield return (object) endlessChallenge.StartCoroutine(endlessChallenge.menu.InitializeAsync(true));
    if (!endlessChallenge.menu.isFailedInit)
      ((MonoBehaviour) endlessChallenge.menu).StartCoroutine(endlessChallenge.menu.playSceneStartEffect());
    Singleton<PopupManager>.GetInstance().dismiss();
    yield return (object) null;
  }

  public void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();
}
