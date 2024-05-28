// Decompiled with JetBrains decompiler
// Type: XParamIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Prefabs/Icons/XParamIcon")]
public class XParamIcon : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite uiSprite_;
  [SerializeField]
  private Sprite[] icons_;
  private Dictionary<string, XParamIcon.SpriteType> dicType_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Icons/XParamIcon").Load<GameObject>();
  }

  public UI2DSprite uiSprite => this.uiSprite_;

  public Sprite this[string key] => this.getSprite(key);

  public Sprite this[XParamIcon.SpriteType type] => this[(int) type];

  public Sprite this[int id]
  {
    get
    {
      int index = id - 1;
      return index < 0 || this.icons_.Length <= index ? (Sprite) null : this.icons_[index];
    }
  }

  private Sprite getSprite(string key)
  {
    if (this.dicType_ == null)
      this.dicType_ = new Dictionary<string, XParamIcon.SpriteType>()
      {
        {
          "HP",
          XParamIcon.SpriteType.Hp
        },
        {
          "力",
          XParamIcon.SpriteType.Strength
        },
        {
          "魔",
          XParamIcon.SpriteType.Intelligence
        },
        {
          "守",
          XParamIcon.SpriteType.Vitality
        },
        {
          "精",
          XParamIcon.SpriteType.Mind
        },
        {
          "速",
          XParamIcon.SpriteType.Agility
        },
        {
          "技",
          XParamIcon.SpriteType.Dexterity
        },
        {
          "運",
          XParamIcon.SpriteType.Lucky
        }
      };
    XParamIcon.SpriteType type;
    if (this.dicType_.TryGetValue(key, out type))
      return this[type];
    Debug.LogError((object) ("Not Found Sprite Key!! XParamIcon[" + key + "]"));
    return (Sprite) null;
  }

  public enum SpriteType
  {
    Hp = 1,
    Strength = 2,
    Intelligence = 3,
    Vitality = 4,
    Mind = 5,
    Agility = 6,
    Dexterity = 7,
    Lucky = 8,
  }
}
