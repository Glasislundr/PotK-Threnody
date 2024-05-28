// Decompiled with JetBrains decompiler
// Type: Quest00231Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00231Scene : NGSceneBase
{
  [SerializeField]
  private Quest00231Menu menu;

  public static void ChangeScene(
    WebAPI.Response.EventTop eventInfo,
    PunitiveExpeditionEventReward[] rewardList,
    bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_31", (stack ? 1 : 0) != 0, (object) eventInfo, (object) rewardList);
  }

  public IEnumerator onStartSceneAsync(
    WebAPI.Response.EventTop eventInfo,
    PunitiveExpeditionEventReward[] rewardList)
  {
    Quest00231Scene quest00231Scene = this;
    if (eventInfo.IsGuild())
    {
      quest00231Scene.bgmFile = "BgmGuild";
      quest00231Scene.bgmName = "bgm085";
    }
    else
    {
      quest00231Scene.bgmFile = "";
      quest00231Scene.bgmName = "bgm001";
    }
    if (Object.op_Inequality((Object) quest00231Scene.menu, (Object) null))
    {
      IEnumerator e = quest00231Scene.menu.Init(eventInfo, rewardList);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
