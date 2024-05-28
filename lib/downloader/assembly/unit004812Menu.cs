// Decompiled with JetBrains decompiler
// Type: unit004812Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class unit004812Menu : BackButtonMenuBase
{
  public unit004812Scene scene;
  public EffectControllerPrincessSynthesis effect;
  private PlayerUnit basePlayerUnit_;
  private PlayerUnit resultPlayerUnit_;
  private List<int> otherData;
  public Dictionary<string, object> showPopupData;
  public List<List<PlayerUnit>> selectedMaterialPlayerUnits;
  public List<Unit004832Menu.ResultPlayerUnit> resultPlayerUnits;
  public List<Unit004832Menu.OhterInfo> otherInfos;
  public List<Dictionary<string, object>> showPopupDatas;
  [SerializeField]
  private GameObject backButton_;

  public Unit00468Scene.Mode mode { get; set; }

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

  public IEnumerator SetEffectData(
    List<PlayerUnit> num_list,
    List<PlayerUnit> result_list,
    List<int> other_list)
  {
    this.basePlayerUnit_ = result_list[0];
    this.resultPlayerUnit_ = result_list[1];
    ((Component) this.effect).gameObject.SetActive(true);
    this.otherData = other_list;
    IEnumerator e = this.effect.Set(num_list, result_list, other_list, this.backButton_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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
      if (this.mode == Unit00468Scene.Mode.UnitLumpTouta)
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_8_13", false, (object) this.selectedMaterialPlayerUnits, (object) this.resultPlayerUnits, (object) this.otherInfos, (object) this.showPopupDatas);
      else
        Unit004813Scene.changeScene(false, this.basePlayerUnit_, this.resultPlayerUnit_, this.otherData, this.showPopupData, this.mode);
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnStartcomposite()
  {
  }
}
