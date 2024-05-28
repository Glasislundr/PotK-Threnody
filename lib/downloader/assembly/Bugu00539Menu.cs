// Decompiled with JetBrains decompiler
// Type: Bugu00539Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu00539Menu : BackButtonMenuBase
{
  public Bugu00539Scene scene;
  public EffectControllerArmorSythesis effect;
  [SerializeField]
  private GameObject back_button_;
  private bool is_new_;
  private Action backSceneCallback;

  private IEnumerator SkipCurrentAnimation()
  {
    if (!this.effect.sound_manager_.result)
    {
      Singleton<NGSoundManager>.GetInstance().stopSE();
      Time.timeScale = 100f;
      float tempFixedDeltaTime = Time.fixedDeltaTime;
      Time.fixedDeltaTime = tempFixedDeltaTime * 100f;
      while (this.effect.isAnimation)
        yield return (object) this.effect.isAnimation;
      Time.timeScale = 1f;
      Time.fixedDeltaTime = tempFixedDeltaTime;
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
      if (!this.scene.sythesisItem.gear.kind.isEquip)
        Bugu00561Scene.changeScene(false, this.scene.sythesisItem, this.scene.sythesisItem.isNew, true);
      else
        Gacha00611Scene.ChangeScene(false, this.is_new_, 0, this.scene.sythesisItem, this.scene.baseItem, this.scene.targetReisou, this.scene.baseReisou, this.backSceneCallback, true, this.scene.addReisouJewel);
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnStartcomposite()
  {
    Debug.Log((object) "click default event IbtnStartcomposite");
  }

  public IEnumerator SetEffectData(
    List<GameCore.ItemInfo> num_list,
    bool is_new,
    GameCore.ItemInfo item_data,
    string[] anim_pattern,
    GameCore.ItemInfo baseItem,
    Action backSceneCallback)
  {
    this.is_new_ = is_new;
    ((Component) this.effect).gameObject.SetActive(true);
    this.backSceneCallback = backSceneCallback;
    IEnumerator e = this.effect.Set(num_list, is_new, item_data, this.back_button_, anim_pattern, baseItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
