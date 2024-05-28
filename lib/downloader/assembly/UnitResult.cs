// Decompiled with JetBrains decompiler
// Type: UnitResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UniLinq;
using UnityEngine;

#nullable disable
public class UnitResult : MonoBehaviour
{
  [SerializeField]
  private GameObject linkCharacter;
  [Header("上限突破")]
  [SerializeField]
  private UnitResultLimitBreak[] limitBreaks;
  [Header("共鳴率・好感度")]
  [SerializeField]
  private GameObject dirDearRoot;
  [SerializeField]
  private GameObject slcDearDegreeBase;
  [SerializeField]
  private UISprite slcTextDearDegree;
  [SerializeField]
  private NGxBlinkEx dirDearDegree;
  [SerializeField]
  private UILabel txtDearDegree;
  [SerializeField]
  private UILabel txtDearDegreeUpAmount;
  [SerializeField]
  private GameObject txtDearDegreeNone;
  [Header("淘汰値")]
  [SerializeField]
  private GameObject dirUnityRoot;
  [SerializeField]
  private NGxBlinkEx dirToutaBlink;
  [SerializeField]
  private UILabel txtToutaValue;
  [SerializeField]
  private UILabel txtToutaUpAmount;
  [SerializeField]
  private UILabel txtBuildupToutaValue;
  [SerializeField]
  private UILabel txtBuildupToutaUpAmount;
  [SerializeField]
  private GameObject dirBuildupUnityGauge;
  private int viewBuildupUnityValue;
  private GaugeRunner gaugeAnime;

  public IEnumerator Init(GameObject normalPrefab, Unit004832Menu.ResultPlayerUnit resultPlayerUnit)
  {
    UnitResult unitResult = this;
    UnitIcon linkCharacterScript = normalPrefab.CloneAndGetComponent<UnitIcon>(unitResult.linkCharacter.transform);
    IEnumerator e = linkCharacterScript.setSimpleUnit(resultPlayerUnit.afterPlayerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    linkCharacterScript.BottomModeValue = UnitIconBase.BottomMode.Level;
    linkCharacterScript.setLevelText(resultPlayerUnit.afterPlayerUnit.total_level.ToString());
    ((Collider) linkCharacterScript.buttonBoxCollider).enabled = false;
    for (int index = 0; index < unitResult.limitBreaks.Length; ++index)
    {
      if (index + 1 <= resultPlayerUnit.afterPlayerUnit.breakthrough_count)
        unitResult.limitBreaks[index].OnOff(true);
      else
        unitResult.limitBreaks[index].OnOff(false);
    }
    if (resultPlayerUnit.afterPlayerUnit.is_trust)
    {
      unitResult.slcDearDegreeBase.SetActive(true);
      unitResult.txtDearDegreeNone.SetActive(false);
      unitResult.slcTextDearDegree.spriteName = !resultPlayerUnit.afterPlayerUnit.unit.IsSea ? (!resultPlayerUnit.afterPlayerUnit.unit.IsResonanceUnit ? "slc_txt_Relevance.png__GUI__023-4-6_sozai__023-4-6_sozai_prefab" : "slc_txt_Relevance.png__GUI__023-4-6_sozai__023-4-6_sozai_prefab") : "slc_text_Favorability.png__GUI__023-4-6_sozai__023-4-6_sozai_prefab";
      UISpriteData atlasSprite = unitResult.slcTextDearDegree.GetAtlasSprite();
      ((UIWidget) unitResult.slcTextDearDegree).width = atlasSprite.width;
      ((UIWidget) unitResult.slcTextDearDegree).height = atlasSprite.height;
      unitResult.dirDearDegree.SetChildren(((Component) unitResult.dirDearDegree).transform.GetChildren().Select<Transform, GameObject>((Func<Transform, GameObject>) (x => ((Component) x).gameObject)).ToArray<GameObject>());
      ((Behaviour) unitResult.dirDearDegree).enabled = true;
      unitResult.txtDearDegree.text = string.Format("{0}%", (object) (Math.Round((double) resultPlayerUnit.afterPlayerUnit.trust_rate * 100.0) / 100.0));
      float num = resultPlayerUnit.afterPlayerUnit.trust_rate - resultPlayerUnit.beforePlayerUnit.trust_rate;
      unitResult.txtDearDegreeUpAmount.text = string.Format("+{0}%", (object) (Math.Round((double) num * 100.0) / 100.0));
    }
    else
    {
      unitResult.slcDearDegreeBase.SetActive(false);
      unitResult.txtDearDegreeNone.SetActive(true);
    }
    unitResult.dirToutaBlink.SetChildren(new GameObject[2]
    {
      ((Component) unitResult.txtToutaValue).gameObject,
      ((Component) unitResult.txtToutaUpAmount).gameObject
    });
    ((Behaviour) unitResult.dirToutaBlink).enabled = true;
    unitResult.txtToutaValue.SetTextLocalize(resultPlayerUnit.afterPlayerUnit.unity_value);
    int num1 = resultPlayerUnit.afterPlayerUnit.unity_value - resultPlayerUnit.beforePlayerUnit.unity_value;
    unitResult.txtToutaUpAmount.SetTextLocalize(string.Format("+{0}", (object) num1));
    float buildupUnityValueF1 = resultPlayerUnit.beforePlayerUnit.buildup_unity_value_f;
    double buildupUnityValueF2 = (double) resultPlayerUnit.afterPlayerUnit.buildup_unity_value_f;
    unitResult.viewBuildupUnityValue = (int) Math.Floor((double) buildupUnityValueF1);
    unitResult.txtBuildupToutaValue.SetTextLocalize(unitResult.viewBuildupUnityValue);
    Decimal num2 = ((Decimal) (float) buildupUnityValueF2 - (Decimal) buildupUnityValueF1) * 100M;
    unitResult.txtBuildupToutaUpAmount.SetTextLocalize(string.Format("+{0}%", (object) (int) num2));
    int num3 = (int) ((Decimal) buildupUnityValueF1 * 100M) % 100;
    int num4 = (int) ((Decimal) (float) buildupUnityValueF2 * 100M) % 100;
    int loopNum = (int) Math.Floor(buildupUnityValueF2) - (int) Math.Floor((double) buildupUnityValueF1);
    unitResult.gaugeAnime = new GaugeRunner(unitResult.dirBuildupUnityGauge, (float) num3 / 100f, (float) num4 / 100f, loopNum, new Func<GameObject, int, IEnumerator>(unitResult.OnBuildupUnityGaugeLooped));
    unitResult.dirUnityRoot.SetActive(true);
    unitResult.dirDearRoot.SetActive(false);
  }

  private IEnumerator OnBuildupUnityGaugeLooped(GameObject obj, int count)
  {
    this.txtBuildupToutaValue.SetTextLocalize(this.viewBuildupUnityValue + count + 1);
    yield break;
  }

  public GaugeRunner GetGaugeAnime() => this.gaugeAnime;

  public bool SkipUnityGauge()
  {
    if (!this.gaugeAnime.isRunning)
      return false;
    this.gaugeAnime.Skip();
    return true;
  }

  public void ChangeUnityViewToDearView()
  {
    this.dirUnityRoot.SetActive(false);
    this.dirDearRoot.SetActive(true);
  }
}
