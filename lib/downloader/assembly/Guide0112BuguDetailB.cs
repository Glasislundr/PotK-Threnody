// Decompiled with JetBrains decompiler
// Type: Guide0112BuguDetailB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide0112BuguDetailB : BackButtonMenuBase
{
  private Guide01142bMenu menu;
  [SerializeField]
  protected GameObject SupplyDetail;
  [SerializeField]
  protected GameObject GearDetail;
  [SerializeField]
  protected UILabel TxtNumber;
  [SerializeField]
  protected GameObject dirNumber;
  [SerializeField]
  protected UI2DSprite LinkItem;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel GearIntroduction;
  [HideInInspector]
  public int index;
  private bool isDispNumber;
  private GearGear actualGear;
  private ItemInfo info;

  public IEnumerator InitDetailedScreen(GearGear gear)
  {
    if (Object.op_Inequality((Object) this.SupplyDetail, (Object) null))
      this.SupplyDetail.SetActive(false);
    if (Object.op_Inequality((Object) this.GearDetail, (Object) null))
      this.GearDetail.SetActive(true);
    Future<Sprite> spriteF = gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.LinkItem.sprite2D = spriteF.Result;
    UI2DSprite linkItem1 = this.LinkItem;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) linkItem1).width = num1;
    UI2DSprite linkItem2 = this.LinkItem;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) linkItem2).height = num2;
    if (Object.op_Inequality((Object) this.TxtTitle, (Object) null))
      this.TxtTitle.SetTextLocalize(gear.name);
    this.GearIntroduction.SetTextLocalize(gear.description);
  }

  public void Init(Guide01142bMenu m, GearGear gear, bool dispN)
  {
    this.menu = m;
    this.actualGear = gear;
    this.isDispNumber = dispN;
    this.SetNumber(gear, this.isDispNumber);
  }

  public void SetNumber(GearGear gear, bool isDispNumber)
  {
    this.dirNumber.SetActive(isDispNumber);
    this.TxtNumber.SetTextLocalize("NO." + (gear.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public void SetContainerPosition()
  {
    Transform childInFind1 = ((Component) this).transform.GetChildInFind("Middle");
    ((Component) childInFind1).transform.localPosition = new Vector3(0.0f, (float) (((double) this.menu.scroll.scrollView.panel.GetViewSize().y - (double) ((Component) childInFind1).GetComponent<UIWidget>().height) / 2.0), 0.0f);
    Transform childInFind2 = ((Component) this).transform.GetChildInFind("Bottom");
    ((Component) childInFind2).transform.localPosition = new Vector3(0.0f, (float) -(((double) this.menu.scroll.scrollView.panel.GetViewSize().y - (double) ((Component) childInFind2).GetComponent<UIWidget>().height) / 2.0), 0.0f);
  }

  public void SetGearInformation()
  {
    this.SetNumber(this.actualGear, this.isDispNumber);
    if (Object.op_Inequality((Object) this.dirNumber, (Object) null))
      this.dirNumber.SetActive(this.isDispNumber);
    this.menu.SetTitleText(this.actualGear.name);
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.info != null)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.info.itemID;
    this.backScene();
  }
}
