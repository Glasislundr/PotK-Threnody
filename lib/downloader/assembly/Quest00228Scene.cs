// Decompiled with JetBrains decompiler
// Type: Quest00228Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00228Scene : NGSceneBase
{
  [SerializeField]
  private Quest00228Menu menu;

  public static void ChangeScene(QuestScoreCampaignProgress qscp, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_28", (stack ? 1 : 0) != 0, (object) qscp);
  }

  public static void ChangeScene(Description description, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_28", (stack ? 1 : 0) != 0, (object) description);
  }

  public static void ChangeScene(QuestExtraDescription[] descriptions, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_28", (stack ? 1 : 0) != 0, (object[]) new QuestExtraDescription[1][]
    {
      descriptions
    });
  }

  public static void ChangeScene(int term_id, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_28", (stack ? 1 : 0) != 0, (object) term_id);
  }

  public static void ChangeScene(GuildRaidPeriod period, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_28", (stack ? 1 : 0) != 0, (object) period);
  }

  public IEnumerator onStartSceneAsync(QuestScoreCampaignProgress qscp)
  {
    IEnumerator e = this.menu.Init(qscp);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Description description)
  {
    IEnumerator e = this.menu.Init(description);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(QuestExtraDescription[] descriptions)
  {
    IEnumerator e = this.menu.Init(descriptions);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int term_id)
  {
    yield return (object) this.menu.Init(term_id);
  }

  public IEnumerator onStartSceneAsync(GuildRaidPeriod period)
  {
    IEnumerator e = this.menu.Init(period);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onEndScene() => DetailController.Release();
}
