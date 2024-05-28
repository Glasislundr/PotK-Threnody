// Decompiled with JetBrains decompiler
// Type: Popup017181Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class Popup017181Menu : BattleBackButtonMenuBase
{
  [SerializeField]
  private UILabel label;

  public void Awake()
  {
    NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
    if (instance.isPvp || instance.isPvnpc)
      this.label.SetTextLocalize(Consts.GetInstance().BATTLE_ESCAPE_POPUP_PVP);
    else if (instance.isGvg && instance.battleInfo.battleId.Equals(string.Empty))
      this.label.SetTextLocalize(string.Empty);
    else
      this.label.SetTextLocalize(Consts.GetInstance().BATTLE_ESCAPE_POPUP);
  }

  public void IbtnYes()
  {
    this.battleManager.getController<BattleAIController>().stopAIAction();
    this.battleManager.getManager<BattleTimeManager>().setPhaseState(BL.Phase.surrender, true);
    this.battleManager.isRetire = true;
    Singleton<NGGameDataManager>.GetInstance().questAutoLap = false;
    this.battleManager.popupCloseAll();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo() => this.battleManager.popupDismiss();
}
