// Decompiled with JetBrains decompiler
// Type: NGDuelDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGDuelDataManager : Singleton<NGDuelDataManager>
{
  private BattleMap mMapData;
  private GameObject mMapObject;
  private Color duelAmbientLight = Color.white;
  [SerializeField]
  private Transform mRoot3d;
  private GameObject duelMap;
  private int oldMapId;
  private bool isPreloadCommonEffects;
  private Dictionary<string, RuntimeAnimatorController> cameraAnimator = new Dictionary<string, RuntimeAnimatorController>();
  private Dictionary<int, KeyValuePair<GameObject, RuntimeAnimatorController>> playerUnitResourceCache = new Dictionary<int, KeyValuePair<GameObject, RuntimeAnimatorController>>();
  private Dictionary<string, KeyValuePair<GameObject, RuntimeAnimatorController>> playerUnitVehicleResourceCache = new Dictionary<string, KeyValuePair<GameObject, RuntimeAnimatorController>>();
  private Dictionary<int, GameObject> playerUnitGearResourceCache = new Dictionary<int, GameObject>();
  private Dictionary<int, BE.UnitResource.Attachment> playerUnitEffectResourceCache = new Dictionary<int, BE.UnitResource.Attachment>();
  private Dictionary<string, ClipEventEffectData> preloadClipEventEffectData = new Dictionary<string, ClipEventEffectData>();
  private Dictionary<string, GameObject> preloadDuelEffect = new Dictionary<string, GameObject>();
  private Dictionary<int, Texture> duelCutin = new Dictionary<int, Texture>();
  private List<GameObject> destroyList = new List<GameObject>();
  private NGDuelDataManager.DuelEffectLoader<GameObject>[] attackEffectLoader;
  private NGDuelDataManager.DuelEffectLoader<GameObject>[] magicBulletLoader;
  private List<MagicBullet> loadingMagicBulletTempList = new List<MagicBullet>();
  private NGDuelDataManager.DuelEffectLoader<ClipEventEffectData>[] clipEventEffectLoader;
  private List<UnitUnit>[] preloadCutinUnitMasterLists;
  private IEnumerator _backgroundPreloadingTask;
  private bool turnEffectLoadLock;

  private void addUnitResource(
    PlayerUnit unit,
    GameObject modelPrefab,
    RuntimeAnimatorController animator)
  {
    if (unit == (PlayerUnit) null || this.playerUnitResourceCache.ContainsKey(unit.id))
      return;
    KeyValuePair<GameObject, RuntimeAnimatorController> keyValuePair = new KeyValuePair<GameObject, RuntimeAnimatorController>(modelPrefab, animator);
    this.playerUnitResourceCache.Add(unit.id, keyValuePair);
  }

  private void addUnitVehicleResource(
    PlayerUnit unit,
    GameObject modelPrefab,
    RuntimeAnimatorController animator)
  {
    if (unit == (PlayerUnit) null || this.playerUnitVehicleResourceCache.ContainsKey(unit.unit.vehicle_model_name))
      return;
    KeyValuePair<GameObject, RuntimeAnimatorController> keyValuePair = new KeyValuePair<GameObject, RuntimeAnimatorController>(modelPrefab, animator);
    this.playerUnitVehicleResourceCache.Add(unit.unit.vehicle_model_name, keyValuePair);
  }

  private void addUnitGearResource(GearGear gear, GameObject modelPrefab)
  {
    if (gear == null || this.playerUnitGearResourceCache.ContainsKey(gear.resource_reference_gear_id.ID))
      return;
    this.playerUnitGearResourceCache.Add(gear.resource_reference_gear_id.ID, modelPrefab);
  }

  private void addUnitEffectResource(int effectId, string node, GameObject modelPrefab)
  {
    if (this.playerUnitEffectResourceCache.ContainsKey(effectId))
      return;
    this.playerUnitEffectResourceCache.Add(effectId, new BE.UnitResource.Attachment(node, modelPrefab));
  }

  public ClipEventEffectData GetPreloadClipEventEffectData(string name)
  {
    return string.IsNullOrEmpty(name) || !this.preloadClipEventEffectData.ContainsKey(name) ? (ClipEventEffectData) null : this.preloadClipEventEffectData[name];
  }

  public void AddPreloadClipEventEffectData(ClipEventEffectData effectData)
  {
    if (Object.op_Equality((Object) effectData, (Object) null) || string.IsNullOrEmpty(((Object) effectData).name) || this.preloadClipEventEffectData.ContainsKey(((Object) effectData).name))
      return;
    this.preloadClipEventEffectData.Add(((Object) effectData).name, effectData);
  }

  public GameObject GetPreloadDuelEffect(string name, Transform parent = null)
  {
    if (string.IsNullOrEmpty(name) || !this.preloadDuelEffect.ContainsKey(name))
      return (GameObject) null;
    GameObject preloadDuelEffect = this.preloadDuelEffect[name].Clone(parent);
    this.destroyList.Add(preloadDuelEffect);
    return preloadDuelEffect;
  }

  public void AddPreloadDuelEffect(GameObject prefab)
  {
    if (Object.op_Equality((Object) prefab, (Object) null) || string.IsNullOrEmpty(((Object) prefab).name) || this.preloadDuelEffect.ContainsKey(((Object) prefab).name))
      return;
    this.preloadDuelEffect.Add(((Object) prefab).name, prefab);
  }

  public Texture GetDuelCutin(int unitId)
  {
    return !this.duelCutin.ContainsKey(unitId) ? (Texture) null : this.duelCutin[unitId];
  }

  public void AddDuelCutin(int unitId, Texture cutin)
  {
    if (this.duelCutin.ContainsKey(unitId))
      return;
    this.duelCutin.Add(unitId, cutin);
  }

  public void AddDestroyList(GameObject gameObject) => this.destroyList.Add(gameObject);

  public GameObject mDamagePrefab { get; private set; }

  public GameObject mCriticalEffect { get; private set; }

  public GameObject mMissEffect { get; private set; }

  public GameObject mShadow { get; private set; }

  public GameObject mDuelSupport { get; private set; }

  public GameObject mCriticalFlash { get; private set; }

  public GameObject mWeakEffect { get; private set; }

  public GameObject mResistEffect { get; private set; }

  public GameObject mDustEffect { get; private set; }

  protected override void Initialize() => this.Init();

  protected override void Finlaize() => this.ClearFullCache();

  public void Init() => this.ClearFullCache();

  private void ClearFullCache()
  {
    this.ClearOneTimeDuelCache();
    Object.Destroy((Object) this.mMapObject);
    this.mMapObject = (GameObject) null;
    this.mDamagePrefab = (GameObject) null;
    this.mMissEffect = (GameObject) null;
    this.mShadow = (GameObject) null;
    this.mDuelSupport = (GameObject) null;
    this.mCriticalEffect = (GameObject) null;
    this.mCriticalFlash = (GameObject) null;
    this.mWeakEffect = (GameObject) null;
    this.mResistEffect = (GameObject) null;
    this.mDustEffect = (GameObject) null;
    this.cameraAnimator.Clear();
    this.destroyList.TrimExcess();
    this.loadingMagicBulletTempList.TrimExcess();
    this.oldMapId = -1;
    this.mMapData = (BattleMap) null;
    this.isPreloadCommonEffects = false;
  }

  public void ClearOneTimeDuelCache()
  {
    this.StopBackGroundPreload();
    foreach (Object destroy in this.destroyList)
      Object.Destroy(destroy);
    this.destroyList.Clear();
    this.playerUnitResourceCache.Clear();
    this.playerUnitVehicleResourceCache.Clear();
    this.playerUnitGearResourceCache.Clear();
    this.playerUnitEffectResourceCache.Clear();
    this.preloadDuelEffect.Clear();
    this.preloadClipEventEffectData.Clear();
    this.loadingMagicBulletTempList.Clear();
    this.attackEffectLoader = (NGDuelDataManager.DuelEffectLoader<GameObject>[]) null;
    this.magicBulletLoader = (NGDuelDataManager.DuelEffectLoader<GameObject>[]) null;
    this.clipEventEffectLoader = (NGDuelDataManager.DuelEffectLoader<ClipEventEffectData>[]) null;
    this.preloadCutinUnitMasterLists = (List<UnitUnit>[]) null;
    this.turnEffectLoadLock = false;
  }

  public IEnumerator CreateMapCache(BL.Stage stage, bool isDirectLoad)
  {
    bool through = false;
    IEnumerator e;
    if (isDirectLoad)
    {
      if (this.oldMapId != stage.id)
      {
        this.oldMapId = stage.id;
        this.mMapData = MasterData.BattleMap[this.oldMapId];
        Future<Texture2D> lightMapF = this.mMapData.LoadDuelFarLightmap();
        e = lightMapF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        LightmapSettings.lightmapsMode = (LightmapsMode) 0;
        LightmapSettings.lightmaps = new LightmapData[1]
        {
          new LightmapData() { lightmapColor = lightMapF.Result }
        };
        lightMapF = (Future<Texture2D>) null;
      }
      else
        through = true;
    }
    else
    {
      try
      {
        if (this.oldMapId != stage.mapId)
        {
          this.oldMapId = stage.mapId;
          this.mMapData = MasterData.BattleMap[this.oldMapId];
        }
        else
          through = true;
      }
      catch (KeyNotFoundException ex)
      {
        Debug.LogError((object) string.Format("KeyNotFoundException: Battle Map ID:{0}", (object) this.oldMapId));
        this.mMapData = ((IEnumerable<BattleMap>) MasterData.BattleMapList).First<BattleMap>();
      }
    }
    if (!through)
    {
      Object.Destroy((Object) this.mMapObject);
      this.mMapObject = (GameObject) null;
      Future<GameObject> mapPrefabF = this.mMapData.LoadDuelMap();
      e = mapPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = mapPrefabF.Result;
      this.mRoot3d.localPosition = Vector3.zero;
      this.mRoot3d.localScale = Vector3.one;
      this.mRoot3d.localRotation = new Quaternion();
      this.mMapObject = result.Clone(this.mRoot3d);
      this.mMapObject.transform.position = Vector3.zero;
      this.mMapObject.transform.rotation = Quaternion.identity;
      NGBattle3DObjectManager.ApplyLightmapUV(this.mMapObject, 0);
      this.SetActiveMap(false);
    }
  }

  public IEnumerator CreateMap(
    BL.Stage stage,
    bool isDirectLoad,
    Transform root3d,
    Light directionalLight)
  {
    IEnumerator e = this.CreateMapCache(stage, isDirectLoad);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetActiveMap(true);
    ((Component) directionalLight).transform.rotation = this.mMapData.getDuelDirectionalLightRotate();
    directionalLight.color = this.mMapData.getDuelDirectionalLightColor();
    directionalLight.intensity = this.mMapData.duel_directional_light_intensity;
    this.duelAmbientLight = this.mMapData.getDuelAmbientColor();
    RenderSettings.ambientLight = this.duelAmbientLight;
  }

  public void ResetLight(Light directionalLight)
  {
    if (this.mMapData == null)
      return;
    RenderSettings.ambientLight = this.mMapData.getDuelAmbientColor();
    ((Component) directionalLight).transform.rotation = this.mMapData.getDuelDirectionalLightRotate();
    directionalLight.color = this.mMapData.getDuelDirectionalLightColor();
    directionalLight.intensity = this.mMapData.duel_directional_light_intensity;
  }

  public void SetActiveMap(bool active)
  {
    if (Object.op_Equality((Object) this.mMapObject, (Object) null))
      return;
    this.mMapObject.SetActive(active);
  }

  public bool IsBackgroundPreloading() => this._backgroundPreloadingTask != null;

  public void StartBackGroundPreload(DuelColosseumResult duelResult)
  {
    if (duelResult.player == (BL.Unit) null || duelResult.opponent == (BL.Unit) null)
      return;
    this.StartBackGroundPreload(duelResult.player, duelResult.opponent);
  }

  public void StartBackGroundPreload(DuelResult duelResult)
  {
    if (duelResult.isHeal)
      return;
    this.StartBackGroundPreload(duelResult.attack, duelResult.defense);
  }

  public void StartBackGroundPreload(BL.Unit attack, BL.Unit defense)
  {
    this.StopBackGroundPreload();
    if (attack.isFacility || defense.isFacility)
      return;
    this._backgroundPreloadingTask = this.preloadDuelResources(attack.playerUnit, attack.metamorphosis, defense.playerUnit, defense.metamorphosis);
    this.StartCoroutine(this._backgroundPreloadingTask);
  }

  public void StopBackGroundPreload()
  {
    if (this._backgroundPreloadingTask == null)
      return;
    this.StopCoroutine(this._backgroundPreloadingTask);
    this._backgroundPreloadingTask = (IEnumerator) null;
  }

  private IEnumerator preloadDuelResources(
    PlayerUnit attack,
    SkillMetamorphosis attackMetamor,
    PlayerUnit defense,
    SkillMetamorphosis defenseMetamor)
  {
    yield return (object) this.PreloadCommonDuelEffect();
    if (attack != (PlayerUnit) null)
      yield return (object) this.PreloadPlayerUnit(attack, attackMetamor);
    if (defense != (PlayerUnit) null)
      yield return (object) this.PreloadPlayerUnit(defense, defenseMetamor);
    this._backgroundPreloadingTask = (IEnumerator) null;
  }

  public IEnumerator PreloadCommonDuelEffect()
  {
    NGDuelDataManager ngDuelDataManager = this;
    if (!ngDuelDataManager.isPreloadCommonEffects)
    {
      List<Future<GameObject>> futures = new List<Future<GameObject>>();
      ResourceManager instance = Singleton<ResourceManager>.GetInstance();
      Future<GameObject> future1 = instance.Load<GameObject>("BattleEffects/duel/ef511_damage_number");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future1.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_0)));
      Future<GameObject> future2 = instance.Load<GameObject>("BattleEffects/duel/ef518_ui_critica");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future2.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_1)));
      Future<GameObject> future3 = instance.Load<GameObject>("BattleEffects/duel/ef519_ui_miss");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future3.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_2)));
      Future<GameObject> future4 = instance.Load<GameObject>("BattleEffects/duel/unit_shadow_duel");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future4.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_3)));
      Future<GameObject> future5 = instance.Load<GameObject>("BattleEffects/duel/ef510_bufu_number");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future5.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_4)));
      Future<GameObject> future6 = instance.Load<GameObject>("BattleEffects/duel/ef522_common_critical_hit");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future6.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_5)));
      Future<GameObject> future7 = instance.Load<GameObject>("BattleEffects/duel/ef524_ui_weakness");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future7.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_6)));
      Future<GameObject> future8 = instance.Load<GameObject>("BattleEffects/duel/ef525_ui_resist");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future8.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_7)));
      Future<GameObject> future9 = instance.Load<GameObject>("BattleEffects/duel/ef516_cloud_dust_dual");
      // ISSUE: reference to a compiler-generated method
      futures.Add(future9.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CPreloadCommonDuelEffect\u003Eb__85_8)));
      IEnumerator e = futures.Sequence<GameObject>().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ngDuelDataManager.isPreloadCommonEffects = true;
    }
  }

  public void SetupTurnEffectBackgroundLoader(
    DuelResult duelResult,
    NGDuelUnit playerDUnit,
    NGDuelUnit enemyDUnit)
  {
    this.setupAttackEffectLoader(duelResult, playerDUnit, enemyDUnit);
    this.setupMagicBulletEffectLoader(duelResult);
    this.setupClipEventEffectLoader(duelResult);
    this.setupPreloadCutinLoader(duelResult);
    this.turnEffectLoadLock = false;
  }

  public void StartTurnEffectBackgroundLoad(int turnNum)
  {
    this.StartCoroutine(this.loadTurnEffect(turnNum));
  }

  private IEnumerator loadTurnEffect(int turnNum)
  {
    while (this.turnEffectLoadLock)
      yield return (object) null;
    this.turnEffectLoadLock = true;
    if (this.attackEffectLoader != null && this.attackEffectLoader.Length > turnNum && this.attackEffectLoader[turnNum] != null)
      yield return (object) this.attackEffectLoader[turnNum].DoLoad();
    if (this.magicBulletLoader != null && this.magicBulletLoader.Length > turnNum && this.magicBulletLoader[turnNum] != null)
    {
      yield return (object) this.magicBulletLoader[turnNum].DoLoad();
      foreach (MagicBullet loadingMagicBulletTemp in this.loadingMagicBulletTempList)
      {
        IEnumerator e = loadingMagicBulletTemp.preloadPrefabs();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.loadingMagicBulletTempList.Clear();
    }
    if (this.clipEventEffectLoader != null && this.clipEventEffectLoader.Length > turnNum && this.clipEventEffectLoader[turnNum] != null)
      yield return (object) this.clipEventEffectLoader[turnNum].DoLoad();
    yield return (object) this.loadPreloadCutinByTurnNumber(turnNum);
    this.turnEffectLoadLock = false;
  }

  public bool IsTurnEffectLoadFinished(int turnNum)
  {
    bool flag = true;
    if (this.attackEffectLoader != null && this.attackEffectLoader.Length > turnNum && this.attackEffectLoader[turnNum] != null)
      flag = flag && this.attackEffectLoader[turnNum].IsDone;
    if (this.magicBulletLoader != null && this.magicBulletLoader.Length > turnNum && this.magicBulletLoader[turnNum] != null)
      flag = flag && this.magicBulletLoader[turnNum].IsDone && this.loadingMagicBulletTempList.Count == 0;
    if (this.clipEventEffectLoader != null && this.clipEventEffectLoader.Length > turnNum && this.clipEventEffectLoader[turnNum] != null)
      flag = flag && this.clipEventEffectLoader[turnNum].IsDone;
    if (this.preloadCutinUnitMasterLists != null && this.preloadCutinUnitMasterLists.Length > turnNum && this.preloadCutinUnitMasterLists[turnNum] != null)
      flag = flag && this.preloadCutinUnitMasterLists[turnNum].Count == 0;
    return flag;
  }

  private void setupAttackEffectLoader(
    DuelResult duelResult,
    NGDuelUnit playerDUnit,
    NGDuelUnit enemyDUnit)
  {
    this.attackEffectLoader = new NGDuelDataManager.DuelEffectLoader<GameObject>[((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>()];
    HashSet<string>[] stringSetArray = new HashSet<string>[((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>()];
    int turnNum = 0;
    foreach (BL.DuelTurn turn in duelResult.turns)
    {
      stringSetArray[turnNum] = new HashSet<string>();
      NGDuelUnit unit = (NGDuelUnit) null;
      if (duelResult.isPlayerAttack && turn.isAtackker || !duelResult.isPlayerAttack && !turn.isAtackker)
        unit = playerDUnit;
      else if (duelResult.isPlayerAttack && !turn.isAtackker || !duelResult.isPlayerAttack && turn.isAtackker)
        unit = enemyDUnit;
      stringSetArray[turnNum].UnionWith((IEnumerable<string>) this.getWeaponTrailEffectNameList(duelResult, turnNum, unit));
      foreach (BL.Skill skill in ((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills))
      {
        if (skill.skill.duel_effect != null)
          stringSetArray[turnNum].UnionWith((IEnumerable<string>) skill.skill.duel_effect.preloadEffectFileNameList);
      }
      if (turn.ailmentEffects != null)
      {
        foreach (BattleskillAilmentEffect ailmentEffect in turn.ailmentEffects)
        {
          if (!string.IsNullOrEmpty(ailmentEffect.duel_effect_name))
            stringSetArray[turnNum].Add(ailmentEffect.duel_effect_name);
        }
      }
      if (turn.attackerAilmentEffects != null)
      {
        foreach (BattleskillAilmentEffect attackerAilmentEffect in turn.attackerAilmentEffects)
        {
          if (!string.IsNullOrEmpty(attackerAilmentEffect.duel_effect_name))
            stringSetArray[turnNum].Add(attackerAilmentEffect.duel_effect_name);
        }
      }
      if (turn.invokeSkillExtraInfo != null && turn.invokeSkillExtraInfo.Any<string>((Func<string, bool>) (x => x == "absolute_defense")))
        stringSetArray[turnNum].Add("ef120_holy_shield_s");
      ++turnNum;
    }
    if (playerDUnit.beforeAilmentEffectIDs != null)
    {
      foreach (string str in ((IEnumerable<int>) playerDUnit.beforeAilmentEffectIDs).Where<int>((Func<int, bool>) (x => MasterData.BattleskillAilmentEffect.ContainsKey(x))).Select<int, string>((Func<int, string>) (x => MasterData.BattleskillAilmentEffect[x].duel_effect_name)))
      {
        if (!string.IsNullOrEmpty(str))
          stringSetArray[0].Add(str);
      }
    }
    if (enemyDUnit.beforeAilmentEffectIDs != null)
    {
      foreach (string str in ((IEnumerable<int>) enemyDUnit.beforeAilmentEffectIDs).Where<int>((Func<int, bool>) (x => MasterData.BattleskillAilmentEffect.ContainsKey(x))).Select<int, string>((Func<int, string>) (x => MasterData.BattleskillAilmentEffect[x].duel_effect_name)))
      {
        if (!string.IsNullOrEmpty(str))
          stringSetArray[0].Add(str);
      }
    }
    int index = 0;
    foreach (HashSet<string> stringSet in stringSetArray)
    {
      List<Future<GameObject>> futureList = new List<Future<GameObject>>();
      foreach (string str in stringSet)
      {
        Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/" + str);
        futureList.Add(future);
      }
      this.attackEffectLoader[index] = new NGDuelDataManager.DuelEffectLoader<GameObject>(futureList, new Action<GameObject>(this.AddPreloadDuelEffect));
      ++index;
    }
  }

  private void setupPreloadCutinLoader(DuelResult duelResult)
  {
    this.preloadCutinUnitMasterLists = new List<UnitUnit>[((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>()];
    int index = 0;
    foreach (BL.DuelTurn turn in duelResult.turns)
    {
      this.preloadCutinUnitMasterLists[index] = new List<UnitUnit>();
      foreach (BL.Skill skill in ((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills))
      {
        if (skill.skill.duel_effect != null)
        {
          foreach (UnitUnit preloadCutinUnit in skill.skill.duel_effect.preloadCutinUnitList)
            this.preloadCutinUnitMasterLists[index].Add(preloadCutinUnit);
        }
      }
      ++index;
    }
  }

  private IEnumerator loadPreloadCutinByTurnNumber(int turnNum)
  {
    if (((IEnumerable<List<UnitUnit>>) this.preloadCutinUnitMasterLists).Count<List<UnitUnit>>() > turnNum && this.preloadCutinUnitMasterLists[turnNum] != null)
    {
      foreach (UnitUnit unitUnit in this.preloadCutinUnitMasterLists[turnNum])
      {
        UnitUnit unit = unitUnit;
        if (!Object.op_Inequality((Object) this.GetDuelCutin(unit.ID), (Object) null))
        {
          Future<Sprite> fs = unit.LoadCutin();
          IEnumerator e = fs.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          Texture texture = (Texture) fs.Result.texture;
          if (Object.op_Inequality((Object) texture, (Object) null))
            this.AddDuelCutin(unit.ID, texture);
          else
            Debug.LogError((object) ("preloadEffect Cutin Load Error  ID:" + (object) unit.ID));
          fs = (Future<Sprite>) null;
          unit = (UnitUnit) null;
        }
      }
      this.preloadCutinUnitMasterLists[turnNum].Clear();
    }
  }

  public IEnumerator LoadUnitControllerDuelEffect(NGDuelUnit playerDUnit, NGDuelUnit enemyDUnit)
  {
    NGDuelDataManager ngDuelDataManager = this;
    HashSet<string> stringSet = new HashSet<string>();
    stringSet.UnionWith((IEnumerable<string>) playerDUnit.controllerPreloadEffectList);
    stringSet.UnionWith((IEnumerable<string>) enemyDUnit.controllerPreloadEffectList);
    if (stringSet.Count > 0)
    {
      List<Future<GameObject>> futures = new List<Future<GameObject>>();
      foreach (string str in stringSet)
      {
        Future<GameObject> future = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/" + str);
        // ISSUE: reference to a compiler-generated method
        futures.Add(future.Then<GameObject>(new Func<GameObject, GameObject>(ngDuelDataManager.\u003CLoadUnitControllerDuelEffect\u003Eb__94_0)));
      }
      IEnumerator e = futures.Sequence<GameObject>().Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void setupMagicBulletEffectLoader(DuelResult duelResult)
  {
    this.magicBulletLoader = new NGDuelDataManager.DuelEffectLoader<GameObject>[duelResult.turns.Length];
    HashSet<string>[] stringSetArray = new HashSet<string>[duelResult.turns.Length];
    int index1 = 0;
    foreach (BL.DuelTurn turn in duelResult.turns)
    {
      stringSetArray[index1] = new HashSet<string>();
      if (turn.attackStatus != null && turn.attackStatus.magicBullet != null)
        stringSetArray[index1].Add(turn.attackStatus.magicBullet.prefabName);
      ++index1;
    }
    if (duelResult.attackAttackStatus != null && duelResult.attackAttackStatus.magicBullet != null)
      stringSetArray[0].Add(duelResult.attackAttackStatus.magicBullet.prefabName);
    if (duelResult.defenseAttackStatus != null && duelResult.defenseAttackStatus.magicBullet != null)
      stringSetArray[0].Add(duelResult.defenseAttackStatus.magicBullet.prefabName);
    ResourceManager instance = Singleton<ResourceManager>.GetInstance();
    int index2 = 0;
    foreach (HashSet<string> stringSet in stringSetArray)
    {
      List<Future<GameObject>> futureList = new List<Future<GameObject>>();
      foreach (string str in stringSet)
      {
        Future<GameObject> future = instance.LoadOrNull<GameObject>("BattleEffects/duel/MagicBullets/" + str);
        if (future != null)
          futureList.Add(future);
      }
      this.magicBulletLoader[index2] = new NGDuelDataManager.DuelEffectLoader<GameObject>(futureList, (Action<GameObject>) (f =>
      {
        if (!Object.op_Inequality((Object) f, (Object) null))
          return;
        this.AddPreloadDuelEffect(f);
        MagicBullet component = f.GetComponent<MagicBullet>();
        if (!Object.op_Inequality((Object) component, (Object) null))
          return;
        this.loadingMagicBulletTempList.Add(component);
      }));
      ++index2;
    }
  }

  private void setupClipEventEffectLoader(DuelResult duelResult)
  {
    this.clipEventEffectLoader = new NGDuelDataManager.DuelEffectLoader<ClipEventEffectData>[((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>()];
    HashSet<string>[] stringSetArray = new HashSet<string>[((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>()];
    int index1 = 0;
    foreach (BL.DuelTurn turn in duelResult.turns)
    {
      stringSetArray[index1] = new HashSet<string>();
      foreach (BL.Skill skill in ((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills))
      {
        if (skill.skill.duel_effect != null)
          stringSetArray[index1].UnionWith((IEnumerable<string>) skill.skill.duel_effect.preloadClipEventEffectDataFileNameList);
      }
      ++index1;
    }
    int index2 = 0;
    foreach (HashSet<string> stringSet in stringSetArray)
    {
      List<Future<ClipEventEffectData>> futureList = new List<Future<ClipEventEffectData>>();
      foreach (string str in stringSet)
      {
        Future<ClipEventEffectData> future = Singleton<ResourceManager>.GetInstance().Load<ClipEventEffectData>("BattleEffects/duel/" + str);
        futureList.Add(future);
      }
      this.clipEventEffectLoader[index2] = new NGDuelDataManager.DuelEffectLoader<ClipEventEffectData>(futureList, new Action<ClipEventEffectData>(this.AddPreloadClipEventEffectData));
      ++index2;
    }
  }

  private List<string> getWeaponTrailEffectNameList(
    DuelResult duelResult,
    int turnNum,
    NGDuelUnit unit)
  {
    List<string> trailEffectNameList = new List<string>();
    DuelElementTrailEffect elementTrailEffect = unit.GetElementTrailEffect(turnNum);
    if (elementTrailEffect != null && !string.IsNullOrEmpty(elementTrailEffect.trail_effect_name))
      trailEffectNameList.Add(elementTrailEffect.trail_effect_name);
    return trailEffectNameList;
  }

  public IEnumerator PreloadPlayerUnit(PlayerUnit unit, SkillMetamorphosis metamor)
  {
    SkillMetamorphosis skillMetamorphosis = metamor;
    int metamorId = skillMetamorphosis != null ? skillMetamorphosis.metamorphosis_id : 0;
    Future<GameObject> fmp = unit.LoadModelDuel(metamorId);
    IEnumerator e = fmp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject modelPrefab = fmp.Result;
    Future<RuntimeAnimatorController> frac = unit.LoadDuelAnimator(metamorId);
    e = frac.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    RuntimeAnimatorController result1 = frac.Result;
    this.addUnitResource(unit, modelPrefab, result1);
    string effect_node;
    Future<GameObject> feff = unit.LoadModelUnitAuraEffect(out effect_node, metamorId);
    e = feff.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result2 = feff.Result;
    this.addUnitEffectResource(metamorId, effect_node, result2);
    GearGear weapon;
    Future<GameObject> fweapon;
    if (unit.unit.non_disp_weapon != 1)
    {
      weapon = unit.equippedWeaponGearOrInitial;
      fweapon = weapon.LoadModel();
      e = fweapon.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.addUnitGearResource(weapon, fweapon.Result);
      weapon = (GearGear) null;
      fweapon = (Future<GameObject>) null;
    }
    if (unit.unit.non_disp_weapon == 0)
    {
      weapon = unit.equippedShieldGearOrNull;
      if (weapon != null)
      {
        fweapon = weapon.LoadModel();
        e = fweapon.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.addUnitGearResource(weapon, fweapon.Result);
        fweapon = (Future<GameObject>) null;
      }
      weapon = (GearGear) null;
    }
    if (!string.IsNullOrEmpty(unit.unit.vehicle_model_name))
    {
      fweapon = unit.unit.LoadModelDuelVehicle();
      e = fweapon.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject vehicleModelPrefab = fweapon.Result;
      Future<RuntimeAnimatorController> fvrac = unit.unit.LoadAnimatorControllerDuelVehicle(unit.equippedWeaponGearOrInitial.model_kind);
      e = fvrac.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      RuntimeAnimatorController result3 = fvrac.Result;
      this.addUnitVehicleResource(unit, vehicleModelPrefab, result3);
      fweapon = (Future<GameObject>) null;
      vehicleModelPrefab = (GameObject) null;
      fvrac = (Future<RuntimeAnimatorController>) null;
    }
  }

  public Future<RuntimeAnimatorController> LoadDuelCamera(string name)
  {
    return this.cameraAnimator.ContainsKey(name) ? Future.Single<RuntimeAnimatorController>(this.cameraAnimator[name]) : Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format("Animators/Camera/{0}", (object) name)).Then<RuntimeAnimatorController>((Func<RuntimeAnimatorController, RuntimeAnimatorController>) (x =>
    {
      this.cameraAnimator.Add(name, x);
      return x;
    }));
  }

  private class DuelEffectLoader<T>
  {
    private List<Future<T>> loadItemList = new List<Future<T>>();

    public bool IsDone { get; private set; }

    public DuelEffectLoader(List<Future<T>> futureList, Action<T> onceLoadCallback)
    {
      foreach (Future<T> future in futureList)
        this.loadItemList.Add(future.Then<T>((Func<T, T>) (f =>
        {
          onceLoadCallback(f);
          return f;
        })));
      this.IsDone = false;
    }

    public IEnumerator DoLoad()
    {
      if (!this.IsDone)
      {
        IEnumerator e = this.loadItemList.Sequence<T>().Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.IsDone = true;
      }
    }
  }
}
