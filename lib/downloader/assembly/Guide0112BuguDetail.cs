// Decompiled with JetBrains decompiler
// Type: Guide0112BuguDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide0112BuguDetail : Unit00443Menu
{
  [SerializeField]
  private GearGear actualGear;
  [SerializeField]
  private bool dispNumber;
  [SerializeField]
  private UILabel TxtNumber;
  [SerializeField]
  private GameObject dirNumber;
  [SerializeField]
  private GameObject zoomButton;
  private Guide01142Menu menu;
  [HideInInspector]
  public int index;
  private int beforeIndex;

  public IEnumerator Init(Guide01142Menu m, GearGear gear, bool isDispNumber)
  {
    Guide0112BuguDetail guide0112BuguDetail = this;
    guide0112BuguDetail.menu = m;
    guide0112BuguDetail.actualGear = gear;
    guide0112BuguDetail.dispNumber = isDispNumber;
    guide0112BuguDetail.SetNumber(guide0112BuguDetail.actualGear);
    if (Object.op_Inequality((Object) guide0112BuguDetail.dirNumber, (Object) null))
      guide0112BuguDetail.dirNumber.SetActive(guide0112BuguDetail.dispNumber);
    guide0112BuguDetail.TxtTitle = guide0112BuguDetail.menu.title;
    guide0112BuguDetail.rarityStarsIcon = guide0112BuguDetail.menu.raritySIcons;
    IEnumerator e = guide0112BuguDetail.indicatorPage1.LoadPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0112BuguDetail.indicatorPage1.Init(guide0112BuguDetail.actualGear);
    guide0112BuguDetail.indicatorPage2.Init(guide0112BuguDetail.actualGear);
    guide0112BuguDetail.indicator.resetScrollView();
    ((Component) guide0112BuguDetail.indicatorPage1).transform.localScale = Vector3.one;
    e = guide0112BuguDetail.SetIncrementalParameter(guide0112BuguDetail.actualGear, guide0112BuguDetail.DirAddStauts);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = guide0112BuguDetail.actualGear.LoadSpriteBasic();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0112BuguDetail.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = guide0112BuguDetail.DynWeaponIll;
    Rect textureRect = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = guide0112BuguDetail.DynWeaponIll;
    textureRect = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) guide0112BuguDetail.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator Init(Guide01142Menu m, GearGear gear, int quantity)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Guide0112BuguDetail guide0112BuguDetail = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      guide0112BuguDetail.indicatorPage1.SetUnit(quantity);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) guide0112BuguDetail.StartCoroutine(guide0112BuguDetail.Init(m, gear, false));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void SetContainerPosition(Guide01142Menu m)
  {
    Transform childInFind1 = ((Component) this).transform.GetChildInFind("Top");
    ((Component) childInFind1).transform.localPosition = new Vector3(0.0f, (float) (((double) m.scroll.scrollView.panel.GetViewSize().y - (double) ((Component) childInFind1).GetComponent<UIWidget>().height) / 2.0), 0.0f);
    Transform childInFind2 = ((Component) this).transform.GetChildInFind("Bottom__anim_fade01");
    ((Component) childInFind2).transform.localPosition = new Vector3(0.0f, (float) -(((double) m.scroll.scrollView.panel.GetViewSize().y - (double) ((Component) childInFind2).GetComponent<UIWidget>().height) / 2.0), 0.0f);
  }

  public void SetNumber(GearGear gear)
  {
    this.TxtNumber.SetTextLocalize("NO." + (gear.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public override void IbtnZoom() => Unit00446Scene.changeScene(true, this.actualGear);

  public void SetGearInformation()
  {
    this.SetNumber(this.actualGear);
    if (Object.op_Inequality((Object) this.dirNumber, (Object) null))
      this.dirNumber.SetActive(this.dispNumber);
    this.SetTitleText(this.actualGear.name);
    RarityIcon.SetRarity(this.actualGear, this.rarityStarsIcon);
  }

  public void SetForcusInfoPage(int index) => this.indicator.setItemPositionQuick(index);

  public int GetForcusInfoPage() => this.indicator.selected;

  public void SetEnableInfoSlideSE(bool enable) => this.indicator.SeEnable = enable;

  public void SetActiveZoomButton(bool isActive)
  {
    if (Object.op_Equality((Object) this.zoomButton, (Object) null))
      return;
    this.zoomButton.SetActive(isActive);
  }
}
