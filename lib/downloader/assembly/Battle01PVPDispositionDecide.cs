// Decompiled with JetBrains decompiler
// Type: Battle01PVPDispositionDecide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01PVPDispositionDecide : NGBattleMenuBase
{
  private Battle01SelectNode _selectNode;

  private Battle01SelectNode selectNode
  {
    get
    {
      if (Object.op_Equality((Object) this._selectNode, (Object) null))
        this._selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) this).transform);
      return this._selectNode;
    }
  }

  private void Awake()
  {
    EventDelegate.Set(((Component) this).GetComponent<UIButton>().onClick, new EventDelegate((MonoBehaviour) this, "onClick"));
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01PVPDispositionDecide dispositionDecide = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    dispositionDecide._selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) dispositionDecide).transform);
    return false;
  }

  public void onClick()
  {
    if (this.IsPushAndSet())
      return;
    this.env.core.currentUnitPosition?.resetOriginalPosition(this.env.core, true);
    this.selectNode?.onBack();
    this.StartCoroutine(this.IsPushOff());
  }
}
