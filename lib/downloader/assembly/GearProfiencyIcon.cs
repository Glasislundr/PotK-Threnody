// Decompiled with JetBrains decompiler
// Type: GearProfiencyIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GearProfiencyIcon : IconPrefabBase
{
  [SerializeField]
  private UI2DSprite iconSprite;
  private static readonly string[] IconName = new string[7]
  {
    "hyphen",
    "E",
    "D",
    "C",
    "B",
    "A",
    "S"
  };
  private const int INDEX_HYPHEN = 0;
  private const int LEVEL_MIN = 1;
  private static GameObject self;

  public void Init(int level, bool bHyphen = false)
  {
    level = !bHyphen ? Mathf.Clamp(level, 1, GearProfiencyIcon.IconName.Length - 1) : 0;
    this.iconSprite.sprite2D = Resources.Load<Sprite>(string.Format("Icons/Materials/{0}GearProficiency/dyn_Rankicon_{1}", Singleton<NGGameDataManager>.GetInstance().IsSea ? (object) "Sea/" : (object) "", (object) GearProfiencyIcon.IconName[level]));
  }

  public static GameObject GetPrefab()
  {
    if (Object.op_Equality((Object) GearProfiencyIcon.self, (Object) null))
      GearProfiencyIcon.self = Resources.Load<GameObject>("Icons/GearProfiencyIcon");
    return GearProfiencyIcon.self;
  }
}
