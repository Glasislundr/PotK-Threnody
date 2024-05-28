// Decompiled with JetBrains decompiler
// Type: Quest00221Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00221Scene : NGSceneBase
{
  private static string defaultSceneName = "quest002_2_1";
  private Quest00221DetailMenu mainMenu_;

  public Quest00221DetailMenu mainMenu
  {
    get
    {
      if (Object.op_Equality((Object) this.mainMenu_, (Object) null))
        this.mainMenu_ = this.menuBase as Quest00221DetailMenu;
      return this.mainMenu_;
    }
  }

  public static void changeDetailScene(QuestDetailData data, bool isStack = false)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSceneManager>.GetInstance().changeScene(Quest00221Scene.defaultSceneName + "_sea", (isStack ? 1 : 0) != 0, (object) data);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene(Quest00221Scene.defaultSceneName, (isStack ? 1 : 0) != 0, (object) data);
  }

  public IEnumerator onStartSceneAsync(QuestDetailData data)
  {
    Quest00221Scene mon = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = data.Wait((MonoBehaviour) mon, new Action<WebAPI.Response.UserError>(mon.\u003ConStartSceneAsync\u003Eb__5_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (data.isValidate)
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        e = mon.SetSeaBackground();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      e = mon.mainMenu.coInitialize(data);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetSeaBackground()
  {
    Quest00221Scene quest00221Scene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_sea").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      quest00221Scene.backgroundPrefab = bgF.Result;
  }

  public override void onSceneInitialized()
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    base.onSceneInitialized();
  }
}
