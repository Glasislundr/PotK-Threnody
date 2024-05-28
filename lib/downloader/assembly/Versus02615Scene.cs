// Decompiled with JetBrains decompiler
// Type: Versus02615Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02615Scene : NGSceneBase
{
  [SerializeField]
  private Versus02615Menu menu;

  public static void ChangeScene(
    bool stack,
    RankingGroup[] ranking_data,
    WebAPI.Response.PvpBootCampaign_rewards[] campaign_rewards)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_15", (stack ? 1 : 0) != 0, (object) ranking_data, (object) campaign_rewards);
  }

  public IEnumerator onStartSceneAsync(
    RankingGroup[] ranking_data,
    WebAPI.Response.PvpBootCampaign_rewards[] campaign_rewards)
  {
    yield return (object) this.menu.Init(ranking_data, campaign_rewards);
  }
}
