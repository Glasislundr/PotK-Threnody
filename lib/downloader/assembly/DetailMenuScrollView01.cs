// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollView01
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailMenuScrollView01 : DetailMenuScrollViewBase
{
  [SerializeField]
  private GameObject extraSkillObject;
  [SerializeField]
  private UI2DSprite extraSKill;
  [SerializeField]
  private UIButton extraSKillBtn;
  [SerializeField]
  private UILabel txtDearDegree;
  [SerializeField]
  private LoveGaugeController loveGaugeController;
  [SerializeField]
  private GameObject floatingSkillDialog;
  [SerializeField]
  protected NGTweenGaugeScale lvGauge;
  [SerializeField]
  protected NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected GameObject[] slc_LBicon_None;
  [SerializeField]
  protected GameObject[] slc_LBicon_Blue;
  [SerializeField]
  protected GameObject slc_Limitbreak;
  [SerializeField]
  protected UILabel txt_Attack;
  [SerializeField]
  protected UILabel txt_Cost;
  [SerializeField]
  protected UILabel txt_Critical;
  [SerializeField]
  protected UILabel txt_Defense;
  [SerializeField]
  protected UILabel txt_Dexterity;
  [SerializeField]
  protected UILabel txt_Evasion;
  [SerializeField]
  protected UILabel txt_Fighting;
  [SerializeField]
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  [SerializeField]
  protected UILabel txt_Movement;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel txt_Lvmax;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  protected UILabel txt_Hpmax;
  [SerializeField]
  protected GameObject slc_maxStarHp;
  [SerializeField]
  protected UILabel TxtJobname;
  [SerializeField]
  protected UILabel TxtPrincesstype;
  [SerializeField]
  private GameObject dir_3DModel;
  [SerializeField]
  private GameObject ui3DModelLoadDummy;
  [SerializeField]
  private UILabel txt_Unity;
  public UI3DModel ui_3DModel;
  public GameObject unitmodel;
  private Battle0171111Event floatingSkillDialogObject;
  private PlayerUnit pUnit;

  public GameObject Dir3DModel => this.dir_3DModel;

  public UI3DModel UI3DModel => this.ui_3DModel;

  public UITexture UI3DModelTexture => this.ui_3DModel.uiTexture;

  private IEnumerator LoadExtraSkill(UnitSkillAwake skill, PlayerUnit unit)
  {
    Future<Sprite> spriteF = skill.skill.LoadBattleSkillIcon();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.extraSKill.sprite2D = spriteF.Result;
    if ((double) skill.need_affection > (double) unit.trust_rate)
      ((UIWidget) this.extraSKill).color = Color.gray;
  }

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    this.pUnit = playerUnit;
    if (Object.op_Inequality((Object) this.floatingSkillDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.floatingSkillDialogObject, (Object) null))
      {
        this.floatingSkillDialog.transform.Clear();
        this.floatingSkillDialogObject = prefabs[0].Clone(this.floatingSkillDialog.transform).GetComponentInChildren<Battle0171111Event>();
      }
      this.floatingSkillDialogObject.setSkillLv(0);
      ((Component) ((Component) this.floatingSkillDialogObject).transform.parent).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) this.extraSkillObject, (Object) null))
    {
      this.extraSkillObject.SetActive(false);
      if (playerUnit.unit.trust_target_flag)
      {
        this.extraSkillObject.SetActive(true);
        double num = Math.Round((double) playerUnit.trust_rate * 100.0) / 100.0;
        this.txtDearDegree.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_TRUST_RATE_PERCENT, (IDictionary) new Hashtable()
        {
          {
            (object) "trust_rate",
            (object) string.Format("{0}", (object) num)
          }
        }));
        int trustRateMax = PlayerUnit.GetTrustRateMax();
        this.loveGaugeController.setValue((int) playerUnit.trust_rate, (int) playerUnit.trust_max_rate, trustRateMax, false);
        UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
        if (awakeSkills == null || awakeSkills.Length == 0)
        {
          this.extraSkillObject.SetActive(false);
        }
        else
        {
          IEnumerator e = this.LoadExtraSkill(awakeSkills[0], playerUnit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    Func<int, int, float> func = (Func<int, int, float>) ((val, max) => val <= 0 ? 0.0f : (float) val / (float) max);
    int max1 = playerUnit.exp_next + playerUnit.exp - 1;
    int n = playerUnit.exp;
    if (n > max1)
      n = max1;
    this.lvGauge.setValue(n, max1, false);
    Judgement.NonBattleParameter nonBattleParameter;
    if (!this.isMemory)
    {
      nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit);
      this.setText(this.txt_Lv, playerUnit.level);
      this.txt_Lvmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
      {
        {
          (object) "max",
          (object) playerUnit.max_level
        }
      }));
    }
    else
    {
      nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnitMemory(playerUnit);
      this.setText(this.txt_Lv, playerUnit.memory_level);
      this.txt_Lvmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
      {
        {
          (object) "max",
          (object) playerUnit.max_level
        }
      }));
    }
    if (playerUnit.tower_is_entry && Singleton<CommonRoot>.GetInstance().headerType == CommonRoot.HeaderType.Tower)
    {
      this.setText(this.txt_Hp, TowerUtil.GetHp(nonBattleParameter.Hp, playerUnit.TowerHpRate));
      this.hpGauge.setValue(TowerUtil.GetHp(nonBattleParameter.Hp, playerUnit.TowerHpRate), nonBattleParameter.Hp, false);
    }
    else
      this.setText(this.txt_Hp, nonBattleParameter.Hp);
    this.txt_Hpmax.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_DETAIL_MAX, (IDictionary) new Hashtable()
    {
      {
        (object) "max",
        (object) nonBattleParameter.Hp
      }
    }));
    this.slc_maxStarHp.SetActive(playerUnit.hp.is_max);
    this.setText(this.txt_Cost, playerUnit.cost);
    this.setText(this.txt_Movement, nonBattleParameter.Move);
    this.setText(this.txt_Attack, nonBattleParameter.PhysicalAttack);
    this.setText(this.txt_Defense, nonBattleParameter.PhysicalDefense);
    this.setText(this.txt_Matk, nonBattleParameter.MagicAttack);
    this.setText(this.txt_Mdef, nonBattleParameter.MagicDefense);
    this.setText(this.txt_Dexterity, nonBattleParameter.Hit);
    this.setText(this.txt_Critical, nonBattleParameter.Critical);
    this.setText(this.txt_Evasion, nonBattleParameter.Evasion);
    this.setText(this.txt_Fighting, nonBattleParameter.Combat);
    this.setParamUnity(playerUnit);
    this.TxtJobname.SetTextLocalize(playerUnit.unit.job.name);
    this.TxtPrincesstype.SetTextLocalize(playerUnit.unit_type.name);
    if (!this.isEarthMode)
      this.setBicon(playerUnit.breakthrough_count, playerUnit.unit.breakthrough_limit);
    return true;
  }

  private void setParamUnity(PlayerUnit playerUnit)
  {
    this.txt_Unity.SetTextLocalize(playerUnit.unityTotal.ToString());
  }

  public override IEnumerator setModel(
    PlayerUnit playerUnit,
    GameObject modelPrefab,
    Vector3 modelPos,
    bool light,
    Action action = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DetailMenuScrollView01 menuScrollView01 = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Inequality((Object) menuScrollView01.unitmodel, (Object) null))
      Object.Destroy((Object) menuScrollView01.unitmodel);
    if (Object.op_Equality((Object) menuScrollView01.ui_3DModel, (Object) null))
    {
      menuScrollView01.dir_3DModel.transform.Clear();
      menuScrollView01.ui_3DModel = modelPrefab.Clone(menuScrollView01.dir_3DModel.transform).GetComponent<UI3DModel>();
    }
    menuScrollView01.ui_3DModel.lightOn = light;
    menuScrollView01.ui_3DModel.SetScale = 220f;
    ((Component) menuScrollView01.ui_3DModel.ModelCamera).transform.localPosition = modelPos;
    menuScrollView01.ui_3DModel.Remove();
    ((UIRect) menuScrollView01.ui_3DModel.uiTexture).alpha = 0.0f;
    menuScrollView01.ui3DModelLoadDummy.SetActive(true);
    menuScrollView01.StartCoroutine(menuScrollView01.loadUI3DModelAsync(playerUnit, modelPos, action));
    return false;
  }

  private IEnumerator loadUI3DModelAsync(PlayerUnit playerUnit, Vector3 modelPos, Action action = null)
  {
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<NGSceneManager>.GetInstance().IsDangerAsyncLoadResource()));
    yield return (object) this.ui_3DModel.Unit(playerUnit, (Action) (() =>
    {
      this.unitmodel = this.ui_3DModel.model_creater_.BaseModel;
      ((Component) this.ui_3DModel.ModelCamera).transform.localPosition = modelPos;
      ((UIRect) this.ui_3DModel.uiTexture).alpha = 1f;
      this.ui3DModelLoadDummy.SetActive(false);
    }));
    if (action != null)
      action();
  }

  public void cacheModel() => this.unitmodel = this.ui_3DModel.model_creater_.BaseModel;

  private void setBicon(int count, int max)
  {
    for (int index = 0; index < this.slc_LBicon_None.Length; ++index)
    {
      this.slc_LBicon_None[index].SetActive(false);
      this.slc_LBicon_Blue[index].SetActive(false);
      if (index < max)
        this.slc_LBicon_None[index].SetActive(true);
    }
    for (int index = 0; index < max; ++index)
    {
      this.slc_LBicon_None[index].SetActive(index >= count);
      this.slc_LBicon_Blue[index].SetActive(index < count);
    }
    if (max != 0)
      return;
    this.slc_Limitbreak.SetActive(false);
  }

  public void onClicExtraSKill()
  {
    UnitSkillAwake[] awakeSkills = this.pUnit.GetAwakeSkills();
    if (awakeSkills == null || awakeSkills.Length == 0)
      return;
    UnitSkillAwake unitSkillAwake = awakeSkills[0];
    BattleskillSkill skill = unitSkillAwake.skill;
    string empty = string.Empty;
    string addDescription;
    if ((double) unitSkillAwake.need_affection > (double) this.pUnit.trust_rate)
      addDescription = Consts.Format(this.pUnit.unit.IsSea ? Consts.GetInstance().popup_004_ExtraSkill_affection_condition : Consts.GetInstance().popup_004_ExtraSkill_affection_condition_second, (IDictionary) new Hashtable()
      {
        {
          (object) "percent",
          (object) unitSkillAwake.need_affection
        }
      });
    else
      addDescription = Consts.GetInstance().popup_004_ExtraSkill_affection_complete;
    this.floatingSkillDialogObject.setData(skill, addDescription);
    this.floatingSkillDialogObject.Show();
  }

  private void OnEnable() => this.setActive3DModel(true);

  private void OnDisable() => this.setActive3DModel(false);

  private void setActive3DModel(bool bActive)
  {
    if (Object.op_Equality((Object) this.ui_3DModel, (Object) null) || Object.op_Equality((Object) this.ui_3DModel.ModelCamera, (Object) null) || Object.op_Equality((Object) ((Component) this.ui_3DModel.ModelCamera).gameObject, (Object) null))
      return;
    ((Component) this.ui_3DModel.ModelCamera).gameObject.SetActive(bActive);
  }
}
