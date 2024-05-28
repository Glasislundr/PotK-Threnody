// Decompiled with JetBrains decompiler
// Type: Startup00012ButtonManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class Startup00012ButtonManager : MonoBehaviour
{
  public string scene;
  public string param;

  public void onChangeScene()
  {
    if (this.scene == "")
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1002");
    if (Regex.IsMatch(this.scene, "https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+"))
      App.OpenUrl(this.scene);
    else if (this.scene == "quest002_4" || this.scene == "quest002_4_lost_ragnarok")
    {
      LastPlayPlayerStoryQuestSIds playerStoryQuestSids = SMManager.Get<LastPlayPlayerStoryQuestSIds>();
      QuestStoryS questStoryS1;
      QuestStoryS questStoryS2;
      Quest00240723Scene.ChangeScene0024(true, playerStoryQuestSids == null ? (!(this.scene == "quest002_4") ? 19 : 1) : (!(this.scene == "quest002_4") ? (!playerStoryQuestSids.lost_ragnarok_quest_s_id.HasValue || !MasterData.QuestStoryS.TryGetValue(playerStoryQuestSids.lost_ragnarok_quest_s_id.Value, out questStoryS2) ? 19 : questStoryS2.quest_l_QuestStoryL) : (!playerStoryQuestSids.heaven_quest_s_id.HasValue || !MasterData.QuestStoryS.TryGetValue(playerStoryQuestSids.heaven_quest_s_id.Value, out questStoryS1) ? 1 : questStoryS1.quest_l_QuestStoryL)), true);
    }
    else if (this.scene == "quest002_19" || this.scene == "quest002_20")
      this.StartCoroutine(this.QuestExtraCheck(int.Parse(this.param), this.scene));
    else
      Singleton<NGSceneManager>.GetInstance().changeScene(this.scene, false);
  }

  public void changeScene(int L) => Quest00240723Scene.ChangeScene0024(true, L, true);

  private IEnumerator QuestExtraCheck(int param, string scene)
  {
    Startup00012ButtonManager startup00012ButtonManager = this;
    Future<WebAPI.Response.QuestProgressExtra> request = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = request.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (request.Result != null)
    {
      if (!((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>()).Select<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x.quest_extra_s.ID)).Contains<int>(param))
      {
        startup00012ButtonManager.StartCoroutine(PopupCommon.Show(Consts.GetInstance().QUEST_FILED_TITLE, Consts.GetInstance().QUEST_FILED_DISCRIPTION));
      }
      else
      {
        switch (scene)
        {
          case "quest002_19":
            Quest00219Scene.ChangeScene(param);
            break;
          case "quest002_20":
            Quest00220Scene.ChangeScene00220(param);
            break;
        }
      }
    }
  }
}
