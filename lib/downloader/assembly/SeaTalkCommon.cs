// Decompiled with JetBrains decompiler
// Type: SeaTalkCommon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public static class SeaTalkCommon
{
  public const int PROPOSE_ITEM_ID = 14999999;
  public const int DESTRUCTION_ITEM_ID = 14999998;
  public const string TALK_BG_PREFAB_NAME = "HimeTalkBackground";
  public const int TALK_LINE_HEIGHT = 29;
  private const int TALK_LINE_MAX_CHAR_COUNT = 18;
  public static Dictionary<int, CallUnitGroup> CallUnitGroupLarge = new Dictionary<int, CallUnitGroup>();
  public static Dictionary<int, CallUnitGroup> CallUnitGroupSmall = new Dictionary<int, CallUnitGroup>();
  public static Dictionary<int, CallUnitGroup> CallUnitGroupClothing = new Dictionary<int, CallUnitGroup>();
  public static Dictionary<int, CallUnitGroup> CallUnitGroupGeneration = new Dictionary<int, CallUnitGroup>();

  static SeaTalkCommon()
  {
    SeaTalkCommon.CallUnitGroupLarge = ((IEnumerable<CallUnitGroup>) MasterData.CallUnitGroupList).Where<CallUnitGroup>((Func<CallUnitGroup, bool>) (x => x.group_type == 1)).ToDictionary<CallUnitGroup, int, CallUnitGroup>((Func<CallUnitGroup, int>) (x => x.large_category_id), (Func<CallUnitGroup, CallUnitGroup>) (x => x));
    SeaTalkCommon.CallUnitGroupSmall = ((IEnumerable<CallUnitGroup>) MasterData.CallUnitGroupList).Where<CallUnitGroup>((Func<CallUnitGroup, bool>) (x => x.group_type == 2)).ToDictionary<CallUnitGroup, int, CallUnitGroup>((Func<CallUnitGroup, int>) (x => x.small_category_id), (Func<CallUnitGroup, CallUnitGroup>) (x => x));
    SeaTalkCommon.CallUnitGroupClothing = ((IEnumerable<CallUnitGroup>) MasterData.CallUnitGroupList).Where<CallUnitGroup>((Func<CallUnitGroup, bool>) (x => x.group_type == 3)).ToDictionary<CallUnitGroup, int, CallUnitGroup>((Func<CallUnitGroup, int>) (x => x.clothing_category_id), (Func<CallUnitGroup, CallUnitGroup>) (x => x));
    SeaTalkCommon.CallUnitGroupGeneration = ((IEnumerable<CallUnitGroup>) MasterData.CallUnitGroupList).Where<CallUnitGroup>((Func<CallUnitGroup, bool>) (x => x.group_type == 4)).ToDictionary<CallUnitGroup, int, CallUnitGroup>((Func<CallUnitGroup, int>) (x => x.generation_category_id), (Func<CallUnitGroup, CallUnitGroup>) (x => x));
  }

  public static string GetJPWeek(DateTime dt)
  {
    switch (dt.DayOfWeek)
    {
      case DayOfWeek.Sunday:
        return "日";
      case DayOfWeek.Monday:
        return "月";
      case DayOfWeek.Tuesday:
        return "火";
      case DayOfWeek.Wednesday:
        return "水";
      case DayOfWeek.Thursday:
        return "木";
      case DayOfWeek.Friday:
        return "金";
      case DayOfWeek.Saturday:
        return "土";
      default:
        Debug.LogError((object) "存在しない曜日データです");
        return "";
    }
  }

  public static int GetLineCount(string text)
  {
    int num = 1;
    int lineCount = 1;
    for (int index = 0; index < text.Length; ++index)
    {
      if (text[index] == '\n')
      {
        num = 1;
        ++lineCount;
      }
      else if (num >= 18)
      {
        num = 1;
        ++lineCount;
      }
      else
        ++num;
    }
    return lineCount;
  }

  public static int GetHeight(int lineCount) => lineCount * 29;

  public static void ProcessingComment(UILabel label)
  {
    int num1 = ((UIWidget) label).width / (label.fontSize + label.spacingX);
    int num2 = 1;
    List<int> intList = new List<int>();
    for (int index = 0; index < label.text.Length; ++index)
    {
      if (label.text[index] == '\n')
      {
        num2 = 1;
        intList.Add(index);
      }
      else if (num2 >= num1)
      {
        num2 = 1;
        intList.Add(index);
      }
      else if (intList.Count < 2)
        ++num2;
      else
        break;
    }
    if (intList.Count < 2)
      return;
    string str1 = label.text.Remove(intList[1]);
    int num3 = intList[1] - intList[0];
    string str2;
    if (num1 - num3 > 2)
    {
      str2 = str1 + "・・・";
    }
    else
    {
      int num4 = 3 - (num1 - num3);
      str2 = str1.Remove(str1.Length - num4) + "・・・";
    }
    label.text = str2;
  }

  public static string GetReplaceMessage(
    string text,
    TalkUnitInfo talkUnitInfo,
    PlayerTalkMessage playerTalkMessage)
  {
    text = text.Replace("{1人称}", talkUnitInfo.callCharacter.first_person);
    text = text.Replace("{マスター呼称}", talkUnitInfo.callCharacter.master_name);
    if (playerTalkMessage.condition_type.HasValue && playerTalkMessage.condition_id.HasValue)
      text = text.Replace("{要望}", SeaTalkCommon.GetChoiceItemOrGoDatePriceName(playerTalkMessage));
    text = text.Replace("{青デート場所名}", talkUnitInfo.callCharacter.blue_date_place);
    text = text.Replace("{赤デート時間帯}", talkUnitInfo.seaDateHit.time_zone_id.name);
    text = text.Replace("{赤デート場所名}", talkUnitInfo.seaDateSpotSettings.date_name);
    Random random = new Random(playerTalkMessage.player_message_id);
    int presentAffinitieValue = random.Next(0, talkUnitInfo.presentAffinities.Count);
    GearGear gearGear = ((IEnumerable<GearGear>) MasterData.GearGearList).First<GearGear>((Func<GearGear, bool>) (x => x.ID == talkUnitInfo.presentAffinities[presentAffinitieValue].gear_id));
    text = text.Replace("{好きなプレゼント名}", gearGear.name);
    switch (talkUnitInfo.callCharacter.like_item_id)
    {
      case 12:
        text = text.Replace("{好み}", "綺麗なもの");
        break;
      case 13:
        text = text.Replace("{好み}", "可愛いもの");
        break;
      case 14:
        text = text.Replace("{好み}", "格好いいもの");
        break;
      case 15:
        text = text.Replace("{好み}", "個性的なもの");
        break;
      default:
        Debug.LogError((object) string.Format("想定していない「好みの誓約アイテムカテゴリーID」です {0}", (object) talkUnitInfo.callCharacter.like_item_id));
        break;
    }
    int index1 = random.Next(0, talkUnitInfo.otherCountryItems.Count);
    CallItem otherCountryItem = talkUnitInfo.otherCountryItems[index1];
    GearGear otherCountryGear = talkUnitInfo.otherCountryGears[index1];
    text = text.Replace("{国アイテム名}", talkUnitInfo.countryGear.name);
    text = text.Replace("{R素材アイテム名}", talkUnitInfo.rareMaterialGear.name);
    text = text.Replace("{属性石アイテム名}", talkUnitInfo.elementStoneGear.name);
    text = text.Replace("{粉末アイテム名}", talkUnitInfo.elementPowderGear.name);
    text = text.Replace("{宝石アイテム名}", talkUnitInfo.elementJewelryGear.name);
    text = text.Replace("{花アイテム名}", talkUnitInfo.elementFlowerGear.name);
    text = text.Replace("{R贈り物アイテム名}", talkUnitInfo.rarePresentGear.name);
    text = text.Replace("{所属国以外アイテム名}", otherCountryGear.name);
    text = text.Replace("{自属性石アイテム入手ヒント}", talkUnitInfo.elementStoneItem.tips_talk);
    text = text.Replace("{所属国アイテム入手ヒント}", talkUnitInfo.countryItem.tips_talk);
    text = text.Replace("{所属国以外アイテム入手ヒント}", otherCountryItem.tips_talk);
    if (talkUnitInfo.callUnitGroups.Count > 0)
    {
      int index2 = random.Next(0, talkUnitInfo.callUnitGroups.Count);
      CallUnitGroup callUnitGroup;
      MasterData.CallUnitGroup.TryGetValue(talkUnitInfo.callUnitGroups[index2].dislike_group_id, out callUnitGroup);
      switch (callUnitGroup.group_type)
      {
        case 1:
          UnitGroupLargeCategory groupLargeCategory;
          MasterData.UnitGroupLargeCategory.TryGetValue(callUnitGroup.large_category_id, out groupLargeCategory);
          text = text.Replace("{不仲グループ名}", groupLargeCategory.name);
          break;
        case 2:
          UnitGroupSmallCategory groupSmallCategory;
          MasterData.UnitGroupSmallCategory.TryGetValue(callUnitGroup.small_category_id, out groupSmallCategory);
          text = text.Replace("{不仲グループ名}", groupSmallCategory.name);
          break;
        case 3:
          UnitGroupClothingCategory clothingCategory;
          MasterData.UnitGroupClothingCategory.TryGetValue(callUnitGroup.clothing_category_id, out clothingCategory);
          text = text.Replace("{不仲グループ名}", clothingCategory.name);
          break;
        case 4:
          UnitGroupGenerationCategory generationCategory;
          MasterData.UnitGroupGenerationCategory.TryGetValue(callUnitGroup.generation_category_id, out generationCategory);
          text = text.Replace("{不仲グループ名}", generationCategory.name);
          break;
      }
    }
    text = text.Replace("{嫌いなR贈り物名}", talkUnitInfo.reverseRarePresentGear.name);
    return text;
  }

  private static string GetChoiceItemOrGoDatePriceName(PlayerTalkMessage playerTalkMessage)
  {
    int id = playerTalkMessage.condition_id.Value;
    switch ((TalkMessageConditonType) playerTalkMessage.condition_type.Value)
    {
      case TalkMessageConditonType.SendItem:
        GearGear gearGear;
        MasterData.GearGear.TryGetValue(id, out gearGear);
        return gearGear.name;
      case TalkMessageConditonType.GoDate:
      case TalkMessageConditonType.GoDateBlue:
      case TalkMessageConditonType.GoDateRed:
        SeaDateDateSpotDisplaySetting spotDisplaySetting = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).FirstOrDefault<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x => x.datespot.ID == id));
        if (spotDisplaySetting != null)
          return spotDisplaySetting.date_name;
        break;
      default:
        Debug.LogError((object) string.Format("想定していないcondition_typeです: {0}", (object) playerTalkMessage.condition_type));
        break;
    }
    return "{要望}";
  }

  public static void UpdateTalkBatch(PlayerTalkPartner[] playerTalkPartners)
  {
    Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = false;
    foreach (PlayerTalkPartner playerTalkPartner in playerTalkPartners)
    {
      if (playerTalkPartner.unread_count > 0)
      {
        Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = true;
        break;
      }
      if (playerTalkPartner.receivable_reward)
      {
        Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = true;
        break;
      }
    }
    Sea030HomeMenu[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<Sea030HomeMenu>();
    if (objectsOfTypeAll.Length == 0)
      return;
    objectsOfTypeAll[0].UpdateBadgeTalk(SMManager.Get<SeaPlayer>());
  }
}
