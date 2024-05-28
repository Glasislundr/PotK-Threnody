// Decompiled with JetBrains decompiler
// Type: Startup000LoginBonusConfirmScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Startup000LoginBonusConfirmScene : NGSceneBase
{
  [SerializeField]
  private Transform menuAnchor;
  private WebAPI.Response.LoginbonusTop mResponse;
  private LoginbonusLoginbonus mBonus;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("startup000_login_bonus_confirm", stack);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Startup000LoginBonusConfirmScene bonusConfirmScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<WebAPI.Response.LoginbonusTop> apiF = WebAPI.LoginbonusTop();
    IEnumerator e = apiF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (apiF.Result == null)
    {
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      yield return (object) null;
    }
    bonusConfirmScene.mResponse = apiF.Result;
    if (!MasterData.LoginbonusLoginbonus.TryGetValue(bonusConfirmScene.mResponse.login_bonus_id, out bonusConfirmScene.mBonus))
    {
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      yield return (object) null;
    }
    Future<GameObject> loadFt;
    switch (bonusConfirmScene.mBonus.draw_type)
    {
      case LoginbonusDrawType.monthly:
        loadFt = new ResourceObject("Prefabs/startup000_14/loginBonus_monthly_confirm_old").Load<GameObject>();
        e = loadFt.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bonusConfirmScene.menuBase = (NGMenuBase) loadFt.Result.CloneAndGetComponent<Startup000LoginBonusConfirmMenu>(bonusConfirmScene.menuAnchor);
        break;
      case LoginbonusDrawType.monthly_by_day:
        loadFt = new ResourceObject("Prefabs/startup000_14/loginBonus_monthly").Load<GameObject>();
        e = loadFt.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bonusConfirmScene.menuBase = (NGMenuBase) loadFt.Result.CloneAndGetComponent<Startup00014MakeupMonthly>(bonusConfirmScene.menuAnchor);
        break;
      default:
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
        yield return (object) null;
        break;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    Startup000LoginBonusConfirmScene bonusConfirmScene = this;
    IEnumerator e;
    switch (bonusConfirmScene.mBonus.draw_type)
    {
      case LoginbonusDrawType.monthly:
        bonusConfirmScene.isActiveHeader = true;
        e = ((Startup000LoginBonusConfirmMenu) bonusConfirmScene.menuBase).Init(bonusConfirmScene.mResponse);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case LoginbonusDrawType.monthly_by_day:
        bonusConfirmScene.isActiveHeader = false;
        e = ((Startup00014MakeupMonthly) bonusConfirmScene.menuBase).InitSceneAsync(bonusConfirmScene.mResponse);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  public void onStartScene()
  {
    if (this.mBonus.draw_type == LoginbonusDrawType.monthly_by_day)
      ((Startup00014MakeupMonthly) this.menuBase).OnStartConfirmScene();
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onBackSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
  }

  public void onBackScene()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading)
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    if (this.mBonus.draw_type != LoginbonusDrawType.monthly_by_day)
      return;
    ((Startup00014MakeupMonthly) this.menuBase).OnBackScene();
  }
}
