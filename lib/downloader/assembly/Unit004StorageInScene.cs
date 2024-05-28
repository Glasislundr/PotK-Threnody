// Decompiled with JetBrains decompiler
// Type: Unit004StorageInScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004StorageInScene : NGSceneBase
{
  [SerializeField]
  private Unit004StorageInMenu menu;

  public static void changeScene(bool stack, bool isBack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage_in", (stack ? 1 : 0) != 0, (object) isBack);
  }

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_storage_in", stack);
  }

  public IEnumerator onStartSceneAsync(bool isBack)
  {
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e1 = WebAPI.UnitReservesCount((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    int count = SMManager.Get<PlayerUnitReservesCount>().count;
    e1 = this.menu.Init(playerUnits, count, Persist.unit00411SortAndFilter, isBack);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    IEnumerator e1 = WebAPI.UnitReservesCount((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    int count = SMManager.Get<PlayerUnitReservesCount>().count;
    e1 = this.menu.Init(playerUnits, count, Persist.unit00411SortAndFilter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  public void onStartScene()
  {
  }

  public void onStartScene(bool isBack)
  {
  }

  public override void onEndScene()
  {
    base.onEndScene();
    ((Component) this).GetComponentInChildren<NGxScroll2>().scrollView.Press(false);
  }
}
