// Decompiled with JetBrains decompiler
// Type: PopupFacilityDetailMenu
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
public class PopupFacilityDetailMenu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private int depthProperty = 11;
  private Guild0282Menu guildMapMenu;
  [SerializeField]
  private UILabel lblFacilityName;
  [SerializeField]
  private UILabel lblCost;
  [SerializeField]
  private UILabel lblDescription;
  [SerializeField]
  private UILabel lblPossession;
  [SerializeField]
  private List<GameObject> passageIcon;
  [SerializeField]
  private List<GameObject> destructionIcon;
  [SerializeField]
  private List<GameObject> visivilityIcon;
  [SerializeField]
  private UILabel TxtHP;
  [SerializeField]
  private UILabel TxtPhysicalDef;
  [SerializeField]
  private UILabel TxtMagicDef;
  [SerializeField]
  private GameObject dir_facility_thumb_container;
  [SerializeField]
  private GameObject dyn_Skillproperty1;
  [SerializeField]
  private GameObject dyn_Skillproperty2;
  private GameObject sellnumEnterPopup;
  private PlayerGuildFacility facilityInfo;
  private Action<int, int, int> actionSell;
  private bool isPush;
  private GameObject commonOkPopup;
  private GameObject prefabGenre;

  private IEnumerator ShowSellNumEnterPopup()
  {
    PopupFacilityDetailMenu facilityDetailMenu = this;
    int setCnt = 0;
    PlayerGuildTownSlot[] playerGuildTownSlotArray = SMManager.Get<PlayerGuildTownSlot[]>();
    if (playerGuildTownSlotArray != null)
    {
      for (int index = 0; index < playerGuildTownSlotArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        int num = ((IEnumerable<PlayerGuildTownSlotPosition>) playerGuildTownSlotArray[index].facilities_data).Count<PlayerGuildTownSlotPosition>(new Func<PlayerGuildTownSlotPosition, bool>(facilityDetailMenu.\u003CShowSellNumEnterPopup\u003Eb__21_0));
        if (setCnt < num)
          setCnt = num;
      }
      if (facilityDetailMenu.facilityInfo.hasnum == setCnt)
      {
        Singleton<PopupManager>.GetInstance().open(facilityDetailMenu.commonOkPopup).GetComponent<GuildOkPopup>().Initialize(Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_TITLE, Consts.GetInstance().POPUP_GUILD_FACILITY_SELL_ALL_INSTALL);
      }
      else
      {
        GameObject popup = facilityDetailMenu.sellnumEnterPopup.Clone();
        popup.SetActive(false);
        IEnumerator e = popup.GetComponent<GuildFacilitySellNumEnterPopup>().InitializeAsync(facilityDetailMenu.facilityInfo, facilityDetailMenu.guildMapMenu, setCnt, facilityDetailMenu.actionSell);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        popup = (GameObject) null;
      }
      facilityDetailMenu.isPush = false;
    }
  }

  private IEnumerator SetIcon(UnitUnit facilityUnit)
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/FacilityIcon/dir_facility_thumb").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = prefabF.Result.Clone(this.dir_facility_thumb_container.transform).GetComponent<FacilityIcon>().SetUnit(PlayerUnit.FromFacility(facilityUnit), true, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetFacilityIcon(this.passageIcon, facilityUnit.facility.is_puton);
    this.SetFacilityIcon(this.destructionIcon, facilityUnit.facility.is_target);
    this.SetFacilityIcon(this.visivilityIcon, facilityUnit.facility.is_view);
  }

  private void SetFacilityIcon(List<GameObject> iconObj, bool flg)
  {
    if (iconObj != null && iconObj.Count < 2)
      return;
    iconObj[0].SetActive(flg);
    iconObj[1].SetActive(!flg);
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> popup;
    IEnumerator e;
    if (Object.op_Equality((Object) this.commonOkPopup, (Object) null))
    {
      popup = Res.Prefabs.popup.popup_028_guild_common_ok__anim_popup01.Load<GameObject>();
      e = popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.commonOkPopup = popup.Result;
      popup = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.prefabGenre, (Object) null))
    {
      popup = Res.Icons.SkillGenreIcon.Load<GameObject>();
      e = popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabGenre = popup.Result;
      popup = (Future<GameObject>) null;
    }
  }

  public IEnumerator InitializeAsync(int facilityID)
  {
    IEnumerator e = this.CommonInit(facilityID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == facilityID));
    this.lblPossession.SetTextLocalize(playerGuildFacility != null ? playerGuildFacility.hasnum : 0);
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu guildMapMenu,
    PlayerGuildFacility facility,
    GameObject sellNumEnterPopupObj,
    Action<int, int, int> actionSell)
  {
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CommonInit(facility.master.ID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sellnumEnterPopup = sellNumEnterPopupObj;
    this.facilityInfo = facility;
    this.guildMapMenu = guildMapMenu;
    this.lblPossession.SetTextLocalize(facility.hasnum);
    this.actionSell = actionSell;
  }

  public IEnumerator CommonInit(int facilityID)
  {
    PopupFacilityDetailMenu facilityDetailMenu = this;
    if (Object.op_Inequality((Object) ((Component) facilityDetailMenu).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) facilityDetailMenu).GetComponent<UIWidget>()).alpha = 0.0f;
    FacilityLevel facilityData = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Where<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.facility_MapFacility == facilityID)).FirstOrDefault<FacilityLevel>();
    if (facilityData != null)
    {
      IEnumerator e = facilityDetailMenu.SetIcon(facilityData.unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      facilityDetailMenu.lblFacilityName.SetTextLocalize(facilityData.unit.name);
      facilityDetailMenu.lblCost.SetTextLocalize(facilityData.unit.cost);
      facilityDetailMenu.lblDescription.SetText(facilityData.unit.description);
      Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(PlayerUnit.FromFacility(facilityData.unit));
      facilityDetailMenu.TxtHP.SetTextLocalize(nonBattleParameter.Hp);
      facilityDetailMenu.TxtPhysicalDef.SetTextLocalize(nonBattleParameter.PhysicalDefense);
      facilityDetailMenu.TxtMagicDef.SetTextLocalize(nonBattleParameter.MagicDefense);
      BattleskillSkill battleskill = ((IEnumerable<PlayerUnitSkills>) facilityData.unit.facilitySkills).FirstOrDefault<PlayerUnitSkills>()?.skill;
      if (battleskill != null && (battleskill.genre1.HasValue || battleskill.genre2.HasValue))
      {
        if (Object.op_Equality((Object) facilityDetailMenu.prefabGenre, (Object) null))
        {
          Future<GameObject> ldPrefab = Res.Icons.SkillGenreIcon.Load<GameObject>();
          e = ldPrefab.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          facilityDetailMenu.prefabGenre = ldPrefab.Result;
          ldPrefab = (Future<GameObject>) null;
        }
        BattleskillGenre? genre1 = battleskill.genre1;
        if (genre1.HasValue)
        {
          GameObject gameObject = facilityDetailMenu.prefabGenre.Clone(facilityDetailMenu.dyn_Skillproperty1.transform);
          UIWidget component1 = facilityDetailMenu.dyn_Skillproperty1.GetComponent<UIWidget>();
          SkillGenreIcon component2 = gameObject.GetComponent<SkillGenreIcon>();
          component2.Init(genre1);
          if (Object.op_Inequality((Object) component1, (Object) null))
            component2.SetSize(Mathf.RoundToInt(component1.localSize.x), Mathf.RoundToInt(component1.localSize.y));
          NGUITools.AdjustDepth(gameObject, facilityDetailMenu.depthProperty);
        }
        BattleskillGenre? genre2 = battleskill.genre2;
        if (genre2.HasValue)
        {
          GameObject gameObject = facilityDetailMenu.prefabGenre.Clone(facilityDetailMenu.dyn_Skillproperty2.transform);
          UIWidget component3 = facilityDetailMenu.dyn_Skillproperty2.GetComponent<UIWidget>();
          SkillGenreIcon component4 = gameObject.GetComponent<SkillGenreIcon>();
          component4.Init(genre2);
          if (Object.op_Inequality((Object) component3, (Object) null))
            component4.SetSize(Mathf.RoundToInt(component3.localSize.x), Mathf.RoundToInt(component3.localSize.y));
          NGUITools.AdjustDepth(gameObject, facilityDetailMenu.depthProperty);
        }
      }
      facilityDetailMenu.isPush = false;
    }
  }

  public void onSellButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    if (Object.op_Equality((Object) this.sellnumEnterPopup, (Object) null))
      return;
    this.StartCoroutine(this.ShowSellNumEnterPopup());
  }

  public override void onBackButton()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
