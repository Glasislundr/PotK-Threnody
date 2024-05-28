// Decompiled with JetBrains decompiler
// Type: EarthUnitNumIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EarthUnitNumIcon : MonoBehaviour
{
  [SerializeField]
  private Vector3 onesScale;
  [SerializeField]
  private Vector3 tensScale;
  [SerializeField]
  private Vector3 onesPos;
  [SerializeField]
  private Vector3[] tensPosList;
  [SerializeField]
  private Sprite[] baseSprites;
  [SerializeField]
  private Sprite[] numSprites;
  [SerializeField]
  private UI2DSprite numBase;
  [SerializeField]
  private UI2DSprite[] digitObject;

  public void SetNumIcon(int num, bool isGorgeous = false)
  {
    EarthUnitNumIcon.Leader index = !isGorgeous ? EarthUnitNumIcon.Leader.None : EarthUnitNumIcon.Leader.Leader;
    ((Component) this.numBase).gameObject.SetActive(true);
    this.numBase.sprite2D = this.baseSprites[(int) index];
    UI2DSprite numBase = this.numBase;
    Rect textureRect1 = this.baseSprites[(int) index].textureRect;
    int width = (int) ((Rect) ref textureRect1).width;
    Rect textureRect2 = this.baseSprites[(int) index].textureRect;
    int height = (int) ((Rect) ref textureRect2).height;
    ((UIWidget) numBase).SetDimensions(width, height);
    int num1 = num >= 10 ? 1 : 0;
    int num2 = num % 10;
    int num3 = num / 10;
    if (num1 == 1)
    {
      ((Component) this.digitObject[0]).transform.localPosition = this.tensPosList[0];
      ((Component) this.digitObject[0]).transform.localScale = this.tensScale;
      this.ChangeNumSprite(this.digitObject[0], num2);
      ((Component) this.digitObject[1]).transform.localPosition = this.tensPosList[1];
      ((Component) this.digitObject[1]).transform.localScale = this.tensScale;
      this.ChangeNumSprite(this.digitObject[1], num3);
    }
    else
    {
      ((Component) this.digitObject[0]).transform.localPosition = this.onesPos;
      ((Component) this.digitObject[0]).transform.localScale = this.onesScale;
      this.ChangeNumSprite(this.digitObject[0], num2);
      ((Component) this.digitObject[1]).gameObject.SetActive(false);
    }
  }

  private void ChangeNumSprite(UI2DSprite sprites, int num)
  {
    ((Component) sprites).gameObject.SetActive(false);
    if (num < 0)
      return;
    ((Component) sprites).gameObject.SetActive(true);
    sprites.sprite2D = this.numSprites[num];
    UI2DSprite ui2Dsprite = sprites;
    Rect textureRect = this.numSprites[num].textureRect;
    int width = (int) ((Rect) ref textureRect).width;
    textureRect = this.numSprites[num].textureRect;
    int height = (int) ((Rect) ref textureRect).height;
    ((UIWidget) ui2Dsprite).SetDimensions(width, height);
  }

  private enum DigitType
  {
    One,
    Ten,
  }

  private enum Leader
  {
    None,
    Leader,
  }
}
