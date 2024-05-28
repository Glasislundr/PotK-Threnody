// Decompiled with JetBrains decompiler
// Type: HenshinContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class HenshinContainer : MonoBehaviour
{
  [SerializeField]
  private GameObject dirAlways_;
  [SerializeField]
  private GameObject dirBefore_;
  [SerializeField]
  private GameObject dirAfter_;
  [SerializeField]
  private HenshinContainer.Schedule[] schedule_;
  private bool bReady_;
  private bool bReset_ = true;
  private bool bCompleted_;
  private int indexSchedule_;
  private float wait_;

  public GameObject dirAlways => this.dirAlways_;

  public GameObject dirBefore => this.dirBefore_;

  public GameObject dirAfter => this.dirAfter_;

  private void Awake()
  {
    this.bReady_ = this.schedule_ != null && this.schedule_.Length != 0;
    this.resetScheduler();
  }

  private void resetScheduler()
  {
    if (!this.bReady_)
      return;
    foreach (HenshinContainer.Schedule schedule in this.schedule_)
      schedule.isFirst_ = true;
  }

  public void resetHenshin()
  {
    this.bReset_ = true;
    this.bCompleted_ = false;
    this.resetScheduler();
  }

  public void skipHenshin()
  {
    if (!this.bReady_)
      return;
    this.bReset_ = false;
    this.bCompleted_ = true;
    this.indexSchedule_ = this.schedule_.Length - 1;
    this.initHenshin(this.schedule_[this.indexSchedule_]);
  }

  public bool updateHenshin()
  {
    if (!this.bReady_ || this.bCompleted_)
      return false;
    if (this.bReset_)
    {
      this.bReset_ = false;
      this.updateScheduler(true);
      this.initHenshin(this.schedule_[0]);
    }
    else if ((double) this.wait_ <= (double) Time.time)
    {
      this.updateScheduler(false);
      this.initHenshin(this.schedule_.Length > this.indexSchedule_ ? this.schedule_[this.indexSchedule_] : (HenshinContainer.Schedule) null);
    }
    return !this.bCompleted_;
  }

  private void updateScheduler(bool binitialize)
  {
    if (binitialize)
    {
      this.indexSchedule_ = 0;
    }
    else
    {
      HenshinContainer.Schedule schedule = this.schedule_[this.indexSchedule_];
      if (schedule.isGoto_ && schedule.gotoCount_ > 0 && this.schedule_.Length > schedule.gotoIndex_)
      {
        if (schedule.isFirst_)
        {
          schedule.isFirst_ = false;
          schedule.count_ = 1;
          this.indexSchedule_ = schedule.gotoIndex_;
        }
        else if (schedule.count_ < schedule.gotoCount_)
        {
          ++schedule.count_;
          this.indexSchedule_ = schedule.gotoIndex_;
        }
        else
          ++this.indexSchedule_;
      }
      else
        ++this.indexSchedule_;
      if (this.schedule_.Length > this.indexSchedule_)
        return;
      this.bCompleted_ = true;
      this.indexSchedule_ = this.schedule_.Length - 1;
    }
  }

  private void initHenshin(HenshinContainer.Schedule s)
  {
    if (s != null)
    {
      this.wait_ = s.life_ + Time.time;
      this.setUnitSetting(this.dirAlways, s.always_);
      this.setUnitSetting(this.dirBefore, s.before_);
      this.setUnitSetting(this.dirAfter, s.after_);
    }
    else
    {
      if (Object.op_Inequality((Object) this.dirAlways, (Object) null))
        this.dirAlways.SetActive(true);
      this.dirBefore.SetActive(false);
      this.dirAfter.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
      this.dirAfter.SetActive(true);
      this.bCompleted_ = true;
    }
  }

  private void setUnitSetting(GameObject go, HenshinContainer.UnitSetting usetting)
  {
    if (Object.op_Equality((Object) go, (Object) null))
      return;
    if (usetting.control_ == HenshinContainer.ControlType.Position)
    {
      go.SetActive(true);
      go.transform.localPosition = Vector2.op_Implicit(usetting.position_);
    }
    else
      go.SetActive(usetting.isEnable_);
  }

  private enum ControlType
  {
    Active,
    Position,
  }

  [Serializable]
  private class UnitSetting
  {
    public HenshinContainer.ControlType control_;
    public bool isEnable_ = true;
    public Vector2 position_ = Vector2.zero;
  }

  [Serializable]
  private class Schedule
  {
    public float life_;
    public HenshinContainer.UnitSetting always_ = new HenshinContainer.UnitSetting();
    public HenshinContainer.UnitSetting before_ = new HenshinContainer.UnitSetting();
    public HenshinContainer.UnitSetting after_ = new HenshinContainer.UnitSetting();
    public bool isGoto_;
    public int gotoIndex_;
    public int gotoCount_;
    [NonSerialized]
    public bool isFirst_ = true;
    [NonSerialized]
    public int count_;
  }
}
