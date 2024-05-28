// Decompiled with JetBrains decompiler
// Type: Battle01Submenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01Submenu : BattleBackButtonMenuBase
{
  public SelectParts sight;
  public SelectParts area;
  public SelectParts type;
  private BL.BattleModified<BL.StructValue<bool>> isViewDengerAreaModified;
  private BL.BattleModified<BL.StructValue<int>> sightModified;
  private BL.BattleModified<BL.StructValue<bool>> isViewUnitTypeModified;
  private Battle01SelectNode selectNode;
  private GameObject menuPopupPrefab;

  protected override IEnumerator Start_Battle()
  {
    Battle01Submenu battle01Submenu = this;
    NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
    if (battle01Submenu.isViewDengerAreaModified == null)
      battle01Submenu.isViewDengerAreaModified = BL.Observe<BL.StructValue<bool>>(instance.environment.core.isViewDengerArea);
    if (battle01Submenu.sightModified == null)
      battle01Submenu.sightModified = BL.Observe<BL.StructValue<int>>(instance.environment.core.sight);
    if (battle01Submenu.isViewUnitTypeModified == null)
      battle01Submenu.isViewUnitTypeModified = BL.Observe<BL.StructValue<bool>>(instance.environment.core.isViewUnitType);
    Future<GameObject> f = (Future<GameObject>) null;
    f = instance.isPvp || instance.isPvnpc ? Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/popup/Battle01718Prefab_pvp") : (!instance.isGvg ? (!instance.isEarth ? Res.Prefabs.popup.Battle01718Prefab.Load<GameObject>() : Res.Prefabs.popup.Battle06718Prefab.Load<GameObject>()) : Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/popup/Battle01718Prefab_gvg"));
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01Submenu.menuPopupPrefab = f.Result;
    battle01Submenu.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) battle01Submenu).gameObject);
  }

  protected override void LateUpdate_Battle()
  {
    if (this.isViewDengerAreaModified.isChangedOnce())
      this.area.setValue(this.isViewDengerAreaModified.value.value ? 1 : 0);
    if (this.sightModified.isChangedOnce())
      this.sight.setValue(this.sightModified.value.value);
    if (!this.isViewUnitTypeModified.isChangedOnce())
      return;
    this.type.setValue(this.isViewUnitTypeModified.value.value ? 1 : 0);
  }

  public void onButtonArea()
  {
    if (this.env.core.phaseState.state == BL.Phase.pvp_disposition)
      return;
    this.isViewDengerAreaModified.value.value = this.area.inclementLoop() == 1;
  }

  public void onButtonSight() => this.sightModified.value.value = this.sight.inclementLoop();

  public void onButtonType()
  {
    this.isViewUnitTypeModified.value.value = this.type.inclementLoop() == 1;
  }

  public void onButtonMenu()
  {
    if (!this.battleManager.isBattleEnable || !this.battleManager.isPvp && this.env.core.phaseState.state != BL.Phase.player && this.env.core.phaseState.state != BL.Phase.pvp_disposition || Singleton<CommonRoot>.GetInstance().isActive3DUIMask || this.env.core.isAutoBattle.value || Singleton<TutorialRoot>.GetInstance().IsAdviced)
      return;
    this.selectNode.backToTop();
    this.battleManager.popupOpen(this.menuPopupPrefab);
  }

  public override void onBackButton()
  {
    if (!this.selectNode.canOpenMenu())
      return;
    this.onButtonMenu();
  }
}
