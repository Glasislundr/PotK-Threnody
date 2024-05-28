// Decompiled with JetBrains decompiler
// Type: Shop0074Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop0074Menu : ShopArticleListMenu
{
  [SerializeField]
  protected UILabel TxtOwnnumber;

  public override IEnumerator Init(Future<GameObject> cellPrefab)
  {
    IEnumerator e = base.Init(cellPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.TxtOwnnumber.SetTextLocalize(SMManager.Get<Player>().money);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
