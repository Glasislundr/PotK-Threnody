// Decompiled with JetBrains decompiler
// Type: Shop00742Bugu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00742Bugu : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtAttack;
  [SerializeField]
  protected UILabel TxtCritical;
  [SerializeField]
  protected UILabel TxtDefense;
  [SerializeField]
  protected UILabel TxtDexterity;
  [SerializeField]
  protected UILabel TxtEvasion;
  [SerializeField]
  protected UILabel TxtLeaderSkillname;
  [SerializeField]
  protected UILabel TxtMatk;
  [SerializeField]
  protected UILabel TxtMdef;
  [SerializeField]
  protected UILabel TxtMove;
  [SerializeField]
  protected UILabel TxtName;
  [SerializeField]
  protected UILabel TxtSkillexplanation;
  [SerializeField]
  protected UI2DSprite SlcTarget;
  [SerializeField]
  protected UI2DSprite rarityStarIcon;
  [SerializeField]
  private UI2DSprite skillIcon;
  [SerializeField]
  private UI2DSprite weaponIcon;
  [SerializeField]
  private GameObject skillDescription;
  [SerializeField]
  private GameObject dirSpAttack;
  [SerializeField]
  private SPAtkTypeIcon WeaponSpAttack01;
  [SerializeField]
  private SPAtkTypeIcon WeaponSpAttack02;

  public IEnumerator Init(GearGear target)
  {
    this.TxtAttack.SetTextLocalize(target.power);
    if (target.attack_type != GearAttackType.magic)
    {
      this.TxtAttack.SetTextLocalize(target.power);
      this.TxtMatk.SetTextLocalize(0);
    }
    else
    {
      this.TxtAttack.SetTextLocalize(0);
      this.TxtMatk.SetTextLocalize(target.power);
    }
    this.TxtCritical.SetTextLocalize(target.critical);
    this.TxtDefense.SetTextLocalize(target.physical_defense);
    this.TxtMdef.SetTextLocalize(target.magic_defense);
    this.TxtMove.SetTextLocalize(target.min_range.ToString() + "-" + (object) target.max_range);
    this.SetTitleText(target);
    this.TxtEvasion.SetTextLocalize(target.evasion);
    this.TxtDexterity.SetTextLocalize(target.hit);
    if (target.skills.Length != 0)
    {
      this.TxtSkillexplanation.SetText(target.skills[0].skill.description);
      this.TxtLeaderSkillname.SetText(target.skills[0].skill.name);
    }
    else
      this.skillDescription.SetActive(false);
    IEnumerator e = this.BuguSpriteCreate(target.LoadSpriteBasic());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> loader = Res.Icons.GearKindIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    loader.Result.Clone(((Component) this.weaponIcon).transform).GetComponent<GearKindIcon>().Init(target.kind, target.GetElement());
    loader = (Future<GameObject>) null;
    e = this.RarityCreate(target);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!target.isReisou())
    {
      UnitFamily[] specialAttackTargets = target.SpecialAttackTargets;
      this.dirSpAttack.SetActive(specialAttackTargets.Length >= 1);
      if (this.dirSpAttack.activeSelf)
      {
        this.WeaponSpAttack01.InitKindId(specialAttackTargets[0]);
        ((UIWidget) ((Component) this.WeaponSpAttack01).gameObject.GetComponent<UI2DSprite>()).depth = 400;
        if (specialAttackTargets.Length == 2)
        {
          this.WeaponSpAttack02.InitKindId(specialAttackTargets[1]);
          ((UIWidget) ((Component) this.WeaponSpAttack02).gameObject.GetComponent<UI2DSprite>()).depth = 400;
        }
      }
    }
    else
      this.dirSpAttack.SetActive(false);
  }

  protected void SetTitleText(GearGear gear)
  {
    ((Component) this.TxtName).gameObject.SetActive(true);
    this.TxtName.SetText(gear.name);
  }

  protected virtual IEnumerator BuguSpriteCreate(Future<Sprite> spriteF)
  {
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = spriteF.Result;
    UI2DSprite slcTarget1 = this.SlcTarget;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) slcTarget1).width = num1;
    UI2DSprite slcTarget2 = this.SlcTarget;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) slcTarget2).height = num2;
    ((Component) this.SlcTarget).transform.localScale = new Vector3(0.5f, 0.5f);
  }

  protected virtual IEnumerator RarityCreate(GearGear target)
  {
    RarityIcon.SetRarity(target, this.rarityStarIcon);
    yield break;
  }

  public virtual void IbtnPopupClose()
  {
  }

  public virtual void IbtnPopupClose2()
  {
  }

  public virtual void IbtnPopupClose3()
  {
  }
}
