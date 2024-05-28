// Decompiled with JetBrains decompiler
// Type: LumpToutaResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/ResultMenu")]
public class LumpToutaResultMenu : MonoBehaviour
{
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIScrollBar scrollBar;
  private bool isBlockTouch;
  private Queue<IEnumerator> QueueOnTouchToNext = new Queue<IEnumerator>();
  private List<UnitResult> UnitResults = new List<UnitResult>();

  public IEnumerator Init(
    List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits,
    int incrementMedal,
    GainTrustResult[] gainTrustResults,
    UnlockQuest[] unlockQuests)
  {
    LumpToutaResultMenu lumpToutaResultMenu = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/unit004_LumpTouta/dir_Unit_Result").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitResultPrefab = prefabF.Result;
    prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject normalPrefab = prefabF.Result;
    if (resultPlayerUnits.Count != gainTrustResults.Length)
      Debug.LogError((object) "resultPlayerUnits.Count != gainTrustResults.Length Not Same Count!!");
    for (int i = 0; i < resultPlayerUnits.Count; ++i)
    {
      UnitResult unitResultScript = unitResultPrefab.CloneAndGetComponent<UnitResult>(((Component) lumpToutaResultMenu.grid).transform);
      yield return (object) unitResultScript.Init(normalPrefab, resultPlayerUnits[i]);
      lumpToutaResultMenu.UnitResults.Add(unitResultScript);
      unitResultScript = (UnitResult) null;
    }
    // ISSUE: method pointer
    lumpToutaResultMenu.grid.onReposition = new UIGrid.OnReposition((object) lumpToutaResultMenu, __methodptr(\u003CInit\u003Eb__6_0));
    lumpToutaResultMenu.grid.Reposition();
    lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.UnitResultUnityGaugeSkip());
    lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.UnitResultPageNext());
    for (int index = 0; index < resultPlayerUnits.Count; ++index)
    {
      if (gainTrustResults[index].has_new_player_awake_skill)
        lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.ExtraSKillRememberSequence(resultPlayerUnits[index]));
    }
    if (unlockQuests != null && unlockQuests.Length != 0)
      lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.CharacterQuestStoryPopup(unlockQuests));
    if (incrementMedal > 0)
      lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.RareMedalGetPopup(incrementMedal));
    lumpToutaResultMenu.QueueOnTouchToNext.Enqueue(lumpToutaResultMenu.changeSceneLumpTouta());
  }

  public void OnStartScene()
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.gaugeAnimation());
  }

  private IEnumerator gaugeAnimation()
  {
    List<GaugeRunner> gaugeRunnerList = new List<GaugeRunner>();
    foreach (UnitResult unitResult in this.UnitResults)
      gaugeRunnerList.Add(unitResult.GetGaugeAnime());
    yield return (object) GaugeRunner.Run(gaugeRunnerList.ToArray());
    ((Behaviour) this.scrollView).enabled = true;
    ((Behaviour) this.scrollBar).enabled = true;
  }

  public void OnTouchToNext()
  {
    if (this.isBlockTouch)
      return;
    this.StartCoroutine(this.QueueOnTouchToNext.Dequeue());
  }

  private IEnumerator UnitResultUnityGaugeSkip()
  {
    bool flag = false;
    foreach (UnitResult unitResult in this.UnitResults)
      flag |= unitResult.SkipUnityGauge();
    if (!flag)
      ((Behaviour) this.scrollView).enabled = true;
    ((Behaviour) this.scrollBar).enabled = true;
    yield break;
  }

  private IEnumerator UnitResultPageNext()
  {
    using (List<UnitResult>.Enumerator enumerator = this.UnitResults.GetEnumerator())
    {
      while (enumerator.MoveNext())
        enumerator.Current.ChangeUnityViewToDearView();
      yield break;
    }
  }

  private IEnumerator ExtraSKillRememberSequence(Unit004832Menu.ResultPlayerUnit resultPlayerUnit)
  {
    this.setBlockTouch(true);
    GameObject Prefab = (GameObject) null;
    Future<GameObject> prefabF;
    IEnumerator e;
    if (resultPlayerUnit.afterPlayerUnit.unit.IsSea)
    {
      prefabF = new ResourceObject("Animations/extraskill/FavorabilityRatingEffect").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
      prefabF = (Future<GameObject>) null;
    }
    else if (resultPlayerUnit.afterPlayerUnit.unit.IsResonanceUnit)
    {
      prefabF = new ResourceObject("Animations/common_gear_skill/CommonGearSkillEffect").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Prefab = prefabF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
      prefabF = (Future<GameObject>) null;
    }
    if (!Object.op_Equality((Object) Prefab, (Object) null))
    {
      bool isFinished = false;
      bool flag = (double) resultPlayerUnit.beforePlayerUnit.trust_rate < (double) Consts.GetInstance().TRUST_RATE_LEVEL_SIZE && (double) resultPlayerUnit.afterPlayerUnit.trust_rate >= (double) Consts.GetInstance().TRUST_RATE_LEVEL_SIZE;
      FavorabilityRatingEffect script = Prefab.GetComponent<FavorabilityRatingEffect>();
      e = script.Init(FavorabilityRatingEffect.AnimationType.SkillRelease, resultPlayerUnit.afterPlayerUnit, (Action) (() =>
      {
        Singleton<PopupManager>.GetInstance().dismiss();
        isFinished = true;
      }), !flag);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(Prefab, isCloned: true);
      script.StartEffect();
      while (!isFinished)
        yield return (object) null;
      this.setBlockTouch(false);
      this.OnTouchToNext();
    }
  }

  private IEnumerator CharacterQuestStoryPopup(UnlockQuest[] unlockQuests)
  {
    this.setBlockTouch(true);
    Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnlockQuest[] unlockQuestArray = unlockQuests;
    for (int index = 0; index < unlockQuestArray.Length; ++index)
    {
      QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
      Battle020112Menu o = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Battle020112Menu>();
      yield return (object) o.Init(quest);
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
        yield return (object) null;
      o = (Battle020112Menu) null;
    }
    unlockQuestArray = (UnlockQuest[]) null;
    this.setBlockTouch(false);
    this.OnTouchToNext();
  }

  private IEnumerator RareMedalGetPopup(int increment_medal)
  {
    this.setBlockTouch(true);
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_004_8_13__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabf.Result;
    Popup004813Menu component = Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Popup004813Menu>();
    component.SetText(increment_medal);
    bool isFinished = false;
    component.SetIbtnOKCallback((Action) (() => isFinished = true));
    while (!isFinished)
      yield return (object) null;
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta", false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_LumpTouta_Result");
  }

  private IEnumerator changeSceneLumpTouta()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_LumpTouta", false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_LumpTouta_Result");
    yield break;
  }

  private void setBlockTouch(bool flag)
  {
    this.isBlockTouch = flag;
    ((Behaviour) this.scrollView).enabled = !flag;
    ((Behaviour) this.scrollBar).enabled = !flag;
  }
}
