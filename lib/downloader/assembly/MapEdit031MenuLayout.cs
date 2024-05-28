// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MapEdit;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class MapEdit031MenuLayout : MapEditMenuBase
{
  [SerializeField]
  private bool isSmoothLocate_;
  [SerializeField]
  private bool isSmoothDrag_;
  [SerializeField]
  private GameObject topInterface_;
  private bool isDrag_;
  private Vector3 posOrnament_;
  private Vector2 offsetTouch_;
  private MapEditMovementControl cntlMovement_;
  private NGTweenParts tweenInterface_;

  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.Layout;

  protected override IEnumerator initializeAsync()
  {
    MapEdit031MenuLayout edit031MenuLayout = this;
    if (Object.op_Equality((Object) edit031MenuLayout.cntlMovement_, (Object) null))
    {
      Future<GameObject> ldprefab = MapEdit.Prefabs.movement_mode.Load<GameObject>();
      IEnumerator e = ldprefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = ldprefab.Result.Clone(edit031MenuLayout.topInterface_.transform);
      edit031MenuLayout.cntlMovement_ = gameObject.GetComponent<MapEditMovementControl>();
      edit031MenuLayout.tweenInterface_ = gameObject.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) edit031MenuLayout.tweenInterface_, (Object) null))
        edit031MenuLayout.tweenInterface_.forceActive(false);
      else
        gameObject.SetActive(false);
      if (Object.op_Inequality((Object) edit031MenuLayout.cntlMovement_, (Object) null) && Object.op_Inequality((Object) edit031MenuLayout.cntlMovement_.btnOk_, (Object) null) && Object.op_Inequality((Object) edit031MenuLayout.cntlMovement_.btnCancel_, (Object) null))
      {
        EventDelegate.Set(edit031MenuLayout.cntlMovement_.btnOk_.onClick, new EventDelegate.Callback(edit031MenuLayout.onClickedOk));
        EventDelegate.Set(edit031MenuLayout.cntlMovement_.btnCancel_.onClick, new EventDelegate.Callback(((BackButtonMenuBase) edit031MenuLayout).onBackButton));
      }
      ldprefab = (Future<GameObject>) null;
    }
  }

  private void onClickedOk()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    if (Object.op_Equality((Object) currentOrnament, (Object) null))
      return;
    MapEditPanel panelUnderOrnament = this.getPanelUnderOrnament();
    if (Object.op_Equality((Object) panelUnderOrnament, (Object) null) || !panelUnderOrnament.checkLocation() || this.waitAndSet())
      return;
    if (Object.op_Inequality((Object) this.currentPanel_, (Object) panelUnderOrnament))
    {
      currentOrnament.setPosition(panelUnderOrnament.center_, true);
      this.setCurrentPanel(panelUnderOrnament);
    }
    currentOrnament.setSelected(false);
    this.topMenu_.setLocation();
  }

  protected override void onEnable()
  {
    if (Object.op_Equality((Object) this.currentOrnament_, (Object) null))
    {
      this.waitAndSet();
      this.topMenu_.backLayout();
    }
    else
    {
      this.ui3DEvent_.isEnabled_ = true;
      this.ui3DEvent_.setEventPress(new EventDelegate.Callback(this.onPress));
      this.ui3DEvent_.resetEventLongPress();
      this.ui3DEvent_.setEventDrag(new Action<Vector2>(this.onDrag), new EventDelegate.Callback(this.onDragStart), new EventDelegate.Callback(this.onDragEnd));
      this.topMenu_.changePanelDrawLayout();
      this.topMenu_.changeOrnamentDrawLayout();
      MapEditPanel panelUnderOrnament = this.getPanelUnderOrnament();
      this.setActiveMovementControl(true, Object.op_Inequality((Object) panelUnderOrnament, (Object) null) && panelUnderOrnament.checkLocation());
      this.topMenu_.updateCost();
    }
  }

  protected override void onDisable()
  {
    this.topMenu_.resetPanelDraw();
    this.topMenu_.resetOrnamentDraw();
  }

  protected override void Update()
  {
    base.Update();
    if (this.isWait_)
      return;
    this.updateMovementControl();
  }

  public override void onBackButton()
  {
    if (this.waitAndSet())
      return;
    MapEditOrnament currentOrnament = this.currentOrnament_;
    TrackOrnament trackOrnament = this.topMenu_.undoLocation();
    if (trackOrnament.isNew_)
    {
      this.topMenu_.returnStorageWithEffect();
    }
    else
    {
      if (Object.op_Inequality((Object) currentOrnament, (Object) null))
        currentOrnament.setSelected(false);
      MapEditPanel panel = this.topMenu_.getPanel(trackOrnament.row_, trackOrnament.column_);
      if (Object.op_Inequality((Object) panel, (Object) null))
      {
        this.topMenu_.setCurrentPanel(panel, cameraCenter: false);
        this.cntlCamera_.setLookAtTarget(panel.center_);
      }
      this.topMenu_.backLayout();
    }
  }

  private void onPress()
  {
    if (Object.op_Equality((Object) this.currentOrnament_, (Object) null))
    {
      this.setActiveMovementControl(false);
    }
    else
    {
      MapEditPanel touchPanel = this.getTouchPanel();
      if (!Object.op_Inequality((Object) touchPanel, (Object) null))
        return;
      this.setCurrentPanel(touchPanel);
      if (!touchPanel.checkLocation())
        return;
      this.currentOrnament_.setPosition(this.currentPanel_.center_, !this.isSmoothLocate_);
      this.setActiveMovementControl(true, true);
    }
  }

  private void onDragStart()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    this.isDrag_ = Object.op_Inequality((Object) currentOrnament, (Object) null) && currentOrnament.checkHit(UICamera.lastTouchPosition);
    if (!this.isDrag_ && Object.op_Inequality((Object) currentOrnament, (Object) null) && Object.op_Inequality((Object) this.cntlCamera_.Camera, (Object) null))
    {
      MapEditPanel mapEditPanel = this.topMenu_.castPanel(currentOrnament.hitPanel());
      this.isDrag_ = Object.op_Equality((Object) this.topMenu_.getTouchPanel(), (Object) mapEditPanel);
    }
    if (this.isDrag_)
    {
      this.posOrnament_ = this.cntlCamera_.Camera.WorldToScreenPoint(this.currentOrnament_.nowPosition_);
      this.offsetTouch_.x = this.posOrnament_.x - UICamera.lastTouchPosition.x;
      this.offsetTouch_.y = this.posOrnament_.y - UICamera.lastTouchPosition.y;
      this.currentOrnament_.setAutoSearchPanel(true, this.topMenu_);
      this.setActiveMovementControl(false);
    }
    else
      this.cntlCamera_.onPress();
  }

  private void onDrag(Vector2 delta)
  {
    if (this.isDrag_)
    {
      if (Object.op_Equality((Object) this.currentOrnament_, (Object) null))
        return;
      if (this.isSmoothDrag_)
      {
        this.posOrnament_.x += delta.x;
        this.posOrnament_.y += delta.y;
        this.currentOrnament_.setPosition(this.cntlCamera_.Camera.ScreenToWorldPoint(new Vector3(this.posOrnament_.x, this.posOrnament_.y, this.cntlCamera_.Camera.WorldToScreenPoint(this.currentOrnament_.nowPosition_).z)));
      }
      else
      {
        Vector2 vector2 = Vector2.op_Addition(UICamera.lastTouchPosition, this.offsetTouch_);
        this.currentOrnament_.setPosition(this.cntlCamera_.Camera.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, this.cntlCamera_.Camera.WorldToScreenPoint(this.currentOrnament_.nowPosition_).z)), true);
      }
    }
    else
      this.cntlCamera_.onDrag(delta);
  }

  private void onDragEnd()
  {
    bool enabledOk = false;
    if (this.isDrag_)
    {
      if (Object.op_Inequality((Object) this.currentOrnament_, (Object) null))
      {
        this.currentOrnament_.setAutoSearchPanel(false);
        if (Object.op_Equality((Object) this.currentPanel_, (Object) null))
        {
          MapEditPanel panelUnderOrnament = this.getPanelUnderOrnament();
          if (Object.op_Inequality((Object) panelUnderOrnament, (Object) null))
            this.topMenu_.setCurrentPanel(panelUnderOrnament, cameraCenter: false);
        }
        if (Object.op_Inequality((Object) this.currentPanel_, (Object) null) && this.currentPanel_.checkLocation())
        {
          this.currentOrnament_.setPosition(this.currentPanel_.center_, true);
          enabledOk = true;
        }
        else
          this.currentOrnament_.setPosition(this.currentOrnament_.nowPosition_, true);
      }
      this.cntlCamera_.setLookAtTarget(this.currentOrnament_.nowPosition_);
    }
    if (this.isDrag_)
      this.setActiveMovementControl(true, enabledOk);
    this.isDrag_ = false;
  }

  private void setActiveMovementControl(bool bActive, bool enabledOk = false)
  {
    if (Object.op_Equality((Object) this.cntlMovement_, (Object) null))
      return;
    if (bActive && Object.op_Inequality((Object) this.currentOrnament_, (Object) null))
    {
      if (Object.op_Inequality((Object) this.tweenInterface_, (Object) null))
        this.tweenInterface_.isActive = true;
      else
        ((Component) this.cntlMovement_).gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.cntlMovement_.btnOk_, (Object) null))
        ((UIButtonColor) this.cntlMovement_.btnOk_).isEnabled = enabledOk;
      this.updateMovementControl();
    }
    else if (Object.op_Inequality((Object) this.tweenInterface_, (Object) null))
      this.tweenInterface_.isActive = false;
    else
      ((Component) this.cntlMovement_).gameObject.SetActive(false);
  }

  private void updateMovementControl()
  {
    if (Object.op_Equality((Object) this.cntlMovement_, (Object) null) || !((Component) this.cntlMovement_).gameObject.activeSelf || Object.op_Equality((Object) this.currentOrnament_, (Object) null))
      return;
    ((Component) this.cntlMovement_).transform.position = Vector2.op_Implicit(this.position3Dto2D(((Component) this.currentOrnament_).transform.position));
  }
}
