// Decompiled with JetBrains decompiler
// Type: Explore033RankingRewardScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033RankingRewardScene : NGSceneBase
{
  [SerializeField]
  private Explore033RankingRewardMenu menu;

  public static void changeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("explore033_RankingReward", true);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync()
  {
    Explore033RankingRewardScene rankingRewardScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/ExploreChallengeBackground").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    rankingRewardScene.backgroundPrefab = bgF.Result;
    yield return (object) rankingRewardScene.menu.InitializeAsync();
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;
}
