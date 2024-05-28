// Decompiled with JetBrains decompiler
// Type: EffectControllerPrincessEvolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectControllerPrincessEvolution : EffectControllerPrincessEvolutionBase
{
  [SerializeField]
  private GameObject odd_;
  [SerializeField]
  private GameObject even_;
  [SerializeField]
  private List<GameObject> thumList_odd_;
  [SerializeField]
  private List<GameObject> thumList_even_;
  [SerializeField]
  private List<MeshRenderer> meshList_odd_;
  [SerializeField]
  private List<MeshRenderer> meshList_even_;
  [SerializeField]
  private List<AnimationUnitIcon> unitIconList_;
  [SerializeField]
  private CommonRarityAnim common_rarity_anim_;
  [SerializeField]
  private GameObject base_model_;
  [SerializeField]
  private Transform base_trans_;
  [SerializeField]
  private MeshRenderer base_image_;
  [SerializeField]
  private GameObject Rarity;
  [SerializeField]
  private GameObject Reincarnation;
  [SerializeField]
  private GameObject new_model_;
  [SerializeField]
  private Transform new_trans_;
  [SerializeField]
  private MeshRenderer new_image_;
  private Animator new_animator_;
  [SerializeField]
  private GameObject is_new_;
  private GameObject animationUnitIconPrefab;
  private bool isNormalUnit = true;

  public override IEnumerator Initialize(PrincesEvolutionParam param, GameObject backBtn)
  {
    EffectControllerPrincessEvolution princessEvolution = this;
    princessEvolution.animationRoot.SetActive(false);
    princessEvolution.back_button_ = backBtn;
    princessEvolution.back_button_.SetActive(true);
    princessEvolution.soundManager.result = false;
    princessEvolution.is_new_.SetActive(param.is_new);
    princessEvolution.isAnimation = true;
    List<MeshRenderer> meshList = new List<MeshRenderer>();
    List<GameObject> thumList = new List<GameObject>();
    if (param.materiaqlUnits.Count % 2 == 0)
    {
      meshList = princessEvolution.meshList_even_;
      thumList = princessEvolution.thumList_even_;
      princessEvolution.even_.SetActive(true);
      princessEvolution.odd_.SetActive(false);
    }
    else
    {
      meshList = princessEvolution.meshList_odd_;
      thumList = princessEvolution.thumList_odd_;
      princessEvolution.even_.SetActive(false);
      princessEvolution.odd_.SetActive(true);
    }
    princessEvolution.soundManager.setResult(param.resultUnit.unit.rarity.index + 1);
    princessEvolution.Rarity.gameObject.SetActive(false);
    princessEvolution.Reincarnation.gameObject.SetActive(false);
    if (param.mode == Unit00499Scene.Mode.Evolution || param.mode == Unit00499Scene.Mode.EarthEvolution)
    {
      princessEvolution.Rarity.gameObject.SetActive(true);
      princessEvolution.SetCommonRarity(princessEvolution.common_rarity_anim_.rarity_obj_list_, param.resultUnit.unit.rarity.index);
    }
    else
      princessEvolution.Reincarnation.gameObject.SetActive(true);
    IEnumerator e;
    if (Object.op_Equality((Object) princessEvolution.animationUnitIconPrefab, (Object) null))
    {
      Future<GameObject> fAnimationUnitIconPrefab = Res.Prefabs.ArmorRepair.AnimationUnitIcon.Load<GameObject>();
      e = fAnimationUnitIconPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      princessEvolution.animationUnitIconPrefab = fAnimationUnitIconPrefab.Result;
      fAnimationUnitIconPrefab = (Future<GameObject>) null;
    }
    for (int i = 0; i < meshList.Count; ++i)
    {
      if (i < param.materiaqlUnits.Count)
      {
        e = princessEvolution.SetTextureUnitThum(param.materiaqlUnits[i].unit.ID, meshList[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        princessEvolution.unitIconList_[i] = princessEvolution.SetCloneUnitIcon(princessEvolution.unitIconList_[i], ((Component) meshList[i]).transform, princessEvolution.animationUnitIconPrefab, param.materiaqlUnits[i]);
        ((Component) princessEvolution.unitIconList_[i]).transform.localEulerAngles = new Vector3(0.0f, 180f, 0.0f);
      }
      else
        thumList[i].SetActive(false);
    }
    if (Object.op_Inequality((Object) princessEvolution.base_model_, (Object) null))
      Object.Destroy((Object) princessEvolution.base_model_);
    if (Object.op_Inequality((Object) princessEvolution.new_model_, (Object) null))
      Object.Destroy((Object) princessEvolution.new_model_);
    if (param.baseUnit.unit.IsNormalUnit)
    {
      e = princessEvolution.CreateModel(princessEvolution.base_trans_, param.baseUnit, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      princessEvolution.base_model_ = princessEvolution.model_creater.BaseModel;
      ((Component) princessEvolution.base_trans_).gameObject.SetActive(true);
      ((Component) princessEvolution.base_image_).gameObject.SetActive(false);
    }
    else
    {
      e = princessEvolution.SetTextureUnit(param.baseUnit.unit.ID, princessEvolution.base_image_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) princessEvolution.base_trans_).gameObject.SetActive(false);
      ((Component) princessEvolution.base_image_).gameObject.SetActive(true);
    }
    princessEvolution.isNormalUnit = param.resultUnit.unit.IsNormalUnit;
    if (princessEvolution.isNormalUnit)
    {
      e = princessEvolution.CreateModel(princessEvolution.new_trans_, param.resultUnit, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      princessEvolution.new_model_ = princessEvolution.model_creater.BaseModel;
      princessEvolution.new_animator_ = princessEvolution.model_creater.UnitAnimator;
      ((Component) princessEvolution.new_trans_).gameObject.SetActive(false);
      ((Component) princessEvolution.new_image_).gameObject.SetActive(false);
    }
    else
    {
      e = princessEvolution.SetTextureUnit(param.resultUnit.unit.ID, princessEvolution.new_image_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) princessEvolution.new_trans_).gameObject.SetActive(false);
      ((Component) princessEvolution.new_image_).gameObject.SetActive(false);
    }
    yield return (object) princessEvolution.CreateRarityStartObjects(param.resultUnit.unit);
    princessEvolution.animationRoot.SetActive(true);
  }

  private IEnumerator CreateRarityStartObjects(UnitUnit unit)
  {
    Future<GameObject> rarityStarObj = (Future<GameObject>) null;
    int num = unit.rarity.index + 1;
    switch (num)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar1.Load<GameObject>();
        break;
      case 5:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar2.Load<GameObject>();
        break;
      case 6:
        rarityStarObj = Res.Prefabs.common_animation.common_rarity.raritystar3.Load<GameObject>();
        break;
      default:
        Debug.LogError((object) ("想定していないレアリティのため、星Prefabを取得できません: " + (object) num));
        break;
    }
    IEnumerator e = rarityStarObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = rarityStarObj.Result;
    for (int index = 0; index <= unit.rarity.index; ++index)
    {
      if (!Object.op_Equality((Object) this.common_rarity_anim_.rarity_list[unit.rarity.index].rarity_list[index], (Object) null))
        result.Clone(this.common_rarity_anim_.rarity_list[unit.rarity.index].rarity_list[index].transform);
    }
  }

  public void Evolution()
  {
    ((Component) this.base_trans_).gameObject.SetActive(false);
    ((Component) this.base_image_).gameObject.SetActive(false);
    if (this.isNormalUnit)
      ((Component) this.new_trans_).gameObject.SetActive(true);
    else
      ((Component) this.new_image_).gameObject.SetActive(true);
    if (Object.op_Equality((Object) this.new_animator_, (Object) null))
      return;
    ((Behaviour) this.new_animator_).enabled = true;
  }
}
