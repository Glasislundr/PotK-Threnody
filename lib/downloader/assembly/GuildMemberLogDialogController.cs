// Decompiled with JetBrains decompiler
// Type: GuildMemberLogDialogController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildMemberLogDialogController : MonoBehaviour
{
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject headPlaceholder;
  [SerializeField]
  private GameObject tailPlaceholder;
  [SerializeField]
  private GameObject memberLogItemPrefab;
  public string playerID;
  private const int maxBufferedMemberLogItemCount = 10;
  private List<GameObject> bufferedMemberLogItems = new List<GameObject>();
  private const int maxSavedMemberLogCount = 1000;
  private List<GuildChatMessageData> receivedMessageDataList = new List<GuildChatMessageData>();
  private List<GuildChatMessageData> displayingMemberLogDataList = new List<GuildChatMessageData>();
  private int memberLogItemHeight;
  private const float dragToLoadThreshold = 180f;
  private bool isUpdatingMemberLogData;
  private Coroutine updateMemberLogDataCoroutine;

  private void Awake()
  {
  }

  private void Start() => this.StartCoroutine(this.Initialize());

  private void Update()
  {
    this.UpdateMemberLogItemList();
    if (!this.scrollView.isDragging || this.isUpdatingMemberLogData || (double) this.scrollView.panel.finalClipRegion.y - (double) this.headPlaceholder.transform.localPosition.y < 180.0)
      return;
    this.OnPullDownMemberLogListFromTop();
  }

  public IEnumerator Initialize()
  {
    GuildMemberLogDialogController dialogController = this;
    yield return (object) new WaitForSeconds(0.01f);
    ((Component) dialogController.grid).transform.localPosition = new Vector3(0.0f, dialogController.scrollView.panel.finalClipRegion.w / 2f, 0.0f);
    dialogController.headPlaceholder.transform.localPosition = Vector3.zero;
    dialogController.tailPlaceholder.transform.localPosition = Vector3.zero;
    ((Component) dialogController.scrollView).transform.localPosition = Vector3.zero;
    dialogController.scrollView.panel.clipOffset = new Vector2(-((Component) dialogController.scrollView).transform.localPosition.x, -((Component) dialogController.scrollView).transform.localPosition.y);
    dialogController.scrollView.UpdateScrollbars(true);
    if (Object.op_Equality((Object) dialogController.memberLogItemPrefab, (Object) null))
    {
      Future<GameObject> prefab = Res.Prefabs.guild.guild_member_log_list.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      dialogController.memberLogItemPrefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    dialogController.memberLogItemHeight = dialogController.memberLogItemPrefab.GetComponent<UIWidget>().height;
    for (int index = 0; index < 10; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(dialogController.memberLogItemPrefab);
      gameObject.SetActive(false);
      dialogController.bufferedMemberLogItems.Add(gameObject);
    }
    dialogController.updateMemberLogDataCoroutine = dialogController.StartCoroutine(dialogController.UpdateLatestMemberLogData());
  }

  private IEnumerator UpdateLatestMemberLogData()
  {
    GuildMemberLogDialogController dialogController = this;
    if (!dialogController.isUpdatingMemberLogData)
    {
      dialogController.isUpdatingMemberLogData = true;
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.GuildlogMemberShowLatest> future = WebAPI.GuildlogMemberShowLatest(dialogController.playerID, new Action<WebAPI.Response.UserError>(dialogController.\u003CUpdateLatestMemberLogData\u003Eb__19_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (future.Result != null)
      {
        WebAPI.Response.GuildlogMemberShowLatestGuild_logs[] guildLogs = future.Result.guild_logs;
        dialogController.receivedMessageDataList.Clear();
        for (int index = 0; index < guildLogs.Length; ++index)
        {
          GuildChatMessageData guildChatMessageData = new GuildChatMessageData(guildLogs[index]);
          dialogController.receivedMessageDataList.Add(guildChatMessageData);
        }
        int addedNewItemCount = dialogController.receivedMessageDataList.Count<GuildChatMessageData>();
        dialogController.receivedMessageDataList = dialogController.receivedMessageDataList.OrderBy<GuildChatMessageData, long>((Func<GuildChatMessageData, long>) (data => data.messageID)).ToList<GuildChatMessageData>();
        dialogController.displayingMemberLogDataList.AddRange((IEnumerable<GuildChatMessageData>) dialogController.receivedMessageDataList);
        int deletedOldItemCount = 0;
        if (dialogController.displayingMemberLogDataList.Count > 1000)
          deletedOldItemCount = dialogController.displayingMemberLogDataList.Count - 1000;
        for (int index = 0; index < deletedOldItemCount; ++index)
          dialogController.displayingMemberLogDataList.RemoveAt(0);
        dialogController.AdjustPlaceholderAndScrollView(addedNewItemCount, 0, deletedOldItemCount);
      }
      dialogController.isUpdatingMemberLogData = false;
    }
  }

  private IEnumerator UpdateEarlierMemberLogData()
  {
    GuildMemberLogDialogController dialogController = this;
    if (!dialogController.isUpdatingMemberLogData)
    {
      dialogController.isUpdatingMemberLogData = true;
      long num = 0;
      if (dialogController.displayingMemberLogDataList.Count > 0)
        num = dialogController.displayingMemberLogDataList[0].messageID;
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.GuildlogMemberShowPast> future = WebAPI.GuildlogMemberShowPast(num.ToString(), dialogController.playerID, new Action<WebAPI.Response.UserError>(dialogController.\u003CUpdateEarlierMemberLogData\u003Eb__20_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (future.Result != null)
      {
        WebAPI.Response.GuildlogMemberShowPastGuild_logs[] guildLogs = future.Result.guild_logs;
        dialogController.receivedMessageDataList.Clear();
        for (int index = 0; index < guildLogs.Length; ++index)
        {
          GuildChatMessageData guildChatMessageData = new GuildChatMessageData(guildLogs[index]);
          dialogController.receivedMessageDataList.Add(guildChatMessageData);
        }
        int addedNewItemCount = dialogController.receivedMessageDataList.Count<GuildChatMessageData>();
        dialogController.receivedMessageDataList = dialogController.receivedMessageDataList.OrderBy<GuildChatMessageData, long>((Func<GuildChatMessageData, long>) (data => data.messageID)).ToList<GuildChatMessageData>();
        int count = dialogController.receivedMessageDataList.Count;
        List<GuildChatMessageData> guildChatMessageDataList = new List<GuildChatMessageData>();
        guildChatMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) dialogController.receivedMessageDataList);
        guildChatMessageDataList.AddRange((IEnumerable<GuildChatMessageData>) dialogController.displayingMemberLogDataList);
        dialogController.displayingMemberLogDataList = guildChatMessageDataList;
        int deletedOldItemCount = 0;
        if (dialogController.displayingMemberLogDataList.Count > 1000)
          deletedOldItemCount = dialogController.displayingMemberLogDataList.Count - 1000;
        for (int index = 0; index < deletedOldItemCount; ++index)
          dialogController.displayingMemberLogDataList.RemoveAt(0);
        dialogController.AdjustPlaceholderAndScrollView(addedNewItemCount, count, deletedOldItemCount);
      }
      dialogController.isUpdatingMemberLogData = false;
    }
  }

  private void AdjustPlaceholderAndScrollView(
    int addedNewItemCount,
    int addedOldItemCount,
    int deletedOldItemCount)
  {
    Transform transform = this.headPlaceholder.transform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, new Vector3(0.0f, (float) (this.memberLogItemHeight * (addedOldItemCount - deletedOldItemCount)), 0.0f));
    this.tailPlaceholder.transform.localPosition = new Vector3(0.0f, this.headPlaceholder.transform.localPosition.y - (float) (this.memberLogItemHeight * this.displayingMemberLogDataList.Count), 0.0f);
    if ((double) this.headPlaceholder.transform.localPosition.y - (double) this.tailPlaceholder.transform.localPosition.y < (double) this.scrollView.panel.finalClipRegion.w)
      this.tailPlaceholder.transform.localPosition = new Vector3(0.0f, this.headPlaceholder.transform.localPosition.y - this.scrollView.panel.finalClipRegion.w, 0.0f);
    if (addedNewItemCount > 0)
    {
      ((Component) this.scrollView).transform.localPosition = new Vector3(0.0f, -(this.tailPlaceholder.transform.localPosition.y + this.scrollView.panel.finalClipRegion.w), 0.0f);
      this.scrollView.panel.clipOffset = new Vector2(0.0f, -((Component) this.scrollView).transform.localPosition.y);
    }
    this.scrollView.UpdateScrollbars(true);
  }

  private void UpdateMemberLogItemList()
  {
    foreach (GameObject bufferedMemberLogItem in this.bufferedMemberLogItems)
      bufferedMemberLogItem.SetActive(false);
    if ((double) this.scrollView.panel.finalClipRegion.y - (double) this.scrollView.panel.finalClipRegion.w > (double) this.headPlaceholder.transform.localPosition.y || (double) this.tailPlaceholder.transform.localPosition.y > (double) this.scrollView.panel.finalClipRegion.y)
      return;
    int num1 = 0;
    if ((double) this.headPlaceholder.transform.localPosition.y > (double) this.scrollView.panel.finalClipRegion.y)
      num1 = Mathf.FloorToInt((this.headPlaceholder.transform.localPosition.y - this.scrollView.panel.finalClipRegion.y) / (float) this.memberLogItemHeight);
    int num2 = this.displayingMemberLogDataList.Count - 1;
    if ((double) this.scrollView.panel.finalClipRegion.y - (double) this.scrollView.panel.finalClipRegion.w > (double) this.tailPlaceholder.transform.localPosition.y)
      num2 = Mathf.FloorToInt((this.headPlaceholder.transform.localPosition.y - (this.scrollView.panel.finalClipRegion.y - this.scrollView.panel.finalClipRegion.w)) / (float) this.memberLogItemHeight);
    for (int index = num1; index <= num2 && index <= this.displayingMemberLogDataList.Count - 1; ++index)
    {
      GameObject bufferedMemberLogItem = this.bufferedMemberLogItems[index - num1];
      bufferedMemberLogItem.transform.SetParent(((Component) this.grid).transform);
      bufferedMemberLogItem.transform.localScale = Vector3.one;
      bufferedMemberLogItem.transform.localPosition = new Vector3(0.0f, this.headPlaceholder.transform.localPosition.y - (float) (this.memberLogItemHeight * index), 0.0f);
      bufferedMemberLogItem.GetComponent<GuildChatMessageItemController>().InitializeMemberLogItem(this.displayingMemberLogDataList[index]);
      bufferedMemberLogItem.SetActive(true);
    }
  }

  public void OnCloseButtonClicked()
  {
    if (this.updateMemberLogDataCoroutine != null)
      this.StopCoroutine(this.updateMemberLogDataCoroutine);
    this.displayingMemberLogDataList.Clear();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void OnPullDownMemberLogListFromTop()
  {
    this.updateMemberLogDataCoroutine = this.StartCoroutine(this.UpdateEarlierMemberLogData());
  }
}
