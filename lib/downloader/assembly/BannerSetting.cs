// Decompiled with JetBrains decompiler
// Type: BannerSetting
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
public class BannerSetting : BannerBase
{
  public int ID;
  [SerializeField]
  public int priority;
  [SerializeField]
  private float delay = 3f;
  [SerializeField]
  private GameObject togo;
  [SerializeField]
  private FloatButton Button;
  [SerializeField]
  private GameObject BannerEffectSprite;
  [SerializeField]
  private UIUnityMaskRenderer effectRenderer;
  [SerializeField]
  private Animator animator;
  private DateTime serverTime;
  private EventDelegate del;
  private bool DestroyFlag;
  private bool notJump;

  public UIUnityMaskRenderer EffectRenderer => this.effectRenderer;

  public bool DestroyButton => this.DestroyFlag;

  private bool isUseWebImage(Banner banner)
  {
    return !(banner.transition.scene_name == "quest002_20") && !(banner.transition.scene_name == "quest002_19") && !(banner.transition.scene_name == "quest002_26");
  }

  public IEnumerator Init(
    Banner banner,
    BannersProc parent,
    DateTime serverTime,
    Action callback = null,
    bool isStack = true)
  {
    BannerSetting bannerSetting = this;
    bannerSetting.serverTime = serverTime;
    bannerSetting.DestroyFlag = !BannerSetting.judgeTime(banner, serverTime);
    if (!bannerSetting.DestroyFlag)
    {
      IEnumerator e;
      if (!bannerSetting.isUseWebImage(banner))
      {
        e = bannerSetting.LoadAndSetImage(banner);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = bannerSetting.LoadAndSetImage(banner.url);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      bannerSetting.effectRenderer.SetTexture("_MaskTex", ((UIWidget) bannerSetting.IdleSprite).mainTexture);
      bannerSetting.setEmphasisEffectVisibility(banner.emphasis);
      e = bannerSetting.SetTransition(banner, parent, callback, isStack);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!bannerSetting.DestroyFlag)
      {
        bannerSetting.delay = (float) banner.duration_seconds;
        bannerSetting.ID = banner.id;
        bannerSetting.priority = banner.priority;
        if (!bannerSetting.notJump)
          EventDelegate.Set(bannerSetting.BtnFormation.onClick, bannerSetting.del);
        // ISSUE: reference to a compiler-generated method
        EventDelegate.Set(bannerSetting.BtnFormation.onOver, new EventDelegate.Callback(bannerSetting.\u003CInit\u003Eb__18_0));
        // ISSUE: reference to a compiler-generated method
        EventDelegate.Set(bannerSetting.BtnFormation.onOut, new EventDelegate.Callback(bannerSetting.\u003CInit\u003Eb__18_1));
        bannerSetting.onOut();
      }
    }
  }

  public static bool judgeTime(Banner banner, DateTime now)
  {
    return !banner.end_at.HasValue || now < banner.end_at.Value;
  }

  public void onOver()
  {
  }

  public void onOut()
  {
  }

  private IEnumerator onBannerEventConnection(
    BannerSetting.BannerType type,
    Banner banner,
    Action callback,
    bool isStack)
  {
    IEnumerator e;
    if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Future<WebAPI.Response.QuestProgressExtra> Extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(error);
      }));
      e = Extra.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (Extra.Result != null)
        WebAPI.SetLatestResponsedAt("QuestProgressExtra");
      Extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
    }
    PlayerExtraQuestS[] source = SMManager.Get<PlayerExtraQuestS[]>();
    if (banner.transition.arg1 != 0 && !((IEnumerable<PlayerExtraQuestS>) source).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s != null && x.quest_extra_s.ID == banner.transition.arg1)))
    {
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
    }
    else
    {
      switch (type)
      {
        case BannerSetting.BannerType.BANNER_TYPE_L:
          if (callback != null)
            callback();
          Quest00219Scene.ChangeScene(banner.transition.arg1, isStack);
          break;
        case BannerSetting.BannerType.BANNER_TYPE_M:
          if (callback != null)
            callback();
          Quest00220Scene.ChangeScene00220(banner.transition.arg1);
          break;
        case BannerSetting.BannerType.BANNER_TYPE_RANKING_EVENT:
          if (callback != null)
            callback();
          Quest00226Scene.ChangeScene(banner.transition.arg1, isStack);
          break;
      }
    }
  }

  private void onBannerEvent(
    BannerSetting.BannerType type,
    Banner banner,
    Action callback,
    bool isStack)
  {
    this.StopAnime();
    Singleton<CommonRoot>.GetInstance().StartCoroutine(this.onBannerEventConnection(type, banner, callback, isStack));
  }

  public static bool IsExistSpritePath(Banner banner)
  {
    BannerBase.GetSpriteIdlePath(banner.id, BannerBase.Type.mypage);
    string spriteIdlePathQuest;
    if (banner.transition.scene_name == "quest002_20")
    {
      QuestExtraS masterS = MasterData.QuestExtraS[banner.transition.arg1];
      spriteIdlePathQuest = BannerBase.GetSpriteIdlePathQuest(masterS, masterS.quest_m_QuestExtraM, QuestExtra.SeekType.M);
    }
    else
    {
      if (!(banner.transition.scene_name == "quest002_19") && !(banner.transition.scene_name == "quest002_26"))
        return !string.IsNullOrEmpty(banner.url);
      QuestExtraS masterS = MasterData.QuestExtraS[banner.transition.arg1];
      spriteIdlePathQuest = BannerBase.GetSpriteIdlePathQuest(masterS, masterS.quest_l_QuestExtraL, QuestExtra.SeekType.L);
    }
    return Singleton<ResourceManager>.GetInstance().Contains(spriteIdlePathQuest);
  }

  private IEnumerator LoadAndSetImage(Banner banner)
  {
    BannerSetting bannerSetting = this;
    string path = BannerBase.GetSpriteIdlePath(banner.id, BannerBase.Type.mypage);
    if (banner.transition.scene_name == "quest002_20")
    {
      QuestExtraS masterS = MasterData.QuestExtraS[banner.transition.arg1];
      path = BannerBase.GetSpriteIdlePathQuest(masterS, masterS.quest_m_QuestExtraM, QuestExtra.SeekType.M);
    }
    else if (banner.transition.scene_name == "quest002_19" || banner.transition.scene_name == "quest002_26")
    {
      QuestExtraS masterS = MasterData.QuestExtraS[banner.transition.arg1];
      path = BannerBase.GetSpriteIdlePathQuest(masterS, masterS.quest_l_QuestExtraL, QuestExtra.SeekType.L);
    }
    Future<Texture2D> resource = Singleton<ResourceManager>.GetInstance().LoadOrNull<Texture2D>(path);
    IEnumerator e = resource.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = resource.Result;
    ((Texture) result).wrapMode = (TextureWrapMode) 1;
    float width = (float) ((Texture) result).width;
    float height = (float) ((Texture) result).height;
    Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, width, height), new Vector2(0.0f, 0.0f), 1f, 100U, (SpriteMeshType) 0);
    bannerSetting.IdleSprite.sprite2D = sprite;
    ((UIWidget) bannerSetting.IdleSprite).SetDimensions((int) width, (int) height);
  }

  public IEnumerator SetTransition(
    Banner banner,
    BannersProc parent,
    Action callback,
    bool isStack)
  {
    BannerSetting bannerSetting = this;
    CampaignQuest.RankingEventTerm rankingEventTerm = CampaignQuest.RankingEventTerm.normal;
    ((Behaviour) bannerSetting.Button).enabled = true;
    bannerSetting.notJump = false;
    if (banner.end_at.HasValue)
      bannerSetting.EndTime = banner.end_at.Value;
    if (banner.transition.scene_name == "mypage001_8_2")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_2", false, (object) banner.transition.arg1);
      }));
    else if (banner.transition.scene_name == "quest002_20")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        this.onBannerEvent(BannerSetting.BannerType.BANNER_TYPE_M, banner, callback, isStack);
      }));
    else if (banner.transition.scene_name == "quest002_19" || banner.transition.scene_name == "quest002_26")
    {
      int idl = MasterData.QuestExtraS[banner.transition.arg1].quest_l_QuestExtraL;
      if (banner.transition.scene_name == "quest002_26")
      {
        QuestScoreCampaignProgress[] source = SMManager.Get<QuestScoreCampaignProgress[]>();
        if (source == null)
        {
          Future<WebAPI.Response.QuestProgressExtra> extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
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
            source = SMManager.Get<QuestScoreCampaignProgress[]>();
            extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
          }
        }
        if (source != null)
        {
          QuestScoreCampaignProgress campaign = ((IEnumerable<QuestScoreCampaignProgress>) source).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == idl));
          if (campaign != null)
          {
            rankingEventTerm = CampaignQuest.GetEvetnTerm(campaign, bannerSetting.serverTime);
            if (rankingEventTerm == CampaignQuest.RankingEventTerm.normal)
              bannerSetting.EndTime = campaign.end_at;
            else if (rankingEventTerm == CampaignQuest.RankingEventTerm.aggregate)
            {
              bannerSetting.EndTime = campaign.final_at;
              ((Behaviour) bannerSetting.Button).enabled = false;
            }
            else if (rankingEventTerm == CampaignQuest.RankingEventTerm.receive)
              bannerSetting.EndTime = campaign.latest_end_at;
          }
        }
      }
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (banner.transition.scene_name == "quest002_19")
        {
          this.onBannerEvent(BannerSetting.BannerType.BANNER_TYPE_L, banner, callback, isStack);
        }
        else
        {
          if (rankingEventTerm == CampaignQuest.RankingEventTerm.aggregate)
            return;
          this.onBannerEvent(BannerSetting.BannerType.BANNER_TYPE_RANKING_EVENT, banner, callback, isStack);
        }
      }));
    }
    else if (banner.transition.scene_name == "gacha006_3")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Singleton<NGSceneManager>.GetInstance().changeScene(banner.transition.scene_name, false, (object) banner.transition.arg1);
      }));
    else if (banner.transition.scene_name == "quest002_30")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Quest00230Scene.ChangeScene(false, banner.transition.arg1);
      }));
    else if (banner.transition.scene_name == "quest002_5")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Quest0025Scene.changeScene0025(false, new Quest0025Scene.Quest0025Param(banner.transition.arg2, banner.transition.arg1, false));
      }));
    else if (banner.transition.scene_name == "quest002_4")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
        QuestStoryS questStoryS = (QuestStoryS) null;
        Quest00240723Menu.StoryMode storyMode = !MasterData.QuestStoryS.TryGetValue(banner.transition.arg1, out questStoryS) ? Quest00240723Menu.StoryMode.LostRagnarok : (Quest00240723Menu.StoryMode) questStoryS.quest_xl_QuestStoryXL;
        if (questStoryS != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == banner.transition.arg1)))
          Quest00240723Scene.ChangeScene0024(false, MasterData.QuestStoryS[banner.transition.arg1].quest_l_QuestStoryL, true);
        else
          Quest00240723Scene.ChangeScene0024(false, ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => (Quest00240723Menu.StoryMode) x.quest_story_s.quest_xl_QuestStoryXL == storyMode)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).Max(), true);
      }));
    else if (banner.transition.scene_name == "raid_top")
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Singleton<NGGameDataManager>.GetInstance().IsSea = false;
        PlayerAffiliation current = PlayerAffiliation.Current;
        if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) == 0)
          MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
        else
          RaidTopScene.ChangeSceneBattleFinish();
      }));
    else if (banner.transition.scene_name == "")
      bannerSetting.notJump = true;
    else
      bannerSetting.del = new EventDelegate((EventDelegate.Callback) (() =>
      {
        parent.StopScroll();
        this.StopAnime();
        this.setEmphasisEffectVisibility(false);
        if (callback != null)
          callback();
        Singleton<NGSceneManager>.GetInstance().changeScene(banner.transition.scene_name, false);
      }));
    bannerSetting.togo.SetActive(false);
    TimeSpan timeSpan = bannerSetting.EndTime - bannerSetting.serverTime;
    if (banner.show_exp && timeSpan.TotalMilliseconds > 0.0)
    {
      bannerSetting.togo.SetActive(true);
      bannerSetting.SetTime(bannerSetting.serverTime, rankingEventTerm);
    }
  }

  public void setEmphasisEffectVisibility(bool isVisible)
  {
    if (!Object.op_Inequality((Object) this.BannerEffectSprite, (Object) null))
      return;
    this.BannerEffectSprite.gameObject.SetActive(isVisible);
  }

  public void StartTween()
  {
    TweenAlpha component = ((Component) this).GetComponent<TweenAlpha>();
    ((UITweener) component).delay = 0.0f;
    EventDelegate.Set(((UITweener) component).onFinished, new EventDelegate((EventDelegate.Callback) (() => this.EndTween()))
    {
      oneShot = true
    });
    ((UITweener) component).PlayForward();
  }

  public void EndTween()
  {
    TweenAlpha component = ((Component) this).GetComponent<TweenAlpha>();
    ((UITweener) component).delay = this.delay;
    EventDelegate.Set(((UITweener) component).onFinished, new EventDelegate((EventDelegate.Callback) (() => this.Next()))
    {
      oneShot = true
    });
    ((UITweener) component).PlayReverse();
  }

  public void Next()
  {
    ((Component) ((Component) this).transform.parent).GetComponent<BannersProc>().LoopBannerNext();
  }

  public void StartAnime()
  {
    if (!Object.op_Inequality((Object) this.animator, (Object) null) || !((Component) this.animator).gameObject.activeInHierarchy)
      return;
    this.animator.SetBool("isPlay", true);
  }

  public void StopAnime()
  {
    if (!Object.op_Inequality((Object) this.animator, (Object) null) || !((Component) this.animator).gameObject.activeInHierarchy)
      return;
    this.animator.SetBool("isPlay", false);
  }

  private enum BannerType
  {
    BANNER_TYPE_L,
    BANNER_TYPE_M,
    BANNER_TYPE_OTHER,
    BANNER_TYPE_RANKING_EVENT,
  }
}
