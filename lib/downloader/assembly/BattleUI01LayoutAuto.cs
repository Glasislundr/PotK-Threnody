// Decompiled with JetBrains decompiler
// Type: BattleUI01LayoutAuto
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (NGTweenParts))]
public class BattleUI01LayoutAuto : MonoBehaviour
{
  [SerializeField]
  [Tooltip("オート時にアクティブにする主部分")]
  private NGTweenParts topAutoStatus_;
  [SerializeField]
  [Tooltip("オート時に不要なアタッチ以下のオブジェクト")]
  private NGTweenParts[] ignoreParts_;
  private NGTweenParts myTween_;
  private bool request_;
  private bool isModeAuto_;

  public void activate(bool isActive, bool isModeAuto, bool isEnabled)
  {
    this.isModeAuto_ = isModeAuto;
    if (Object.op_Equality((Object) this.myTween_, (Object) null))
      this.myTween_ = ((Component) this).GetComponent<NGTweenParts>();
    this.myTween_.isActive = isActive;
    if (isActive)
    {
      this.request_ = false;
      bool v = !isModeAuto;
      bool flag = isActive & isModeAuto;
      foreach (NGTweenParts ignorePart in this.ignoreParts_)
      {
        if (flag && !isEnabled)
          ignorePart.resetActive(false);
        else if (((Component) ((Component) ignorePart).transform.parent).gameObject.activeInHierarchy)
          ignorePart.isActive = v;
        else
          ignorePart.resetActive(v);
      }
    }
    else
      this.request_ = true;
    this.topAutoStatus_.isActive = isActive & isModeAuto;
  }

  private void OnDisable()
  {
    if (!this.request_)
      return;
    this.request_ = false;
    bool v = !this.isModeAuto_;
    foreach (NGTweenParts ignorePart in this.ignoreParts_)
      ignorePart.resetActive(v);
  }
}
