// Decompiled with JetBrains decompiler
// Type: PopupAttackClassDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupAttackClassDetail : PopupAutoCloseOnAnyTap
{
  [SerializeField]
  [Tooltip("攻撃区分アイコン")]
  private AttackClassIcon iconAttackClass_;
  [SerializeField]
  [Tooltip("攻撃区分名")]
  private UILabel txtName_;
  [SerializeField]
  [Tooltip("説明")]
  private UILabel txtDescription_;

  public static Future<GameObject> createPrefabLoader()
  {
    return new ResourceObject("Prefabs/unit004_4_3/popup_AttackClass_detail").Load<GameObject>();
  }

  public static IEnumerator show(GameObject prefab, GearGear gear)
  {
    if (gear != null && gear.gearClassification != null)
    {
      Singleton<PopupManager>.GetInstance().open(prefab, isViewBack: false).GetComponent<PopupAttackClassDetail>().initialize(gear);
      while (Singleton<PopupManager>.GetInstance().isOpen)
        yield return (object) null;
    }
  }

  private void initialize(GearGear gear)
  {
    this.setEventOnAnyTap();
    GearAttackClassificationTable classificationTable;
    BattleskillSkill skill;
    if (!MasterData.GearAttackClassificationTable.TryGetValue(gear.gearClassification.attack_classification_GearAttackClassification, out classificationTable) || (skill = classificationTable.skill) == null)
    {
      ((Component) this.iconAttackClass_).gameObject.SetActive(false);
      ((Component) this.txtName_).gameObject.SetActive(false);
      ((Component) this.txtDescription_).gameObject.SetActive(false);
    }
    else
    {
      this.iconAttackClass_.Initialize(gear.gearClassification.attack_classification);
      this.txtName_.SetTextLocalize(skill.name);
      this.txtDescription_.SetTextLocalize(skill.description);
    }
  }
}
