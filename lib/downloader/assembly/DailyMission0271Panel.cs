// Decompiled with JetBrains decompiler
// Type: DailyMission0271Panel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class DailyMission0271Panel : MonoBehaviour
{
  [SerializeField]
  public UI2DSprite IconObject;
  [SerializeField]
  public UILabel txtLabel;
  [SerializeField]
  public UILabel txtMissionProgress;
  [SerializeField]
  public GameObject dirClear;
  [SerializeField]
  public GameObject dirPanel;
  [SerializeField]
  public UIButton popupButton;
  [SerializeField]
  public UIButton clearButton;
  private BingoRewardGroup reward;

  public IEnumerator Init(
    DailyMission0271PanelRoot.DailyMissionView viewModel)
  {
    this.reward = viewModel.rewards[0];
    if (viewModel.isClear)
    {
      this.changeClearState();
    }
    else
    {
      this.dirClear.SetActive(false);
      this.dirPanel.SetActive(true);
      this.txtMissionProgress.SetTextLocalize(viewModel.progressText);
    }
    this.txtLabel.SetTextLocalize(viewModel.name);
    IEnumerator e = this.getIconImageAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void changeClearState()
  {
    this.txtMissionProgress.text = "";
    this.dirClear.SetActive(true);
    this.dirPanel.SetActive(false);
  }

  private IEnumerator getIconImageAsync()
  {
    string path;
    switch (this.reward.reward_type_id)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        path = "Icons/Unit_Icon";
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        path = "Icons/Weapon_Icon";
        break;
      case MasterDataTable.CommonRewardType.money:
        path = "Icons/Zeny_Icon";
        break;
      case MasterDataTable.CommonRewardType.coin:
        path = "Icons/Kiseki_Icon";
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        path = "Icons/ManaPoint_Icon";
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        path = "Icons/BattleMedal_Icon";
        break;
      case MasterDataTable.CommonRewardType.gacha_ticket:
        path = "Icons/GachaTicket_Icon";
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        path = MasterData.BattleskillSkill[this.reward.reward_id].getSkillIconPath();
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        CommonTicket commonTicket = MasterData.CommonTicket[this.reward.reward_id];
        path = "Coin/{0}/Coin_S".F((object) (commonTicket.icon_id != 0 ? commonTicket.icon_id : commonTicket.ID));
        break;
      default:
        path = "Icons/Common_Icon";
        break;
    }
    Future<Sprite> future = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.IconObject.sprite2D = future.Result;
  }
}
