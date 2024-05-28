// Decompiled with JetBrains decompiler
// Type: Versus0262MenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Versus0262MenuBase : NGMenuBase
{
  [SerializeField]
  protected UILabel txtBonus;
  [SerializeField]
  protected UILabel txtBonus2;
  [SerializeField]
  protected UILabel txtBonus3;
  [SerializeField]
  protected UILabel txtLeaderSkill;
  [SerializeField]
  protected UILabel txtPass;
  [SerializeField]
  protected UILabel txtTimeLimit;
  [SerializeField]
  protected UILabel txtTotalPower;

  public virtual void ibtnfriendoff() => Debug.Log((object) "click default event IbtnFriendOff");

  public virtual void IbtnFriendOn() => Debug.Log((object) "click default event IbtnFriendOn");

  public virtual void IbtnOrganization()
  {
    Debug.Log((object) "click default event IbtnOrganization");
  }

  public virtual void IbtnPassOff() => Debug.Log((object) "click default event IbtnPassOff");

  public virtual void IbtnPassOn() => Debug.Log((object) "click default event IbtnPassOn");

  public virtual void IbtnStartMatch() => Debug.Log((object) "click default event IbtnStartMatch");

  public virtual void IbtnTeamCondition()
  {
    Debug.Log((object) "click default event IbtnTeamCondition");
  }

  public virtual void IbtnWarExperience()
  {
    Debug.Log((object) "click default event IbtnWarExperience");
  }
}
