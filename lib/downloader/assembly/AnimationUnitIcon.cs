// Decompiled with JetBrains decompiler
// Type: AnimationUnitIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimationUnitIcon : MonoBehaviour
{
  private PlayerUnit player_unit_;
  [SerializeField]
  private SpriteRenderer frame_;
  [SerializeField]
  private SpriteRenderer back_;
  [SerializeField]
  private SpriteRenderer type_;
  [SerializeField]
  private SpriteRenderer level1_;
  [SerializeField]
  private SpriteRenderer level2_;
  [SerializeField]
  private SpriteRenderer princessType;
  [SerializeField]
  private GameObject star_;
  [SerializeField]
  private GameObject new_;
  [SerializeField]
  private GameObject level_;
  [SerializeField]
  private List<Sprite> rank_list_;
  [SerializeField]
  private List<Sprite> num_list_;
  [SerializeField]
  private List<Sprite> princessTypeList;
  [SerializeField]
  private SpriteRenderer rarityStar;

  public void SetType(GearKindEnum kind, CommonElement element)
  {
    this.type_.sprite = Resources.Load<Sprite>(string.Format("Icons/Materials/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()));
  }

  public void SetStar()
  {
    Sprite sprite = RarityIcon.GetSprite(this.player_unit_.unit, false);
    this.rarityStar.sprite = Sprite.Create(sprite.texture, sprite.textureRect, new Vector2(0.5f, 0.0f), 1f, 100U, (SpriteMeshType) 0);
  }

  private void SetStar(UnitUnit unit)
  {
    Sprite sprite = RarityIcon.GetSprite(unit, false);
    this.rarityStar.sprite = Sprite.Create(sprite.texture, sprite.textureRect, new Vector2(0.5f, 0.0f), 1f, 100U, (SpriteMeshType) 0);
  }

  public void SetNew(bool flag) => this.new_.SetActive(flag);

  private void SetLevelNum()
  {
    this.level_.SetActive(true);
    int totalLevel = this.player_unit_.total_level;
    int index1 = totalLevel % 10;
    int index2 = totalLevel / 10;
    this.level1_.sprite = this.num_list_[index1];
    this.level2_.sprite = this.num_list_[index2];
    if (index2 != 0)
      return;
    ((Component) this.level2_).gameObject.SetActive(false);
  }

  private void SetPrincessType(PlayerUnit unit)
  {
    ((Component) this.princessType).gameObject.SetActive(false);
    if (!unit.unit.IsNormalUnit)
      return;
    int index = unit.unit_type.ID - 1;
    if (this.princessTypeList.Count <= index || !Object.op_Inequality((Object) this.princessTypeList[index], (Object) null))
      return;
    ((Component) this.princessType).gameObject.SetActive(true);
    this.princessType.sprite = this.princessTypeList[unit.unit_type.ID - 1];
  }

  public void Set(PlayerUnit player_unit, bool is_new = false)
  {
    if (player_unit.unit == null)
      return;
    this.player_unit_ = player_unit;
    this.SetType(this.player_unit_.unit.kind.Enum, this.player_unit_.GetElement());
    this.SetStar();
    this.SetNew(is_new);
    this.SetPrincessType(player_unit);
    if (player_unit.unit.awake_unit_flag)
    {
      this.frame_.sprite = Resources.Load<Sprite>(string.Format("Prefabs/UnitIcon/Materials/s2_gold"));
      this.back_.sprite = Resources.Load<Sprite>(string.Format("Prefabs/UnitIcon/Materials/s_back_gold"));
    }
    if (player_unit.unit.IsNormalUnit)
    {
      this.SetLevelNum();
      this.star_.transform.localPosition = new Vector3(0.0f, -0.43f, 0.0f);
    }
    else
      this.star_.transform.localPosition = new Vector3(0.12f, -0.6f, 0.0f);
  }

  public void Set(UnitUnit unit, bool is_new = false)
  {
    this.SetType(unit.kind.Enum, unit.GetElement());
    this.SetStar(unit);
    this.SetNew(is_new);
    this.star_.transform.localPosition = new Vector3(0.12f, -0.6f, 0.0f);
  }
}
