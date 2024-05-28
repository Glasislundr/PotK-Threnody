// Decompiled with JetBrains decompiler
// Type: CommonSeaHeaderExp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class CommonSeaHeaderExp : CommonHeaderBase
{
  private GameObject popupPlayerStatus;
  [SerializeField]
  private Transform popupState;
  private UIWidget popupStateWidget;
  private TweenAlpha popupAnimation;
  private SeaHeaderPopupPlayerStatus playerStatus;
  private bool createPrefab;
  protected Modified<PlayerAffiliation> playerAffiliation;
  protected Modified<SeaPlayer> seaPlayer;

  private void Awake()
  {
    this.createPrefab = false;
    this.popupStateWidget = ((Component) this.popupState).GetComponent<UIWidget>();
    this.popupAnimation = ((Component) this.popupState).GetComponent<TweenAlpha>();
    this.playerAffiliation = SMManager.Observe<PlayerAffiliation>();
    this.seaPlayer = SMManager.Observe<SeaPlayer>();
    this.StartCoroutine(this.CreatePrefab());
  }

  private void Start() => this.Init();

  private void OnPress(bool isDown)
  {
    if (isDown)
    {
      this.SetTextNextExpToNowLv();
      this.AnimSet(1f);
    }
    else
      this.AnimSet(0.0f);
  }

  private IEnumerator CreatePrefab()
  {
    CommonSeaHeaderExp commonSeaHeaderExp = this;
    if (Object.op_Inequality((Object) commonSeaHeaderExp.popupPlayerStatus, (Object) null))
      Object.Destroy((Object) commonSeaHeaderExp.popupPlayerStatus);
    commonSeaHeaderExp.popupPlayerStatus = (GameObject) null;
    Future<GameObject> featureObj = Res.Prefabs.mypage.popup_PlayerStatus_sea.Load<GameObject>();
    IEnumerator e = featureObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    commonSeaHeaderExp.popupPlayerStatus = featureObj.Result.Clone(commonSeaHeaderExp.popupState);
    commonSeaHeaderExp.playerStatus = commonSeaHeaderExp.popupPlayerStatus.GetComponent<SeaHeaderPopupPlayerStatus>();
    commonSeaHeaderExp.ap = commonSeaHeaderExp.playerStatus.GetCommonAP();
    commonSeaHeaderExp.bp = commonSeaHeaderExp.playerStatus.GetCommonDP();
    commonSeaHeaderExp.createPrefab = true;
  }

  private void AnimSet(float to)
  {
    if (Object.op_Equality((Object) this.popupAnimation, (Object) null) || Object.op_Equality((Object) this.popupStateWidget, (Object) null))
    {
      Debug.LogWarning((object) "初期化できてない");
    }
    else
    {
      this.popupAnimation.to = to;
      ((Behaviour) this.popupAnimation).enabled = false;
      this.popupAnimation.from = ((UIRect) this.popupStateWidget).alpha;
      ((UITweener) this.popupAnimation).duration = Mathf.Abs(this.popupAnimation.to - this.popupAnimation.from) / 4f;
      ((UITweener) this.popupAnimation).ResetToBeginning();
      ((Behaviour) this.popupAnimation).enabled = true;
    }
  }

  private void SetItemCount(int now_itemCount = 0, int max_ItemCount = 0)
  {
    this.playerStatus.SetItemCount(now_itemCount, max_ItemCount);
  }

  private void SetUnitCount(int now_UnitCount = 0, int max_UnitCount = 0)
  {
    this.playerStatus.SetUnitCount(now_UnitCount, max_UnitCount);
  }

  private void SetTextNextExpToNowLv() => this.playerStatus.SetPlayerStatus(this.player.Value);

  private void UpdateDpRecoveryTime()
  {
    if (!this.isChangedOnceTimeCounter)
      return;
    this.bp.setTime(this.timeCounter.Value.DpRecoverySecondsPerPoint);
  }

  private bool UpdateDpPoint()
  {
    if (this.seaPlayer.IsChangedOnce())
      this.bp.setValue(this.seaPlayer.Value.dp);
    return this.isChangedOncePlayer;
  }

  protected override void Update()
  {
    if (Object.op_Equality((Object) this.ap, (Object) null) || Object.op_Equality((Object) this.bp, (Object) null) || !this.createPrefab || this.player.Value == null)
      return;
    base.Update();
    this.UpdateApRecoveryTime();
    this.UpdateDpRecoveryTime();
    if ((this.UpdateUnits() || this.isChangedOncePlayer) && this.units.Value != null)
      this.SetUnitCount(this.units.Value.Length, this.player.Value.max_units);
    if ((this.UpdateItems() || this.isChangedOncePlayer) && this.items.Value != null)
      this.SetItemCount(((IEnumerable<PlayerItem>) this.items.Value).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).ToArray<PlayerItem>().Length, this.player.Value.max_items);
    this.UpdateApGauge();
    if (this.playerAffiliation.IsChangedOnce())
      this.playerStatus.SetGuildStatus(this.playerAffiliation.Value);
    if (this.seaPlayer.Value == null)
      return;
    this.UpdateDpPoint();
  }
}
