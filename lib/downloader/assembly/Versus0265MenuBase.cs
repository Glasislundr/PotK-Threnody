// Decompiled with JetBrains decompiler
// Type: Versus0265MenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Versus0265MenuBase : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtEnemyPlayerName;
  [SerializeField]
  protected UILabel TxtEnemyRank;
  [SerializeField]
  protected UILabel TxtLeague;
  [SerializeField]
  protected UILabel TxtMatching;
  [SerializeField]
  protected UILabel TxtTotalPower;
  [SerializeField]
  protected UILabel TxtToVictory;
  [SerializeField]
  protected UILabel TxtToVictorySub;
  [SerializeField]
  protected UILabel TxtVictoryPoint;
  [SerializeField]
  protected UILabel TxtWinLose;

  public virtual void IbtnAreaOff() => Debug.Log((object) "click default event IbtnAreaOff");

  public virtual void IbtnAreaOn() => Debug.Log((object) "click default event IbtnAreaOn");

  public virtual void IbtnMenu() => Debug.Log((object) "click default event IbtnMenu");

  public virtual void IbtnSight1() => Debug.Log((object) "click default event IbtnSight1");

  public virtual void IbtnSight2() => Debug.Log((object) "click default event IbtnSight2");

  public virtual void IbtnSight3() => Debug.Log((object) "click default event IbtnSight3");

  public virtual void IbtnUndoUp_AnimBack()
  {
    Debug.Log((object) "click default event IbtnUndoUp_AnimBack");
  }

  public virtual void IbtnWait() => Debug.Log((object) "click default event IbtnWait");
}
