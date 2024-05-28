// Decompiled with JetBrains decompiler
// Type: Quest00226Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest00226Menu : Quest00219Menu
{
  [SerializeField]
  private UILabel txtEventTotalPt;
  [SerializeField]
  private UILabel txtEventRank;
  [SerializeField]
  private UILabel txtTotalScorePt;
  [SerializeField]
  private UILabel txtEventPeriod;
  [SerializeField]
  private GameObject objEffectParent;
  [SerializeField]
  private GameObject dir_EventButton;
  [SerializeField]
  private GameObject dir_EventButton_without_ranking;
  [SerializeField]
  private GameObject dir_RankingInfo;
  [SerializeField]
  private GameObject dir_TotalScoreInfo;
  [SerializeField]
  private GameObject dir_TotalScoreInfoBack;
  protected QuestScoreCampaignProgress questScoreCampaignProgress;

  public Transform GetEffectParent => this.objEffectParent.transform;

  public IEnumerator Init(
    bool AlreadyReceived,
    PlayerExtraQuestS[] ExtraData,
    WebAPI.Response.QuestscoreRewardRewards[] rewards,
    int lid,
    QuestScoreCampaignProgress qscp)
  {
    if (qscp == null)
    {
      Quest00217Scene.ChangeScene(false);
    }
    else
    {
      this.questScoreCampaignProgress = qscp;
      IEnumerator e = this.Init(Res.Prefabs.quest002_26.list.Load<GameObject>(), Res.Prefabs.quest002_26.list.Load<GameObject>(), AlreadyReceived, ExtraData, rewards, lid);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.txtEventTotalPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) qscp.player.score_total
        }
      }));
      this.txtTotalScorePt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) qscp.player.score_total
        }
      }));
      this.txtEventRank.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_RANK, (IDictionary) new Hashtable()
      {
        {
          (object) "rank",
          (object) qscp.player.rank
        }
      }));
      string str1 = string.Format("{0:00}", (object) qscp.start_at.Month);
      string str2 = string.Format("{0:00}", (object) qscp.start_at.Day);
      string str3 = string.Format("{0:00}", (object) qscp.start_at.Hour);
      string str4 = string.Format("{0:00}", (object) qscp.start_at.Minute);
      string str5 = string.Format("{0:00}", (object) qscp.end_at.Month);
      string str6 = string.Format("{0:00}", (object) qscp.end_at.Day);
      string str7 = string.Format("{0:00}", (object) qscp.end_at.Hour);
      string str8 = string.Format("{0:00}", (object) qscp.end_at.Minute);
      this.txtEventPeriod.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00226_PERIOSD, (IDictionary) new Hashtable()
      {
        {
          (object) "start_mounth",
          (object) str1
        },
        {
          (object) "start_day",
          (object) str2
        },
        {
          (object) "start_hour",
          (object) str3
        },
        {
          (object) "start_minue",
          (object) str4
        },
        {
          (object) "end_mounth",
          (object) str5
        },
        {
          (object) "end_day",
          (object) str6
        },
        {
          (object) "end_hour",
          (object) str7
        },
        {
          (object) "end_minue",
          (object) str8
        }
      }));
      if (!this.questScoreCampaignProgress.score_ranking_disabled)
      {
        this.dir_EventButton.SetActive(true);
        this.dir_EventButton_without_ranking.SetActive(false);
        this.dir_RankingInfo.SetActive(true);
        this.dir_TotalScoreInfo.SetActive(false);
      }
      else if (this.questScoreCampaignProgress.total_reward_exists)
      {
        this.dir_EventButton.SetActive(false);
        this.dir_EventButton_without_ranking.SetActive(true);
        this.dir_RankingInfo.SetActive(false);
        this.dir_TotalScoreInfo.SetActive(true);
      }
      else
      {
        this.dir_EventButton.SetActive(false);
        this.dir_EventButton_without_ranking.SetActive(true);
        this.dir_RankingInfo.SetActive(false);
        this.dir_TotalScoreInfo.SetActive(false);
      }
      this.dir_TotalScoreInfoBack.SetActive(!this.dir_TotalScoreInfo.activeSelf);
    }
  }

  public IEnumerator Init(
    Future<GameObject> ListPrefab,
    Future<GameObject> ScrollPrefab,
    bool AlreadyReceived,
    PlayerExtraQuestS[] ExtraData,
    WebAPI.Response.QuestscoreRewardRewards[] rewards,
    int lid)
  {
    Quest00226Menu menu = this;
    ((Component) menu.grid).transform.Clear();
    PlayerExtraQuestS[] list = ((IEnumerable<PlayerExtraQuestS>) ExtraData).M(lid);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.serverTime = ServerTime.NowAppTime();
    Future<Texture2D> futureEvent = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(menu.LoadSpriteEvent(list[0].quest_extra_s.quest_l.banner_image_id.GetValueOrDefault(list[0].quest_extra_s.quest_l_QuestExtraL)));
    e = futureEvent.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = futureEvent.Result;
    Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
    ((Object) sprite).name = ((Object) result).name;
    menu.EventSprite.sprite2D = sprite;
    e = ListPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject listPrefab = ListPrefab.Result;
    e = ScrollPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject scrollPrefab = ScrollPrefab.Result;
    bool isBtnImage = false;
    PlayerExtraQuestS playerExtraQuestS1 = list[0];
    menu.grid.cellHeight = 114f;
    if (BannerBase.PathExist(playerExtraQuestS1.quest_extra_s, playerExtraQuestS1.quest_extra_s.quest_m.ID, QuestExtra.SeekType.M))
    {
      isBtnImage = true;
      menu.grid.cellHeight = 168f;
    }
    QuestScoreCampaignProgress[] ScoreCampaingProgress = SMManager.Get<QuestScoreCampaignProgress[]>();
    PlayerExtraQuestS[] playerExtraQuestSArray = list;
    for (int index = 0; index < playerExtraQuestSArray.Length; ++index)
    {
      PlayerExtraQuestS extra = playerExtraQuestSArray[index];
      bool isClear = true;
      bool isNew = true;
      int score = 0;
      foreach (PlayerExtraQuestS playerExtraQuestS2 in ((IEnumerable<PlayerExtraQuestS>) ExtraData).S(extra.quest_extra_s.quest_l_QuestExtraL, extra.quest_extra_s.quest_m_QuestExtraM))
      {
        if (!playerExtraQuestS2.is_clear)
          isClear = false;
        if (!playerExtraQuestS2.is_new)
          isNew = false;
      }
      QuestScoreCampaignProgress campaignProgress = ((IEnumerable<QuestScoreCampaignProgress>) ScoreCampaingProgress).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == extra.quest_extra_s.quest_l_QuestExtraL));
      if (campaignProgress != null)
        score = campaignProgress.GetQuestMScoreFromMID(extra.quest_extra_s.quest_m_QuestExtraM);
      if (isBtnImage)
      {
        e = menu.ScrollInit(extra, scrollPrefab, isClear, isNew, score);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = menu.ListInit(extra, listPrefab, isClear, isNew, score);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    playerExtraQuestSArray = (PlayerExtraQuestS[]) null;
    menu.grid.Reposition();
    menu.scrollview.ResetPosition();
    if (!AlreadyReceived && menu.questScoreCampaignProgress.player.score_total > 0)
    {
      Future<GameObject> rewardPopupF = Res.Prefabs.popup.popup_002_26_1__anim_popup01.Load<GameObject>();
      e = rewardPopupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject rewardPopup = rewardPopupF.Result.Clone(menu.objEffectParent.transform);
      e = rewardPopup.GetComponent<Popup002261Menu>().Init(menu.questScoreCampaignProgress, rewards, menu, (Action) (() => Object.DestroyObject((Object) rewardPopup.gameObject)));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      rewardPopupF = (Future<GameObject>) null;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator ScrollInit(
    PlayerExtraQuestS extra,
    GameObject prefab,
    bool isClear,
    bool isNew,
    int score)
  {
    IEnumerator e = this.CellInit(extra, prefab, isClear, isNew, score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator ListInit(
    PlayerExtraQuestS extra,
    GameObject prefab,
    bool isClear,
    bool isNew,
    int score)
  {
    IEnumerator e = this.CellInit(extra, prefab, isClear, isNew, score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CellInit(
    PlayerExtraQuestS extra,
    GameObject prefab,
    bool isClear,
    bool isNew,
    int score)
  {
    Quest00226Menu quest00226Menu = this;
    GameObject list = Object.Instantiate<GameObject>(prefab);
    list.transform.parent = ((Component) quest00226Menu.grid).transform;
    list.transform.localScale = Vector3.one;
    list.transform.localPosition = Vector3.zero;
    Quest00226List quest00226List = list.GetComponent<Quest00226List>();
    IEnumerator e = quest00226List.Init(extra, isClear, isNew, score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Set(quest00226List.Dock.onClick, (EventDelegate.Callback) (() => this.ChangeScene00220(extra, list)));
  }

  public void IbtnRewardCheck()
  {
    Quest00227Scene.ChangeScene(this.questScoreCampaignProgress, true);
  }

  public void IbtnRanking()
  {
    Quest00229Scene.ChangeScene(this.questScoreCampaignProgress.id, true);
  }

  public void IbtnHowToPlay() => Quest00228Scene.ChangeScene(this.questScoreCampaignProgress, true);
}
