// Decompiled with JetBrains decompiler
// Type: Bugu005WeaponStorageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu005WeaponStorageScene : NGSceneBase
{
  public Bugu005WeaponStorageMenu menu;

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_weapon_storage", stack);
  }

  public virtual IEnumerator onStartSceneAsync()
  {
    Bugu005WeaponStorageScene weaponStorageScene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Future<GameObject> bgF = Res.Prefabs.BackGround.DefaultBackground_storage.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    weaponStorageScene.backgroundPrefab = bgF.Result;
    e = weaponStorageScene.menu.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public virtual void onBackScene() => this.menu.onBackScene();

  public override void onEndScene() => this.menu.onEndScene();
}
