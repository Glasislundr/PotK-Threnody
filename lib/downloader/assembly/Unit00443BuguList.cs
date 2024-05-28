// Decompiled with JetBrains decompiler
// Type: Unit00443BuguList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00443BuguList : MonoBehaviour
{
  public UI2DSprite DynWeaponIll;
  [SerializeField]
  private GearGear actualGear;
  [SerializeField]
  private bool dispNumber;
  [SerializeField]
  private UILabel TxtNumber;
  [SerializeField]
  private GameObject dirNumber;
  private Unit00443Menu menu00443;
  private int beforeIndex;

  public int index { get; set; }

  public IEnumerator Init(Unit00443Menu m, GearGear gear, bool isDispNumber)
  {
    this.menu00443 = m;
    this.actualGear = gear;
    this.dispNumber = isDispNumber;
    this.SetNumber(this.actualGear);
    if (Object.op_Inequality((Object) this.dirNumber, (Object) null))
      this.dirNumber.SetActive(this.dispNumber);
    Future<Sprite> spriteF = this.actualGear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = this.DynWeaponIll;
    Rect textureRect = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = this.DynWeaponIll;
    textureRect = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) this.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator Init(Unit00443Menu m, GearGear gear, int quantity)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit00443BuguList unit00443BuguList = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit00443BuguList.StartCoroutine(unit00443BuguList.Init(m, gear, false));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator Init(ItemInfo item)
  {
    if (Object.op_Inequality((Object) this.dirNumber, (Object) null))
      this.dirNumber.SetActive(false);
    Future<Sprite> spriteF = item.isSupply ? item.supply.LoadSpriteBasic() : item.gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.DynWeaponIll.sprite2D = spriteF.Result;
    UI2DSprite dynWeaponIll1 = this.DynWeaponIll;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) dynWeaponIll1).width = num1;
    UI2DSprite dynWeaponIll2 = this.DynWeaponIll;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) dynWeaponIll2).height = num2;
    ((Component) this.DynWeaponIll).transform.localScale = Vector2.op_Implicit(new Vector2(0.8f, 0.8f));
    yield return (object) new WaitForEndOfFrame();
  }

  public void SetContainerPosition(float viewSizeY)
  {
    Transform childInFind1 = ((Component) this).transform.GetChildInFind("Top");
    ((Component) childInFind1).transform.localPosition = new Vector3(0.0f, (float) (((double) viewSizeY - (double) ((Component) childInFind1).GetComponent<UIWidget>().height) / 2.0), 0.0f);
    Transform childInFind2 = ((Component) this).transform.GetChildInFind("Bottom__anim_fade01");
    ((Component) childInFind2).transform.localPosition = new Vector3(0.0f, (float) -(((double) viewSizeY - (double) ((Component) childInFind2).GetComponent<UIWidget>().height) / 2.0), 0.0f);
  }

  public void SetNumber(GearGear gear)
  {
    this.TxtNumber.SetTextLocalize("NO." + (gear.history_group_number % 10000).ToString().PadLeft(4, '0'));
  }

  public void SetGearInformation()
  {
    this.SetNumber(this.actualGear);
    if (Object.op_Inequality((Object) this.dirNumber, (Object) null))
      this.dirNumber.SetActive(this.dispNumber);
    if (!Object.op_Inequality((Object) this.menu00443, (Object) null))
      return;
    ((Component) this.menu00443.TxtTitle).gameObject.SetActive(true);
    this.menu00443.TxtTitle.SetText(this.actualGear.name);
    RarityIcon.SetRarity(this.actualGear, this.menu00443.rarityStarsIcon);
  }
}
