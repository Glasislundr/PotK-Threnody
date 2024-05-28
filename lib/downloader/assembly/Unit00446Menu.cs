// Decompiled with JetBrains decompiler
// Type: Unit00446Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00446Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtBattleTitle;
  public UI2DSprite GearSpriteObject;
  public UI2DSprite rarityStars;
  public UILabel TxtTitle;

  public IEnumerator SetSprite(GearGear targetgear)
  {
    Future<Sprite> spriteF = targetgear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.GearSpriteObject.sprite2D = spriteF.Result;
    UI2DSprite gearSpriteObject1 = this.GearSpriteObject;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) gearSpriteObject1).width = num1;
    UI2DSprite gearSpriteObject2 = this.GearSpriteObject;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) gearSpriteObject2).height = num2;
    RarityIcon.SetRarity(targetgear, this.rarityStars);
    this.TxtTitle.SetTextLocalize(targetgear.name);
  }

  public void changeScene()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public virtual void IbtnBattleBack()
  {
  }

  public override void onBackButton() => this.changeScene();
}
