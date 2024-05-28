// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeMaterialContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using JobChangeData;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004JobChangeMaterialContainer : MonoBehaviour
{
  [SerializeField]
  private Unit004JobChangeMaterialContainer.MaterialControl[] controls_ = new Unit004JobChangeMaterialContainer.MaterialControl[DefValues.NUM_MATERIALSLOT];
  private Unit004JobChangeMenu menu_;
  private PlayerMaterialUnit[] materials_;

  public bool isEnabled
  {
    get => ((Component) this).gameObject.activeSelf;
    set => ((Component) this).gameObject.SetActive(value);
  }

  public void setAlpha(float a)
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIRect) component).alpha = a;
  }

  public void initialize(Unit004JobChangeMenu menu, GameObject iconPrefab)
  {
    this.menu_ = menu;
    foreach (Unit004JobChangeMaterialContainer.MaterialControl control in this.controls_)
      control.icon_ = iconPrefab.Clone(control.lnkIcon_).GetComponent<UnitIcon>();
  }

  public IEnumerator doUpdateMaterials(
    PlayerMaterialUnit[] materials,
    bool bBase,
    bool bUnlocked,
    bool conditionsLocked)
  {
    this.materials_ = materials;
    PlayerMaterialUnit[] playerMaterials = SMManager.Get<PlayerMaterialUnit[]>();
    string nullQuantity = Consts.GetInstance().JOBCHANGE_NULL_QUANTITY;
    bool bDisable = bBase | bUnlocked | conditionsLocked;
    for (int n = 0; n < this.controls_.Length; ++n)
    {
      Unit004JobChangeMaterialContainer.MaterialControl mc = this.controls_[n];
      mc.objTop_.SetActive(true);
      if (materials.Length <= n)
      {
        mc.icon_.unit = (UnitUnit) null;
        mc.icon_.SetEmpty();
        mc.icon_.SetCounter(0);
        mc.icon_.Gray = bDisable;
        ((UIButtonColor) mc.icon_.Button).isEnabled = false;
        mc.txtQuantity_.SetTextLocalize(nullQuantity);
        ((UIWidget) mc.txtQuantity_).color = bDisable ? Color.gray : Color.white;
      }
      else
      {
        PlayerMaterialUnit mu = materials[n];
        if (bDisable)
          mc.txtQuantity_.SetTextLocalize(nullQuantity);
        else
          mc.txtQuantity_.SetTextLocalize(mu.quantity);
        PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(playerMaterials, (Predicate<PlayerMaterialUnit>) (m => m._unit == mu._unit));
        int q = playerMaterialUnit != null ? playerMaterialUnit.quantity : 0;
        ((UIWidget) mc.txtQuantity_).color = bDisable ? Color.gray : (q >= mu.quantity ? Color.white : Color.red);
        UnitUnit unit = mu.unit;
        yield return (object) mc.icon_.SetUnit(unit, unit.GetElement(), false);
        mc.icon_.Gray = bDisable || q < mu.quantity;
        mc.icon_.BottomModeValue = UnitIconBase.BottomMode.Nothing;
        mc.icon_.SetCounter(q, isDisplayBelowZero: true);
        this.setEventOnClickedMaterial(mc.icon_, mu);
        mc = (Unit004JobChangeMaterialContainer.MaterialControl) null;
      }
    }
  }

  private void setEventOnClickedMaterial(UnitIcon icon, PlayerMaterialUnit materialUnit)
  {
    ((UIButtonColor) icon.Button).isEnabled = true;
    PlayerUnit playerUnit = PlayerUnit.CreateByPlayerMaterialUnit(materialUnit);
    icon.onClick = (Action<UnitIconBase>) (ui => this.onClickedMaterial(playerUnit));
  }

  private void onClickedMaterial(PlayerUnit selected)
  {
    if (this.menu_.isCustomPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit0042Scene.changeScene(true, selected, ((IEnumerable<PlayerMaterialUnit>) this.materials_).Select<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, PlayerUnit>) (m => PlayerUnit.CreateByPlayerMaterialUnit(m))).ToArray<PlayerUnit>());
  }

  [Serializable]
  private class MaterialControl
  {
    public GameObject objTop_;
    public Transform lnkIcon_;
    public UILabel txtQuantity_;
    [NonSerialized]
    public UnitIcon icon_;
  }
}
