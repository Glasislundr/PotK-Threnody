// Decompiled with JetBrains decompiler
// Type: Versus02611Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02611Scene : NGSceneBase
{
  [SerializeField]
  private Versus02611Menu menu;

  public static void ChangeScene(bool stack, WebAPI.Response.PvpBoot pvpInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_11", (stack ? 1 : 0) != 0, (object) pvpInfo);
  }

  public IEnumerator onStartSceneAsync(WebAPI.Response.PvpBoot pvpInfo)
  {
    IEnumerator e = this.menu.Init(pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
