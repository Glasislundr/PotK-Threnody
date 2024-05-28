// Decompiled with JetBrains decompiler
// Type: Shop00742UnitOther
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00742UnitOther : Shop00742Unit
{
  [SerializeField]
  protected UILabel TxtFlavor;

  public IEnumerator Initialize(UnitUnit target)
  {
    Shop00742UnitOther shop00742UnitOther = this;
    shop00742UnitOther.TxtName.SetText(target.name);
    shop00742UnitOther.TxtFlavor.SetText(target.description);
    IEnumerator e = shop00742UnitOther.RarityCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = shop00742UnitOther.UnitOtherSpriteCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
