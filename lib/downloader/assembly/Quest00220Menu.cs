// Decompiled with JetBrains decompiler
// Type: Quest00220Menu
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
[AddComponentMenu("Scenes/QuestExtra/S_Menu")]
public class Quest00220Menu : QuestStageMenuBase
{
  private bool isKeyQuest;
  public bool isSideQuest;

  protected override void Update()
  {
    if (!this.SceneStart || this.hscrollButtons == null)
      return;
    base.Update();
    this.UpdateButton();
  }

  public IEnumerator Initialize(
    PlayerExtraQuestS[] ExtraData,
    int L,
    int M,
    int S,
    bool forcus,
    bool isKeyQuest)
  {
    Quest00220Menu quest00220Menu = this;
    quest00220Menu.isKeyQuest = isKeyQuest;
    PlayerExtraQuestS[] ExtraDataS = ((IEnumerable<PlayerExtraQuestS>) ExtraData).S(L, M);
    bool? isOpen = new bool?();
    QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
    if (source != null && source.Length != 0)
    {
      QuestScoreCampaignProgress campaignProgress = ((IEnumerable<QuestScoreCampaignProgress>) source).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == L));
      if (campaignProgress != null)
        isOpen = new bool?(campaignProgress.is_open);
    }
    quest00220Menu.popSelectedSID(ref S, ref forcus);
    IEnumerator e = quest00220Menu.InitExtraQuest(ExtraDataS, L, M, S, forcus, isOpen);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Persist.integralNoaTutorial.Data.beginnersQuest)
    {
      Persist.integralNoaTutorial.Data.beginnersQuest = false;
      Persist.integralNoaTutorial.Flush();
      Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("newchapter_quest_tutorial");
    }
  }

  protected override void SetTextLimitation(int s_id)
  {
    AssocList<int, QuestExtraLimitation> questExtraLimitation = MasterData.QuestExtraLimitation;
    QuestExtraLimitationLabel[] limitationLabelList = MasterData.QuestExtraLimitationLabelList;
    Func<KeyValuePair<int, QuestExtraLimitation>, bool> predicate = (Func<KeyValuePair<int, QuestExtraLimitation>, bool>) (n => n.Value.quest_s_id_QuestExtraS == s_id);
    KeyValuePair<int, QuestExtraLimitation>[] array = questExtraLimitation.Where<KeyValuePair<int, QuestExtraLimitation>>(predicate).ToArray<KeyValuePair<int, QuestExtraLimitation>>();
    if (((IEnumerable<KeyValuePair<int, QuestExtraLimitation>>) array).Count<KeyValuePair<int, QuestExtraLimitation>>() == 0)
    {
      this.EntryInfoScript.IsDisplay = false;
    }
    else
    {
      int target_id = ((IEnumerable<KeyValuePair<int, QuestExtraLimitation>>) array).First<KeyValuePair<int, QuestExtraLimitation>>().Value.ID;
      this.EntryInfoScript.TextNormal = ((IEnumerable<QuestExtraLimitationLabel>) limitationLabelList).Where<QuestExtraLimitationLabel>((Func<QuestExtraLimitationLabel, bool>) (n => n.ID == target_id)).First<QuestExtraLimitationLabel>().label;
      this.EntryInfoScript.Normal = true;
      this.EntryInfoScript.Hurd = false;
    }
    QuestExtraS questExtraS = MasterData.QuestExtraS[s_id];
    if (questExtraS != null && questExtraS.gender_restriction != UnitGender.none)
    {
      ((Component) this.TxtGenderRestriction).gameObject.SetActive(true);
      this.TxtGenderRestriction.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_002_GENDERRESTRICTION, (IDictionary) new Hashtable()
      {
        {
          (object) "gender",
          (object) UnitGenderText.GetText(questExtraS.gender_restriction)
        }
      }));
    }
    else
      ((Component) this.TxtGenderRestriction).gameObject.SetActive(false);
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    ((Behaviour) this.btnBack).enabled = false;
    this.tweenSettingDefault();
    if (this.isKeyQuest)
    {
      Quest002171Scene.ChangeScene(false);
    }
    else
    {
      QuestConverterData questS;
      if ((questS = this.StageDataS.First<QuestConverterData>()).seek_type == QuestExtra.SeekType.L)
      {
        QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
        if (source != null)
        {
          if (((IEnumerable<QuestScoreCampaignProgress>) source).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == questS.id_L)) != null)
            Quest00226Scene.ChangeScene(questS.id_S, false);
          else
            Quest00219Scene.ChangeScene(questS.id_S, false);
        }
        else
          Quest00219Scene.ChangeScene(questS.id_S, false);
      }
      else if (questS.seek_type == QuestExtra.SeekType.M)
      {
        QuestExtraLL questLl = MasterData.QuestExtraM[questS.id_M].quest_ll;
        if (questLl == null)
        {
          if (!this.isSideQuest)
            Quest00217Scene.ChangeScene(false, questS.top_category);
          else
            Quest002SideStoryScene.ChangeScene(false, questS.top_category);
        }
        else
          Quest00218Scene.backOrChangeScene(questLl.ID, new int?(questS.id_S));
      }
    }
    this.isSideQuest = false;
  }
}
