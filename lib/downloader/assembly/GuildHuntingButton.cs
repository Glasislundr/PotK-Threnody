// Decompiled with JetBrains decompiler
// Type: GuildHuntingButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class GuildHuntingButton : MypageEventButton
{
  [SerializeField]
  private UIButton mButton;
  [SerializeField]
  private GameObject mOnTimeObj;
  [SerializeField]
  private GameObject mOffTimeObj;
  [SerializeField]
  private GameObject mRewardTimeObj;
  [SerializeField]
  private UILabel mOnTimeRestLbl;
  [SerializeField]
  private UILabel mRewardTimeRestLbl;
  private EventInfo mEventInfo;
  private DateTime mTime;

  public void UpdateButtonState(EventInfo eventInfo)
  {
    this.mEventInfo = eventInfo;
    this.mTime = ServerTime.NowAppTimeAddDelta();
    this.UpdateButtonState();
  }

  public override bool IsActive()
  {
    return this.mEventInfo != null && this.mEventInfo.start_at.CompareTo(this.mTime) < 0 && this.mEventInfo.final_at.CompareTo(this.mTime) > 0;
  }

  public override bool IsBadge() => this.mEventInfo.is_bonus_term;

  public override void SetActive(bool value)
  {
    if (value)
    {
      ((UIButtonColor) this.mButton).isEnabled = true;
      this.mOffTimeObj.SetActive(false);
      if (this.mEventInfo.end_at.CompareTo(this.mTime) < 0)
      {
        TimeSpan self = this.mEventInfo.final_at - this.mTime;
        this.mRewardTimeObj.SetActive(true);
        this.mRewardTimeRestLbl.SetTextLocalize(self.DisplayString());
        this.mOnTimeObj.SetActive(false);
      }
      else
      {
        TimeSpan self = this.mEventInfo.end_at - this.mTime;
        this.mOnTimeObj.SetActive(true);
        this.mOnTimeRestLbl.SetTextLocalize(self.DisplayStringForGuildHunting());
        this.mRewardTimeObj.SetActive(false);
      }
    }
    else
    {
      ((UIButtonColor) this.mButton).isEnabled = false;
      this.mOffTimeObj.SetActive(true);
      this.mRewardTimeObj.SetActive(false);
      this.mOnTimeObj.SetActive(false);
    }
  }
}
