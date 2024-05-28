// Decompiled with JetBrains decompiler
// Type: Versus0261MenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Versus0261MenuBase : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtMatchRemain;
  [SerializeField]
  protected UILabel TxtRemain;

  public virtual void IbtnBack() => Debug.Log((object) "click default event IbtnBack");

  public virtual void IbtnClassMatch() => Debug.Log((object) "click default event IbtnClassMatch");

  public virtual void IbtnRundomMatch()
  {
    Debug.Log((object) "click default event IbtnRundomMatch");
  }

  public virtual void IbtnFriendMatch()
  {
    Debug.Log((object) "click default event IbtnFriendMatch");
  }

  public virtual void IbtnReplay() => Debug.Log((object) "click default event IbtnReplay");
}
