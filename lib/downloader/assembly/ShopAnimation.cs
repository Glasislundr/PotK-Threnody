// Decompiled with JetBrains decompiler
// Type: ShopAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopAnimation : MonoBehaviour
{
  [SerializeField]
  private Shop0071LeaderStand leaderStand;
  [SerializeField]
  private Animator ShopControllAnim;
  private RuntimeAnimatorController ShopControllAnimRuntime;
  private int LimitedParam = Animator.StringToHash("Limited");
  private int NormalParam = Animator.StringToHash("Normal");
  private int Stand = Animator.StringToHash(nameof (Stand));
  private bool isLimit;

  public IEnumerator Init()
  {
    Future<RuntimeAnimatorController> animF = new ResourceObject("Prefabs/shop007_1/Anims/Change_Shop").Load<RuntimeAnimatorController>();
    IEnumerator e = animF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ShopControllAnimRuntime = animF.Result;
    this.ShopControllAnim.runtimeAnimatorController = this.ShopControllAnimRuntime;
    this.leaderStand.SetLeaderCharacter();
    yield return (object) null;
    this.ShopControllAnim.SetTrigger("FirstStand");
    this.ChangeBanners(true);
  }

  public void ChangeStandCharacter(bool isLimit)
  {
    this.isLimit = isLimit;
    this.ShopControllAnim.SetTrigger(this.Stand);
  }

  public void ChangeBanners(bool isLimit)
  {
    if (isLimit)
      this.ShopControllAnim.SetTrigger(this.LimitedParam);
    else
      this.ShopControllAnim.SetTrigger(this.NormalParam);
  }

  public bool IsWait()
  {
    int num = 0;
    while (this.ShopControllAnim.layerCount < 0)
    {
      AnimatorStateInfo animatorStateInfo = this.ShopControllAnim.GetCurrentAnimatorStateInfo(num);
      if (!((AnimatorStateInfo) ref animatorStateInfo).IsName("Wait"))
        return false;
      ++num;
    }
    return true;
  }
}
