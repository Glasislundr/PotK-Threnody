// Decompiled with JetBrains decompiler
// Type: Tower029RankingScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029RankingScene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "tower029_ranking";
  [SerializeField]
  private Tower029RankingMenu menu_;

  public static void changeScene(int period_id, int tower_id, bool bstack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Tower029RankingScene.DEFAULT_NAME, (bstack ? 1 : 0) != 0, (object) period_id, (object) tower_id);
  }

  public IEnumerator onStartSceneAsync(int period_id, int tower_id)
  {
    Tower029RankingScene tower029RankingScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = tower029RankingScene.menu_.coInitalize(period_id, tower_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029RankingScene.bgmFile = TowerUtil.BgmFile;
    tower029RankingScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene(int period_id, int tower_id)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
  }
}
