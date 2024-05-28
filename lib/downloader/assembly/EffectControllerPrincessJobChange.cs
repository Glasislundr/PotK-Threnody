// Decompiled with JetBrains decompiler
// Type: EffectControllerPrincessJobChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EffectControllerPrincessJobChange : EffectControllerPrincessEvolutionBase
{
  [SerializeField]
  private GameObject odd_;
  [SerializeField]
  private GameObject even_;
  [SerializeField]
  private GameObject[] thumList_odd_;
  [SerializeField]
  private GameObject[] thumList_even_;
  [SerializeField]
  private MeshRenderer[] meshList_odd_;
  [SerializeField]
  private MeshRenderer[] meshList_even_;
  [SerializeField]
  private AnimationUnitIcon[] unitIconList_;
  [SerializeField]
  private GameObject base_model_;
  [SerializeField]
  private Transform base_trans_;
  [SerializeField]
  private MeshRenderer base_image_;
  [SerializeField]
  private GameObject new_model_;
  [SerializeField]
  private Transform new_trans_;
  [SerializeField]
  private MeshRenderer new_image_;
  [SerializeField]
  private SpriteRenderer title_sprite_;
  [SerializeField]
  private Material[] title_mat_;
  private Animator new_animator_;
  private GameObject animationUnitIconPrefab;
  private bool isNormalUnit = true;

  public override IEnumerator Initialize(PrincesEvolutionParam param, GameObject backBtn)
  {
    EffectControllerPrincessJobChange princessJobChange = this;
    princessJobChange.animationRoot.SetActive(false);
    princessJobChange.back_button_ = backBtn;
    princessJobChange.back_button_.SetActive(true);
    if (Object.op_Inequality((Object) princessJobChange.soundManager, (Object) null))
    {
      princessJobChange.soundManager.result = false;
      PrincessJobChangeSoundEffect soundManager = princessJobChange.soundManager as PrincessJobChangeSoundEffect;
      if (Object.op_Inequality((Object) soundManager, (Object) null))
      {
        UnitUnit unitUnit = param.resultUnit.unit;
        UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == unitUnit.ID && x.job_metamor_id == param.resultUnit.job_id));
        if (unitModel != null && !string.IsNullOrEmpty(unitModel.job_change_voice))
        {
          soundManager.voiceFile = unitUnit.unitVoicePattern.file_name;
          soundManager.selectorLabel = unitUnit.unitVoicePattern.SelectorLabel;
          soundManager.voiceCompleted = unitModel.job_change_voice;
        }
      }
    }
    princessJobChange.isAnimation = true;
    MeshRenderer[] meshList;
    GameObject[] thumList;
    if (param.materiaqlUnits.Count % 2 == 0)
    {
      meshList = princessJobChange.meshList_even_;
      thumList = princessJobChange.thumList_even_;
      princessJobChange.even_.SetActive(true);
      princessJobChange.odd_.SetActive(false);
    }
    else
    {
      meshList = princessJobChange.meshList_odd_;
      thumList = princessJobChange.thumList_odd_;
      princessJobChange.even_.SetActive(false);
      princessJobChange.odd_.SetActive(true);
    }
    if (Object.op_Inequality((Object) princessJobChange.soundManager, (Object) null))
      princessJobChange.soundManager.setResult(param.resultUnit.unit.rarity.index + 1);
    IEnumerator e;
    if (Object.op_Equality((Object) princessJobChange.animationUnitIconPrefab, (Object) null))
    {
      Future<GameObject> fAnimationUnitIconPrefab = Res.Prefabs.ArmorRepair.AnimationUnitIcon.Load<GameObject>();
      e = fAnimationUnitIconPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      princessJobChange.animationUnitIconPrefab = fAnimationUnitIconPrefab.Result;
      fAnimationUnitIconPrefab = (Future<GameObject>) null;
    }
    int countMaterial = param.materiaqlUnits.Count;
    for (int i = 0; i < meshList.Length; ++i)
    {
      if (i < countMaterial)
      {
        e = princessJobChange.SetTextureUnitThum(param.materiaqlUnits[i].unit.ID, meshList[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        princessJobChange.unitIconList_[i] = princessJobChange.SetCloneUnitIcon(princessJobChange.unitIconList_[i], ((Component) meshList[i]).transform, princessJobChange.animationUnitIconPrefab, param.materiaqlUnits[i]);
        ((Component) princessJobChange.unitIconList_[i]).transform.localEulerAngles = new Vector3(0.0f, 180f, 0.0f);
      }
      else
        thumList[i].SetActive(false);
    }
    if (Object.op_Inequality((Object) princessJobChange.base_model_, (Object) null))
      Object.Destroy((Object) princessJobChange.base_model_);
    if (Object.op_Inequality((Object) princessJobChange.new_model_, (Object) null))
      Object.Destroy((Object) princessJobChange.new_model_);
    e = princessJobChange.CreateModel(princessJobChange.base_trans_, param.baseUnit, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    princessJobChange.base_model_ = princessJobChange.model_creater.BaseModel;
    ((Component) princessJobChange.base_trans_).gameObject.SetActive(true);
    ((Component) princessJobChange.base_image_).gameObject.SetActive(false);
    princessJobChange.isNormalUnit = param.resultUnit.unit.IsNormalUnit;
    e = princessJobChange.CreateModel(princessJobChange.new_trans_, param.resultUnit, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    princessJobChange.new_model_ = princessJobChange.model_creater.BaseModel;
    princessJobChange.new_animator_ = princessJobChange.model_creater.UnitAnimator;
    ((Component) princessJobChange.new_trans_).gameObject.SetActive(false);
    ((Component) princessJobChange.new_image_).gameObject.SetActive(false);
    ((Renderer) princessJobChange.title_sprite_).material = princessJobChange.title_mat_[param.resultUnit.getJobData().is_vertex_x ? 1 : 0];
    Texture mainTexture = ((Renderer) princessJobChange.title_sprite_).material.mainTexture;
    princessJobChange.title_sprite_.sprite = Sprite.Create(mainTexture as Texture2D, new Rect(0.0f, 0.0f, (float) mainTexture.width, (float) mainTexture.height), new Vector2(0.5f, 0.5f));
    princessJobChange.animationRoot.SetActive(true);
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
