// Decompiled with JetBrains decompiler
// Type: StatusReady
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class StatusReady
{
  [SerializeField]
  public List<SpriteNumberSelectDirect> slc_Remain_hours;
  [SerializeField]
  public List<SpriteNumberSelectDirect> slc_Remain_minutes;
  [SerializeField]
  private NGTweenGaugeFillAmount slc_enemy_pvp_HP_Gauge;
  [SerializeField]
  private NGTweenGaugeFillAmount slc_player_pvp_HP_Gauge;
  [SerializeField]
  private GuildStatus myStatus;
  [SerializeField]
  private GuildStatus enStatus;

  public GuildStatus MyStatus
  {
    get => this.myStatus;
    set => this.myStatus = value;
  }

  public GuildStatus EnStatus
  {
    get => this.enStatus;
    set => this.enStatus = value;
  }

  public IEnumerator ResourceLoad(GuildRegistration myData, GuildRegistration enData)
  {
    IEnumerator e = this.MyStatus.ResourceLoad(myData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.EnStatus.ResourceLoad(enData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetStatus(GuildRegistration myData, GuildRegistration enData)
  {
    this.MyStatus.SetStatus(myData);
    this.EnStatus.SetStatus(enData);
  }

  public void UpdateStatus(GuildRegistration myData, GuildRegistration enData)
  {
    this.MyStatus.UpdateStatus(myData);
    this.EnStatus.UpdateStatus(enData);
  }
}
