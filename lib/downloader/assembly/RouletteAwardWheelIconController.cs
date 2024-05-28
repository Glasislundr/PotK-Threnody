// Decompiled with JetBrains decompiler
// Type: RouletteAwardWheelIconController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RouletteAwardWheelIconController : MonoBehaviour
{
  [SerializeField]
  private Transform numberDigitSpritesContainer;
  [SerializeField]
  private List<UISprite> numberDigitSprites;
  private const int maxNumber = 9999;
  private const string spriteNameFormat = "slc_{0}.png__GUI__roulette_Common__roulette_Common_prefab";
  [SerializeField]
  private GameObject awardIconContainer;

  public IEnumerator Init(int awardCount, MasterDataTable.CommonRewardType awardType, int awardID)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RouletteAwardWheelIconController wheelIconController = this;
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
    wheelIconController.SetAwardCount(awardCount);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) wheelIconController.StartCoroutine(wheelIconController.SetAwardIcon(awardType, awardID));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void SetAwardCount(int count)
  {
    if (count > 9999)
      count = 9999;
    List<int> intList = new List<int>();
    for (; count > 0; count /= 10)
      intList.Add(count % 10);
    intList.Reverse();
    for (int index = 0; index < this.numberDigitSprites.Count - 1; ++index)
    {
      if (index < intList.Count)
      {
        this.numberDigitSprites[index].spriteName = string.Format("slc_{0}.png__GUI__roulette_Common__roulette_Common_prefab", (object) intList[index]);
        ((Component) this.numberDigitSprites[index]).gameObject.SetActive(true);
      }
      else
        ((Component) this.numberDigitSprites[index]).gameObject.SetActive(false);
    }
    Bounds bounds = ((UIWidget) this.numberDigitSprites[this.numberDigitSprites.Count - 1]).CalculateBounds(this.numberDigitSpritesContainer);
    for (int index = 0; index < this.numberDigitSprites.Count - 1; ++index)
    {
      if (((Component) this.numberDigitSprites[index]).gameObject.activeSelf)
        ((Bounds) ref bounds).Encapsulate(((UIWidget) this.numberDigitSprites[index]).CalculateBounds(this.numberDigitSpritesContainer));
    }
    for (int index = 0; index < this.numberDigitSprites.Count; ++index)
    {
      Vector3 localPosition = ((Component) this.numberDigitSprites[index]).transform.localPosition;
      ((Component) this.numberDigitSprites[index]).transform.localPosition = new Vector3(localPosition.x - ((Bounds) ref bounds).center.x, localPosition.y - ((Bounds) ref bounds).center.y, localPosition.z);
    }
  }

  private IEnumerator SetAwardIcon(MasterDataTable.CommonRewardType awardType, int awardID)
  {
    IEnumerator e = this.awardIconContainer.GetOrAddComponent<CreateIconObject>().CreateThumbnail(awardType, awardID, visibleBottom: false, isButton: false, isRouletteWheelIcon: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
