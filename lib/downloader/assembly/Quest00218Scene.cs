// Decompiled with JetBrains decompiler
// Type: Quest00218Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestExtra/LxM_Scene")]
public class Quest00218Scene : NGSceneBase
{
  private static readonly string DefaultSceneName = "quest002_18";
  [SerializeField]
  private Quest00218Menu menu_;
  private bool isInit_;
  private int runningCoroutine_;
  private string bgName_;

  public static void changeScene(int LLId, int? SId = null, bool stack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Quest00218Scene.DefaultSceneName, (stack ? 1 : 0) != 0, (object) new Quest00218Scene.BootParam(LLId, SId));
  }

  public static void backOrChangeScene(int LLId, int? SId)
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene(Quest00218Scene.DefaultSceneName))
      return;
    Quest00218Scene.changeScene(LLId, SId, false);
  }

  public IEnumerator onStartSceneAsync(Quest00218Scene.BootParam param)
  {
    Quest00218Scene quest00218Scene = this;
    quest00218Scene.runningCoroutine_ = 0;
    if (quest00218Scene.isInit_)
    {
      quest00218Scene.StartCoroutine(quest00218Scene.doBGChange());
      yield return (object) quest00218Scene.menu_.updateTime();
      while (quest00218Scene.runningCoroutine_ > 0)
        yield return (object) null;
    }
    else
    {
      quest00218Scene.isInit_ = true;
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      yield return (object) null;
      QuestExtraLL questLL = MasterData.QuestExtraLL[param.LLId_];
      quest00218Scene.bgName_ = questLL.background_image_name;
      quest00218Scene.StartCoroutine(quest00218Scene.doBGChange());
      if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
      {
        Future<WebAPI.Response.QuestProgressExtra> extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        IEnumerator e1 = extra.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (extra.Result == null)
        {
          yield break;
        }
        else
        {
          WebAPI.SetLatestResponsedAt("QuestProgressExtra");
          extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
        }
      }
      PlayerExtraQuestS[] quests = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).L_M(param.LLId_);
      QuestExtraTimetable questExtraTimetable = SMManager.Get<QuestExtraTimetable>();
      yield return (object) quest00218Scene.menu_.initializeAsync(questLL, quests, questExtraTimetable.emphasis, questExtraTimetable.notice, param.focusSId_);
      if (quests.Length != 0)
        quest00218Scene.menu_.isSideQuest = quests[0].IsSideQuest();
      yield return (object) null;
      while (quest00218Scene.runningCoroutine_ > 0)
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  private IEnumerator doBGChange()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00218Scene quest00218Scene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      --quest00218Scene.runningCoroutine_;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ++quest00218Scene.runningCoroutine_;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) ((Component) quest00218Scene).GetComponent<BGChange>().ExtraBGprefabCreate(quest00218Scene.bgName_);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onStartScene(Quest00218Scene.BootParam param)
  {
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, param.LLId_);
  }

  public class BootParam
  {
    public int LLId_ { get; private set; }

    public int? focusSId_ { get; private set; }

    public BootParam(int LLId, int? SId)
    {
      this.LLId_ = LLId;
      this.focusSId_ = SId;
    }
  }
}
