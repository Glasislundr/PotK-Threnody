// Decompiled with JetBrains decompiler
// Type: Bugu0058Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu0058Scene : NGSceneBase
{
  [SerializeField]
  private Bugu0058Menu menu;
  private GameCore.ItemInfo baseGear;

  public static void changeScene(bool stack, GameCore.ItemInfo baseItem)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_8", (stack ? 1 : 0) != 0, (object) baseItem);
  }

  public static void changeScene(bool stack, List<GameCore.ItemInfo> selectItemList)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_8", (stack ? 1 : 0) != 0, (object) selectItemList);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Bugu0058Scene bugu0058Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ComposeBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu0058Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync(GameCore.ItemInfo playerItem)
  {
    this.baseGear = playerItem;
    IEnumerator e = this.menu.SetMenuAsync(this.baseGear, new List<InventoryItem>().Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToList<GameCore.ItemInfo>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(List<GameCore.ItemInfo> selectItemList)
  {
    IEnumerator e = this.menu.SetMenuAsync(this.baseGear, selectItemList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
