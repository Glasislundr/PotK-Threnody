// Decompiled with JetBrains decompiler
// Type: Explore033TreasureEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033TreasureEffect : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject mCreateIcon;
  [SerializeField]
  private UILabel mRewardNameLbl;
  [SerializeField]
  private TweenPosition mEffectTweenPos;
  [SerializeField]
  private ParticleSystem mFinishEffect;

  public IEnumerator InitAsync(ExploreDropReward reward, Vector3 effectTweenToWorldPos)
  {
    this.mRewardNameLbl.SetTextLocalize(reward.reward_title);
    IEnumerator e = this.mCreateIcon.CreateThumbnail(reward.reward_type, reward.reward_id, reward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component = this.mCreateIcon.GetIcon().GetComponent<UnitIcon>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.RarityCenter();
    ((Component) this.mCreateIcon).gameObject.SetActive(true);
    this.mEffectTweenPos.to = ((Component) this.mEffectTweenPos).transform.parent.InverseTransformPoint(effectTweenToWorldPos);
  }

  public void onTweenFinish() => this.mFinishEffect.Play();

  private void onFinish()
  {
  }
}
