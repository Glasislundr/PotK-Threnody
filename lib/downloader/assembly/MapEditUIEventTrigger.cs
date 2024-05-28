// Decompiled with JetBrains decompiler
// Type: MapEditUIEventTrigger
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
public class MapEditUIEventTrigger : MonoBehaviour
{
  public static MapEditUIEventTrigger current_;
  public List<EventDelegate> onPress_ = new List<EventDelegate>();
  public List<EventDelegate> onLongPress_ = new List<EventDelegate>();
  public List<Func<IEnumerator>> onLongPressLoop_ = new List<Func<IEnumerator>>();
  public float longPressDuration_ = 1f;
  public List<EventDelegate> onDragStart_ = new List<EventDelegate>();
  public List<Action<Vector2>> onDrag_ = new List<Action<Vector2>>();
  public List<EventDelegate> onDragEnd_ = new List<EventDelegate>();
  private bool isLongPressed_;
  private bool isActiveLongPress_;
  private bool isStartDrag_;

  public bool isEnabled_
  {
    get
    {
      if (!((Behaviour) this).enabled)
        return false;
      Collider component = ((Component) this).GetComponent<Collider>();
      return Object.op_Inequality((Object) component, (Object) null) && component.enabled;
    }
    set
    {
      if (!value)
        this.stopLongPress();
      Collider component = ((Component) this).GetComponent<Collider>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.enabled = value;
      else
        ((Behaviour) this).enabled = value;
    }
  }

  private void startLongPress()
  {
    this.isActiveLongPress_ = true;
    this.StartCoroutine("doLongPress");
    this.isLongPressed_ = false;
  }

  private void stopLongPress()
  {
    if (!this.isActiveLongPress_)
      return;
    this.StopCoroutine("doLongPress");
    this.isActiveLongPress_ = false;
  }

  private IEnumerator doLongPress()
  {
    MapEditUIEventTrigger editUiEventTrigger = this;
    yield return (object) new WaitForSeconds(editUiEventTrigger.longPressDuration_);
    editUiEventTrigger.isLongPressed_ = true;
    MapEditUIEventTrigger.current_ = editUiEventTrigger;
    EventDelegate.Execute(editUiEventTrigger.onLongPress_);
    if (editUiEventTrigger.onLongPressLoop_ != null)
    {
      IEnumerator e = editUiEventTrigger.onLongPressLoop_.Select<Func<IEnumerator>, IEnumerator>((Func<Func<IEnumerator>, IEnumerator>) (f => f())).WaitAll();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    MapEditUIEventTrigger.current_ = (MapEditUIEventTrigger) null;
    editUiEventTrigger.isActiveLongPress_ = false;
  }

  private void OnPress(bool pressed)
  {
    if (!this.isEnabled_ || UICamera.currentTouch == null)
      return;
    if (pressed)
      this.startLongPress();
    else
      this.stopLongPress();
  }

  private void OnClick()
  {
    if (!this.isEnabled_ || UICamera.currentTouch == null || this.isLongPressed_)
      return;
    MapEditUIEventTrigger.current_ = this;
    EventDelegate.Execute(this.onPress_);
    MapEditUIEventTrigger.current_ = (MapEditUIEventTrigger) null;
  }

  private void OnDragStart()
  {
    this.stopLongPress();
    if (!this.isEnabled_ || UICamera.currentTouch == null)
      return;
    this.isStartDrag_ = true;
    MapEditUIEventTrigger.current_ = this;
    EventDelegate.Execute(this.onDragStart_);
    MapEditUIEventTrigger.current_ = (MapEditUIEventTrigger) null;
  }

  private void OnDrag(Vector2 delta)
  {
    if (!this.isStartDrag_)
      return;
    MapEditUIEventTrigger.current_ = this;
    foreach (Action<Vector2> action in this.onDrag_)
      action(delta);
    MapEditUIEventTrigger.current_ = (MapEditUIEventTrigger) null;
  }

  private void OnDragEnd()
  {
    if (!this.isStartDrag_)
      return;
    this.isStartDrag_ = false;
    MapEditUIEventTrigger.current_ = this;
    EventDelegate.Execute(this.onDragEnd_);
    MapEditUIEventTrigger.current_ = (MapEditUIEventTrigger) null;
  }

  private void OnEnable()
  {
    this.stopLongPress();
    this.isLongPressed_ = false;
    this.isStartDrag_ = false;
  }

  public void resetEventPress() => this.onPress_.Clear();

  public void resetEventLongPress()
  {
    this.stopLongPress();
    this.isLongPressed_ = false;
    this.onLongPress_.Clear();
    this.onLongPressLoop_.Clear();
  }

  public void resetEventDrag()
  {
    this.isStartDrag_ = false;
    this.onDragStart_.Clear();
    this.onDrag_.Clear();
    this.onDragEnd_.Clear();
  }

  public void setEventPress(EventDelegate.Callback eventPress)
  {
    this.onPress_.Clear();
    this.addEventPress(eventPress);
  }

  public void addEventPress(EventDelegate.Callback eventPress)
  {
    if (eventPress == null)
      return;
    EventDelegate.Add(this.onPress_, eventPress);
  }

  public void setEventLongPress(
    EventDelegate.Callback eventLongPress,
    Func<IEnumerator> eventLongPressLoop = null)
  {
    this.onLongPress_.Clear();
    this.onLongPressLoop_.Clear();
    this.addEventLongPress(eventLongPress, eventLongPressLoop);
  }

  public void addEventLongPress(
    EventDelegate.Callback eventLongPress,
    Func<IEnumerator> eventLongPressLoop = null)
  {
    if (eventLongPress == null)
      return;
    EventDelegate.Add(this.onLongPress_, eventLongPress);
    if (eventLongPressLoop == null)
      return;
    this.onLongPressLoop_.Add(eventLongPressLoop);
  }

  public void setEventDrag(
    Action<Vector2> eventDrag,
    EventDelegate.Callback eventStart = null,
    EventDelegate.Callback eventEnd = null)
  {
    this.onDragStart_.Clear();
    this.onDrag_.Clear();
    this.onDragEnd_.Clear();
    this.addEventDrag(eventDrag, eventStart, eventEnd);
  }

  public void addEventDrag(
    Action<Vector2> eventDrag,
    EventDelegate.Callback eventStart = null,
    EventDelegate.Callback eventEnd = null)
  {
    if (eventStart != null)
      EventDelegate.Add(this.onDragStart_, eventStart);
    if (eventDrag != null)
      this.onDrag_.Add(eventDrag);
    if (eventEnd == null)
      return;
    EventDelegate.Add(this.onDragEnd_, eventEnd);
  }
}
