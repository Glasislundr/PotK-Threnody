// Decompiled with JetBrains decompiler
// Type: StatusUsual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[Serializable]
public class StatusUsual
{
  public UILabel txt_waiting_for_entery_to_start;
  [SerializeField]
  private GuildStatus myStatus;

  public GuildStatus MyStatus
  {
    get => this.myStatus;
    set => this.myStatus = value;
  }

  public IEnumerator ResourceLoad(GuildRegistration myData)
  {
    IEnumerator e = this.MyStatus.ResourceLoad(myData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
