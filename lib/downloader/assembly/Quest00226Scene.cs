// Decompiled with JetBrains decompiler
// Type: Quest00226Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00226Scene : NGSceneBase
{
  [SerializeField]
  private Quest00226Menu menu00226;
  protected bool IsInit;
  private bool initialize;
  private string bgName_ = string.Empty;

  public static void ChangeScene(int Sid, bool stack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_26", (stack ? 1 : 0) != 0, (object) Sid);
  }

  public IEnumerator onStartSceneAsync()
  {
    Quest00226Scene quest00226Scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (!quest00226Scene.IsInit)
    {
      quest00226Scene.initialize = true;
      quest00226Scene.IsInit = true;
      int sid = Array.Find<QuestExtraS>(MasterData.QuestExtraSList, (Predicate<QuestExtraS>) (x => x.quest_l_QuestExtraL == 1)).ID;
      PlayerExtraQuestS[] Extra = SMManager.Get<PlayerExtraQuestS[]>();
      IEnumerator e = ((Component) quest00226Scene).GetComponent<BGChange>().ExtraBGprefabCreate("");
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = quest00226Scene.menu00226.Init(Res.Prefabs.quest002_19.list.Load<GameObject>(), Res.Prefabs.quest002_17.scroll.Load<GameObject>(), Extra, 1, sid, new int[0], new QuestExtraTimetableNotice[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Extra = (PlayerExtraQuestS[]) null;
    }
  }

  public IEnumerator onStartSceneAsync(int Sid)
  {
    Quest00226Scene quest00226Scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e1;
    if (!quest00226Scene.IsInit)
    {
      quest00226Scene.initialize = false;
      quest00226Scene.IsInit = true;
      Future<WebAPI.Response.QuestscoreReward> reward = WebAPI.QuestscoreReward((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<NGSceneManager>.GetInstance().changeScene("quest002_17", false);
      }));
      e1 = reward.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (reward.Result != null)
      {
        WebAPI.Response.QuestscoreRewardRewards[] Reward = reward.Result.rewards;
        bool AlreadyReceived = reward.Result.already_received;
        QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
        PlayerExtraQuestS[] Extra = SMManager.Get<PlayerExtraQuestS[]>();
        QuestScoreCampaignProgress campaignData = (QuestScoreCampaignProgress) null;
        int lid = 0;
        string name = "";
        foreach (PlayerExtraQuestS playerExtraQuestS in Extra)
        {
          PlayerExtraQuestS ex = playerExtraQuestS;
          if (ex.quest_extra_s != null && ex.quest_extra_s.ID == Sid)
          {
            lid = ex.quest_extra_s.quest_l_QuestExtraL;
            name = ex.quest_extra_s.quest_l.background_image_name;
            campaignData = ((IEnumerable<QuestScoreCampaignProgress>) source).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == ex.quest_extra_s.quest_l.ID));
            break;
          }
        }
        quest00226Scene.bgName_ = name;
        if (campaignData != null)
          quest00226Scene.initialize = true;
        e1 = ((Component) quest00226Scene).GetComponent<BGChange>().ExtraBGprefabCreate(name);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        e1 = quest00226Scene.menu00226.Init(AlreadyReceived, Extra, Reward, lid, campaignData);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        reward = (Future<WebAPI.Response.QuestscoreReward>) null;
        Reward = (WebAPI.Response.QuestscoreRewardRewards[]) null;
        Extra = (PlayerExtraQuestS[]) null;
        campaignData = (QuestScoreCampaignProgress) null;
      }
    }
    else if (!string.IsNullOrEmpty(quest00226Scene.bgName_))
    {
      e1 = ((Component) quest00226Scene).GetComponent<BGChange>().ExtraBGprefabCreate(quest00226Scene.bgName_);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;

  public void onStartScene(int Sid)
  {
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, Sid);
    if (!this.initialize)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
