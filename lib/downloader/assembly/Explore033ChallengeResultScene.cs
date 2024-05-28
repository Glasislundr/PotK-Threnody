// Decompiled with JetBrains decompiler
// Type: Explore033ChallengeResultScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Explore033ChallengeResultScene : NGSceneBase
{
  [SerializeField]
  private Explore033ChallengeResultMenu menu;
  [SerializeField]
  private GameObject touchToNext;
  private ColosseumUtility.Info info;
  private List<ResultMenuBase> sequences;
  private bool toNextSequence;
  private bool isStarted;
  private Colosseum0234Scene.Param bootParam;
  private ResultMenuBase nowPlayBase;
  private GameCore.ColosseumResult result;
  private bool isWin;

  public override void onEndScene() => this.sequences.Clear();

  public void IbtnTouchToNext()
  {
    this.toNextSequence = true;
    if (!Object.op_Inequality((Object) this.nowPlayBase, (Object) null))
      return;
    this.nowPlayBase.isSkip = true;
  }

  public static void ChangeScene(GameCore.ColosseumResult result, Gladiator gladiator)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_Challenge_Result", false, (object) result, (object) gladiator);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Explore033ChallengeResultScene challengeResultScene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/ExploreChallengeBackground").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    challengeResultScene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(GameCore.ColosseumResult result, Gladiator gladiator)
  {
    Explore033ChallengeResultScene challengeResultScene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    challengeResultScene.isWin = result.isWin();
    challengeResultScene.result = result;
    ResultMenuBase.Param param = (ResultMenuBase.Param) null;
    Future<WebAPI.Response.ExploreChallengeFinish> receive = WebAPI.ExploreChallengeFinish(result.colosseumTransactionID, result.GetResultJsonString(), challengeResultScene.isWin, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = receive.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (receive.Result != null)
    {
      Singleton<NGGameDataManager>.GetInstance().challenge_point = receive.Result.challenge_point;
      param = new ResultMenuBase.Param(receive.Result);
      if (challengeResultScene.isWin)
        yield return (object) Singleton<ExploreDataManager>.GetInstance().AddFloor();
      challengeResultScene.sequences = new List<ResultMenuBase>()
      {
        (ResultMenuBase) ((Component) challengeResultScene).GetComponent<Explore033ChallengeResultMenu>(),
        (ResultMenuBase) ((Component) challengeResultScene).GetComponent<Explore033ChallengeRewardMenu>(),
        (ResultMenuBase) null
      };
      e1 = challengeResultScene.InitMenus(param, gladiator);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
  }

  public void onStartScene(GameCore.ColosseumResult result, Gladiator gladiator)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus());
  }

  private IEnumerator InitMenus(ResultMenuBase.Param param, Gladiator gladiator)
  {
    this.touchToNext.SetActive(false);
    foreach (ResultMenuBase sequence in this.sequences)
    {
      if (Object.op_Inequality((Object) sequence, (Object) null))
      {
        IEnumerator e = sequence.Init(param, gladiator);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RunMenus()
  {
    List<ResultMenuBase>.Enumerator seqe = this.sequences.GetEnumerator();
    IEnumerator e;
    while (seqe.MoveNext())
    {
      this.nowPlayBase = seqe.Current;
      if (!Object.op_Equality((Object) this.nowPlayBase, (Object) null))
      {
        this.touchToNext.SetActive(true);
        e = this.nowPlayBase.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.toNextSequence = false;
        while (!this.toNextSequence)
          yield return (object) null;
        this.toNextSequence = false;
        e = this.nowPlayBase.OnFinish();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        break;
    }
    this.nowPlayBase = (ResultMenuBase) null;
    this.touchToNext.SetActive(true);
    ((Collider) this.touchToNext.GetComponent<BoxCollider>()).enabled = false;
    while (seqe.MoveNext())
    {
      e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Explore033TopScene.changeScene(false, !this.isWin);
  }
}
