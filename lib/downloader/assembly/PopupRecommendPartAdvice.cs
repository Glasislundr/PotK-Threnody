// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartAdvice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/Advice")]
public class PopupRecommendPartAdvice : PopupRecommendPart
{
  [SerializeField]
  private Transform lnkRoleIcon_;
  [SerializeField]
  private UILabel txtRoleName_;
  [SerializeField]
  private UILabel txtRoleDescription_;
  [SerializeField]
  private UILabel txtActivity_;
  [SerializeField]
  private HeadlineLabel txtHeadline_;
  [SerializeField]
  private GameObject objFull_;
  private UnitAdvice advice_;
  private GameObject fullPrefab_;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    PopupRecommendPartAdvice recommendPartAdvice = this;
    MasterData.UnitAdvice.TryGetValue(playerUnit.unit.same_character_id, out recommendPartAdvice.advice_);
    if (recommendPartAdvice.advice_ == null || string.IsNullOrEmpty(recommendPartAdvice.advice_.advice))
    {
      ((Component) recommendPartAdvice).gameObject.SetActive(false);
    }
    else
    {
      Future<GameObject> ldIcon = UnitRoleIcon.createLoader();
      yield return (object) ldIcon.Wait();
      UnitRole unitRole = recommendPartAdvice.advice_.role;
      UnitActivityScenes activity = recommendPartAdvice.advice_.activity;
      if (unitRole != null)
      {
        yield return (object) ldIcon.Result.Clone(recommendPartAdvice.lnkRoleIcon_).GetComponent<UnitRoleIcon>().doInitialize(unitRole);
        recommendPartAdvice.txtRoleName_.SetTextLocalize(unitRole.name);
        recommendPartAdvice.txtRoleDescription_.SetTextLocalize(unitRole.description);
      }
      if (activity != null)
        recommendPartAdvice.txtActivity_.SetTextLocalize(activity.description);
      recommendPartAdvice.setHeadline(recommendPartAdvice.advice_.advice);
      ((Component) recommendPartAdvice).gameObject.SetActive(true);
      if (recommendPartAdvice.objFull_.activeSelf)
        recommendPartAdvice.StartCoroutine("doLoadResource");
    }
  }

  private IEnumerator doLoadResource()
  {
    if (Object.op_Equality((Object) this.fullPrefab_, (Object) null))
    {
      Future<GameObject> ld = PopupUnitAdvice.loadResource();
      yield return (object) ld.Wait();
      this.fullPrefab_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
  }

  private void setHeadline(string txt)
  {
    this.objFull_.SetActive(this.txtHeadline_.SetHeadline(txt));
  }

  public void onClickedFull()
  {
    if (Object.op_Equality((Object) this.fullPrefab_, (Object) null))
      return;
    PopupUnitAdvice.open(this.fullPrefab_, this.advice_);
  }
}
