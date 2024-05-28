// Decompiled with JetBrains decompiler
// Type: BattleUI01ShortCutConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI01ShortCutConfig : BattleMonoBehaviour
{
  [SerializeField]
  [Tooltip("\"オート\"/\"簡易演出\"/\"戦闘速度変速\"ボタン群の先頭ノードをセット")]
  private NGTweenParts topConfig_;
  [SerializeField]
  [Tooltip("オート戦闘ボタンの先頭ノードをセット")]
  private GameObject topAutoBattle_;
  [SerializeField]
  [Tooltip("押すとオート戦闘が有効になるボタンをセット")]
  private UIButton btnEnabledAutoBattle_;
  [SerializeField]
  [Tooltip("押すとオート戦闘が無効になるボタンをセット")]
  private UIButton btnDisabledAutoBattle_;
  [SerializeField]
  [Tooltip("オートボタン無効期間中の表示")]
  private GameObject objWaitAutoBattle_;
  [SerializeField]
  [Tooltip("簡易戦闘ボタンの先頭ノードをセット")]
  private GameObject topSimpleBattle_;
  [SerializeField]
  [Tooltip("押すと簡易戦闘が有効になるボタンをセット")]
  private UIButton btnEnabledSimpleBattle_;
  [SerializeField]
  [Tooltip("押すと簡易戦闘が無効になるボタンをセット")]
  private UIButton btnDisabledSimpleBattle_;
  [SerializeField]
  [Tooltip("簡易戦闘ボタン無効期間中の表示")]
  private GameObject objWaitSimpleBattle_;
  [SerializeField]
  [Tooltip("戦闘進行速度変更オブジェクト先頭ノードをセット")]
  private GameObject topBattleSpeed_;
  private BL.StructValue<bool> requestAutoBattle_ = new BL.StructValue<bool>(false);
  private BL.StructValue<bool> requestSimpleBattle_ = new BL.StructValue<bool>(false);
  private Battle01SelectNode selectNode_;
  private bool isWait_;
  private bool isControlAll_;
  private bool isControlAutoBattle_;
  private bool isControlSimpleBattle_;
  private bool isControlBattleSpeed_;
  private bool coroutineAutoBattle_;
  private bool coroutineSimpleBattle_;
  private BL.BattleModified<BL.PhaseState> phaseStateModified;
  private const float WAIT_BUTTON_ON = 1f;

  public BL.BattleModified<BL.StructValue<bool>> requestAutoBattleModified_ { get; private set; }

  public BL.BattleModified<BL.StructValue<bool>> requestSimpleBattleModified_ { get; private set; }

  public bool isActive_
  {
    get => this.isControlAll_ && this.topConfig_.isActive;
    set
    {
      if (!this.isControlAll_ || this.topConfig_.isActive == value)
        return;
      this.topConfig_.isActive = value;
      this.isWait_ = true;
    }
  }

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUI01ShortCutConfig ui01ShortCutConfig = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ui01ShortCutConfig.requestAutoBattle_.value = ui01ShortCutConfig.env.core.isAutoBattle.value;
    ui01ShortCutConfig.requestSimpleBattle_.value = ui01ShortCutConfig.battleManager.noDuelScene;
    ui01ShortCutConfig.requestAutoBattleModified_ = new BL.BattleModified<BL.StructValue<bool>>(ui01ShortCutConfig.requestAutoBattle_);
    ui01ShortCutConfig.requestSimpleBattleModified_ = new BL.BattleModified<BL.StructValue<bool>>(ui01ShortCutConfig.requestSimpleBattle_);
    ui01ShortCutConfig.selectNode_ = ((Component) ui01ShortCutConfig).GetComponent<Battle01SelectNode>();
    ui01ShortCutConfig.isWait_ = false;
    ui01ShortCutConfig.phaseStateModified = BL.Observe<BL.PhaseState>(ui01ShortCutConfig.env.core.phaseState);
    if ((ui01ShortCutConfig.battleManager.isPvp || ui01ShortCutConfig.battleManager.isPvnpc) && !PerformanceConfig.GetInstance().EnablePvPAutoButton)
    {
      ui01ShortCutConfig.topAutoBattle_.gameObject.SetActive(false);
      ((Component) ui01ShortCutConfig.btnEnabledAutoBattle_).gameObject.SetActive(false);
      ((Component) ui01ShortCutConfig.btnDisabledAutoBattle_).gameObject.SetActive(false);
      ui01ShortCutConfig.requestAutoBattle_.value = false;
      ui01ShortCutConfig.isControlAutoBattle_ = false;
      ui01ShortCutConfig.topAutoBattle_ = (GameObject) null;
    }
    ui01ShortCutConfig.resetControlButtons(ui01ShortCutConfig.env.core.battleInfo.isAutoBattleEnable, true, true);
    if (ui01ShortCutConfig.isControlAutoBattle_)
      ui01ShortCutConfig.setAutoBattleButton(ui01ShortCutConfig.requestAutoBattle_.value, true);
    if (ui01ShortCutConfig.isControlSimpleBattle_)
      ui01ShortCutConfig.setSimpleBattleButton(ui01ShortCutConfig.requestSimpleBattle_.value, true);
    return false;
  }

  protected override void Update_Battle()
  {
    if (!this.isControlAll_)
      return;
    if (this.isWait_ && this.topConfig_.isActive == ((Component) this.topConfig_).gameObject.activeSelf)
      this.isWait_ = false;
    if (!this.isControlSimpleBattle_ || this.requestSimpleBattle_.value == this.battleManager.noDuelScene || this.requestSimpleBattleModified_.isChanged)
      return;
    if (this.coroutineSimpleBattle_)
    {
      this.StopCoroutine("doWaitEnabledSimpleBattleButton");
      this.coroutineSimpleBattle_ = false;
    }
    this.setRequestSimpleBattle(this.battleManager.noDuelScene, true);
  }

  private void OnEnable()
  {
    this.recoveryAutoBattleButton();
    this.recoverySimpleBattleButton();
  }

  private void recoveryAutoBattleButton()
  {
    if (!this.coroutineAutoBattle_)
      return;
    this.coroutineAutoBattle_ = false;
    ((Component) this.btnEnabledAutoBattle_).gameObject.SetActive(true);
    this.objWaitAutoBattle_.SetActive(false);
  }

  private void recoverySimpleBattleButton()
  {
    if (!this.coroutineSimpleBattle_)
      return;
    this.coroutineSimpleBattle_ = false;
    ((Component) this.btnEnabledSimpleBattle_).gameObject.SetActive(true);
    this.objWaitSimpleBattle_.SetActive(false);
  }

  public void resetControlButtons(
    bool enabledAutoBattle,
    bool enabledSimpleBattle,
    bool enabledBattleSpeed)
  {
    bool flag = !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish();
    if (flag)
    {
      enabledAutoBattle = false;
      enabledSimpleBattle = false;
    }
    this.isControlAutoBattle_ = enabledAutoBattle && Object.op_Inequality((Object) this.topAutoBattle_, (Object) null) && Object.op_Inequality((Object) this.btnEnabledAutoBattle_, (Object) null) && Object.op_Inequality((Object) this.btnDisabledAutoBattle_, (Object) null) && Object.op_Inequality((Object) this.objWaitAutoBattle_, (Object) null);
    if (!this.isControlAutoBattle_ && Object.op_Inequality((Object) this.topAutoBattle_, (Object) null))
    {
      if (flag)
        ((IEnumerable<UIButton>) ((Component) this.topAutoBattle_.transform).GetComponentsInChildren<UIButton>()).ForEach<UIButton>((Action<UIButton>) (x => ((UIButtonColor) x).isEnabled = false));
      else
        this.topAutoBattle_.SetActive(false);
    }
    this.isControlSimpleBattle_ = enabledSimpleBattle && Object.op_Inequality((Object) this.topSimpleBattle_, (Object) null) && Object.op_Inequality((Object) this.btnEnabledSimpleBattle_, (Object) null) && Object.op_Inequality((Object) this.btnDisabledSimpleBattle_, (Object) null) && Object.op_Inequality((Object) this.objWaitSimpleBattle_, (Object) null);
    if (!this.isControlSimpleBattle_ && Object.op_Inequality((Object) this.topSimpleBattle_, (Object) null))
    {
      if (flag)
        ((IEnumerable<UIButton>) ((Component) this.topSimpleBattle_.transform).GetComponentsInChildren<UIButton>()).ForEach<UIButton>((Action<UIButton>) (x => ((UIButtonColor) x).isEnabled = false));
      else
        this.topSimpleBattle_.SetActive(false);
    }
    this.isControlBattleSpeed_ = enabledBattleSpeed && Object.op_Inequality((Object) this.topBattleSpeed_, (Object) null);
    if (!this.isControlBattleSpeed_ && Object.op_Inequality((Object) this.topBattleSpeed_, (Object) null))
      this.topBattleSpeed_.SetActive(false);
    this.isControlAll_ = Object.op_Inequality((Object) this.topConfig_, (Object) null) && Object.op_Inequality((Object) this.selectNode_, (Object) null) && (this.isControlAutoBattle_ || this.isControlSimpleBattle_ || this.isControlBattleSpeed_);
    if (!Object.op_Inequality((Object) this.topConfig_, (Object) null))
      return;
    if (this.isControlAll_)
      this.topConfig_.forceActive(true);
    else
      this.topConfig_.resetActive(false);
  }

  public void onClickedAutoBattleEnabled() => this.setRequestAutoBattle(true);

  public void onClickedAutoBattleDisabled() => this.setRequestAutoBattle(false);

  private void setRequestAutoBattle(bool isAuto, bool isInit = false)
  {
    if (Object.op_Inequality((Object) this.selectNode_, (Object) null) && (this.selectNode_.IsPush || this.selectNode_.IsForceAutoDisable) || this.isWait_ || !this.isControlAutoBattle_)
      return;
    if (this.requestAutoBattle_.value != isAuto)
    {
      this.requestAutoBattle_.value = isAuto;
      this.setAutoBattleButton(isAuto, isInit);
    }
    else if (isInit)
      this.setAutoBattleButton(isAuto, isInit);
    if (!isInit)
      return;
    this.requestAutoBattleModified_.isChangedOnce();
  }

  private void setAutoBattleButton(bool isAuto, bool isInit)
  {
    UIButton btn;
    if (isAuto)
    {
      ((Component) this.btnEnabledAutoBattle_).gameObject.SetActive(false);
      btn = this.btnDisabledAutoBattle_;
    }
    else
    {
      btn = this.btnEnabledAutoBattle_;
      ((Component) this.btnDisabledAutoBattle_).gameObject.SetActive(false);
    }
    if (isInit)
    {
      ((Component) btn).gameObject.SetActive(true);
      this.objWaitAutoBattle_.SetActive(false);
    }
    else
      this.StartCoroutine(this.doWaitEnabledAutoBattleButton(btn));
  }

  private IEnumerator doWaitEnabledAutoBattleButton(UIButton btn)
  {
    if (Object.op_Equality((Object) btn, (Object) this.btnEnabledAutoBattle_))
    {
      this.coroutineAutoBattle_ = true;
      ((Component) btn).gameObject.SetActive(false);
      this.objWaitAutoBattle_.SetActive(true);
      yield return (object) new WaitForSeconds(1f);
      ((Component) btn).gameObject.SetActive(true);
    }
    else
      ((Component) btn).gameObject.SetActive(true);
    this.objWaitAutoBattle_.SetActive(false);
    this.coroutineAutoBattle_ = false;
  }

  public void onClickedAutoBattleEnabledForOvO() => this.setRequestAutoBattleForOvO(true);

  public void onClickedAutoBattleDisabledForOvO() => this.setRequestAutoBattleForOvO(false);

  private void setRequestAutoBattleForOvO(bool isAuto, bool isInit = false)
  {
    if (Object.op_Inequality((Object) this.selectNode_, (Object) null) && (this.selectNode_.IsPush || this.selectNode_.IsForceAutoDisable) || this.isWait_ || !this.isControlAutoBattle_)
      return;
    if (this.requestAutoBattle_.value != isAuto | isInit)
      this.setAutoBattleButtonForOvO(isAuto, isInit);
    if (!isInit)
      return;
    this.requestAutoBattleModified_.isChangedOnce();
  }

  private void setAutoBattleButtonForOvO(bool isAuto, bool isInit)
  {
    UIButton btn;
    if (isAuto)
    {
      ((Component) this.btnEnabledAutoBattle_).gameObject.SetActive(false);
      btn = this.btnDisabledAutoBattle_;
    }
    else
    {
      btn = this.btnEnabledAutoBattle_;
      ((Component) this.btnDisabledAutoBattle_).gameObject.SetActive(false);
    }
    if (isInit)
    {
      if (this.requestAutoBattle_.value != isAuto)
        this.requestAutoBattle_.value = isAuto;
      ((Component) btn).gameObject.SetActive(true);
      this.objWaitAutoBattle_.SetActive(false);
    }
    else
      this.StartCoroutine(this.doWaitEnabledAutoBattleButtonForOvO(btn, isAuto));
  }

  private IEnumerator doWaitEnabledAutoBattleButtonForOvO(UIButton btn, bool isAuto)
  {
    BattleUI01ShortCutConfig ui01ShortCutConfig = this;
    if (Object.op_Equality((Object) btn, (Object) ui01ShortCutConfig.btnEnabledAutoBattle_))
    {
      ui01ShortCutConfig.coroutineAutoBattle_ = true;
      ((Component) btn).gameObject.SetActive(false);
      ui01ShortCutConfig.objWaitAutoBattle_.SetActive(true);
      while (true)
      {
        while (ui01ShortCutConfig.phaseStateModified.isChangedOnce())
        {
          switch (ui01ShortCutConfig.phaseStateModified.value.state)
          {
            case BL.Phase.player_start:
            case BL.Phase.pvp_player_start:
              if (ui01ShortCutConfig.env.core.playerActionUnits.value.FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.cantChangeCurrent)) == null)
                goto case BL.Phase.enemy_start;
              else
                continue;
            case BL.Phase.enemy_start:
            case BL.Phase.enemy:
            case BL.Phase.player_end:
            case BL.Phase.enemy_end:
            case BL.Phase.pvp_enemy_start:
              ui01ShortCutConfig.requestAutoBattle_.value = isAuto;
              yield return (object) new WaitForSeconds(1f);
              ((Component) btn).gameObject.SetActive(true);
              goto label_8;
            default:
              goto label_5;
          }
        }
label_5:
        yield return (object) null;
      }
    }
    else
    {
      ui01ShortCutConfig.requestAutoBattle_.value = isAuto;
      ((Component) btn).gameObject.SetActive(true);
    }
label_8:
    ui01ShortCutConfig.objWaitAutoBattle_.SetActive(false);
    ui01ShortCutConfig.coroutineAutoBattle_ = false;
  }

  public void onClickedSimpleBattleEnabled() => this.setRequestSimpleBattle(true);

  public void onClickedSimpleBattleDisabled() => this.setRequestSimpleBattle(false);

  private void setRequestSimpleBattle(bool isSimple, bool isInit = false)
  {
    if (this.isWait_ || !this.isControlSimpleBattle_)
      return;
    if (this.requestSimpleBattle_.value != isSimple)
    {
      this.requestSimpleBattle_.value = isSimple;
      this.setSimpleBattleButton(isSimple, isInit);
    }
    else if (isInit)
      this.setSimpleBattleButton(isSimple, isInit);
    if (!isInit)
      return;
    this.requestSimpleBattleModified_.isChangedOnce();
  }

  private void setSimpleBattleButton(bool isSimple, bool isInit)
  {
    UIButton uiButton;
    if (isSimple)
    {
      ((Component) this.btnEnabledSimpleBattle_).gameObject.SetActive(false);
      uiButton = this.btnDisabledSimpleBattle_;
    }
    else
    {
      uiButton = this.btnEnabledSimpleBattle_;
      ((Component) this.btnDisabledSimpleBattle_).gameObject.SetActive(false);
    }
    if (isInit)
    {
      ((Component) uiButton).gameObject.SetActive(true);
      this.objWaitSimpleBattle_.SetActive(false);
    }
    else
      this.StartCoroutine("doWaitEnabledSimpleBattleButton", (object) uiButton);
  }

  private IEnumerator doWaitEnabledSimpleBattleButton(UIButton btn)
  {
    if (Object.op_Equality((Object) btn, (Object) this.btnEnabledSimpleBattle_))
    {
      this.coroutineSimpleBattle_ = true;
      ((Component) btn).gameObject.SetActive(false);
      this.objWaitSimpleBattle_.SetActive(true);
      yield return (object) new WaitForSeconds(1f);
      ((Component) btn).gameObject.SetActive(true);
    }
    else
      ((Component) btn).gameObject.SetActive(true);
    this.objWaitSimpleBattle_.SetActive(false);
    this.coroutineSimpleBattle_ = false;
  }
}
