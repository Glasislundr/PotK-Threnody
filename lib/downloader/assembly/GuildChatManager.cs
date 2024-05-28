// Decompiled with JetBrains decompiler
// Type: GuildChatManager
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
public class GuildChatManager : BackButtonMenuBase
{
  private const string GUILD_CHAT_STAMP_ATLAS_PATH = "GUI/chat_stamp_group{0}/chat_stamp_group{1}_prefab";
  private const string GUILD_CHAT_STAMP_SPRITE_NAME = "slc_stamp_id_{0}.png__GUI__chat_stamp_group{1}__chat_stamp_group{2}_prefab";
  private const string GUILD_CHAT_STAMP_DEFAULT_WHITE_SPRITE_NAME = "slc_white.png__GUI__common__common_prefab";
  private const int GUILD_RAID_LOG_DECK_VIEW_COUNT = 5;
  [SerializeField]
  private GameObject Top;
  [SerializeField]
  private GameObject TopFit;
  [SerializeField]
  private GameObject Middle;
  [SerializeField]
  private GameObject ScrollContainer;
  [SerializeField]
  private GameObject MiniLoad;
  [SerializeField]
  private GameObject Bottom;
  [SerializeField]
  private GameObject BottomFit;
  [SerializeField]
  private GameObject ChatSimple;
  [SerializeField]
  private GameObject DropShadowBlack;
  [SerializeField]
  private GameObject newMessageContainer;
  [SerializeField]
  private GameObject maintenanceBlock;
  [SerializeField]
  private TweenAlpha topFitTweenAlpha;
  [SerializeField]
  private TweenAlpha scrollContainerTweenAlpha;
  [SerializeField]
  private TweenAlpha bottomFitTweenAlpha;
  [SerializeField]
  private TweenPosition topFitTweenPosition;
  [SerializeField]
  private TweenPosition bottomFitTweenPosition;
  [SerializeField]
  private TweenColor dropShadowBlackTweenColor;
  [SerializeField]
  private UIButton[] messageFilterTypeIdleButtons;
  [SerializeField]
  private UIButton[] messageFilterTypeHighlightButtons;
  [SerializeField]
  private UIButton BBSButton;
  [SerializeField]
  private UIButton sendMessageButton;
  [SerializeField]
  private UIButton stampOpenButton;
  [SerializeField]
  private GameObject dir_NotEntry;
  [SerializeField]
  private GameObject[] notEntryMask;
  [SerializeField]
  private UIInput chatMessageInput;
  [SerializeField]
  private UILabel chatMessageLabel;
  [SerializeField]
  public GuildChatSimpleListController simpleListController;
  [SerializeField]
  public GuildChatDetailedListController detailedListController;
  [SerializeField]
  public GuildChatStampSelectViewController stampSelectViewController;
  private readonly float[] messageUpdateIntervals = new float[2]
  {
    15f,
    30f
  };
  private GuildChatManager.IntervalStateMode IntervalState;
  private const float remainedMessageUpdateInterval = 0.5f;
  private const float messageUpdateRetryInterval = 1f;
  private const int maxSavedDataCount = 1000;
  private Coroutine updateLatestMessageRepeatingCoroutine;
  private Coroutine updateEarliestMessageCoroutine;
  private Coroutine setDisplayingMessageTypeCoroutine;
  private Coroutine updateLatestMessageNowCoroutine;
  private Coroutine refreshMessageIconCoroutine;
  private bool isUpdatingMessage;
  private bool isNotEntry;
  private bool isReceivedMessage;
  private bool shouldScrollToBottom;
  private GuildChatManager.GuildChatStatus currentGuildChatStatus = GuildChatManager.GuildChatStatus.NotOpened;
  private bool isGuildChatPaused;
  private GuildChatManager.GuildChatMessageFilterType currentDisplayingMessageFilterType;
  private List<GuildChatMessageData> allMessageDataList = new List<GuildChatMessageData>();
  private List<GuildChatMessageData> displayingMessageList = new List<GuildChatMessageData>();
  public bool isFirstTimeOpeningDetailedView = true;
  private string messageBackup;
  [HideInInspector]
  public GameObject simpleMessageItemPrefab;
  [HideInInspector]
  public GameObject memberLogItemPrefab;
  [HideInInspector]
  public GameObject memberChatItemPrefab;
  [HideInInspector]
  public GameObject playerChatItemPrefab;
  [HideInInspector]
  public GameObject playerLogItemPrefab;
  [HideInInspector]
  public GameObject systemLogItemPrefab;
  [HideInInspector]
  public GameObject deletedMessageItemPrefab;
  [HideInInspector]
  public GameObject memberStampItemPrefab;
  [HideInInspector]
  public GameObject playerStampItemPrefab;
  [HideInInspector]
  public GameObject stampSelectItemPrefab;
  [HideInInspector]
  public GameObject stampGroupSelectItemPrefab;
  [HideInInspector]
  public GameObject guildRaidLogPrefab;
  [SerializeField]
  private UIAtlas commonAtlas;
  public static bool scrollWheel_flg = false;
  public bool isOpenTapDisabled;
  private static Dictionary<int, GuildChatManager.SpriteCache> spriteCache = new Dictionary<int, GuildChatManager.SpriteCache>();
  private static Dictionary<int, UIAtlas> stampAtlasCache = new Dictionary<int, UIAtlas>();

  public GuildChatManager.GuildChatStatus GetCurrentGuildChatStatus()
  {
    return this.currentGuildChatStatus;
  }

  public bool GetGuildChatPaused() => this.isGuildChatPaused;

  public bool isDetailViewOpened
  {
    get => this.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView;
  }

  private void Awake() => ((UIButtonColor) this.sendMessageButton).isEnabled = false;

  private void Start()
  {
    this.chatMessageInput.caretColor = Color.black;
    ((Component) this.chatMessageInput).transform.localPosition = Vector2.op_Implicit(new Vector2(176f, 80f));
    ((Component) this.chatMessageInput).GetComponent<UIWidget>().SetDimensions(416, 110);
    this.chatMessageLabel.maxLineCount = 3;
    this.chatMessageLabel.overflowMethod = (UILabel.Overflow) 0;
    ((Component) this.chatMessageLabel).GetComponent<UIWidget>().SetDimensions(382, 99);
    ((Component) this.sendMessageButton).transform.localPosition = new Vector3(252f, 77f, 0.0f);
    ((Component) this.stampOpenButton).transform.localPosition = new Vector3(-279f, 77f, 0.0f);
  }

  private new void Update()
  {
    base.Update();
    if (!((UIButtonColor) this.sendMessageButton).isEnabled || !Input.GetKeyDown((KeyCode) 271) && !Input.GetKey((KeyCode) 13))
      return;
    this.OnSendMessageButtonClicked();
  }

  private IEnumerator InitializeMessageItemPrefabs()
  {
    Future<GameObject> prefab = Res.Prefabs.guild_chat.guild_chat_simple_list.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.simpleMessageItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_message_player.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerChatItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_log_player.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerLogItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_message_member.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.memberChatItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_log_member.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.memberLogItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_log_system.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.systemLogItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_deleted.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.deletedMessageItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_stamp_member.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.memberStampItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_stamp_player.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerStampItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_list_stamp_choice.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.stampSelectItemPrefab = prefab.Result;
    prefab = Res.Prefabs.guild_chat.guild_chat_group_ibtn_stamp_base.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.stampGroupSelectItemPrefab = prefab.Result;
    prefab = new ResourceObject("Prefabs/guild_chat/guild_chat_list_raidlog_Info").Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildRaidLogPrefab = prefab.Result;
  }

  public void OnChatOpenButtonClicked() => this.OpenDetailedChat();

  public void OpenDetailedChat()
  {
    if (this.isOpenTapDisabled)
      return;
    GuildChatManager.scrollWheel_flg = true;
    if (Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>(), (Object) null) && ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>().IsPushAndSet())
      return;
    if (Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Closed)
      ((Component) this).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().headerChat.DisableNewIcon();
    if (!PlayerAffiliation.Current.isGuildMember())
    {
      this.dir_NotEntry.SetActive(true);
      this.ScrollContainer.SetActive(false);
      for (int index = 0; index < this.notEntryMask.Length; ++index)
        this.notEntryMask[index].SetActive(true);
    }
    else
    {
      this.dir_NotEntry.SetActive(false);
      this.ScrollContainer.SetActive(true);
      for (int index = 0; index < this.notEntryMask.Length; ++index)
        this.notEntryMask[index].SetActive(false);
    }
    this.Top.SetActive(true);
    this.Middle.SetActive(true);
    this.DropShadowBlack.SetActive(true);
    this.BottomFit.SetActive(true);
    this.newMessageContainer.SetActive(true);
    if (Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Closed)
      this.ChatSimple.SetActive(false);
    this.PlayGuildChatAnimation(GuildChatManager.GuildChatAnimationType.Simple_To_Detailed, new EventDelegate.Callback(this.OnOpenDetailedChatFinishDelegate));
  }

  private void OnOpenDetailedChatFinishDelegate()
  {
    this.ChatSimple.SetActive(false);
    this.currentGuildChatStatus = GuildChatManager.GuildChatStatus.DetailedView;
    Debug.Log((object) "<color=green>Status is set to DetailedView.</color>");
  }

  public void OnGuildSearchButton()
  {
    if (Singleton<NGSceneManager>.GetInstance().sceneName != "guild028_1_1")
      this.isNotEntry = true;
    this.CloseDetailedChat();
  }

  public void OnCloseButtonClicked() => this.CloseDetailedChat();

  public void SimpleChatActive() => this.ChatSimple.SetActive(true);

  public void CloseDetailedChat()
  {
    this.dir_NotEntry.SetActive(false);
    for (int index = 0; index < this.notEntryMask.Length; ++index)
      this.notEntryMask[index].SetActive(false);
    if (Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Object.op_Inequality((Object) ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>(), (Object) null))
      ((Component) Singleton<NGSceneManager>.GetInstance().sceneBase).GetComponent<NGMenuBase>().IsPush = false;
    GuildChatManager.scrollWheel_flg = false;
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.DetailedView)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      this.stampSelectViewController.CloseStampSelectView();
      this.newMessageContainer.SetActive(false);
      if (Object.op_Inequality((Object) Singleton<NGSceneManager>.GetInstance().sceneBase, (Object) null) && Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus != NGSceneBase.GuildChatDisplayingStatus.Closed)
        this.ChatSimple.SetActive(true);
      this.PlayGuildChatAnimation(GuildChatManager.GuildChatAnimationType.Detailed_To_Simple, new EventDelegate.Callback(this.OnCloseDetailedChatFinishDelegate));
    }
  }

  private void OnCloseDetailedChatFinishDelegate()
  {
    this.Top.SetActive(false);
    this.Middle.SetActive(false);
    this.DropShadowBlack.SetActive(false);
    this.BottomFit.SetActive(false);
    if (Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Closed)
    {
      this.ChatSimple.SetActive(true);
      this.CloseGuildChat();
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage")
        Singleton<CommonRoot>.GetInstance().ActiveBaseHomeMenu(true);
    }
    else
      this.currentGuildChatStatus = GuildChatManager.GuildChatStatus.SimpleView;
    if (this.isNotEntry)
    {
      this.isNotEntry = false;
      Guild02811Scene.ChangeScene();
    }
    Debug.Log((object) "<color=green>Status is set to SimpleView.</color>");
  }

  public void OnAllButtonClicked()
  {
    if (this.setDisplayingMessageTypeCoroutine != null || !PlayerAffiliation.Current.isGuildMember())
      return;
    this.setDisplayingMessageTypeCoroutine = this.StartCoroutine(this.SetDisplayingMessageType(GuildChatManager.GuildChatMessageFilterType.All));
  }

  public void OnChatOnlyButtonClicked()
  {
    if (this.setDisplayingMessageTypeCoroutine != null || !PlayerAffiliation.Current.isGuildMember())
      return;
    this.setDisplayingMessageTypeCoroutine = this.StartCoroutine(this.SetDisplayingMessageType(GuildChatManager.GuildChatMessageFilterType.ChatOnly));
  }

  public void OnRaidOnlyButtonClicked()
  {
    if (this.setDisplayingMessageTypeCoroutine != null || !PlayerAffiliation.Current.isGuildMember())
      return;
    this.setDisplayingMessageTypeCoroutine = this.StartCoroutine(this.SetDisplayingMessageType(GuildChatManager.GuildChatMessageFilterType.GuildRaidOnly));
  }

  public void OnLogOnlyButtonClicked()
  {
    if (this.setDisplayingMessageTypeCoroutine != null || !PlayerAffiliation.Current.isGuildMember())
      return;
    this.setDisplayingMessageTypeCoroutine = this.StartCoroutine(this.SetDisplayingMessageType(GuildChatManager.GuildChatMessageFilterType.LogOnly));
  }

  public void OnPullDownMessageListFromTop()
  {
    this.updateEarliestMessageCoroutine = this.StartCoroutine(this.UpdateEarlierMessage());
  }

  public void OnBBSTabButtonClicked()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
      return;
    this.OpenBBSViewerDialog();
  }

  public void OpenBBSViewerDialog()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
      return;
    this.StartCoroutine(this.OpenBBSViewerDialogCoroutine());
  }

  public IEnumerator OpenBBSViewerDialogCoroutine()
  {
    while (this.detailedListController.bbsViewDialogPrefab == null)
      yield return (object) null;
    GameObject prefab = this.detailedListController.bbsViewDialogPrefab.Result.Clone();
    prefab.GetComponent<GuildChatBBSViewerController>().InitializeBBSViewerDialog();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void OnInputChatMessageChange()
  {
    ((UIButtonColor) this.sendMessageButton).isEnabled = !this.chatMessageInput.value.isEmptyOrWhitespace();
  }

  public void OnSendMessageButtonClicked()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
      return;
    this.StartCoroutine(this.SendMessage());
  }

  public void OnStampSelectButtonClicked()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
      return;
    this.stampSelectViewController.OpenStampSelectView();
  }

  public override void onBackButton() => this.OnBackButtonClicked();

  public void OnBackButtonClicked()
  {
    if (this.stampSelectViewController.isStampSelectViewOpened)
    {
      this.stampSelectViewController.CloseStampSelectView();
    }
    else
    {
      if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.DetailedView)
        return;
      this.CloseDetailedChat();
    }
  }

  private IEnumerator SendMessage()
  {
    GuildChatManager guildChatManager = this;
    string log_text = guildChatManager.chatMessageLabel.text.ToConverter().Replace("\n", "");
    if (!(log_text == ""))
    {
      guildChatManager.chatMessageLabel.SetTextLocalize("");
      guildChatManager.chatMessageInput.value = guildChatManager.chatMessageLabel.text;
      guildChatManager.messageBackup = log_text;
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Future<WebAPI.Response.GuildlogWrite> future = WebAPI.SilentGuildlogWrite(log_text, new Action<WebAPI.Response.UserError>(guildChatManager.SendMessageErrorCallback));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (future.Result != null)
      {
        guildChatManager.isReceivedMessage = true;
        guildChatManager.messageBackup = (string) null;
        guildChatManager.shouldScrollToBottom = true;
        if (guildChatManager.updateLatestMessageNowCoroutine == null)
          guildChatManager.updateLatestMessageNowCoroutine = guildChatManager.StartCoroutine(guildChatManager.UpdateLatestMessageNow());
      }
    }
  }

  private void SendMessageErrorCallback(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Code == "GLD011")
      this.StartCoroutine(this.OpenNGWordDialog());
    else if (error.Code == "GLD015")
    {
      this.StartMaintenanceMode();
    }
    else
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }
  }

  private IEnumerator OpenNGWordDialog()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_028_guild_ng_word__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Guild028NgWordPopup>().Initialize((Action) (() => { }));
  }

  public void SendStamp(int stampID) => this.StartCoroutine(this.SendStampCoroutine(stampID));

  private IEnumerator SendStampCoroutine(int stampID)
  {
    GuildChatManager guildChatManager = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.GuildlogSendStamp> future = WebAPI.SilentGuildlogSendStamp(stampID, new Action<WebAPI.Response.UserError>(guildChatManager.SendStampErrorCallback));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (future.Result != null)
    {
      guildChatManager.isReceivedMessage = true;
      guildChatManager.shouldScrollToBottom = true;
      if (guildChatManager.updateLatestMessageNowCoroutine == null)
        guildChatManager.updateLatestMessageNowCoroutine = guildChatManager.StartCoroutine(guildChatManager.UpdateLatestMessageNow());
    }
  }

  private void SendStampErrorCallback(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Code == "GLD015")
    {
      this.StartMaintenanceMode();
    }
    else
    {
      Singleton<PopupManager>.GetInstance().closeAll();
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }
  }

  private IEnumerator UpdateLatestMessageRepeating()
  {
    GuildChatManager guildChatManager = this;
    if (PlayerAffiliation.Current.isGuildMember())
    {
      while (true)
      {
        while (guildChatManager.isUpdatingMessage || guildChatManager.detailedListController.IsScrollViewDragging())
          yield return (object) new WaitForSeconds(1f);
        guildChatManager.isUpdatingMessage = true;
        int remainedLogCount = 0;
        IEnumerator e;
        do
        {
          long num1 = 0;
          if (guildChatManager.allMessageDataList.Count > 0)
            num1 = guildChatManager.allMessageDataList[guildChatManager.allMessageDataList.Count - 1].messageID;
          Singleton<CommonRoot>.GetInstance().loadingMode = 2;
          // ISSUE: reference to a compiler-generated method
          Future<WebAPI.Response.GuildlogAutoupdate> future = WebAPI.SilentGuildlogAutoupdate(num1.ToString(), new Action<WebAPI.Response.UserError>(guildChatManager.\u003CUpdateLatestMessageRepeating\u003Eb__107_0));
          e = future.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          if (future.Result != null)
          {
            GuildLog[] guildLogs = future.Result.guild_logs;
            remainedLogCount = future.Result.remaining_logs_count;
            List<GuildChatMessageData> source = new List<GuildChatMessageData>();
            for (int index = 0; index < guildLogs.Length; ++index)
            {
              GuildChatMessageData guildChatMessageData = new GuildChatMessageData(guildLogs[index]);
              source.Add(guildChatMessageData);
            }
            List<GuildChatMessageData> list = source.OrderBy<GuildChatMessageData, long>((Func<GuildChatMessageData, long>) (data => data.messageID)).ToList<GuildChatMessageData>();
            guildChatManager.allMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) list);
            guildChatManager.setAllMassageIsRaidDeckView();
            List<GuildChatMessageData> deletedOldDataList = new List<GuildChatMessageData>();
            if (guildChatManager.allMessageDataList.Count > 1000)
            {
              int num2 = guildChatManager.allMessageDataList.Count - 1000;
              for (int index = 0; index < num2; ++index)
              {
                deletedOldDataList.Add(guildChatManager.allMessageDataList[0]);
                guildChatManager.allMessageDataList.RemoveAt(0);
              }
            }
            guildChatManager.UpdateDisplayingMessageDataList(guildChatManager.currentDisplayingMessageFilterType);
            guildChatManager.UpdateSimpleMessageItemList(list);
            guildChatManager.AdjustDetailedMessageItemListPlaceholder(guildChatManager.shouldScrollToBottom, list, deletedOldDataList: deletedOldDataList);
            guildChatManager.shouldScrollToBottom = false;
          }
          guildChatManager.SetIntervalState(guildChatManager.isReceivedMessage);
          guildChatManager.isReceivedMessage = false;
          if (remainedLogCount > 0)
            yield return (object) new WaitForSeconds(0.5f);
          future = (Future<WebAPI.Response.GuildlogAutoupdate>) null;
        }
        while (remainedLogCount > 0);
        guildChatManager.isUpdatingMessage = false;
        e = guildChatManager.WaitForUpdateInterval(guildChatManager.IntervalState);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void setAllMassageIsRaidDeckView()
  {
    List<long> longList = new List<long>();
    List<GuildChatMessageData> list = this.allMessageDataList.Where<GuildChatMessageData>((Func<GuildChatMessageData, bool>) (x => x.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog && x.kind_id != 45 && x.kind_id != 46)).OrderByDescending<GuildChatMessageData, DateTime>((Func<GuildChatMessageData, DateTime>) (x => x.sendTime)).ToList<GuildChatMessageData>();
    for (int index = 0; index < 5 && index < list.Count<GuildChatMessageData>(); ++index)
      longList.Add(list[index].messageID);
    foreach (GuildChatMessageData allMessageData in this.allMessageDataList)
      allMessageData.isRaidDeckView = longList.Contains(allMessageData.messageID);
  }

  private IEnumerator WaitForUpdateInterval(GuildChatManager.IntervalStateMode state)
  {
    yield return (object) new WaitForSeconds(this.messageUpdateIntervals[(int) this.IntervalState]);
  }

  private void SetIntervalState(bool isUpdate)
  {
    if (isUpdate)
      this.IntervalState = GuildChatManager.IntervalStateMode.IntervalFirst;
    else
      this.IntervalState = GuildChatManager.IntervalStateMode.IntervalSecond;
  }

  private IEnumerator UpdateLatestMessageNow()
  {
    GuildChatManager guildChatManager = this;
    while (guildChatManager.isUpdatingMessage)
      yield return (object) null;
    guildChatManager.isUpdatingMessage = true;
    int remainedLogCount = 0;
    do
    {
      long num1 = 0;
      if (guildChatManager.allMessageDataList.Count > 0)
        num1 = guildChatManager.allMessageDataList[guildChatManager.allMessageDataList.Count - 1].messageID;
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.GuildlogAutoupdate> future = WebAPI.SilentGuildlogAutoupdate(num1.ToString(), new Action<WebAPI.Response.UserError>(guildChatManager.\u003CUpdateLatestMessageNow\u003Eb__111_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (future.Result != null)
      {
        GuildLog[] guildLogs = future.Result.guild_logs;
        remainedLogCount = future.Result.remaining_logs_count;
        List<GuildChatMessageData> source = new List<GuildChatMessageData>();
        for (int index = 0; index < guildLogs.Length; ++index)
        {
          GuildChatMessageData guildChatMessageData = new GuildChatMessageData(guildLogs[index]);
          source.Add(guildChatMessageData);
        }
        List<GuildChatMessageData> list = source.OrderBy<GuildChatMessageData, long>((Func<GuildChatMessageData, long>) (data => data.messageID)).ToList<GuildChatMessageData>();
        guildChatManager.allMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) list);
        guildChatManager.setAllMassageIsRaidDeckView();
        List<GuildChatMessageData> deletedOldDataList = new List<GuildChatMessageData>();
        if (guildChatManager.allMessageDataList.Count > 1000)
        {
          int num2 = guildChatManager.allMessageDataList.Count - 1000;
          for (int index = 0; index < num2; ++index)
          {
            deletedOldDataList.Add(guildChatManager.allMessageDataList[0]);
            guildChatManager.allMessageDataList.RemoveAt(0);
          }
        }
        guildChatManager.UpdateDisplayingMessageDataList(guildChatManager.currentDisplayingMessageFilterType);
        guildChatManager.UpdateSimpleMessageItemList(list);
        guildChatManager.AdjustDetailedMessageItemListPlaceholder(guildChatManager.shouldScrollToBottom, list, deletedOldDataList: deletedOldDataList);
        guildChatManager.shouldScrollToBottom = false;
      }
      if (remainedLogCount > 0)
        yield return (object) new WaitForSeconds(0.5f);
      future = (Future<WebAPI.Response.GuildlogAutoupdate>) null;
    }
    while (remainedLogCount > 0);
    guildChatManager.isUpdatingMessage = false;
    guildChatManager.updateLatestMessageNowCoroutine = (Coroutine) null;
  }

  private IEnumerator UpdateEarlierMessage()
  {
    GuildChatManager guildChatManager = this;
    if (!guildChatManager.isUpdatingMessage && PlayerAffiliation.Current.isGuildMember())
    {
      guildChatManager.isUpdatingMessage = true;
      guildChatManager.MiniLoad.SetActive(true);
      DateTime startTime = DateTime.Now;
      long num1 = 0;
      if (guildChatManager.allMessageDataList.Count > 0)
        num1 = guildChatManager.allMessageDataList[0].messageID;
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.GuildlogShowPast> future = WebAPI.SilentGuildlogShowPast(num1.ToString(), new Action<WebAPI.Response.UserError>(guildChatManager.\u003CUpdateEarlierMessage\u003Eb__112_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (future.Result != null)
      {
        GuildLog[] guildLogs = future.Result.guild_logs;
        List<GuildChatMessageData> receivedDataList = new List<GuildChatMessageData>();
        for (int index = 0; index < guildLogs.Length; ++index)
          receivedDataList.Add(new GuildChatMessageData(guildLogs[index]));
        receivedDataList = receivedDataList.OrderBy<GuildChatMessageData, long>((Func<GuildChatMessageData, long>) (data => data.messageID)).ToList<GuildChatMessageData>();
        List<GuildChatMessageData> guildChatMessageDataList = new List<GuildChatMessageData>();
        guildChatMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) receivedDataList);
        guildChatMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) guildChatManager.allMessageDataList);
        guildChatManager.allMessageDataList = guildChatMessageDataList;
        List<GuildChatMessageData> deletedMessageDataList = new List<GuildChatMessageData>();
        if (guildChatManager.allMessageDataList.Count > 1000)
        {
          int num2 = guildChatManager.allMessageDataList.Count - 1000;
          for (int index = 0; index < num2; ++index)
          {
            deletedMessageDataList.Add(guildChatManager.allMessageDataList[0]);
            guildChatManager.allMessageDataList.RemoveAt(0);
          }
        }
        guildChatManager.UpdateDisplayingMessageDataList(guildChatManager.currentDisplayingMessageFilterType);
        double num3 = (DateTime.Now - startTime).TotalSeconds;
        if (num3 < 1.0)
          num3 = 1.0 - num3;
        yield return (object) new WaitForSeconds((float) num3);
        guildChatManager.MiniLoad.SetActive(false);
        guildChatManager.detailedListController.StopScrollViewMovement();
        yield return (object) new WaitForSeconds(0.01f);
        guildChatManager.AdjustDetailedMessageItemListPlaceholder(addedOldDataList: receivedDataList, deletedOldDataList: deletedMessageDataList);
        receivedDataList = (List<GuildChatMessageData>) null;
        deletedMessageDataList = (List<GuildChatMessageData>) null;
      }
      guildChatManager.isUpdatingMessage = false;
      guildChatManager.updateEarliestMessageCoroutine = (Coroutine) null;
    }
  }

  public void OpenGuildChat()
  {
    if (!PlayerAffiliation.Current.isGuildMember())
      return;
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.NotOpened)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      this.currentGuildChatStatus = GuildChatManager.GuildChatStatus.SimpleView;
      this.StartCoroutine(this.OpenGuildChatCoroutine());
    }
  }

  public void OpenFooterGuildChat()
  {
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.NotOpened)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      this.currentGuildChatStatus = GuildChatManager.GuildChatStatus.DetailedView;
      this.StartCoroutine(this.OpenDetailedGuildChatCoroutine());
    }
  }

  private IEnumerator OpenGuildChatCoroutine()
  {
    GuildChatManager guildChatManager = this;
    IEnumerator e = guildChatManager.InitializeMessageItemPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guildChatManager.currentGuildChatStatus == GuildChatManager.GuildChatStatus.SimpleView)
    {
      guildChatManager.InitializeGuildChatAnimations();
      guildChatManager.Top.SetActive(false);
      guildChatManager.Middle.SetActive(false);
      guildChatManager.DropShadowBlack.SetActive(false);
      guildChatManager.BottomFit.SetActive(false);
      guildChatManager.newMessageContainer.SetActive(false);
      guildChatManager.ChatSimple.SetActive(true);
      ((Component) guildChatManager).gameObject.SetActive(true);
      guildChatManager.updateLatestMessageRepeatingCoroutine = guildChatManager.StartCoroutine(guildChatManager.UpdateLatestMessageRepeating());
    }
  }

  private IEnumerator OpenDetailedGuildChatCoroutine()
  {
    GuildChatManager guildChatManager = this;
    IEnumerator e = guildChatManager.InitializeMessageItemPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guildChatManager.currentGuildChatStatus == GuildChatManager.GuildChatStatus.DetailedView)
    {
      guildChatManager.updateLatestMessageRepeatingCoroutine = guildChatManager.StartCoroutine(guildChatManager.UpdateLatestMessageRepeating());
      guildChatManager.OpenDetailedChat();
    }
  }

  public void RecoverFromConnectionError(WebError error)
  {
    if (this.currentGuildChatStatus == GuildChatManager.GuildChatStatus.NotOpened || this.isGuildChatPaused)
      return;
    this.isUpdatingMessage = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (error.Request.Path.Contains("/guildlog/autoupdate"))
    {
      Debug.Log((object) "<color=yellow>Recover autoupdate.</color>");
      if (this.updateLatestMessageRepeatingCoroutine == null)
        return;
      this.StopCoroutine(this.updateLatestMessageRepeatingCoroutine);
      this.updateLatestMessageRepeatingCoroutine = this.StartCoroutine(this.UpdateLatestMessageRepeating());
    }
    else
    {
      if (!error.Request.Path.Contains("/guildlog/write"))
        return;
      Debug.Log((object) "<color=yellow>Recover sending message.</color>");
      if (this.messageBackup == null)
        return;
      this.chatMessageLabel.SetTextLocalize(this.messageBackup);
      this.chatMessageInput.value = this.chatMessageLabel.text;
    }
  }

  public void CloseAllGuildPopupDialogs()
  {
    if (this.currentGuildChatStatus == GuildChatManager.GuildChatStatus.NotOpened || this.isGuildChatPaused)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private void InitializeGuildChatAnimations()
  {
    ((UITweener) this.topFitTweenAlpha).tweenFactor = 0.0f;
    ((UITweener) this.scrollContainerTweenAlpha).tweenFactor = 0.0f;
    ((UITweener) this.bottomFitTweenAlpha).tweenFactor = 0.0f;
    ((UITweener) this.topFitTweenPosition).tweenFactor = 0.0f;
    ((UITweener) this.bottomFitTweenPosition).tweenFactor = 0.0f;
    ((UITweener) this.dropShadowBlackTweenColor).tweenFactor = 0.0f;
  }

  public void CloseGuildChat()
  {
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.SimpleView && this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.DetailedView)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      this.currentGuildChatStatus = GuildChatManager.GuildChatStatus.NotOpened;
      this.isGuildChatPaused = false;
      ((Component) this).gameObject.SetActive(false);
      this.isFirstTimeOpeningDetailedView = true;
      this.stampSelectViewController.isFirstTimeOpeningStampSelectView = true;
      this.StopAllGuildChatCoroutines();
      this.isUpdatingMessage = false;
      if (Object.op_Inequality((Object) this.simpleListController, (Object) null))
        this.simpleListController.ClearMessageItemList();
      this.allMessageDataList.Clear();
      this.displayingMessageList.Clear();
    }
  }

  public void StopAllGuildChatCoroutines()
  {
    if (this.updateLatestMessageRepeatingCoroutine != null)
    {
      this.StopCoroutine(this.updateLatestMessageRepeatingCoroutine);
      this.updateLatestMessageRepeatingCoroutine = (Coroutine) null;
    }
    if (this.updateEarliestMessageCoroutine != null)
    {
      this.StopCoroutine(this.updateEarliestMessageCoroutine);
      this.updateEarliestMessageCoroutine = (Coroutine) null;
    }
    if (this.setDisplayingMessageTypeCoroutine != null)
    {
      this.StopCoroutine(this.setDisplayingMessageTypeCoroutine);
      this.setDisplayingMessageTypeCoroutine = (Coroutine) null;
    }
    if (this.updateLatestMessageNowCoroutine != null)
    {
      this.StopCoroutine(this.updateLatestMessageNowCoroutine);
      this.updateLatestMessageNowCoroutine = (Coroutine) null;
    }
    if (this.refreshMessageIconCoroutine != null)
    {
      this.StopCoroutine(this.refreshMessageIconCoroutine);
      this.refreshMessageIconCoroutine = (Coroutine) null;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  protected void StopCoroutine(Coroutine co)
  {
    Singleton<NGSceneManager>.GetInstance().StopCoroutine(co);
  }

  public void PauseAndHideGuildChat()
  {
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.SimpleView && this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.DetailedView)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      if (this.isGuildChatPaused)
        return;
      this.isGuildChatPaused = true;
      this.StartCoroutine(this.PauseAndHideCoroutine());
    }
  }

  private IEnumerator PauseAndHideCoroutine()
  {
    GuildChatManager guildChatManager = this;
    while (guildChatManager.isUpdatingMessage)
      yield return (object) null;
    guildChatManager.StopAllGuildChatCoroutines();
    guildChatManager.isUpdatingMessage = false;
    ((Component) guildChatManager).gameObject.SetActive(false);
  }

  public void ResumeAndShowGuildChat()
  {
    if (this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.SimpleView && this.currentGuildChatStatus != GuildChatManager.GuildChatStatus.DetailedView)
    {
      this.LogGuildChatStatusError();
    }
    else
    {
      if (!this.isGuildChatPaused)
        return;
      this.isGuildChatPaused = false;
      ((Component) this).gameObject.SetActive(true);
      this.updateLatestMessageRepeatingCoroutine = this.StartCoroutine(this.UpdateLatestMessageRepeating());
    }
  }

  private IEnumerator SetDisplayingMessageType(
    GuildChatManager.GuildChatMessageFilterType filterType)
  {
    if (this.currentDisplayingMessageFilterType != filterType)
    {
      this.currentDisplayingMessageFilterType = filterType;
      this.SetTabButtons(filterType);
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      while (this.isUpdatingMessage)
        yield return (object) null;
      this.isUpdatingMessage = true;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      this.detailedListController.StopScrollViewMovement();
      this.UpdateDisplayingMessageDataList(filterType);
      this.ResetDetailedMessageItemListPlaceholder();
      this.isUpdatingMessage = false;
      this.setDisplayingMessageTypeCoroutine = (Coroutine) null;
    }
  }

  private void UpdateDisplayingMessageDataList(
    GuildChatManager.GuildChatMessageFilterType filterType)
  {
    switch (filterType)
    {
      case GuildChatManager.GuildChatMessageFilterType.All:
        this.displayingMessageList = this.allMessageDataList.Select<GuildChatMessageData, GuildChatMessageData>((Func<GuildChatMessageData, GuildChatMessageData>) (data => data)).ToList<GuildChatMessageData>();
        break;
      case GuildChatManager.GuildChatMessageFilterType.ChatOnly:
        this.displayingMessageList = this.allMessageDataList.Where<GuildChatMessageData>((Func<GuildChatMessageData, bool>) (data => data.messageType == GuildChatMessageData.GuildChatMessageType.MemberChat || data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerChat || data.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp || data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp)).ToList<GuildChatMessageData>();
        break;
      case GuildChatManager.GuildChatMessageFilterType.GuildRaidOnly:
        this.displayingMessageList = this.allMessageDataList.Where<GuildChatMessageData>((Func<GuildChatMessageData, bool>) (data => data.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog || data.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)).ToList<GuildChatMessageData>();
        break;
      case GuildChatManager.GuildChatMessageFilterType.LogOnly:
        this.displayingMessageList = this.allMessageDataList.Where<GuildChatMessageData>((Func<GuildChatMessageData, bool>) (data => data.messageType == GuildChatMessageData.GuildChatMessageType.MemberLog || data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerLog || data.messageType == GuildChatMessageData.GuildChatMessageType.SystemLog)).ToList<GuildChatMessageData>();
        break;
    }
  }

  private List<GuildChatMessageData> GetFilteredDataList(
    GuildChatManager.GuildChatMessageFilterType filterType,
    List<GuildChatMessageData> dataList)
  {
    List<GuildChatMessageData> filteredDataList = new List<GuildChatMessageData>();
    if (dataList != null)
    {
      switch (filterType)
      {
        case GuildChatManager.GuildChatMessageFilterType.All:
          filteredDataList = dataList;
          break;
        case GuildChatManager.GuildChatMessageFilterType.ChatOnly:
          for (int index = 0; index < dataList.Count; ++index)
          {
            if (dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.MemberChat || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.PlayerChat || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp)
              filteredDataList.Add(dataList[index]);
          }
          break;
        case GuildChatManager.GuildChatMessageFilterType.GuildRaidOnly:
          for (int index = 0; index < dataList.Count; ++index)
          {
            if (dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)
              filteredDataList.Add(dataList[index]);
          }
          break;
        case GuildChatManager.GuildChatMessageFilterType.LogOnly:
          for (int index = 0; index < dataList.Count; ++index)
          {
            if (dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.MemberLog || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.PlayerLog || dataList[index].messageType == GuildChatMessageData.GuildChatMessageType.SystemLog)
              filteredDataList.Add(dataList[index]);
          }
          break;
      }
    }
    return filteredDataList;
  }

  private void AdjustDetailedMessageItemListPlaceholder(
    bool shouldScrollToBottom = false,
    List<GuildChatMessageData> addedNewDataList = null,
    List<GuildChatMessageData> addedOldDataList = null,
    List<GuildChatMessageData> deletedOldDataList = null)
  {
    this.detailedListController.SetDisplayingMessageDataList(this.displayingMessageList);
    this.detailedListController.AdjustPlaceholderPosition(this.GetFilteredDataList(this.currentDisplayingMessageFilterType, addedNewDataList), this.GetFilteredDataList(this.currentDisplayingMessageFilterType, addedOldDataList), this.GetFilteredDataList(this.currentDisplayingMessageFilterType, deletedOldDataList), shouldScrollToBottom);
    int count1 = addedNewDataList == null ? 0 : addedNewDataList.Count;
    int count2 = addedOldDataList == null ? 0 : addedOldDataList.Count;
    int count3 = deletedOldDataList == null ? 0 : deletedOldDataList.Count;
    if (count1 <= 0 && count2 - count3 <= 0)
      return;
    this.isReceivedMessage = true;
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_2500");
  }

  private void ResetDetailedMessageItemListPlaceholder()
  {
    this.detailedListController.SetDisplayingMessageDataList(this.displayingMessageList);
    this.detailedListController.ResetPlaceholderPosition();
  }

  private void UpdateSimpleMessageItemList(List<GuildChatMessageData> receivedDataList)
  {
    this.EndMaintenanceMode();
    List<GuildChatMessageData> dataList = new List<GuildChatMessageData>();
    int index1 = receivedDataList.Count - 1;
    for (int index2 = 1; index1 >= 0 && index2 <= 3; ++index2)
    {
      if (!receivedDataList[index1].isDeleted)
        dataList.Add(receivedDataList[index1]);
      --index1;
    }
    dataList.Reverse();
    this.simpleListController.AddSimpleMessageItems(dataList);
  }

  private void SetTabButtons(
    GuildChatManager.GuildChatMessageFilterType filterType)
  {
    for (int index = 0; index < this.messageFilterTypeIdleButtons.Length; ++index)
    {
      if (filterType == (GuildChatManager.GuildChatMessageFilterType) index)
        ((Component) this.messageFilterTypeIdleButtons[index]).gameObject.SetActive(false);
      else
        ((Component) this.messageFilterTypeIdleButtons[index]).gameObject.SetActive(true);
    }
    for (int index = 0; index < this.messageFilterTypeHighlightButtons.Length; ++index)
    {
      if (filterType == (GuildChatManager.GuildChatMessageFilterType) index)
        ((Component) this.messageFilterTypeHighlightButtons[index]).gameObject.SetActive(true);
      else
        ((Component) this.messageFilterTypeHighlightButtons[index]).gameObject.SetActive(false);
    }
  }

  public void StartMaintenanceMode()
  {
    if (this.currentGuildChatStatus == GuildChatManager.GuildChatStatus.DetailedView)
      this.CloseDetailedChat();
    this.maintenanceBlock.SetActive(true);
    ((Component) this.simpleListController).gameObject.SetActive(false);
  }

  public void EndMaintenanceMode()
  {
    this.maintenanceBlock.SetActive(false);
    ((Component) this.simpleListController).gameObject.SetActive(true);
  }

  public void SetSprite(UI2DSprite sprite, int spriteID)
  {
    if (spriteID != 0)
    {
      GuildChatManager.SpriteCache spriteCache;
      if (GuildChatManager.spriteCache.TryGetValue(spriteID, out spriteCache))
        sprite.sprite2D = spriteCache.unitIconSprite;
      else
        this.StartCoroutine(this.SetSpriteFromResourceSchedule(sprite, spriteID));
    }
    else
      sprite.sprite2D = (Sprite) null;
  }

  private IEnumerator SetSpriteFromResourceSchedule(UI2DSprite sprite, int spriteID)
  {
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/c_thum", (object) spriteID));
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sprite.sprite2D = spriteF.Result;
    GuildChatManager.spriteCache[spriteID] = new GuildChatManager.SpriteCache(sprite.sprite2D);
  }

  public void SetStampSprite(UISprite targetSprite, int stampID)
  {
    GuildStamp guildStamp = ((IEnumerable<GuildStamp>) MasterData.GuildStampList).FirstOrDefault<GuildStamp>((Func<GuildStamp, bool>) (x => x.ID == stampID));
    if (guildStamp != null)
    {
      UIAtlas uiAtlas = (UIAtlas) null;
      if (GuildChatManager.stampAtlasCache.TryGetValue(guildStamp.groupID.ID, out uiAtlas))
      {
        targetSprite.atlas = uiAtlas;
        string str1 = guildStamp.groupID.ID.ToString("000");
        string str2 = string.Format("slc_stamp_id_{0}.png__GUI__chat_stamp_group{1}__chat_stamp_group{2}_prefab", (object) stampID.ToString("00"), (object) str1, (object) str1);
        targetSprite.spriteName = str2;
        UIButton component = ((Component) targetSprite).GetComponent<UIButton>();
        if (!Object.op_Inequality((Object) component, (Object) null))
          return;
        component.normalSprite = str2;
      }
      else
        this.StartCoroutine(this.SetStampSpriteFromResourceCoroutine(guildStamp.groupID.ID, stampID, targetSprite));
    }
    else
    {
      targetSprite.atlas = this.commonAtlas;
      string str = "slc_white.png__GUI__common__common_prefab";
      targetSprite.spriteName = str;
      UIButton component = ((Component) targetSprite).GetComponent<UIButton>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      component.normalSprite = str;
    }
  }

  private IEnumerator SetStampSpriteFromResourceCoroutine(
    int stampGroupID,
    int stampID,
    UISprite targetSprite)
  {
    string formattedStampGroupID = stampGroupID.ToString("000");
    Future<GameObject> atlasGameObjectF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("GUI/chat_stamp_group{0}/chat_stamp_group{1}_prefab", (object) formattedStampGroupID, (object) formattedStampGroupID));
    IEnumerator e = atlasGameObjectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UIAtlas component1 = atlasGameObjectF.Result.GetComponent<UIAtlas>();
    if (!GuildChatManager.stampAtlasCache.Keys.Contains<int>(stampGroupID))
      GuildChatManager.stampAtlasCache.Add(stampGroupID, component1);
    targetSprite.atlas = component1;
    string str = string.Format("slc_stamp_id_{0}.png__GUI__chat_stamp_group{1}__chat_stamp_group{2}_prefab", (object) stampID.ToString("00"), (object) formattedStampGroupID, (object) formattedStampGroupID);
    targetSprite.spriteName = str;
    UIButton component2 = ((Component) targetSprite).GetComponent<UIButton>();
    if (Object.op_Inequality((Object) component2, (Object) null))
      component2.normalSprite = str;
  }

  public void RefreshMessageItemIcon()
  {
    if (this.refreshMessageIconCoroutine != null || !PlayerAffiliation.Current.isGuildMember())
      return;
    this.refreshMessageIconCoroutine = this.StartCoroutine(this.RefreshUserIconCoroutine());
  }

  private IEnumerator RefreshUserIconCoroutine()
  {
    while (this.isUpdatingMessage)
      yield return (object) null;
    this.isUpdatingMessage = true;
    foreach (GuildChatMessageData allMessageData in this.allMessageDataList)
    {
      GuildChatMessageData data = allMessageData;
      if (data.messageType != GuildChatMessageData.GuildChatMessageType.SystemLog && data.messageType != GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)
      {
        GuildMembership guildMembership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == data.senderID));
        data.membership = guildMembership;
        data.spriteID = 0;
        if (data.membership != null)
          data.spriteID = guildMembership.player.leader_unit_unit.resource_reference_unit_id.ID;
      }
    }
    this.isUpdatingMessage = false;
    this.refreshMessageIconCoroutine = (Coroutine) null;
  }

  public void PlayGuildChatAnimation(
    GuildChatManager.GuildChatAnimationType animationType,
    EventDelegate.Callback finishCallback)
  {
    List<UITweener> uiTweenerList = new List<UITweener>();
    UITweener[] componentsInChildren = ((Component) this).gameObject.GetComponentsInChildren<UITweener>();
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      switch (animationType)
      {
        case GuildChatManager.GuildChatAnimationType.Simple_To_Detailed:
        case GuildChatManager.GuildChatAnimationType.Detailed_To_Simple:
          if ((GuildChatManager.GuildChatAnimationType) componentsInChildren[index].tweenGroup == animationType || componentsInChildren[index].tweenGroup == 21)
          {
            uiTweenerList.Add(componentsInChildren[index]);
            break;
          }
          if ((GuildChatManager.GuildChatAnimationType) componentsInChildren[index].tweenGroup == animationType || componentsInChildren[index].tweenGroup == 21 || componentsInChildren[index].tweenGroup == 24)
          {
            uiTweenerList.Add(componentsInChildren[index]);
            break;
          }
          break;
        case GuildChatManager.GuildChatAnimationType.Open_Stamp_Panel:
        case GuildChatManager.GuildChatAnimationType.Close_Stamp_Panel:
          if ((GuildChatManager.GuildChatAnimationType) componentsInChildren[index].tweenGroup == animationType || componentsInChildren[index].tweenGroup == 31)
          {
            uiTweenerList.Add(componentsInChildren[index]);
            break;
          }
          break;
      }
    }
    UITweener uiTweener = uiTweenerList[0];
    for (int index = 1; index < uiTweenerList.Count; ++index)
    {
      uiTweenerList[index].onFinished.Clear();
      if ((double) uiTweener.duration + (double) uiTweener.delay < (double) uiTweenerList[index].duration + (double) uiTweenerList[index].delay)
        uiTweener = uiTweenerList[index];
    }
    uiTweener.SetOnFinished(finishCallback);
    switch (animationType)
    {
      case GuildChatManager.GuildChatAnimationType.Simple_To_Detailed:
      case GuildChatManager.GuildChatAnimationType.Open_Stamp_Panel:
        using (List<UITweener>.Enumerator enumerator = uiTweenerList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UITweener current = enumerator.Current;
            current.tweenFactor = 0.0f;
            current.PlayForward();
          }
          break;
        }
      case GuildChatManager.GuildChatAnimationType.Detailed_To_Simple:
        using (List<UITweener>.Enumerator enumerator = uiTweenerList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UITweener current = enumerator.Current;
            if (current.tweenGroup == 21 || current.tweenGroup == 24)
            {
              current.tweenFactor = 1f;
              current.PlayReverse();
            }
            else
            {
              current.tweenFactor = 0.0f;
              current.PlayForward();
            }
          }
          break;
        }
      case GuildChatManager.GuildChatAnimationType.Close_Stamp_Panel:
        using (List<UITweener>.Enumerator enumerator = uiTweenerList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UITweener current = enumerator.Current;
            if (current.tweenGroup == 31)
            {
              current.tweenFactor = 1f;
              current.PlayReverse();
            }
            else
            {
              current.tweenFactor = 0.0f;
              current.PlayForward();
            }
          }
          break;
        }
    }
  }

  public void OpenMemberLogDialog(string playerID)
  {
    this.StartCoroutine(this.OpenMemberLogCoroutine(playerID));
  }

  private IEnumerator OpenMemberLogCoroutine(string playerID)
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_028_guild_member_log__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab1 = prefab.Result.Clone();
    prefab1.GetComponent<GuildMemberLogDialogController>().playerID = playerID;
    Singleton<PopupManager>.GetInstance().open(prefab1, isCloned: true);
  }

  private void LogGuildChatStatusError()
  {
    Debug.Log((object) ("<color=red>Status not right: " + this.currentGuildChatStatus.ToString() + "</color>"));
  }

  private enum IntervalStateMode
  {
    IntervalFirst,
    IntervalSecond,
    MAX,
  }

  public enum GuildChatStatus
  {
    NotOpened = 1,
    SimpleView = 2,
    DetailedView = 3,
  }

  private enum GuildChatMessageFilterType
  {
    All,
    ChatOnly,
    GuildRaidOnly,
    LogOnly,
  }

  public class SpriteCache
  {
    public Sprite unitIconSprite;

    public SpriteCache(Sprite sprite) => this.unitIconSprite = sprite;
  }

  public enum GuildChatAnimationType
  {
    Both_From_Simple_And_Detailed = 21, // 0x00000015
    Simple_To_Detailed = 22, // 0x00000016
    Detailed_To_Simple = 23, // 0x00000017
    Simple_Chat = 24, // 0x00000018
    Both_To_Open_And_Close_Stamp_Panel = 31, // 0x0000001F
    Open_Stamp_Panel = 32, // 0x00000020
    Close_Stamp_Panel = 33, // 0x00000021
  }
}
