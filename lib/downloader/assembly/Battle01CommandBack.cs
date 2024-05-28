// Decompiled with JetBrains decompiler
// Type: Battle01CommandBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandBack : BattleBackButtonMenuBase
{
  private Battle01SelectNode selectNode;
  private PVPManager _pvpManager;

  private PVPManager pvpManager
  {
    get
    {
      if (Object.op_Equality((Object) this._pvpManager, (Object) null))
        this._pvpManager = Singleton<PVPManager>.GetInstance();
      return this._pvpManager;
    }
  }

  private void Awake()
  {
    EventDelegate.Set(((Component) this).GetComponent<UIButton>().onClick, new EventDelegate((MonoBehaviour) this, "onClick"));
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandBack battle01CommandBack = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01CommandBack.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) battle01CommandBack).transform);
    return false;
  }

  public override void onBackButton() => this.onClick();

  public bool OnBackButtonImmediate() => this._onClick(0.0f);

  public void FindSelectNode()
  {
    this.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) this).transform);
  }

  public void onClick() => this._onClick();

  private bool _onClick(float wait = 0.1f)
  {
    if (Singleton<PopupManager>.GetInstance().isOpen || !this.battleManager.isBattleEnable || this.battleManager.isPvp && this.env.core.phaseState.state != BL.Phase.pvp_wait_preparing && this.pvpManager.isSending || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel)
      return false;
    if (this.env.core.phaseState.state == BL.Phase.pvp_disposition && this.env.core.unitCurrent.unit != (BL.Unit) null)
      this.env.core.currentUnitPosition.cancelMove(this.env);
    this.selectNode.onBackWithWait(wait);
    return true;
  }
}
