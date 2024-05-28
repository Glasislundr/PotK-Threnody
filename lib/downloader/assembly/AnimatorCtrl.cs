// Decompiled with JetBrains decompiler
// Type: AnimatorCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimatorCtrl : MonoBehaviour
{
  [SerializeField]
  private List<AnimatorCtrl.SetParType> set_type_list_ = new List<AnimatorCtrl.SetParType>();

  public void SetBool(string param_name, bool flag)
  {
    this.set_type_list_.Add(new AnimatorCtrl.SetParType(param_name, AnimatorCtrl.spt.BOOL, this.set_type_list_.Count)
    {
      flag = flag
    });
  }

  public void SetFloat(string param_name, float value)
  {
    this.set_type_list_.Add(new AnimatorCtrl.SetParType(param_name, AnimatorCtrl.spt.FLOAT, this.set_type_list_.Count)
    {
      val_float = value
    });
  }

  public void SetInteger(string param_name, int value)
  {
    this.set_type_list_.Add(new AnimatorCtrl.SetParType(param_name, AnimatorCtrl.spt.INT, this.set_type_list_.Count)
    {
      val_int = value
    });
  }

  public void SetTrigger(string param_name)
  {
    this.set_type_list_.Add(new AnimatorCtrl.SetParType(param_name, AnimatorCtrl.spt.TRIGGER, this.set_type_list_.Count)
    {
      trigger = true
    });
  }

  public void Reset() => this.set_type_list_.Clear();

  private void OnEnable()
  {
    Animator component = ((Component) this).gameObject.GetComponent<Animator>();
    foreach (AnimatorCtrl.SetParType setType in this.set_type_list_)
    {
      switch (setType.param_type)
      {
        case AnimatorCtrl.spt.BOOL:
          component.SetBool(setType.param_name, setType.flag);
          continue;
        case AnimatorCtrl.spt.INT:
          component.SetInteger(setType.param_name, setType.val_int);
          continue;
        case AnimatorCtrl.spt.FLOAT:
          component.SetFloat(setType.param_name, setType.val_float);
          continue;
        case AnimatorCtrl.spt.TRIGGER:
          component.SetBool(setType.param_name, setType.trigger);
          continue;
        default:
          continue;
      }
    }
  }

  private void OnDisable()
  {
  }

  private enum spt
  {
    BOOL,
    INT,
    FLOAT,
    TRIGGER,
  }

  [Serializable]
  private class SetParType
  {
    public string param_name = "";
    public AnimatorCtrl.spt param_type;
    public int id;
    public bool flag;
    public bool trigger;
    public int val_int;
    public float val_float;

    public SetParType(string name, AnimatorCtrl.spt type, int n)
    {
      this.param_name = name;
      this.param_type = type;
      this.id = n;
    }
  }
}
