// Decompiled with JetBrains decompiler
// Type: Versus02612ScrollRewardBox
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
public class Versus02612ScrollRewardBox : MonoBehaviour
{
  [SerializeField]
  private int betweenMargin;
  [SerializeField]
  private int belowMargin;
  [SerializeField]
  private Transform[] targetParents;
  [SerializeField]
  private Transform dirTarget;
  [SerializeField]
  private UISprite slcState;
  [SerializeField]
  private UILabel slcStateDescription;
  [SerializeField]
  private GameObject slcGotReward;
  [SerializeField]
  private GameObject slcNotGotReward;
  [SerializeField]
  private GameObject slcAcquired;
  [SerializeField]
  private GameObject slcRankTitleBase;
  [SerializeField]
  private UILabel textRankTitle;
  private GameObject emblemPrefab;
  private GameObject rewardPrefab;
  private static readonly string[] spriteName = new string[5]
  {
    "slc_ClassUp.png__GUI__026_12_sozai__026_12_sozai_prefab",
    "slc_ClassStayed.png__GUI__026_12_sozai__026_12_sozai_prefab",
    "slc_ClassDown.png__GUI__026_12_sozai__026_12_sozai_prefab",
    "slc_ClassTitle.png__GUI__026_12_sozai__026_12_sozai_prefab",
    "slc_ClassNew.png__GUI__026_12_sozai__026_12_sozai_prefab"
  };

  public IEnumerator Init(
    IEnumerable<WebAPI.Response.PvpSeasonCloseClass_rewards> data)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach<WebAPI.Response.PvpSeasonCloseClass_rewards>((Action<WebAPI.Response.PvpSeasonCloseClass_rewards>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = x.show_text
    })));
    string spriteName = Versus02612ScrollRewardBox.spriteName[data.First<WebAPI.Response.PvpSeasonCloseClass_rewards>().class_reward_type - 1];
    IEnumerator e = this.Init(setData, spriteName);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(List<PvpClassReward> data, bool reached_class)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<PvpClassReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = x.reward_type,
      txt = x.reward_message
    })));
    bool? gotReward = new bool?();
    if (data[0].class_reward_type == PvpClassRewardTypeEnum.first_promotion)
      gotReward = new bool?(reached_class);
    string spriteName = Versus02612ScrollRewardBox.spriteName[(int) (data[0].class_reward_type - 1)];
    IEnumerator e = this.Init(setData, spriteName, gotReward: gotReward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(
    IEnumerable<WebAPI.Response.PvpRankingCloseRanking_rewards> data)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach<WebAPI.Response.PvpRankingCloseRanking_rewards>((Action<WebAPI.Response.PvpRankingCloseRanking_rewards>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = x.show_text
    })));
    PvpRankingCondition rankingCondition = MasterData.PvpRankingCondition[data.First<WebAPI.Response.PvpRankingCloseRanking_rewards>().condition_id];
    IEnumerator e = this.Init(setData, rankingCondition.image_name, rankingCondition.disp_text);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(WebAPI.Response.PvpBootCampaign_rewards data)
  {
    yield return (object) this.Init(new List<Versus02612ScrollRewardBox.RewardData>()
    {
      new Versus02612ScrollRewardBox.RewardData()
      {
        rewardID = data.reward_id,
        type = (MasterDataTable.CommonRewardType) data.reward_type_id,
        txt = data.reward_title
      }
    }, "slc_Normal.png__GUI__026_12_sozai__026_12_sozai_prefab", data.show_title, new bool?(data.is_received));
  }

  public IEnumerator Init(
    IEnumerable<PvpClassRankingReward> data,
    int ranking = 0,
    bool is_belong = false,
    bool is_period = true)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach<PvpClassRankingReward>((Action<PvpClassRankingReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = x.reward_type,
      txt = x.reward_message
    })));
    PvpRankingCondition rankingCategory = data.First<PvpClassRankingReward>().ranking_category;
    IEnumerator e = this.Init(setData, rankingCategory.image_name, rankingCategory.disp_text);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.slcRankTitleBase, (Object) null) & is_belong)
    {
      this.slcRankTitleBase.gameObject.SetActive(true);
      this.slcRankTitleBase.transform.GetChildInFind("txt_Term");
      if (!is_period)
      {
        this.textRankTitle.SetText(string.Format(Consts.GetInstance().PVP_CLASS_MATCH_REWARD_RANK_BELONG, (object) ranking));
        if (Object.op_Inequality((Object) this.slcAcquired, (Object) null))
          this.slcAcquired.SetActive(true);
      }
    }
  }

  public IEnumerator Init(List<QuestScoreRankingReward> data, int scoreCampaignID)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<QuestScoreRankingReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)
    })));
    KeyValuePair<int, QuestExtraScoreRankingReward> keyValuePair = MasterData.QuestExtraScoreRankingReward.Where<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (x => x.Value.campaign_id == scoreCampaignID)).FirstOrDefault<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (x => x.Value.group_id == data[0].ranking_group_id));
    IEnumerator e = this.Init(setData, keyValuePair.Value.image_name, keyValuePair.Value.display_text);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(List<QuestScoreAchivementReward> data, int[] achivement_cleard)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<QuestScoreAchivementReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)
    })));
    KeyValuePair<int, QuestExtraScoreAchivementReward> keyValuePair = MasterData.QuestExtraScoreAchivementReward.FirstOrDefault<KeyValuePair<int, QuestExtraScoreAchivementReward>>((Func<KeyValuePair<int, QuestExtraScoreAchivementReward>, bool>) (x => x.Value.ID == data[0].id));
    bool? gotReward = new bool?();
    gotReward = new bool?(((IEnumerable<int>) achivement_cleard).Contains<int>(keyValuePair.Value.ID));
    IEnumerator e = this.Init(setData, keyValuePair.Value.image_name, keyValuePair.Value.display_text, gotReward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(List<WebAPI.Response.QuestscoreRewardRewards> data, int scoreCampaignID)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<WebAPI.Response.QuestscoreRewardRewards>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id.Value,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id.Value, x.reward_quantity)
    })));
    KeyValuePair<int, QuestExtraScoreRankingReward> keyValuePair = MasterData.QuestExtraScoreRankingReward.Where<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (x => x.Value.campaign_id == scoreCampaignID)).FirstOrDefault<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (x => x.Value.group_id == data[0].ranking_group_id));
    IEnumerator e = this.Init(setData, keyValuePair.Value.image_name, keyValuePair.Value.display_text);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(List<PunitiveExpeditionEventReward> data, bool all, bool isClear)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<PunitiveExpeditionEventReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = x.reward_type_id,
      txt = CommonRewardType.GetRewardName(x.reward_type_id, x.reward_id, x.reward_quantity, x.is_guild_reward),
      isGuildReward = x.is_guild_reward
    })));
    string empty = string.Empty;
    string spriteDescription;
    if (all)
      spriteDescription = Consts.Format(Consts.GetInstance().QUEST_00231_ALL_LIST_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "personal",
          (object) data[0].display_text1
        },
        {
          (object) nameof (all),
          (object) data[0].display_text2
        }
      });
    else
      spriteDescription = Consts.Format(Consts.GetInstance().QUEST_00231_ALL_PERSONAL_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "personal",
          (object) data[0].display_text1
        }
      });
    IEnumerator e = this.Init(setData, data[0].image_name, spriteDescription, new bool?(isClear));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(List<GvgWholeRewardMaster> data)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach((Action<GvgWholeRewardMaster>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = x.reward_type,
      txt = CommonRewardType.GetRewardName(x.reward_type, x.reward_id, x.reward_quantity)
    })));
    IEnumerator e = this.Init(setData, "slc_Normal.png__GUI__026_12_sozai__026_12_sozai_prefab", data[0].reward_title);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitRaid(
    List<Versus02612ScrollRewardBox.RewardData> rewardData,
    int rankind_condition_id)
  {
    GuildRaidRankingRewardCondition rankingRewardCondition = MasterData.GuildRaidRankingRewardCondition[rankind_condition_id];
    IEnumerator e = this.Init(rewardData, rankingRewardCondition.image_name, rankingRewardCondition.display_text);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitExploreRanking(
    List<Versus02612ScrollRewardBox.RewardData> rewardData,
    int rankind_condition_id)
  {
    ExploreRankingCondition rankingCondition = MasterData.ExploreRankingCondition[rankind_condition_id];
    IEnumerator e = this.Init(rewardData, rankingCondition.image_name, rankingCondition.display_text, isUnitRarityCenter: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitExploreFloor(
    List<Versus02612ScrollRewardBox.RewardData> rewardData,
    int floor,
    int floor_arrival)
  {
    string spriteName = "slc_Normal.png__GUI__026_12_sozai__026_12_sozai_prefab";
    string spriteDescription = Consts.GetInstance().EXPLORE_FLOOR_REWARD_TITLE_FORMAT.F((object) floor);
    bool flag = floor_arrival >= floor;
    IEnumerator e = this.Init(rewardData, spriteName, spriteDescription, new bool?(flag), true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitCorpsStage(int stageId, bool gotten)
  {
    CorpsStageClearReward[] array = ((IEnumerable<CorpsStageClearReward>) MasterData.CorpsStageClearRewardList).Where<CorpsStageClearReward>((Func<CorpsStageClearReward, bool>) (x => x.stage_id == stageId)).ToArray<CorpsStageClearReward>();
    List<Versus02612ScrollRewardBox.RewardData> data = new List<Versus02612ScrollRewardBox.RewardData>();
    foreach (CorpsStageClearReward stageClearReward in array)
      data.Add(new Versus02612ScrollRewardBox.RewardData()
      {
        rewardID = stageClearReward.reward_id,
        type = stageClearReward.entity_type,
        txt = stageClearReward.reward_name
      });
    string spriteName = "slc_Normal.png__GUI__026_12_sozai__026_12_sozai_prefab";
    string empty = string.Empty;
    IEnumerator e = this.Init(data, spriteName, empty, new bool?(gotten), true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(IEnumerable<QuestScoreTotalReward> data, int total_score)
  {
    List<Versus02612ScrollRewardBox.RewardData> setData = new List<Versus02612ScrollRewardBox.RewardData>();
    data.ForEach<QuestScoreTotalReward>((Action<QuestScoreTotalReward>) (x => setData.Add(new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = x.reward_id,
      type = (MasterDataTable.CommonRewardType) x.reward_type_id,
      txt = CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)
    })));
    bool? gotReward = new bool?();
    QuestScoreTotalReward firstData = data.First<QuestScoreTotalReward>();
    gotReward = new bool?(total_score >= firstData.score_needed);
    KeyValuePair<int, QuestExtraTotalScoreReward> keyValuePair = MasterData.QuestExtraTotalScoreReward.FirstOrDefault<KeyValuePair<int, QuestExtraTotalScoreReward>>((Func<KeyValuePair<int, QuestExtraTotalScoreReward>, bool>) (x => x.Value.ID == firstData.id));
    IEnumerator e = this.Init(setData, keyValuePair.Value.image_name, keyValuePair.Value.display_text, gotReward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator Init(
    List<Versus02612ScrollRewardBox.RewardData> data,
    string spriteName,
    string spriteDescription = "",
    bool? gotReward = null,
    bool isUnitRarityCenter = false)
  {
    Versus02612ScrollRewardBox versus02612ScrollRewardBox = this;
    versus02612ScrollRewardBox.slcState.spriteName = spriteName;
    if (!string.IsNullOrEmpty(spriteDescription))
    {
      ((Component) versus02612ScrollRewardBox.slcStateDescription).gameObject.SetActive(true);
      versus02612ScrollRewardBox.slcStateDescription.SetText(spriteDescription);
    }
    if (Object.op_Inequality((Object) versus02612ScrollRewardBox.slcGotReward, (Object) null))
      versus02612ScrollRewardBox.slcGotReward.SetActive(gotReward.HasValue && gotReward.Value);
    if (Object.op_Inequality((Object) versus02612ScrollRewardBox.slcNotGotReward, (Object) null))
      versus02612ScrollRewardBox.slcNotGotReward.SetActive(gotReward.HasValue && !gotReward.Value);
    Future<GameObject> rewardPrefabF = Res.Prefabs.versus026_12.dir_Reward_Item.Load<GameObject>();
    IEnumerator e = rewardPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02612ScrollRewardBox.rewardPrefab = rewardPrefabF.Result;
    Future<GameObject> emblemPrefabF = Res.Prefabs.versus026_12.dir_Reward_Title.Load<GameObject>();
    e = emblemPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02612ScrollRewardBox.emblemPrefab = emblemPrefabF.Result;
    int yPos = 0;
    foreach (var data1 in data.Select((n, i) => new
    {
      n = n,
      i = i
    }))
    {
      var d = data1;
      if (d.i < versus02612ScrollRewardBox.targetParents.Length)
      {
        GameObject obj = (GameObject) null;
        if (d.n.type == MasterDataTable.CommonRewardType.emblem)
        {
          obj = versus02612ScrollRewardBox.emblemPrefab.Clone(versus02612ScrollRewardBox.targetParents[d.i]);
          e = obj.GetComponent<Versus02612RewardTitle>().Init(d.n.rewardID, d.n.isGuildReward);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          bool isLineObj = d.i != data.Count - 1;
          obj = versus02612ScrollRewardBox.rewardPrefab.Clone(versus02612ScrollRewardBox.targetParents[d.i]);
          e = obj.GetComponent<Versus02612ScrollRewardItem>().CreateItem(d.n.type, d.n.rewardID, d.n.txt, isLineObj, isUnitRarityCenter);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        versus02612ScrollRewardBox.targetParents[d.i].localPosition = new Vector3(0.0f, (float) -yPos, 0.0f);
        yPos += obj.GetComponent<UIWidget>().height + versus02612ScrollRewardBox.betweenMargin;
        obj = (GameObject) null;
        d = null;
      }
      else
        break;
    }
    UISprite component1 = ((Component) versus02612ScrollRewardBox).GetComponent<UISprite>();
    BoxCollider component2 = ((Component) versus02612ScrollRewardBox).GetComponent<BoxCollider>();
    ((UIWidget) component1).height = yPos - versus02612ScrollRewardBox.betweenMargin + versus02612ScrollRewardBox.belowMargin - (int) versus02612ScrollRewardBox.dirTarget.localPosition.y;
    component2.size = new Vector3((float) ((UIWidget) component1).width, (float) ((UIWidget) component1).height);
    component2.center = new Vector3(0.0f, (float) (-((UIWidget) component1).height / 2));
  }

  public class RewardData
  {
    public int rewardID;
    public MasterDataTable.CommonRewardType type;
    public string txt;
    public bool isGuildReward;
  }
}
