// Decompiled with JetBrains decompiler
// Type: GuildMapObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class GuildMapObject : BackButtonMenuBase
{
  [SerializeField]
  private bool isInitialize;
  private bool overlapControl;
  private UITweener[] tweens;
  private float tweensTime;

  public bool IsInitialize
  {
    get => this.isInitialize;
    set => this.isInitialize = value;
  }

  public IEnumerator StartUp()
  {
    GuildMapObject guildMapObject = this;
    guildMapObject.isInitialize = false;
    ((Component) guildMapObject).transform.localPosition = new Vector3(999f, 999f, 0.0f);
    if (guildMapObject.tweens == null)
    {
      guildMapObject.tweens = NGTween.findTweeners(((Component) guildMapObject).gameObject, true);
      float num1 = 0.0f;
      foreach (UITweener tween in guildMapObject.tweens)
      {
        if (tween.style != 1)
        {
          float num2 = tween.delay + tween.duration;
          num1 = (double) num2 > (double) num1 ? num2 : num1;
        }
      }
      guildMapObject.tweensTime = num1;
    }
    guildMapObject.overlapControl = true;
    guildMapObject.PlayTween(false);
    yield return (object) new WaitForSeconds(guildMapObject.tweensTime);
    if (Object.op_Inequality((Object) ((Component) guildMapObject).transform, (Object) null))
      ((Component) guildMapObject).transform.localPosition = Vector3.zero;
    guildMapObject.isInitialize = true;
  }

  public override void onBackButton() => this.backScene();

  public void SetActive(bool flag)
  {
    if (!Object.op_Inequality((Object) ((Component) this).gameObject, (Object) null))
      return;
    ((Component) this).gameObject.SetActive(flag);
  }

  public bool PlayTween(bool flag)
  {
    if (this.tweens == null || this.overlapControl == flag)
      return false;
    this.overlapControl = flag;
    if (flag)
    {
      this.SetActive(flag);
      ((IEnumerable<UITweener>) this.tweens).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == 13)).ForEach<UITweener>((Action<UITweener>) (x => ((Behaviour) x).enabled = false));
      NGTween.playTweens(this.tweens, NGTween.Kind.START);
    }
    else if (((Component) this).gameObject.activeSelf)
    {
      this.StartCoroutine(this.DelaySetActive(flag, this.tweensTime));
      ((IEnumerable<UITweener>) this.tweens).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == 12)).ForEach<UITweener>((Action<UITweener>) (x => ((Behaviour) x).enabled = false));
      NGTween.playTweens(this.tweens, NGTween.Kind.END);
    }
    return true;
  }

  private IEnumerator DelaySetActive(bool flag, float time)
  {
    while (flag == this.overlapControl)
    {
      if ((double) time >= 0.0)
      {
        time -= Time.deltaTime;
        yield return (object) true;
      }
      else
      {
        this.SetActive(flag);
        break;
      }
    }
  }
}
