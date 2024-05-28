// Decompiled with JetBrains decompiler
// Type: SeaTalkMessageMenu
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
public class SeaTalkMessageMenu : BackButtonMenuBase
{
  private const float REPLY_MERGIN_HEIGHT = 20f;
  private const float REPLY_ITEM_HEIGHT = 136f;
  private const int REPLY_UNDER_ADD_MERGIN = 23;
  [Header("Top")]
  [SerializeField]
  private UILabel partnerName;
  [Header("Middle")]
  [SerializeField]
  private UIDragScrollView dragScrollView;
  [SerializeField]
  private UIButton replyCloseScrollView;
  [SerializeField]
  private UIScrollView scrollView;
  private Transform t_scrollView;
  [SerializeField]
  private UIScrollBar scrollBar;
  [SerializeField]
  private Transform unreadLine;
  [SerializeField]
  private SeaTalkMessage unreadComponent;
  [SerializeField]
  private Transform tailPlaceholder;
  [Header("Bottom")]
  [SerializeField]
  private GameObject bottomfit;
  [SerializeField]
  private UILabel missionProgress;
  [SerializeField]
  private GameObject receivableRewardBatch;
  [Header("返信周り")]
  [SerializeField]
  private GameObject replyCloseButton;
  [SerializeField]
  private GameObject callDestructionButton;
  [SerializeField]
  private UIButton replyButton;
  [SerializeField]
  private TweenColor replyButtonLabelTween;
  [SerializeField]
  private GameObject replyArrow;
  [SerializeField]
  private UISprite replyChoiceViewBase;
  private int initReplyChoiceViewBaseHeight;
  [SerializeField]
  private GameObject replyChoiceView;
  [SerializeField]
  private UILabel replyMonologue;
  [SerializeField]
  private Transform replyItem;
  [SerializeField]
  private Transform replyIconParent;
  [SerializeField]
  private UILabel replyLabel;
  private GameObject unitIconPrefab;
  private GameObject tempMessage;
  private UILabel tempMessageLabel;
  private GameObject messagePartnerPrefab;
  private GameObject messagePlayerPrefab;
  private GameObject messageDatePrefab;
  private GameObject messageGiftPrefab;
  private GameObject messageWritingPrefab;
  private List<SeaTalkMessageInfo> infos = new List<SeaTalkMessageInfo>();
  private List<Transform> partnerMessages = new List<Transform>();
  private List<SeaTalkMessage> partnerMessagesComponents = new List<SeaTalkMessage>();
  private List<Transform> playerMessages = new List<Transform>();
  private List<SeaTalkMessage> playerMessagesComponents = new List<SeaTalkMessage>();
  private List<Transform> dateMessages = new List<Transform>();
  private List<SeaTalkMessage> dateMessagesComponents = new List<SeaTalkMessage>();
  private List<Transform> giftMessages = new List<Transform>();
  private List<SeaTalkMessage> giftMessagesComponents = new List<SeaTalkMessage>();
  private Transform writing;
  private SeaTalkMessage writingComponent;
  private GameObject replyChoicePrefab;
  private GameObject replyGivePrefab;
  private GameObject replyChoiceDatePrefab;
  private GameObject seaTalkTutorial;
  private List<SeaTalkReply> talkReplys = new List<SeaTalkReply>();
  private float replyTweenY;
  private Sea030HomeMenu seaHomeMenu;
  private WebAPI.Response.SeaTalkMessage talkMessageResponse;
  private PlayerCallLetter playerCallLetter;
  private WebAPI.Response.SeaTalkPartner talkPartnerResponse;
  private TalkUnitInfo talkUnitInfo = new TalkUnitInfo();
  private bool isBlockEventForAnimation;
  private PlayerTalkMessage[] addPlayerTalkMessages;

  private void SetReplyButton(bool isOn)
  {
    if (isOn)
    {
      ((UIButtonColor) this.replyButton).isEnabled = true;
      ((Behaviour) this.replyButtonLabelTween).enabled = true;
      ((Component) this.replyButtonLabelTween).GetComponent<UILabel>().SetTextLocalize("[ffffff]返信する[-]");
      this.replyArrow.SetActive(true);
    }
    else
    {
      ((UIButtonColor) this.replyButton).isEnabled = false;
      ((Behaviour) this.replyButtonLabelTween).enabled = false;
      ((Component) this.replyButtonLabelTween).GetComponent<UILabel>().SetTextLocalize("[949494]返信する[-]");
      this.replyArrow.SetActive(false);
    }
  }

  public void SetMissionsAndBatch(PlayerCallMission[] missions)
  {
    bool flag = ((IEnumerable<PlayerCallMission>) missions).Any<PlayerCallMission>((Func<PlayerCallMission, bool>) (x => x.mission_status == 3));
    foreach (PlayerTalkPartner partner in this.talkPartnerResponse.partners)
    {
      if (partner.letter.same_character_id == this.playerCallLetter.same_character_id)
      {
        partner.receivable_reward = flag;
        break;
      }
    }
    SeaTalkCommon.UpdateTalkBatch(this.talkPartnerResponse.partners);
    this.talkMessageResponse.missions = missions;
    this.receivableRewardBatch.SetActive(false);
    foreach (PlayerCallMission mission in this.talkMessageResponse.missions)
    {
      if (mission.mission_status == 3)
        this.receivableRewardBatch.SetActive(true);
    }
  }

  private void Awake()
  {
    this.t_scrollView = ((Component) this.scrollView).transform;
    // ISSUE: method pointer
    ((Component) this.scrollView).GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved((object) this, __methodptr(OnMove));
    this.replyChoiceView.SetActive(true);
    this.initReplyChoiceViewBaseHeight = ((UIWidget) this.replyChoiceViewBase).height;
  }

  public IEnumerator onInitSceneAsync()
  {
    yield return (object) this.LoadResource();
  }

  public IEnumerator onBackSceneUpdateGift()
  {
    if (this.replyIconParent.childCount > 0)
    {
      SeaTalkMessageInfo info = this.infos[this.infos.Count - 1];
      if (info.callMessage.message_type_id == 5 && info.playerTalkMessage.condition_id.HasValue)
      {
        this.ReplyGiftUpdate(((Component) this.replyIconParent.GetChild(0)).gameObject.GetComponent<ItemIcon>(), info.playerTalkMessage.condition_id.Value);
        yield break;
      }
    }
  }

  public IEnumerator AddMessageView()
  {
    if (this.addPlayerTalkMessages != null && this.addPlayerTalkMessages.Length != 0)
    {
      List<SeaTalkMessageInfo> addInfos = new List<SeaTalkMessageInfo>();
      this.AddInfos(this.addPlayerTalkMessages, addInfos);
      yield return (object) this.AddInfosAnimation(addInfos);
      yield return (object) this.CreateReplyChoice();
      this.UpdateReplyChoiceView();
      this.addPlayerTalkMessages = (PlayerTalkMessage[]) null;
    }
  }

  public static void SeaTalkPartnerRefresh()
  {
    SeaTalkPartnerScene[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<SeaTalkPartnerScene>();
    if (objectsOfTypeAll.Length == 0)
      return;
    objectsOfTypeAll[0].IsInit = false;
  }

  public IEnumerator onStartSceneAsync(
    TalkUnitInfo talkUnitInfo,
    WebAPI.Response.SeaTalkMessage talkMessageResponse,
    WebAPI.Response.SeaTalkPartner talkPartnerResponse,
    Sea030HomeMenu homeMenu)
  {
    this.talkMessageResponse = talkMessageResponse;
    this.playerCallLetter = ((IEnumerable<PlayerCallLetter>) talkMessageResponse.player_call_letters).First<PlayerCallLetter>((Func<PlayerCallLetter, bool>) (x => x.same_character_id == talkUnitInfo.unit.same_character_id));
    this.talkPartnerResponse = talkPartnerResponse;
    this.talkUnitInfo = talkUnitInfo;
    yield return (object) this.SetPartnerUnit();
    this.partnerName.text = talkUnitInfo.unit.name;
    this.CreateInfos();
    this.scrollView.ResetPosition();
    this.SetScrollSettings(true);
    this.UpdateMissionProgress();
    this.receivableRewardBatch.SetActive(false);
    foreach (PlayerCallMission mission in talkMessageResponse.missions)
    {
      if (mission.mission_status == 3)
        this.receivableRewardBatch.SetActive(true);
    }
    yield return (object) this.CreateReplyChoice();
    this.UpdateReplyChoiceView();
    if (!Persist.seaTutorialData.Exists || Persist.seaTutorialData.Exists && !Persist.seaTutorialData.Data.seaTalkTutorial)
    {
      IEnumerator e;
      if (Object.op_Equality((Object) this.seaTalkTutorial, (Object) null))
      {
        Future<GameObject> ft = new ResourceObject("Prefabs/unit004_2/popup_sea_talk_tutorial").Load<GameObject>();
        e = ft.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.seaTalkTutorial = ft.Result;
        ft = (Future<GameObject>) null;
      }
      GameObject popup = Singleton<PopupManager>.GetInstance().open(this.seaTalkTutorial, isNonSe: true, isNonOpenAnime: true);
      e = popup.GetComponent<SimpleScrollContentsPopup>().Initialize((Action) (() =>
      {
        Persist.seaTutorialData.Data.seaTalkTutorial = true;
        Persist.seaTutorialData.Flush();
        Singleton<PopupManager>.GetInstance().dismiss();
      }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
      popup = (GameObject) null;
    }
    this.replyCloseButton.SetActive(false);
  }

  private void UpdateMissionProgress()
  {
    int num = 0;
    foreach (PlayerCallMission mission in this.talkMessageResponse.missions)
    {
      CallMission callMission;
      MasterData.CallMission.TryGetValue(mission.mission_id, out callMission);
      if (mission.count >= callMission.number_times)
        ++num;
    }
    this.missionProgress.text = string.Format("[b]{0}/{1}[-]", (object) num, (object) this.talkMessageResponse.missions.Length);
  }

  private IEnumerator SetPartnerUnit()
  {
    SeaTalkMessageMenu seaTalkMessageMenu = this;
    seaTalkMessageMenu.unitIconPrefab = seaTalkMessageMenu.unitIconPrefab.Clone(((Component) seaTalkMessageMenu).transform);
    UnitIcon unitIcon = seaTalkMessageMenu.unitIconPrefab.GetComponent<UnitIcon>();
    IEnumerator e = unitIcon.SetUnit(seaTalkMessageMenu.talkUnitInfo.unit, seaTalkMessageMenu.talkUnitInfo.unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) unitIcon.Button).gameObject.SetActive(false);
    unitIcon.BottomBaseObject = false;
    seaTalkMessageMenu.unitIconPrefab.SetActive(false);
  }

  private void CreateInfos()
  {
    this.tempMessage = this.messagePlayerPrefab.Clone(this.bottomfit.transform);
    this.tempMessage.transform.localPosition = new Vector3(-750f, 0.0f, 0.0f);
    this.tempMessageLabel = this.tempMessage.GetComponent<SeaTalkMessage>().Comment;
    PlayerTalkMessage playerTalkMessage = (PlayerTalkMessage) null;
    this.tempMessage.SetActive(true);
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < this.talkMessageResponse.messages.Length; ++index)
    {
      PlayerTalkMessage message = this.talkMessageResponse.messages[index];
      SeaTalkMessageInfo seaTalkMessageInfo = new SeaTalkMessageInfo();
      if (!flag2)
      {
        seaTalkMessageInfo.InitDate(message);
        --index;
        flag2 = true;
      }
      else if (playerTalkMessage != null && playerTalkMessage.created_at.Date < message.created_at.Date)
      {
        seaTalkMessageInfo.InitDate(message);
        --index;
        playerTalkMessage = (PlayerTalkMessage) null;
      }
      else if (!flag1 && message.player_message_id == this.talkMessageResponse.last_index)
      {
        seaTalkMessageInfo.InitUnread(message);
        --index;
        flag1 = true;
      }
      else
      {
        seaTalkMessageInfo.Init(message, this.talkUnitInfo, this.tempMessageLabel);
        playerTalkMessage = message;
      }
      this.infos.Add(seaTalkMessageInfo);
    }
    this.tempMessage.SetActive(false);
    SeaTalkMessageInfo info = this.infos[this.infos.Count - 1];
    if (info.playerTalkMessage.expire_at.HasValue)
      info.LimitTimeAnimation = true;
    if (this.talkMessageResponse.last_index <= 0)
    {
      this.talkMessageResponse.last_index = this.infos.Count - 1;
    }
    else
    {
      int num = this.infos.Count - 1;
      for (int index = 0; index < this.infos.Count; ++index)
      {
        if (this.infos[index].MessageViewType != TalkMessageViewType.Date && this.infos[index].MessageViewType != TalkMessageViewType.UnreadLine && this.infos[index].playerTalkMessage.player_message_id == this.talkMessageResponse.last_index)
        {
          num = index;
          break;
        }
      }
      this.talkMessageResponse.last_index = num;
    }
    this.SetInfoPostionY(this.infos, this.infos[0].UpperHeight / 2f);
    this.tailPlaceholder.localPosition = new Vector3(0.0f, info.PositionY - info.UnderHeight / 2f, 0.0f);
  }

  private void SetInfoPostionY(List<SeaTalkMessageInfo> targetInfos, float initY)
  {
    for (int index1 = 0; index1 < targetInfos.Count; ++index1)
    {
      targetInfos[index1].PositionY = -initY;
      int index2 = index1 + 1;
      float num = 0.0f;
      if (index2 < targetInfos.Count)
        num = (float) ((double) targetInfos[index1].UnderHeight / 2.0 + (double) targetInfos[index2].UpperHeight / 2.0);
      initY += num;
    }
  }

  private void SetScrollSettings(bool isUnread)
  {
    float num1 = Mathf.Abs(this.infos[this.talkMessageResponse.last_index].PositionY) + this.infos[this.talkMessageResponse.last_index].UnderHeight / 2f;
    if ((double) num1 >= (double) this.scrollView.panel.height)
    {
      if (this.infos.Count - 1 > this.talkMessageResponse.last_index)
      {
        float num2 = 0.0f;
        if (isUnread)
        {
          SeaTalkMessageInfo seaTalkMessageInfo = this.infos.FirstOrDefault<SeaTalkMessageInfo>((Func<SeaTalkMessageInfo, bool>) (x => x.MessageViewType == TalkMessageViewType.UnreadLine));
          if (seaTalkMessageInfo != null)
          {
            float num3 = Mathf.Abs(seaTalkMessageInfo.PositionY) - seaTalkMessageInfo.UpperHeight / 2f;
            if ((double) Mathf.Abs(this.infos[this.infos.Count - 1].PositionY) + (double) this.infos[this.infos.Count - 1].UnderHeight / 2.0 - (double) num3 >= (double) this.scrollView.panel.height)
              num2 = Mathf.Abs(seaTalkMessageInfo.PositionY) - seaTalkMessageInfo.UpperHeight / 2f;
            else
              this.talkMessageResponse.last_index = this.infos.Count - 1;
          }
          else
            this.talkMessageResponse.last_index = this.infos.Count - 1;
        }
        else
          num2 = num1 - this.scrollView.panel.height;
        this.scrollView.MoveRelative(new Vector3(0.0f, num2, 0.0f));
      }
      ((Behaviour) this.dragScrollView).enabled = true;
    }
    if (this.infos.Count - 1 <= this.talkMessageResponse.last_index)
      ((UIProgressBar) this.scrollBar).value = 1f;
    if (this.scrollView.shouldMoveVertically)
    {
      ((Behaviour) this.dragScrollView).enabled = true;
    }
    else
    {
      ((UIProgressBar) this.scrollBar).value = 0.0f;
      ((Behaviour) this.dragScrollView).enabled = false;
    }
  }

  private void InitReplyChoice()
  {
    foreach (SeaTalkReply talkReply in this.talkReplys)
    {
      ((Component) talkReply).gameObject.SetActive(false);
      Object.DestroyImmediate((Object) ((Component) talkReply).gameObject);
    }
    this.talkReplys.Clear();
    this.replyIconParent.Clear();
    ((UIWidget) this.replyChoiceViewBase).height = this.initReplyChoiceViewBaseHeight;
  }

  private IEnumerator CreateReplyChoice()
  {
    this.InitReplyChoice();
    SeaTalkMessageInfo lastInfo = this.infos[this.infos.Count - 1];
    CallMessage lastCallMessage = lastInfo.callMessage;
    this.callDestructionButton.SetActive(this.playerCallLetter.call_status == 3);
    if (lastCallMessage == null)
    {
      this.SetReplyButton(false);
    }
    else
    {
      TalkMessageType messageTypeId = (TalkMessageType) lastCallMessage.message_type_id;
      if (!new List<TalkMessageType>()
      {
        TalkMessageType.Question,
        TalkMessageType.GoDate,
        TalkMessageType.SendItem
      }.Contains(messageTypeId))
      {
        this.SetReplyButton(false);
      }
      else
      {
        this.SetReplyButton(true);
        IOrderedEnumerable<CallMessage> source = ((IEnumerable<CallMessage>) MasterData.CallMessageList).Where<CallMessage>((Func<CallMessage, bool>) (x => x.message_set_id == lastCallMessage.message_set_id && x.view_order > lastCallMessage.view_order)).OrderBy<CallMessage, int>((Func<CallMessage, int>) (x => x.view_order));
        CallMessage callMessage1 = (CallMessage) null;
        List<CallMessage> callMessageList = new List<CallMessage>();
        foreach (CallMessage callMessage2 in (IEnumerable<CallMessage>) source)
        {
          CallMessage choiceCallMessage = callMessage2;
          if (callMessage1 == null && choiceCallMessage.message_type_id == 9)
          {
            callMessage1 = choiceCallMessage;
          }
          else
          {
            switch (messageTypeId)
            {
              case TalkMessageType.Question:
                if (choiceCallMessage.message_type_id == 3)
                {
                  callMessageList = source.Where<CallMessage>((Func<CallMessage, bool>) (x => x.view_order == choiceCallMessage.view_order && x.message_type_id == 3)).ToList<CallMessage>();
                  break;
                }
                break;
              case TalkMessageType.GoDate:
                if (choiceCallMessage.message_type_id == 7)
                {
                  callMessageList.Add(choiceCallMessage);
                  break;
                }
                break;
              case TalkMessageType.SendItem:
                if (choiceCallMessage.message_type_id == 7 || choiceCallMessage.message_type_id == 6)
                {
                  callMessageList = source.Where<CallMessage>((Func<CallMessage, bool>) (x => x.view_order == choiceCallMessage.view_order && x.message_type_id == 7 || x.message_type_id == 6)).ToList<CallMessage>();
                  break;
                }
                break;
              default:
                Debug.LogError((object) string.Format("想定していないlastMessageTypeです {0}", (object) messageTypeId));
                break;
            }
            if (callMessageList.Count > 0)
              break;
          }
        }
        if (callMessageList.Count <= 0)
          Debug.LogError((object) string.Format("返信選択肢がありません lastCallMessage.ID: {0}", (object) lastCallMessage.ID));
        this.replyMonologue.text = callMessage1 != null ? SeaTalkCommon.GetReplaceMessage(callMessage1.text, this.talkUnitInfo, lastInfo.playerTalkMessage) : "なんて、返信しよう";
        Action<CallMessage> onReplyAction = (Action<CallMessage>) (m_replyMessage => this.OnTalkReply(m_replyMessage));
        foreach (CallMessage callMessage3 in callMessageList)
        {
          SeaTalkReply component = (callMessage3.message_type_id != 6 ? this.replyChoicePrefab.Clone(this.replyChoiceView.transform) : this.replyGivePrefab.Clone(this.replyChoiceView.transform)).GetComponent<SeaTalkReply>();
          component.Init(callMessage3, onReplyAction, this.talkUnitInfo, lastInfo.playerTalkMessage);
          this.talkReplys.Add(component);
        }
        if (messageTypeId == TalkMessageType.GoDate)
          this.talkReplys.Add(this.replyChoiceDatePrefab.Clone(this.replyChoiceView.transform).GetComponent<SeaTalkReply>());
        Future<GameObject> f;
        ItemIcon itemIcon;
        IEnumerator e;
        if (messageTypeId == TalkMessageType.SendItem)
        {
          ((Component) this.replyItem).gameObject.SetActive(true);
          f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
          e = f.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          itemIcon = f.Result.Clone(this.replyIconParent).GetComponent<ItemIcon>();
          if (lastInfo.playerTalkMessage.condition_id.HasValue)
          {
            int? conditionId = lastInfo.playerTalkMessage.condition_id;
            int num = 0;
            if (!(conditionId.GetValueOrDefault() == num & conditionId.HasValue))
            {
              int reward_id = lastInfo.playerTalkMessage.condition_id.Value;
              GearGear gear = (GearGear) null;
              MasterData.GearGear.TryGetValue(reward_id, out gear);
              if (gear != null)
              {
                e = itemIcon.InitByGear(gear, gear.GetElement());
                while (e.MoveNext())
                  yield return e.Current;
                e = (IEnumerator) null;
                this.ReplyGiftUpdate(itemIcon, reward_id);
                itemIcon.onClick = (Action<ItemIcon>) (_ => this.OnGiftListScenePopup(gear));
              }
              else
                this.ReplyGiftUpdate(itemIcon, reward_id);
            }
          }
          f = (Future<GameObject>) null;
          itemIcon = (ItemIcon) null;
        }
        else if (messageTypeId == TalkMessageType.Question)
        {
          if (((IEnumerable<CallMessage>) MasterData.CallMessageList).Where<CallMessage>((Func<CallMessage, bool>) (x => x.message_set_id == lastCallMessage.message_set_id)).Any<CallMessage>((Func<CallMessage, bool>) (x => x.message_type_id == 8)))
          {
            ((Component) this.replyItem).gameObject.SetActive(true);
            f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
            e = f.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            itemIcon = f.Result.Clone(this.replyIconParent).GetComponent<ItemIcon>();
            itemIcon.isButtonActive = false;
            GearGear gear = (GearGear) null;
            MasterData.GearGear.TryGetValue(14999999, out gear);
            e = itemIcon.InitByGear(gear, gear.GetElement());
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            this.ReplyProposeItemUpdate(itemIcon);
            f = (Future<GameObject>) null;
            itemIcon = (ItemIcon) null;
          }
          else
            ((Component) this.replyItem).gameObject.SetActive(false);
        }
        else
          ((Component) this.replyItem).gameObject.SetActive(false);
      }
    }
  }

  private void ReplyGiftUpdate(ItemIcon itemIcon, int gear_id)
  {
    PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == gear_id));
    if (playerMaterialGear == (PlayerMaterialGear) null || playerMaterialGear.quantity <= 0)
    {
      itemIcon.Gray = true;
      this.replyLabel.text = "贈り物が足りない";
      ((UIButtonColor) this.talkReplys.First<SeaTalkReply>((Func<SeaTalkReply, bool>) (x => x.CallMessage.message_type_id == 6)).replyButton).isEnabled = false;
    }
    else
    {
      itemIcon.Gray = false;
      this.replyLabel.text = "贈り物を渡す";
      ((UIButtonColor) this.talkReplys.First<SeaTalkReply>((Func<SeaTalkReply, bool>) (x => x.CallMessage.message_type_id == 6)).replyButton).isEnabled = true;
    }
  }

  private void OnGiftListScenePopup(GearGear gear) => Sea030GiftListScene.ChangeScene(true, gear);

  private void ReplyProposeItemUpdate(ItemIcon itemIcon)
  {
    PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == 14999999));
    if (playerMaterialGear == (PlayerMaterialGear) null || playerMaterialGear.quantity <= 0)
    {
      itemIcon.Gray = true;
      this.replyLabel.text = "誓約締結アイテムが足りない";
      ((UIButtonColor) this.talkReplys[0].replyButton).isEnabled = false;
    }
    else
    {
      itemIcon.Gray = false;
      this.replyLabel.text = "誓約締結アイテムを消費する";
      ((UIButtonColor) this.talkReplys[0].replyButton).isEnabled = true;
    }
  }

  private void OnTalkReply(CallMessage callMessage)
  {
    this.StartCoroutine(this.IOnTalkReply(callMessage));
  }

  private IEnumerator IOnTalkReply(CallMessage selectCallMessage)
  {
    SeaTalkMessageInfo info = this.infos[this.infos.Count - 1];
    CallMessage callMessage = info.callMessage;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2);
    IEnumerator e1;
    if (callMessage.message_type_id == 5 && selectCallMessage.message_type_id == 6)
    {
      Future<WebAPI.Response.SeaTalkGift> api = WebAPI.SeaTalkGift(info.playerTalkMessage.condition_id.Value, this.talkUnitInfo.unit.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      }));
      e1 = api.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.SeaTalkGift result = api.Result;
      this.talkMessageResponse.missions = result.missions;
      this.addPlayerTalkMessages = result.messages;
      for (int index = 0; index < Singleton<NGGameDataManager>.GetInstance().callLetter.Length; ++index)
      {
        if (Singleton<NGGameDataManager>.GetInstance().callLetter[index].same_character_id == result.player_call_letters[0].same_character_id)
          Singleton<NGGameDataManager>.GetInstance().callLetter[index] = result.player_call_letters[0];
      }
      this.SetMissionsAndBatch(result.missions);
      this.UpdateMissionProgress();
      api = (Future<WebAPI.Response.SeaTalkGift>) null;
    }
    else
    {
      Future<WebAPI.Response.SeaTalkReply> api = WebAPI.SeaTalkReply(selectCallMessage.ID, this.talkUnitInfo.unit.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      }));
      e1 = api.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.SeaTalkReply result = api.Result;
      this.addPlayerTalkMessages = result.messages;
      for (int index = 0; index < Singleton<NGGameDataManager>.GetInstance().callLetter.Length; ++index)
      {
        if (Singleton<NGGameDataManager>.GetInstance().callLetter[index].same_character_id == result.player_call_letters[0].same_character_id)
          Singleton<NGGameDataManager>.GetInstance().callLetter[index] = result.player_call_letters[0];
      }
      api = (Future<WebAPI.Response.SeaTalkReply>) null;
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    SeaTalkMessageMenu.SeaTalkPartnerRefresh();
    this.addPlayerTalkMessages = ((IEnumerable<PlayerTalkMessage>) this.addPlayerTalkMessages).OrderBy<PlayerTalkMessage, int>((Func<PlayerTalkMessage, int>) (x => x.player_message_id)).ToArray<PlayerTalkMessage>();
    this.OnReplyClose();
    this.SetReplyButton(false);
    yield return (object) this.AddMessageView();
    yield return (object) this.Conclusion();
  }

  private void AddInfos(PlayerTalkMessage[] playerTalkMessages, List<SeaTalkMessageInfo> addInfos)
  {
    foreach (SeaTalkMessageInfo info in this.infos)
    {
      info.LimitTimeAnimation = false;
      if (info.MessageViewType == TalkMessageViewType.Date)
        info.UpdateDate();
    }
    PlayerTalkMessage playerTalkMessage1 = this.infos[this.infos.Count - 1].playerTalkMessage;
    this.tempMessage.SetActive(true);
    for (int index = 0; index < playerTalkMessages.Length; ++index)
    {
      PlayerTalkMessage playerTalkMessage2 = playerTalkMessages[index];
      SeaTalkMessageInfo seaTalkMessageInfo = new SeaTalkMessageInfo();
      if (playerTalkMessage1 != null && playerTalkMessage1.created_at.Date < playerTalkMessage2.created_at.Date)
      {
        seaTalkMessageInfo.InitDate(playerTalkMessage2);
        --index;
        playerTalkMessage1 = (PlayerTalkMessage) null;
      }
      else
      {
        seaTalkMessageInfo.Init(playerTalkMessage2, this.talkUnitInfo, this.tempMessageLabel);
        playerTalkMessage1 = playerTalkMessage2;
      }
      addInfos.Add(seaTalkMessageInfo);
    }
    this.tempMessage.gameObject.SetActive(false);
    SeaTalkMessageInfo addInfo = addInfos[addInfos.Count - 1];
    if (addInfo.playerTalkMessage.expire_at.HasValue)
      addInfo.LimitTimeAnimation = true;
    float initY = Mathf.Abs(this.tailPlaceholder.localPosition.y) + addInfos[0].UnderHeight / 2f;
    this.SetInfoPostionY(addInfos, initY);
  }

  private IEnumerator AddInfosAnimation(List<SeaTalkMessageInfo> addInfos)
  {
    this.isBlockEventForAnimation = true;
    float y = Mathf.Abs(this.tailPlaceholder.localPosition.y);
    for (int i = 0; i < addInfos.Count; ++i)
    {
      SeaTalkMessageInfo info = addInfos[i];
      y += info.UnderHeight;
      this.infos.Add(info);
      this.talkMessageResponse.last_index = this.infos.Count - 1;
      this.tailPlaceholder.localPosition = new Vector3(0.0f, -y, 0.0f);
      if (info.MessageViewType == TalkMessageViewType.Date || info.MessageViewType == TalkMessageViewType.Gift)
      {
        this.scrollView.ResetPosition();
        this.SetScrollSettings(false);
        this.OnMove(((Component) this.scrollView).GetComponent<UIPanel>());
      }
      else
      {
        if (info.MessageViewType == TalkMessageViewType.Partner)
        {
          info.MessageViewType = TalkMessageViewType.Writing;
          if (Object.op_Inequality((Object) this.writing, (Object) null))
            ((Component) this.writing).gameObject.SetActive(true);
        }
        this.scrollView.ResetPosition();
        this.SetScrollSettings(false);
        this.OnMove(((Component) this.scrollView).GetComponent<UIPanel>());
        if (info.MessageViewType == TalkMessageViewType.Player)
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1074");
        yield return (object) new WaitForSeconds((float) info.callMessage.view_wait_time);
        if (info.MessageViewType == TalkMessageViewType.Writing)
        {
          ((Component) this.writing).gameObject.SetActive(false);
          info.MessageViewType = TalkMessageViewType.Partner;
          this.scrollView.ResetPosition();
          this.SetScrollSettings(false);
          this.OnMove(((Component) this.scrollView).GetComponent<UIPanel>());
        }
        if (info.MessageViewType == TalkMessageViewType.Partner)
        {
          if (info.IsBadMood)
            Singleton<NGSoundManager>.GetInstance().playSE("SE_1075");
          else
            Singleton<NGSoundManager>.GetInstance().playSE("SE_1073");
        }
        info = (SeaTalkMessageInfo) null;
      }
    }
    this.isBlockEventForAnimation = false;
  }

  private void UpdateReplyChoiceView()
  {
    if (this.talkReplys.Count <= 0)
    {
      this.replyTweenY = 0.0f;
    }
    else
    {
      this.replyTweenY = 18f;
      Transform transform = this.replyChoiceView.transform;
      float num1 = transform.localPosition.y - (float) (((double) ((UIWidget) this.replyMonologue).height + 20.0) / 2.0);
      ((Component) this.replyMonologue).transform.localPosition = new Vector3(transform.localPosition.x, num1, transform.localPosition.z);
      this.replyTweenY += (float) ((UIWidget) this.replyMonologue).height + 20f;
      float num2 = num1 - (float) (((double) ((UIWidget) this.replyMonologue).height + 20.0) / 2.0);
      foreach (SeaTalkReply talkReply in this.talkReplys)
      {
        float height = talkReply.GetHeight();
        ((Component) talkReply).transform.localPosition = new Vector3(0.0f, num2 - height / 2f, 0.0f);
        float num3 = height + 20f;
        num2 -= num3;
        this.replyTweenY += num3;
      }
      if (this.replyIconParent.childCount > 0)
      {
        this.replyItem.localPosition = new Vector3(0.0f, (float) ((double) num2 - 68.0 - 20.0), 0.0f);
        this.replyTweenY += 156f;
      }
      this.replyTweenY += 23f;
      UISprite replyChoiceViewBase = this.replyChoiceViewBase;
      ((UIWidget) replyChoiceViewBase).height = ((UIWidget) replyChoiceViewBase).height + Mathf.RoundToInt(this.replyTweenY);
    }
  }

  public IEnumerator Conclusion()
  {
    SeaTalkMessageMenu seaTalkMessageMenu = this;
    CallMessage callMessage = seaTalkMessageMenu.infos[seaTalkMessageMenu.infos.Count - 1].callMessage;
    if (callMessage != null && callMessage.message_type_id == 8 && !((IEnumerable<PlayerTalkPartner>) seaTalkMessageMenu.talkPartnerResponse.partners).Any<PlayerTalkPartner>((Func<PlayerTalkPartner, bool>) (x => x.letter.call_status == 3)))
    {
      seaTalkMessageMenu.isBlockEventForAnimation = true;
      yield return (object) new WaitForSeconds(3f);
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2);
      Future<WebAPI.Response.SeaCallConclusion> api = WebAPI.SeaCallConclusion(seaTalkMessageMenu.talkUnitInfo.unit.same_character_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = api.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.SeaCallConclusion result = api.Result;
      Singleton<NGGameDataManager>.GetInstance().callLetter = api.Result.player_call_letters;
      // ISSUE: reference to a compiler-generated method
      seaTalkMessageMenu.playerCallLetter = ((IEnumerable<PlayerCallLetter>) api.Result.player_call_letters).First<PlayerCallLetter>(new Func<PlayerCallLetter, bool>(seaTalkMessageMenu.\u003CConclusion\u003Eb__82_2));
      seaTalkMessageMenu.addPlayerTalkMessages = ((IEnumerable<PlayerTalkMessage>) result.messages).OrderBy<PlayerTalkMessage, int>((Func<PlayerTalkMessage, int>) (x => x.player_message_id)).ToArray<PlayerTalkMessage>();
      SeaTalkMessageMenu.SeaTalkPartnerRefresh();
      Singleton<NGSceneManager>.GetInstance().changeScene("sea030_CallSkillRelease", true, (object) seaTalkMessageMenu.talkUnitInfo.unit.same_character_id);
      seaTalkMessageMenu.IsPush = false;
      seaTalkMessageMenu.isBlockEventForAnimation = false;
    }
  }

  private IEnumerator LoadResource()
  {
    Future<GameObject> f = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIconPrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_himeTalk_Message_2Line").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.messagePartnerPrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_myTalk_Message_1Line").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.messagePlayerPrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_Talk_date").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.messageDatePrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_Item_Message").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.messageGiftPrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_himeTalk_Message_Writing").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.messageWritingPrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_Reply").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.replyChoicePrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_Give").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.replyGivePrefab = f.Result;
    f = new ResourceObject("Prefabs/sea030_talk/dir_Call").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.replyChoiceDatePrefab = f.Result;
  }

  private void OnMove(UIPanel scrollViewPanel)
  {
    Vector3[] worldCorners = ((UIRect) scrollViewPanel).worldCorners;
    for (int index = 0; index < 4; ++index)
    {
      Vector3 vector3_1 = worldCorners[index];
      Vector3 vector3_2 = ((Component) scrollViewPanel).transform.InverseTransformPoint(vector3_1);
      worldCorners[index] = vector3_2;
    }
    float y1 = worldCorners[2].y;
    float y2 = worldCorners[0].y;
    BetterList<int> betterList = new BetterList<int>();
    for (int index = 0; index < this.infos.Count; ++index)
    {
      if ((double) this.infos[index].PositionY <= (double) y1 && (double) this.infos[index].PositionY >= (double) y2)
        betterList.Add(index);
    }
    int num1 = -1;
    int num2 = -1;
    int num3 = -1;
    int num4 = -1;
    bool flag = false;
    foreach (int index1 in betterList)
    {
      SeaTalkMessageInfo info = this.infos[index1];
      int index2 = -1;
      switch (info.MessageViewType)
      {
        case TalkMessageViewType.Partner:
          ++num1;
          index2 = num1;
          break;
        case TalkMessageViewType.Player:
          ++num2;
          index2 = num2;
          break;
        case TalkMessageViewType.Date:
          ++num3;
          index2 = num3;
          break;
        case TalkMessageViewType.Gift:
          ++num4;
          index2 = num4;
          break;
        case TalkMessageViewType.UnreadLine:
          flag = true;
          break;
      }
      if (this.IsClone(index2, info))
        this.MessageClone(info);
      SeaTalkMessage messageComponent = this.GetMessageComponent(index2, info);
      messageComponent.BeforeInfo = messageComponent.Info;
      messageComponent.Info = info;
      messageComponent.UpdateDate();
      Transform message = this.GetMessage(index2, info);
      message.localPosition = new Vector3(0.0f, info.PositionY, 0.0f);
      ((Component) message).gameObject.SetActive(true);
    }
    for (int index = num1 + 1; index < this.partnerMessages.Count; ++index)
      ((Component) this.partnerMessages[index]).gameObject.SetActive(false);
    for (int index = num2 + 1; index < this.playerMessages.Count; ++index)
      ((Component) this.playerMessages[index]).gameObject.SetActive(false);
    for (int index = num3 + 1; index < this.dateMessages.Count; ++index)
      ((Component) this.dateMessages[index]).gameObject.SetActive(false);
    for (int index = num4 + 1; index < this.giftMessages.Count; ++index)
      ((Component) this.giftMessages[index]).gameObject.SetActive(false);
    if (flag)
      return;
    ((Component) this.unreadLine).gameObject.SetActive(false);
  }

  private bool IsClone(int index, SeaTalkMessageInfo info)
  {
    switch (info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        if (index >= this.partnerMessages.Count)
          return true;
        break;
      case TalkMessageViewType.Player:
        if (index >= this.playerMessages.Count)
          return true;
        break;
      case TalkMessageViewType.Date:
        if (index >= this.dateMessages.Count)
          return true;
        break;
      case TalkMessageViewType.Gift:
        if (index >= this.giftMessages.Count)
          return true;
        break;
      case TalkMessageViewType.Writing:
        if (Object.op_Equality((Object) this.writing, (Object) null))
          return true;
        break;
      case TalkMessageViewType.UnreadLine:
        return false;
      default:
        Debug.LogError((object) "想定していないTalkMessageViewTypeです");
        break;
    }
    return false;
  }

  private void MessageClone(SeaTalkMessageInfo info)
  {
    GameObject gameObject1 = (GameObject) null;
    switch (info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        gameObject1 = this.messagePartnerPrefab;
        break;
      case TalkMessageViewType.Player:
        gameObject1 = this.messagePlayerPrefab;
        break;
      case TalkMessageViewType.Date:
        gameObject1 = this.messageDatePrefab;
        break;
      case TalkMessageViewType.Gift:
        gameObject1 = this.messageGiftPrefab;
        break;
      case TalkMessageViewType.Writing:
        gameObject1 = this.messageWritingPrefab;
        break;
      default:
        Debug.LogError((object) "想定していないTalkMessageViewTypeです");
        break;
    }
    GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject1);
    gameObject2.transform.parent = this.t_scrollView;
    gameObject2.transform.localScale = Vector3.one;
    gameObject2.transform.localPosition = Vector3.zero;
    gameObject2.transform.localRotation = Quaternion.identity;
    SeaTalkMessage component = gameObject2.GetComponent<SeaTalkMessage>();
    switch (info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        this.partnerMessages.Add(gameObject2.transform);
        this.partnerMessagesComponents.Add(component);
        component.BaseUnitIcon = this.unitIconPrefab;
        break;
      case TalkMessageViewType.Player:
        this.playerMessages.Add(gameObject2.transform);
        this.playerMessagesComponents.Add(component);
        break;
      case TalkMessageViewType.Date:
        this.dateMessages.Add(gameObject2.transform);
        this.dateMessagesComponents.Add(component);
        break;
      case TalkMessageViewType.Gift:
        this.giftMessages.Add(gameObject2.transform);
        this.giftMessagesComponents.Add(component);
        break;
      case TalkMessageViewType.Writing:
        this.writing = gameObject2.transform;
        this.writingComponent = component;
        component.BaseUnitIcon = this.unitIconPrefab;
        break;
      default:
        Debug.LogError((object) "想定していないSeaTalkMessageInfoです");
        break;
    }
  }

  private Transform GetMessage(int index, SeaTalkMessageInfo info)
  {
    switch (info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        return this.partnerMessages[index];
      case TalkMessageViewType.Player:
        return this.playerMessages[index];
      case TalkMessageViewType.Date:
        return this.dateMessages[index];
      case TalkMessageViewType.Gift:
        return this.giftMessages[index];
      case TalkMessageViewType.Writing:
        return this.writing;
      case TalkMessageViewType.UnreadLine:
        return this.unreadLine;
      default:
        Debug.LogError((object) "想定していないSeaTalkMessageInfoです");
        return (Transform) null;
    }
  }

  private SeaTalkMessage GetMessageComponent(int index, SeaTalkMessageInfo info)
  {
    switch (info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        return this.partnerMessagesComponents[index];
      case TalkMessageViewType.Player:
        return this.playerMessagesComponents[index];
      case TalkMessageViewType.Date:
        return this.dateMessagesComponents[index];
      case TalkMessageViewType.Gift:
        return this.giftMessagesComponents[index];
      case TalkMessageViewType.Writing:
        return this.writingComponent;
      case TalkMessageViewType.UnreadLine:
        return this.unreadComponent;
      default:
        Debug.LogError((object) "想定していないSeaTalkMessageInfoです");
        return (SeaTalkMessage) null;
    }
  }

  public void OnMenu()
  {
    if (this.isBlockEventForAnimation)
      return;
    Singleton<CommonRoot>.GetInstance().GetSeaHeaderComponent().OpenMenuForTalk();
  }

  public void OnMissionProgress()
  {
    if (this.isBlockEventForAnimation)
      return;
    this.StartCoroutine(this.OpenMissionPopup());
  }

  public IEnumerator OpenMissionPopup()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/sea030_PledgeMission/sea030_PledgeMission").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = f.Result;
    popup = Singleton<PopupManager>.GetInstance().open(popup, clip: "SE_1044");
    popup.SetActive(false);
    yield return (object) popup.GetComponent<SeaTalkMission>().Init(this.talkUnitInfo, this.talkMessageResponse.missions);
    popup.SetActive(true);
  }

  public void OnItem()
  {
    if (this.isBlockEventForAnimation)
      return;
    Sea030GiftListScene.ChangeScene(true, (GearGear) null);
  }

  public void OnReply()
  {
    if (this.isBlockEventForAnimation)
      return;
    TweenPosition component = this.bottomfit.GetComponent<TweenPosition>();
    component.to = new Vector3(0.0f, component.from.y + this.replyTweenY, 0.0f);
    NGTween.playTween((UITweener) component);
    this.replyCloseButton.SetActive(true);
    this.SetReplyButton(false);
    ((Behaviour) this.replyCloseScrollView).enabled = true;
    ((Behaviour) this.dragScrollView).enabled = false;
  }

  public void OnReplyClose()
  {
    if (this.isBlockEventForAnimation)
      return;
    NGTween.playTween((UITweener) this.bottomfit.GetComponent<TweenPosition>(), true);
    this.replyCloseButton.SetActive(false);
    this.SetReplyButton(true);
    ((Behaviour) this.replyCloseScrollView).enabled = false;
    ((Behaviour) this.dragScrollView).enabled = true;
  }

  public void onClickedCallDestruction()
  {
    if (this.isBlockEventForAnimation)
      return;
    this.StartCoroutine(this.OpenCallDestructionPopup());
  }

  private IEnumerator OpenCallDestructionPopup()
  {
    SeaTalkMessageMenu seaTalkMessageMenu = this;
    Future<GameObject> f = new ResourceObject("Prefabs/sea030_talk/popup_030_sea_PledgeCancell__anim_fade").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = Singleton<PopupManager>.GetInstance().open(f.Result, maskAlpha: 0.7f, fadeOutFlag: true);
    popup.SetActive(false);
    yield return (object) popup.GetComponent<SeaTalkDestruction>().Init(seaTalkMessageMenu.playerCallLetter, seaTalkMessageMenu.talkUnitInfo, new Action(seaTalkMessageMenu.IbtnBack));
    popup.SetActive(true);
  }

  public void onClickedHelp()
  {
    if (this.isBlockEventForAnimation)
      return;
    HelpCategory helpCategory = (HelpCategory) null ?? Array.Find<HelpCategory>(MasterData.HelpCategoryList, (Predicate<HelpCategory>) (x => x.ID == 35));
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (Singleton<NGSceneManager>.GetInstance().sceneName == "help015_2")
      Help0152Scene.ChangeScene(false, helpCategory);
    else
      Help0152Scene.ChangeScene(true, helpCategory);
  }

  public override void onBackButton() => this.IbtnBack();

  private void IbtnBack()
  {
    if (this.isBlockEventForAnimation || this.IsPushAndSet())
      return;
    SeaTalkMessageInfo info = this.infos[this.infos.Count - 1];
    if (info.playerTalkMessage.expire_at.HasValue && ServerTime.NowAppTimeAddDelta() > info.playerTalkMessage.expire_at.Value)
      SeaTalkMessageMenu.SeaTalkPartnerRefresh();
    if (Object.op_Inequality((Object) this.seaHomeMenu, (Object) null))
    {
      this.seaHomeMenu.isReturnSelectSpot = true;
      this.seaHomeMenu.isSelectedSpot = false;
      this.seaHomeMenu.ResetParticalObj();
    }
    this.backScene();
  }
}
