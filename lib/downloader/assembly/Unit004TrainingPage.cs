// Decompiled with JetBrains decompiler
// Type: Unit004TrainingPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitTraining;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (NGTweenParts))]
[RequireComponent(typeof (TweenAlpha))]
[AddComponentMenu("Scenes/Unit/Training/PageBase")]
public abstract class Unit004TrainingPage : MonoBehaviour
{
  [SerializeField]
  protected UILabel txtMyZeny_;
  [SerializeField]
  protected UILabel txtCost_;
  protected Unit004TrainingMenu mainMenu_;
  protected Ingredients target_;
  protected bool isWaitInitalize_;
  protected bool isResetBase_;
  private bool isPlayStartEffect_;
  private bool isInitializedTopTweenAlpha_;
  protected GameObject unitIconPrefab_;
  protected GameObject statusDetailPrefab_;
  private Dictionary<Unit004TrainingPage.SpriteNo, Unit004TrainingPage.UnitSpriteManager> dicUnitSprite_;

  protected bool IsPush
  {
    get => this.mainMenu_.IsPush;
    set => this.mainMenu_.IsPush = value;
  }

  protected bool IsPushAndSet() => this.mainMenu_.IsPushAndSet();

  protected Action exceptionBackScene => this.mainMenu_.exceptionBackScene;

  public bool isLoadingResources { get; private set; }

  protected void setBackSceneFromResult(PlayerUnit result)
  {
    ((Component) this.mainMenu_).gameObject.GetComponent<Unit004TrainingScene>().setBackSceneFromResult(this.page, result);
  }

  public IEnumerator doLoadResources()
  {
    this.isLoadingResources = true;
    yield return (object) this.loadResources();
    this.isLoadingResources = false;
  }

  public IEnumerator doInitialize(Unit004TrainingMenu main, Ingredients target)
  {
    this.mainMenu_ = main;
    this.target_ = target;
    yield return (object) this.doInitialize();
  }

  public IEnumerator doChange(Ingredients targetNext, bool bModified)
  {
    this.preChangeTarget(targetNext, bModified);
    this.target_ = targetNext;
    yield return (object) this.doChange(bModified);
  }

  public abstract TrainingType page { get; }

  protected abstract IEnumerator doInitialize();

  protected abstract IEnumerator loadResources();

  protected virtual void preChangeTarget(Ingredients targetNext, bool bModifiedBase)
  {
  }

  protected abstract IEnumerator doChange(bool modifiedBase);

  protected bool isFocus_ { get; private set; }

  public void show(bool nonEffect = false) => this.playStartEffect(true, nonEffect);

  public void hide(bool nonEffect = false) => this.playStartEffect(false, nonEffect);

  private void playStartEffect(bool bForward, bool nonEffect)
  {
    this.initialzeTopTweenAlpha();
    this.isFocus_ = bForward;
    if (!bForward && !((Component) this).gameObject.activeSelf)
    {
      this.onDisabledEffectFinished();
    }
    else
    {
      if (bForward && !nonEffect)
        ((Component) this).gameObject.GetComponent<UIRect>().alpha = 0.0f;
      NGTweenParts component = ((Component) this).gameObject.GetComponent<NGTweenParts>();
      this.isPlayStartEffect_ = true;
      if (nonEffect)
      {
        component.resetActive(bForward);
        this.onStartEffectFinished();
      }
      else if (bForward && ((Component) this).gameObject.activeSelf)
        component.forceActive(true);
      else
        component.isActive = bForward;
    }
  }

  private void initialzeTopTweenAlpha()
  {
    if (this.isInitializedTopTweenAlpha_)
      return;
    this.isInitializedTopTweenAlpha_ = true;
    TweenAlpha component = ((Component) this).GetComponent<TweenAlpha>();
    ((UITweener) component).delay = 0.0f;
    UITweener[] componentsInChildren = ((Component) this).gameObject.GetComponentsInChildren<UITweener>(true);
    int[] groups = new int[3]{ 11, 12, 13 };
    float num = 0.0f;
    foreach (UITweener uiTweener in ((IEnumerable<UITweener>) componentsInChildren).Where<UITweener>((Func<UITweener, bool>) (x => !((Behaviour) x).enabled && x.style == null && ((IEnumerable<int>) groups).Contains<int>(x.tweenGroup))))
      num = Mathf.Max(num, uiTweener.delay + uiTweener.duration);
    ((UITweener) component).duration = num;
  }

  public void onStartEffectFinished()
  {
    if (!this.isPlayStartEffect_)
      return;
    this.isPlayStartEffect_ = false;
    if (this.isFocus_)
      this.onEnabledEffectFinished();
    else
      this.onDisabledEffectFinished();
  }

  protected virtual void onEnabledEffectFinished()
  {
    ((Component) this).gameObject.GetComponent<UIRect>().alpha = 1f;
  }

  protected virtual void onDisabledEffectFinished()
  {
    ((Component) this).gameObject.SetActive(false);
  }

  protected void setCostZeny(long cost)
  {
    Player current = Player.Current;
    this.txtMyZeny_.SetTextLocalize(current.money);
    this.txtCost_.SetTextLocalize(cost);
    ((UIWidget) this.txtCost_).color = cost <= current.money ? Color.white : Color.red;
  }

  protected IEnumerator doLoadCommonPrefab()
  {
    Future<GameObject> unitIconPrefabF;
    if (Object.op_Equality((Object) this.unitIconPrefab_, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) unitIconPrefabF.Wait();
      this.unitIconPrefab_ = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.statusDetailPrefab_, (Object) null))
    {
      unitIconPrefabF = new ResourceObject("Prefabs/unit/dir_X_unit_status_detail").Load<GameObject>();
      yield return (object) unitIconPrefabF.Wait();
      this.statusDetailPrefab_ = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
  }

  protected IEnumerator initUnitIcon(
    GameObject goParent,
    PlayerUnit unit,
    bool before,
    bool bottomInfoLevel = false,
    Unit004TrainingPage.SpriteNo afterNo = Unit004TrainingPage.SpriteNo.After)
  {
    UnitIcon ui = ((Component) goParent.transform).GetComponentInChildren<UnitIcon>(true);
    if (Object.op_Equality((Object) ui, (Object) null))
    {
      while (Object.op_Equality((Object) this.unitIconPrefab_, (Object) null))
        yield return (object) null;
      ui = this.unitIconPrefab_.CloneAndGetComponent<UnitIcon>(goParent.transform);
      if (!bottomInfoLevel)
        ui.RarityCenter();
    }
    yield return (object) ui.SetPlayerUnit(unit, (PlayerUnit[]) null, (PlayerUnit) null, true, false);
    if (bottomInfoLevel)
    {
      ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      ui.setLevelText(unit.level.ToString());
    }
    if (unit == (PlayerUnit) null)
    {
      ui.onClick = (Action<UnitIconBase>) (_ => { });
      ui.onLongPress = (Action<UnitIconBase>) (_ => { });
    }
    else if (before)
    {
      ui.onClick = (Action<UnitIconBase>) (_ =>
      {
        if (this.IsPushAndSet())
          return;
        this.onClickedBeforeUnitIcon(unit);
      });
      ui.onLongPress = (Action<UnitIconBase>) (_ =>
      {
        if (this.IsPushAndSet())
          return;
        this.onLongPressedBeforeUnitIcon(unit);
      });
    }
    else if (afterNo == Unit004TrainingPage.SpriteNo.After && this.isDisabledAfterUnitIconButton)
    {
      ui.Gray = true;
      ((Collider) ui.buttonBoxCollider).enabled = false;
    }
    else
    {
      ui.Gray = false;
      ((Collider) ui.buttonBoxCollider).enabled = true;
      ui.onClick = (Action<UnitIconBase>) (_ =>
      {
        if (this.IsPushAndSet())
          return;
        this.onClickedAfterUnitIcon(unit, afterNo);
      });
      ui.onLongPress = (Action<UnitIconBase>) (_ =>
      {
        if (this.IsPushAndSet())
          return;
        this.onLongPressedAfterUnitIcon(unit, afterNo);
      });
    }
  }

  protected virtual bool isDisabledAfterUnitIconButton => false;

  protected Unit004TrainingPage.UnitSpriteManager getUnitSpriteManager(
    Unit004TrainingPage.SpriteNo no)
  {
    if (this.dicUnitSprite_ == null)
      this.dicUnitSprite_ = new Dictionary<Unit004TrainingPage.SpriteNo, Unit004TrainingPage.UnitSpriteManager>(Enum.GetValues(typeof (Unit004TrainingPage.SpriteNo)).Length);
    Unit004TrainingPage.UnitSpriteManager unitSpriteManager;
    if (this.dicUnitSprite_.TryGetValue(no, out unitSpriteManager))
      return unitSpriteManager;
    unitSpriteManager = new Unit004TrainingPage.UnitSpriteManager();
    this.dicUnitSprite_.Add(no, unitSpriteManager);
    return unitSpriteManager;
  }

  protected virtual void onClickedBeforeUnitIcon(PlayerUnit unit)
  {
    if (Object.op_Equality((Object) this.statusDetailPrefab_, (Object) null))
      this.IsPush = false;
    else
      this.StartCoroutine(this.doPopupStatusDetail(unit, Unit004TrainingPage.SpriteNo.Before));
  }

  protected virtual void onLongPressedBeforeUnitIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, (PlayerUnit[]) null, true);
  }

  protected virtual void onClickedAfterUnitIcon(PlayerUnit unit, Unit004TrainingPage.SpriteNo no)
  {
    if (Object.op_Equality((Object) this.statusDetailPrefab_, (Object) null))
      this.IsPush = false;
    else
      this.StartCoroutine(this.doPopupStatusDetail(unit, no));
  }

  protected virtual void onLongPressedAfterUnitIcon(
    PlayerUnit unit,
    Unit004TrainingPage.SpriteNo no)
  {
    Unit0042Scene.changeSceneEvolutionUnit(true, unit, (PlayerUnit[]) null, true, no == Unit004TrainingPage.SpriteNo.Memory, IsBuguLock: true);
  }

  private IEnumerator doPopupStatusDetail(PlayerUnit unit, Unit004TrainingPage.SpriteNo no)
  {
    Unit004TrainingPage.UnitSpriteManager usm = this.getUnitSpriteManager(no);
    yield return (object) usm.doLoad(unit);
    Singleton<PopupManager>.GetInstance().open(this.statusDetailPrefab_).GetComponent<Unit004StatusDetailDialog>().Initialize(unit, usm.sprite_, no == Unit004TrainingPage.SpriteNo.Memory);
  }

  protected List<int> getMaterialUnitIDs(
    PlayerUnit base_unit,
    PlayerUnit[] sources,
    UnitUnit[] materials)
  {
    List<int> materialUnitIds = new List<int>();
    List<PlayerUnit> list = ((IEnumerable<PlayerUnit>) sources).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id != base_unit.id)).ToList<PlayerUnit>();
    for (int index = 0; index < materials.Length; ++index)
    {
      UnitUnit x = materials[index];
      if (x.IsNormalUnit)
      {
        foreach (PlayerUnit playerUnit in (IEnumerable<PlayerUnit>) list.Where<PlayerUnit>((Func<PlayerUnit, bool>) (y => y._unit == x.ID)).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (y => y.level)))
        {
          if (!materialUnitIds.Contains(playerUnit.id))
          {
            materialUnitIds.Add(playerUnit.id);
            break;
          }
        }
      }
    }
    return materialUnitIds;
  }

  protected List<int> getMaterialMaterialUnitIDs(PlayerMaterialUnit[] sources, UnitUnit[] materials)
  {
    List<int> source = new List<int>();
    for (int index = 0; index < materials.Length; ++index)
    {
      UnitUnit x = materials[index];
      if (x.IsMaterialUnit)
      {
        foreach (PlayerMaterialUnit playerMaterialUnit in ((IEnumerable<PlayerMaterialUnit>) sources).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (y => y._unit == x.ID)))
        {
          PlayerMaterialUnit p = playerMaterialUnit;
          if (source.Count<int>((Func<int, bool>) (i => i == p.id)) < p.quantity)
          {
            source.Add(p.id);
            break;
          }
        }
      }
    }
    return source;
  }

  protected Dictionary<int, Queue<PlayerUnit>> makePlayerUnitMaterials(
    UnitUnit[] materials,
    int exclude_id)
  {
    Dictionary<UnitUnit, int> source1 = new Dictionary<UnitUnit, int>(materials.Length);
    for (int index = 0; index < materials.Length; ++index)
    {
      if (!source1.ContainsKey(materials[index]))
        source1.Add(materials[index], 0);
      source1[materials[index]]++;
    }
    List<Tuple<UnitUnit, int>> list = source1.Select<KeyValuePair<UnitUnit, int>, Tuple<UnitUnit, int>>((Func<KeyValuePair<UnitUnit, int>, Tuple<UnitUnit, int>>) (p => Tuple.Create<UnitUnit, int>(p.Key, p.Value))).ToList<Tuple<UnitUnit, int>>();
    Dictionary<int, Queue<PlayerUnit>> dictionary = new Dictionary<int, Queue<PlayerUnit>>();
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnitArray = SMManager.Get<PlayerMaterialUnit[]>();
    HashSet<int> source2 = new HashSet<int>(list.Where<Tuple<UnitUnit, int>>((Func<Tuple<UnitUnit, int>, bool>) (x => x.Item1.IsNormalUnit)).Select<Tuple<UnitUnit, int>, int>((Func<Tuple<UnitUnit, int>, int>) (y => y.Item1.ID)));
    HashSet<int> source3 = new HashSet<int>(list.Where<Tuple<UnitUnit, int>>((Func<Tuple<UnitUnit, int>, bool>) (x => x.Item1.IsMaterialUnit)).Select<Tuple<UnitUnit, int>, int>((Func<Tuple<UnitUnit, int>, int>) (y => y.Item1.ID)));
    if (source2.Any<int>())
    {
      foreach (PlayerUnit playerUnit in playerUnitArray)
      {
        if (playerUnit.id != exclude_id && source2.Contains(playerUnit._unit))
        {
          Queue<PlayerUnit> playerUnitQueue;
          if (!dictionary.TryGetValue(playerUnit._unit, out playerUnitQueue))
          {
            playerUnitQueue = new Queue<PlayerUnit>();
            dictionary.Add(playerUnit._unit, playerUnitQueue);
          }
          playerUnitQueue.Enqueue(playerUnit);
        }
      }
    }
    if (source3.Any<int>())
    {
      foreach (PlayerMaterialUnit playerMaterialUnit in playerMaterialUnitArray)
      {
        PlayerMaterialUnit pmu = playerMaterialUnit;
        if (pmu.quantity > 0 && source3.Contains(pmu._unit))
        {
          Queue<PlayerUnit> playerUnitQueue;
          if (!dictionary.TryGetValue(pmu._unit, out playerUnitQueue))
          {
            playerUnitQueue = new Queue<PlayerUnit>();
            dictionary.Add(pmu._unit, playerUnitQueue);
          }
          int num = Mathf.Min(list.First<Tuple<UnitUnit, int>>((Func<Tuple<UnitUnit, int>, bool>) (x => x.Item1.ID == pmu._unit)).Item2, pmu.quantity);
          for (int count = 0; count < num; ++count)
            playerUnitQueue.Enqueue(PlayerUnit.CreateByPlayerMaterialUnit(pmu, count));
        }
      }
    }
    return dictionary;
  }

  protected void showAdvice(string name = null) => this.mainMenu_.showAdvice(name);

  protected enum Condition
  {
    Money,
    Material,
    Level,
    Favorite,
    UnityValue,
    Limit,
  }

  [Flags]
  public enum ErrorFlag
  {
    Clear = 0,
    Money = 1,
    Material = 2,
    Level = 4,
    Favorite = 8,
    UnityValue = 16, // 0x00000010
    Limit = 32, // 0x00000020
    Any = Limit | UnityValue | Favorite | Level | Material | Money, // 0x0000003F
  }

  protected class UnitSpriteManager
  {
    public int unit_id_;
    public int job_id_;
    public Sprite sprite_;

    public IEnumerator doLoad(PlayerUnit unit)
    {
      UnitUnit unit1 = unit.unit;
      int id = unit.getJobData().ID;
      if (!Object.op_Inequality((Object) this.sprite_, (Object) null) || this.unit_id_ != unit1.ID || this.job_id_ != id)
      {
        this.sprite_ = (Sprite) null;
        this.unit_id_ = unit1.ID;
        this.job_id_ = id;
        Future<Sprite> ld = unit1.LoadSpriteLarge(id, 1f);
        yield return (object) ld.Wait();
        this.sprite_ = ld.Result;
      }
    }
  }

  protected enum SpriteNo
  {
    Before,
    After,
    Memory,
  }
}
