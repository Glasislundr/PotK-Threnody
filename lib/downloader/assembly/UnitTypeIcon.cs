// Decompiled with JetBrains decompiler
// Type: UnitTypeIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitTypeIcon : MonoBehaviour
{
  private static readonly string prefix_ = "slc_Princess_";
  private static readonly string suffix_ = ".png__GUI__princess_type__princess_type_prefab";
  private static readonly Dictionary<UnitTypeEnum, string> dicUnitType_ = new Dictionary<UnitTypeEnum, string>()
  {
    {
      UnitTypeEnum.ouki,
      "King"
    },
    {
      UnitTypeEnum.meiki,
      "Life"
    },
    {
      UnitTypeEnum.kouki,
      "Attack"
    },
    {
      UnitTypeEnum.maki,
      "Magic"
    },
    {
      UnitTypeEnum.syuki,
      "Defense"
    },
    {
      UnitTypeEnum.syouki,
      "Technical"
    }
  };

  public bool SetSprite(UnitTypeEnum unitType)
  {
    return UnitTypeIcon.SetAtlasSprite(((Component) this).GetComponent<UISprite>(), unitType);
  }

  public static bool SetAtlasSprite(UISprite sprite, UnitTypeEnum unitType)
  {
    return Object.op_Inequality((Object) sprite, (Object) null) && UnitTypeIcon.SetAtlasSprite(sprite, unitType, UnitTypeIcon.prefix_, UnitTypeIcon.dicUnitType_, UnitTypeIcon.suffix_);
  }

  public static bool SetAtlasSprite(
    UISprite sprite,
    UnitTypeEnum unitType,
    string prefix,
    Dictionary<UnitTypeEnum, string> dic,
    string suffix)
  {
    string str;
    if (!dic.TryGetValue(unitType, out str))
    {
      Debug.LogError((object) string.Format("(MasterDataTable.UnitTypeEnum){0} に対応した文字列が定義されていません.", (object) (int) unitType));
      return false;
    }
    sprite.spriteName = prefix + str + suffix;
    UISpriteData atlasSprite = sprite.GetAtlasSprite();
    if (atlasSprite == null)
    {
      Debug.LogError((object) ("\"" + prefix + str + suffix + "\"はAltas(" + ((Object) sprite.atlas).name + ")に存在しません"));
      return false;
    }
    ((UIWidget) sprite).width = atlasSprite.width;
    ((UIWidget) sprite).height = atlasSprite.height;
    return true;
  }
}
