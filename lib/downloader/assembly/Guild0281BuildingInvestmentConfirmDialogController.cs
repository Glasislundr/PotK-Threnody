// Decompiled with JetBrains decompiler
// Type: Guild0281BuildingInvestmentConfirmDialogController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using UnityEngine;

#nullable disable
public class Guild0281BuildingInvestmentConfirmDialogController : MonoBehaviour
{
  [SerializeField]
  private UILabel investmentDescriptionLabel;
  [SerializeField]
  private UILabel rankBeforeLabel;
  [SerializeField]
  private UILabel rankAfterLabel;
  [SerializeField]
  private UILabel status1BeforeTitleLabel;
  [SerializeField]
  private UILabel status2BeforeTitleLabel;
  [SerializeField]
  private UILabel status1AfterTitleLabel;
  [SerializeField]
  private UILabel status2AfterTitleLabel;
  [SerializeField]
  private UILabel status1BeforeLabel;
  [SerializeField]
  private UILabel status2BeforeLabel;
  [SerializeField]
  private UILabel status1AfterLabel;
  [SerializeField]
  private UILabel status2AfterLabel;
  [SerializeField]
  private GameObject moneyNotEnoughCaution;
  [SerializeField]
  private UILabel priceLabel;
  [SerializeField]
  private UILabel possessionLabel;
  [SerializeField]
  private UIButton confirmButton;
  private GuildHq selectedFacility;
  private Guild0281FacilityInfoController facilityInfoController;
  private bool isProcessingInvestment;

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void Initialize(
    GuildHq selectedFacility,
    Guild0281FacilityInfoController facilityInfoController)
  {
    this.selectedFacility = selectedFacility;
    this.facilityInfoController = facilityInfoController;
    this.investmentDescriptionLabel.SetTextLocalize(selectedFacility.base_name + Consts.GetInstance().Guild0281MENU_FACILITY_INVESTMENT_DIALOG_DESCRIPTION);
    this.rankBeforeLabel.SetTextLocalize(selectedFacility.rank);
    this.rankAfterLabel.SetTextLocalize(selectedFacility.rank + 1);
    string text1;
    string text2;
    GuildBaseBonusType? nullable1;
    GuildBaseBonusType? nullable2;
    switch (selectedFacility.base_type)
    {
      case GuildBaseType.walls:
        text1 = Consts.GetInstance().Guild0281MENU_FACILITY_STATUS_TITLE_HIT_POINT;
        text2 = "";
        nullable1 = new GuildBaseBonusType?(GuildBaseBonusType.hit_point);
        nullable2 = new GuildBaseBonusType?();
        break;
      case GuildBaseType.tower:
        text1 = Consts.GetInstance().Guild0281MENU_FACILITY_STATUS_TITLE_PHYSICAL_ATK;
        text2 = Consts.GetInstance().Guild0281MENU_FACILITY_STATUS_TITLE_MAGICAL_ATK;
        nullable1 = new GuildBaseBonusType?(GuildBaseBonusType.physical_attack);
        nullable2 = new GuildBaseBonusType?(GuildBaseBonusType.magic_attack);
        break;
      case GuildBaseType.scaffold:
        text1 = Consts.GetInstance().Guild0281MENU_FACILITY_STATUS_TITLE_ACCURACY_RATE;
        text2 = Consts.GetInstance().Guild0281MENU_FACILITY_STATUS_TITLE_AVOIDANCE;
        nullable1 = new GuildBaseBonusType?(GuildBaseBonusType.accuracy_rate);
        nullable2 = new GuildBaseBonusType?(GuildBaseBonusType.avoidance);
        break;
      default:
        Debug.LogError((object) "The type of the selected facility does not exist!");
        return;
    }
    this.status1BeforeTitleLabel.SetTextLocalize(text1);
    this.status1AfterTitleLabel.SetTextLocalize(text1);
    this.status2BeforeTitleLabel.SetTextLocalize(text2);
    this.status2AfterTitleLabel.SetTextLocalize(text2);
    GuildBaseBonusType? nullable3;
    foreach (GuildBaseBonus bonuse in selectedFacility.bonuses)
    {
      int bonusType1 = (int) bonuse.bonus_type;
      nullable3 = nullable1;
      int valueOrDefault1 = (int) nullable3.GetValueOrDefault();
      if (bonusType1 == valueOrDefault1 & nullable3.HasValue)
      {
        this.status1BeforeLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
      }
      else
      {
        int bonusType2 = (int) bonuse.bonus_type;
        nullable3 = nullable2;
        int valueOrDefault2 = (int) nullable3.GetValueOrDefault();
        if (bonusType2 == valueOrDefault2 & nullable3.HasValue)
          this.status2BeforeLabel.SetTextLocalize(this.GetFormattedStatusValue(bonuse.bonus_amount));
      }
    }
    foreach (GuildBaseBonus guildBaseBonus in MasterData.GuildBaseBonusList)
    {
      if (guildBaseBonus.base_rank == selectedFacility.rank + 1)
      {
        int bonusType3 = (int) guildBaseBonus.bonus_type;
        nullable3 = nullable1;
        int valueOrDefault3 = (int) nullable3.GetValueOrDefault();
        if (bonusType3 == valueOrDefault3 & nullable3.HasValue)
        {
          this.status1AfterLabel.SetTextLocalize(this.GetFormattedStatusValue(guildBaseBonus.bonus_amount));
        }
        else
        {
          int bonusType4 = (int) guildBaseBonus.bonus_type;
          nullable3 = nullable2;
          int valueOrDefault4 = (int) nullable3.GetValueOrDefault();
          if (bonusType4 == valueOrDefault4 & nullable3.HasValue)
            this.status2AfterLabel.SetTextLocalize(this.GetFormattedStatusValue(guildBaseBonus.bonus_amount));
        }
      }
    }
    if (!nullable2.HasValue)
    {
      ((Component) this.status2BeforeTitleLabel).gameObject.SetActive(false);
      ((Component) this.status2AfterTitleLabel).gameObject.SetActive(false);
      ((Component) this.status2AfterLabel).gameObject.SetActive(false);
      ((Component) this.status2BeforeLabel).gameObject.SetActive(false);
    }
    int nextPrice = selectedFacility.next_price;
    int money = PlayerAffiliation.Current.guild.money;
    this.priceLabel.SetTextLocalize(nextPrice);
    this.possessionLabel.SetTextLocalize(money);
    if (money < nextPrice)
    {
      ((UIButtonColor) this.confirmButton).isEnabled = false;
      ((UIWidget) this.possessionLabel).color = Color.red;
      this.moneyNotEnoughCaution.SetActive(true);
    }
    else
    {
      ((UIButtonColor) this.confirmButton).isEnabled = true;
      ((UIWidget) this.possessionLabel).color = Color.white;
      this.moneyNotEnoughCaution.SetActive(false);
    }
  }

  public void OnConfirmButtonClicked()
  {
    if (this.isProcessingInvestment)
      return;
    this.isProcessingInvestment = true;
    this.facilityInfoController.InvestFacility(this.selectedFacility);
  }

  public void OnCancelButtonClicked() => Singleton<PopupManager>.GetInstance().dismiss();

  private string GetFormattedStatusValue(int value, bool isValueOnly = false)
  {
    return !isValueOnly ? string.Format("{0}%UP", (object) value) : value.ToString();
  }
}
