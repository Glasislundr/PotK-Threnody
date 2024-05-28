// Decompiled with JetBrains decompiler
// Type: Unit004TrainingScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnitTraining;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/Scene")]
public class Unit004TrainingScene : NGSceneBase
{
  public static readonly string DefSceneName = "unit004_training";
  private bool firstInit_ = true;
  private long? verPlayerUnits_;
  private bool isFromResult_;

  private Unit004TrainingMenu menu => this.menuBase as Unit004TrainingMenu;

  public static void changeScene(bool stack, PlayerUnit target)
  {
    Unit004TrainingScene.changeScene(stack, new Ingredients(Singleton<NGGameDataManager>.GetInstance().currentTraining)
    {
      baseUnit = target
    }, new bool?(false));
  }

  private static void changeScene(
    bool stack,
    Ingredients param,
    bool? bDisabledTab,
    Action exceptionBackScene = null)
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    bool flag = instance.setUnitTrainingParam(param);
    if (bDisabledTab.HasValue)
      instance.unitTrainingOption.isDisabledTab = bDisabledTab.Value;
    if (exceptionBackScene != null)
      Singleton<NGSceneManager>.GetInstance().changeScene(Unit004TrainingScene.DefSceneName, (stack ? 1 : 0) != 0, (object) flag, (object) instance.unitTrainingOption.isDisabledTab, (object) exceptionBackScene);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene(Unit004TrainingScene.DefSceneName, (stack ? 1 : 0) != 0, (object) flag, (object) instance.unitTrainingOption.isDisabledTab);
  }

  public static void changeCombine(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialPlayerUnits = null,
    bool? bDisabledTab = null,
    Action exceptionBackScene = null)
  {
    Unit004TrainingScene.changeScene(stack, new Ingredients(TrainingType.Combine)
    {
      baseUnit = basePlayerUnit,
      materialPlayerUnits = materialPlayerUnits
    }, bDisabledTab, exceptionBackScene);
  }

  private void autoOpenUnityToutaPopupCoroutine()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (instance.fromPopup != NGGameDataManager.FromPopup.Unit004Combine)
      return;
    instance.OnceOpenPopup<GameObject>(this.menu.unityDetailPrefabs, (NGMenuBase) null, (Action) null);
    instance.fromPopup = NGGameDataManager.FromPopup.None;
  }

  public static void changeReinforce(
    bool stack,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] materialPlayerUnits = null,
    bool? bDisabledTab = null,
    Action exceptionBackScene = null)
  {
    Unit004TrainingScene.changeScene(stack, new Ingredients(TrainingType.Reinforce)
    {
      baseUnit = basePlayerUnit,
      materialPlayerUnits = materialPlayerUnits
    }, bDisabledTab, exceptionBackScene);
  }

  public static void changeEvolution(
    bool stack,
    PlayerUnit basePlayerUnit,
    bool? bDisabledTab = null,
    Action exceptionBackScene = null)
  {
    Unit004TrainingScene.changeScene(stack, new Ingredients(TrainingType.Evolution)
    {
      baseUnit = basePlayerUnit
    }, bDisabledTab, exceptionBackScene);
  }

  public static void changeReincarnation(
    bool stack,
    PlayerUnit basePlayerUnit,
    bool? bDisabledTab = null,
    Action exceptionBackScene = null)
  {
    Unit004TrainingScene.changeScene(stack, new Ingredients(TrainingType.Reincarnation)
    {
      baseUnit = basePlayerUnit
    }, bDisabledTab, exceptionBackScene);
  }

  public IEnumerator onStartSceneAsync(bool bReset, bool bDisabledTab, Action exceptionBackScene)
  {
    this.menu.exceptionBackScene = exceptionBackScene;
    yield return (object) this.onStartSceneAsync(bReset, bDisabledTab);
  }

  public IEnumerator onStartSceneAsync(bool bReset, bool bDisabledTab)
  {
    Ingredients currentTrainingParam = Singleton<NGGameDataManager>.GetInstance().currentTrainingParam;
    if (currentTrainingParam == null || currentTrainingParam.baseUnit == (PlayerUnit) null)
    {
      yield return (object) this.doWaitError();
    }
    else
    {
      bReset = bReset || this.firstInit_;
      this.firstInit_ = false;
      if (bReset)
        Singleton<NGGameDataManager>.GetInstance().clearPreviewInheritance();
      this.checkUpdatedBaseUnit(currentTrainingParam);
      if (currentTrainingParam.baseUnit == (PlayerUnit) null)
      {
        yield return (object) this.doWaitError();
      }
      else
      {
        yield return (object) this.menu.doInitialize(currentTrainingParam, bReset, bDisabledTab);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
      }
    }
  }

  private void checkUpdatedBaseUnit(Ingredients param)
  {
    long num1 = SMManager.Revision<PlayerUnit[]>();
    if (this.verPlayerUnits_.HasValue)
    {
      long num2 = num1;
      long? verPlayerUnits = this.verPlayerUnits_;
      long valueOrDefault = verPlayerUnits.GetValueOrDefault();
      if (!(num2 == valueOrDefault & verPlayerUnits.HasValue))
      {
        int unit_id = param.baseUnit.id;
        param.baseUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == unit_id));
        this.menu.resetIngredients(param, true);
      }
    }
    this.verPlayerUnits_ = new long?(num1);
  }

  public void onStartScene(bool bReset, bool bDisabledTab, Action exceptionBackScene)
  {
    this.onStartScene(bReset, bDisabledTab);
  }

  public void onStartScene(bool bReset, bool bDisabledTab)
  {
    this.autoOpenUnityToutaPopupCoroutine();
  }

  public IEnumerator onBackSceneAsync(bool bReset, bool bDisabledTab, Action exceptionBackScene)
  {
    yield return (object) this.onBackSceneAsync(false, bDisabledTab);
  }

  public void onBackScene(bool bReset, bool bDisabledTab, Action exceptionBackScene)
  {
    this.onBackScene(false, bDisabledTab);
  }

  public IEnumerator onBackSceneAsync(bool bReset, bool bDisabledTab)
  {
    if (!this.isFromResult_)
    {
      long num = SMManager.Revision<PlayerUnit[]>();
      long? verPlayerUnits = this.verPlayerUnits_;
      long valueOrDefault = verPlayerUnits.GetValueOrDefault();
      if (num == valueOrDefault & verPlayerUnits.HasValue)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        yield break;
      }
    }
    yield return (object) this.onStartSceneAsync(false, bDisabledTab);
  }

  public void onBackScene(bool bReset, bool bDisabledTab)
  {
    this.isFromResult_ = false;
    this.autoOpenUnityToutaPopupCoroutine();
  }

  private IEnumerator doWaitError()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
    while (true)
      yield return (object) null;
  }

  public void setBackSceneFromResult(TrainingType type, PlayerUnit result)
  {
    this.menu.resetIngredients(new Ingredients(type)
    {
      baseUnit = result
    });
    this.isFromResult_ = true;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
  }

  private void OnDestroy()
  {
    Singleton<NGGameDataManager>.GetInstance().resetTrainingParam(Singleton<TutorialRoot>.GetInstance().IsTutorialFinish());
  }
}
