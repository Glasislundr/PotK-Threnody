// Decompiled with JetBrains decompiler
// Type: SeaHomeUnitActionAnimeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SeaHomeUnitActionAnimeController : StateMachineBehaviour
{
  private SeaHomeUnitController owner;

  public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.owner = this.owner ?? ((Component) ((Component) animator).transform.parent).gameObject.GetComponent<SeaHomeUnitController>();
    if (!Object.op_Inequality((Object) this.owner, (Object) null) || Random.Range(0, 10000) >= 2000)
      return;
    this.owner.SetLookuped();
  }

  public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.owner = this.owner ?? ((Component) ((Component) animator).transform.parent).gameObject.GetComponent<SeaHomeUnitController>();
    if (!Object.op_Inequality((Object) this.owner, (Object) null))
      return;
    this.owner.ResetLookuped();
  }
}
