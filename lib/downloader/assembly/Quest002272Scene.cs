// Decompiled with JetBrains decompiler
// Type: Quest002272Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest002272Scene : NGSceneBase
{
  [SerializeField]
  private Quest002272Menu menu;

  public static void ChangeScene(
    bool stack,
    QuestScoreCampaignProgressScore_achivement_rewards achivement_reward,
    int[] achivement_cleard,
    string title,
    int score)
  {
    Quest002272SceneChangeData quest002272SceneChangeData = new Quest002272SceneChangeData(achivement_reward, achivement_cleard, title, score);
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_27_2", (stack ? 1 : 0) != 0, (object) quest002272SceneChangeData);
  }

  public static void ChangeScene(
    bool stack,
    QuestScoreTotalReward[] rewards,
    string title,
    int score)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_27_2", (stack ? 1 : 0) != 0, (object) rewards, (object) title, (object) score);
  }

  public IEnumerator onStartSceneAsync(Quest002272SceneChangeData data)
  {
    IEnumerator e = this.menu.Initialize(data.achivement_reward, data.achivement_cleard, data.title, data.score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(QuestScoreTotalReward[] rewards, string title, int score)
  {
    IEnumerator e = this.menu.Initialize(rewards, title, score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
