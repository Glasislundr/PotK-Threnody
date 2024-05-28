// Decompiled with JetBrains decompiler
// Type: Quest0528MenuMapChipNum
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest0528MenuMapChipNum : MonoBehaviour
{
  private static readonly float MapChipSizeOrigin = 50f;
  private static readonly string NumSpriteName = "slc_num_MapIcon_{0}.png__GUI__button_text__button_text_prefab";
  [SerializeField]
  private GameObject[] selectObject;
  [SerializeField]
  private GameObject[] dirDigit;
  [SerializeField]
  private UISprite oneDigitSprite;
  [SerializeField]
  private UISprite[] tensDigitSprite;
  private BL.ForceID unitType;
  private int mapChipSize;

  public void Init(int number, int size, BL.ForceID fieldUnitType)
  {
    Quest0528MenuMapChipNum.Digit index = Quest0528MenuMapChipNum.Digit.One;
    this.unitType = fieldUnitType;
    float num = (float) size / Quest0528MenuMapChipNum.MapChipSizeOrigin;
    ((Component) this).transform.localScale = new Vector3(num, num, 1f);
    if (number == 0)
    {
      ((IEnumerable<GameObject>) this.dirDigit).ToggleOnce(-1);
      ((Component) this.oneDigitSprite).gameObject.SetActive(false);
      ((Component) this.tensDigitSprite[0]).gameObject.SetActive(false);
      ((Component) this.tensDigitSprite[1]).gameObject.SetActive(false);
    }
    else
    {
      if (number > 9)
      {
        index = Quest0528MenuMapChipNum.Digit.Tens;
        ((IEnumerable<UISprite>) this.tensDigitSprite).ForEach<UISprite>((Action<UISprite>) (x => ((Component) x).gameObject.SetActive(true)));
        this.tensDigitSprite[0].spriteName = string.Format(Quest0528MenuMapChipNum.NumSpriteName, (object) (number % 10));
        UISpriteData atlasSprite1 = this.tensDigitSprite[0].GetAtlasSprite();
        ((UIWidget) this.tensDigitSprite[0]).SetDimensions(atlasSprite1.width, atlasSprite1.height);
        this.tensDigitSprite[1].spriteName = string.Format(Quest0528MenuMapChipNum.NumSpriteName, (object) (number / 10));
        UISpriteData atlasSprite2 = this.tensDigitSprite[1].GetAtlasSprite();
        ((UIWidget) this.tensDigitSprite[1]).SetDimensions(atlasSprite2.width, atlasSprite2.height);
      }
      else
      {
        ((Component) this.oneDigitSprite).gameObject.SetActive(true);
        this.oneDigitSprite.spriteName = string.Format(Quest0528MenuMapChipNum.NumSpriteName, (object) number);
        UISpriteData atlasSprite = this.oneDigitSprite.GetAtlasSprite();
        ((UIWidget) this.oneDigitSprite).SetDimensions(atlasSprite.width, atlasSprite.height);
      }
      ((IEnumerable<GameObject>) this.dirDigit).ToggleOnce((int) index);
    }
  }

  public void StartSelectAnim()
  {
    int index = 0;
    if (this.unitType == BL.ForceID.player)
      index = 0;
    else if (this.unitType == BL.ForceID.enemy)
      index = 1;
    ((IEnumerable<GameObject>) this.selectObject).ToggleOnce(index);
    ((IEnumerable<UITweener>) this.selectObject[index].GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
  }

  public void StopSelectAnim() => ((IEnumerable<GameObject>) this.selectObject).ToggleOnce(-1);

  private enum Digit
  {
    One,
    Tens,
  }
}
