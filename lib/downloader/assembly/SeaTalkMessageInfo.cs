// Decompiled with JetBrains decompiler
// Type: SeaTalkMessageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SeaTalkMessageInfo
{
  private const int BACKGROUND_MERGIN_HEIGHT = 30;
  private const int NO_LIMIT_TIME_MERGIN = 30;
  private const int LIMIT_TIME_MESSAGE_HEIGHT = 80;
  private const float LIMIT_TIME_ADD_Y = 7f;
  private const int DATE_HEIGHT = 50;
  private const int UNREAD_LINE_HEIGHT = 30;
  private const int GIFT_HEIGHT = 170;
  public TalkMessageViewType MessageViewType;
  public string Message;
  public bool IsBadMood;
  public string SendTime;
  public string LimitTime;
  public bool LimitTimeAnimation;
  public Color KeepLimitTimeAnimationIconColor = SeaTalkMessage.DEFAULT_LIMIT_ICON_COLOR;
  public Color KeepLimitTimeAnimationTimeColor = Color.white;
  public Sprite GiftSprite;
  public int BackgroundHeight;
  public int BackgroundWidth;
  public float PositionY;
  public float TypeIconPositionY;
  public float LimitTimePositionY;
  public float LimitTimeIconPositionX;
  private static Dictionary<int, Sprite> gearCache = new Dictionary<int, Sprite>();
  public PlayerTalkMessage playerTalkMessage;
  private TalkUnitInfo talkUnitInfo;
  private UILabel tempMessageLabel;
  public CallMessage callMessage;

  public void Init(
    PlayerTalkMessage playerTalkMessage,
    TalkUnitInfo talkUnitInfo,
    UILabel tempMessageLabel)
  {
    this.playerTalkMessage = playerTalkMessage;
    this.talkUnitInfo = talkUnitInfo;
    this.tempMessageLabel = tempMessageLabel;
    MasterData.CallMessage.TryGetValue(playerTalkMessage.message_id, out this.callMessage);
    this.SetMessageType();
    switch (this.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        this.CreatePartner();
        break;
      case TalkMessageViewType.Player:
        this.CreatePlayer();
        break;
      case TalkMessageViewType.Gift:
        this.CreateGift();
        break;
      default:
        Debug.LogError((object) "想定していないTalkMessageViewTypeです");
        break;
    }
  }

  public void InitDate(PlayerTalkMessage playerTalkMessage)
  {
    this.playerTalkMessage = playerTalkMessage;
    this.MessageViewType = TalkMessageViewType.Date;
    this.CreateDate();
  }

  public void InitUnread(PlayerTalkMessage playerTalkMessage)
  {
    this.playerTalkMessage = playerTalkMessage;
    this.MessageViewType = TalkMessageViewType.UnreadLine;
    this.CreateUnread();
  }

  private void SetMessageType()
  {
    if (this.playerTalkMessage.message_type == 6 && this.callMessage == null)
    {
      this.MessageViewType = TalkMessageViewType.Gift;
    }
    else
    {
      switch (this.callMessage.message_type_id)
      {
        case 1:
        case 2:
        case 4:
        case 5:
        case 8:
        case 10:
          this.MessageViewType = TalkMessageViewType.Partner;
          break;
        case 3:
        case 6:
        case 7:
          this.MessageViewType = TalkMessageViewType.Player;
          break;
        default:
          Debug.LogError((object) string.Format("想定していないTalkMessageTypeです {0}", (object) (TalkMessageType) this.callMessage.message_type_id));
          break;
      }
    }
  }

  private void CreatePartner()
  {
    this.Message = SeaTalkCommon.GetReplaceMessage(this.callMessage.text, this.talkUnitInfo, this.playerTalkMessage);
    this.tempMessageLabel.text = this.Message;
    int lineCount = SeaTalkCommon.GetLineCount(this.Message);
    this.TypeIconPositionY = (float) ((double) -lineCount * 29.0 / 2.0);
    this.BackgroundHeight = SeaTalkCommon.GetHeight(lineCount) + 30;
    if (this.callMessage.message_type_id == 10)
      this.IsBadMood = true;
    if (!this.playerTalkMessage.expire_at.HasValue)
    {
      this.LimitTime = "";
      this.LimitTimeAnimation = false;
    }
    else
    {
      this.LimitTime = this.playerTalkMessage.expire_at.Value.ToString("M/d HH:mmまで");
      this.LimitTimePositionY = (float) -((double) this.UpperHeight / 2.0 + 7.0);
    }
    this.SendTime = this.playerTalkMessage.created_at.ToString("HH:mm");
  }

  private void CreatePlayer()
  {
    this.Message = SeaTalkCommon.GetReplaceMessage(this.callMessage.text, this.talkUnitInfo, this.playerTalkMessage);
    this.tempMessageLabel.text = this.Message;
    this.BackgroundHeight = ((UIWidget) this.tempMessageLabel).height + 30;
    this.SendTime = this.playerTalkMessage.created_at.ToString("HH:mm");
  }

  private void CreateDate()
  {
    this.BackgroundHeight = 50;
    DateTime dateTime = ServerTime.NowAppTimeAddDelta();
    if (dateTime.Date == this.playerTalkMessage.created_at.Date)
    {
      this.Message = "今日";
      this.BackgroundWidth = 64;
    }
    else if (dateTime.AddDays(-1.0).Date == this.playerTalkMessage.created_at.Date)
    {
      this.Message = "昨日";
      this.BackgroundWidth = 64;
    }
    else if (dateTime.Year == this.playerTalkMessage.created_at.Year)
    {
      this.Message = this.playerTalkMessage.created_at.ToString("M/d ({0})");
      this.Message = string.Format(this.Message, (object) SeaTalkCommon.GetJPWeek(this.playerTalkMessage.created_at));
      this.BackgroundWidth = 105;
    }
    else
    {
      this.Message = this.playerTalkMessage.created_at.ToString("yyyy年M月d日 ({0})");
      this.Message = string.Format(this.Message, (object) SeaTalkCommon.GetJPWeek(this.playerTalkMessage.created_at));
      this.BackgroundWidth = 200;
    }
  }

  public void UpdateDate() => this.CreateDate();

  private void CreateUnread() => this.BackgroundHeight = 30;

  private void CreateGift()
  {
    this.BackgroundHeight = 170;
    Singleton<CommonRoot>.GetInstance().StartCoroutine(this.SetGiftSprite());
    this.SendTime = this.playerTalkMessage.created_at.ToString("HH:mm");
  }

  private IEnumerator SetGiftSprite()
  {
    int gearId = this.playerTalkMessage.condition_id.Value;
    Sprite sprite;
    if (SeaTalkMessageInfo.gearCache.TryGetValue(gearId, out sprite))
    {
      this.GiftSprite = sprite;
    }
    else
    {
      GearGear gearGear;
      MasterData.GearGear.TryGetValue(gearId, out gearGear);
      Future<Sprite> spriteF = gearGear.LoadSpriteThumbnail();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      SeaTalkMessageInfo.gearCache[gearId] = spriteF.Result;
      this.GiftSprite = spriteF.Result;
      spriteF = (Future<Sprite>) null;
    }
  }

  public float UpperHeight
  {
    get
    {
      switch (this.MessageViewType)
      {
        case TalkMessageViewType.Partner:
        case TalkMessageViewType.Player:
        case TalkMessageViewType.Writing:
          return (float) (this.BackgroundHeight + 30);
        case TalkMessageViewType.Date:
        case TalkMessageViewType.Gift:
        case TalkMessageViewType.UnreadLine:
          return (float) this.BackgroundHeight;
        default:
          Debug.LogError((object) "想定していないTalkMessageViewTypeです");
          return 0.0f;
      }
    }
  }

  public float UnderHeight
  {
    get
    {
      switch (this.MessageViewType)
      {
        case TalkMessageViewType.Partner:
        case TalkMessageViewType.Writing:
          return string.IsNullOrEmpty(this.LimitTime) ? (float) (this.BackgroundHeight + 30) : (float) (this.BackgroundHeight + 80);
        case TalkMessageViewType.Player:
          return (float) (this.BackgroundHeight + 30);
        case TalkMessageViewType.Date:
        case TalkMessageViewType.Gift:
        case TalkMessageViewType.UnreadLine:
          return (float) this.BackgroundHeight;
        default:
          Debug.LogError((object) "想定していないTalkMessageViewTypeです");
          return 0.0f;
      }
    }
  }
}
