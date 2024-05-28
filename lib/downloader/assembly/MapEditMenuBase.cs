// Decompiled with JetBrains decompiler
// Type: MapEditMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public abstract class MapEditMenuBase : BackButtonMenuBase
{
  [SerializeField]
  [Tooltip("この制御がアクティブな時だけ表示する物をセット")]
  protected GameObject[] cntlObjects_;
  private bool initialized_;
  private Func<object, IEnumerator> processAsync_;
  private object processArg1_;

  public abstract MapEdit031TopMenu.EditState editState_ { get; }

  protected abstract IEnumerator initializeAsync();

  protected abstract void onEnable();

  protected abstract void onDisable();

  protected void addControlObject(GameObject go)
  {
    if (Object.op_Equality((Object) go, (Object) null))
      return;
    if (this.cntlObjects_ == null)
      this.cntlObjects_ = new GameObject[0];
    if (((IEnumerable<GameObject>) this.cntlObjects_).Contains<GameObject>(go))
      return;
    List<GameObject> list = ((IEnumerable<GameObject>) this.cntlObjects_).ToList<GameObject>();
    list.Add(go);
    this.cntlObjects_ = list.ToArray();
  }

  protected MapEdit031TopMenu topMenu_ { get; private set; }

  protected MapEditUIEventTrigger ui3DEvent_ => this.topMenu_.ui3DEvent_;

  protected MapEditCameraController cntlCamera_ => this.topMenu_.cntlCamera_;

  protected MapEditPanel currentPanel_ => this.topMenu_.currentPanel_;

  protected MapEditOrnament currentOrnament_ => this.topMenu_.currentOrnament_;

  public bool isInitialized_ => this.initialized_;

  public bool isMenuActive_
  {
    get => ((Behaviour) this).enabled;
    set
    {
      if (((Behaviour) this).enabled == value)
        return;
      this.forceActive(value);
    }
  }

  protected bool isWait_ { get; private set; }

  protected bool waitAndSet()
  {
    if (this.isWait_)
      return true;
    this.isWait_ = true;
    return false;
  }

  protected void clearWait() => this.isWait_ = false;

  protected bool hasProcessAsync_ => this.processAsync_ != null;

  protected IEnumerator processAsync()
  {
    IEnumerator enumerator = this.processAsync_(this.processArg1_);
    this.processAsync_ = (Func<object, IEnumerator>) null;
    this.processArg1_ = (object) null;
    return enumerator;
  }

  public void setProcessAsync(Func<object, IEnumerator> proc, object arg1)
  {
    this.processAsync_ = proc;
    this.processArg1_ = arg1;
  }

  private IEnumerator Start()
  {
    while (!this.initialized_)
      yield return (object) null;
  }

  public IEnumerator InitializeAsync()
  {
    MapEditMenuBase mapEditMenuBase = this;
    mapEditMenuBase.topMenu_ = ((Component) mapEditMenuBase).GetComponent<MapEdit031TopMenu>();
    mapEditMenuBase.initialized_ = false;
    mapEditMenuBase.isWait_ = true;
    IEnumerator e = mapEditMenuBase.initializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mapEditMenuBase.topMenu_.requestDeactivate(((IEnumerable<GameObject>) mapEditMenuBase.cntlObjects_).ToList<GameObject>());
    ((Behaviour) mapEditMenuBase).enabled = false;
    mapEditMenuBase.initialized_ = true;
  }

  private void forceActive(bool isEnabled)
  {
    ((Behaviour) this).enabled = isEnabled;
    if (isEnabled)
    {
      this.isWait_ = false;
      this.onEnable();
    }
    else
      this.onDisable();
    if (this.cntlObjects_ == null)
      return;
    if (Object.op_Inequality((Object) this.topMenu_, (Object) null))
    {
      if (isEnabled)
        this.topMenu_.requestActivate(((IEnumerable<GameObject>) this.cntlObjects_).ToList<GameObject>());
      else
        this.topMenu_.requestDeactivate(((IEnumerable<GameObject>) this.cntlObjects_).ToList<GameObject>());
    }
    else
    {
      foreach (GameObject cntlObject in this.cntlObjects_)
      {
        NGTweenParts component = cntlObject.GetComponent<NGTweenParts>();
        if (Object.op_Equality((Object) component, (Object) null))
          cntlObject.SetActive(isEnabled);
        else
          component.forceActive(isEnabled);
      }
    }
  }

  protected MapEditPanel getTouchPanel() => this.topMenu_.getTouchPanel();

  protected void setCurrentPanel(MapEditPanel panel) => this.topMenu_.setCurrentPanel(panel);

  protected void clearCurrentPanel() => this.topMenu_.clearCurrentPanel();

  protected void setCurrentOrnament(MapEditOrnament ornament)
  {
    this.topMenu_.setCurrentOrnament(ornament);
  }

  protected void clearCurrentOrnament() => this.topMenu_.clearCurrentOrnamnet();

  protected MapEditPanel getPanelUnderOrnament()
  {
    return !Object.op_Inequality((Object) this.currentOrnament_, (Object) null) ? (MapEditPanel) null : this.topMenu_.castPanel(this.currentOrnament_.hitPanel());
  }

  protected Vector2 position3Dto2D(Vector3 worldPos)
  {
    Vector3 viewportPoint = this.cntlCamera_.Camera.WorldToViewportPoint(worldPos);
    return Vector2.op_Implicit(UICamera.currentCamera.ViewportToWorldPoint(viewportPoint));
  }
}
