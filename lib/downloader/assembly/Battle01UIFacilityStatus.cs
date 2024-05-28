// Decompiled with JetBrains decompiler
// Type: Battle01UIFacilityStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01UIFacilityStatus : NGBattleMenuBase
{
  public NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected UI2DSprite attribution;
  [SerializeField]
  protected UIWidget thumb_container;
  [SerializeField]
  protected Transform passage;
  [SerializeField]
  protected Transform destruction;
  [SerializeField]
  protected Transform visibility;
  [SerializeField]
  protected int depthSkillTargetParent = 11;
  [SerializeField]
  protected Transform skillTargetParent1;
  [SerializeField]
  protected Transform skillTargetParent2;
  [SerializeField]
  protected GameObject node_without_hp_gauge;
  [SerializeField]
  protected GameObject node_hp_gauge;
  [SerializeField]
  protected GameObject node_dir_base_own;
  [SerializeField]
  protected GameObject node_dir_base_enemy;
  [SerializeField]
  protected UILabel txt_name;
  [SerializeField]
  protected UILabel txt_description;
  [SerializeField]
  protected UILabel txt_Hp_num_numerator;
  [SerializeField]
  protected UILabel txt_Hp_num_denominator;
  [SerializeField]
  protected UILabel txt_LV_num;
  [SerializeField]
  protected UILabel txt_DEF_num;
  [SerializeField]
  protected UILabel txt_INT_num;
  private BL.BattleModified<BL.Unit> modified;
  private FacilityIcon facilityIcon;
  private GameObject skillTgtIcon1;
  private GameObject skillTgtIcon2;
  private bool isSetHp;

  private void Awake()
  {
    ((Behaviour) this.attribution).enabled = false;
    foreach (UIWidget componentsInChild in ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true))
    {
      if (Object.op_Equality((Object) ((Component) componentsInChild).GetComponent<UIButton>(), (Object) null))
        ((UIRect) componentsInChild).alpha = 0.0f;
    }
  }

  public override IEnumerator onInitAsync()
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/FacilityIcon/dir_facility_thumb").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.facilityIcon = prefabF.Result.Clone(((Component) this.thumb_container).gameObject.transform).GetComponent<FacilityIcon>();
    NGUITools.AdjustDepth(((Component) this.facilityIcon).gameObject, this.thumb_container.depth);
    this.facilityIcon.SetBasedOnHeight(this.thumb_container.height);
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    BoxCollider componentInChildren2 = ((Component) trans).GetComponentInChildren<BoxCollider>();
    if (!Object.op_Inequality((Object) componentInChildren1, (Object) null) || !Object.op_Inequality((Object) componentInChildren2, (Object) null))
      return icon;
    ((UIWidget) componentInChildren1).SetDimensions((int) componentInChildren2.size.x, (int) componentInChildren2.size.y);
    UI2DSprite ui2Dsprite = componentInChildren1;
    ((UIWidget) ui2Dsprite).depth = ((UIWidget) ui2Dsprite).depth + 150;
    return icon;
  }

  private IEnumerator doSetIcon(BL.Unit unit)
  {
    IEnumerator e = this.facilityIcon.SetUnit(unit.playerUnit, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.attribution.sprite2D = GuildUtil.LoadKindSprite((FacilityCategory) unit.unit.facility.category_id, unit.GetElement());
    ((Behaviour) this.attribution).enabled = true;
    if (unit.playerUnit.skills.Length != 0)
    {
      BattleskillSkill skill = unit.playerUnit.skills[0].skill;
      if (Object.op_Equality((Object) this.skillTgtIcon1, (Object) null) && Object.op_Equality((Object) this.skillTgtIcon2, (Object) null))
      {
        Future<GameObject> skillTargetPrefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
        e = skillTargetPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = skillTargetPrefabF.Result;
        this.skillTgtIcon1 = this.createIcon(result, this.skillTargetParent1);
        this.skillTgtIcon2 = this.createIcon(result, this.skillTargetParent2);
        skillTargetPrefabF = (Future<GameObject>) null;
      }
      ((Component) this.skillTargetParent1).gameObject.SetActive(true);
      ((Component) this.skillTargetParent2).gameObject.SetActive(true);
      this.skillTgtIcon1.GetComponent<SkillGenreIcon>().Init(skill.genre1);
      this.skillTgtIcon2.GetComponent<SkillGenreIcon>().Init(skill.genre2);
      NGUITools.AdjustDepth(this.skillTgtIcon1, this.depthSkillTargetParent);
      NGUITools.AdjustDepth(this.skillTgtIcon2, this.depthSkillTargetParent);
      skill = (BattleskillSkill) null;
    }
    else
    {
      ((Component) this.skillTargetParent1).gameObject.SetActive(false);
      ((Component) this.skillTargetParent2).gameObject.SetActive(false);
    }
  }

  protected override void LateUpdate_Battle()
  {
    if (this.modified == null)
      return;
    bool flag = this.modified.isChangedOnce();
    if (flag)
    {
      BL.Unit unit = this.modified.value;
      Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) unit);
      this.StartCoroutine(this.doSetIcon(unit));
      this.setText(this.txt_name, unit.unit.name);
      this.setText(this.txt_description, unit.unit.description);
      this.setText(this.txt_LV_num, unit.lv);
      this.txt_Hp_num_denominator.SetTextLocalize("/" + battleParameter.Hp.ToString());
      this.setText(this.txt_DEF_num, battleParameter.PhysicalDefense);
      this.setText(this.txt_INT_num, battleParameter.MagicDefense);
      foreach (UIWidget componentsInChild in ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true))
      {
        if ((double) ((UIRect) componentsInChild).alpha == 0.0 && Object.op_Equality((Object) ((Component) componentsInChild).GetComponent<UIButton>(), (Object) null))
          ((UIRect) componentsInChild).alpha = 1f;
      }
    }
    if (this.battleManager.isBattleEnable)
    {
      if (!flag && !this.isSetHp)
        return;
      BL.Unit beUnit = this.modified.value;
      Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) beUnit);
      this.hpGauge.setValue(beUnit.hp, battleParameter.Hp);
      this.setText(this.txt_Hp_num_numerator, beUnit.hp);
      this.isSetHp = false;
    }
    else
    {
      if (!flag)
        return;
      this.isSetHp = true;
    }
  }

  public void setUnit(BL.Unit unit)
  {
    this.hpGauge.setValue(unit.hp, unit.parameter.Hp, false);
    this.modified = BL.Observe<BL.Unit>(unit);
    bool isEnemy = unit.playerUnit.is_enemy;
    this.node_dir_base_own.SetActive(!isEnemy);
    this.node_dir_base_enemy.SetActive(isEnemy);
    this.setIconOnOff(this.passage, unit.isPutOn);
    this.setIconOnOff(this.destruction, unit.isAttackTarget);
    this.setIconOnOff(this.visibility, unit.isView);
  }

  private void setIconOnOff(Transform parent, bool on)
  {
    GameObject gameObject1 = (GameObject) null;
    GameObject gameObject2 = (GameObject) null;
    foreach (Component component in parent)
    {
      GameObject gameObject3 = component.gameObject;
      if (((Object) gameObject3).name.LastIndexOf("_on") != -1)
        gameObject1 = gameObject3;
      else if (((Object) gameObject3).name.LastIndexOf("_off") != -1)
        gameObject2 = gameObject3;
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
      {
        if (Object.op_Inequality((Object) gameObject2, (Object) null))
          break;
      }
    }
    if (Object.op_Inequality((Object) gameObject1, (Object) null))
      gameObject1.SetActive(on);
    if (!Object.op_Inequality((Object) gameObject2, (Object) null))
      return;
    gameObject2.SetActive(!on);
  }

  public BL.Unit getUnit() => this.modified.value;
}
