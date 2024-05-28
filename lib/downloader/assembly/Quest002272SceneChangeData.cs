// Decompiled with JetBrains decompiler
// Type: Quest002272SceneChangeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
public class Quest002272SceneChangeData
{
  public QuestScoreCampaignProgressScore_achivement_rewards achivement_reward;
  public int[] achivement_cleard;
  public string title;
  public int score;

  public Quest002272SceneChangeData(
    QuestScoreCampaignProgressScore_achivement_rewards achivementReward,
    int[] achivementCleard,
    string title,
    int score)
  {
    this.achivement_reward = achivementReward;
    this.achivement_cleard = achivementCleard;
    this.title = title;
    this.score = score;
  }
}
