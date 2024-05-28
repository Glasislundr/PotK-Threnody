// Decompiled with JetBrains decompiler
// Type: Bugu0053Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu0053Scene : NGSceneBase
{
  public Bugu0053Menu menu;
  private const int SELECT_MAX = 5;
  private List<InventoryItem> playerGears = new List<InventoryItem>();

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().clearStack("bugu005_3");
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_3", stack);
  }

  public static void changeScene(bool stack, List<InventoryItem> gearList)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_3", (stack ? 1 : 0) != 0, (object) gearList);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Bugu0053Scene bugu0053Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ComposeBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu0053Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.menu.InitGearsSynthesis(this.playerGears, SMManager.Get<Player>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(List<InventoryItem> gearList)
  {
    Player player = SMManager.Get<Player>();
    IEnumerator e = this.menu.InitGearsSynthesis(gearList, player);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public void onStartScene(List<InventoryItem> gearList)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public override void onEndScene() => this.menu.onEndScene();
}
