// Decompiled with JetBrains decompiler
// Type: Battle02UIPlayerStatusDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Battle02UIPlayerStatusDetail : Battle02MenuBase
{
  [SerializeField]
  protected UI2DSprite link_Character;
  [SerializeField]
  protected UILabel txt_CharaName_18;
  [SerializeField]
  protected UILabel txt_Lv_22;
  [SerializeField]
  protected UI2DSprite link_SPAttack1;
  [SerializeField]
  protected UI2DSprite link_SPAttack2;
  [SerializeField]
  protected UILabel txt_Movement_22;
  [SerializeField]
  protected UILabel txt_Rangement_22;
  public GameObject label_Done;
  public GameObject label_Standby;
  public GameObject label_Dead;
  public GameObject spa_icon_prefab;
  [SerializeField]
  protected Transform[] spaTypeIconParent;
  private GameObject[] spaTypeIcon = new GameObject[2];

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = NGUITools.CalculateNextDepth(((Component) trans).gameObject);
    return icon;
  }

  private string getAttackRange(BL.Unit v)
  {
    return string.Format("{0} - {1}", (object) ((IEnumerable<int>) v.attackRange).Min(), (object) ((IEnumerable<int>) v.attackRange).Max());
  }

  protected override IEnumerator Start_Battle()
  {
    yield break;
  }

  protected override void LateUpdate_Battle()
  {
  }

  public override void UpdateData()
  {
    if (this.modified == null || !this.modified.isChangedOnce())
      return;
    BL.Unit unit1 = this.modified.value;
    UILabel txtCharaName18 = this.txt_CharaName_18;
    UnitUnit unit2 = unit1.unit;
    SkillMetamorphosis metamorphosis = unit1.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unit2.getName(metamorphosisId);
    this.setText(txtCharaName18, name);
    Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) unit1);
    this.setColordText(this.txt_Movement_22, battleParameter.Move, battleParameter.MoveIncr);
    this.setText(this.txt_Rangement_22, this.getAttackRange(unit1));
    this.CreateUnitSprite(this.link_Character);
    UnitFamily[] specialAttackTargets = unit1.playerUnit.equippedWeaponGearOrInitial.SpecialAttackTargets;
    for (int index = 0; index < specialAttackTargets.Length && this.spaTypeIconParent.Length > index; ++index)
    {
      this.spaTypeIcon[index] = this.createIcon(this.spa_icon_prefab, this.spaTypeIconParent[index]);
      this.spaTypeIcon[index].GetComponent<SPAtkTypeIcon>().InitKindId(specialAttackTargets[index]);
    }
    ((Behaviour) ((Component) this.spaTypeIconParent[0]).gameObject.GetComponent<UI2DSprite>()).enabled = false;
    ((Behaviour) ((Component) this.spaTypeIconParent[1]).gameObject.GetComponent<UI2DSprite>()).enabled = false;
    if (Singleton<NGBattleManager>.GetInstance().environment.core.isCompleted(unit1))
    {
      if (unit1.isDead)
      {
        this.label_Done.SetActive(false);
        this.label_Standby.SetActive(false);
        this.label_Dead.SetActive(true);
      }
      else
      {
        this.label_Done.SetActive(true);
        this.label_Standby.SetActive(false);
        this.label_Dead.SetActive(false);
      }
    }
    else
    {
      this.label_Done.SetActive(false);
      this.label_Standby.SetActive(true);
      this.label_Dead.SetActive(false);
    }
  }

  public override void onBackButton()
  {
  }
}
