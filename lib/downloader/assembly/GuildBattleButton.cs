// Decompiled with JetBrains decompiler
// Type: GuildBattleButton
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
public class GuildBattleButton : MypageEventButton
{
  [SerializeField]
  private GameObject mBattleTimeObj;
  [SerializeField]
  private GameObject mMatchingTimeObj;
  [SerializeField]
  private GameObject mAggregatingTimeObj;
  [SerializeField]
  private GameObject mRuleBoardObj;
  [SerializeField]
  private UILabel mBattleTimeRestLbl;
  [SerializeField]
  private UILabel mMatchingTimeRestLbl;
  [SerializeField]
  private UILabel mRuleNameLbl;
  private GvgStatus mGvgStatus;
  private string mRuleName;
  private IEnumerator mTimerTask;

  public override void UpdateButtonState()
  {
    PlayerAffiliation current = PlayerAffiliation.Current;
    this.mGvgStatus = current.guild.gvg_status;
    this.mRuleName = current.gvgPeriod?.rule_name;
    base.UpdateButtonState();
  }

  public override bool IsActive()
  {
    switch (this.mGvgStatus)
    {
      case GvgStatus.preparing:
        return true;
      case GvgStatus.fighting:
        return true;
      case GvgStatus.aggregating:
        return true;
      default:
        return false;
    }
  }

  public override bool IsBadge() => false;

  public override void SetActive(bool value)
  {
    this.StopTimerTask();
    switch (PlayerAffiliation.Current.guild.gvg_status)
    {
      case GvgStatus.preparing:
        this.mBattleTimeObj.SetActive(false);
        this.mMatchingTimeObj.SetActive(true);
        this.mAggregatingTimeObj.SetActive(false);
        this.StartGVGStartTimerTask();
        break;
      case GvgStatus.fighting:
        this.mBattleTimeObj.SetActive(true);
        this.mMatchingTimeObj.SetActive(false);
        this.mAggregatingTimeObj.SetActive(false);
        this.StartGVGEndTimerTask();
        break;
      case GvgStatus.aggregating:
        this.mBattleTimeObj.SetActive(false);
        this.mMatchingTimeObj.SetActive(false);
        this.mAggregatingTimeObj.SetActive(true);
        break;
      default:
        this.mBattleTimeObj.SetActive(false);
        this.mMatchingTimeObj.SetActive(false);
        this.mAggregatingTimeObj.SetActive(false);
        break;
    }
    if (string.IsNullOrEmpty(this.mRuleName))
    {
      this.mRuleBoardObj.SetActive(false);
    }
    else
    {
      this.mRuleNameLbl.SetTextLocalize(this.mRuleName);
      this.mRuleBoardObj.SetActive(true);
    }
  }

  private void StartGVGStartTimerTask()
  {
    this.mTimerTask = GuildUtil.TimeCountText(this.mMatchingTimeRestLbl, Consts.GetInstance().GUILD_TOP_REMAIN_TIME, (double) GuildUtil.GVGStartHour(), (Action) (() => this.UpdateButtonState()), (MonoBehaviour) this);
    this.StartCoroutine(this.mTimerTask);
  }

  private void StartGVGEndTimerTask()
  {
    this.mTimerTask = GuildUtil.TimeCountText(this.mBattleTimeRestLbl, Consts.GetInstance().GUILD_TOP_REMAIN_TIME, (double) GuildUtil.GVGEndHour(), (Action) (() => this.UpdateButtonState()), (MonoBehaviour) this);
    this.StartCoroutine(this.mTimerTask);
  }

  private void StopTimerTask()
  {
    if (this.mTimerTask == null)
      return;
    this.StopCoroutine(this.mTimerTask);
    this.mTimerTask = (IEnumerator) null;
  }

  private void OnDisable() => this.StopTimerTask();

  private void OnDestroy() => this.StopTimerTask();
}
