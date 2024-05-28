// Decompiled with JetBrains decompiler
// Type: GuildGiftScrollParts
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
public class GuildGiftScrollParts : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject itemIcon;
  [SerializeField]
  private GameObject giftItemPos;
  [SerializeField]
  private GameObject unitIconPos;
  [SerializeField]
  private UILabel playerNameLabel;
  [SerializeField]
  private UILabel itemNameLabel;
  [SerializeField]
  private UILabel LimitTimeLabel;
  [SerializeField]
  private UI2DSprite dynTitle;
  private GuildMemberGift gift;
  private UnitIcon unitIcon;
  [SerializeField]
  private UIButton button;

  public UIButton GetButton() => this.button;

  public IEnumerator Initialize(GuildMemberGift data)
  {
    this.gift = data;
    if (Object.op_Inequality((Object) this.LimitTimeLabel, (Object) null))
    {
      TimeSpan self = data.limit_at - ServerTime.NowAppTime();
      this.LimitTimeLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_GIFT_RECEIVE_LIMIT_TIME, (IDictionary) new Hashtable()
      {
        {
          (object) "time",
          (object) self.DisplayString()
        }
      }));
    }
    Future<Sprite> emblem = EmblemUtility.LoadEmblemSprite(data.player.player_emblem_id);
    IEnumerator e = emblem.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dynTitle.sprite2D = emblem.Result;
    this.playerNameLabel.SetTextLocalize(this.gift.player.player_name);
    if (Object.op_Inequality((Object) this.itemIcon.GetIcon(), (Object) null))
      Object.Destroy((Object) this.itemIcon.GetIcon());
    e = this.itemIcon.CreateThumbnail((MasterDataTable.CommonRewardType) data.gift_reward_type_id, data.gift_reward_id, data.gift_reward_quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (data.gift_reward_type_id)
    {
      case 1:
      case 24:
        UnitIcon component1 = this.itemIcon.GetIcon().GetComponent<UnitIcon>();
        if (Object.op_Inequality((Object) component1, (Object) null))
        {
          component1.setLevelText("1");
          component1.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          this.itemNameLabel.SetTextLocalize(component1.Unit.name);
          break;
        }
        break;
      case 2:
        ItemIcon component2 = this.itemIcon.GetIcon().GetComponent<ItemIcon>();
        if (Object.op_Inequality((Object) component2, (Object) null))
        {
          component2.QuantitySupply = true;
          component2.EnableQuantity(data.gift_reward_quantity);
          SupplySupply supplySupply = (SupplySupply) null;
          if (MasterData.SupplySupply.TryGetValue(this.gift.gift_reward_id, out supplySupply))
          {
            this.itemNameLabel.SetTextLocalize(supplySupply.name);
            break;
          }
          break;
        }
        break;
      case 3:
      case 26:
      case 35:
        ItemIcon component3 = this.itemIcon.GetIcon().GetComponent<ItemIcon>();
        if (Object.op_Inequality((Object) component3, (Object) null))
        {
          component3.QuantitySupply = false;
          GearGear gearGear = (GearGear) null;
          if (MasterData.GearGear.TryGetValue(this.gift.gift_reward_id, out gearGear))
          {
            this.itemNameLabel.SetTextLocalize(gearGear.name);
            break;
          }
          break;
        }
        break;
      default:
        if (Object.op_Inequality((Object) this.itemIcon.GetIcon().GetComponent<UnitIcon>(), (Object) null))
        {
          UnitIcon component4 = this.itemIcon.GetIcon().GetComponent<UnitIcon>();
          component4.setLevelText("1");
          component4.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          this.itemNameLabel.SetTextLocalize(component4.Unit.name);
        }
        if (Object.op_Inequality((Object) this.itemIcon.GetIcon().GetComponent<ItemIcon>(), (Object) null))
        {
          this.itemIcon.GetIcon().GetComponent<ItemIcon>().QuantitySupply = false;
          GearGear gearGear = (GearGear) null;
          if (MasterData.GearGear.TryGetValue(this.gift.gift_reward_id, out gearGear))
            this.itemNameLabel.SetTextLocalize(gearGear.name);
          SupplySupply supplySupply = (SupplySupply) null;
          if (MasterData.SupplySupply.TryGetValue(this.gift.gift_reward_id, out supplySupply))
            this.itemNameLabel.SetTextLocalize(supplySupply.name);
        }
        if (Object.op_Inequality((Object) this.itemIcon.GetIcon().GetComponent<UniqueIcons>(), (Object) null))
        {
          this.itemNameLabel.SetTextLocalize(this.itemIcon.GetIcon().GetComponent<UniqueIcons>().itemName);
          break;
        }
        break;
    }
    Future<GameObject> PrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = PrefabF.Result;
    if (Object.op_Equality((Object) this.unitIcon, (Object) null))
      this.unitIcon = result.Clone(this.unitIconPos.transform).GetComponent<UnitIcon>();
    PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(data.player.leader_unit_unit, data.player.leader_unit_level);
    byUnitunit.job_id = data.player.leader_unit_job_id;
    e = this.unitIcon.SetUnit(byUnitunit, data.player.leader_unit_unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.setLevelText(data.player.leader_unit_level.ToString());
    this.unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.unitIcon.Button.onClick.Clear();
    this.unitIcon.Button.onLongPress.Clear();
  }

  public void FriendDetailScene(GuildMemberGift data)
  {
    Unit0042Scene.changeSceneFriendUnit(true, data.player.player_id, 0);
  }
}
