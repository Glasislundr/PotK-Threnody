// Decompiled with JetBrains decompiler
// Type: BattlePanelParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public class BattlePanelParts : BattleMonoBehaviour
{
  public Material[] cellMaterials;
  private BL.BattleModified<BL.Panel> modified;
  private FieldButton attackButton;
  private FieldButton healButton;
  private FieldButton loadingButton;
  private FieldButton waitButton;
  private Renderer _renderer;
  private GameObject fieldEventEffect;
  private readonly string fieldJumpEffect_path = "BattleEffects/field/ef056_fe_jump_mark";
  private GameObject fieldJumpEffect;

  private void Awake() => this._renderer = ((Component) this).GetComponent<Renderer>();

  private FieldButton cloneButton(GameObject prefab)
  {
    FieldButton component = prefab.CloneAndGetComponent<FieldButton>(((Component) this).transform);
    ((Component) component).transform.localPosition = this.getLocalPosition();
    return component;
  }

  public void setGuardArea(GameObject prefab)
  {
    prefab.Clone(((Component) this).transform).transform.localPosition = this.getLocalPosition();
  }

  public void initButtons(GameObject attack, GameObject heal, GameObject loading, GameObject wait)
  {
    this.attackButton = this.cloneButton(attack);
    this.attackButton.isActive = false;
    this.healButton = this.cloneButton(heal);
    this.healButton.isActive = false;
    this.loadingButton = this.cloneButton(loading);
    this.loadingButton.isActive = false;
    this.waitButton = this.cloneButton(wait);
    this.waitButton.isActive = false;
  }

  public void setAttackButton(GameObject prefab)
  {
    if (Object.op_Equality((Object) this.attackButton, (Object) null))
      this.attackButton = this.cloneButton(prefab);
    this.attackButton.isActive = true;
    if (Object.op_Inequality((Object) this.healButton, (Object) null))
      this.healButton.isActive = false;
    if (!Object.op_Inequality((Object) this.loadingButton, (Object) null))
      return;
    this.loadingButton.isActive = false;
  }

  public void setHealButton(GameObject prefab)
  {
    if (Object.op_Equality((Object) this.healButton, (Object) null))
      this.healButton = this.cloneButton(prefab);
    this.healButton.isActive = true;
    if (Object.op_Inequality((Object) this.attackButton, (Object) null))
      this.attackButton.isActive = false;
    if (!Object.op_Inequality((Object) this.loadingButton, (Object) null))
      return;
    this.loadingButton.isActive = false;
  }

  public void setLoadingButton(GameObject prefab)
  {
    if (Object.op_Equality((Object) this.loadingButton, (Object) null))
      this.loadingButton = this.cloneButton(prefab);
    this.loadingButton.isActive = true;
    if (Object.op_Inequality((Object) this.attackButton, (Object) null))
      this.attackButton.isActive = false;
    if (!Object.op_Inequality((Object) this.healButton, (Object) null))
      return;
    this.healButton.isActive = false;
  }

  public void hideButton()
  {
    if (Object.op_Inequality((Object) this.attackButton, (Object) null))
      this.attackButton.isActive = false;
    if (Object.op_Inequality((Object) this.healButton, (Object) null))
      this.healButton.isActive = false;
    if (!Object.op_Inequality((Object) this.loadingButton, (Object) null))
      return;
    this.loadingButton.isActive = false;
  }

  public void setWaitButton(GameObject prefab)
  {
    if (Object.op_Equality((Object) this.waitButton, (Object) null))
      this.waitButton = this.cloneButton(prefab);
    this.waitButton.isActive = true;
  }

  public void hideWaitButton()
  {
    if (!Object.op_Inequality((Object) this.waitButton, (Object) null))
      return;
    this.waitButton.isActive = false;
  }

  public void buttonDown(bool v)
  {
    if (Object.op_Inequality((Object) this.attackButton, (Object) null) && this.attackButton.isActive)
      this.attackButton.isDown = v;
    if (Object.op_Inequality((Object) this.healButton, (Object) null) && this.healButton.isActive)
      this.healButton.isDown = v;
    if (!Object.op_Inequality((Object) this.loadingButton, (Object) null) || !this.loadingButton.isActive)
      return;
    this.loadingButton.isDown = v;
  }

  public void setPanel(BL.Panel panel)
  {
    this.modified = BL.Observe<BL.Panel>(panel);
    this.env.panelResource[panel].gameObject = ((Component) this).gameObject;
    if (!panel.hasEvent || !Object.op_Inequality((Object) this.env.dropDataResource[panel.fieldEvent].prefab, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.fieldEventEffect, (Object) null))
      Object.Destroy((Object) this.fieldEventEffect);
    this.fieldEventEffect = this.env.dropDataResource[panel.fieldEvent].prefab.Clone(((Component) this).transform);
    this.fieldEventEffect.transform.localPosition = this.getLocalPosition();
  }

  public Vector3 getLocalPosition()
  {
    return new Vector3(0.0f, ((Component) this).GetComponent<PanelInit>().panelHeightNonScale + 0.1f, 0.0f);
  }

  public BL.Panel getPanel() => this.modified.value;

  public float getHeight() => ((Component) this).GetComponent<PanelInit>().panelHeight;

  private Material attributeMaterial(BL.Panel panel)
  {
    if (panel.checkAttribute(BL.PanelAttribute.test))
      return this.cellMaterials[4];
    switch (panel.attribute)
    {
      case BL.PanelAttribute.playermove | BL.PanelAttribute.danger:
        return this.cellMaterials[8];
      case BL.PanelAttribute.playermove | BL.PanelAttribute.danger | BL.PanelAttribute.moving:
        return this.cellMaterials[9];
      case BL.PanelAttribute.danger | BL.PanelAttribute.attack_range:
        return this.cellMaterials[10];
      case BL.PanelAttribute.danger | BL.PanelAttribute.heal_range:
      case BL.PanelAttribute.playermove | BL.PanelAttribute.danger | BL.PanelAttribute.heal_range:
      case BL.PanelAttribute.playermove | BL.PanelAttribute.target_heal | BL.PanelAttribute.heal_range:
      case BL.PanelAttribute.danger | BL.PanelAttribute.attack_range | BL.PanelAttribute.heal_range:
        return this.cellMaterials[11];
      default:
        if (panel.checkAttribute(BL.PanelAttribute.target_heal))
          return this.cellMaterials[3];
        if (panel.checkAttribute(BL.PanelAttribute.target_attack))
          return this.cellMaterials[5];
        if (panel.checkAttribute(BL.PanelAttribute.heal_range))
          return this.cellMaterials[3];
        if (panel.checkAttribute(BL.PanelAttribute.attack_range))
          return this.cellMaterials[5];
        if (panel.checkAttribute(BL.PanelAttribute.moving))
          return this.cellMaterials[7];
        if (panel.checkAttribute(BL.PanelAttribute.playermove))
          return this.cellMaterials[1];
        if (panel.checkAttribute(BL.PanelAttribute.neutralmove) || panel.checkAttribute(BL.PanelAttribute.enemymove))
          return this.cellMaterials[2];
        if (panel.checkAttribute(BL.PanelAttribute.danger))
          return this.cellMaterials[4];
        return panel.checkAttribute(BL.PanelAttribute.reserve0) ? this.cellMaterials[6] : this.cellMaterials[0];
    }
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.modified.isChangedOnce())
      return;
    BL.Panel panel = this.modified.value;
    Material material = this.attributeMaterial(panel);
    if (Object.op_Inequality((Object) this._renderer.material, (Object) material))
    {
      if (Object.op_Inequality((Object) this._renderer.material, (Object) null))
        Object.Destroy((Object) this._renderer.material);
      this._renderer.material = material;
    }
    if (Object.op_Inequality((Object) this.fieldEventEffect, (Object) null) && !panel.hasEvent)
    {
      Object.Destroy((Object) this.fieldEventEffect);
      this.fieldEventEffect = (GameObject) null;
    }
    if (panel.isJumping)
    {
      if (!Object.op_Equality((Object) this.fieldJumpEffect, (Object) null))
        return;
      this.fieldJumpEffect = Singleton<NGBattleManager>.GetInstance().fieldJumpEffectPrefab.Clone(((Component) this).transform);
      this.fieldJumpEffect.transform.localPosition = this.getLocalPosition();
    }
    else
    {
      if (!Object.op_Inequality((Object) this.fieldJumpEffect, (Object) null))
        return;
      Object.Destroy((Object) this.fieldJumpEffect);
      this.fieldJumpEffect = (GameObject) null;
    }
  }
}
