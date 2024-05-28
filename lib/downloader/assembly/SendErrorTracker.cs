// Decompiled with JetBrains decompiler
// Type: SendErrorTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SendErrorTracker : MonoBehaviour
{
  private static List<SendErrorTracker> trackers = new List<SendErrorTracker>();

  public static bool isSendError { get; private set; }

  private void Awake()
  {
    SendErrorTracker.isSendError = true;
    SendErrorTracker.trackers.Add(this);
  }

  private void OnDestroy()
  {
    SendErrorTracker.trackers.Remove(this);
    if (SendErrorTracker.trackers.Count > 0)
      return;
    SendErrorTracker.isSendError = false;
  }
}
