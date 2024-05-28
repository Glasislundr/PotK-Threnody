// Decompiled with JetBrains decompiler
// Type: Battle01PVPNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01PVPNode : BattleMonoBehaviour
{
  [SerializeField]
  private NGTweenParts[] pvp_disposition_nodes;
  [SerializeField]
  private NGTweenParts[] pvp_battle_nodes;
  [SerializeField]
  private Vector3 cameraOffset;
  [SerializeField]
  private UIButton btnArea;
  [SerializeField]
  private Battle01PVPDispositionDecide dispositionDecide;
  private BL.BattleModified<BL.PhaseState> phaseStateModified;
  private bool isDisposition;
  private bool isDead;
  private BattleInputObserver _inputObserver;
  private BattleFieldAttribute _fieldAttribute;
  private Battle01PVPHeader _battle01PVPHeader;
  private const string dir_EffectOwn_Leader_Path = "Prefabs/Pvp/dir_EffectOwn_Leader";
  private const string dir_EffectEnemy_Leader_Path = "Prefabs/Pvp/dir_EffectEnemy_Leader";

  public Battle01PVPDispositionDecide DispositionDecide => this.dispositionDecide;

  private BattleInputObserver inputObserver
  {
    get
    {
      if (Object.op_Equality((Object) this._inputObserver, (Object) null))
        this._inputObserver = this.battleManager.getController<BattleInputObserver>();
      return this._inputObserver;
    }
  }

  private BattleFieldAttribute fieldAttribute
  {
    get
    {
      if (Object.op_Equality((Object) this._fieldAttribute, (Object) null))
        this._fieldAttribute = this.battleManager.getController<BattleFieldAttribute>();
      return this._fieldAttribute;
    }
  }

  public Battle01PVPHeader PVPHeader
  {
    get
    {
      if (Object.op_Equality((Object) this._battle01PVPHeader, (Object) null))
        this._battle01PVPHeader = ((Component) this.pvp_battle_nodes[0]).GetComponent<Battle01PVPHeader>();
      return this._battle01PVPHeader;
    }
  }

  public override IEnumerator onInitAsync()
  {
    this.PVPHeader.PlayerGauge.InitLeaderEffect("dir_EffectOwn_Leader", "player");
    this.PVPHeader.EnemyGauge.InitLeaderEffect("dir_EffectEnemy_Leader", "enemy");
    yield break;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPNode battle01PvpNode = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!battle01PvpNode.battleManager.isOvo)
      return false;
    battle01PvpNode.phaseStateModified = BL.Observe<BL.PhaseState>(battle01PvpNode.env.core.phaseState);
    foreach (NGTweenParts pvpDispositionNode in battle01PvpNode.pvp_disposition_nodes)
      pvpDispositionNode.isActive = true;
    ((UIButtonColor) battle01PvpNode.btnArea).isEnabled = false;
    return false;
  }

  protected override void Update_Battle()
  {
    if (this.isDead || !this.battleManager.isOvo || !this.phaseStateModified.isChangedOnce())
      return;
    switch (this.phaseStateModified.value.state)
    {
      case BL.Phase.pvp_move_unit_waiting:
      case BL.Phase.pvp_player_start:
      case BL.Phase.pvp_enemy_start:
      case BL.Phase.pvp_result:
      case BL.Phase.pvp_disposition:
      case BL.Phase.pvp_start_init:
      case BL.Phase.pvp_exception:
        bool flag = this.phaseStateModified.value.state == BL.Phase.pvp_disposition;
        if (!flag)
        {
          foreach (NGTweenParts pvpDispositionNode in this.pvp_disposition_nodes)
            pvpDispositionNode.isActive = flag;
          ((UIButtonColor) this.btnArea).isEnabled = true;
        }
        foreach (NGTweenParts pvpBattleNode in this.pvp_battle_nodes)
          pvpBattleNode.isActive = !flag;
        if (flag)
        {
          foreach (BL.Panel panel in this.battleManager.gameEngine.formationPanel)
            panel.setAttribute(BL.PanelAttribute.playermove);
          this.setCameraTarget(this.battleManager.gameEngine.formationPanel);
          this.inputObserver.setDispositionMode(this.battleManager.gameEngine.formationPanel);
        }
        else
        {
          foreach (BL.Panel panel in this.battleManager.gameEngine.formationPanel)
            panel.unsetAttribute(BL.PanelAttribute.playermove);
          this.inputObserver.setDispositionMode((HashSet<BL.Panel>) null);
          if (this.isDisposition)
          {
            this.battleManager.getManager<BattleTimeManager>().setCurrentUnit((BL.Unit) null);
            this.battleManager.saveEnvironment(true);
            this.dispositionNodeDestroy();
            break;
          }
        }
        this.isDisposition = flag;
        break;
      case BL.Phase.pvp_restart:
        foreach (NGTweenParts pvpDispositionNode in this.pvp_disposition_nodes)
          pvpDispositionNode.isActive = false;
        foreach (NGTweenParts pvpBattleNode in this.pvp_battle_nodes)
          pvpBattleNode.isActive = true;
        this.dispositionNodeDestroy();
        break;
    }
  }

  private void dispositionNodeDestroy()
  {
    this.isDead = true;
    this.StartCoroutine(this.doDead());
  }

  private IEnumerator doDead()
  {
    Battle01PVPNode battle01PvpNode = this;
    Debug.LogWarning((object) " ========= doDead");
    if (((Component) battle01PvpNode.pvp_disposition_nodes[0]).gameObject.activeSelf)
      yield return (object) null;
    Debug.LogWarning((object) " ========= active OK");
    foreach (Component pvpDispositionNode in battle01PvpNode.pvp_disposition_nodes)
      Object.Destroy((Object) pvpDispositionNode.gameObject);
    Object.Destroy((Object) battle01PvpNode);
  }

  private void setCameraTarget(HashSet<BL.Panel> pl)
  {
    BattleCameraController controller = this.battleManager.getController<BattleCameraController>();
    Vector3 vector3_1 = Vector3.zero;
    foreach (BL.Panel key in pl)
      vector3_1 = Vector3.op_Addition(vector3_1, this.env.panelResource[key].gameObject.transform.position);
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(vector3_1.x / (float) pl.Count, vector3_1.y / (float) pl.Count, vector3_1.z / (float) pl.Count);
    Vector3 vector3_2 = this.battleManager.order == 0 ? this.cameraOffset : Vector3.op_UnaryNegation(this.cameraOffset);
    controller.setLookAtTarget(Vector3.op_Addition(vector3_1, vector3_2));
  }
}
