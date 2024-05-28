// Decompiled with JetBrains decompiler
// Type: SeaTalkMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SeaTalkMessage : MonoBehaviour
{
  public static Color DEFAULT_LIMIT_ICON_COLOR = new Color(0.5019608f, 0.5019608f, 0.5019608f);
  [Header("姫、プレイヤー、日付のみ")]
  [SerializeField]
  private UISprite normalBalloon;
  [SerializeField]
  private UILabel comment;
  [Header("姫、プレイヤー、贈り物のみ")]
  [SerializeField]
  private Transform sendTimeParent;
  [SerializeField]
  private UILabel sendTime;
  [Header("姫のみ")]
  [SerializeField]
  private Transform unitIconParent;
  [SerializeField]
  private Transform messageTypeIconParent;
  [SerializeField]
  private UISprite messageTypeIcon;
  [SerializeField]
  private GameObject limitView;
  private Transform t_limitView;
  [SerializeField]
  private UISprite limitIcon;
  private Transform t_limitIcon;
  [SerializeField]
  private UILabel limitTime;
  [SerializeField]
  private List<TweenColor> limitTimeTweenColors;
  [SerializeField]
  private UISprite angryBalloon;
  [SerializeField]
  private Transform angryTypeIconParent;
  [SerializeField]
  private UISprite angryTypeIcon;
  [SerializeField]
  private Transform angrySendTimeParent;
  [SerializeField]
  private UILabel angrySendTime;
  [Header("贈り物のみ")]
  [SerializeField]
  private UI2DSprite giftSprite;
  public SeaTalkMessageInfo Info;
  public SeaTalkMessageInfo BeforeInfo;
  public GameObject BaseUnitIcon;
  private GameObject unitIcon;

  public UILabel Comment => this.comment;

  public void UpdateDate()
  {
    switch (this.Info.MessageViewType)
    {
      case TalkMessageViewType.Partner:
        this.SetPartner();
        break;
      case TalkMessageViewType.Player:
        this.SetPlayer();
        break;
      case TalkMessageViewType.Date:
        this.SetDate();
        break;
      case TalkMessageViewType.Gift:
        this.SetGift();
        break;
      case TalkMessageViewType.Writing:
        this.SetWriting();
        break;
    }
  }

  private void SetPartner()
  {
    this.comment.text = this.Info.Message;
    UISprite uiSprite;
    if (this.Info.IsBadMood)
    {
      ((Component) this.normalBalloon).gameObject.SetActive(false);
      ((Component) this.angryBalloon).gameObject.SetActive(true);
      ((UIWidget) this.angryBalloon).height = this.Info.BackgroundHeight + 30;
      uiSprite = this.angryTypeIcon;
      this.angryTypeIconParent.localPosition = new Vector3(this.angryTypeIconParent.localPosition.x, this.Info.TypeIconPositionY, 0.0f);
      this.angrySendTime.text = this.Info.SendTime;
      this.angrySendTimeParent.localPosition = new Vector3(this.angrySendTimeParent.localPosition.x, this.Info.TypeIconPositionY, 0.0f);
    }
    else
    {
      ((Component) this.normalBalloon).gameObject.SetActive(true);
      ((Component) this.angryBalloon).gameObject.SetActive(false);
      ((UIWidget) this.normalBalloon).height = this.Info.BackgroundHeight;
      uiSprite = this.messageTypeIcon;
      this.messageTypeIconParent.localPosition = new Vector3(this.messageTypeIconParent.localPosition.x, this.Info.TypeIconPositionY, 0.0f);
      this.sendTime.text = this.Info.SendTime;
      this.sendTimeParent.localPosition = new Vector3(this.sendTimeParent.localPosition.x, this.Info.TypeIconPositionY, 0.0f);
    }
    ((Component) uiSprite).gameObject.SetActive(true);
    switch (this.Info.callMessage.message_type_id)
    {
      case 2:
        uiSprite.spriteName = "icon_Reply.png__GUI__sea_talk__sea_talk_prefab";
        break;
      case 4:
      case 8:
        uiSprite.spriteName = "icon_Date.png__GUI__sea_talk__sea_talk_prefab";
        break;
      case 5:
        uiSprite.spriteName = "icon_Gift.png__GUI__sea_talk__sea_talk_prefab";
        break;
      case 10:
        uiSprite.spriteName = "icon_Angry.png__GUI__sea_talk__sea_talk_prefab";
        break;
      default:
        ((Component) uiSprite).gameObject.SetActive(false);
        break;
    }
    ((UIWidget) uiSprite).MakePixelPerfect();
    this.SetUnitIcon();
    if (this.Info.LimitTime == "")
    {
      this.limitView.SetActive(false);
      ((UIWidget) this.limitIcon).color = SeaTalkMessage.DEFAULT_LIMIT_ICON_COLOR;
      ((UIWidget) this.limitTime).color = Color.white;
      this.limitTimeTweenColors.ForEach((Action<TweenColor>) (x => ((Behaviour) x).enabled = false));
    }
    else
    {
      this.limitView.SetActive(true);
      if (Object.op_Equality((Object) this.t_limitView, (Object) null))
        this.t_limitView = this.limitView.transform;
      this.t_limitView.localPosition = new Vector3(this.t_limitView.localPosition.x, this.Info.LimitTimePositionY, 0.0f);
      this.limitTime.text = this.Info.LimitTime;
      if (this.Info.LimitTimeAnimation)
      {
        if (this.BeforeInfo != null && this.Info != this.BeforeInfo)
        {
          ((UIWidget) this.limitIcon).color = this.Info.KeepLimitTimeAnimationIconColor;
          ((UIWidget) this.limitTime).color = this.Info.KeepLimitTimeAnimationTimeColor;
        }
        foreach (Behaviour limitTimeTweenColor in this.limitTimeTweenColors)
          limitTimeTweenColor.enabled = true;
      }
      else
      {
        if (this.BeforeInfo != null && this.BeforeInfo.LimitTimeAnimation)
        {
          this.BeforeInfo.KeepLimitTimeAnimationIconColor = ((UIWidget) this.limitIcon).color;
          this.BeforeInfo.KeepLimitTimeAnimationTimeColor = ((UIWidget) this.limitTime).color;
        }
        ((UIWidget) this.limitIcon).color = SeaTalkMessage.DEFAULT_LIMIT_ICON_COLOR;
        ((UIWidget) this.limitTime).color = Color.white;
        foreach (Behaviour limitTimeTweenColor in this.limitTimeTweenColors)
          limitTimeTweenColor.enabled = false;
      }
      if (Object.op_Equality((Object) this.t_limitIcon, (Object) null))
        this.t_limitIcon = ((Component) this.limitIcon).transform;
      if ((double) this.Info.LimitTimeIconPositionX == 0.0)
        this.Info.LimitTimeIconPositionX = (float) -((double) ((UIWidget) this.limitTime).width - (double) ((Component) this.limitTime).transform.localPosition.x);
      this.t_limitIcon.localPosition = new Vector3(this.Info.LimitTimeIconPositionX, 1f, 0.0f);
    }
  }

  private void SetPlayer()
  {
    this.comment.text = this.Info.Message;
    ((UIWidget) this.normalBalloon).height = this.Info.BackgroundHeight;
    this.sendTime.text = this.Info.SendTime;
  }

  private void SetDate()
  {
    this.comment.text = this.Info.Message;
    ((UIWidget) this.normalBalloon).width = this.Info.BackgroundWidth;
  }

  private void SetGift()
  {
    this.sendTime.text = this.Info.SendTime;
    this.giftSprite.sprite2D = this.Info.GiftSprite;
  }

  private void SetWriting() => this.SetUnitIcon();

  private void SetUnitIcon()
  {
    if (Object.op_Equality((Object) this.unitIcon, (Object) null))
      this.unitIcon = this.BaseUnitIcon.Clone(this.unitIconParent);
    this.unitIcon.SetActive(true);
  }
}
