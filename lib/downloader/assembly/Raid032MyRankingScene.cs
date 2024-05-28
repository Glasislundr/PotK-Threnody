// Decompiled with JetBrains decompiler
// Type: Raid032MyRankingScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032MyRankingScene : NGSceneBase
{
  [SerializeField]
  private Raid032MyRankingMenu menu;

  public static void changeScene(GuildRaid raid, bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().clearStack("raid032_MyRanking");
    Singleton<NGSceneManager>.GetInstance().changeScene("raid032_MyRanking", (isStack ? 1 : 0) != 0, (object) raid);
  }

  public IEnumerator onStartSceneAsync(GuildRaid raid)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForEndOfFrame();
    IEnumerator e = this.menu.coInitalize(raid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(GuildRaid raid) => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public override IEnumerator onEndSceneAsync()
  {
    Raid032MyRankingScene raid032MyRankingScene = this;
    float startTime = Time.time;
    while (!raid032MyRankingScene.isTweenFinished && (double) Time.time - (double) startTime < (double) raid032MyRankingScene.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    raid032MyRankingScene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) raid032MyRankingScene.\u003C\u003En__0();
  }
}
