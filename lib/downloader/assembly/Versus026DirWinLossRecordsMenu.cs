// Decompiled with JetBrains decompiler
// Type: Versus026DirWinLossRecordsMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Versus026DirWinLossRecordsMenu : BackButtonMenuBase
{
  private WebAPI.Response.PvpClassMatchHistory apiResponse;
  [SerializeField]
  private NGxScroll Scroll;
  [SerializeField]
  private UILabel txtSceneTitle;
  private GameObject unitPrefab;
  private Versus02613Scene.BootParam param_;

  public IEnumerator Init(Versus02613Scene.BootParam param)
  {
    this.param_ = param;
    this.txtSceneTitle.SetText(Consts.GetInstance().VERSUS_0026_WIN_LOSS_RECORDS_TITLE);
    Future<WebAPI.Response.PvpClassMatchHistory> apiF = WebAPI.PvpClassMatchHistory(param.current.playerId, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.apiResponse = apiF.Result;
    WebAPI.Response.PvpClassMatchHistoryClass_match_records[] history = this.apiResponse.class_match_records;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitPrefab = prefabF.Result;
    e = this.SetScroll(history);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetScroll(
    WebAPI.Response.PvpClassMatchHistoryClass_match_records[] pvpMatchHistory)
  {
    Versus026DirWinLossRecordsMenu vMenu = this;
    vMenu.Scroll.Clear();
    Future<GameObject> winLossRecordsF = Res.Prefabs.versus026_win_loss_records.dir_win_loss_records.Load<GameObject>();
    IEnumerator e = winLossRecordsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject winLossRecords = winLossRecordsF.Result;
    foreach (WebAPI.Response.PvpClassMatchHistoryClass_match_records info in ((IEnumerable<WebAPI.Response.PvpClassMatchHistoryClass_match_records>) pvpMatchHistory).OrderByDescending<WebAPI.Response.PvpClassMatchHistoryClass_match_records, DateTime>((Func<WebAPI.Response.PvpClassMatchHistoryClass_match_records, DateTime>) (x => x.start_at)).ToList<WebAPI.Response.PvpClassMatchHistoryClass_match_records>())
    {
      GameObject gameObject = winLossRecords.Clone();
      vMenu.Scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus026DirWinLossRecordsScrollItem>().CreateItem(vMenu, vMenu.unitPrefab, info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    vMenu.Scroll.ResolvePosition();
  }

  public void ChangeSceneToDetails(string battleID)
  {
    Versus026DirWinLossRecordsDetailsScene.ChangeScene(true, this.param_, battleID, this.param_.current.playerId);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.param_.pop();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
