// Decompiled with JetBrains decompiler
// Type: GuildChatMessageItemController
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
public class GuildChatMessageItemController : MonoBehaviour
{
  [SerializeField]
  private UILabel messageContent;
  [SerializeField]
  private UILabel senderName;
  [SerializeField]
  private UILabel senderName2;
  [SerializeField]
  private UILabel sendTime;
  [SerializeField]
  private UILabel colon;
  [SerializeField]
  private UI2DSprite senderIcon;
  [SerializeField]
  private UIButton messageButton;
  [SerializeField]
  private UISprite guildMasterIcon;
  [SerializeField]
  private UISprite guildSubmasterIcon;
  [SerializeField]
  private UISprite stampImage;
  [SerializeField]
  private GameObject sprBtnTeamLog;
  public GuildChatMessageData originalMessageData;
  private const int nameMaxLengthInSimpleView = 8;
  private const int contentMaxLengthInSimpleView = 19;
  private GameObject guildRaidLogDeckPrefab;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void InitializeDetailedMessageItem(GuildChatMessageData messageData)
  {
    this.originalMessageData = messageData;
    this.sendTime.SetTextLocalize(messageData.GetFormattedSendTime());
    if (this.originalMessageData.isDeleted)
      return;
    if (this.originalMessageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp || this.originalMessageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp)
      Singleton<CommonRoot>.GetInstance().guildChatManager.SetStampSprite(this.stampImage, this.originalMessageData.stampID);
    if (this.originalMessageData.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog)
    {
      string messageContent = this.originalMessageData.messageContent;
      if (messageContent.LastIndexOf('@') == messageContent.Length - 1)
        this.messageContent.SetTextLocalize(messageContent.Substring(0, messageContent.LastIndexOf('@', messageContent.Length - 2)));
      else
        this.messageContent.SetTextLocalize(this.originalMessageData.messageContent);
    }
    else
      this.messageContent.SetTextLocalize(this.originalMessageData.messageContent);
    if (this.originalMessageData.messageType != GuildChatMessageData.GuildChatMessageType.SystemLog && this.originalMessageData.messageType != GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)
    {
      Singleton<CommonRoot>.GetInstance().guildChatManager.SetSprite(this.senderIcon, this.originalMessageData.spriteID);
      if (Object.op_Inequality((Object) this.senderName, (Object) null))
        this.senderName.SetTextLocalize(this.originalMessageData.senderName);
      if (Object.op_Inequality((Object) this.senderName2, (Object) null))
        this.senderName2.SetTextLocalize(this.originalMessageData.senderName);
      if (Object.op_Inequality((Object) this.guildMasterIcon, (Object) null))
        ((Component) this.guildMasterIcon).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.guildSubmasterIcon, (Object) null))
        ((Component) this.guildSubmasterIcon).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.senderName, (Object) null))
        ((Component) this.senderName).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) this.senderName2, (Object) null))
        ((Component) this.senderName2).gameObject.SetActive(false);
      if (this.originalMessageData.membership != null)
      {
        switch (this.originalMessageData.membership.role)
        {
          case GuildRole.sub_master:
            if (Object.op_Implicit((Object) this.guildSubmasterIcon))
              ((Component) this.guildSubmasterIcon).gameObject.SetActive(true);
            if (Object.op_Inequality((Object) this.senderName2, (Object) null))
            {
              ((Component) this.senderName2).gameObject.SetActive(true);
              break;
            }
            break;
          case GuildRole.master:
            if (Object.op_Implicit((Object) this.guildMasterIcon))
              ((Component) this.guildMasterIcon).gameObject.SetActive(true);
            if (Object.op_Inequality((Object) this.senderName2, (Object) null))
            {
              ((Component) this.senderName2).gameObject.SetActive(true);
              break;
            }
            break;
          default:
            if (Object.op_Inequality((Object) this.senderName, (Object) null))
            {
              ((Component) this.senderName).gameObject.SetActive(true);
              break;
            }
            break;
        }
      }
      else if (Object.op_Inequality((Object) this.senderName, (Object) null))
        ((Component) this.senderName).gameObject.SetActive(true);
    }
    switch (this.originalMessageData.messageType)
    {
      case GuildChatMessageData.GuildChatMessageType.PlayerChat:
      case GuildChatMessageData.GuildChatMessageType.MemberChat:
        EventDelegate.Set(this.messageButton.onClick, new EventDelegate.Callback(this.OnChatMessageClicked));
        break;
      case GuildChatMessageData.GuildChatMessageType.PlayerLog:
      case GuildChatMessageData.GuildChatMessageType.MemberLog:
      case GuildChatMessageData.GuildChatMessageType.SystemLog:
      case GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog:
        EventDelegate.Set(this.messageButton.onClick, new EventDelegate.Callback(this.OnLogMessageClicked));
        break;
      case GuildChatMessageData.GuildChatMessageType.PlayerStamp:
      case GuildChatMessageData.GuildChatMessageType.MemberStamp:
        EventDelegate.Set(this.messageButton.onClick, new EventDelegate.Callback(this.OnStampMessageClicked));
        break;
      case GuildChatMessageData.GuildChatMessageType.GuildRaidLog:
        EventDelegate.Set(this.messageButton.onClick, new EventDelegate.Callback(this.OnRaidLogMessageClicked));
        break;
    }
    if (this.originalMessageData.messageType != GuildChatMessageData.GuildChatMessageType.GuildRaidLog)
      return;
    if (Object.op_Inequality((Object) this.sprBtnTeamLog, (Object) null))
      this.sprBtnTeamLog.SetActive(messageData.isRaidDeckView);
    ((Behaviour) this.messageButton).enabled = messageData.isRaidDeckView;
  }

  public void InitializeSimpleMessageItem(GuildChatMessageData messageData)
  {
    this.originalMessageData = messageData;
    Color white = Color.white;
    if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerChat || messageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberChat)
    {
      white = Color.white;
      this.messageContent.supportEncoding = false;
    }
    else if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.SystemLog || messageData.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidSystemLog)
    {
      // ISSUE: explicit constructor call
      ((Color) ref white).\u002Ector(0.278f, 0.776f, 0.349f);
      this.messageContent.supportEncoding = true;
    }
    else if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerLog || messageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberLog)
    {
      // ISSUE: explicit constructor call
      ((Color) ref white).\u002Ector(1f, 0.706f, 0.0f);
      this.messageContent.supportEncoding = true;
    }
    else if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp || messageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp)
    {
      white = Color.white;
      this.messageContent.supportEncoding = false;
    }
    else if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog)
    {
      white = Color.white;
      this.messageContent.supportEncoding = false;
    }
    string text1 = this.originalMessageData.senderName;
    if (text1.Length > 8)
      text1 = text1.Substring(0, 7) + Consts.GetInstance().GUILD_CHAT_ELLIPSIS;
    this.senderName.SetTextLocalize(text1);
    ((UIWidget) this.senderName).color = white;
    string text2;
    if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.PlayerStamp || messageData.messageType == GuildChatMessageData.GuildChatMessageType.MemberStamp)
    {
      text2 = Consts.GetInstance().GUILD_CHAT_SIMPLE_MESSAGE_STAMP_SENT;
    }
    else
    {
      text2 = this.originalMessageData.isDeleted ? string.Empty : this.originalMessageData.messageContent;
      if (messageData.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog && text2.LastIndexOf('@') == text2.Length - 1)
        text2 = text2.Substring(0, text2.LastIndexOf('@', text2.Length - 2));
    }
    if (text2.Length > 19)
      text2 = text2.Substring(0, 18) + Consts.GetInstance().GUILD_CHAT_ELLIPSIS;
    this.messageContent.SetTextLocalize(text2);
    ((UIWidget) this.messageContent).color = white;
    ((UIWidget) this.colon).color = white;
  }

  public void InitializeMemberLogItem(GuildChatMessageData messageData)
  {
    this.originalMessageData = messageData;
    this.sendTime.SetTextLocalize(messageData.GetFormattedSendTime());
    string messageContent = this.originalMessageData.messageContent;
    if (this.originalMessageData.messageType == GuildChatMessageData.GuildChatMessageType.GuildRaidLog && messageContent.LastIndexOf('@') == messageContent.Length - 1)
      this.messageContent.SetTextLocalize(messageContent.Substring(0, messageContent.LastIndexOf('@', messageContent.Length - 2)));
    else
      this.messageContent.SetTextLocalize(this.originalMessageData.messageContent);
  }

  public void UpdateDisplayingSendTime()
  {
    this.sendTime.SetTextLocalize(this.originalMessageData.GetFormattedSendTime());
  }

  public void OnChatMessageClicked()
  {
  }

  private IEnumerator ChatMessageClickedHandler(GuildChatMessageData data)
  {
    GuildMembership guildMembership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).First<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
    if (guildMembership != null)
    {
      Future<GameObject> future;
      if (guildMembership.role == GuildRole.master || guildMembership.role == GuildRole.sub_master)
      {
        while (Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController.guildChatContextMenuGuildmasterPopup01 == null)
          yield return (object) null;
        future = Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController.guildChatContextMenuGuildmasterPopup01;
      }
      else
      {
        while (Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController.guildChatContextMenuMemberPopup01 == null)
          yield return (object) null;
        future = Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController.guildChatContextMenuMemberPopup01;
      }
      GameObject prefab = future.Result.Clone();
      prefab.GetComponent<GuildChatContextMenuDialogController>().Initialize(data);
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
  }

  public void OnLogMessageClicked()
  {
    Debug.Log((object) ("<color=yellow>Scene name: </color>" + this.originalMessageData.transition.scene_name));
  }

  private IEnumerator LogMessageClickedHandler(GuildChatMessageData data)
  {
    GuildChatDetailedListController detailedListController = Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController;
    while (detailedListController.guildChatContextMenuLogPopup01 == null)
      yield return (object) null;
    GameObject prefab = detailedListController.guildChatContextMenuLogPopup01.Result.Clone();
    prefab.GetComponent<GuildChatContextMenuDialogController>().Initialize(data);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private void OnStampMessageClicked()
  {
  }

  public void OnRaidLogMessageClicked()
  {
    this.StartCoroutine(this.ShowRaidLogPopup(this.originalMessageData));
  }

  private IEnumerator ShowRaidLogPopup(GuildChatMessageData data)
  {
    if (Object.op_Equality((Object) this.guildRaidLogDeckPrefab, (Object) null))
    {
      Future<GameObject> prefabf = new ResourceObject("Prefabs/raid032_chat/dir_guild_Deck").Load<GameObject>();
      IEnumerator e = prefabf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildRaidLogDeckPrefab = prefabf.Result;
      prefabf = (Future<GameObject>) null;
    }
    yield return (object) Singleton<PopupManager>.GetInstance().open(this.guildRaidLogDeckPrefab).GetComponent<GuildRaidLogTeamPopup>().InitializeAsync(data);
  }
}
