// Decompiled with JetBrains decompiler
// Type: CommonHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using UnityEngine;

#nullable disable
public class CommonHeader : CommonHeaderBase
{
  public CommonHeaderLevel level;
  public UILabel jobText;
  public UILabel moneyText;
  public UILabel playerText;
  public UILabel pointText;
  public UILabel stoneText;
  public UILabel _lbl_date;
  public UILabel _lbl_time;
  public UILabel _lbl_wday;
  public UISprite deviceBatteryIcon;
  public UISprite deviceBatteryGauge;
  private DateTime _last_datetime;
  public static string[] _wday_label = new string[7]
  {
    "SUN",
    "MON",
    "TUE",
    "WED",
    "THU",
    "FRI",
    "SAT"
  };
  public CommonHeaderChat headerChat;
  [SerializeField]
  private TweenPosition entryTweenAnime;

  private void Awake()
  {
    ((Component) ((Component) this).transform.Find("DebugContainer")).gameObject.SetActive(false);
  }

  private void Start()
  {
    this.Init();
    if (Object.op_Inequality((Object) this._lbl_date, (Object) null))
      this.UpdateDateTime(DateTime.Now);
    ((Component) this.deviceBatteryIcon).gameObject.SetActive(false);
    ((Component) this.deviceBatteryGauge).gameObject.SetActive(false);
  }

  protected void UpdateDateTime(DateTime now)
  {
    this.setText(this._lbl_date, string.Format("{0:00}/{1:00}", (object) now.Month, (object) now.Day));
    this.setText(this._lbl_time, string.Format("{0:00}:{1:00}", (object) now.Hour, (object) now.Minute));
    this.setText(this._lbl_wday, CommonHeader._wday_label[(int) now.DayOfWeek]);
    this._last_datetime = now;
    this._last_datetime = this._last_datetime.AddMilliseconds((double) -now.Millisecond);
    this._last_datetime = this._last_datetime.AddSeconds((double) -now.Second);
  }

  private void ShowBatteryInfo()
  {
    ((Component) this.deviceBatteryIcon).gameObject.SetActive(true);
    ((Component) this.deviceBatteryGauge).gameObject.SetActive(true);
    this.deviceBatteryGauge.fillAmount = SystemInfo.batteryLevel;
    if ((double) SystemInfo.batteryLevel <= 0.20000000298023224)
    {
      ((UIWidget) this.deviceBatteryIcon).color = Color.yellow;
      ((UIWidget) this.deviceBatteryGauge).color = Color.yellow;
    }
    else
    {
      ((UIWidget) this.deviceBatteryIcon).color = Color.gray;
      ((UIWidget) this.deviceBatteryGauge).color = Color.gray;
    }
  }

  protected override void Update()
  {
    if (this.player.Value == null)
      return;
    base.Update();
    if (this.UpdateApGauge())
    {
      Player player = this.player.Value;
      this.bp.setValue(player.bp);
      this.level.setLevel(player.level);
      this.level.setExperience(player.exp, player.exp_next + player.exp);
      if (Persist.tutorial.Data.IsFinishTutorial())
        this.setText(this.playerText, player.name);
      else
        this.setText(this.playerText, Persist.tutorial.Data.PlayerName);
      this.setText(this.moneyText, string.Concat((object) player.money));
      this.setText(this.pointText, string.Concat((object) player.medal));
      this.setText(this.stoneText, string.Concat((object) player.coin));
    }
    if (!Object.op_Inequality((Object) this._lbl_date, (Object) null))
      return;
    DateTime now = DateTime.Now;
    if ((now - this._last_datetime).TotalMinutes < 1.0)
      return;
    this.UpdateDateTime(now);
  }

  public void PlayEntryAnime() => ((UITweener) this.entryTweenAnime).PlayForward();

  public void PlayExitAnime() => ((UITweener) this.entryTweenAnime).PlayReverse();
}
