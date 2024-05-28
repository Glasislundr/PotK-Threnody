// Decompiled with JetBrains decompiler
// Type: Versus0261Menu
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
public class Versus0261Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtAttention;
  [SerializeField]
  private UISprite slcExpBoost;
  [SerializeField]
  private UI2DSprite slcCampaign;
  [SerializeField]
  private UIButton buttonBack;
  [SerializeField]
  private UIButton buttonRandom;
  [SerializeField]
  private UIButton buttonFriend;
  [SerializeField]
  private UIButton buttonClassMatch;
  [SerializeField]
  private UIButton buttonClassMatchBanner;
  [SerializeField]
  private GameObject classMatchComingSoon;
  [SerializeField]
  private GameObject classMatchBannerComingSoon;
  [SerializeField]
  private UILabel TxtMatchRemain;
  [SerializeField]
  private UILabel TxtRankingTally;
  [SerializeField]
  private UILabel TxtRemain;
  [SerializeField]
  private UISprite slcTimeBase;
  [SerializeField]
  private UISprite slcRankingTallyBase;
  [SerializeField]
  private GameObject dynLampGrid;
  [SerializeField]
  private GameObject slcEventBase;
  [SerializeField]
  private CreateIconObject iconObject;
  [SerializeField]
  private GameObject dirBonus;
  [SerializeField]
  private UILabel TxtNumItem;
  private WebAPI.Response.PvpBoot pvpInfo;
  private bool hasBannerImage;
  private bool Newface_flg;
  private bool isEventOpen;
  private static readonly string ObjectName_LampOn = "slc_icon_Multi_Lamp_01_on";

  public bool needTitleBack { get; private set; }

  public IEnumerator Initialize(WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus0261Menu versus0261Menu = this;
    versus0261Menu.pvpInfo = pvpInfo;
    versus0261Menu.SetText(pvpInfo.remaining_times, pvpInfo.limit_times);
    versus0261Menu.SetSomethingBoost(pvpInfo.pvp_campaigns);
    IEnumerator e;
    if (pvpInfo.pvp_maintenance)
    {
      e = PopupCommon.Show(pvpInfo.pvp_maintenance_title, pvpInfo.pvp_maintenance_message, (Action) (() => MypageScene.ChangeScene()));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (!pvpInfo.is_latest_client_version)
    {
      // ISSUE: reference to a compiler-generated method
      ModalWindow.Show(Consts.GetInstance().TOP_SCENE_CHANGE_NEXT_SCENE_LOOP_1, Consts.GetInstance().VERSUS_NOT_LATEST_VERSION_DESCRIPTION_PC, new Action(versus0261Menu.\u003CInitialize\u003Eb__29_0));
    }
    else if (pvpInfo.is_battle)
    {
      bool popupWait = true;
      ModalWindow.Show(Consts.GetInstance().VERSUS_0026_UNRESOLVED_ERROR_TITLE, Consts.GetInstance().VERSUS_0026_UNRESOLVED_ERROR_MESSAGE, (Action) (() => popupWait = false));
      yield return (object) new WaitWhile((Func<bool>) (() => popupWait));
      versus0261Menu.needTitleBack = true;
    }
    else
    {
      if (pvpInfo.is_tutorial && !versus0261Menu.Newface_flg)
      {
        ((UIButtonColor) versus0261Menu.buttonFriend).isEnabled = pvpInfo.is_tutorial_battle_end;
        if (Persist.pvpInfo.Data.currentPage == 2 && pvpInfo.is_tutorial_battle_end)
        {
          Persist.pvpInfo.Data.currentPage = 3;
          Persist.pvpInfo.Flush();
        }
        if (Persist.pvpInfo.Data.currentPage == 0)
        {
          // ISSUE: reference to a compiler-generated method
          Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("pvp1", new Dictionary<string, Func<Transform, UIButton>>()
          {
            {
              "versus_multi7",
              (Func<Transform, UIButton>) (root => ((Component) root.GetChildInFind("Middle")).GetComponentInChildren<UIButton>())
            }
          }, new Action(versus0261Menu.\u003CInitialize\u003Eb__29_5));
        }
        else if (Persist.pvpInfo.Data.currentPage == 3 || Persist.pvpInfo.Data.currentPage == 4)
        {
          // ISSUE: reference to a compiler-generated method
          Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("pvp5", new Dictionary<string, Func<Transform, UIButton>>()
          {
            {
              "versus_multi8",
              (Func<Transform, UIButton>) (root => ((Component) root.GetChildInFind("Middle")).GetComponentInChildren<UIButton>())
            }
          }, new Action(versus0261Menu.\u003CInitialize\u003Eb__29_7));
        }
        else if (Persist.pvpInfo.Data.currentPage == 5)
          versus0261Menu.IbtnFriendMatch();
      }
      else if (Persist.pvpInfo.Data.currentPage < Versus026MatchBase.PVP_TUTORIAL_FRIEND_END_PAGE)
      {
        Persist.pvpInfo.Data.currentPage = Versus026MatchBase.PVP_TUTORIAL_FRIEND_END_PAGE;
        Persist.pvpInfo.Flush();
      }
      versus0261Menu.isEventOpen = versus0261Menu.checkEventOpen();
      ((Component) versus0261Menu.buttonClassMatch).gameObject.SetActive(!versus0261Menu.isEventOpen);
      ((Component) versus0261Menu.buttonClassMatchBanner).gameObject.SetActive(versus0261Menu.isEventOpen);
      if (pvpInfo.rank_aggregate)
      {
        ((UIButtonColor) versus0261Menu.buttonClassMatch).isEnabled = false;
        ((UIButtonColor) versus0261Menu.buttonClassMatchBanner).isEnabled = false;
        versus0261Menu.classMatchComingSoon.SetActive(false);
        versus0261Menu.classMatchBannerComingSoon.SetActive(false);
      }
      else
      {
        ((UIButtonColor) versus0261Menu.buttonClassMatch).isEnabled = true;
        ((UIButtonColor) versus0261Menu.buttonClassMatchBanner).isEnabled = true;
        versus0261Menu.classMatchComingSoon.SetActive(false);
        versus0261Menu.classMatchBannerComingSoon.SetActive(false);
      }
      if (!versus0261Menu.isEventOpen)
      {
        e = versus0261Menu.SetCampaignImage(pvpInfo.pvp_campaigns);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        yield return (object) versus0261Menu.SetBannerImage(pvpInfo.pvp_campaigns);
        ((Component) versus0261Menu.buttonClassMatch).gameObject.SetActive(!versus0261Menu.hasBannerImage);
        ((Component) versus0261Menu.buttonClassMatchBanner).gameObject.SetActive(versus0261Menu.hasBannerImage);
      }
      if (Persist.pvpInfo.Data.currentPage >= Versus026MatchBase.PVP_TUTORIAL_FRIEND_END_PAGE && SMManager.Get<Player>().IsClassMatch())
      {
        versus0261Menu.slcEventBase.SetActive(versus0261Menu.isEventOpen && versus0261Menu.hasBannerImage);
      }
      else
      {
        ((UIButtonColor) versus0261Menu.buttonClassMatch).isEnabled = false;
        ((UIButtonColor) versus0261Menu.buttonClassMatchBanner).isEnabled = false;
        versus0261Menu.classMatchComingSoon.SetActive(!versus0261Menu.isEventOpen);
        versus0261Menu.classMatchBannerComingSoon.SetActive(versus0261Menu.isEventOpen);
      }
      versus0261Menu.SetNextReward();
    }
  }

  private bool checkEventOpen()
  {
    return ((IEnumerable<PvPCampaign>) this.pvpInfo.pvp_campaigns).Any<PvPCampaign>();
  }

  public void SetSomethingBoost(PvPCampaign[] campaigns)
  {
    ((Component) this.slcExpBoost).gameObject.SetActive(((IEnumerable<PvPCampaign>) campaigns).Any<PvPCampaign>((Func<PvPCampaign, bool>) (x => x.campaign.campaign_type_id == 2)));
  }

  public IEnumerator SetCampaignImage(PvPCampaign[] campaigns)
  {
    IEnumerable<PvPCampaign> source = ((IEnumerable<PvPCampaign>) campaigns).Where<PvPCampaign>((Func<PvPCampaign, bool>) (x => x.image_url != ""));
    if (source.Any<PvPCampaign>())
    {
      PvPCampaign pvPcampaign = source.FirstOrDefault<PvPCampaign>((Func<PvPCampaign, bool>) (x => x.campaign.campaign_type_id == 3));
      string url = pvPcampaign == null ? source.OrderByDescending<PvPCampaign, int>((Func<PvPCampaign, int>) (x => x.campaign.campaign_type_id)).First<PvPCampaign>().image_url : pvPcampaign.image_url;
      IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(url, this.slcCampaign);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator SetBannerImage(PvPCampaign[] campaigns)
  {
    this.hasBannerImage = false;
    PvPCampaign pvPcampaign = ((IEnumerable<PvPCampaign>) campaigns).FirstOrDefault<PvPCampaign>((Func<PvPCampaign, bool>) (x => !string.IsNullOrEmpty(x.button_image_url)));
    if (pvPcampaign != null)
    {
      string buttonImageUrl = pvPcampaign.button_image_url;
      UI2DSprite spr = ((Component) this.buttonClassMatchBanner).gameObject.GetComponent<UI2DSprite>();
      spr.sprite2D = (Sprite) null;
      IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(buttonImageUrl, spr);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.hasBannerImage = Object.op_Inequality((Object) spr.sprite2D, (Object) null);
      spr = (UI2DSprite) null;
    }
  }

  public void SetText(int remainNum, int limitNum)
  {
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetTextLocalize(instance.VERSUS_00261TITLE);
    ((Component) this.TxtRemain).gameObject.SetActive(true);
    this.TxtRemain.SetTextLocalize(Consts.Format(instance.VERSUS_00262BONUS, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) remainNum
      }
    }));
    this.txtAttention.SetTextLocalize(Consts.Format(instance.VERSUS_00261ATTENTION, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) limitNum
      }
    }));
    this.SetMatchRemainText();
  }

  private void SetMatchRemainText()
  {
    Player player = SMManager.Get<Player>();
    if (this.pvpInfo.rank_aggregate)
    {
      DateTime dateTime = this.pvpInfo.aggregate_finish_time.Value;
      string text = Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_RANK_AGREGATE, (IDictionary) new Hashtable()
      {
        {
          (object) "remainTime",
          (object) this.SetRemainTime(dateTime.Hour, dateTime.Minute)
        }
      });
      ((Component) this.TxtRankingTally).gameObject.SetActive(true);
      this.TxtRankingTally.SetTextLocalize(text);
      ((Component) this.TxtMatchRemain).gameObject.SetActive(false);
      this.dynLampGrid.gameObject.SetActive(false);
      ((Component) this.slcRankingTallyBase).gameObject.SetActive(true);
      ((Component) this.slcTimeBase).gameObject.SetActive(false);
    }
    else if (player.mp <= 0 && this.pvpInfo.aggregate_finish_time.HasValue && this.pvpInfo.aggregate_finish_time.Value <= player.mp_full_recovery_at)
    {
      DateTime dateTime = this.pvpInfo.matches_finish_time.Value;
      string text = Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_BEFORE_RANK_AGREGATE, (IDictionary) new Hashtable()
      {
        {
          (object) "remainTime",
          (object) this.SetRemainTime(dateTime.Hour, dateTime.Minute)
        }
      });
      ((Component) this.TxtRankingTally).gameObject.SetActive(true);
      this.TxtRankingTally.SetTextLocalize(text);
      ((Component) this.TxtMatchRemain).gameObject.SetActive(false);
      this.dynLampGrid.gameObject.SetActive(false);
      ((Component) this.slcRankingTallyBase).gameObject.SetActive(true);
      ((Component) this.slcTimeBase).gameObject.SetActive(false);
    }
    else
    {
      DateTime mpFullRecoveryAt = player.mp_full_recovery_at;
      string text = Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_TOP, (IDictionary) new Hashtable()
      {
        {
          (object) "remainTime",
          (object) this.SetRemainTime(mpFullRecoveryAt.Hour, mpFullRecoveryAt.Minute)
        }
      });
      ((Component) this.TxtMatchRemain).gameObject.SetActive(true);
      this.TxtMatchRemain.SetTextLocalize(text);
      ((Component) this.TxtRankingTally).gameObject.SetActive(false);
      this.dynLampGrid.gameObject.SetActive(true);
      ((Component) this.slcRankingTallyBase).gameObject.SetActive(false);
      ((Component) this.slcTimeBase).gameObject.SetActive(true);
      this.StartCoroutine(this.CreateLampPrefab(player.mp, player.mp_max));
    }
  }

  private IEnumerator CreateLampPrefab(int mp, int mpMax)
  {
    List<Transform> transformList = new List<Transform>();
    foreach (Transform transform in this.dynLampGrid.gameObject.transform)
      transformList.Add(((Component) transform).gameObject.transform);
    this.dynLampGrid.gameObject.transform.DetachChildren();
    foreach (Transform transform in transformList)
    {
      transform.parent = (Transform) null;
      Object.Destroy((Object) ((Component) transform).gameObject);
    }
    Future<GameObject> prefabF = new ResourceObject("Prefabs/versus026_12/slc_icon_Multi_Lamp").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    for (int index = 0; index < mpMax; ++index)
    {
      GameObject gameObject1 = result;
      GameObject gameObject2 = ((Component) gameObject1.transform.Find(Versus0261Menu.ObjectName_LampOn)).gameObject;
      if (index < mp)
        NGUITools.AddChild(this.dynLampGrid, gameObject2);
      else
        ((Component) NGUITools.AddChild(this.dynLampGrid, gameObject1).transform.Find(Versus0261Menu.ObjectName_LampOn)).gameObject.SetActive(false);
    }
    this.dynLampGrid.GetComponent<UIGrid>().Reposition();
  }

  private void SetNextReward()
  {
    foreach (Component component in ((Component) this.iconObject).gameObject.transform)
      Object.Destroy((Object) component.gameObject);
    bool flag = false;
    if (this.pvpInfo.next_first_battle_reward != null)
    {
      int rewardQuantity = this.pvpInfo.next_first_battle_reward.reward_quantity;
      if (rewardQuantity > 0)
      {
        this.StartCoroutine(this.CreateItemIcon());
        this.TxtNumItem.SetTextLocalize("×" + (object) rewardQuantity);
        flag = true;
      }
    }
    this.dirBonus.gameObject.SetActive(flag);
  }

  private IEnumerator CreateItemIcon()
  {
    int rewardQuantity = this.pvpInfo.next_first_battle_reward.reward_quantity;
    yield return (object) this.iconObject.CreateThumbnail((MasterDataTable.CommonRewardType) this.pvpInfo.next_first_battle_reward.reward_type_id, this.pvpInfo.next_first_battle_reward.reward_id, isButton: false);
  }

  private string SetRemainTime(int hour, int minute)
  {
    Consts instance = Consts.GetInstance();
    return Consts.Format(minute < 10 ? instance.VERSUS_00261REMAIN_TIME_SUB : instance.VERSUS_00261REMAIN_TIME, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (hour),
        (object) hour.ToLocalizeNumberText()
      },
      {
        (object) nameof (minute),
        (object) minute.ToLocalizeNumberText()
      }
    });
  }

  public void IbtnClassMatch()
  {
    if (this.IsPushAndSet())
      return;
    Versus02610Scene.ChangeScene(true, this.pvpInfo);
  }

  public void IbtnRundomMatch()
  {
    if (this.IsPushAndSet())
      return;
    Versus0262Scene.ChangeScene0262(true, PvpMatchingTypeEnum.normal, this.pvpInfo);
  }

  public void IbtnFriendMatch()
  {
    if (this.IsPushAndSet())
      return;
    Versus0262Scene.ChangeScene0262(true, PvpMatchingTypeEnum.guest, this.pvpInfo);
  }

  public void IbtnMap()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.CreateMapPrefab());
  }

  private IEnumerator CreateMapPrefab()
  {
    Future<GameObject> prefabF = Res.Prefabs.versus026_1.dir_MapCheck.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().openAlert(prefabF.Result).GetComponent<popup0261MapCheck>().Init(this.pvpInfo.stage.stage_id, this.pvpInfo.stage.RemainingTime());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
