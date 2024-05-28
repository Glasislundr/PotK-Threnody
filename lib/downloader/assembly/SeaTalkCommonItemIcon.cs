// Decompiled with JetBrains decompiler
// Type: SeaTalkCommonItemIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class SeaTalkCommonItemIcon : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite thum;

  public IEnumerator Init()
  {
    Future<Sprite> f = Singleton<ResourceManager>.GetInstance().Load<Sprite>("AssetBundle/Resources/Gears/114003/2D/weapon_thum");
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.thum.sprite2D = f.Result;
  }
}
