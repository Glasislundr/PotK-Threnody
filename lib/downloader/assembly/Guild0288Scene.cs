// Decompiled with JetBrains decompiler
// Type: Guild0288Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guild0288Scene : NGSceneBase
{
  [SerializeField]
  private Guild0288GuildInBattleResultMenu resultMenu;
  [SerializeField]
  private GameObject touchToNext;
  private List<ResultMenuBase> sequences;
  private bool toNextSequence;
  private bool isInitialized;
  private bool isStarted;
  private WebAPI.Response.GvgBattleFinish guildBattleResultData;
  private string enemyPlayerID;

  public override IEnumerator onInitSceneAsync()
  {
    Guild0288Scene guild0288Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.DefaultBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guild0288Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(
    WebAPI.Response.GvgBattleFinish guildBattleResultData,
    string enemyPlayerID)
  {
    Guild0288Scene guild0288Scene = this;
    if (!guild0288Scene.isInitialized)
    {
      guild0288Scene.isInitialized = true;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
      guild0288Scene.guildBattleResultData = guildBattleResultData;
      guild0288Scene.enemyPlayerID = enemyPlayerID;
      guild0288Scene.sequences = new List<ResultMenuBase>()
      {
        (ResultMenuBase) guild0288Scene.resultMenu
      };
      IEnumerator e = guild0288Scene.InitMenus(guild0288Scene.guildBattleResultData);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
  }

  public void onStartScene(
    WebAPI.Response.GvgBattleFinish guildBattleResultData,
    string enemyPlayerID)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.StartCoroutine(this.RunMenus());
  }

  private IEnumerator InitMenus(WebAPI.Response.GvgBattleFinish resultData)
  {
    this.touchToNext.SetActive(false);
    foreach (ResultMenuBase sequence in this.sequences)
    {
      if (Object.op_Inequality((Object) sequence, (Object) null))
      {
        IEnumerator e = sequence.Init(resultData);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RunMenus()
  {
    List<ResultMenuBase>.Enumerator seqe = this.sequences.GetEnumerator();
    while (seqe.MoveNext())
    {
      ResultMenuBase seq = seqe.Current;
      if (!Object.op_Equality((Object) seq, (Object) null))
      {
        IEnumerator e = seq.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.touchToNext.SetActive(true);
        while (!this.toNextSequence)
          yield return (object) null;
        this.touchToNext.SetActive(false);
        this.toNextSequence = false;
        e = seq.OnFinish();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        seq = (ResultMenuBase) null;
      }
      else
        break;
    }
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    Guild0282Scene.ChangeSceneBattleFinish(this.enemyPlayerID, this.guildBattleResultData.capture_star, false);
  }

  public static void ChangeScene(WebAPI.Response.GvgBattleFinish resultData, string enemyPlayerID)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    instance.changeScene("guild028_8", false, (object) resultData, (object) enemyPlayerID);
  }

  public void IbtnTouchToNext() => this.toNextSequence = true;
}
