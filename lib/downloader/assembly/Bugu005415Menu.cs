// Decompiled with JetBrains decompiler
// Type: Bugu005415Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005415Menu : BackButtonMenuBase
{
  public EffectControllerArmorRepair effect;
  [SerializeField]
  private GameObject back_button_;

  private IEnumerator SkipCurrentAnimation()
  {
    if (!this.effect.sound_effect_.result)
    {
      Singleton<NGSoundManager>.GetInstance().stopSE();
      Time.timeScale = 100f;
      float tempFixedDeltaTime = Time.fixedDeltaTime;
      Time.fixedDeltaTime = tempFixedDeltaTime * 100f;
      while (this.effect.isAnimation)
        yield return (object) this.effect.isAnimation;
      Time.timeScale = 1f;
      Time.fixedDeltaTime = tempFixedDeltaTime;
      yield return (object) null;
      this.effect.EndEffect();
    }
  }

  public virtual void IbtnBack()
  {
    if (this.effect.isAnimation)
    {
      this.StartCoroutine(this.SkipCurrentAnimation());
    }
    else
    {
      Singleton<NGSoundManager>.GetInstance().stopSE();
      this.backScene();
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnStartcomposite()
  {
  }

  public IEnumerator SetEffectData(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairRepair_results> result_list)
  {
    ((Component) this.effect).gameObject.SetActive(true);
    IEnumerator e = this.effect.Set(thum_list, this.back_button_, result_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetEffectData(
    List<ItemInfo> thum_list,
    List<WebAPI.Response.ItemGearRepairListRepair_results> result_list)
  {
    ((Component) this.effect).gameObject.SetActive(true);
    IEnumerator e = this.effect.Set(thum_list, this.back_button_, result_powered_list: result_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
