// Decompiled with JetBrains decompiler
// Type: TalkUnitInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class TalkUnitInfo
{
  public UnitUnit unit;
  public CallCharacter callCharacter;
  public SeaDateHitExpansionLottery seaDateHit;
  public SeaDateDateSpotDisplaySetting seaDateSpotSettings;
  public List<SeaPresentPresentAffinity> presentAffinities;
  public List<CallUnitGroup> callUnitGroups = new List<CallUnitGroup>();
  public CallItem countryItem;
  public GearGear countryGear;
  public List<CallItem> otherCountryItems = new List<CallItem>();
  public List<GearGear> otherCountryGears = new List<GearGear>();
  public GearGear rareMaterialGear;
  public CallItem elementStoneItem;
  public GearGear elementStoneGear;
  public GearGear elementPowderGear;
  public GearGear elementJewelryGear;
  public GearGear elementFlowerGear;
  public GearGear rarePresentGear;
  public GearGear reverseRarePresentGear;

  public void Init(int sameCharacterId)
  {
    this.unit = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).First<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == sameCharacterId));
    this.callCharacter = ((IEnumerable<CallCharacter>) MasterData.CallCharacterList).First<CallCharacter>((Func<CallCharacter, bool>) (x => x.same_character_id == sameCharacterId));
    IEnumerable<SeaDateHitExpansionLottery> source1 = ((IEnumerable<SeaDateHitExpansionLottery>) MasterData.SeaDateHitExpansionLotteryList).Where<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x => x.time_zone_id_SeaHomeTimeZone != 0 && x.date_spot_id_SeaDateDateSpot != 0));
    this.seaDateHit = source1.FirstOrDefault<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x =>
    {
      int? characterIdUnitUnit = x.same_character_id_UnitUnit;
      int sameCharacterId1 = this.callCharacter.same_character_id;
      return characterIdUnitUnit.GetValueOrDefault() == sameCharacterId1 & characterIdUnitUnit.HasValue;
    })) ?? source1.First<SeaDateHitExpansionLottery>((Func<SeaDateHitExpansionLottery, bool>) (x =>
    {
      int? characterId = x.character_id;
      int characterUnitCharacter = this.unit.character_UnitCharacter;
      return characterId.GetValueOrDefault() == characterUnitCharacter & characterId.HasValue;
    }));
    this.seaDateSpotSettings = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).First<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x => x.datespot == this.seaDateHit.date_spot_id));
    SeaPresentAffinity affinity;
    MasterData.SeaPresentAffinity.TryGetValue(2, out affinity);
    IEnumerable<SeaPresentPresentAffinity> source2 = ((IEnumerable<SeaPresentPresentAffinity>) MasterData.SeaPresentPresentAffinityList).Where<SeaPresentPresentAffinity>((Func<SeaPresentPresentAffinity, bool>) (x => x.affinity == affinity));
    IEnumerable<SeaPresentPresentAffinity> source3 = source2.Where<SeaPresentPresentAffinity>((Func<SeaPresentPresentAffinity, bool>) (x =>
    {
      int? characterIdUnitUnit = x.same_character_id_UnitUnit;
      int sameCharacterId2 = this.callCharacter.same_character_id;
      return characterIdUnitUnit.GetValueOrDefault() == sameCharacterId2 & characterIdUnitUnit.HasValue;
    }));
    this.presentAffinities = source3.Count<SeaPresentPresentAffinity>() > 0 ? source3.ToList<SeaPresentPresentAffinity>() : source2.Where<SeaPresentPresentAffinity>((Func<SeaPresentPresentAffinity, bool>) (x =>
    {
      int? characterId = x.character_id;
      int characterUnitCharacter = this.unit.character_UnitCharacter;
      return characterId.GetValueOrDefault() == characterUnitCharacter & characterId.HasValue;
    })).ToList<SeaPresentPresentAffinity>();
    IEnumerable<CallItem> source4 = ((IEnumerable<CallItem>) MasterData.CallItemList).Where<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 1));
    CallItem callItem1 = source4.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_sub_category_id == 2));
    CallItem callItem2 = source4.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_sub_category_id == 3));
    CallItem callItem3 = source4.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_sub_category_id == 4));
    CallItem callItem4 = source4.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_sub_category_id == 5));
    UnitGroup unitGroup = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == this.unit.ID));
    switch (unitGroup.group_small_category_id.ID)
    {
      case 7:
        this.countryItem = callItem1;
        this.otherCountryItems.Add(callItem2);
        this.otherCountryItems.Add(callItem3);
        this.otherCountryItems.Add(callItem4);
        break;
      case 8:
        this.countryItem = callItem2;
        this.otherCountryItems.Add(callItem1);
        this.otherCountryItems.Add(callItem3);
        this.otherCountryItems.Add(callItem4);
        break;
      case 9:
        this.countryItem = callItem3;
        this.otherCountryItems.Add(callItem1);
        this.otherCountryItems.Add(callItem2);
        this.otherCountryItems.Add(callItem4);
        break;
      default:
        this.countryItem = callItem4;
        this.otherCountryItems.Add(callItem1);
        this.otherCountryItems.Add(callItem2);
        this.otherCountryItems.Add(callItem3);
        break;
    }
    MasterData.GearGear.TryGetValue(this.countryItem.ID, out this.countryGear);
    foreach (CallItem otherCountryItem in this.otherCountryItems)
    {
      GearGear gearGear;
      MasterData.GearGear.TryGetValue(otherCountryItem.ID, out gearGear);
      this.otherCountryGears.Add(gearGear);
    }
    MasterData.GearGear.TryGetValue(((IEnumerable<CallItem>) MasterData.CallItemList).First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 6)).ID, out this.rareMaterialGear);
    IEnumerable<CallItem> source5 = ((IEnumerable<CallItem>) MasterData.CallItemList).Where<CallItem>((Func<CallItem, bool>) (x => (CommonElement) x.elemental_id == this.unit.GetElement()));
    this.elementStoneItem = source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 7));
    MasterData.GearGear.TryGetValue(this.elementStoneItem.ID, out this.elementStoneGear);
    MasterData.GearGear.TryGetValue(source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 8)).ID, out this.elementPowderGear);
    MasterData.GearGear.TryGetValue(source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 9)).ID, out this.elementJewelryGear);
    MasterData.GearGear.TryGetValue(source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 10)).ID, out this.elementFlowerGear);
    MasterData.GearGear.TryGetValue(source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 11 && x.call_item_sub_category_id == this.callCharacter.like_item_id)).ID, out this.rarePresentGear);
    CallItemCategory dislike_item_id = CallItemCategory.Beautiful;
    switch (this.callCharacter.like_item_id)
    {
      case 12:
        dislike_item_id = CallItemCategory.Tiny;
        break;
      case 13:
        dislike_item_id = CallItemCategory.Cool;
        break;
      case 14:
        dislike_item_id = CallItemCategory.OnlyOne;
        break;
      case 15:
        dislike_item_id = CallItemCategory.Beautiful;
        break;
      default:
        Debug.LogError((object) string.Format("想定外のlike_item_idです {0}", (object) (CallItemCategory) this.callCharacter.like_item_id));
        break;
    }
    MasterData.GearGear.TryGetValue(source5.First<CallItem>((Func<CallItem, bool>) (x => x.call_item_category_id == 11 && (CallItemCategory) x.call_item_sub_category_id == dislike_item_id)).ID, out this.reverseRarePresentGear);
    if (SeaTalkCommon.CallUnitGroupLarge.ContainsKey(unitGroup.group_large_category_id_UnitGroupLargeCategory))
      this.callUnitGroups.Add(SeaTalkCommon.CallUnitGroupLarge[unitGroup.group_large_category_id_UnitGroupLargeCategory]);
    if (SeaTalkCommon.CallUnitGroupSmall.ContainsKey(unitGroup.group_small_category_id_UnitGroupSmallCategory))
      this.callUnitGroups.Add(SeaTalkCommon.CallUnitGroupSmall[unitGroup.group_small_category_id_UnitGroupSmallCategory]);
    if (SeaTalkCommon.CallUnitGroupClothing.ContainsKey(unitGroup.group_clothing_category_id_UnitGroupClothingCategory))
      this.callUnitGroups.Add(SeaTalkCommon.CallUnitGroupClothing[unitGroup.group_clothing_category_id_UnitGroupClothingCategory]);
    if (SeaTalkCommon.CallUnitGroupClothing.ContainsKey(unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory))
      this.callUnitGroups.Add(SeaTalkCommon.CallUnitGroupClothing[unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory]);
    if (!SeaTalkCommon.CallUnitGroupGeneration.ContainsKey(unitGroup.group_generation_category_id_UnitGroupGenerationCategory))
      return;
    this.callUnitGroups.Add(SeaTalkCommon.CallUnitGroupGeneration[unitGroup.group_generation_category_id_UnitGroupGenerationCategory]);
  }
}
