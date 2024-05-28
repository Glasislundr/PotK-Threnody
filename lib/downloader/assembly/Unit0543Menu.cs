// Decompiled with JetBrains decompiler
// Type: Unit0543Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit0543Menu : BackButtonMenuBase
{
  private UnitUnit targetUnit;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtCvName;
  [SerializeField]
  protected UILabel TxtIllustratorname;
  [SerializeField]
  protected GameObject DynCharacter;
  [SerializeField]
  protected Transform weaponTypeIconParent;
  [SerializeField]
  protected GameObject weaponTypeIconPrefab;
  private GameObject goWeaponType;
  [SerializeField]
  private UI2DSprite rarityStarsTypeIconParent;
  [SerializeField]
  private GameObject slcAwakening;
  [SerializeField]
  private UISprite slcCountry;
  [SerializeField]
  private UI2DSprite slcInclusion;
  [SerializeField]
  private GameObject containerR;
  [SerializeField]
  private GameObject containerL;

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public IEnumerator Init(UnitUnit unit, bool isSame, CommonElement element = CommonElement.none)
  {
    this.targetUnit = unit;
    this.TxtTitle.SetTextLocalize(this.targetUnit.name);
    if (this.targetUnit.cv_name.CompareTo("") == 0)
      this.containerL.SetActive(false);
    else
      this.TxtCvName.SetTextLocalize(this.targetUnit.cv_name);
    if (this.targetUnit.illustrator_name.CompareTo("") == 0)
      this.containerR.SetActive(false);
    else
      this.TxtIllustratorname.SetTextLocalize(this.targetUnit.illustrator_name);
    this.goWeaponType = this.createIcon(this.weaponTypeIconPrefab, this.weaponTypeIconParent);
    this.goWeaponType.GetComponent<GearKindIcon>().Init(unit.kind, element);
    RarityIcon.SetRarity(this.targetUnit, this.rarityStarsTypeIconParent, true, isSame);
    IEnumerator e = this.SetCharacterLargeImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (unit.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        unit.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) this.slcInclusion, (Object) null))
    {
      ((Component) this.slcInclusion).gameObject.SetActive(false);
      if (unit.inclusion_ip.HasValue)
      {
        ((Component) this.slcInclusion).gameObject.SetActive(true);
        e = unit.SetInclusionIP(this.slcInclusion);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    if (Object.op_Inequality((Object) this.slcAwakening, (Object) null))
      this.slcAwakening.SetActive(false);
    if (unit.awake_unit_flag)
      this.slcAwakening.SetActive(true);
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = ((UIWidget) componentInChildren1).depth + 1000;
    ((Component) componentInChildren1).transform.localPosition = Vector3.zero;
    return icon;
  }

  public IEnumerator SetCharacterLargeImage()
  {
    Future<GameObject> goFuture = this.targetUnit.LoadMypage();
    IEnumerator e = goFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject go = goFuture.Result.Clone(this.DynCharacter.transform);
    go.transform.localPosition = new Vector3(this.targetUnit.illust_pattern.illust_x, this.targetUnit.illust_pattern.illust_y, 0.0f);
    go.transform.localScale = new Vector3(this.targetUnit.illust_pattern.illust_scale, this.targetUnit.illust_pattern.illust_scale, this.targetUnit.illust_pattern.illust_scale);
    e = this.targetUnit.SetLargeSpriteSnap(go, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.targetUnit.SetLargeSpriteWithMask(go, Res.GUI._004_3_sozai.mask_chara.Load<Texture2D>(), 5, y: -80);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onEndScene()
  {
    Resources.UnloadUnusedAssets();
    NGSoundManager instanceOrNull = Singleton<NGSoundManager>.GetInstanceOrNull();
    if (!Object.op_Inequality((Object) instanceOrNull, (Object) null))
      return;
    instanceOrNull.stopVoice();
  }
}
