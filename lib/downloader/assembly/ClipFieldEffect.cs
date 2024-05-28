// Decompiled with JetBrains decompiler
// Type: ClipFieldEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class ClipFieldEffect : BattleMonoBehaviour
{
  private NGSoundManager soundManager;
  private BL.Unit target;
  private GameObject[] prefabs;

  protected override IEnumerator Start_Battle()
  {
    this.soundManager = Singleton<NGSoundManager>.GetInstance();
    yield break;
  }

  public void onPlayEffect(string var)
  {
    int index = int.Parse(var);
    if (this.target == (BL.Unit) null)
      Debug.LogError((object) "onPlayEffect: targetユニットが指定されていない！");
    else if (index >= this.prefabs.Length)
    {
      Debug.LogError((object) ("onPlayEffectで指定した(id:" + (object) index + ")prefabが無い！"));
      Debug.LogError((object) ("設定されているプレハブ数: " + (object) this.prefabs.Length));
    }
    else
      this.battleManager.battleEffects.fieldEffectsStart(this.prefabs[index], this.target);
  }

  public void onPlayBGM(string var) => this.soundManager.playBGM(var);

  public void onPlaySE(string var) => this.soundManager.playSE(var);

  public void onAnimationTrigger(string var)
  {
    if (this.target == (BL.Unit) null)
      Debug.LogError((object) "onAnimationTrigger: targetユニットが指定されていない！");
    else
      this.env.unitResource[this.target].gameObject.GetComponent<UnitUpdate>().setAnimationTrigger(var);
  }

  public void onEndEffect() => this.battleManager.battleEffects.onPopupDismiss();

  public void setEffectData(BL.FieldEffect effect)
  {
    if (Object.op_Equality((Object) this.battleManager, (Object) null))
      this.battleManager = Singleton<NGBattleManager>.GetInstance();
    this.prefabs = this.battleManager.battleEffects.effectResourceAllPrefabs(effect);
    if (effect.fieldEffect.category == BattleFieldEffectCategory.boss)
      this.target = this.env.core.getBossUnit();
    else
      this.target = (BL.Unit) null;
  }
}
