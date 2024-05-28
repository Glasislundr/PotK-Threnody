// Decompiled with JetBrains decompiler
// Type: Startup000144Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Startup000144Scene : NGSceneBase
{
  public Startup000144Menu menu;
  public Transform middleTransform;

  public override IEnumerator onInitSceneAsync()
  {
    Future<GameObject> future = Res.Prefabs.popup.popup_000_14_4__anim_popup01.Load<GameObject>();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    future.Result.Clone(this.middleTransform);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(PlayerLoginBonus loginBonus)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Startup000144Scene startup000144Scene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      startup000144Scene.menu = ((Component) startup000144Scene).gameObject.GetComponentInChildren<Startup000144Menu>();
      startup000144Scene.menu.InitScene(loginBonus);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
