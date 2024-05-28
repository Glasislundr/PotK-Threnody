// Decompiled with JetBrains decompiler
// Type: Raid032HuntingInfoMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Raid032HuntingInfoMenu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;
  [SerializeField]
  private GameObject noHuntingInfo;

  public IEnumerator InitAsync(
    GameObject listItemPrefab,
    GameObject unitIconPrefab,
    WebAPI.Response.GuildraidSubjugationHistory history)
  {
    if (history.subjugation_histories == null || history.subjugation_histories.Length == 0)
    {
      this.noHuntingInfo.SetActive(true);
    }
    else
    {
      this.noHuntingInfo.SetActive(false);
      foreach (WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories info in (IEnumerable<WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories>) ((IEnumerable<WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories>) history.subjugation_histories).OrderByDescending<WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories, DateTime>((Func<WebAPI.Response.GuildraidSubjugationHistorySubjugation_histories, DateTime>) (x => x.defeated_at)))
      {
        GameObject gameObject = listItemPrefab.Clone();
        this.scroll.Add(gameObject);
        yield return (object) gameObject.GetComponent<Raid032HuntingInfoScrollItem>().InitAsync(info, unitIconPrefab);
      }
      this.scroll.ResolvePosition();
    }
  }

  public void ClearScrollView() => this.scroll.Clear();

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
