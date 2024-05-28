// Decompiled with JetBrains decompiler
// Type: Unit0042Scene
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
using System.Text.RegularExpressions;
using UnitDetails;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042Scene")]
public class Unit0042Scene : NGSceneBase
{
  public Unit0042Menu menu;
  protected bool init;
  private long? versionUnitList;

  public Unit0042Scene.BootParam bootParam { get; private set; }

  public bool isNotExistUnits { get; private set; }

  private static bool IsSeaScene()
  {
    return Singleton<NGGameDataManager>.GetInstance().IsSea || Singleton<NGSceneManager>.GetInstance().IsSeaByChangeScene();
  }

  private static void callChangeScene(
    bool stack,
    string player_id,
    Unit0042Scene.BootParam param,
    bool bUnitTerminal = false)
  {
    string sceneName = !param.playerUnit.unit.IsNormalUnit ? "unit004_2_material" : Unit0042Scene.getSceneName(stack, bUnitTerminal);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) player_id, (object) param.playerUnit.id, (object) param);
  }

  private static string getSceneName(bool bStack, bool bUnitTerminal = false)
  {
    NGSceneManager instance1 = Singleton<NGSceneManager>.GetInstance();
    NGGameDataManager instance2 = Singleton<NGGameDataManager>.GetInstance();
    if (!bUnitTerminal)
    {
      string pattern = instance2.IsSea ? "unit004_2_sea" : "unit004_2";
      if (bStack)
        bUnitTerminal = Regex.IsMatch(instance1.sceneName, pattern);
      if (!bUnitTerminal)
        bUnitTerminal = instance1.isMatchSceneNameInStack(pattern);
    }
    string sceneName = !bUnitTerminal ? "unit004_2" : "unit004_2_terminal";
    if (instance2.IsSea)
      sceneName += "_sea";
    return sceneName;
  }

  public static void changeScene(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool IsMaterial = false,
    bool IsMemory = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.isMaterial = IsMaterial;
    bootParam.isMemory = IsMemory;
    if (!IsMemory && playerUnit.unit.IsNormalUnit)
    {
      bootParam.setControl(Control.OverkillersBase | Control.OverkillersSlot);
      if (playerUnit.player_id == Player.Current.id)
        bootParam.setControl(Control.OverkillersEdit | Control.OverkillersMove);
    }
    Unit0042Scene.callChangeScene(stack, "", bootParam);
  }

  public static void changeSceneCustomDeck(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.setControl(Control.CustomDeck);
    Unit0042Scene.callChangeScene(stack, "", bootParam);
  }

  public static void changeTerminal(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool IsMemory = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.isMemory = IsMemory;
    if (!IsMemory && playerUnit.unit.IsNormalUnit)
      bootParam.setControl(Control.OverkillersBase | Control.OverkillersSlot);
    Unit0042Scene.callChangeScene(stack, "", bootParam, true);
  }

  public static void changeSceneSelf(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool IsMemory = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.isMemory = IsMemory;
    if (!IsMemory && playerUnit.unit.IsNormalUnit)
      bootParam.setControl(Control.OverkillersSlot | Control.SelfAbility);
    Unit0042Scene.callChangeScene(stack, "", bootParam);
  }

  public static void changeSceneGuestUnit(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.isGuest = true;
    bootParam.limited = true;
    string sceneName = Unit0042Scene.getSceneName(stack);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) "", (object) playerUnit.id, (object) bootParam);
  }

  public static void changeSceneFriendUnit(bool stack, string player_id, int targetPlayerUnitId)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = (PlayerUnit) null;
    bootParam.playerUnits = (PlayerUnit[]) null;
    bootParam.limited = true;
    bootParam.setControl(Control.OverkillersSlot);
    string sceneName = Unit0042Scene.getSceneName(stack);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) player_id, (object) targetPlayerUnitId, (object) bootParam);
  }

  public static void changeSceneFriendUnit(bool stack, string player_id, PlayerUnit playerUnit)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = (PlayerUnit[]) null;
    bootParam.limited = true;
    bootParam.setControl(Control.OverkillersSlot);
    string sceneName = Unit0042Scene.getSceneName(stack);
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) player_id, (object) playerUnit.id, (object) bootParam);
  }

  public static void changeSceneEvolutionUnit(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool IsMaterial = false,
    bool IsMemory = false,
    bool IsToutaPlusNoEnable = false,
    bool IsBuguLock = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.limited = true;
    bootParam.isMaterial = IsMaterial;
    bootParam.isMemory = IsMemory;
    bootParam.isBuguLock = IsBuguLock;
    bootParam.setControl(Control.OverkillersSlot | Control.SelfAbility);
    bootParam.isToutaPlusNoEnable = IsToutaPlusNoEnable;
    Unit0042Scene.callChangeScene(stack, "", bootParam);
  }

  public static void changeSceneReincarnationTypeUnit(
    bool stack,
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool IsMaterial = false,
    bool IsMemory = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.limited = true;
    bootParam.isMaterial = IsMaterial;
    bootParam.isMemory = IsMemory;
    bootParam.isReincarnationType = true;
    bootParam.setControl(Control.OverkillersSlot | Control.SelfAbility);
    Unit0042Scene.callChangeScene(stack, "", bootParam, true);
  }

  public static void changeSceneMyGvgAtkDeck(
    bool stack,
    PlayerUnit playerUnit,
    bool IsMaterial = false,
    bool IsMemory = false)
  {
    Unit0042Scene.changeSceneMyGvgDeck(stack, new Unit0042Scene.BootParam()
    {
      isMyGvgAtkDeck = true
    }, playerUnit, IsMaterial, IsMemory);
  }

  public static void changeSceneMyGvgDefDeck(
    bool stack,
    PlayerUnit playerUnit,
    bool IsMaterial = false,
    bool IsMemory = false)
  {
    Unit0042Scene.changeSceneMyGvgDeck(stack, new Unit0042Scene.BootParam()
    {
      isMyGvgDefDeck = true
    }, playerUnit, IsMaterial, IsMemory);
  }

  public static void changeSceneMyGvgDeck(
    bool stack,
    Unit0042Scene.BootParam param,
    PlayerUnit playerUnit,
    bool IsMaterial = false,
    bool IsMemory = false)
  {
    param.playerUnit = playerUnit;
    param.playerUnits = (PlayerUnit[]) null;
    param.isMaterial = IsMaterial;
    param.isMemory = IsMemory;
    if (!IsMemory && playerUnit.unit.IsNormalUnit)
    {
      param.setControl(Control.OverkillersBase | Control.OverkillersSlot);
      if (playerUnit.player_id == Player.Current.id)
        param.setControl(Control.OverkillersEdit | Control.OverkillersMove);
    }
    Unit0042Scene.callChangeScene(stack, "", param);
  }

  public static void changeSceneGvgUnit(
    bool stack,
    PlayerUnit unit,
    PlayerUnit[] playerUnits,
    bool bNotUpdate = false)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = unit;
    bootParam.playerUnits = playerUnits;
    bootParam.limited = true;
    bootParam.setControl(Control.OverkillersBase | Control.OverkillersSlot);
    bootParam.isNotUpdate = bNotUpdate;
    string sceneName = "unit004_2";
    if (Unit0042Scene.IsSeaScene())
      sceneName = "unit004_2_sea";
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, (stack ? 1 : 0) != 0, (object) "", (object) unit.id, (object) bootParam);
  }

  public static void changeSceneMypageEdit(PlayerUnit playerUnit, PlayerUnit[] playerUnits)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = playerUnit;
    bootParam.playerUnits = playerUnits;
    bootParam.limited = true;
    bootParam.setControl(Control.OverkillersBase | Control.OverkillersSlot);
    Unit0042Scene.callChangeScene(true, "", bootParam);
  }

  public static void changeOverkillersUnitDetail(
    PlayerUnit baseUnit,
    PlayerUnit targetUnit,
    PlayerUnit[] units = null)
  {
    Unit0042Scene.BootParam bootParam = new Unit0042Scene.BootParam();
    bootParam.playerUnit = targetUnit;
    bootParam.playerUnits = units;
    bootParam.baseUnit = baseUnit;
    bootParam.limited = true;
    bootParam.setControl(Control.OverkillersUnit);
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_UnitEquip_Detail", true, (object) "", (object) targetUnit.id, (object) bootParam);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit0042Scene unit0042Scene = this;
    ((Component) unit0042Scene).GetComponent<UIRoot>().scalingStyle = (UIRoot.Scaling) 1;
    Future<GameObject> bgF = (Future<GameObject>) null;
    bgF = !PerformanceConfig.GetInstance().IsSpeedPriority ? new ResourceObject("Prefabs/BackGround/UnitBackground_anim").Load<GameObject>() : new ResourceObject("Prefabs/BackGround/UnitBackground_anim_no_effect").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Scene.backgroundPrefab = bgF.Result;
    if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      unit0042Scene.bgmFile = TowerUtil.BgmFile;
      unit0042Scene.bgmName = TowerUtil.BgmName;
    }
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      e = unit0042Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(
    string player_id,
    int targetPlayerUnitId,
    Unit0042Scene.SavedBootParam param)
  {
    param.checkCreateDummyUnits();
    yield return (object) this.onStartSceneAsync(player_id, targetPlayerUnitId, (Unit0042Scene.BootParam) param);
  }

  public IEnumerator onStartSceneAsync(
    string player_id,
    int targetPlayerUnitId,
    Unit0042Scene.BootParam param)
  {
    Unit0042Scene unit0042Scene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (Singleton<NGGameDataManager>.GetInstance().isEditCustomDeck)
      param.setControl(Control.CustomDeck);
    unit0042Scene.setBootParam(param);
    yield return (object) null;
    yield return (object) null;
    PlayerUnit[] unitList;
    IEnumerator e;
    if (!param.isGuest)
    {
      if (!string.IsNullOrEmpty(player_id))
      {
        if (param.playerUnit == (PlayerUnit) null || param.controlFlags.IsOn(Control.OverkillersSlot) && param.playerUnit.isAnyOverkillersUnits && param.playerUnit.cache_overkillers_units == null)
        {
          if (!unit0042Scene.init)
          {
            Unit0042Scene.FriendInfo friendInfo;
            PlayerUnit[] leaderUnitOverKillers;
            if (Singleton<NGGameDataManager>.GetInstance().IsSea)
            {
              // ISSUE: reference to a compiler-generated method
              Future<WebAPI.Response.SeaFriendStatus> futureF = WebAPI.SeaFriendStatus(player_id, targetPlayerUnitId, new Action<WebAPI.Response.UserError>(unit0042Scene.\u003ConStartSceneAsync\u003Eb__33_0));
              e = futureF.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              WebAPI.Response.SeaFriendStatus result = futureF.Result;
              if (result == null)
              {
                yield break;
              }
              else
              {
                friendInfo = new Unit0042Scene.FriendInfo(result);
                leaderUnitOverKillers = result.target_leader_unit_over_killers;
                futureF = (Future<WebAPI.Response.SeaFriendStatus>) null;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              Future<WebAPI.Response.FriendStatus> futureF = WebAPI.FriendStatus(player_id, targetPlayerUnitId, new Action<WebAPI.Response.UserError>(unit0042Scene.\u003ConStartSceneAsync\u003Eb__33_1));
              e = futureF.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              WebAPI.Response.FriendStatus result = futureF.Result;
              if (result == null)
              {
                yield break;
              }
              else
              {
                friendInfo = new Unit0042Scene.FriendInfo(result);
                leaderUnitOverKillers = result.target_leader_unit_over_killers;
                futureF = (Future<WebAPI.Response.FriendStatus>) null;
              }
            }
            if (param.playerUnit == (PlayerUnit) null)
            {
              param.playerUnit = friendInfo.target_leader_unit;
              param.playerUnit.primary_equipped_gear = param.playerUnit.FindEquippedGear(friendInfo.target_player_items);
              param.playerUnit.primary_equipped_gear2 = param.playerUnit.FindEquippedGear2(friendInfo.target_player_items);
              param.playerUnit.primary_equipped_gear3 = param.playerUnit.FindEquippedGear3(friendInfo.target_player_items);
              param.playerUnit.primary_equipped_reisou = param.playerUnit.FindEquippedReisou(friendInfo.target_player_items, friendInfo.target_player_reisou_items);
              param.playerUnit.primary_equipped_reisou2 = param.playerUnit.FindEquippedReisou2(friendInfo.target_player_items, friendInfo.target_player_reisou_items);
              param.playerUnit.primary_equipped_reisou3 = param.playerUnit.FindEquippedReisou3(friendInfo.target_player_items, friendInfo.target_player_reisou_items);
              PlayerAwakeSkill playerAwakeSkill = (PlayerAwakeSkill) null;
              foreach (int? equipAwakeSkillId in param.playerUnit.equip_awake_skill_ids)
              {
                int? awakeSkillID = equipAwakeSkillId;
                playerAwakeSkill = ((IEnumerable<PlayerAwakeSkill>) friendInfo.target_player_awake_skills).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x =>
                {
                  int id = x.id;
                  int? nullable = awakeSkillID;
                  int valueOrDefault = nullable.GetValueOrDefault();
                  return id == valueOrDefault & nullable.HasValue;
                }));
              }
              param.playerUnit.primary_equipped_awake_skill = playerAwakeSkill;
              param.playerUnit.usedPrimary = PlayerUnit.UsedPrimary.All;
            }
            else
            {
              param.playerUnit.importOverkillersUnits(leaderUnitOverKillers);
              param.playerUnit.resetOverkillersParameter();
            }
            unitList = new PlayerUnit[1]{ param.playerUnit };
            e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) unitList, false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = unit0042Scene.menu.Init(param.playerUnit, unitList, true, param.limited, false, false, param.isMaterial, param.playerUnit.primary_equipped_gear, param.playerUnit.primary_equipped_gear2, param.playerUnit.primary_equipped_gear3, param.isMemory);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            unit0042Scene.init = true;
            unitList = (PlayerUnit[]) null;
          }
        }
        else if (!unit0042Scene.init)
        {
          unitList = new PlayerUnit[1]{ param.playerUnit };
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) unitList, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = unit0042Scene.menu.Init(param.playerUnit, unitList, true, param.limited, false, false, param.isMaterial, param.playerUnit.primary_equipped_gear, param.playerUnit.primary_equipped_gear2, param.playerUnit.primary_equipped_gear3, param.isMemory);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          unit0042Scene.init = true;
          unitList = (PlayerUnit[]) null;
        }
      }
      else
      {
        unitList = (PlayerUnit[]) null;
        if (param.isMyGvgAtkDeck || param.isMyGvgDefDeck)
        {
          param.playerUnits = ((IEnumerable<PlayerUnit>) (param.isMyGvgAtkDeck ? GuildUtil.gvgDeckAttack : GuildUtil.gvgDeckDefense).player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
          unitList = param.playerUnits = unit0042Scene.filterNormalOrMaterial(param.playerUnit.unit.IsNormalUnit, param.playerUnits);
        }
        else if (param.playerUnits == null)
          unitList = new PlayerUnit[1]{ param.playerUnit };
        else
          unitList = param.playerUnits = unit0042Scene.filterNormalOrMaterial(param.playerUnit.unit.IsNormalUnit, param.playerUnits);
        if (!unit0042Scene.init || !unit0042Scene.menu.CheckLength(unitList) || unit0042Scene.versionUnitList.HasValue && unit0042Scene.isModifiedPlayerUnits)
        {
          PlayerUnit centerUnit = unit0042Scene.init ? unit0042Scene.menu.CurrentUnit : param.playerUnit;
          if (param.controlFlags.IsOff(Control.CustomDeck))
          {
            int id = centerUnit.id;
            if (!param.isNotUpdate)
              centerUnit = unit0042Scene.updateUnits(ref unitList, centerUnit);
            unitList = ((IEnumerable<PlayerUnit>) unitList).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
            if (unitList.Length == 0)
            {
              unit0042Scene.endSceneErrorNotExistUnits();
              yield break;
            }
            else
            {
              if (centerUnit == (PlayerUnit) null || id != centerUnit.id)
              {
                unit0042Scene.menu.hideQuestMenu();
                unit0042Scene.cancelAutoOpenUnityToutaPopupCoroutine();
              }
              unit0042Scene.menu.scrollView.Clear();
              if (param.playerUnits != null)
                param.playerUnits = unitList;
              if (centerUnit == (PlayerUnit) null)
                centerUnit = ((IEnumerable<PlayerUnit>) unitList).First<PlayerUnit>();
              param.playerUnit = centerUnit;
            }
          }
          else
            unit0042Scene.menu.scrollView.Clear();
          unit0042Scene.versionUnitList = new long?(SMManager.Revision<PlayerUnit[]>());
          e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) unitList, false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = unit0042Scene.menu.Init(centerUnit, unitList, false, param.limited, false, false, param.isMaterial, isMemory: param.isMemory, baseUnit: param.baseUnit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          unit0042Scene.init = true;
          centerUnit = (PlayerUnit) null;
        }
        else
        {
          List<PlayerUnit> source = new List<PlayerUnit>();
          if (param.controlFlags.IsOff(Control.CustomDeck | Control.NotUpdate))
          {
            PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
            foreach (PlayerUnit playerUnit1 in unitList)
            {
              PlayerUnit x = playerUnit1;
              if (x.unit.IsNormalUnit)
              {
                PlayerUnit playerUnit2 = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (y => y.id == x.id));
                if (playerUnit2 != (PlayerUnit) null)
                  source.Add(playerUnit2);
              }
              else
                source.Add(x);
            }
          }
          if (source.Any<PlayerUnit>() && !param.isReincarnationType)
          {
            e = unit0042Scene.menu.UpdateAllPage(source.ToArray());
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            e = unit0042Scene.menu.UpdateAllPage(((IEnumerable<PlayerUnit>) unitList).ToArray<PlayerUnit>());
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        unitList = (PlayerUnit[]) null;
      }
    }
    else if (!unit0042Scene.init)
    {
      unitList = (PlayerUnit[]) null;
      if (param.playerUnits == null)
        unitList = new PlayerUnit[1]{ param.playerUnit };
      else
        unitList = param.playerUnits = unit0042Scene.filterNormalOrMaterial(param.playerUnit.unit.IsNormalUnit, param.playerUnits);
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) unitList, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0042Scene.menu.scrollView.Clear();
      e = unit0042Scene.menu.Init(param.playerUnit, unitList, false, param.limited, false, false, param.isMaterial, isMemory: param.isMemory);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0042Scene.init = true;
      unitList = (PlayerUnit[]) null;
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    unit0042Scene.AutoOpenUnityToutaPopupCoroutine();
    GameObject currentScrollObject = unit0042Scene.menu.CurrentScrollObject;
    if (Object.op_Inequality((Object) currentScrollObject, (Object) null))
    {
      UITweener[] tweeners = NGTween.findTweeners(currentScrollObject, true);
      NGTween.playTweens(tweeners, NGTween.Kind.START_END);
      NGTween.playTweens(tweeners, NGTween.Kind.START);
    }
  }

  private void setBootParam(Unit0042Scene.BootParam param)
  {
    if (this.menu.IsTerminal)
    {
      param.clearControl(Control.OverkillersEdit);
      param.setControl(Control.Limited);
    }
    this.bootParam = param;
  }

  private PlayerUnit updateUnits(ref PlayerUnit[] units, PlayerUnit current)
  {
    if (!this.versionUnitList.HasValue || !this.isModifiedPlayerUnits)
      return current;
    PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
    int num = -1;
    int index1 = -1;
    int id = current.id;
    for (int index2 = 0; index2 < units.Length; ++index2)
    {
      int unitId = units[index2].id;
      units[index2] = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (x => x.id == unitId));
      if (units[index2] != (PlayerUnit) null)
        num = index2;
      if (unitId == id)
        index1 = num;
    }
    current = index1 < 0 ? (PlayerUnit) null : units[index1];
    return current;
  }

  private bool isModifiedPlayerUnits
  {
    get
    {
      long num = SMManager.Revision<PlayerUnit[]>();
      long? versionUnitList = this.versionUnitList;
      long valueOrDefault = versionUnitList.GetValueOrDefault();
      return !(num == valueOrDefault & versionUnitList.HasValue);
    }
  }

  public IEnumerator onStartSceneAsync(Unit0042Scene.SavedBootParam param)
  {
    param.checkCreateDummyUnits();
    yield return (object) this.onStartSceneAsync((Unit0042Scene.BootParam) param);
  }

  public IEnumerator onStartSceneAsync(Unit0042Scene.BootParam param)
  {
    this.setBootParam(param);
    if (!this.init)
    {
      PlayerUnit[] units = param.playerUnits = this.filterNormalOrMaterial(param.playerUnit.unit.IsNormalUnit, param.playerUnits);
      IEnumerator e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<PlayerUnit>) units, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.menu.Init(param.playerUnit, units, false, param.limited, true, false, param.isMaterial, param.playerUnit.primary_equipped_gear, param.playerUnit.primary_equipped_gear2, param.playerUnit.primary_equipped_gear3, param.isMemory);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.init = true;
      units = (PlayerUnit[]) null;
    }
    this.AutoOpenUnityToutaPopupCoroutine();
  }

  private PlayerUnit[] filterNormalOrMaterial(bool fillNormal, PlayerUnit[] units)
  {
    return units == null || units.Length == 0 ? units : (fillNormal ? ((IEnumerable<PlayerUnit>) units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.IsNormalUnit)) : ((IEnumerable<PlayerUnit>) units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && !x.unit.IsNormalUnit))).ToArray<PlayerUnit>();
  }

  public override void onEndScene()
  {
    if (this.tweens != null)
    {
      foreach (UITweener tween in this.tweens)
      {
        if (Object.op_Inequality((Object) tween, (Object) null))
          ((Behaviour) tween).enabled = false;
      }
      this.tweenTimeoutTime = 10f;
    }
    this.menu.onEndScene();
  }

  public override IEnumerator onEndSceneAsync()
  {
    Unit0042Scene receiver = this;
    if (Singleton<NGGameDataManager>.GetInstance().IsFromPopupStageList)
      receiver.init = false;
    IEnumerator e = receiver.menu.onEndSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receiver.tweens != null)
    {
      foreach (UITweener tween in receiver.tweens)
      {
        if (Object.op_Inequality((Object) tween, (Object) null))
          ((Behaviour) tween).enabled = true;
      }
      NGTween.setOnTweenFinished(receiver.tweens, (MonoBehaviour) receiver, "oneFrameWait");
    }
    else
      receiver.isTweenFinished = true;
  }

  protected void oneFrameWait() => this.StartCoroutine(this._oneFrameWait());

  protected IEnumerator _oneFrameWait()
  {
    Unit0042Scene unit0042Scene = this;
    yield return (object) null;
    yield return (object) null;
    yield return (object) null;
    yield return (object) null;
    unit0042Scene.isTweenFinished = true;
  }

  private void errorCallback(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    WebAPI.DefaultUserErrorCallback(error);
    this.menu.IbtnBack();
  }

  private IEnumerator SetSeaBgm()
  {
    Unit0042Scene unit0042Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit0042Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit0042Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  public void onStartScene(
    string player_id,
    int targetPlayerUnitId,
    Unit0042Scene.SavedBootParam param)
  {
    this.onStartScene(player_id, targetPlayerUnitId, (Unit0042Scene.BootParam) param);
  }

  public void onStartScene(string player_id, int targetPlayerUnitId, Unit0042Scene.BootParam param)
  {
    if (this.isNotExistUnits)
    {
      this.StartCoroutine(this.doWaitEndSceneErrorNotExistUnits());
    }
    else
    {
      this.endLoad();
      if (param.playerUnit == (PlayerUnit) null || param.playerUnit.is_storage)
        return;
      Singleton<TutorialRoot>.GetInstance().ShowAdvice("unit004_2_player_unit");
    }
  }

  public void onStartScene(Unit0042Scene.SavedBootParam param)
  {
    this.onStartScene((Unit0042Scene.BootParam) param);
  }

  public void onStartScene(Unit0042Scene.BootParam param) => this.endLoad();

  public void endLoad()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    GameObject currentScrollObject = this.menu.CurrentScrollObject;
    if (!Object.op_Inequality((Object) currentScrollObject, (Object) null))
      return;
    UITweener[] tweeners = NGTween.findTweeners(currentScrollObject, true);
    NGTween.playTweens(tweeners, NGTween.Kind.START_END);
    NGTween.playTweens(tweeners, NGTween.Kind.START);
  }

  private void OnDestroy()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    instance.resetOverkillersSlotReleaseConditions();
  }

  private void AutoOpenUnityToutaPopupCoroutine()
  {
    if (this.menu.IsTerminal)
      return;
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    switch (instance.fromPopup)
    {
      case NGGameDataManager.FromPopup.Unit0042SceneUnity:
        instance.OnceOpenPopup<Future<GameObject>>(this.menu.unityDetailPrefabs, (NGMenuBase) this.menu, new Action(this.menu.preOpenUnityPopup));
        break;
      case NGGameDataManager.FromPopup.Unit0042SceneRecommend:
        instance.OnceOpenPopup<GameObject>(this.menu.recommendPrefabs, (NGMenuBase) this.menu, new Action(this.menu.preOpenRecomendPopup));
        break;
      case NGGameDataManager.FromPopup.Unit004Combine:
        return;
      case NGGameDataManager.FromPopup.Quest002201Scene:
        return;
      case NGGameDataManager.FromPopup.Unit0042SceneCharacterQuest:
        instance.OnceOpenPopup<GameObject>((GameObject[]) null, (NGMenuBase) this.menu, new Action(this.menu.preOpenCharacterQuestPopup));
        break;
      default:
        return;
    }
    instance.fromPopup = NGGameDataManager.FromPopup.None;
  }

  private void endSceneErrorNotExistUnits()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.isNotExistUnits = true;
    this.cancelAutoOpenUnityToutaPopupCoroutine();
  }

  private void cancelAutoOpenUnityToutaPopupCoroutine()
  {
    switch (Singleton<NGGameDataManager>.GetInstance().fromPopup)
    {
      case NGGameDataManager.FromPopup.Unit0042SceneUnity:
      case NGGameDataManager.FromPopup.Unit0042SceneRecommend:
      case NGGameDataManager.FromPopup.Unit0042SceneCharacterQuest:
        Singleton<NGGameDataManager>.GetInstance().clearScenePopupRecovery();
        break;
    }
  }

  private IEnumerator doWaitEndSceneErrorNotExistUnits()
  {
    yield return (object) null;
    yield return (object) null;
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = false;
    instance.isTouchBlock = false;
    instance.HideLoadingLayer();
    this.menu.IsPush = false;
    this.menu.IbtnBack();
  }

  public void entryMenu(NGMenuBase mb)
  {
    List<NGMenuBase> ngMenuBaseList = this.menuBases != null ? new List<NGMenuBase>((IEnumerable<NGMenuBase>) this.menuBases) : new List<NGMenuBase>();
    if (!ngMenuBaseList.Contains(mb))
      ngMenuBaseList.Add(mb);
    this.menuBases = ngMenuBaseList.ToArray();
  }

  public class BootParam
  {
    public virtual PlayerUnit playerUnit { get; set; }

    public virtual PlayerUnit[] playerUnits { get; set; }

    public Control controlFlags { get; protected set; }

    public virtual PlayerUnit baseUnit { get; set; }

    public bool limited
    {
      get => this.controlFlags.IsOn(Control.Limited);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.Limited) : this.controlFlags.Clear(Control.Limited);
      }
    }

    public bool isGuest
    {
      get => this.controlFlags.IsOn(Control.Guest);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.Guest) : this.controlFlags.Clear(Control.Guest);
      }
    }

    public bool isMemory
    {
      get => this.controlFlags.IsOn(Control.Memory);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.Memory) : this.controlFlags.Clear(Control.Memory);
      }
    }

    public bool isMaterial
    {
      get => this.controlFlags.IsOn(Control.Material);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.Material) : this.controlFlags.Clear(Control.Material);
      }
    }

    public bool isReincarnationType
    {
      get => this.controlFlags.IsOn(Control.ReincarnationType);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.ReincarnationType) : this.controlFlags.Clear(Control.ReincarnationType);
      }
    }

    public bool isToutaPlusNoEnable
    {
      get => this.controlFlags.IsOn(Control.ToutaPlusNoEnable);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.ToutaPlusNoEnable) : this.controlFlags.Clear(Control.ToutaPlusNoEnable);
      }
    }

    public bool isMyGvgAtkDeck
    {
      get => this.controlFlags.IsOn(Control.MyGvgAtkDeck);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.MyGvgAtkDeck) : this.controlFlags.Clear(Control.MyGvgAtkDeck);
      }
    }

    public bool isMyGvgDefDeck
    {
      get => this.controlFlags.IsOn(Control.MyGvgDefDeck);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.MyGvgDefDeck) : this.controlFlags.Clear(Control.MyGvgDefDeck);
      }
    }

    public bool isNotUpdate
    {
      get => this.controlFlags.IsOn(Control.NotUpdate);
      set
      {
        this.controlFlags = value ? this.controlFlags.Set(Control.NotUpdate) : this.controlFlags.Clear(Control.NotUpdate);
      }
    }

    public bool isBuguLock { get; set; }

    public void setControl(Control flags) => this.controlFlags = this.controlFlags.Set(flags);

    public void clearControl(Control flags) => this.controlFlags = this.controlFlags.Clear(flags);
  }

  public class SavedBootParam : Unit0042Scene.BootParam
  {
    private int playerUnitId_;
    private int[] playerUnitIds_;
    private int baseUnitId_;

    public SavedBootParam(Unit0042Scene.BootParam orignal)
    {
      this.playerUnit = orignal.playerUnit;
      this.playerUnits = orignal.playerUnits;
      this.controlFlags = orignal.controlFlags;
      this.baseUnit = orignal.baseUnit;
    }

    public override PlayerUnit playerUnit
    {
      get
      {
        if (base.playerUnit != (PlayerUnit) null)
          return base.playerUnit;
        return this.playerUnitId_ == 0 ? (PlayerUnit) null : (base.playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == this.playerUnitId_)));
      }
      set
      {
        base.playerUnit = (PlayerUnit) null;
        this.playerUnitId_ = value != (PlayerUnit) null ? value.id : 0;
      }
    }

    public override PlayerUnit[] playerUnits
    {
      get
      {
        if (base.playerUnits != null)
          return base.playerUnits;
        PlayerUnit[] units = SMManager.Get<PlayerUnit[]>();
        base.playerUnits = ((IEnumerable<int>) this.playerUnitIds_).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i => Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == i)))).ToArray<PlayerUnit>();
        this.playerUnitIds_ = (int[]) null;
        return base.playerUnits;
      }
      set
      {
        base.playerUnits = (PlayerUnit[]) null;
        this.playerUnitIds_ = value != null ? ((IEnumerable<PlayerUnit>) value).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>() : (int[]) null;
      }
    }

    public override PlayerUnit baseUnit
    {
      get
      {
        if (base.baseUnit != (PlayerUnit) null)
          return base.baseUnit;
        return this.baseUnitId_ == 0 ? (PlayerUnit) null : (base.baseUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == this.baseUnitId_)));
      }
      set
      {
        base.baseUnit = (PlayerUnit) null;
        this.baseUnitId_ = value != (PlayerUnit) null ? value.id : 0;
      }
    }

    public void checkCreateDummyUnits()
    {
      if (base.playerUnits != null || this.playerUnitIds_ != null && this.playerUnitIds_.Length != 0 || !(this.playerUnit != (PlayerUnit) null))
        return;
      base.playerUnits = new PlayerUnit[1]
      {
        this.playerUnit
      };
      this.playerUnitIds_ = (int[]) null;
    }
  }

  private class FriendInfo
  {
    public PlayerUnit target_leader_unit;
    public PlayerItem[] target_player_items;
    public PlayerAwakeSkill[] target_player_awake_skills;
    public PlayerGearReisouSchema[] target_player_reisou_items;

    public FriendInfo(WebAPI.Response.SeaFriendStatus info)
    {
      this.target_leader_unit = info.target_leader_unit;
      this.target_player_items = info.target_player_items;
      this.target_player_awake_skills = info.target_player_awake_skills;
      this.target_player_reisou_items = info.target_player_reisou_items;
      if (this.target_leader_unit == (PlayerUnit) null)
        return;
      this.target_leader_unit.importOverkillersUnits(info.target_leader_unit_over_killers);
      this.target_leader_unit.resetOverkillersParameter();
    }

    public FriendInfo(WebAPI.Response.FriendStatus info)
    {
      this.target_leader_unit = info.target_leader_unit;
      this.target_player_items = info.target_player_items;
      this.target_player_awake_skills = info.target_player_awake_skills;
      this.target_player_reisou_items = info.target_player_reisou_items;
      if (this.target_leader_unit == (PlayerUnit) null)
        return;
      this.target_leader_unit.importOverkillersUnits(info.target_leader_unit_over_killers);
      this.target_leader_unit.resetOverkillersParameter();
    }
  }
}
