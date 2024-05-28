// Decompiled with JetBrains decompiler
// Type: GuildChatDetailedListController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuildChatDetailedListController : MonoBehaviour
{
  [SerializeField]
  private GuildChatManager guildChatManager;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private SpringPanel springPanel;
  [SerializeField]
  private GameObject bottomArrow;
  [SerializeField]
  private UIPanel panel;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject headPlaceholder;
  [SerializeField]
  private GameObject tailPlaceholder;
  public Future<GameObject> guildChatContextMenuLogPopup01;
  public Future<GameObject> guildChatContextMenuGuildmasterPopup01;
  public Future<GameObject> guildChatContextMenuMemberPopup01;
  public Future<GameObject> bbsViewDialogPrefab;
  public Future<GameObject> bbsEditorDialogPrefab;
  private float itemHeight;
  private float itemStampHeight;
  private float clipSoftnessY;
  private List<GuildChatMessageData> displayingMessageDataList = new List<GuildChatMessageData>();
  private List<GameObject> bufferedMemberLogItems;
  private List<GameObject> bufferedMemberChatItems;
  private List<GameObject> bufferedPlayerLogItems;
  private List<GameObject> bufferedPlayerChatItems;
  private List<GameObject> bufferedSystemLogItems;
  private List<GameObject> bufferedDeletedItems;
  private List<GameObject> bufferedMemberStampItems;
  private List<GameObject> bufferedPlayerStampItems;
  private List<GameObject> bufferedGuildRaidLogItems;
  private List<GameObject> bufferedGuildRaidSystemLogItems;
  private const int bufferedMessageItemCount = 10;
  private const float dragToLoadThreshold = 180f;
  private const int showBottomArrowThreshold = 2;
  private bool isNGUIProcessFinished;
  private bool isScrollViewDraggedDown;
  public bool shouldUpdateDetailedMessageItemList;
  private string oldGuildID;

  private void Awake()
  {
    this.InitializeBufferedMessageItems();
    this.StartCoroutine(this.InitializePopupDialogPrefabs());
  }

  private void OnEnable()
  {
    if (!this.guildChatManager.isFirstTimeOpeningDetailedView)
      return;
    this.oldGuildID = PlayerAffiliation.Current.guild_id;
    this.isNGUIProcessFinished = false;
    this.StartCoroutine(this.WaitForNGUIProcessToFinish());
    this.guildChatManager.isFirstTimeOpeningDetailedView = false;
  }

  private void Start()
  {
  }

  private IEnumerator WaitForNGUIProcessToFinish()
  {
    yield return (object) new WaitForSeconds(0.01f);
    this.ResetPlaceholderPosition();
    yield return (object) new WaitForSeconds(0.01f);
    ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, -(this.tailPlaceholder.transform.localPosition.y + this.panel.finalClipRegion.w), ((Component) this.scrollView).transform.localPosition.z);
    this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
    this.isNGUIProcessFinished = true;
  }

  private void Update()
  {
    if (PlayerAffiliation.Current.guild_id != this.oldGuildID)
    {
      this.guildChatManager.StopAllGuildChatCoroutines();
      this.oldGuildID = PlayerAffiliation.Current.guild_id;
    }
    else
    {
      if (!this.isNGUIProcessFinished)
        return;
      this.UpdateDetailedMessageList();
      this.UpdateBottomArrow();
      if (this.scrollView.isDragging)
      {
        this.isScrollViewDraggedDown = true;
        this.guildChatManager.OnPullDownMessageListFromTop();
      }
      if ((double) Input.GetAxis("Mouse ScrollWheel") == 0.0)
        return;
      this.guildChatManager.OnPullDownMessageListFromTop();
    }
  }

  public void ResetPlaceholderPosition()
  {
    this.CleanAllBufferedMessageItems();
    ((Component) this.grid).transform.localPosition = new Vector3(0.0f, this.panel.finalClipRegion.w / 2f, 0.0f);
    this.headPlaceholder.transform.localPosition = Vector3.zero;
    float messageListHeight = this.GetMessageListHeight(this.displayingMessageDataList);
    if ((double) this.panel.finalClipRegion.w > (double) messageListHeight)
    {
      this.tailPlaceholder.transform.localPosition = new Vector3(0.0f, -this.panel.finalClipRegion.w, 0.0f);
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, 0.0f - this.clipSoftnessY, ((Component) this.scrollView).transform.localPosition.z);
      this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
    }
    else
    {
      this.tailPlaceholder.transform.localPosition = new Vector3(0.0f, -messageListHeight, 0.0f);
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, messageListHeight - this.panel.finalClipRegion.w + this.clipSoftnessY, ((Component) this.scrollView).transform.localPosition.z);
      this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
    }
    this.scrollView.UpdateScrollbars(true);
    this.shouldUpdateDetailedMessageItemList = true;
    this.RefreshDisplayingMessageItemPosition();
  }

  public void AdjustPlaceholderPosition(
    List<GuildChatMessageData> newDataList,
    List<GuildChatMessageData> oldDataList,
    List<GuildChatMessageData> deletedDataList,
    bool scrollToBottomNow = false)
  {
    float num1 = this.panel.finalClipRegion.y - this.panel.finalClipRegion.w - this.tailPlaceholder.transform.localPosition.y;
    float num2 = this.GetMessageListHeight(oldDataList) - this.GetMessageListHeight(deletedDataList);
    Transform transform = this.headPlaceholder.transform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, new Vector3(0.0f, num2, 0.0f));
    this.tailPlaceholder.transform.localPosition = Vector3.op_Addition(this.headPlaceholder.transform.localPosition, new Vector3(0.0f, -this.GetMessageListHeight(this.displayingMessageDataList), 0.0f));
    if ((double) this.headPlaceholder.transform.localPosition.y - (double) this.tailPlaceholder.transform.localPosition.y < (double) this.panel.finalClipRegion.w)
      this.tailPlaceholder.transform.localPosition = Vector3.op_Subtraction(this.headPlaceholder.transform.localPosition, new Vector3(0.0f, this.panel.finalClipRegion.w, 0.0f));
    if ((double) this.panel.finalClipRegion.y > (double) this.headPlaceholder.transform.localPosition.y && !this.scrollView.isDragging)
    {
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, -this.headPlaceholder.transform.localPosition.y - this.clipSoftnessY, ((Component) this.scrollView).transform.localPosition.z);
      this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
    }
    else if ((double) this.panel.finalClipRegion.y - (double) this.panel.finalClipRegion.w < (double) this.tailPlaceholder.transform.localPosition.y && !this.scrollView.isDragging)
    {
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, (float) -((double) this.tailPlaceholder.transform.localPosition.y + (double) this.panel.finalClipRegion.w) + this.clipSoftnessY, ((Component) this.scrollView).transform.localPosition.z);
      this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
    }
    if (newDataList.Count > 0)
    {
      if (scrollToBottomNow || (double) num1 < (double) this.itemHeight * 2.0)
      {
        this.springPanel.target = new Vector3(((Component) this.scrollView).transform.localPosition.x, -(this.tailPlaceholder.transform.localPosition.y + this.panel.finalClipRegion.w) + this.clipSoftnessY, ((Component) this.scrollView).transform.localPosition.z);
        ((Behaviour) this.springPanel).enabled = true;
      }
      else if (Singleton<CommonRoot>.GetInstance().guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView)
        this.ShowBottomArrow();
    }
    this.scrollView.UpdateScrollbars(true);
    this.shouldUpdateDetailedMessageItemList = true;
    this.RefreshDisplayingMessageItemPosition();
  }

  private void RefreshDisplayingMessageItemPosition()
  {
    float y = this.headPlaceholder.transform.localPosition.y;
    for (int index = 0; index < this.displayingMessageDataList.Count; ++index)
    {
      GuildChatMessageData displayingMessageData = this.displayingMessageDataList[index];
      displayingMessageData.topPosition = y;
      float num = displayingMessageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp || displayingMessageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp ? this.itemStampHeight : this.itemHeight;
      displayingMessageData.bottomPosition = y - num;
      y -= num;
    }
  }

  public void SetDisplayingMessageDataList(List<GuildChatMessageData> messageDataList)
  {
    this.displayingMessageDataList = messageDataList;
  }

  private void UpdateDetailedMessageList()
  {
    if (this.shouldUpdateDetailedMessageItemList)
    {
      if ((double) this.scrollView.panel.finalClipRegion.y - (double) this.scrollView.panel.finalClipRegion.w > (double) this.headPlaceholder.transform.localPosition.y || (double) this.tailPlaceholder.transform.localPosition.y > (double) this.scrollView.panel.finalClipRegion.y)
        return;
      List<int> intList = new List<int>();
      List<long> visibleMessageIDs = new List<long>();
      float y = this.panel.finalClipRegion.y;
      float num = y - this.panel.finalClipRegion.w;
      for (int index = 0; index < this.displayingMessageDataList.Count; ++index)
      {
        GuildChatMessageData displayingMessageData = this.displayingMessageDataList[index];
        if ((double) displayingMessageData.topPosition < (double) y && (double) displayingMessageData.topPosition > (double) num || (double) displayingMessageData.bottomPosition < (double) y && (double) displayingMessageData.bottomPosition > (double) num)
        {
          intList.Add(index);
          visibleMessageIDs.Add(displayingMessageData.messageID);
        }
      }
      this.ClearInvisibleBufferedMessageItems(visibleMessageIDs);
      foreach (int index in intList)
        this.UpdateMessageItem(this.displayingMessageDataList[index]);
      this.shouldUpdateDetailedMessageItemList = false;
    }
    if (!Vector3.op_Inequality(this.scrollView.currentMomentum, Vector3.zero) && !((Behaviour) this.springPanel).isActiveAndEnabled)
      return;
    this.shouldUpdateDetailedMessageItemList = true;
  }

  private void UpdateBottomArrow()
  {
    if ((double) this.tailPlaceholder.transform.localPosition.y < (double) this.scrollView.panel.finalClipRegion.y - (double) this.scrollView.panel.finalClipRegion.w)
      return;
    this.HideBottomArrow();
  }

  private void HideBottomArrow()
  {
    if (!this.bottomArrow.activeSelf)
      return;
    TweenPosition component = this.bottomArrow.GetComponent<TweenPosition>();
    ((UITweener) component).SetOnFinished((EventDelegate.Callback) (() => this.bottomArrow.SetActive(false)));
    ((UITweener) component).PlayReverse();
    ((UITweener) this.bottomArrow.GetComponent<TweenAlpha>()).PlayReverse();
  }

  private void ShowBottomArrow()
  {
    if (this.bottomArrow.activeSelf)
      return;
    this.bottomArrow.SetActive(true);
    TweenPosition componentInChildren = this.bottomArrow.GetComponentInChildren<TweenPosition>();
    ((UITweener) componentInChildren).onFinished.Clear();
    ((UITweener) ((Component) componentInChildren).GetComponent<TweenPosition>()).PlayForward();
    ((UITweener) this.bottomArrow.GetComponent<TweenAlpha>()).PlayForward();
  }

  private void UpdateMessageItem(GuildChatMessageData data)
  {
    List<GameObject> gameObjectList = (List<GameObject>) null;
    if (data.isDeleted)
      gameObjectList = this.bufferedDeletedItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerChat)
      gameObjectList = this.bufferedPlayerChatItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerLog)
      gameObjectList = this.bufferedPlayerLogItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.MemberChat)
      gameObjectList = this.bufferedMemberChatItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.MemberLog)
      gameObjectList = this.bufferedMemberLogItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.SystemLog)
      gameObjectList = this.bufferedSystemLogItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp)
      gameObjectList = this.bufferedMemberStampItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp)
      gameObjectList = this.bufferedPlayerStampItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog)
      gameObjectList = this.bufferedGuildRaidLogItems;
    else if (data.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)
      gameObjectList = this.bufferedGuildRaidSystemLogItems;
    bool flag1 = false;
    for (int index = 0; index < gameObjectList.Count; ++index)
    {
      if (gameObjectList[index].activeSelf && gameObjectList[index].GetComponent<GuildChatMessageItemController>().originalMessageData.messageID == data.messageID)
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
      return;
    bool flag2 = false;
    for (int index = 0; index < gameObjectList.Count; ++index)
    {
      if (!gameObjectList[index].activeSelf)
      {
        flag2 = true;
        GuildChatMessageItemController component = gameObjectList[index].GetComponent<GuildChatMessageItemController>();
        component.InitializeDetailedMessageItem(data);
        ((Component) component).transform.SetParent(((Component) this.grid).transform);
        ((Component) component).transform.localScale = Vector3.one;
        ((Component) component).transform.localPosition = new Vector3(0.0f, data.topPosition, 0.0f);
        ((Component) component).gameObject.SetActive(true);
        break;
      }
    }
    if (flag2)
      return;
    Debug.Log((object) "<color=red>Available buffered item not found!!!</color>");
  }

  private IEnumerator InitializePopupDialogPrefabs()
  {
    this.guildChatContextMenuLogPopup01 = Res.Prefabs.popup.popup_028_guild_chat_context_menu_log__anim_popup01.Load<GameObject>();
    IEnumerator e = this.guildChatContextMenuLogPopup01.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildChatContextMenuGuildmasterPopup01 = Res.Prefabs.popup.popup_028_guild_chat_context_menu_guildmaster__anim_popup01.Load<GameObject>();
    e = this.guildChatContextMenuGuildmasterPopup01.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildChatContextMenuMemberPopup01 = Res.Prefabs.popup.popup_028_guild_chat_context_menu_member__anim_popup01.Load<GameObject>();
    e = this.guildChatContextMenuMemberPopup01.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.bbsViewDialogPrefab = Res.Prefabs.popup.popup_028_guild_chat_bbs__anim_popup01.Load<GameObject>();
    e = this.bbsViewDialogPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.bbsEditorDialogPrefab = Res.Prefabs.popup.popup_028_guild_chat_bbs_edit__anim_popup01.Load<GameObject>();
    e = this.bbsEditorDialogPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void ClearInvisibleBufferedMessageItems(List<long> visibleMessageIDs)
  {
    for (int index = 0; index < 10; ++index)
    {
      GuildChatMessageItemController component1 = this.bufferedDeletedItems[index].GetComponent<GuildChatMessageItemController>();
      if (component1.originalMessageData != null && !visibleMessageIDs.Contains(component1.originalMessageData.messageID))
      {
        component1.originalMessageData = (GuildChatMessageData) null;
        ((Component) component1).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component2 = this.bufferedMemberChatItems[index].GetComponent<GuildChatMessageItemController>();
      if (component2.originalMessageData != null && !visibleMessageIDs.Contains(component2.originalMessageData.messageID))
      {
        component2.originalMessageData = (GuildChatMessageData) null;
        ((Component) component2).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component3 = this.bufferedMemberLogItems[index].GetComponent<GuildChatMessageItemController>();
      if (component3.originalMessageData != null && !visibleMessageIDs.Contains(component3.originalMessageData.messageID))
      {
        component3.originalMessageData = (GuildChatMessageData) null;
        ((Component) component3).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component4 = this.bufferedPlayerChatItems[index].GetComponent<GuildChatMessageItemController>();
      if (component4.originalMessageData != null && !visibleMessageIDs.Contains(component4.originalMessageData.messageID))
      {
        component4.originalMessageData = (GuildChatMessageData) null;
        ((Component) component4).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component5 = this.bufferedPlayerLogItems[index].GetComponent<GuildChatMessageItemController>();
      if (component5.originalMessageData != null && !visibleMessageIDs.Contains(component5.originalMessageData.messageID))
      {
        component5.originalMessageData = (GuildChatMessageData) null;
        ((Component) component5).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component6 = this.bufferedSystemLogItems[index].GetComponent<GuildChatMessageItemController>();
      if (component6.originalMessageData != null && !visibleMessageIDs.Contains(component6.originalMessageData.messageID))
      {
        component6.originalMessageData = (GuildChatMessageData) null;
        ((Component) component6).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component7 = this.bufferedMemberStampItems[index].GetComponent<GuildChatMessageItemController>();
      if (component7.originalMessageData != null && !visibleMessageIDs.Contains(component7.originalMessageData.messageID))
      {
        component7.originalMessageData = (GuildChatMessageData) null;
        ((Component) component7).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component8 = this.bufferedGuildRaidLogItems[index].GetComponent<GuildChatMessageItemController>();
      if (component8.originalMessageData != null && !visibleMessageIDs.Contains(component8.originalMessageData.messageID))
      {
        component8.originalMessageData = (GuildChatMessageData) null;
        ((Component) component8).gameObject.SetActive(false);
      }
      GuildChatMessageItemController component9 = this.bufferedGuildRaidSystemLogItems[index].GetComponent<GuildChatMessageItemController>();
      if (component9.originalMessageData != null && !visibleMessageIDs.Contains(component9.originalMessageData.messageID))
      {
        component9.originalMessageData = (GuildChatMessageData) null;
        ((Component) component9).gameObject.SetActive(false);
      }
    }
  }

  private void CleanAllBufferedMessageItems()
  {
    for (int index = 0; index < 10; ++index)
    {
      GuildChatMessageItemController component1 = this.bufferedDeletedItems[index].GetComponent<GuildChatMessageItemController>();
      component1.originalMessageData = (GuildChatMessageData) null;
      ((Component) component1).gameObject.SetActive(false);
      GuildChatMessageItemController component2 = this.bufferedMemberChatItems[index].GetComponent<GuildChatMessageItemController>();
      component2.originalMessageData = (GuildChatMessageData) null;
      ((Component) component2).gameObject.SetActive(false);
      GuildChatMessageItemController component3 = this.bufferedMemberLogItems[index].GetComponent<GuildChatMessageItemController>();
      component3.originalMessageData = (GuildChatMessageData) null;
      ((Component) component3).gameObject.SetActive(false);
      GuildChatMessageItemController component4 = this.bufferedPlayerChatItems[index].GetComponent<GuildChatMessageItemController>();
      component4.originalMessageData = (GuildChatMessageData) null;
      ((Component) component4).gameObject.SetActive(false);
      GuildChatMessageItemController component5 = this.bufferedPlayerLogItems[index].GetComponent<GuildChatMessageItemController>();
      component5.originalMessageData = (GuildChatMessageData) null;
      ((Component) component5).gameObject.SetActive(false);
      GuildChatMessageItemController component6 = this.bufferedSystemLogItems[index].GetComponent<GuildChatMessageItemController>();
      component6.originalMessageData = (GuildChatMessageData) null;
      ((Component) component6).gameObject.SetActive(false);
      GuildChatMessageItemController component7 = this.bufferedMemberStampItems[index].GetComponent<GuildChatMessageItemController>();
      component7.originalMessageData = (GuildChatMessageData) null;
      ((Component) component7).gameObject.SetActive(false);
      GuildChatMessageItemController component8 = this.bufferedPlayerStampItems[index].GetComponent<GuildChatMessageItemController>();
      component8.originalMessageData = (GuildChatMessageData) null;
      ((Component) component8).gameObject.SetActive(false);
      GuildChatMessageItemController component9 = this.bufferedGuildRaidLogItems[index].GetComponent<GuildChatMessageItemController>();
      component9.originalMessageData = (GuildChatMessageData) null;
      ((Component) component9).gameObject.SetActive(false);
      GuildChatMessageItemController component10 = this.bufferedGuildRaidSystemLogItems[index].GetComponent<GuildChatMessageItemController>();
      component10.originalMessageData = (GuildChatMessageData) null;
      ((Component) component10).gameObject.SetActive(false);
    }
  }

  private void InitializeBufferedMessageItems()
  {
    this.itemHeight = (float) this.guildChatManager.playerChatItemPrefab.GetComponent<UIWidget>().height;
    this.itemStampHeight = (float) this.guildChatManager.playerStampItemPrefab.GetComponent<UIWidget>().height;
    this.clipSoftnessY = ((Component) this.scrollView).GetComponent<UIPanel>().clipSoftness.y;
    this.bufferedPlayerChatItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.playerChatItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedPlayerChatItems.Add(gameObject);
    }
    this.bufferedPlayerLogItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.playerLogItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedPlayerLogItems.Add(gameObject);
    }
    this.bufferedMemberChatItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.memberChatItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedMemberChatItems.Add(gameObject);
    }
    this.bufferedMemberLogItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.memberLogItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedMemberLogItems.Add(gameObject);
    }
    this.bufferedSystemLogItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.systemLogItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedSystemLogItems.Add(gameObject);
    }
    this.bufferedDeletedItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.deletedMessageItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedDeletedItems.Add(gameObject);
    }
    this.bufferedMemberStampItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.memberStampItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedMemberStampItems.Add(gameObject);
    }
    this.bufferedPlayerStampItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.playerStampItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedPlayerStampItems.Add(gameObject);
    }
    this.bufferedGuildRaidLogItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.guildRaidLogPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedGuildRaidLogItems.Add(gameObject);
    }
    this.bufferedGuildRaidSystemLogItems = new List<GameObject>();
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.guildChatManager.systemLogItemPrefab);
      gameObject.SetActive(false);
      gameObject.transform.SetParent(((Component) this.grid).transform);
      this.bufferedGuildRaidSystemLogItems.Add(gameObject);
    }
  }

  public void ScrollToHead()
  {
    ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, -this.headPlaceholder.transform.localPosition.y, ((Component) this.scrollView).transform.localPosition.z);
    this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
  }

  public void ScrollToTail()
  {
    ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, -(this.tailPlaceholder.transform.localPosition.y + this.panel.finalClipRegion.w), ((Component) this.scrollView).transform.localPosition.z);
    this.panel.clipOffset = new Vector2(-((Component) this.scrollView).transform.localPosition.x, -((Component) this.scrollView).transform.localPosition.y);
  }

  public bool IsScrollViewDragging() => this.scrollView.isDragging;

  public void StopScrollViewMovement()
  {
    ((Behaviour) this.springPanel).enabled = false;
    this.scrollView.currentMomentum = Vector3.zero;
  }

  public float GetMessageListHeight(List<GuildChatMessageData> messageList)
  {
    float messageListHeight = 0.0f;
    if (messageList != null)
    {
      for (int index = 0; index < messageList.Count; ++index)
      {
        if (messageList[index].messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp || messageList[index].messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp)
          messageListHeight += this.itemStampHeight;
        else
          messageListHeight += this.itemHeight;
      }
    }
    return messageListHeight;
  }
}
