// Decompiled with JetBrains decompiler
// Type: Bugu00525Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu00525Scene : NGSceneBase
{
  public Bugu00525Menu menu;
  private Bugu00525Scene.Mode? oldMode_;

  public static void ChangeScene(bool stack, Bugu00525Scene.Mode mode)
  {
    switch (mode)
    {
      case Bugu00525Scene.Mode.Weapon:
      case Bugu00525Scene.Mode.WeaponMaterial:
        Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_sell_weapon", (stack ? 1 : 0) != 0, (object) mode);
        break;
      case Bugu00525Scene.Mode.Material:
      case Bugu00525Scene.Mode.Supply:
        Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_sell", (stack ? 1 : 0) != 0, (object) mode);
        break;
      case Bugu00525Scene.Mode.Reisou:
        Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_sell_reisou", (stack ? 1 : 0) != 0, (object) mode);
        break;
    }
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public virtual IEnumerator onStartSceneAsync(Bugu00525Scene.Mode mode)
  {
    Bugu00525Scene bugu00525Scene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Future<GameObject> bgF;
    IEnumerator e;
    if (mode == Bugu00525Scene.Mode.WeaponMaterial)
    {
      bgF = Res.Prefabs.BackGround.DefaultBackground_storage.Load<GameObject>();
      e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Scene.backgroundPrefab = bgF.Result;
    }
    else
    {
      bgF = Res.Prefabs.BackGround.DefaultBackground.Load<GameObject>();
      e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Scene.backgroundPrefab = bgF.Result;
    }
    if (bugu00525Scene.oldMode_.HasValue)
    {
      int num = (int) mode;
      Bugu00525Scene.Mode? oldMode = bugu00525Scene.oldMode_;
      int valueOrDefault = (int) oldMode.GetValueOrDefault();
      if (num == valueOrDefault & oldMode.HasValue)
        goto label_14;
    }
    bugu00525Scene.menu.resetRevisions();
label_14:
    bugu00525Scene.oldMode_ = new Bugu00525Scene.Mode?(mode);
    e = bugu00525Scene.menu.Init(mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(Bugu00525Scene.Mode mode)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isActiveHeader = true;
    Singleton<CommonRoot>.GetInstance().isActiveFooter = true;
  }

  public override void onEndScene() => this.menu.onEndScene();

  public enum Mode
  {
    Weapon,
    WeaponMaterial,
    Material,
    Supply,
    Reisou,
  }
}
