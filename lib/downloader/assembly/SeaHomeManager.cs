// Decompiled with JetBrains decompiler
// Type: SeaHomeManager
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
public class SeaHomeManager : MonoBehaviour
{
  [SerializeField]
  private Light directionalLight;
  [SerializeField]
  private GameObject Root3D;
  [SerializeField]
  private SeaHomeUnitController[] unitControllers;
  [SerializeField]
  private Sea030HomeMenu menu;
  [SerializeField]
  private Sea030HomeScene scene;
  [SerializeField]
  private SeaHomeCameraController cameraController;
  private SeaHomeUnitController nowTouchUnit;
  private SeaHomeUnitController nowLookupedUnit;
  private GameObject shadowPrefab;
  private SeaHomeMap nowMap;
  private GameObject mapObject;
  private bool isInitUnit;
  private bool isShowAllGuest;

  public SeaHomeUnitController[] UnitControlers => this.unitControllers;

  public SeaHomeCameraController.CameraMode CameraMode => this.cameraController.cameraMode;

  public bool IsShowAllGuest => this.isShowAllGuest;

  public bool isUnitInit
  {
    get
    {
      return ((IEnumerable<SeaHomeUnitController>) this.unitControllers).All<SeaHomeUnitController>((Func<SeaHomeUnitController, bool>) (x => x.IsInit));
    }
  }

  public IEnumerator Init()
  {
    SeaHomeManager seaHomeManager = this;
    seaHomeManager.cameraController.Init(seaHomeManager);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ShadowF;
    if (Object.op_Equality((Object) seaHomeManager.shadowPrefab, (Object) null))
    {
      ShadowF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/unit_shadow_duel");
      e = ShadowF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      seaHomeManager.shadowPrefab = ShadowF.Result;
      ShadowF = (Future<GameObject>) null;
    }
    DateTime nowDateTime = ServerTime.NowAppTimeAddDelta();
    SeaHomeMap activeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(nowDateTime);
    if (activeMap != null && (seaHomeManager.nowMap == null || seaHomeManager.nowMap != activeMap))
    {
      seaHomeManager.nowMap = activeMap;
      if (Object.op_Implicit((Object) seaHomeManager.mapObject))
        Object.Destroy((Object) seaHomeManager.mapObject);
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromBattleMap(activeMap.map), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!string.IsNullOrEmpty(activeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(activeMap.bgm_cue_name))
        seaHomeManager.scene.SetBgm(activeMap.bgm_cuesheet_name, activeMap.bgm_cue_name);
      BattleMap mapData = activeMap.map;
      ShadowF = mapData.LoadDuelMap();
      e = ShadowF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      seaHomeManager.mapObject = ShadowF.Result.Clone(seaHomeManager.Root3D.transform);
      LightmapSettings.lightmapsMode = (LightmapsMode) 0;
      LightmapData ld0 = new LightmapData();
      LightmapData ld1 = new LightmapData();
      Future<Texture2D> duelFarF = mapData.LoadDuelFarLightmap();
      Future<Texture2D> fieldFarF = mapData.LoadFieldFarLightmap();
      e = duelFarF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = fieldFarF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ld0.lightmapColor = duelFarF.Result;
      ld1.lightmapColor = fieldFarF.Result;
      LightmapSettings.lightmaps = new LightmapData[2]
      {
        ld0,
        ld1
      };
      ((Component) seaHomeManager.directionalLight).transform.rotation = mapData.getDuelDirectionalLightRotate();
      seaHomeManager.directionalLight.color = mapData.getDuelDirectionalLightColor();
      seaHomeManager.directionalLight.intensity = mapData.duel_directional_light_intensity;
      RenderSettings.ambientLight = mapData.getDuelAmbientColor();
      NGBattle3DObjectManager.ApplyLightmapUV(seaHomeManager.mapObject, 0);
      mapData = (BattleMap) null;
      ShadowF = (Future<GameObject>) null;
      ld0 = (LightmapData) null;
      ld1 = (LightmapData) null;
      duelFarF = (Future<Texture2D>) null;
      fieldFarF = (Future<Texture2D>) null;
    }
    List<int> sameUnitIDs = new List<int>();
    IEnumerable<PlayerUnit> source1 = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsSea));
    int trust_max = PlayerUnit.GetTrustRateMax();
    if (!Persist.seaHomeUnitDate.Exists)
    {
      IEnumerable<PlayerUnit> source2 = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => Mathf.Approximately(x.trust_rate, (float) trust_max)));
      if (source2.Count<PlayerUnit>() > 0)
      {
        IEnumerable<int> source3 = source2.Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.same_character_id));
        for (int index = 0; index < source3.Count<int>() && index < seaHomeManager.unitControllers.Length; ++index)
        {
          int num = source3.ElementAt<int>(index);
          if (!sameUnitIDs.Contains(num))
            sameUnitIDs.Add(num);
        }
      }
    }
    else
    {
      try
      {
        IEnumerable<PlayerUnit> source4 = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => Mathf.Approximately(x.trust_rate, (float) trust_max)));
        if (source4.Count<PlayerUnit>() > 0)
        {
          IEnumerable<int> source5 = source4.Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.same_character_id));
          for (int index = 0; index < source5.Count<int>() && index < seaHomeManager.unitControllers.Length; ++index)
          {
            int num = source5.ElementAt<int>(index);
            if (!Persist.seaHomeUnitDate.Data.TrustMaxSameUnitIDs.Contains(num) && !sameUnitIDs.Contains(num))
              sameUnitIDs.Add(num);
          }
        }
        if (sameUnitIDs.Count < seaHomeManager.unitControllers.Length)
        {
          Tuple<DateTime, DateTime> timeZone = ((IEnumerable<SeaHomeTimeZone>) MasterData.SeaHomeTimeZoneList).FirstOrDefault<SeaHomeTimeZone>((Func<SeaHomeTimeZone, bool>) (x => x.WithIn(Persist.seaHomeUnitDate.Data.saveTime))).GetTimeZone(Persist.seaHomeUnitDate.Data.saveTime);
          if (timeZone.Item1 <= nowDateTime)
          {
            if (timeZone.Item2 >= nowDateTime)
            {
              List<int> displaySameUnitIds = Persist.seaHomeUnitDate.Data.DisplaySameUnitIDs;
              int count = sameUnitIDs.Count;
              int index = 0;
              while (count < seaHomeManager.unitControllers.Length)
              {
                if (index < displaySameUnitIds.Count)
                {
                  if (!sameUnitIDs.Contains(displaySameUnitIds[index]))
                    sameUnitIDs.Add(displaySameUnitIds[index]);
                  ++count;
                  ++index;
                }
                else
                  break;
              }
            }
          }
        }
      }
      catch
      {
        Persist.seaHomeUnitDate.Delete();
        IEnumerable<PlayerUnit> source6 = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => Mathf.Approximately(x.trust_rate, (float) trust_max)));
        if (source6.Count<PlayerUnit>() > 0)
        {
          IEnumerable<int> source7 = source6.Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.same_character_id));
          for (int index = 0; index < source7.Count<int>(); ++index)
          {
            if (index < seaHomeManager.unitControllers.Length)
            {
              int num = source7.ElementAt<int>(index);
              if (!sameUnitIDs.Contains(num))
                sameUnitIDs.Add(num);
            }
            else
              break;
          }
        }
      }
    }
    if (sameUnitIDs.Count < seaHomeManager.unitControllers.Length)
    {
      if (source1.Count<PlayerUnit>() > 0 && sameUnitIDs.Count <= 0)
      {
        float maxTrust = source1.Max<PlayerUnit>((Func<PlayerUnit, float>) (x => x.trust_rate));
        sameUnitIDs.Add(source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => Mathf.Approximately(maxTrust, x.trust_rate))).Shuffle<PlayerUnit>().First<PlayerUnit>().unit.same_character_id);
      }
      Dictionary<int, float> dictionary1 = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !sameUnitIDs.Contains(x.unit.same_character_id))).GroupBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.same_character_id)).ToDictionary<IGrouping<int, PlayerUnit>, int, float>((Func<IGrouping<int, PlayerUnit>, int>) (x => x.Key), (Func<IGrouping<int, PlayerUnit>, float>) (y => y.Max<PlayerUnit>((Func<PlayerUnit, float>) (z => z.trust_rate))));
      Dictionary<int, int> source8 = new Dictionary<int, int>();
      foreach (UnitUnit unitUnit in ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x =>
      {
        if (!x.IsSea || !x.IsNormalUnit || x.character.category != UnitCategory.player || sameUnitIDs.Contains(x.same_character_id))
          return false;
        DateTime dateTime = ServerTime.NowAppTimeAddDelta();
        DateTime? publishedAt = x.published_at;
        return publishedAt.HasValue && dateTime >= publishedAt.GetValueOrDefault();
      })).Distinct<UnitUnit>((IEqualityComparer<UnitUnit>) new LambdaEqualityComparer<UnitUnit>((Func<UnitUnit, UnitUnit, bool>) ((a, b) => a.same_character_id == b.same_character_id))))
      {
        int num = 50;
        if (dictionary1.ContainsKey(unitUnit.same_character_id))
          num += Mathf.RoundToInt(dictionary1[unitUnit.same_character_id]);
        source8.Add(unitUnit.same_character_id, num);
      }
      Dictionary<int, int> dictionary2 = source8.OrderByDescending<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (x => x.Value)).ToDictionary<KeyValuePair<int, int>, int, int>((Func<KeyValuePair<int, int>, int>) (y => y.Key), (Func<KeyValuePair<int, int>, int>) (z => z.Value));
      for (int count = sameUnitIDs.Count; count < seaHomeManager.unitControllers.Length; ++count)
      {
        int key = seaHomeManager.RandomChoice(dictionary2);
        if (key > 0)
          sameUnitIDs.Add(key);
        dictionary2.Remove(key);
      }
    }
    Persist.seaHomeUnitDate.Data.saveTime = nowDateTime;
    Persist.seaHomeUnitDate.Data.DisplaySameUnitIDs = sameUnitIDs;
    Persist.seaHomeUnitDate.Flush();
    List<SeaHomeManager.UnitConrtolleData> source9 = new List<SeaHomeManager.UnitConrtolleData>();
    for (int i = 0; i < sameUnitIDs.Count; ++i)
    {
      PlayerUnit playerUnit = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.same_character_id == sameUnitIDs[i])).OrderByDescending<PlayerUnit, int>((Func<PlayerUnit, int>) (x => !x.favorite ? 0 : 1)).ThenByDescending<PlayerUnit, float>((Func<PlayerUnit, float>) (x => x.trust_rate)).ThenByDescending<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.rarity.index)).ThenBy<PlayerUnit, DateTime>((Func<PlayerUnit, DateTime>) (x => x.created_at)).FirstOrDefault<PlayerUnit>();
      if (playerUnit == (PlayerUnit) null)
      {
        UnitUnit unitUnit = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).FirstOrDefault<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == sameUnitIDs[i]));
        source9.Add(new SeaHomeManager.UnitConrtolleData(unitUnit.ID, (PlayerUnit) null));
      }
      else
        source9.Add(new SeaHomeManager.UnitConrtolleData(playerUnit.unit.ID, playerUnit));
    }
    List<SeaHomeUnitController> homeUnitControllerList = new List<SeaHomeUnitController>();
    for (int index = 0; index < seaHomeManager.unitControllers.Length; ++index)
    {
      SeaHomeUnitController unitController = seaHomeManager.unitControllers[index];
      if (unitController.UnitData != null && unitController.IsInit)
      {
        SeaHomeManager.UnitConrtolleData unitData = source9.FirstOrDefault<SeaHomeManager.UnitConrtolleData>((Func<SeaHomeManager.UnitConrtolleData, bool>) (x => x.Equals((object) unitController.UnitData)));
        if (unitData != null)
        {
          unitController.SetUnitData(unitData, seaHomeManager);
          source9.Remove(unitData);
          continue;
        }
      }
      homeUnitControllerList.Add(unitController);
    }
    if (homeUnitControllerList.Count > 0)
    {
      int num = 0;
      foreach (SeaHomeManager.UnitConrtolleData unitData in source9)
        homeUnitControllerList[num++].SetUnitData(unitData, seaHomeManager);
      seaHomeManager.StartCoroutine("LoadUnits", (object) homeUnitControllerList);
    }
  }

  private int RandomChoice(Dictionary<int, int> choiceData)
  {
    if (choiceData.Count <= 0)
      return 0;
    int num1 = Random.Range(0, choiceData.Sum<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Value)));
    int num2 = 0;
    foreach (KeyValuePair<int, int> keyValuePair in choiceData)
    {
      if (num2 > num1)
        return keyValuePair.Key;
      num2 += keyValuePair.Value;
    }
    return choiceData.First<KeyValuePair<int, int>>().Key;
  }

  public void ResetAmbientLight()
  {
    if (this.nowMap == null)
      return;
    RenderSettings.ambientLight = this.nowMap.map.getDuelAmbientColor();
  }

  public void onEndScene()
  {
    if (this.cameraController.SetMode(SeaHomeCameraController.CameraMode.NORMAL))
    {
      if (this.cameraController.IsLookuped)
      {
        foreach (SeaHomeUnitController unitController in this.unitControllers)
        {
          if (Object.op_Inequality((Object) unitController, (Object) this.nowLookupedUnit))
            unitController.Show();
        }
        this.nowLookupedUnit = (SeaHomeUnitController) null;
      }
      this.cameraController.Reset();
    }
    else if (this.cameraController.IsLookuped)
    {
      if (Object.op_Inequality((Object) this.nowLookupedUnit, (Object) null))
        this.ResetLookuped(this.nowLookupedUnit);
      foreach (SeaHomeUnitController unitController in this.unitControllers)
      {
        if (Object.op_Inequality((Object) unitController, (Object) this.nowLookupedUnit))
          unitController.Show();
      }
      this.nowLookupedUnit = (SeaHomeUnitController) null;
    }
    this.StopCoroutine("LoadUnits");
  }

  public void SetTouchBipObject(GameObject bipObject)
  {
    if (this.unitControllers == null)
      return;
    this.nowTouchUnit = ((IEnumerable<SeaHomeUnitController>) this.unitControllers).FirstOrDefault<SeaHomeUnitController>((Func<SeaHomeUnitController, bool>) (x => Object.op_Inequality((Object) x, (Object) null) && x.IsTouch(bipObject)));
    if (!Object.op_Inequality((Object) this.nowTouchUnit, (Object) null))
      return;
    if (this.cameraController.cameraMode == SeaHomeCameraController.CameraMode.OPERATION)
    {
      if (this.cameraController.IsLookuped)
        this.ResetLookuped(this.nowTouchUnit);
      else
        this.SetLookuped(this.nowTouchUnit);
    }
    else
      this.menu.OnChangeTalkMode(this.nowTouchUnit.UnitData);
  }

  public void ChangeCameraMode()
  {
    if (this.cameraController.cameraMode == SeaHomeCameraController.CameraMode.NORMAL)
    {
      this.cameraController.SetMode(SeaHomeCameraController.CameraMode.OPERATION);
    }
    else
    {
      if (!this.cameraController.SetMode(SeaHomeCameraController.CameraMode.NORMAL))
        return;
      if (this.cameraController.IsLookuped)
      {
        foreach (SeaHomeUnitController unitController in this.unitControllers)
        {
          if (Object.op_Inequality((Object) unitController, (Object) this.nowLookupedUnit))
            unitController.Show();
        }
        this.nowLookupedUnit = (SeaHomeUnitController) null;
      }
      this.cameraController.Reset();
    }
  }

  private IEnumerator LoadUnits(List<SeaHomeUnitController> initUnitControllers)
  {
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource(initUnitControllers.Where<SeaHomeUnitController>((Func<SeaHomeUnitController, bool>) (x => x.UnitData != null)).Select<SeaHomeUnitController, UnitUnit>((Func<SeaHomeUnitController, UnitUnit>) (x => x.UnitData.Unit)), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    foreach (SeaHomeUnitController initUnitController in initUnitControllers)
    {
      e = initUnitController.Init(this.shadowPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.isInitUnit = true;
  }

  private IEnumerator LoadUnit(
    Tuple<SeaHomeManager.UnitConrtolleData, int> changeData)
  {
    SeaHomeManager owner = this;
    owner.unitControllers[changeData.Item2].SetUnitData(changeData.Item1, owner);
    IEnumerator e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) new UnitUnit[1]
    {
      changeData.Item1.Unit
    }, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    owner.unitControllers[changeData.Item2].Clear();
    e = owner.unitControllers[changeData.Item2].Init(owner.shadowPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetLookupedAuto(SeaHomeUnitController lookup)
  {
    if (this.cameraController.cameraMode == SeaHomeCameraController.CameraMode.OPERATION)
      return;
    this.SetLookuped(lookup);
  }

  public void ResetLookupedAuto(SeaHomeUnitController lookup)
  {
    if (this.cameraController.cameraMode == SeaHomeCameraController.CameraMode.OPERATION)
      return;
    this.ResetLookuped(lookup);
  }

  public void SetLookuped(SeaHomeUnitController lookup)
  {
    if (!this.cameraController.SetLookupUnit(lookup.UnitTransform))
      return;
    this.nowLookupedUnit = lookup;
    foreach (SeaHomeUnitController unitController in this.unitControllers)
    {
      if (Object.op_Inequality((Object) unitController, (Object) lookup))
        unitController.Hide();
    }
  }

  public void ResetLookuped(SeaHomeUnitController lookup)
  {
    if (!this.cameraController.ResetLookupUnit(lookup.UnitTransform))
      return;
    this.nowLookupedUnit = (SeaHomeUnitController) null;
    foreach (SeaHomeUnitController unitController in this.unitControllers)
    {
      if (Object.op_Inequality((Object) unitController, (Object) lookup))
        unitController.Show();
    }
  }

  public SeaHomeUnitController.UnitPositions GetNextMovePosition(SeaHomeUnitController target)
  {
    List<SeaHomeUnitController.UnitPositions> candidacy = new List<SeaHomeUnitController.UnitPositions>((IEnumerable<SeaHomeUnitController.UnitPositions>) SeaHomeUnitController.inPositions);
    ((IEnumerable<SeaHomeUnitController>) this.unitControllers).ForEach<SeaHomeUnitController>((Action<SeaHomeUnitController>) (x =>
    {
      if (!candidacy.Contains(x.TargetPosition))
        return;
      candidacy.Remove(x.TargetPosition);
    }));
    if (candidacy.Count <= 0)
      candidacy.AddRange((IEnumerable<SeaHomeUnitController.UnitPositions>) SeaHomeUnitController.inPositions);
    return candidacy.Shuffle<SeaHomeUnitController.UnitPositions>().First<SeaHomeUnitController.UnitPositions>();
  }

  public bool DuplicatePosition(SeaHomeUnitController target)
  {
    List<SeaHomeUnitController.UnitPositions> unitPositionsList = new List<SeaHomeUnitController.UnitPositions>((IEnumerable<SeaHomeUnitController.UnitPositions>) SeaHomeUnitController.inPositions);
    return ((IEnumerable<SeaHomeUnitController>) this.unitControllers).Any<SeaHomeUnitController>((Func<SeaHomeUnitController, bool>) (x => Object.op_Inequality((Object) x, (Object) target) && x.NowWait && x.NowPosition == target.NowPosition));
  }

  public bool CheckDance(SeaHomeUnitController unit)
  {
    return ((IEnumerable<SeaHomeUnitController>) this.unitControllers).All<SeaHomeUnitController>((Func<SeaHomeUnitController, bool>) (x => x.UnitData.PlayerUnit != (PlayerUnit) null && (double) x.UnitData.PlayerUnit.trust_rate >= 25.0 && x.PrevStatus != SeaHomeUnitController.UnitStatus.Dance && (Object.op_Equality((Object) x, (Object) unit) || x.NowWait) && x.NowStand && !x.NowHide)) && ((IEnumerable<SeaHomeUnitController>) this.unitControllers).Select<SeaHomeUnitController, SeaHomeUnitController.UnitPositions>((Func<SeaHomeUnitController, SeaHomeUnitController.UnitPositions>) (x => x.NowPosition)).Distinct<SeaHomeUnitController.UnitPositions>().Count<SeaHomeUnitController.UnitPositions>() == this.unitControllers.Length;
  }

  public void PlayDance()
  {
    Singleton<NGSoundManager>.GetInstance().PlayBgmFile("BgmSeaEvent", "bgm233");
    ((IEnumerable<SeaHomeUnitController>) this.unitControllers).ForEach<SeaHomeUnitController>((Action<SeaHomeUnitController>) (x => x.PlayDance()));
  }

  public void EndDance() => this.scene.PlayBgm();

  public void AllUnitHide()
  {
    if (this.cameraController.IsLookuped)
    {
      if (Object.op_Inequality((Object) this.nowLookupedUnit, (Object) null))
        this.cameraController.ResetLookupUnit(this.nowLookupedUnit.UnitTransform);
      this.nowLookupedUnit = (SeaHomeUnitController) null;
    }
    if (this.cameraController.SetMode(SeaHomeCameraController.CameraMode.NORMAL))
      this.cameraController.Reset();
    foreach (SeaHomeUnitController unitController in this.unitControllers)
      unitController.Hide();
  }

  public void AllUnitShow()
  {
    foreach (SeaHomeUnitController unitController in this.unitControllers)
      unitController.Show();
  }

  public void SetCameraAutoFocus(bool enable) => this.cameraController.IsAutoFocus = enable;

  public void UpdateUnitData(PlayerUnit playeruUnit)
  {
    foreach (SeaHomeUnitController unitController in this.unitControllers)
    {
      if (unitController.UnitData.PlayerUnit != (PlayerUnit) null && unitController.UnitData.PlayerUnit.id == playeruUnit.id)
      {
        unitController.UpdatePlayerUnit(playeruUnit);
        break;
      }
    }
  }

  public class UnitConrtolleData
  {
    public int UnitID;
    public PlayerUnit PlayerUnit;

    public UnitConrtolleData(int id, PlayerUnit playerUnit)
    {
      this.PlayerUnit = playerUnit;
      this.UnitID = id;
    }

    public UnitUnit Unit
    {
      get
      {
        return MasterData.UnitUnit.ContainsKey(this.UnitID) ? MasterData.UnitUnit[this.UnitID] : (UnitUnit) null;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is SeaHomeManager.UnitConrtolleData))
        return false;
      SeaHomeManager.UnitConrtolleData unitConrtolleData = obj as SeaHomeManager.UnitConrtolleData;
      if (unitConrtolleData.UnitID != this.UnitID)
        return false;
      if (unitConrtolleData.PlayerUnit == (PlayerUnit) null && this.PlayerUnit == (PlayerUnit) null)
        return true;
      return unitConrtolleData.PlayerUnit != (PlayerUnit) null && this.PlayerUnit != (PlayerUnit) null && unitConrtolleData.PlayerUnit.id == this.PlayerUnit.id;
    }
  }
}
