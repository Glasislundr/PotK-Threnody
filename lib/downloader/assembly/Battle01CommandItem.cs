// Decompiled with JetBrains decompiler
// Type: Battle01CommandItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandItem : BattleMonoBehaviour
{
  private Battle01SelectNode selectNode;

  private void Awake()
  {
    EventDelegate.Set(((Component) this).GetComponent<UIButton>().onClick, new EventDelegate((MonoBehaviour) this, "onClick"));
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandItem battle01CommandItem = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01CommandItem.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) battle01CommandItem).transform);
    return false;
  }

  public void onClick()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    this.selectNode.useItem();
  }
}
