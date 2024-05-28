// Decompiled with JetBrains decompiler
// Type: Versus02610Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02610Scene : NGSceneBase
{
  [SerializeField]
  private Versus02610Menu menu;
  private static bool is_loading_draw;
  private WebAPI.Response.PvpBoot pvpInfo;
  [SerializeField]
  private GameObject mainpanel;

  public static void ChangeScene(bool stack, bool loading_draw, bool isContinue = false)
  {
    Versus02610Scene.is_loading_draw = loading_draw;
    Versus02610Menu.IsContinue = isContinue;
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_10", stack);
  }

  public static void ChangeScene(bool stack, WebAPI.Response.PvpBoot pvpInfo)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("versus026_10", (stack ? 1 : 0) != 0, (object) pvpInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus02610Scene versus02610Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.MultiBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02610Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = Versus02610Scene.is_loading_draw;
    IEnumerator e;
    if (this.pvpInfo == null)
    {
      if (Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData)
      {
        e = WebAPI.HomeStartUp2().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = false;
      }
      else
      {
        e = WebAPI.HomeStartUp().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot();
      e = futureF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.pvpInfo = futureF.Result;
      futureF = (Future<WebAPI.Response.PvpBoot>) null;
    }
    e = this.onStartSceneAsync(this.pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync(WebAPI.Response.PvpBoot pvpInfo)
  {
    Versus02610Scene scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    scene.mainpanel.SetActive(false);
    yield return (object) null;
    yield return (object) null;
    scene.mainpanel.SetActive(true);
    if (scene.pvpInfo != null)
      pvpInfo = scene.pvpInfo;
    scene.menu.setScene(scene);
    IEnumerator e = scene.menu.Init(PvpMatchingTypeEnum.class_match, pvpInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public override void onEndScene() => this.updatePvpInfo();

  public void updatePvpInfo()
  {
    if (!this.menu.IsUpdate)
      return;
    this.pvpInfo = this.menu.UpdatePvpInfo;
  }
}
