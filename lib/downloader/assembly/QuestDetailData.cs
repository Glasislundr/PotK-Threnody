// Decompiled with JetBrains decompiler
// Type: QuestDetailData
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
public class QuestDetailData
{
  private int[] stages_;
  private bool isWait_;
  private bool isWaitInitEnemyInfo_ = true;

  public bool isWave { get; private set; }

  public CommonQuestType type { get; private set; }

  public int ID { get; private set; }

  public string name { get; private set; }

  public string recommend_strength { get; private set; }

  public GearKindEnum[] kinds { get; private set; }

  public CommonElement[] elements { get; private set; }

  public BattleskillSkill[] ailments { get; private set; }

  public bool isDisplayDrops { get; private set; }

  public QuestDetailData.Drop[] drops { get; private set; }

  public bool isValidate { get; private set; }

  public QuestDetailData(CommonQuestType questType, int id, bool bwave)
  {
    this.isWave = bwave;
    this.type = questType;
    this.ID = id;
    this.isValidate = false;
    this.isWait_ = false;
  }

  public IEnumerator Wait(MonoBehaviour mon, Action<WebAPI.Response.UserError> eventError)
  {
    if (!this.isValidate)
    {
      while (this.isWait_)
        yield return (object) null;
      this.isWait_ = true;
      mon.StartCoroutine(this.initEnemyInfo());
      IEnumerator e;
      if (this.isWave)
      {
        Future<WebAPI.Response.BattleWaveQuestDetail> future = WebAPI.BattleWaveQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
        e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        while (this.isWaitInitEnemyInfo_)
          yield return (object) null;
        WebAPI.Response.BattleWaveQuestDetail result = future.Result;
        if (result == null)
        {
          yield break;
        }
        else
        {
          this.initialize(result);
          future = (Future<WebAPI.Response.BattleWaveQuestDetail>) null;
        }
      }
      else
      {
        switch (this.type)
        {
          case CommonQuestType.Story:
            Future<WebAPI.Response.BattleStoryQuestDetail> future1 = WebAPI.BattleStoryQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
            e = future1.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            while (this.isWaitInitEnemyInfo_)
              yield return (object) null;
            WebAPI.Response.BattleStoryQuestDetail result1 = future1.Result;
            if (result1 == null)
            {
              yield break;
            }
            else
            {
              this.initialize(result1);
              future1 = (Future<WebAPI.Response.BattleStoryQuestDetail>) null;
              break;
            }
          case CommonQuestType.Character:
            Future<WebAPI.Response.BattleCharacterQuestDetail> future2 = WebAPI.BattleCharacterQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
            e = future2.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            while (this.isWaitInitEnemyInfo_)
              yield return (object) null;
            WebAPI.Response.BattleCharacterQuestDetail result2 = future2.Result;
            if (result2 == null)
            {
              yield break;
            }
            else
            {
              this.initialize(result2);
              future2 = (Future<WebAPI.Response.BattleCharacterQuestDetail>) null;
              break;
            }
          case CommonQuestType.Extra:
            Future<WebAPI.Response.BattleExtraQuestDetail> future3 = WebAPI.BattleExtraQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
            e = future3.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            while (this.isWaitInitEnemyInfo_)
              yield return (object) null;
            WebAPI.Response.BattleExtraQuestDetail result3 = future3.Result;
            if (result3 == null)
            {
              yield break;
            }
            else
            {
              this.initialize(result3);
              future3 = (Future<WebAPI.Response.BattleExtraQuestDetail>) null;
              break;
            }
          case CommonQuestType.Harmony:
            Future<WebAPI.Response.BattleHarmonyQuestDetail> future4 = WebAPI.BattleHarmonyQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
            e = future4.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            while (this.isWaitInitEnemyInfo_)
              yield return (object) null;
            WebAPI.Response.BattleHarmonyQuestDetail result4 = future4.Result;
            if (result4 == null)
            {
              yield break;
            }
            else
            {
              this.initialize(result4);
              future4 = (Future<WebAPI.Response.BattleHarmonyQuestDetail>) null;
              break;
            }
          case CommonQuestType.Sea:
            Future<WebAPI.Response.SeaBattleQuestDetail> future5 = WebAPI.SeaBattleQuestDetail(this.ID, (Action<WebAPI.Response.UserError>) (err => this.onWebAPIError(eventError, err)));
            e = future5.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            while (this.isWaitInitEnemyInfo_)
              yield return (object) null;
            WebAPI.Response.SeaBattleQuestDetail result5 = future5.Result;
            if (result5 == null)
            {
              yield break;
            }
            else
            {
              this.initialize(result5);
              future5 = (Future<WebAPI.Response.SeaBattleQuestDetail>) null;
              break;
            }
          default:
            Debug.LogError((object) string.Format("Not Support QuestDetail:{0}", (object) this.type.ToString()));
            this.isWait_ = false;
            yield break;
        }
      }
      Singleton<NGGameDataManager>.GetInstance().setRecommendStrength(this.type, this.ID, this.recommend_strength);
      this.isWait_ = false;
      this.isValidate = true;
    }
  }

  private IEnumerator initEnemyInfo()
  {
    QuestDetailData questDetailData = this;
    questDetailData.isWaitInitEnemyInfo_ = true;
    questDetailData.stages_ = (int[]) null;
    switch (questDetailData.type)
    {
      case CommonQuestType.Story:
        QuestStoryS questStoryS;
        if (MasterData.QuestStoryS.TryGetValue(questDetailData.ID, out questStoryS))
        {
          questDetailData.stages_ = new int[1]
          {
            questStoryS.stage_BattleStage
          };
          break;
        }
        break;
      case CommonQuestType.Character:
        QuestCharacterS questCharacterS;
        if (MasterData.QuestCharacterS.TryGetValue(questDetailData.ID, out questCharacterS))
        {
          questDetailData.stages_ = new int[1]
          {
            questCharacterS.stage_BattleStage
          };
          break;
        }
        break;
      case CommonQuestType.Extra:
        QuestExtraS questExtraS;
        if (MasterData.QuestExtraS.TryGetValue(questDetailData.ID, out questExtraS))
        {
          questDetailData.stages_ = new int[1]
          {
            questExtraS.stage_BattleStage
          };
          break;
        }
        break;
      case CommonQuestType.Harmony:
        QuestHarmonyS questHarmonyS;
        if (MasterData.QuestHarmonyS.TryGetValue(questDetailData.ID, out questHarmonyS))
        {
          questDetailData.stages_ = new int[1]
          {
            questHarmonyS.stage_BattleStage
          };
          break;
        }
        break;
      case CommonQuestType.Sea:
        QuestSeaS questSeaS;
        if (MasterData.QuestSeaS.TryGetValue(questDetailData.ID, out questSeaS))
        {
          questDetailData.stages_ = new int[1]
          {
            questSeaS.stage_BattleStage
          };
          break;
        }
        break;
    }
    if (questDetailData.stages_ != null)
      yield return (object) questDetailData.initEnemyInfo(questDetailData.stages_);
    questDetailData.isWaitInitEnemyInfo_ = false;
  }

  private IEnumerator initEnemyInfo(int[] stages)
  {
    for (int n = 0; n < stages.Length; ++n)
      yield return (object) MasterData.LoadBattleStageEnemy(MasterData.BattleStage[stages[n]]);
    if (MasterData.BattleStageEnemyList == null)
    {
      this.kinds = new GearKindEnum[0];
      this.elements = new CommonElement[0];
      this.ailments = new BattleskillSkill[0];
    }
    else
    {
      PlayerUnit[] array = ((IEnumerable<BattleStageEnemy>) ((IEnumerable<BattleStageEnemy>) MasterData.BattleStageEnemyList).Where<BattleStageEnemy>((Func<BattleStageEnemy, bool>) (x => ((IEnumerable<int>) stages).Contains<int>(x.stage_BattleStage))).ToArray<BattleStageEnemy>()).Select<BattleStageEnemy, PlayerUnit>((Func<BattleStageEnemy, PlayerUnit>) (x => PlayerUnit.FromEnemy(x))).ToArray<PlayerUnit>();
      this.kinds = ((IEnumerable<PlayerUnit>) array).Select<PlayerUnit, GearKindEnum>((Func<PlayerUnit, GearKindEnum>) (x => (GearKindEnum) x.unit.kind_GearKind)).Distinct<GearKindEnum>().OrderBy<GearKindEnum, int>((Func<GearKindEnum, int>) (y => (int) y)).ToArray<GearKindEnum>();
      this.elements = ((IEnumerable<PlayerUnit>) array).Select<PlayerUnit, CommonElement>((Func<PlayerUnit, CommonElement>) (x => x.GetElement())).Distinct<CommonElement>().OrderBy<CommonElement, int>((Func<CommonElement, int>) (y => (int) y)).ToArray<CommonElement>();
      List<BattleskillSkill> skills = new List<BattleskillSkill>();
      for (int index = 0; index < array.Length; ++index)
      {
        PlayerUnit playerUnit = array[index];
        if (playerUnit.leader_skill != null)
          skills.Add(playerUnit.leader_skill.skill);
        skills.AddRange(((IEnumerable<PlayerUnitSkills>) playerUnit.skills).Select<PlayerUnitSkills, BattleskillSkill>((Func<PlayerUnitSkills, BattleskillSkill>) (x => x.skill)));
        PlayerItem equippedGear = playerUnit.equippedGear;
        if (equippedGear != (PlayerItem) null && playerUnit.gear_proficiencies != null)
        {
          int lv = ((IEnumerable<PlayerUnitGearProficiency>) playerUnit.gear_proficiencies).First<PlayerUnitGearProficiency>().level;
          skills.AddRange(((IEnumerable<GearGearSkill>) equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.release_rank <= lv)).Select<GearGearSkill, BattleskillSkill>((Func<GearGearSkill, BattleskillSkill>) (y => y.skill)));
        }
      }
      this.ailments = this.getAilments(skills);
      MasterDataCache.Unload("BattleStageEnemy");
    }
  }

  private void onWebAPIError(
    Action<WebAPI.Response.UserError> eventError,
    WebAPI.Response.UserError errState)
  {
    this.isWait_ = false;
    if (eventError == null)
      return;
    eventError(errState);
  }

  private void initialize(WebAPI.Response.BattleStoryQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.BattleStoryQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.BattleStoryQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.BattleStoryQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private void initialize(WebAPI.Response.BattleWaveQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.BattleWaveQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.BattleWaveQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.BattleWaveQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private void initialize(WebAPI.Response.BattleExtraQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.BattleExtraQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.BattleExtraQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.BattleExtraQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private void initialize(WebAPI.Response.BattleCharacterQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.BattleCharacterQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.BattleCharacterQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.BattleCharacterQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private void initialize(WebAPI.Response.BattleHarmonyQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.BattleHarmonyQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.BattleHarmonyQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.BattleHarmonyQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private void initialize(WebAPI.Response.SeaBattleQuestDetail data)
  {
    this.name = data.quest_name;
    this.recommend_strength = data.recommend_strength;
    this.isDisplayDrops = data.drop_info_display_flag;
    this.drops = ((IEnumerable<WebAPI.Response.SeaBattleQuestDetailDrop_items>) data.drop_items).Select<WebAPI.Response.SeaBattleQuestDetailDrop_items, QuestDetailData.Drop>((Func<WebAPI.Response.SeaBattleQuestDetailDrop_items, QuestDetailData.Drop>) (di => new QuestDetailData.Drop(di.reward_id, di.reward_type_id, di.reward_quantity))).ToArray<QuestDetailData.Drop>();
  }

  private List<CommonElement> getElements(List<BattleskillSkill> skills)
  {
    List<CommonElement> list = skills.Where<BattleskillSkill>((Func<BattleskillSkill, bool>) (s => ((IEnumerable<BattleskillEffect>) s.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element)))).Select<BattleskillSkill, CommonElement>((Func<BattleskillSkill, CommonElement>) (ss => ss.element)).Distinct<CommonElement>().ToList<CommonElement>();
    if (!list.Any<CommonElement>())
      list.Add(CommonElement.none);
    return list;
  }

  private BattleskillSkill[] getAilments(List<BattleskillSkill> skills)
  {
    List<BattleskillSkill> list = skills.Distinct<BattleskillSkill>((IEqualityComparer<BattleskillSkill>) new LambdaEqualityComparer<BattleskillSkill>((Func<BattleskillSkill, BattleskillSkill, bool>) ((a, b) => a.ID == b.ID))).ToList<BattleskillSkill>();
    List<BattleskillSkill> source = new List<BattleskillSkill>();
    foreach (BattleskillSkill battleskillSkill in list)
    {
      if (((IEnumerable<int>) battleskillSkill.InvestSkillIds()).Where<int>((Func<int, bool>) (i => MasterData.BattleskillSkill.ContainsKey(i))).Select<int, BattleskillSkill>((Func<int, BattleskillSkill>) (i => MasterData.BattleskillSkill[i])).Where<BattleskillSkill>((Func<BattleskillSkill, bool>) (s => s.skill_type == BattleskillSkillType.ailment)).Any<BattleskillSkill>())
        source.Add(battleskillSkill);
    }
    return source.OrderBy<BattleskillSkill, int>((Func<BattleskillSkill, int>) (s => s.ID)).ToArray<BattleskillSkill>();
  }

  public class Drop
  {
    public int rewardId { get; private set; }

    public MasterDataTable.CommonRewardType rewardType { get; private set; }

    public int quantity { get; private set; }

    public Drop(int id, int type, int num)
    {
      this.rewardId = id;
      this.rewardType = (MasterDataTable.CommonRewardType) type;
      this.quantity = num;
    }
  }
}
