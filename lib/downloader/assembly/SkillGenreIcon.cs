// Decompiled with JetBrains decompiler
// Type: SkillGenreIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class SkillGenreIcon : IconPrefabBase
{
  public UI2DSprite iconSprite;
  private static GameObject self;

  public void Init(BattleskillGenre? genre)
  {
    if (genre.HasValue)
      this.iconSprite.sprite2D = SkillGenreIcon.loadSprite(genre.Value, Singleton<NGGameDataManager>.GetInstance().IsSea);
    else
      this.iconSprite.sprite2D = (Sprite) null;
  }

  public static Sprite loadSprite(BattleskillGenre genre, bool bSea)
  {
    return Resources.Load<Sprite>((bSea ? "Icons/Materials/Sea/SkillGenre/" : "Icons/Materials/SkillGenre/") + ((int) genre).ToString());
  }

  public static GameObject GetPrefab()
  {
    if (Object.op_Equality((Object) SkillGenreIcon.self, (Object) null))
      SkillGenreIcon.self = Resources.Load<GameObject>("Icons/SkillGenreIcon");
    return SkillGenreIcon.self;
  }
}
