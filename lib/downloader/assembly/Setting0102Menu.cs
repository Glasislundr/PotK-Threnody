// Decompiled with JetBrains decompiler
// Type: Setting0102Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Setting0102Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtAP;
  [SerializeField]
  protected UILabel TxtCP;
  [SerializeField]
  protected UILabel TxtTitle;
  private bool NotificationAp;
  private bool NotificationBp;
  private bool PushNotification;
  private bool NotificationExplore;
  [SerializeField]
  private List<GameObject> IbtnAPONList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnAPOFFList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnBPONList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnBPOFFList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnPushONList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnPushOFFList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnExploreONList = new List<GameObject>();
  [SerializeField]
  private List<GameObject> IbtnExploreOFFList = new List<GameObject>();

  public IEnumerator onInitSceneAsync()
  {
    this.NotificationAp = Persist.notification.Data.Ap;
    this.NotificationBp = Persist.notification.Data.Bp;
    if (this.NotificationAp)
      this.IbtnAPOn();
    else
      this.IbtnAPOff();
    if (this.NotificationBp)
      this.IbtnBPOn();
    else
      this.IbtnBPOff();
    this.PushNotification = Persist.pushnotification.Data.enablePush;
    if (this.PushNotification)
      this.IbtnPushOn();
    else
      this.IbtnPushOff();
    this.NotificationExplore = Persist.notification.Data.Explore;
    if (this.NotificationExplore)
    {
      this.IbtnExploreOn();
    }
    else
    {
      this.IbtnExploreOff();
      yield break;
    }
  }

  public void onEndScene()
  {
    Persist.notification.Data.Ap = this.NotificationAp;
    Persist.notification.Data.Bp = this.NotificationBp;
    Persist.notification.Data.Explore = this.NotificationExplore;
    Persist.notification.Flush();
    if (this.PushNotification)
      GrowthPush.GetInstance().SetTag("PushNotification", "ON");
    else
      GrowthPush.GetInstance().SetTag("PushNotification", "OFF");
    Persist.pushnotification.Data.enablePush = this.PushNotification;
    Persist.pushnotification.Flush();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public virtual void IbtnAPOn()
  {
    if (this.IsPush)
      return;
    this.IbtnAPONList.ToggleOnce(0);
    this.IbtnAPOFFList.ToggleOnce(1);
    this.NotificationAp = true;
  }

  public virtual void IbtnAPOff()
  {
    if (this.IsPush)
      return;
    this.IbtnAPONList.ToggleOnce(1);
    this.IbtnAPOFFList.ToggleOnce(0);
    this.NotificationAp = false;
  }

  public virtual void IbtnBPOn()
  {
    if (this.IsPush)
      return;
    this.IbtnBPONList.ToggleOnce(0);
    this.IbtnBPOFFList.ToggleOnce(1);
    this.NotificationBp = true;
  }

  public virtual void IbtnBPOff()
  {
    if (this.IsPush)
      return;
    this.IbtnBPONList.ToggleOnce(1);
    this.IbtnBPOFFList.ToggleOnce(0);
    this.NotificationBp = false;
  }

  public void IbtnPushOn()
  {
    if (this.IsPush)
      return;
    this.IbtnPushONList.ToggleOnce(0);
    this.IbtnPushOFFList.ToggleOnce(1);
    this.PushNotification = true;
  }

  public void IbtnPushOff()
  {
    if (this.IsPush)
      return;
    this.IbtnPushONList.ToggleOnce(1);
    this.IbtnPushOFFList.ToggleOnce(0);
    this.PushNotification = false;
  }

  public void IbtnExploreOn()
  {
    if (this.IsPush)
      return;
    this.IbtnExploreONList.ToggleOnce(0);
    this.IbtnExploreOFFList.ToggleOnce(1);
    this.NotificationExplore = true;
  }

  public void IbtnExploreOff()
  {
    if (this.IsPush)
      return;
    this.IbtnExploreONList.ToggleOnce(1);
    this.IbtnExploreOFFList.ToggleOnce(0);
    this.NotificationExplore = false;
  }

  private enum ButtonStatus
  {
    ON,
    OFF,
  }
}
