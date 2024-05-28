// Decompiled with JetBrains decompiler
// Type: Unit004TrainingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitTraining;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/Menu")]
public class Unit004TrainingMenu : BackButtonMenuBase
{
  [Header("タブ管理")]
  [SerializeField]
  [Tooltip("「統合(0)/進化(1)/強化(2)/転生(3)」の順に設置")]
  private UIButton[] tabButtons_;
  [SerializeField]
  private GameObject rootPages_;
  private static readonly int MAX_PAGE = Enum.GetValues(typeof (TrainingType)).Length;
  private Dictionary<TrainingType, Unit004TrainingPage> dicPage_ = new Dictionary<TrainingType, Unit004TrainingPage>(Unit004TrainingMenu.MAX_PAGE);
  private Dictionary<TrainingType, bool> dicModifiedBase_ = new Dictionary<TrainingType, bool>(Unit004TrainingMenu.MAX_PAGE);
  private Ingredients param_;
  private Unit004TrainingPage current_;
  private bool isInitializing_;
  private bool isDisabledTab_;
  private bool isWaitChangePage_;
  private const int WAIT_SHOW_LOADING = 1;
  private bool isLoading_;

  public GameObject[] unityDetailPrefabs
  {
    get
    {
      Unit004TrainingPage unit004TrainingPage;
      return this.dicPage_.TryGetValue(TrainingType.Combine, out unit004TrainingPage) ? ((Unit004CombinePage) unit004TrainingPage).unityDetailPrefabs : (GameObject[]) null;
    }
  }

  public GameObject mainPanel => this.rootPages_;

  public Action exceptionBackScene { get; set; }

  public IEnumerator doInitialize(Ingredients param, bool bReset, bool bDisabledTab)
  {
    Unit004TrainingMenu unit004TrainingMenu = this;
    unit004TrainingMenu.IsPush = true;
    unit004TrainingMenu.param_ = param;
    unit004TrainingMenu.isInitializing_ = true;
    unit004TrainingMenu.isDisabledTab_ = bDisabledTab;
    Unit004TrainingPage next;
    for (int key = 0; key < unit004TrainingMenu.tabButtons_.Length; ++key)
    {
      bool flag = (TrainingType) key == param.type;
      ((UIButtonColor) unit004TrainingMenu.tabButtons_[key]).isEnabled = !flag;
      if (unit004TrainingMenu.dicPage_.TryGetValue((TrainingType) key, out next) && !flag)
        next.hide(true);
    }
    if (bReset)
      unit004TrainingMenu.resetModifiedBaseFlags();
    if (unit004TrainingMenu.dicPage_.TryGetValue(param.type, out next))
      yield return (object) unit004TrainingMenu.doChangePage(next, param);
    else
      yield return (object) unit004TrainingMenu.doNewPage(param);
    unit004TrainingMenu.isInitializing_ = false;
    unit004TrainingMenu.IsPush = false;
  }

  public IEnumerator doReset(Ingredients param)
  {
    Unit004TrainingMenu unit004TrainingMenu = this;
    if (!Object.op_Equality((Object) unit004TrainingMenu.current_, (Object) null) && unit004TrainingMenu.current_.page == param.type)
    {
      unit004TrainingMenu.IsPush = true;
      unit004TrainingMenu.resetIngredients(param);
      yield return (object) unit004TrainingMenu.doChangePage(unit004TrainingMenu.current_, param, true);
      unit004TrainingMenu.IsPush = false;
    }
  }

  public void resetIngredients(Ingredients param, bool resetModified = false)
  {
    if (!Singleton<NGGameDataManager>.GetInstance().setUnitTrainingParam(param))
      return;
    if (resetModified)
      param.resetWithoutBase();
    this.resetModifiedBaseFlags();
  }

  private void resetModifiedBaseFlags()
  {
    foreach (TrainingType key in this.dicModifiedBase_.Keys.ToArray<TrainingType>())
      this.dicModifiedBase_[key] = true;
  }

  public void showAdvice(string name)
  {
    if (string.IsNullOrEmpty(name))
      this.isWaitChangePage_ = false;
    else
      this.isWaitChangePage_ = Singleton<TutorialRoot>.GetInstance().ShowAdvice(name, finishCallback: new Action(this.finishedAdvice));
  }

  private void finishedAdvice() => this.isWaitChangePage_ = false;

  private void changePage(TrainingType page)
  {
    if (this.isWaitChangePage_ || this.isLoading_ || Object.op_Equality((Object) this.current_, (Object) null) || this.isDisabledTab_ || this.IsPushAndSet())
      return;
    this.isWaitChangePage_ = true;
    ((UIButtonColor) this.tabButtons_[(int) this.current_.page]).isEnabled = true;
    ((UIButtonColor) this.tabButtons_[(int) page]).isEnabled = false;
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    Ingredients ingredients = instance.getTrainingParam(page);
    if (ingredients == null)
    {
      ingredients = new Ingredients(page);
      ingredients.baseUnit = this.param_.baseUnit;
      instance.setUnitTrainingParam(ingredients);
      this.dicModifiedBase_[page] = true;
    }
    else
      instance.setUnitTrainingParam(ingredients);
    Unit004TrainingPage next;
    if (this.dicPage_.TryGetValue(page, out next))
    {
      if (!Object.op_Inequality((Object) this.current_, (Object) next))
        return;
      this.StartCoroutine(this.doChangePage(next, ingredients));
    }
    else
      this.StartCoroutine(this.doNewPage(ingredients));
  }

  private IEnumerator doNewPage(Ingredients param)
  {
    Unit004TrainingMenu main = this;
    if (Object.op_Inequality((Object) main.current_, (Object) null))
      main.StartCoroutine(main.doWaitChange((Unit004TrainingPage) null));
    string str = "Prefabs/unit004_training/";
    Future<GameObject> ld;
    switch (param.type)
    {
      case TrainingType.Combine:
        ld = new ResourceObject(str + "unit_combine").Load<GameObject>();
        break;
      case TrainingType.Evolution:
        ld = new ResourceObject(str + "unit_evolution").Load<GameObject>();
        break;
      case TrainingType.Reinforce:
        ld = new ResourceObject(str + "unit_reinforce").Load<GameObject>();
        break;
      default:
        ld = new ResourceObject(str + "unit_reincarnation").Load<GameObject>();
        break;
    }
    yield return (object) ld.Wait();
    GameObject gameObject = ld.Result.Clone(main.rootPages_.transform);
    gameObject.GetComponent<UIRect>().alpha = 0.0f;
    Unit004TrainingPage s = gameObject.GetComponent<Unit004TrainingPage>();
    main.StartCoroutine(s.doLoadResources());
    yield return (object) s.doInitialize(main, param);
    main.switchPage(main.current_, s);
    main.dicPage_[param.type] = s;
    main.dicModifiedBase_[param.type] = false;
    main.current_ = s;
    main.param_ = param;
    main.isLoading_ = false;
  }

  private IEnumerator doChangePage(
    Unit004TrainingPage next,
    Ingredients param,
    bool disabledWaitChange = false)
  {
    Unit004TrainingMenu unit004TrainingMenu = this;
    ((Component) next).gameObject.SetActive(true);
    if (!disabledWaitChange)
      unit004TrainingMenu.StartCoroutine(unit004TrainingMenu.doWaitChange(next));
    yield return (object) next.doChange(param, unit004TrainingMenu.dicModifiedBase_[next.page]);
    unit004TrainingMenu.switchPage(unit004TrainingMenu.current_, next);
    unit004TrainingMenu.current_ = next;
    unit004TrainingMenu.param_ = param;
    unit004TrainingMenu.dicModifiedBase_[next.page] = false;
    unit004TrainingMenu.isLoading_ = false;
  }

  private void switchPage(Unit004TrainingPage off, Unit004TrainingPage on)
  {
    if (Object.op_Inequality((Object) off, (Object) null) && Object.op_Inequality((Object) off, (Object) on))
      off.hide(this.isInitializing_);
    on.show();
  }

  private IEnumerator doWaitChange(Unit004TrainingPage next)
  {
    this.isLoading_ = true;
    Unit004TrainingPage current = this.current_;
    DateTime waitLoading = DateTime.Now.AddSeconds(1.0);
    PopupManager popupManager = Singleton<PopupManager>.GetInstance();
    while (popupManager.isOpenNoFinish)
      yield return (object) null;
    if (this.isLoading_)
    {
      popupManager.open((GameObject) null, isViewBack: false);
      bool bLoading = false;
      while (this.isLoading_)
      {
        if (!bLoading && waitLoading <= DateTime.Now)
        {
          bLoading = true;
          Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
        }
        yield return (object) null;
      }
      if (bLoading)
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      popupManager.dismiss();
    }
  }

  public override void onBackButton() => this.onClickedBack();

  public void onClickedBack()
  {
    if (this.IsPushAndSet())
      return;
    Ingredients ingredients = new Ingredients(this.param_.type);
    ingredients.baseUnit = this.param_.baseUnit;
    Singleton<NGGameDataManager>.GetInstance().resetTrainingParam();
    this.resetIngredients(ingredients, true);
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = this.param_.baseUnit.id;
    if (this.exceptionBackScene != null)
      this.exceptionBackScene();
    else
      this.backScene();
  }

  public void onCombine() => this.changePage(TrainingType.Combine);

  public void onEvolution() => this.changePage(TrainingType.Evolution);

  public void onReinforce() => this.changePage(TrainingType.Reinforce);

  public void onReincarnation() => this.changePage(TrainingType.Reincarnation);

  public void onSwipedLeft() => this.swipePage(-1);

  public void onSwipedRight() => this.swipePage(1);

  private void swipePage(int dir)
  {
    if (Object.op_Equality((Object) this.current_, (Object) null))
      return;
    int page = (int) this.current_.page;
    this.changePage(dir >= 0 ? (page != 3 ? (TrainingType) (page + 1) : TrainingType.Combine) : (page != 0 ? (TrainingType) (page - 1) : TrainingType.Reincarnation));
  }
}
