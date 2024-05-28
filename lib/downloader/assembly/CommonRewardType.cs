// Decompiled with JetBrains decompiler
// Type: CommonRewardType
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
public class CommonRewardType
{
  public readonly int type_;
  public readonly int id_;
  public readonly int quantity_;
  public readonly bool is_new_;
  public readonly PlayerUnit unit;
  public readonly PlayerMaterialUnit materialUnit;
  public readonly PlayerItem gear;
  public readonly PlayerMaterialGear materialGear;
  public readonly GearGear gearGear;
  public readonly UnitUnit unitUnit;
  public readonly SupplySupply supplySupply;
  public readonly bool isUnit;
  public readonly bool isMaterialUnit;
  public readonly bool isGear;
  public readonly bool isMaterialGear;
  public readonly bool isSupply;
  public GameObject Icon;
  public global::UnitIcon UnitIconScript;
  public global::ItemIcon ItemIconScript;
  private readonly bool isTemporary;

  public CommonRewardType(
    int type,
    int id,
    int quantity,
    bool is_new = false,
    bool is_reserves = false,
    bool is_temporary = false)
  {
    this.type_ = type;
    this.id_ = id;
    this.quantity_ = quantity;
    this.is_new_ = is_new;
    this.isTemporary = is_temporary;
    this.isUnit = this.type_ == 1;
    this.isMaterialUnit = this.type_ == 24;
    this.isGear = this.type_ == 3;
    this.isMaterialGear = type == 26 || type == 35;
    this.isSupply = this.type_ == 2;
    if (this.isUnit)
    {
      this.unit = Array.Find<PlayerUnit>(!is_reserves ? SMManager.Get<PlayerUnit[]>() : GachaResultData.GetInstance().GetData().GetPlayerUnitReserves(), (Predicate<PlayerUnit>) (Unit => Unit.id == id));
      MasterData.UnitUnit.TryGetValue(id, out this.unitUnit);
    }
    else if (this.isMaterialUnit)
    {
      this.materialUnit = this.isTemporary ? PlayerMaterialUnit.CreateByUnitId(id, quantity) : Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (Unit => Unit.id == id));
      MasterData.UnitUnit.TryGetValue(id, out this.unitUnit);
    }
    else if (this.isGear)
    {
      this.gear = Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (Gear => Gear.id == id));
      MasterData.GearGear.TryGetValue(id, out this.gearGear);
    }
    else if (this.isMaterialGear)
    {
      this.materialGear = Array.Find<PlayerMaterialGear>(SMManager.Get<PlayerMaterialGear[]>(), (Predicate<PlayerMaterialGear>) (Gear => Gear.id == id));
      MasterData.GearGear.TryGetValue(id, out this.gearGear);
    }
    else
    {
      if (!this.isSupply)
        return;
      this.supplySupply = MasterData.SupplySupply[id];
    }
  }

  public void ThenUnit(Action<PlayerUnit> callback)
  {
    if (!this.isUnit)
      return;
    callback(this.unit);
  }

  public void ThenMaterialUnit(Action<PlayerMaterialUnit> callback)
  {
    if (!this.isMaterialUnit)
      return;
    callback(this.materialUnit);
  }

  public void ThenGear(Action<PlayerItem> callback)
  {
    if (!this.isGear)
      return;
    callback(this.gear);
  }

  public void ThenMaterialGear(Action<PlayerMaterialGear> callback)
  {
    if (!this.isMaterialGear)
      return;
    callback(this.materialGear);
  }

  public void UnitIcon(Action<global::UnitIcon> callback)
  {
    if (!Object.op_Inequality((Object) this.UnitIconScript, (Object) null))
      return;
    callback(this.UnitIconScript);
  }

  public void ItemIcon(Action<global::ItemIcon> callback)
  {
    if (!Object.op_Inequality((Object) this.ItemIconScript, (Object) null))
      return;
    callback(this.ItemIconScript);
  }

  public void ThenSupply(Action<SupplySupply> callback)
  {
    if (!this.isGear)
      return;
    callback(this.supplySupply);
  }

  public GameObject GetIcon() => this.Icon;

  public global::UnitIcon GetUnitIcon() => this.UnitIconScript;

  public global::ItemIcon GetItemIcon() => this.ItemIconScript;

  public IEnumerator CreateIcon(Transform parent)
  {
    Future<GameObject> PrefabF;
    IEnumerator e;
    if (this.isUnit || this.isMaterialUnit)
    {
      PrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = PrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Icon = PrefabF.Result.Clone(parent);
      this.UnitIconScript = this.Icon.GetComponent<global::UnitIcon>();
      this.UnitIconScript.NewUnit = this.is_new_;
      PrefabF = (Future<GameObject>) null;
    }
    if (this.isGear || this.isMaterialGear)
    {
      PrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = PrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.Icon = PrefabF.Result.Clone(parent);
      this.ItemIconScript = this.Icon.GetComponent<global::ItemIcon>();
      this.ItemIconScript.NewItem = this.is_new_;
      PrefabF = (Future<GameObject>) null;
    }
  }

  public static string GetRewardName(MasterDataTable.CommonRewardType type, int id, int quantity, bool isGuild = false)
  {
    string empty1 = string.Empty;
    string rewardName;
    switch (type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_UNIT, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.UnitUnit[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.supply:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_SUPPLY, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.SupplySupply[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_GEAR, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.GearGear[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.money:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_ZENY, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.player_exp:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_EXP, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.unit_exp:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_UNITEXP, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.coin:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_STONE, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.recover:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_AP_RECOVER, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.max_unit:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_MAX_UNIT, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.max_item:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_MAX_ITEM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.medal:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.emblem:
        if (isGuild)
        {
          rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_EMBLEM, (IDictionary) new Hashtable()
          {
            {
              (object) "Name",
              (object) MasterData.GuildEmblemUnit[id].name
            }
          });
          break;
        }
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_EMBLEM, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.EmblemEmblem[id].name
          }
        });
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_BATTLE_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.cp_recover:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_CP_RECOVER, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        if (!MasterData.QuestkeyQuestkey.ContainsKey(id))
        {
          rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_KEY_DEFAULT, (IDictionary) new Hashtable()
          {
            {
              (object) "Count",
              (object) quantity
            }
          });
          break;
        }
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_KEY, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.QuestkeyQuestkey[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.gacha_ticket:
        string empty2 = string.Empty;
        Hashtable args1 = new Hashtable()
        {
          {
            (object) "Name",
            (object) (!MasterData.GachaTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_GACHATICKET_COMMONNAME : MasterData.GachaTicket[id].short_name)
          },
          {
            (object) "Count",
            (object) quantity
          }
        };
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_GACHATICKET_NAME, (IDictionary) args1);
        break;
      case MasterDataTable.CommonRewardType.season_ticket:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_SEASONTICKET, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.mp_recover:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_MP_RECOVER, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.unit_ticket:
        string empty3 = string.Empty;
        Hashtable args2 = new Hashtable()
        {
          {
            (object) "Name",
            (object) (!MasterData.SelectTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_UNITTICKET_COMMONNAME : MasterData.SelectTicket[id].short_name)
          },
          {
            (object) "Count",
            (object) quantity
          }
        };
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_UNITTICKET_NAME, (IDictionary) args2);
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_TRUST_SKILL, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.BattleskillSkill[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.dp_recover:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_DP_RECOVER, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.guild_medal:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_GUILD_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.guild_town:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_GUILD_TOWN, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.MapTown[id].name
          }
        });
        break;
      case MasterDataTable.CommonRewardType.guild_facility:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_GUILD_FACILITY, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) CommonRewardType.GetFacilityRewardName(id)
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
        string empty4 = string.Empty;
        Hashtable args3 = new Hashtable()
        {
          {
            (object) "Name",
            (object) (!MasterData.UnitTypeTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_UNITTYPETICKET_COMMONNAME : MasterData.UnitTypeTicket[id].name)
          },
          {
            (object) "Count",
            (object) quantity
          }
        };
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_UNITTYPETICKET_NAME, (IDictionary) args3);
        break;
      case MasterDataTable.CommonRewardType.gear_body:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_MATERIAL_GEAR, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.GearGear[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.raid_medal:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_RAID_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.recovery_item:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_AP_RECOVERY_ITEM, (IDictionary) new Hashtable()
        {
          {
            (object) "Name",
            (object) MasterData.RecoveryItemAPHeal[id].name
          },
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        string empty5 = string.Empty;
        Hashtable args4 = new Hashtable()
        {
          {
            (object) "Name",
            (object) (!MasterData.CommonTicket.ContainsKey(id) ? Consts.GetInstance().UNIQUE_ICON_OTHER : MasterData.CommonTicket[id].name)
          },
          {
            (object) "Count",
            (object) quantity
          }
        };
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_COMMON_TICKET, (IDictionary) args4);
        break;
      default:
        rewardName = Consts.Format(Consts.GetInstance().MYPAGE_0017_OTHER, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
    }
    return rewardName;
  }

  public static string GetRewardName(MasterDataTable.CommonRewardType type, int id)
  {
    string empty1 = string.Empty;
    string rewardName;
    switch (type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        rewardName = MasterData.UnitUnit[id].name;
        break;
      case MasterDataTable.CommonRewardType.supply:
        rewardName = MasterData.SupplySupply[id].name;
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        rewardName = MasterData.GearGear[id].name;
        break;
      case MasterDataTable.CommonRewardType.money:
        rewardName = Consts.GetInstance().UNIQUE_ICON_ZENY;
        break;
      case MasterDataTable.CommonRewardType.player_exp:
        rewardName = Consts.GetInstance().UNIQUE_ICON_EXP;
        break;
      case MasterDataTable.CommonRewardType.unit_exp:
        rewardName = Consts.GetInstance().UNIQUE_ICON_UNITEXT_LONG;
        break;
      case MasterDataTable.CommonRewardType.coin:
        rewardName = Consts.GetInstance().UNIQUE_ICON_KISEKI;
        break;
      case MasterDataTable.CommonRewardType.recover:
        rewardName = Consts.GetInstance().UNIQUE_ICON_APRECOVER;
        break;
      case MasterDataTable.CommonRewardType.max_unit:
        rewardName = Consts.GetInstance().UNIQUE_ICON_MAXUNIT;
        break;
      case MasterDataTable.CommonRewardType.max_item:
        rewardName = Consts.GetInstance().UNIQUE_ICON_MAXITEM;
        break;
      case MasterDataTable.CommonRewardType.medal:
        rewardName = Consts.GetInstance().UNIQUE_ICON_MEDAL;
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        rewardName = Consts.GetInstance().UNIQUE_ICON_POINT;
        break;
      case MasterDataTable.CommonRewardType.emblem:
        rewardName = MasterData.EmblemEmblem[id].name;
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        rewardName = Consts.GetInstance().UNIQUE_ICON_BATTLE_MEDAL;
        break;
      case MasterDataTable.CommonRewardType.cp_recover:
        rewardName = Consts.GetInstance().UNIQUE_ICON_CPRECOVER;
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        rewardName = MasterData.QuestkeyQuestkey[id].name;
        break;
      case MasterDataTable.CommonRewardType.gacha_ticket:
        string empty2 = string.Empty;
        rewardName = !MasterData.GachaTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_GACHATICKET_COMMONNAME : MasterData.GachaTicket[id].short_name;
        break;
      case MasterDataTable.CommonRewardType.season_ticket:
        rewardName = Consts.GetInstance().UNIQUE_ICON_SEASONTICKET;
        break;
      case MasterDataTable.CommonRewardType.mp_recover:
        rewardName = Consts.GetInstance().UNIQUE_ICON_MPRECOVER;
        break;
      case MasterDataTable.CommonRewardType.unit_ticket:
        string empty3 = string.Empty;
        rewardName = !MasterData.SelectTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_UNITTICKET_COMMONNAME : MasterData.SelectTicket[id].short_name;
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        rewardName = MasterData.BattleskillSkill[id].name;
        break;
      case MasterDataTable.CommonRewardType.dp_recover:
        rewardName = Consts.GetInstance().UNIQUE_ICON_DPRECOVER;
        break;
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
        string empty4 = string.Empty;
        rewardName = !MasterData.UnitTypeTicket.ContainsKey(id) ? Consts.GetInstance().MYPAGE_0017_UNITTYPETICKET_COMMONNAME : MasterData.UnitTypeTicket[id].name;
        break;
      case MasterDataTable.CommonRewardType.raid_medal:
        rewardName = Consts.GetInstance().UNIQUE_ICON_RAID_MEDAL;
        break;
      case MasterDataTable.CommonRewardType.recovery_item:
        rewardName = Consts.GetInstance().UNIQUE_ICON_AP_RECOVERY_ITEM;
        break;
      case MasterDataTable.CommonRewardType.common_ticket:
        string empty5 = string.Empty;
        rewardName = !MasterData.CommonTicket.ContainsKey(id) ? Consts.GetInstance().UNIQUE_ICON_OTHER : MasterData.CommonTicket[id].name;
        break;
      default:
        rewardName = Consts.GetInstance().UNIQUE_ICON_OTHER;
        break;
    }
    return rewardName;
  }

  public static long GetHaveCount(MasterDataTable.CommonRewardType type, int id)
  {
    long haveCount = 0;
    if (type <= MasterDataTable.CommonRewardType.gear)
    {
      if (type != MasterDataTable.CommonRewardType.unit)
      {
        if ((uint) (type - 2) > 1U)
          goto label_14;
      }
      else if (MasterData.UnitUnit[id].IsMaterialUnit)
      {
        PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x._unit == id));
        if (playerMaterialUnit != null)
        {
          haveCount = (long) playerMaterialUnit.quantity;
          goto label_38;
        }
        else
          goto label_38;
      }
      else
      {
        haveCount = (long) SMManager.Get<PlayerUnit[]>().AmountHavingTargetUnit(id, type);
        goto label_38;
      }
    }
    else if (type != MasterDataTable.CommonRewardType.material_unit)
    {
      if (type != MasterDataTable.CommonRewardType.material_gear)
      {
        if (type == MasterDataTable.CommonRewardType.gear_body)
        {
          haveCount = haveCount + (long) SMManager.Get<PlayerItem[]>().AmountHavingTargetItem(id, MasterDataTable.CommonRewardType.gear) + (long) SMManager.Get<PlayerMaterialGear[]>().AmountHavingTargetItem(id);
          goto label_38;
        }
        else
          goto label_14;
      }
    }
    else
    {
      PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x._unit == id));
      if (playerMaterialUnit != null)
      {
        haveCount = (long) playerMaterialUnit.quantity;
        goto label_38;
      }
      else
        goto label_38;
    }
    haveCount = haveCount + (long) SMManager.Get<PlayerItem[]>().AmountHavingTargetItem(id, type) + (long) SMManager.Get<PlayerMaterialGear[]>().AmountHavingTargetItem(id);
    goto label_38;
label_14:
    switch (type - 4)
    {
      case (MasterDataTable.CommonRewardType) 0:
        haveCount = SMManager.Get<Player>().money;
        break;
      case MasterDataTable.CommonRewardType.supply:
        haveCount = (long) SMManager.Get<Player>().exp;
        break;
      case MasterDataTable.CommonRewardType.player_exp:
        haveCount = (long) SMManager.Get<Player>().coin;
        break;
      case MasterDataTable.CommonRewardType.gear_experience_point:
        haveCount = (long) SMManager.Get<Player>().max_units;
        break;
      case MasterDataTable.CommonRewardType.gear_training_point:
        haveCount = (long) SMManager.Get<Player>().max_items;
        break;
      case MasterDataTable.CommonRewardType.coin:
        haveCount = (long) SMManager.Get<Player>().medal;
        break;
      case MasterDataTable.CommonRewardType.recover:
        haveCount = (long) SMManager.Get<Player>().friend_point;
        break;
      case MasterDataTable.CommonRewardType.max_item:
        haveCount = (long) SMManager.Get<Player>().battle_medal;
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).FirstOrDefault<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == id));
        if (playerQuestKey != null)
        {
          haveCount = (long) playerQuestKey.quantity;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.emblem:
        PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).FirstOrDefault<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket.ID == id));
        if (playerGachaTicket != null)
        {
          haveCount = (long) playerGachaTicket.quantity;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        PlayerSeasonTicket playerSeasonTicket = ((IEnumerable<PlayerSeasonTicket>) SMManager.Get<PlayerSeasonTicket[]>()).FirstOrDefault<PlayerSeasonTicket>((Func<PlayerSeasonTicket, bool>) (x => x.season_ticket_id == id));
        if (playerSeasonTicket != null)
        {
          haveCount = (long) playerSeasonTicket.quantity;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        PlayerSelectTicketSummary selectTicketSummary = ((IEnumerable<PlayerSelectTicketSummary>) SMManager.Get<PlayerSelectTicketSummary[]>()).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == id));
        if (selectTicketSummary != null)
        {
          haveCount = (long) selectTicketSummary.quantity;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.stamp:
        haveCount = (long) ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).Count<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.skill_id == id));
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == id));
        if (playerGuildFacility != null)
        {
          haveCount = (long) playerGuildFacility.hasnum;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.dp_recover:
        PlayerUnitTypeTicket playerUnitTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == id));
        if (playerUnitTypeTicket != null)
        {
          haveCount = (long) playerUnitTypeTicket.quantity;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.gear_body:
        PlayerRecoveryItem playerRecoveryItem = ((IEnumerable<PlayerRecoveryItem>) SMManager.Get<PlayerRecoveryItem[]>()).FirstOrDefault<PlayerRecoveryItem>((Func<PlayerRecoveryItem, bool>) (x => x.recovery_item_id == id));
        if (playerRecoveryItem != null)
        {
          haveCount = (long) playerRecoveryItem.quantity;
          break;
        }
        break;
    }
label_38:
    return haveCount;
  }

  public static string GetRewardQuantity(MasterDataTable.CommonRewardType type, int id, int quantity)
  {
    string empty = string.Empty;
    string rewardQuantity;
    switch (type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_UNIT_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.supply:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_SUPPLY_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_GEAR_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.money:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_ZENY_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.player_exp:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_EXP_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.coin:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_STONE_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.recover:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_AP_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.max_unit:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_OTHER_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.max_item:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_OTHER_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.medal:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_MEDAL_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_POINT_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.emblem:
        rewardQuantity = Consts.GetInstance().SPECIAL_SHOP_EMBLEM_NUM;
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_BATTLE_MEDAL_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.cp_recover:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_CP_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_KEY_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.gacha_ticket:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_GACHATICKET_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.season_ticket:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_SEASONTICKETNUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.mp_recover:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_MP_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.unit_ticket:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_UNITTICKET_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_TRUST_SKILL_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.dp_recover:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_DP_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_UNITTYPETICKET_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.raid_medal:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_MEDAL_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      case MasterDataTable.CommonRewardType.recovery_item:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_AP_RECOVERY_ITEM_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
      default:
        rewardQuantity = Consts.Format(Consts.GetInstance().SPECIAL_SHOP_OTHER_NUM, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) quantity
          }
        });
        break;
    }
    return rewardQuantity;
  }

  private static string GetFacilityRewardName(int id)
  {
    MapFacility mapFacility;
    if (MasterData.MapFacility.TryGetValue(id, out mapFacility))
      return mapFacility.name;
    FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.unit_UnitUnit == id));
    return facilityLevel != null ? facilityLevel.facility.name : string.Empty;
  }
}
