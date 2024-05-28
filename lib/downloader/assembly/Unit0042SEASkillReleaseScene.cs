// Decompiled with JetBrains decompiler
// Type: Unit0042SEASkillReleaseScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Unit0042SEASkillReleaseScene : NGSceneBase
{
  private static string DefName = "unit004_2_SEASkillRelease";
  [SerializeField]
  private Unit0042SEASkillReleaseMenu menu;
  [SerializeField]
  private GameObject BgObject;
  private int orgDeapth;
  private bool advView;
  private bool popupView;
  private UnitSEASkill unitSkill;

  public static void changeScene(bool bStack, int unitId, int sameCharacterId)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    Unit0043Scene sceneBase = instance.sceneBase as Unit0043Scene;
    if (Object.op_Implicit((Object) sceneBase))
      instance.changeScene(Unit0042SEASkillReleaseScene.DefName, (bStack ? 1 : 0) != 0, (object) unitId, (object) sameCharacterId, (object) sceneBase.BgmFile, (object) sceneBase.BgmName);
    else
      instance.changeScene(Unit0042SEASkillReleaseScene.DefName, (bStack ? 1 : 0) != 0, (object) unitId, (object) sameCharacterId);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Unit0042SEASkillReleaseScene skillReleaseScene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bgF.Result.Clone(skillReleaseScene.BgObject.transform);
    skillReleaseScene.bgmFile = Singleton<NGSoundManager>.GetInstance().GetCueName();
    skillReleaseScene.bgmName = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    yield return (object) null;
  }

  public IEnumerator onStartSceneAsync(
    int unitId,
    int sameCharacterId,
    string bgm_file,
    string bgm_name)
  {
    Unit0042SEASkillReleaseScene skillReleaseScene = this;
    skillReleaseScene.bgmFile = bgm_file;
    skillReleaseScene.bgmName = bgm_name;
    IEnumerator e = skillReleaseScene.onStartSceneAsync(unitId, sameCharacterId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int unitId, int sameCharacterId)
  {
    if (this.unitSkill == null)
    {
      this.unitSkill = ((IEnumerable<UnitSEASkill>) MasterData.UnitSEASkillList).FirstOrDefault<UnitSEASkill>((Func<UnitSEASkill, bool>) (x => x.ID == sameCharacterId));
      IEnumerator e = MasterData.LoadScriptScript(this.unitSkill.script_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (MasterData.ScriptScript != null && MasterData.ScriptScript.ContainsKey(this.unitSkill.script_id))
        this.advView = true;
      if (this.unitSkill.skill_1_BattleskillSkill.HasValue)
      {
        this.popupView = true;
        yield return (object) this.menu.LoadPrefab(unitId, this.unitSkill.skill_1_BattleskillSkill.Value);
      }
    }
  }

  public void onStartScene(int unitId, int sameCharacterId)
  {
    if (this.advView)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
      this.advView = false;
      Story0093Scene.changeScene009_3(true, this.unitSkill.script_id, false);
    }
    else if (!this.advView && !this.popupView)
    {
      Singleton<NGSceneManager>.GetInstance().backScene();
    }
    else
    {
      if (!this.popupView)
        return;
      this.BgObject.SetActive(true);
      this.popupView = false;
      this.StartCoroutine(this.menu.CreatePopup());
    }
  }

  public void onStartScene(int sameCharacterId, string bgm_file, string bgm_name)
  {
  }

  public void onStartScene(int sameCharacterId)
  {
  }

  public override void onEndScene()
  {
  }
}
