// Decompiled with JetBrains decompiler
// Type: MapEditPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class MapEditPanel : MonoBehaviour
{
  private const BL.PanelAttribute ATTRIBUTE_SELECTED = BL.PanelAttribute.danger;
  private const BL.PanelAttribute ATTRIBUTE_LOCATION = BL.PanelAttribute.playermove;
  private const BL.PanelAttribute ATTRIBUTE_NOT_LOCATION = BL.PanelAttribute.neutralmove;
  private const BL.PanelAttribute ATTRIBUTE_DEFENSE = BL.PanelAttribute.attack_range;
  private const BL.PanelAttribute ATTRIBUTE_OFFENSE = BL.PanelAttribute.moving;
  private const BL.PanelAttribute ATTRIBUTE_DEFENDER = BL.PanelAttribute.target_heal;
  private const BL.PanelAttribute ATTRIBUTE_ATTACKER = BL.PanelAttribute.reserve0;
  private WeakReference wOrnament_;
  private bool isLayout_;

  public static MapEditPanel attach(
    GameObject go,
    BL.Panel panel,
    bool isLocate,
    bool isDefender,
    bool isAttacker)
  {
    MapEditPanel mapEditPanel = go.AddComponent<MapEditPanel>();
    mapEditPanel.initialize(panel, isLocate, isDefender, isAttacker);
    return mapEditPanel;
  }

  public BL.Panel panel_ { get; private set; }

  public BattlePanelParts battlePanel_ { get; private set; }

  public Vector3 center_ { get; private set; }

  public MapEditOrnament ornament_
  {
    get
    {
      return this.wOrnament_ == null || !this.wOrnament_.IsAlive ? (MapEditOrnament) null : this.wOrnament_.Target as MapEditOrnament;
    }
  }

  public bool isSelected_ { get; private set; }

  public bool isLocate_ { get; private set; }

  public bool isDefender_ { get; private set; }

  public bool isAttacker_ { get; private set; }

  private void initialize(BL.Panel panel, bool isLocate, bool isDefender, bool isAttacker)
  {
    this.panel_ = panel;
    this.battlePanel_ = ((Component) this).gameObject.GetComponent<BattlePanelParts>();
    PanelInit component = ((Component) this).gameObject.GetComponent<PanelInit>();
    Vector3 position = ((Component) this.battlePanel_).gameObject.transform.position;
    this.center_ = new Vector3(position.x, component.panelHeight, position.z);
    this.isSelected_ = false;
    this.isLocate_ = isLocate && !isDefender && !isAttacker;
    this.isDefender_ = isDefender;
    this.isAttacker_ = isAttacker;
    this.updateDraw();
  }

  public void changeDrawLayout()
  {
    this.isLayout_ = true;
    this.updateDraw();
  }

  public void resetDraw()
  {
    this.isLayout_ = false;
    this.updateDraw();
  }

  public void setSelected(bool isSelected)
  {
    this.isSelected_ = isSelected;
    this.updateDraw();
  }

  private void updateDraw()
  {
    if (this.isLayout_)
    {
      if (this.checkLocation())
      {
        if (this.isSelected_)
          this.panel_.attribute = BL.PanelAttribute.danger;
        else
          this.panel_.attribute = BL.PanelAttribute.playermove;
      }
      else if (this.isDefender_)
        this.panel_.attribute = BL.PanelAttribute.target_heal;
      else if (this.isAttacker_)
        this.panel_.attribute = BL.PanelAttribute.reserve0;
      else
        this.panel_.attribute = BL.PanelAttribute.neutralmove;
    }
    else if (this.isSelected_)
      this.panel_.attribute = BL.PanelAttribute.danger;
    else if (this.isDefender_)
      this.panel_.attribute = BL.PanelAttribute.target_heal;
    else if (this.isAttacker_)
      this.panel_.attribute = BL.PanelAttribute.reserve0;
    else
      this.panel_.clearAttribute();
  }

  public void changeDrawDefense() => this.panel_.attribute = BL.PanelAttribute.attack_range;

  public void changeDrawOffense() => this.panel_.attribute = BL.PanelAttribute.moving;

  public void resetDrawEffect() => this.updateDraw();

  public void setOrnament(MapEditOrnament ornament)
  {
    if (Object.op_Equality((Object) this.ornament_, (Object) ornament))
      return;
    this.wOrnament_ = Object.op_Inequality((Object) ornament, (Object) null) ? new WeakReference((object) ornament) : (WeakReference) null;
  }

  public bool checkLocation()
  {
    return this.isLocate_ && Object.op_Equality((Object) this.ornament_, (Object) null);
  }
}
