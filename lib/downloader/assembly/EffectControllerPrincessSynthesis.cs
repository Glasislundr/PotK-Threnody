// Decompiled with JetBrains decompiler
// Type: EffectControllerPrincessSynthesis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectControllerPrincessSynthesis : EffectController
{
  [SerializeField]
  private List<GameObject> thumList_;
  [SerializeField]
  private MeshRenderer[] baseList_;
  [SerializeField]
  private MeshRenderer[] coverList_;
  [SerializeField]
  private List<MeshRenderer> meshList_;
  [SerializeField]
  private List<MeshRenderer> meshMaleList_;
  [SerializeField]
  private GameObject animation_root_;
  [SerializeField]
  private List<MeshRenderer> synthesis_list_;
  [SerializeField]
  private List<GameObject> success_list_;
  public PrincessSynthesisSoundEffect sound_manager_;

  public void EndSE() => this.sound_manager_.OnPlayResult();

  private IEnumerator initialize(
    List<PlayerUnit> unit_list,
    List<PlayerUnit> result_unit,
    List<int> other_list)
  {
    EffectControllerPrincessSynthesis princessSynthesis = this;
    princessSynthesis.isAnimation = true;
    IEnumerator e;
    for (int i = 0; i < princessSynthesis.thumList_.Count; ++i)
    {
      if (i < unit_list.Count)
      {
        UnitUnit unit = unit_list[i].unit;
        princessSynthesis.setActiveAppendedSkillObject(i, false);
        if (unit.character.gender == UnitGender.male)
        {
          ((Component) princessSynthesis.meshList_[i]).gameObject.SetActive(false);
          ((Component) princessSynthesis.meshMaleList_[i]).gameObject.SetActive(true);
          e = princessSynthesis.SetTextureUnitCutin(unit.ID, princessSynthesis.meshMaleList_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          ((Component) princessSynthesis.meshMaleList_[i]).gameObject.SetActive(false);
          ((Component) princessSynthesis.meshList_[i]).gameObject.SetActive(true);
          e = princessSynthesis.SetTextureUnitCutin(unit.ID, princessSynthesis.meshList_[i]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else
        princessSynthesis.thumList_[i].SetActive(false);
    }
    foreach (MeshRenderer synthesis in princessSynthesis.synthesis_list_)
    {
      e = princessSynthesis.SetTextureUnit(result_unit[1].unit.ID, synthesis, result_unit[1].job_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (other_list[0] == 1)
    {
      princessSynthesis.success_list_[0].SetActive(true);
      princessSynthesis.sound_manager_.setResult(1);
    }
    else
    {
      princessSynthesis.success_list_[1].SetActive(true);
      princessSynthesis.sound_manager_.setResult(3);
    }
  }

  private void setActiveAppendedSkillObject(int index, bool flag)
  {
    if (this.baseList_ == null || this.baseList_.Length <= index || !Object.op_Inequality((Object) this.baseList_[index], (Object) null))
      return;
    ((Component) this.baseList_[index]).gameObject.SetActive(flag);
    ((Component) this.coverList_[index]).gameObject.SetActive(flag);
  }

  public IEnumerator Set(
    List<PlayerUnit> unit_list,
    List<PlayerUnit> result_unit,
    List<int> other_list,
    GameObject back_button)
  {
    EffectControllerPrincessSynthesis princessSynthesis = this;
    IEnumerator e = princessSynthesis.initialize(unit_list, result_unit, other_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    princessSynthesis.back_button_ = back_button;
    princessSynthesis.back_button_.SetActive(true);
    princessSynthesis.sound_manager_.result = false;
    princessSynthesis.animation_root_.SetActive(true);
  }

  public enum ResultUnitID
  {
    OLD,
    EVOLUTION,
  }
}
