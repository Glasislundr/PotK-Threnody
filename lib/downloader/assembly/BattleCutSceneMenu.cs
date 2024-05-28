// Decompiled with JetBrains decompiler
// Type: BattleCutSceneMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleCutSceneMenu : BackButtonMenuBase
{
  [SerializeField]
  private Transform mRoot3d;
  [SerializeField]
  private GameObject mMainCameraObj;
  [SerializeField]
  private GameObject mSubCameraObj;
  [SerializeField]
  private GameObject mPlaySpeedButtonRootNormal;
  [SerializeField]
  private GameObject[] mPlaySpeedButtonsNormal;
  [SerializeField]
  private GameObject mPlaySpeedButtonRootSea;
  [SerializeField]
  private GameObject[] mPlaySpeedButtonsSea;
  private GameObject[] mPlaySpeedButtons;
  private CutSceneUnitModel mUnitModel;
  private int mPlaySpeed;
  private float mOrgPlaySpeed;
  private int mCommonRootOrgDepth;
  private IEnumerator mAnimationEffcet;

  public IEnumerator OnStartSceneAsync(BL.Unit unit, BL.Skill skill)
  {
    BattleCutSceneMenu battleCutSceneMenu = this;
    battleCutSceneMenu.IsPush = true;
    ((Behaviour) battleCutSceneMenu.mMainCameraObj.GetComponentInChildren<Camera>()).enabled = false;
    bool isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    battleCutSceneMenu.mPlaySpeedButtons = !isSea ? battleCutSceneMenu.mPlaySpeedButtonsNormal : battleCutSceneMenu.mPlaySpeedButtonsSea;
    battleCutSceneMenu.mPlaySpeedButtonRootNormal.SetActive(!isSea);
    battleCutSceneMenu.mPlaySpeedButtonRootSea.SetActive(isSea);
    battleCutSceneMenu.mOrgPlaySpeed = Time.timeScale;
    try
    {
      battleCutSceneMenu.mPlaySpeed = Math.Max(1, Persist.duel.Data.speed);
    }
    catch (Exception ex)
    {
      Persist.duel.Delete();
      Persist.duel.Data = new Persist.Duel();
      battleCutSceneMenu.mPlaySpeed = 1;
    }
    battleCutSceneMenu.SetPlaySpeed((float) battleCutSceneMenu.mPlaySpeed);
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    battleCutSceneMenu.mCommonRootOrgDepth = instance.setBlackBGPanelDepth(-1);
    instance.forceActiveBlackBGPanel(true);
    yield return (object) null;
    SkillMetamorphosis metamor = unit.metamorphosis;
    Future<GameObject> mp = unit.playerUnit.LoadModelDuel(metamor != null ? metamor.metamorphosis_id : 0);
    IEnumerator e = mp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitObj = mp.Result.Clone(battleCutSceneMenu.mRoot3d);
    string attach_node;
    Future<GameObject> unitEffect = unit.playerUnit.LoadModelUnitAuraEffect(out attach_node, metamor != null ? metamor.metamorphosis_id : 0);
    Transform effectPoint = !string.IsNullOrEmpty(attach_node) ? unitObj.transform.GetChildInFind(attach_node) : (Transform) null;
    if (Object.op_Equality((Object) effectPoint, (Object) null))
      effectPoint = unitObj.transform.GetChildInFind("Bip");
    if (Object.op_Equality((Object) effectPoint, (Object) null))
      effectPoint = unitObj.transform.GetChildInFind("bip");
    if (Object.op_Inequality((Object) effectPoint, (Object) null))
    {
      e = unitEffect.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = unitEffect.Result;
      if (Object.op_Inequality((Object) result, (Object) null))
        result.Clone(effectPoint);
    }
    e = unit.playerUnit.unit.ProcessAttachAwakeEffect(unitObj);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Object.Destroy((Object) unitObj.GetComponent<NGDuelUnit>());
    battleCutSceneMenu.mUnitModel = unitObj.AddComponent<CutSceneUnitModel>();
    e = battleCutSceneMenu.mUnitModel.Initialize(unit, skill, ((Component) battleCutSceneMenu.mRoot3d).gameObject, battleCutSceneMenu.mMainCameraObj, battleCutSceneMenu.mSubCameraObj);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void OnStartScene()
  {
    ((Behaviour) this.mMainCameraObj.GetComponentInChildren<Camera>()).enabled = true;
    this.mAnimationEffcet = this.ExecAnimation();
    this.StartCoroutine(this.mAnimationEffcet);
    this.IsPush = false;
  }

  public void OnEndScene()
  {
    if (Persist.duel.Data.speed != this.mPlaySpeed)
    {
      Persist.duel.Data.speed = this.mPlaySpeed;
      Persist.duel.Flush();
    }
    Time.timeScale = this.mOrgPlaySpeed;
    Singleton<CommonRoot>.GetInstance().setBlackBGPanelDepth(this.mCommonRootOrgDepth);
    ((Component) this.mRoot3d).gameObject.SetActive(false);
  }

  private IEnumerator ExecAnimation()
  {
    BattleCutSceneMenu battleCutSceneMenu = this;
    battleCutSceneMenu.mUnitModel.StartAnime();
    CommonRoot commonRoot = Singleton<CommonRoot>.GetInstance();
    yield return (object) new WaitForSeconds(0.5f);
    commonRoot.isActiveBlackBGPanel = false;
    yield return (object) new WaitForSeconds(0.3f);
    yield return (object) new WaitForSeconds(battleCutSceneMenu.mUnitModel.GetAnimeSpan());
    if (!battleCutSceneMenu.IsPushAndSet())
      battleCutSceneMenu.backScene();
  }

  private void SetPlaySpeed(float speed)
  {
    for (int index = 0; index < this.mPlaySpeedButtons.Length; ++index)
      this.mPlaySpeedButtons[index].SetActive((double) (index + 1) == (double) speed);
    Time.timeScale = speed;
  }

  public void Skip()
  {
    if (this.IsPushAndSet())
      return;
    if (this.mAnimationEffcet != null)
      this.StopCoroutine(this.mAnimationEffcet);
    this.backScene();
  }

  public void OnSpeedToggleButton()
  {
    if (++this.mPlaySpeed > 3)
      this.mPlaySpeed = 1;
    this.SetPlaySpeed((float) this.mPlaySpeed);
  }

  public override void onBackButton()
  {
    if (this.mAnimationEffcet == null)
      return;
    this.Skip();
  }
}
