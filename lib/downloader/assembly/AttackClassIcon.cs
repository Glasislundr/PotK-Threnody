// Decompiled with JetBrains decompiler
// Type: AttackClassIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AttackClassIcon : IconPrefabBase
{
  private bool firstReference_ = true;
  private static AttackClassIcon.SpriteCache cache_;

  private UI2DSprite sprite => ((Component) this).GetComponent<UI2DSprite>();

  public void Initialize(GearAttackClassification attackClass, CommonElement element = CommonElement.none)
  {
    UI2DSprite sprite = this.sprite;
    if (!Object.op_Inequality((Object) sprite, (Object) null))
      return;
    sprite.sprite2D = this.cache.GetSprite(attackClass, element);
  }

  private AttackClassIcon.SpriteCache cache
  {
    get
    {
      if (this.firstReference_)
      {
        if (AttackClassIcon.cache_ != null)
          ++AttackClassIcon.cache_.countReference_;
        else
          AttackClassIcon.cache_ = new AttackClassIcon.SpriteCache();
        this.firstReference_ = false;
      }
      return AttackClassIcon.cache_;
    }
  }

  private void OnDestroy()
  {
    if (AttackClassIcon.cache_ == null || this.firstReference_)
      return;
    this.firstReference_ = true;
    if (--AttackClassIcon.cache_.countReference_ != 0)
      return;
    AttackClassIcon.cache_.Clear();
    AttackClassIcon.cache_ = (AttackClassIcon.SpriteCache) null;
  }

  private class SpriteCache
  {
    public int countReference_ = 1;
    private Dictionary<string, Sprite> dicSprite_ = new Dictionary<string, Sprite>();
    private readonly HashSet<CommonElement> existElements_ = new HashSet<CommonElement>()
    {
      CommonElement.none,
      CommonElement.fire,
      CommonElement.wind,
      CommonElement.thunder,
      CommonElement.ice,
      CommonElement.light,
      CommonElement.dark
    };
    private readonly Dictionary<GearAttackClassification, string> dicAttackClass_ = new Dictionary<GearAttackClassification, string>()
    {
      {
        GearAttackClassification.slash,
        "zan"
      },
      {
        GearAttackClassification.blow,
        "da"
      },
      {
        GearAttackClassification.pierce,
        "shi"
      },
      {
        GearAttackClassification.shoot,
        "sha"
      },
      {
        GearAttackClassification.magic,
        "ma"
      }
    };

    public void Clear() => this.dicSprite_.Clear();

    public Sprite GetSprite(GearAttackClassification attackClass, CommonElement element = CommonElement.none)
    {
      int num = (int) attackClass;
      string str1 = num.ToString();
      num = (int) element;
      string str2 = num.ToString();
      string key = str1 + "_" + str2;
      Sprite sprite1;
      if (this.dicSprite_.TryGetValue(key, out sprite1))
        return sprite1;
      Sprite sprite2 = this.loadSprite(attackClass, element);
      this.dicSprite_.Add(key, sprite2);
      return sprite2;
    }

    private Sprite loadSprite(GearAttackClassification attackClass, CommonElement element)
    {
      string str = this.toSpritePath(attackClass, element);
      if (str == null)
      {
        Debug.LogError((object) string.Format("GearAttackClassification.{0} CommonElement.{1} に対応したアイコンは有りません。", (object) attackClass, (object) element));
        str = this.blankPath;
      }
      return Resources.Load<Sprite>(str);
    }

    private string toSpritePath(GearAttackClassification attackClass, CommonElement element)
    {
      string str;
      if (!this.dicAttackClass_.TryGetValue(attackClass, out str))
        return (string) null;
      if (!this.existElements_.Contains(element))
      {
        Debug.LogError((object) string.Format("\"CommonElement.{0}\"に対応した攻撃区分のアイコンは有りません。", (object) element));
        element = CommonElement.none;
      }
      return "Icons/Materials/AttackClass/slc_AttackClass_" + element.ToString() + "_" + str;
    }

    private string blankPath => "Icons/Materials/GearKindIcon/9";
  }
}
