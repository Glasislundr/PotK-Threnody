// Decompiled with JetBrains decompiler
// Type: Quest0022Menu
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
public class Quest0022Menu : QuestStageMenuBase
{
  [SerializeField]
  private GameObject Top_dir_RightFit;
  private Quest0022Bonus bonus;
  private int bonusCategory;
  private float timer = 1f;
  private DateTime? timeBonusTarget;
  private GameObject bonusTimeContainer;
  private int firstXL;
  private int firstL;
  private int firstM;
  [SerializeField]
  private bool isDebugBonusTime;
  private const int DEBUG_BONUS_CATEGORY = 2;
  private DateTime DEBUG_BONU_TIME = new DateTime(2015, 4, 10, 0, 9, 0);

  protected override void Update()
  {
    if (!this.SceneStart || this.hscrollButtons == null)
      return;
    base.Update();
    this.UpdateButton();
    if (this.bonusCategory == 0)
      return;
    this.timer -= Time.deltaTime;
    if ((double) this.timer > 0.0)
      return;
    this.StartCoroutine(this.UpdateTimeBonus());
    this.timer = 1f;
  }

  private IEnumerator UpdateTimeBonus()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTime();
    if (this.timeBonusTarget.HasValue)
      this.bonus.SetLimitTimeNumber(this.timeBonusTarget.Value, now);
    else
      this.bonusTimeContainer.SetActive(false);
  }

  public IEnumerator Initialize(
    PlayerStoryQuestS[] StoryData,
    int XL,
    int L,
    int M,
    int S,
    bool hard,
    bool bgCreate,
    bool focus)
  {
    Quest0022Menu quest0022Menu = this;
    Future<GameObject> bonusPrefab = Res.Prefabs.quest002_2.dir_Bonus.Load<GameObject>();
    IEnumerator e = bonusPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0022Menu.bonusTimeContainer = bonusPrefab.Result.Clone(quest0022Menu.Top_dir_RightFit.transform);
    PlayerStoryQuestS[] StoryDataS = StoryData.S(XL, L, M);
    quest0022Menu.bonusCategory = quest0022Menu.isDebugBonusTime ? 2 : StoryDataS[0].bonus_category;
    quest0022Menu.timeBonusTarget = quest0022Menu.isDebugBonusTime ? new DateTime?(quest0022Menu.DEBUG_BONU_TIME) : StoryDataS[0].end_at;
    quest0022Menu.bonus = quest0022Menu.bonusTimeContainer.GetComponent<Quest0022Bonus>();
    if (quest0022Menu.bonusCategory != 0)
    {
      quest0022Menu.bonusTimeContainer.SetActive(true);
      quest0022Menu.bonus.SetBonusCategory(quest0022Menu.bonusCategory);
      e = quest0022Menu.UpdateTimeBonus();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      quest0022Menu.bonusTimeContainer.SetActive(false);
    quest0022Menu.popSelectedSID(ref S, ref focus);
    e = quest0022Menu.InitStoryQuest(StoryDataS, L, M, S, hard, focus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!bgCreate)
      ((Component) quest0022Menu).GetComponent<Quest0022Scene>().bgchange.M_BGAnimation(quest0022Menu.StageDataS[0].numM_in_thisL, Hard: quest0022Menu.hardmode);
  }

  public IEnumerator Initialize(
    PlayerSeaQuestS[] StoryData,
    int XL,
    int L,
    int M,
    int S,
    bool bgCreate,
    bool focus)
  {
    Quest0022Menu quest0022Menu = this;
    quest0022Menu.firstXL = XL;
    quest0022Menu.firstL = L;
    quest0022Menu.firstM = M;
    Future<GameObject> bonusPrefab = new ResourceObject("Prefabs/quest002_2_sea/dir_Bonus_sea").Load<GameObject>();
    IEnumerator e = bonusPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0022Menu.bonusTimeContainer = bonusPrefab.Result.Clone(quest0022Menu.Top_dir_RightFit.transform);
    quest0022Menu.bonusTimeContainer.SetActive(false);
    PlayerSeaQuestS[] StoryDataS = StoryData.S(XL, L, M);
    quest0022Menu.bonusCategory = quest0022Menu.isDebugBonusTime ? 2 : StoryDataS[0].bonus_category;
    quest0022Menu.timeBonusTarget = quest0022Menu.isDebugBonusTime ? new DateTime?(quest0022Menu.DEBUG_BONU_TIME) : StoryDataS[0].end_at;
    quest0022Menu.bonus = quest0022Menu.bonusTimeContainer.GetComponent<Quest0022Bonus>();
    if (quest0022Menu.bonusCategory != 0)
    {
      quest0022Menu.bonusTimeContainer.SetActive(true);
      quest0022Menu.bonus.SetBonusCategory(quest0022Menu.bonusCategory);
      e = quest0022Menu.UpdateTimeBonus();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      quest0022Menu.bonusTimeContainer.SetActive(false);
    quest0022Menu.popSelectedSID(ref S, ref focus);
    e = quest0022Menu.InitSeaQuest(StoryDataS, L, M, S, focus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!bgCreate)
      ((Component) quest0022Menu).GetComponent<Quest0022Scene>().bgchange.M_BGAnimation(quest0022Menu.StageDataS[0].numM_in_thisL);
  }

  protected override void SetTextLimitation(int s_id)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      AssocList<int, QuestSeaLimitation> questSeaLimitation = MasterData.QuestSeaLimitation;
      QuestSeaLimitationLabel[] limitationLabelList = MasterData.QuestSeaLimitationLabelList;
      Func<KeyValuePair<int, QuestSeaLimitation>, bool> predicate = (Func<KeyValuePair<int, QuestSeaLimitation>, bool>) (n => n.Value.quest_s_id_QuestSeaS == s_id);
      KeyValuePair<int, QuestSeaLimitation>[] array = questSeaLimitation.Where<KeyValuePair<int, QuestSeaLimitation>>(predicate).ToArray<KeyValuePair<int, QuestSeaLimitation>>();
      if (((IEnumerable<KeyValuePair<int, QuestSeaLimitation>>) array).Count<KeyValuePair<int, QuestSeaLimitation>>() == 0)
      {
        this.EntryInfoScript.IsDisplay = false;
      }
      else
      {
        int target_id = ((IEnumerable<KeyValuePair<int, QuestSeaLimitation>>) array).First<KeyValuePair<int, QuestSeaLimitation>>().Value.ID;
        string str = string.Join(",", ((IEnumerable<QuestSeaLimitationLabel>) limitationLabelList).Where<QuestSeaLimitationLabel>((Func<QuestSeaLimitationLabel, bool>) (n => n.ID == target_id)).Select<QuestSeaLimitationLabel, string>((Func<QuestSeaLimitationLabel, string>) (x => x.label)).ToArray<string>());
        if (this.hardmode)
          this.EntryInfoScript.TextHurd = str;
        else
          this.EntryInfoScript.TextNormal = str;
        this.EntryInfoScript.Normal = !this.hardmode;
        this.EntryInfoScript.Hurd = this.hardmode;
      }
      QuestSeaS questSeaS = MasterData.QuestSeaS[s_id];
      if (questSeaS != null && questSeaS.gender_restriction != UnitGender.none)
      {
        ((Component) this.TxtGenderRestriction).gameObject.SetActive(true);
        this.TxtGenderRestriction.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_002_GENDERRESTRICTION, (IDictionary) new Hashtable()
        {
          {
            (object) "gender",
            (object) UnitGenderText.GetText(questSeaS.gender_restriction)
          }
        }));
      }
      else
        ((Component) this.TxtGenderRestriction).gameObject.SetActive(false);
    }
    else
    {
      AssocList<int, QuestStoryLimitation> questStoryLimitation = MasterData.QuestStoryLimitation;
      QuestStoryLimitationLabel[] limitationLabelList = MasterData.QuestStoryLimitationLabelList;
      Func<KeyValuePair<int, QuestStoryLimitation>, bool> predicate = (Func<KeyValuePair<int, QuestStoryLimitation>, bool>) (n => n.Value.quest_s_id_QuestStoryS == s_id);
      KeyValuePair<int, QuestStoryLimitation>[] array = questStoryLimitation.Where<KeyValuePair<int, QuestStoryLimitation>>(predicate).ToArray<KeyValuePair<int, QuestStoryLimitation>>();
      if (((IEnumerable<KeyValuePair<int, QuestStoryLimitation>>) array).Count<KeyValuePair<int, QuestStoryLimitation>>() == 0)
      {
        this.EntryInfoScript.IsDisplay = false;
      }
      else
      {
        int target_id = ((IEnumerable<KeyValuePair<int, QuestStoryLimitation>>) array).First<KeyValuePair<int, QuestStoryLimitation>>().Value.ID;
        string str = string.Join(",", ((IEnumerable<QuestStoryLimitationLabel>) limitationLabelList).Where<QuestStoryLimitationLabel>((Func<QuestStoryLimitationLabel, bool>) (n => n.ID == target_id)).Select<QuestStoryLimitationLabel, string>((Func<QuestStoryLimitationLabel, string>) (x => x.label)).ToArray<string>());
        if (this.hardmode)
          this.EntryInfoScript.TextHurd = str;
        else
          this.EntryInfoScript.TextNormal = str;
        this.EntryInfoScript.Normal = !this.hardmode;
        this.EntryInfoScript.Hurd = this.hardmode;
      }
      QuestStoryS questStoryS = MasterData.QuestStoryS[s_id];
      if (questStoryS != null && questStoryS.gender_restriction != UnitGender.none)
      {
        ((Component) this.TxtGenderRestriction).gameObject.SetActive(true);
        this.TxtGenderRestriction.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_002_GENDERRESTRICTION, (IDictionary) new Hashtable()
        {
          {
            (object) "gender",
            (object) UnitGenderText.GetText(questStoryS.gender_restriction)
          }
        }));
      }
      else
        ((Component) this.TxtGenderRestriction).gameObject.SetActive(false);
    }
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    ((Behaviour) this.btnBack).enabled = false;
    this.tweenSettingDefault();
    BGChange component = ((Component) this).GetComponent<BGChange>();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      if (Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack("sea030_quest"))
        this.backScene();
      else
        Sea030_questScene.ChangeScene(false, this.firstXL, this.firstL, this.firstM);
    }
    else
    {
      component.M_BGAnimation(10, Hard: this.hardmode);
      Quest00240723Scene.ChangeScene0024(false, this.PassData, false, false);
    }
  }

  private void OnDisable() => Object.Destroy((Object) this.bonusTimeContainer);

  public override void onEndScene() => base.onEndScene();
}
