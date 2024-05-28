// Decompiled with JetBrains decompiler
// Type: GuildFacilitySellNumEnterPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildFacilitySellNumEnterPopup : BackButtonMonoBehaiviour
{
  private Guild0282Menu guildMapMenu;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UILabel TxtFacilityName;
  [SerializeField]
  private UILabel TxtFacilityDescription;
  [SerializeField]
  private UILabel TxtPossessionNum;
  [SerializeField]
  private UILabel TxtSellValue;
  [SerializeField]
  private UILabel TxtSellNum;
  [SerializeField]
  private UILabel TxtTotalSellValue;
  [SerializeField]
  private UILabel TxtCostNum;
  [SerializeField]
  private UILabel TxtSelectMax;
  [SerializeField]
  private GameObject dyn_facility_thumb_container;
  [SerializeField]
  private UILabel TxtHP;
  [SerializeField]
  private UILabel TxtPhysicalDef;
  [SerializeField]
  private UILabel TxtMagicDef;
  [SerializeField]
  private List<GameObject> passageIcon;
  [SerializeField]
  private List<GameObject> destructionIcon;
  [SerializeField]
  private List<GameObject> visivilityIcon;
  [SerializeField]
  private int depthProperty = 11;
  [SerializeField]
  private GameObject lnkProperty1;
  [SerializeField]
  private GameObject lnkProperty2;
  private GameObject prefabGenre;
  private UI2DSprite LinkItem;
  private int facilityZenny;
  private int facilityCount;
  private int totalFacility;
  private int totalMedal;
  private PlayerGuildFacility facility;
  private Action<int, int, int> actionSell;
  private GameObject CommonYesNoPopup;
  private int setCnt;
  private bool isPush;

  private IEnumerator SetIcon(PlayerGuildFacility facility)
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/FacilityIcon/dir_facility_thumb").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = prefabF.Result.Clone(this.dyn_facility_thumb_container.transform).GetComponent<FacilityIcon>().SetUnit(PlayerUnit.FromFacility(facility.unit), true, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetFacilityIcon(this.passageIcon, facility.master.is_puton);
    this.SetFacilityIcon(this.destructionIcon, facility.master.is_target);
    this.SetFacilityIcon(this.visivilityIcon, facility.master.is_view);
    UnitUnit unit = facility.unit;
    BattleskillSkill battleskill = (unit != null ? ((IEnumerable<PlayerUnitSkills>) unit.facilitySkills).FirstOrDefault<PlayerUnitSkills>() : (PlayerUnitSkills) null)?.skill;
    if (battleskill != null && (battleskill.genre1.HasValue || battleskill.genre2.HasValue))
    {
      if (Object.op_Equality((Object) this.prefabGenre, (Object) null))
      {
        Future<GameObject> ldPrefab = Res.Icons.SkillGenreIcon.Load<GameObject>();
        e = ldPrefab.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.prefabGenre = ldPrefab.Result;
        ldPrefab = (Future<GameObject>) null;
      }
      BattleskillGenre? genre1 = battleskill.genre1;
      if (genre1.HasValue)
      {
        GameObject gameObject = this.prefabGenre.Clone(this.lnkProperty1.transform);
        UIWidget component1 = this.lnkProperty1.GetComponent<UIWidget>();
        SkillGenreIcon component2 = gameObject.GetComponent<SkillGenreIcon>();
        component2.Init(genre1);
        if (Object.op_Inequality((Object) component1, (Object) null))
          component2.SetSize(Mathf.RoundToInt(component1.localSize.x), Mathf.RoundToInt(component1.localSize.y));
        NGUITools.AdjustDepth(gameObject, this.depthProperty);
      }
      BattleskillGenre? genre2 = battleskill.genre2;
      if (genre2.HasValue)
      {
        GameObject gameObject = this.prefabGenre.Clone(this.lnkProperty2.transform);
        UIWidget component3 = this.lnkProperty2.GetComponent<UIWidget>();
        SkillGenreIcon component4 = gameObject.GetComponent<SkillGenreIcon>();
        component4.Init(genre2);
        if (Object.op_Inequality((Object) component3, (Object) null))
          component4.SetSize(Mathf.RoundToInt(component3.localSize.x), Mathf.RoundToInt(component3.localSize.y));
        NGUITools.AdjustDepth(gameObject, this.depthProperty);
      }
    }
  }

  private void SetFacilityIcon(List<GameObject> iconObj, bool flg)
  {
    if (iconObj != null && iconObj.Count < 2)
      return;
    iconObj[0].SetActive(flg);
    iconObj[1].SetActive(!flg);
  }

  protected override void Update()
  {
    this.facilityCount = (int) ((double) ((UIProgressBar) this.slider).value * (double) this.totalFacility);
    if (this.totalFacility <= 1 && (double) ((UIProgressBar) this.slider).value < 1.0)
    {
      if ((double) ((UIProgressBar) this.slider).value >= 0.0099999997764825821)
        this.facilityCount = 1;
    }
    else if (this.facilityCount > this.totalFacility)
    {
      this.facilityCount = this.totalFacility;
      ((UIProgressBar) this.slider).value = (float) this.facilityCount / (float) this.totalFacility;
    }
    this.TxtSellNum.SetTextLocalize(this.facilityCount);
    this.totalMedal = this.facilityZenny * this.facilityCount;
    this.TxtTotalSellValue.SetTextLocalize(this.totalMedal);
  }

  public IEnumerator InitializeAsync(
    PlayerGuildFacility facility,
    Guild0282Menu menu,
    int setCnt,
    Action<int, int, int> actionSell)
  {
    GuildFacilitySellNumEnterPopup sellNumEnterPopup = this;
    if (Object.op_Inequality((Object) ((Component) sellNumEnterPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) sellNumEnterPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    IEnumerator e = sellNumEnterPopup.SetIcon(facility);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sellNumEnterPopup.facility = facility;
    sellNumEnterPopup.setCnt = setCnt;
    sellNumEnterPopup.guildMapMenu = menu;
    sellNumEnterPopup.Set(facility);
    sellNumEnterPopup.actionSell = actionSell;
    sellNumEnterPopup.isPush = false;
  }

  public void Set(PlayerGuildFacility facility)
  {
    this.totalFacility = facility.hasnum - this.setCnt;
    this.facilityZenny = facility.master.sell_price;
    this.TxtFacilityName.SetText(facility.unit.name);
    this.TxtFacilityDescription.SetText(facility.unit.description);
    this.TxtPossessionNum.SetTextLocalize(facility.hasnum);
    this.TxtSellNum.SetTextLocalize(this.totalFacility);
    this.TxtSelectMax.SetTextLocalize(this.totalFacility);
    this.TxtSellValue.SetTextLocalize(this.facilityZenny);
    this.TxtCostNum.SetTextLocalize(facility.unit.cost);
    this.TxtTotalSellValue.SetTextLocalize(this.totalFacility * this.facilityZenny);
    ((UIProgressBar) this.slider).value = 1f;
    Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(PlayerUnit.FromFacility(facility.unit, 1));
    this.TxtHP.SetTextLocalize(nonBattleParameter.Hp);
    this.TxtPhysicalDef.SetTextLocalize(nonBattleParameter.PhysicalDefense);
    this.TxtMagicDef.SetTextLocalize(nonBattleParameter.MagicDefense);
  }

  public void IbtnPopupOk()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    if (this.facilityCount <= 0)
    {
      Singleton<PopupManager>.GetInstance().dismiss();
      this.guildMapMenu.IsPush = false;
    }
    else
      PopupCommonNoYes.Show(Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_TITLE, Consts.Format(Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_DESC, (IDictionary) new Hashtable()
      {
        {
          (object) "name",
          (object) this.facility.unit.name
        },
        {
          (object) "amount",
          (object) this.facilityCount
        },
        {
          (object) "price",
          (object) this.totalMedal
        }
      }), (Action) (() =>
      {
        if (this.actionSell == null)
          this.isPush = false;
        else
          this.actionSell(this.facilityCount, this.facility.master.ID, this.totalMedal);
      }), (Action) (() => this.isPush = false));
  }

  public override void onBackButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
