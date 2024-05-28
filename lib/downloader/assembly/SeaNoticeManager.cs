// Decompiled with JetBrains decompiler
// Type: SeaNoticeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SA.Foundation.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class SeaNoticeManager : MonoBehaviour
{
  public UIButton talkBtn;
  public UILabel unitName;
  public UILabel message;
  public GameObject body;
  private int messageID;
  private int sameCharacterID;
  private long timeStamp;

  public void Init(Sea030HomeMenu homeMenu)
  {
    this.messageID = Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.message_id;
    this.sameCharacterID = Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.same_character_id;
    this.timeStamp = SA_Unix_Time.ToUnixTime(Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.created_at);
    TalkUnitInfo talkUnitInfo = new TalkUnitInfo();
    talkUnitInfo.Init(this.sameCharacterID);
    this.talkBtn.onClick.Clear();
    this.talkBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      homeMenu.isChangeSelectSpot = true;
      SeaTalkMessageScene.ChangeScene(talkUnitInfo, homeMenu);
      this.Hide();
    })));
    this.unitName.SetTextLocalize(((IEnumerable<UnitUnit>) MasterData.UnitUnitList).First<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == this.sameCharacterID)).name);
    CallMessage callMessage;
    MasterData.CallMessage.TryGetValue(this.messageID, out callMessage);
    if (callMessage == null)
      return;
    this.message.text = callMessage.text;
    SeaTalkCommon.ProcessingComment(this.message);
    this.message.text = SeaTalkCommon.GetReplaceMessage(this.message.text, talkUnitInfo, Singleton<NGGameDataManager>.GetInstance().playerTalkMessage);
  }

  public void Show()
  {
    if (Persist.seaHomeUnitDate.Data.messageID == this.messageID && Persist.seaHomeUnitDate.Data.messageUnitID == this.sameCharacterID && Persist.seaHomeUnitDate.Data.timeStamp == this.timeStamp || ServerTime.NowAppTimeAddDelta().Subtract(Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.created_at).Seconds >= 600)
    {
      this.Hide();
    }
    else
    {
      this.body.SetActive(true);
      this.PlaySE();
      ((Behaviour) ((Component) this).gameObject.GetComponent<Animator>()).enabled = true;
      ((Component) this).gameObject.GetComponent<Animator>().Play("Notice_In_anim", -1, 0.0f);
      Persist.seaHomeUnitDate.Data.messageID = this.messageID;
      Persist.seaHomeUnitDate.Data.messageUnitID = this.sameCharacterID;
      Persist.seaHomeUnitDate.Data.timeStamp = this.timeStamp;
      Persist.seaHomeUnitDate.Flush();
    }
  }

  public void Hide()
  {
    ((Behaviour) ((Component) this).gameObject.GetComponent<Animator>()).enabled = false;
    this.body.SetActive(false);
  }

  private void PlaySE() => Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1072");
}
