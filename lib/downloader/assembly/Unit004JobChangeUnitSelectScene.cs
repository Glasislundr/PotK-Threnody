// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeUnitSelectScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004JobChangeUnitSelectScene : NGSceneBase
{
  private static readonly string DefaultName = "unit004_JobChange_List";
  [SerializeField]
  private float waitEndLoading_ = 2f;
  private Unit004JobChangeUnitSelectMenu menu_;

  public static void changeScene(bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004JobChangeUnitSelectScene.DefaultName, isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Unit004JobChangeUnitSelectScene changeUnitSelectScene = this;
    changeUnitSelectScene.menu_ = changeUnitSelectScene.menuBase as Unit004JobChangeUnitSelectMenu;
    int num = changeUnitSelectScene.menu_.isNeedReset ? 1 : 0;
    PlayerUnit[] targets = JobChangeUtil.getTargets();
    if (num != 0)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      changeUnitSelectScene.menu_.SetIconType(UnitMenuBase.IconType.Normal);
      yield return (object) changeUnitSelectScene.menu_.Init(targets);
    }
    else
      yield return (object) changeUnitSelectScene.menu_.UpdateInfoAndScroll(targets);
  }

  public void onStartScene() => this.StartCoroutine(this.doWaitEndLoading());

  private IEnumerator doWaitEndLoading()
  {
    yield return (object) new WaitForSeconds(this.waitEndLoading_);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
