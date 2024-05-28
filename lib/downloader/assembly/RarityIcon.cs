// Decompiled with JetBrains decompiler
// Type: RarityIcon
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
public static class RarityIcon
{
  private static readonly string DETAIL_CURRENT_PATH = "Icons/Materials/RarityStars/";
  private static readonly string DETAIL_SLIVER_IMG_NAME = "{0}s_star_silver_{1}{2}";
  private static readonly string DETAIL_GOLD_IMG_NAME = "{0}s_star_gold_{1}{2}";
  private static readonly string DETAIL_RAINBOW_IMG_NAME = "{0}s_star_rainbow_{1}{2}";
  private static readonly string DETAIL_VERTEX_IMG_NAME = "{0}s_star_vertex_{1}{2}";
  private static readonly string CURRENT_PATH = "Prefabs/UnitIcon/Materials/";
  private static readonly string SLIVER_IMG_NAME = "{0}s_star_silver{1:D2}{2:D2}";
  private static readonly string GOLD_IMG_NAME = "{0}s_star_gold{1:D2}{2:D2}";
  private static readonly string RAINBOW_IMG_NAME = "{0}s_star_rainbow{1:D2}{2:D2}";
  private static readonly string VERTEX_IMG_NAME = "{0}s_star_vertex{1:D2}{2:D2}";
  private static readonly string AWAKE_IMG_NAME = "{0}s_star_god";
  private static string[] spriteNameTBL = new string[7]
  {
    "Icons/Materials/RarityStars/slc_star1",
    "Icons/Materials/RarityStars/slc_star2",
    "Icons/Materials/RarityStars/slc_star3",
    "Icons/Materials/RarityStars/slc_star4",
    "Icons/Materials/RarityStars/slc_star5",
    "Icons/Materials/RarityStars/slc_star6",
    "Icons/Materials/RarityStars/slc_star7"
  };

  public static string GetSpriteName(
    PlayerUnit playerUnit,
    bool isDetail = true,
    bool isSame = false,
    bool isAwake = false)
  {
    return RarityIcon._GetSpriteName(playerUnit, playerUnit.unit, isDetail, isSame, isAwake);
  }

  public static string GetSpriteName(UnitUnit unit, bool isDetail = true, bool isSame = false, bool isAwake = false)
  {
    return RarityIcon._GetSpriteName((PlayerUnit) null, unit, isDetail, isSame, isAwake);
  }

  private static string _GetSpriteName(
    PlayerUnit playerUnit,
    UnitUnit unit,
    bool isDetail = true,
    bool isSame = false,
    bool isAwake = false)
  {
    string spriteName = string.Empty;
    int num1 = unit.rarity.index + 1;
    int num2 = num1;
    if (unit.awake_unit_flag & isAwake)
      return string.Format(RarityIcon.AWAKE_IMG_NAME, (object) RarityIcon.CURRENT_PATH);
    if (!isSame)
    {
      UnitUnit[] array = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => unit.same_character_id == x.same_character_id)).OrderByDescending<UnitUnit, int>((Func<UnitUnit, int>) (x => x.rarity.index)).ToArray<UnitUnit>();
      if (array != null)
        num2 = array[0].rarity.index + 1;
    }
    if (playerUnit != (PlayerUnit) null && playerUnit.job_id != 0)
    {
      MasterDataTable.UnitJob unitJob;
      if (!MasterData.UnitJob.TryGetValue(playerUnit.job_id, out unitJob))
        Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) playerUnit.job_id + "]"));
      if (unitJob != null && unitJob.job_rank >= UnitJobRank.vertex && unitJob.job_rank != UnitJobRank.none)
        return !isDetail ? string.Format(RarityIcon.VERTEX_IMG_NAME, (object) RarityIcon.CURRENT_PATH, (object) num1, (object) num2) : string.Format(RarityIcon.DETAIL_VERTEX_IMG_NAME, (object) RarityIcon.DETAIL_CURRENT_PATH, (object) num1, (object) num2);
    }
    switch (num1)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        spriteName = !isDetail ? string.Format(RarityIcon.SLIVER_IMG_NAME, (object) RarityIcon.CURRENT_PATH, (object) num1, (object) num2) : string.Format(RarityIcon.DETAIL_SLIVER_IMG_NAME, (object) RarityIcon.DETAIL_CURRENT_PATH, (object) num1, (object) num2);
        break;
      case 5:
        spriteName = !isDetail ? string.Format(RarityIcon.GOLD_IMG_NAME, (object) RarityIcon.CURRENT_PATH, (object) num1, (object) num2) : string.Format(RarityIcon.DETAIL_GOLD_IMG_NAME, (object) RarityIcon.DETAIL_CURRENT_PATH, (object) num1, (object) num2);
        break;
      case 6:
        spriteName = !isDetail ? string.Format(RarityIcon.RAINBOW_IMG_NAME, (object) RarityIcon.CURRENT_PATH, (object) num1, (object) num2) : string.Format(RarityIcon.DETAIL_RAINBOW_IMG_NAME, (object) RarityIcon.DETAIL_CURRENT_PATH, (object) num1, (object) num2);
        break;
      default:
        Debug.LogError((object) ("想定していないレアリティのため、レアリティ画像名を取得できません: " + (object) num1));
        break;
    }
    return spriteName;
  }

  private static Sprite GetSprite(GearRarity rarity)
  {
    return Resources.Load<Sprite>(RarityIcon.spriteNameTBL[rarity.index - 1]);
  }

  public static Sprite GetSprite(PlayerUnit plyaerUnit, bool isDetail = true, bool isSame = false, bool isAwake = false)
  {
    return Resources.Load<Sprite>(RarityIcon.GetSpriteName(plyaerUnit, isDetail, isSame, isAwake));
  }

  public static Sprite GetSprite(UnitUnit unit, bool isDetail = true, bool isSame = false, bool isAwake = false)
  {
    return Resources.Load<Sprite>(RarityIcon.GetSpriteName(unit, isDetail, isSame, isAwake));
  }

  public static void SetRarity(
    PlayerUnit playerUnit,
    UI2DSprite dstSprite,
    bool isDetail,
    bool isSame = false,
    bool isAwake = false)
  {
    RarityIcon._SetRarity(playerUnit, playerUnit.unit, dstSprite, isDetail, isSame, isAwake);
  }

  public static void SetRarity(
    UnitUnit unit,
    UI2DSprite dstSprite,
    bool isDetail,
    bool isSame = false,
    bool isAwake = false)
  {
    RarityIcon._SetRarity((PlayerUnit) null, unit, dstSprite, isDetail, isSame, isAwake);
  }

  private static void _SetRarity(
    PlayerUnit playerUnit,
    UnitUnit unit,
    UI2DSprite dstSprite,
    bool isDetail,
    bool isSame = false,
    bool isAwake = false)
  {
    if (Object.op_Equality((Object) dstSprite, (Object) null))
      return;
    Sprite sprite = !(playerUnit == (PlayerUnit) null) ? RarityIcon.GetSprite(playerUnit, isDetail, isSame, isAwake) : RarityIcon.GetSprite(unit, isDetail, isSame, isAwake);
    if (!Object.op_Inequality((Object) sprite, (Object) null))
      return;
    dstSprite.sprite2D = sprite;
    UI2DSprite ui2Dsprite = dstSprite;
    Rect textureRect1 = sprite.textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = sprite.textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    ((Component) dstSprite).gameObject.SetActive(true);
  }

  public static void SetRarity(GearGear gear, UI2DSprite dstSprite)
  {
    if (Object.op_Equality((Object) dstSprite, (Object) null))
      return;
    ((Component) dstSprite).gameObject.SetActive(false);
    if (gear.rarity.index <= 0)
      return;
    Sprite sprite = RarityIcon.GetSprite(gear.rarity);
    if (!Object.op_Inequality((Object) sprite, (Object) null))
      return;
    dstSprite.sprite2D = sprite;
    UI2DSprite ui2Dsprite = dstSprite;
    Rect textureRect1 = sprite.textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = sprite.textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    ((Component) dstSprite).gameObject.SetActive(true);
  }
}
