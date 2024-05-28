// Decompiled with JetBrains decompiler
// Type: UnitUnityGrowthResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class UnitUnityGrowthResult : MonoBehaviour
{
  [SerializeField]
  private float GaugeDuration = 0.1f;
  [SerializeField]
  private float CountupToGaugeSpan = 0.3f;
  [Space(8f)]
  [SerializeField]
  private GameObject mIconAnchor;
  [Space(8f)]
  [SerializeField]
  private UILabel mTxtUnityTotal;
  [SerializeField]
  private UILabel mTxtUnity;
  [SerializeField]
  private UILabel mTxtBuildupUnity;
  [SerializeField]
  private UILabel mTxtBuildupUnityPer;
  [SerializeField]
  private UIWidget mBuildupUnityPerUpObj;
  [SerializeField]
  private UILabel mTxtBuildupUnityPerUp;
  [Space(8f)]
  [SerializeField]
  private LabelCountUpper mCountupper;
  [SerializeField]
  private GameObject mGaugeUp;
  private UnitIcon mUnitIcon;
  private GaugeRunner mGaugeAnime;
  private int mViewBuildupUnityValue;
  private int mGaugeMaxLoopCnt;
  private bool mIsGaugeWaiting;
  private bool mSkipDirty;

  public bool isAnime
  {
    get
    {
      if (this.mCountupper.isAnime || this.mIsGaugeWaiting)
        return true;
      return this.mGaugeAnime != null && this.mGaugeAnime.isRunning;
    }
  }

  public IEnumerator Initialize(PlayerUnit beforeUnit, PlayerUnit afterUnit)
  {
    UnitUnityGrowthResult unityGrowthResult = this;
    unityGrowthResult.mSkipDirty = false;
    unityGrowthResult.mTxtUnityTotal.SetTextLocalize(afterUnit.unityInt);
    unityGrowthResult.mTxtUnity.SetTextLocalize(beforeUnit.unity_value);
    unityGrowthResult.mCountupper.Initialize(beforeUnit.unity_value, afterUnit.unity_value);
    unityGrowthResult.mTxtBuildupUnityPer.SetTextLocalize(afterUnit.unityDec);
    ((UIRect) unityGrowthResult.mTxtBuildupUnityPer).alpha = 0.0f;
    float buildupUnityValueF = beforeUnit.buildup_unity_value_f;
    float after = afterUnit.buildup_unity_value_f;
    unityGrowthResult.mViewBuildupUnityValue = (int) Math.Floor((double) buildupUnityValueF);
    unityGrowthResult.mTxtBuildupUnity.SetTextLocalize(unityGrowthResult.mViewBuildupUnityValue);
    Decimal num1 = ((Decimal) after - (Decimal) buildupUnityValueF) * 100M;
    unityGrowthResult.mTxtBuildupUnityPerUp.SetTextLocalize((int) num1);
    ((UIRect) unityGrowthResult.mBuildupUnityPerUpObj).alpha = 0.0f;
    int num2 = (int) ((Decimal) buildupUnityValueF * 100M) % 100;
    int num3 = (int) ((Decimal) after * 100M) % 100;
    unityGrowthResult.mGaugeMaxLoopCnt = (int) Math.Floor((double) after) - (int) Math.Floor((double) buildupUnityValueF);
    unityGrowthResult.mGaugeAnime = new GaugeRunner(unityGrowthResult.mGaugeUp, (float) num2 / 100f, (float) num3 / 100f, unityGrowthResult.mGaugeMaxLoopCnt, new Func<GameObject, int, IEnumerator>(unityGrowthResult.OnBuildupUnityGaugeLooped), duration: unityGrowthResult.GaugeDuration);
    unityGrowthResult.mGaugeAnime.onFinishCallback = (Action) (() =>
    {
      this.mTxtBuildupUnity.SetTextLocalize((int) Math.Floor((double) after));
      TweenAlpha component = ((Component) this.mBuildupUnityPerUpObj).GetComponent<TweenAlpha>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ((Behaviour) component).enabled = true;
      else
        ((UIRect) this.mBuildupUnityPerUpObj).alpha = 1f;
      ((UIRect) this.mTxtBuildupUnityPer).alpha = 1f;
    });
    IEnumerator e = unityGrowthResult.SetupIcon(afterUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void StartGaugeAnime() => this.StartCoroutine(this.CountupUnity());

  private IEnumerator CountupUnity()
  {
    UnitUnityGrowthResult unityGrowthResult = this;
    unityGrowthResult.mCountupper.StartCountup();
    if (!unityGrowthResult.mSkipDirty && !unityGrowthResult.mCountupper.isFinishCount)
    {
      unityGrowthResult.mIsGaugeWaiting = true;
      yield return (object) new WaitForSeconds(unityGrowthResult.CountupToGaugeSpan);
    }
    unityGrowthResult.StartCoroutine(GaugeRunner.Run(unityGrowthResult.mGaugeAnime));
    unityGrowthResult.mIsGaugeWaiting = false;
    if (unityGrowthResult.mSkipDirty)
      unityGrowthResult.mGaugeAnime.Skip();
  }

  public void SkipAnime()
  {
    this.mSkipDirty = true;
    this.mCountupper.Skip();
    if (!this.mGaugeAnime.isRunning)
      return;
    this.mGaugeAnime.Skip();
  }

  private IEnumerator SetupIcon(PlayerUnit playerUnit)
  {
    Future<GameObject> ft = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/Sea/UnitIcon/normal_sea").Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon icon = ft.Result.CloneAndGetComponent<UnitIcon>(this.mIconAnchor.transform);
    e = icon.SetUnit(playerUnit, playerUnit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.BottomBaseObject = true;
    icon.BottomModeValue = UnitIconBase.BottomMode.Level;
    icon.setLevelText(playerUnit);
  }

  private IEnumerator OnBuildupUnityGaugeLooped(GameObject obj, int count)
  {
    ++this.mViewBuildupUnityValue;
    this.mTxtBuildupUnity.SetTextLocalize(this.mViewBuildupUnityValue);
    yield break;
  }
}
