// Decompiled with JetBrains decompiler
// Type: GearKindIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class GearKindIcon : IconPrefabBase
{
  public UI2DSprite iconSprite;
  private static GameObject self;

  public void Init(GearKind kind, CommonElement element = CommonElement.none)
  {
    this.iconSprite.sprite2D = GearKindIcon.LoadSprite(kind.Enum, element);
  }

  public void Init(int ID, CommonElement element = CommonElement.none)
  {
    this.iconSprite.sprite2D = GearKindIcon.LoadSprite((GearKindEnum) ID, element);
  }

  public static Sprite LoadSprite(GearKindEnum kind, CommonElement element)
  {
    string empty = string.Empty;
    return Resources.Load<Sprite>(!Singleton<NGGameDataManager>.GetInstance().IsSea ? string.Format("Icons/Materials/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()) : string.Format("Icons/Materials/Sea/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()));
  }

  public void None()
  {
    string empty = string.Empty;
    this.iconSprite.sprite2D = Resources.Load<Sprite>(!Singleton<NGGameDataManager>.GetInstance().IsSea ? string.Format("Icons/Materials/GearKindIcon/9") : string.Format("Icons/Materials/Sea/GearKindIcon/s_type_none"));
  }

  public static GameObject GetPrefab()
  {
    if (Object.op_Equality((Object) GearKindIcon.self, (Object) null))
      GearKindIcon.self = Resources.Load<GameObject>("Icons/GearKindIcon");
    return GearKindIcon.self;
  }
}
