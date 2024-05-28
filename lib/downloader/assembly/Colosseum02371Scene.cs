// Decompiled with JetBrains decompiler
// Type: Colosseum02371Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum02371Scene : NGSceneBase
{
  [SerializeField]
  private Colosseum02371Menu menu;
  private RankingPlayer MyRanking;
  private RankingPlayer[] TotalRanking;
  private RankingPlayer[] FriendRanking;

  public static void ChangeScene(ColosseumUtility.Info colosseumInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_7_1", false, (object) colosseumInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Colosseum02371Scene colosseum02371Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02371Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(ColosseumUtility.Info colosseumInfo)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    IEnumerator e1;
    if (colosseumInfo.rankingUpdated || this.TotalRanking == null || this.FriendRanking == null || this.TotalRanking.Length == 0 || this.FriendRanking.Length == 0)
    {
      Future<WebAPI.Response.ColosseumRanking> receive = WebAPI.ColosseumRanking((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = receive.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (receive.Result == null)
      {
        yield break;
      }
      else
      {
        e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        this.TotalRanking = receive.Result.colosseum_ranking;
        this.MyRanking = receive.Result.my_ranking;
        this.FriendRanking = receive.Result.colosseum_friend_ranking;
        receive = (Future<WebAPI.Response.ColosseumRanking>) null;
      }
    }
    e1 = this.menu.Initialize(this.TotalRanking, this.FriendRanking, this.MyRanking, colosseumInfo);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }
}
