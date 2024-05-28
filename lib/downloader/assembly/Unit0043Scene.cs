// Decompiled with JetBrains decompiler
// Type: Unit0043Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Unit0043Scene : NGSceneBase
{
  [SerializeField]
  private Unit0043Menu menu;

  public static void changeScene(
    bool stack,
    UnitUnit unit,
    int job_id,
    bool isSame,
    PlayerUnitSkills koyuDuelSkill = null,
    PlayerUnitSkills koyuMultiSkill = null,
    bool bLibrary = false)
  {
    MasterDataTable.UnitJob unitJob;
    MasterData.UnitJob.TryGetValue(job_id, out unitJob);
    PlayerUnit playerUnit = unit.character.category == UnitCategory.player ? PlayerUnit.FromUnit(unit, 0) : PlayerUnit.create_by_unitunit(unit);
    if (unitJob != null)
      playerUnit.job_id = unitJob.ID;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_3", (stack ? 1 : 0) != 0, (object) new Unit0043Scene.BootParam()
    {
      target = playerUnit,
      isSame = isSame,
      koyuDuelSkill = koyuDuelSkill,
      koyuMultiSkill = koyuMultiSkill,
      isLibrary = bLibrary
    });
  }

  public static void changeScene(bool stack, PlayerUnit unit, bool isSame)
  {
    Unit0043Scene.BootParam bootParam = new Unit0043Scene.BootParam();
    bootParam.target = unit;
    bootParam.isSame = isSame;
    if (unit.player_id == Player.Current.id)
    {
      UnitParameter.SkillSortUnit[] sortedSkills = new UnitParameter(new BL.Unit()
      {
        playerUnit = unit
      }).sortedSkills;
      for (int index = 0; index < sortedSkills.Length; ++index)
      {
        UnitParameter.SkillSortUnit skillSortUnit = sortedSkills[index];
        if (skillSortUnit.group == UnitParameter.SkillGroup.Duel && skillSortUnit.duelSkill.skill.haveKoyuDuelEffect)
        {
          bootParam.koyuDuelSkill = skillSortUnit.duelSkill;
          break;
        }
      }
      for (int index = 0; index < sortedSkills.Length; ++index)
      {
        UnitParameter.SkillSortUnit skillSortUnit = sortedSkills[index];
        if (skillSortUnit.group == UnitParameter.SkillGroup.Multi && skillSortUnit.multiSkill.skill.haveKoyuDuelEffect)
        {
          bootParam.koyuMultiSkill = skillSortUnit.multiSkill;
          break;
        }
      }
    }
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_3", (stack ? 1 : 0) != 0, (object) bootParam);
  }

  public string BgmName => this.bgmName;

  public string BgmFile => this.bgmFile;

  public override IEnumerator onInitSceneAsync()
  {
    Unit0043Scene unit0043Scene = this;
    if (Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      unit0043Scene.bgmFile = TowerUtil.BgmFile;
      unit0043Scene.bgmName = TowerUtil.BgmName;
    }
    else if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      IEnumerator e = unit0043Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (Object.op_Equality((Object) unit0043Scene.backgroundPrefab, (Object) null))
    {
      Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_UnitViewer.Load<GameObject>();
      yield return (object) bgF.Wait();
      unit0043Scene.backgroundPrefab = bgF.Result;
      bgF = (Future<GameObject>) null;
    }
  }

  public IEnumerator onStartSceneAsync(Unit0043Scene.BootParam bp)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = this.menu.Init(bp);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(Unit0043Scene.BootParam bp)
  {
    this.StartCoroutine(this.doDelayHideLoadingLayer());
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    UnitVoicePattern unitVoicePattern = bp.target.unit.unitVoicePattern;
    if (!instance.IsEffectiveCueID(unitVoicePattern.file_name, 92))
      return;
    instance.playVoiceByID(unitVoicePattern, 92);
  }

  public IEnumerator onBackSceneAsync(Unit0043Scene.BootParam bp)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    yield return (object) this.menu.InitOnBackScene();
  }

  public void onBackScene(Unit0043Scene.BootParam bp)
  {
    this.StartCoroutine(this.doDelayHideLoadingLayer());
    this.menu.isChangedSceneDuelDemo = false;
  }

  private IEnumerator doDelayHideLoadingLayer()
  {
    yield return (object) new WaitForSeconds(0.5f);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onEndScene() => this.menu.onEndScene();

  public override IEnumerator onEndSceneAsync()
  {
    Unit0043Scene unit0043Scene = this;
    if (!unit0043Scene.menu.isChangedSceneDuelDemo)
    {
      float startTime = Time.time;
      while (!unit0043Scene.isTweenFinished && (double) Time.time - (double) startTime < (double) unit0043Scene.tweenTimeoutTime)
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      unit0043Scene.isTweenFinished = true;
      yield return (object) null;
      // ISSUE: reference to a compiler-generated method
      yield return (object) unit0043Scene.\u003C\u003En__0();
    }
  }

  private IEnumerator SetSeaBgm()
  {
    Unit0043Scene unit0043Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit0043Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit0043Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  public class BootParam
  {
    public PlayerUnit target { get; set; }

    public bool isSame { get; set; }

    public PlayerUnitSkills koyuDuelSkill { get; set; }

    public PlayerUnitSkills koyuMultiSkill { get; set; }

    public bool isLibrary { get; set; }
  }
}
