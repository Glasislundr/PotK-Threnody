// Decompiled with JetBrains decompiler
// Type: Versus02622Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02622Scene : NGSceneBase
{
  [SerializeField]
  private Versus02622Menu menu;

  public static void ChangeScene(bool stack, PvPRecord freeInfo, PvPRecord friendInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_2_2", (stack ? 1 : 0) != 0, (object) freeInfo, (object) friendInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus02622Scene versus02622Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02622Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot();
    IEnumerator e = futureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.PvpBoot result = futureF.Result;
    e = this.onStartSceneAsync(result.pvp_record, result.pvp_record_by_friend);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(PvPRecord freeInfo, PvPRecord friendInfo)
  {
    IEnumerator e = this.menu.Initialize(freeInfo, friendInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
