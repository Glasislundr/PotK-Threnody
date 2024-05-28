// Decompiled with JetBrains decompiler
// Type: NGMessageUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class NGMessageUI : Singleton<NGMessageUI>
{
  public int up;
  public int littleup;
  public int central;
  public int littledown;
  public int down;
  public NGMessageUI.PosType postype;
  public GameObject messageObj;
  public UILabel messageLabel;

  private void Start() => this.messageObj.SetActive(false);

  private void SetMessage(
    string str,
    float offset,
    float times,
    NGMessageUI.ColorType colorType,
    bool enabledSe)
  {
    this.ChangeBG(colorType);
    if (enabledSe)
      this.PlaySE();
    this.messageObj.transform.localPosition = Vector2.op_Implicit(new Vector2(0.0f, (float) this.GetPosTypeValue() + offset));
    this.messageObj.SetActive(false);
    this.messageObj.SetActive(true);
    this.messageLabel.SetTextLocalize(str);
    TweenAlpha component = this.messageObj.GetComponent<TweenAlpha>();
    if ((double) times == 0.0)
    {
      if (str.Split('\n').Length > 1)
        ((UITweener) component).delay = 3f;
      else
        ((UITweener) component).delay = 1f;
    }
    else
      ((UITweener) component).delay = times;
    ((UITweener) component).SetOnFinished((EventDelegate.Callback) (() => this.TurnOff()));
    ((UITweener) component).ResetToBeginning();
    ((UITweener) component).PlayForward();
  }

  private int GetPosTypeValue()
  {
    switch (this.postype)
    {
      case NGMessageUI.PosType.UP:
        return this.up;
      case NGMessageUI.PosType.LITTLEUP:
        return this.littleup;
      case NGMessageUI.PosType.CENTRAL:
        return this.central;
      case NGMessageUI.PosType.LITTLEDOWN:
        return this.littledown;
      case NGMessageUI.PosType.DOWN:
        return this.down;
      default:
        return 0;
    }
  }

  public void SetMessageByPosType(string str, NGMessageUI.PosType posType = NGMessageUI.PosType.CENTRAL, float offset = 0.0f)
  {
    this.postype = posType;
    this.SetMessage(str, offset, 0.0f, NGMessageUI.ColorType.Black, false);
  }

  public void SetMessageByTime(
    string str,
    float times,
    NGMessageUI.PosType posType = NGMessageUI.PosType.CENTRAL,
    NGMessageUI.ColorType colorType = NGMessageUI.ColorType.Black,
    bool enabledSe = false)
  {
    this.postype = posType;
    this.SetMessage(str, 0.0f, times, colorType, enabledSe);
  }

  protected override void Initialize()
  {
  }

  public void TurnOff() => this.messageObj.SetActive(false);

  private void PlaySE() => Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");

  private void ChangeBG(NGMessageUI.ColorType colorType)
  {
    ((Action<UISprite[]>) (sprites =>
    {
      if (sprites == null || sprites.Length == 0)
        return;
      for (int index = 0; index < sprites.Length; ++index)
      {
        switch (colorType)
        {
          case NGMessageUI.ColorType.Black:
            sprites[index].spriteName = "slc_Listbase_None.png__GUI__common__common_prefab";
            break;
          case NGMessageUI.ColorType.Gray:
            sprites[index].spriteName = "slc_Listbase_gray.png__GUI__common__common_prefab";
            break;
        }
      }
    }))(this.messageObj.GetComponentsInChildren<UISprite>(true));
  }

  public enum ColorType
  {
    Black,
    Gray,
  }

  public enum PosType
  {
    UP,
    LITTLEUP,
    CENTRAL,
    LITTLEDOWN,
    DOWN,
  }
}
