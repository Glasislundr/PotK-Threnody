// Decompiled with JetBrains decompiler
// Type: ButtonNoneState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Behaviour/ButtonNoneState")]
public class ButtonNoneState : MonoBehaviour
{
  public static ButtonNoneState current;
  public string playSE = "SE_1002";
  public List<EventDelegate> onClick = new List<EventDelegate>();

  public virtual bool isEnabled
  {
    get
    {
      if (!((Behaviour) this).enabled)
        return false;
      Collider component = ((Component) this).GetComponent<Collider>();
      return Object.op_Implicit((Object) component) && component.enabled;
    }
    set
    {
      Collider component = ((Component) this).GetComponent<Collider>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.enabled = value;
      else
        ((Behaviour) this).enabled = value;
    }
  }

  protected virtual void OnClick()
  {
    if (!this.isEnabled)
      return;
    ButtonNoneState.current = this;
    EventDelegate.Execute(this.onClick);
    if (!string.IsNullOrEmpty(this.playSE))
    {
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) instance, (Object) null))
        instance.PlaySe(this.playSE);
    }
    ButtonNoneState.current = (ButtonNoneState) null;
  }
}
