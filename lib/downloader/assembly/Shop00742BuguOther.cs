// Decompiled with JetBrains decompiler
// Type: Shop00742BuguOther
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00742BuguOther : Shop00742Bugu
{
  [SerializeField]
  protected UILabel TxtFlavor;

  public IEnumerator Initialize(GearGear target)
  {
    Shop00742BuguOther shop00742BuguOther = this;
    shop00742BuguOther.TxtFlavor.SetText(target.description);
    shop00742BuguOther.TxtName.SetText(target.name);
    IEnumerator e = shop00742BuguOther.RarityCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = shop00742BuguOther.BuguSpriteCreate(target.LoadSpriteBasic());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override IEnumerator BuguSpriteCreate(Future<Sprite> spriteF)
  {
    return base.BuguSpriteCreate(spriteF);
  }

  protected override IEnumerator RarityCreate(GearGear target) => base.RarityCreate(target);
}
