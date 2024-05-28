// Decompiled with JetBrains decompiler
// Type: Quest00219Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestExtra/M_Scene")]
public class Quest00219Scene : NGSceneBase
{
  public Quest00219Menu menu;
  protected bool IsInit;

  public static void ChangeScene(int Sid, bool stack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_19", (stack ? 1 : 0) != 0, (object) Sid);
  }

  public IEnumerator onStartSceneAsync()
  {
    Quest00219Scene quest00219Scene = this;
    if (!quest00219Scene.IsInit)
    {
      quest00219Scene.IsInit = true;
      int sid = Array.Find<QuestExtraS>(MasterData.QuestExtraSList, (Predicate<QuestExtraS>) (x => x.quest_l_QuestExtraL == 1)).ID;
      PlayerExtraQuestS[] Extra = SMManager.Get<PlayerExtraQuestS[]>();
      IEnumerator e = ((Component) quest00219Scene).GetComponent<BGChange>().ExtraBGprefabCreate("");
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = quest00219Scene.menu.Init(Res.Prefabs.quest002_19.list.Load<GameObject>(), Res.Prefabs.quest002_17.scroll.Load<GameObject>(), Extra, 1, sid, new int[0], new QuestExtraTimetableNotice[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public virtual IEnumerator onStartSceneAsync(int Sid)
  {
    Quest00219Scene quest00219Scene = this;
    if (quest00219Scene.menu.IncludingKeyGate || !quest00219Scene.IsInit)
    {
      quest00219Scene.IsInit = true;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      IEnumerator e1;
      if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
      {
        Future<WebAPI.Response.QuestProgressExtra> extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        e1 = extra.Wait();
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
      PlayerExtraQuestS[] Extra = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>();
      QuestExtraTimetable timetable = SMManager.Get<QuestExtraTimetable>();
      PlayerExtraQuestS targetExtra = Array.Find<PlayerExtraQuestS>(Extra, (Predicate<PlayerExtraQuestS>) (x => x._quest_extra_s == Sid));
      int lid = targetExtra.quest_extra_s.quest_l_QuestExtraL;
      string backgroundImageName = targetExtra.quest_extra_s.quest_l.background_image_name;
      e1 = ((Component) quest00219Scene).GetComponent<BGChange>().ExtraBGprefabCreate(backgroundImageName);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = quest00219Scene.menu.Init(Res.Prefabs.quest002_19.list.Load<GameObject>(), Res.Prefabs.quest002_17.scroll.Load<GameObject>(), Extra, lid, Sid, timetable.emphasis, timetable.notice);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      quest00219Scene.menu.isSideQuest = targetExtra.IsSideQuest();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  public void onStartScene()
  {
  }

  public void onStartScene(int Sid)
  {
    Persist.eventStoryPlay.Data.SetReserveList(StoryPlaybackEventPlay.GetPlayIDList(ServerTime.NowAppTime(), this.sceneName), this.sceneName);
    Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, Sid);
  }
}
