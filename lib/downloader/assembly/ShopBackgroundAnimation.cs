// Decompiled with JetBrains decompiler
// Type: ShopBackgroundAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ShopBackgroundAnimation : MonoBehaviour
{
  public static GameObject CurrentShopBackground;
  [SerializeField]
  private Animator ShopBackgroundControllAnim;
  private RuntimeAnimatorController ShopBackgroundControllAnimRuntime;
  private int IsOut = Animator.StringToHash(nameof (IsOut));

  public void Change()
  {
    CommonBackground commonBackground = Singleton<CommonRoot>.GetInstance().getCommonBackground();
    if (Object.op_Equality((Object) ShopBackgroundAnimation.CurrentShopBackground, (Object) null))
    {
      commonBackground.releaseBackground();
    }
    else
    {
      ShopBackgroundAnimation component = ShopBackgroundAnimation.CurrentShopBackground.GetComponent<ShopBackgroundAnimation>();
      Singleton<NGSceneManager>.GetInstance().StartCoroutine(component.OutAnimation(component));
    }
    ShopBackgroundAnimation.CurrentShopBackground = NGUITools.AddChild(((Component) commonBackground.bgContainer).gameObject, ((Component) this).gameObject);
    foreach (UI2DSprite componentsInChild in ShopBackgroundAnimation.CurrentShopBackground.GetComponentsInChildren<UI2DSprite>())
    {
      if (commonBackground.bgContainer.height >= ((UIWidget) componentsInChild).height)
      {
        ((UIWidget) componentsInChild).keepAspectRatio = (UIWidget.AspectRatioSource) 2;
        ((UIRect) componentsInChild).topAnchor.target = ((Component) commonBackground.bgContainer).transform;
        ((UIRect) componentsInChild).topAnchor.absolute = 0;
        ((UIRect) componentsInChild).bottomAnchor.target = ((Component) commonBackground.bgContainer).transform;
        ((UIRect) componentsInChild).bottomAnchor.absolute = 0;
      }
    }
  }

  private IEnumerator OutAnimation(ShopBackgroundAnimation shopBackgroundAnimation)
  {
    this.ShopBackgroundControllAnim.SetTrigger(this.IsOut);
    yield return (object) null;
    AnimatorStateInfo animatorStateInfo1 = this.ShopBackgroundControllAnim.GetCurrentAnimatorStateInfo(0);
    int lastStateHash = ((AnimatorStateInfo) ref animatorStateInfo1).nameHash;
    while (true)
    {
      AnimatorStateInfo animatorStateInfo2 = this.ShopBackgroundControllAnim.GetCurrentAnimatorStateInfo(0);
      if (((AnimatorStateInfo) ref animatorStateInfo2).fullPathHash != lastStateHash || (double) ((AnimatorStateInfo) ref animatorStateInfo2).normalizedTime < 1.0)
        yield return (object) null;
      else
        break;
    }
    Object.DestroyImmediate((Object) ((Component) shopBackgroundAnimation).gameObject);
  }

  public bool IsWait()
  {
    AnimatorStateInfo animatorStateInfo = this.ShopBackgroundControllAnim.GetCurrentAnimatorStateInfo(0);
    return ((AnimatorStateInfo) ref animatorStateInfo).IsName("Wait");
  }
}
