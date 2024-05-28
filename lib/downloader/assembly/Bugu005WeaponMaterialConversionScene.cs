// Decompiled with JetBrains decompiler
// Type: Bugu005WeaponMaterialConversionScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu005WeaponMaterialConversionScene : NGSceneBase
{
  public Bugu005WeaponMaterialConversionMenu menu;
  private Bugu005WeaponMaterialConversionScene.Mode? oldMode_;

  public static void ChangeScene(bool stack, Bugu005WeaponMaterialConversionScene.Mode mode)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_weapon_material", (stack ? 1 : 0) != 0, (object) mode);
  }

  public override IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public virtual IEnumerator onStartSceneAsync(Bugu005WeaponMaterialConversionScene.Mode mode)
  {
    Bugu005WeaponMaterialConversionScene materialConversionScene = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Future<GameObject> bgF;
    IEnumerator e;
    if (mode == Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial)
    {
      bgF = Res.Prefabs.BackGround.DefaultBackground_storage.Load<GameObject>();
      e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      materialConversionScene.backgroundPrefab = bgF.Result;
    }
    else
    {
      bgF = Res.Prefabs.BackGround.DefaultBackground.Load<GameObject>();
      e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      materialConversionScene.backgroundPrefab = bgF.Result;
    }
    if (materialConversionScene.oldMode_.HasValue)
    {
      int num = (int) mode;
      Bugu005WeaponMaterialConversionScene.Mode? oldMode = materialConversionScene.oldMode_;
      int valueOrDefault = (int) oldMode.GetValueOrDefault();
      if (num == valueOrDefault & oldMode.HasValue)
        goto label_14;
    }
    materialConversionScene.menu.resetRevisions();
label_14:
    materialConversionScene.oldMode_ = new Bugu005WeaponMaterialConversionScene.Mode?(mode);
    e = materialConversionScene.menu.Init(mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void onStartScene(Bugu005WeaponMaterialConversionScene.Mode mode)
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
  }
}
