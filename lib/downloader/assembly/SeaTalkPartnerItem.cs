// Decompiled with JetBrains decompiler
// Type: SeaTalkPartnerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class SeaTalkPartnerItem : MonoBehaviour
{
  [SerializeField]
  private GameObject angryIcon;
  [SerializeField]
  private UISprite background;
  [SerializeField]
  private Transform unitIconParent;
  [SerializeField]
  private GameObject pledgeCompletedIcon;
  [SerializeField]
  private GameObject pledgeConcluIcon;
  [SerializeField]
  private UILabel partnerName;
  [SerializeField]
  private UILabel comment;
  [SerializeField]
  private UILabel sendTime;
  [SerializeField]
  private GameObject unreadParent;
  [SerializeField]
  private UILabel unreadLabel;
  [SerializeField]
  private GameObject receivableRewardBatch;
  private PlayerTalkPartner playerTalkPartner;
  private PlayerTalkPartner[] playerTalkPartners;
  private TalkUnitInfo talkUnitInfo = new TalkUnitInfo();

  public IEnumerator Init(
    PlayerTalkPartner playerTalkPartner,
    PlayerTalkPartner[] playerTalkPartners)
  {
    this.playerTalkPartner = playerTalkPartner;
    this.playerTalkPartners = playerTalkPartners;
    this.talkUnitInfo.Init(playerTalkPartner.letter.same_character_id);
    switch (playerTalkPartner.letter.call_status)
    {
      case 1:
        ((UIWidget) this.background).color = Color.white;
        this.pledgeCompletedIcon.SetActive(false);
        this.pledgeConcluIcon.SetActive(false);
        break;
      case 2:
        ((UIWidget) this.background).color = Color.white;
        this.pledgeCompletedIcon.SetActive(true);
        this.pledgeConcluIcon.SetActive(false);
        break;
      case 3:
        ((UIWidget) this.background).color = new Color(0.9647059f, 0.8745098f, 1f);
        ((UIWidget) this.partnerName).color = new Color(0.65882355f, 0.3372549f, 0.78039217f);
        this.pledgeCompletedIcon.SetActive(false);
        this.pledgeConcluIcon.SetActive(true);
        break;
    }
    if (playerTalkPartner.letter.mood_status == 3)
    {
      this.angryIcon.SetActive(true);
      ((UIWidget) this.background).color = new Color(1f, 0.7647059f, 0.7647059f);
      ((UIWidget) this.partnerName).color = new Color(1f, 0.0f, 0.235294119f);
    }
    else
      this.angryIcon.SetActive(false);
    yield return (object) this.CreateUnitIcon();
    this.partnerName.text = this.talkUnitInfo.unit.name;
    CallMessage callMessage;
    MasterData.CallMessage.TryGetValue(playerTalkPartner.message.message_id, out callMessage);
    if (callMessage != null)
    {
      this.comment.text = callMessage.text;
      SeaTalkCommon.ProcessingComment(this.comment);
      this.comment.text = SeaTalkCommon.GetReplaceMessage(this.comment.text, this.talkUnitInfo, playerTalkPartner.message);
    }
    this.sendTime.text = this.GetSendTime(playerTalkPartner.message.created_at);
    int num = Mathf.Min(playerTalkPartner.unread_count, 99);
    if (num <= 0)
    {
      this.unreadParent.SetActive(false);
    }
    else
    {
      this.unreadParent.SetActive(true);
      this.unreadLabel.text = string.Format("{0}", (object) num);
    }
    this.receivableRewardBatch.SetActive(playerTalkPartner.receivable_reward);
  }

  private IEnumerator CreateUnitIcon()
  {
    Future<GameObject> f = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIcon = f.Result.Clone(this.unitIconParent).GetComponent<UnitIcon>();
    e = unitIcon.SetUnit(this.talkUnitInfo.unit, this.talkUnitInfo.unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) unitIcon.Button).gameObject.SetActive(false);
    unitIcon.BottomBaseObject = false;
  }

  private string GetSendTime(DateTime created_at)
  {
    DateTime dateTime = ServerTime.NowAppTimeAddDelta();
    if (dateTime.Date == created_at.Date)
      return created_at.ToString("HH:mm");
    if (dateTime.AddDays(-1.0).Date == created_at.Date)
      return "昨日";
    if (dateTime.AddDays(-7.0).Date <= created_at.Date)
      return SeaTalkCommon.GetJPWeek(created_at) + "曜日";
    return dateTime.Year == created_at.Year ? created_at.ToString("M/d") : created_at.ToString("yyyy/MM/dd");
  }

  public void OnButton()
  {
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1040");
    SeaTalkMessageScene.ChangeScene(this.talkUnitInfo);
  }
}
