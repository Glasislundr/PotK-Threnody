// Decompiled with JetBrains decompiler
// Type: Battle01CommandNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandNode : MonoBehaviour
{
  public Battle01CommandWait cmdWait_;
  public Battle01CommandSkill cmdSkill_;
  public Battle01CommandOugi cmdOugi_;
  public Battle01CommandSEA cmdSEA_;
  public GameObject objSEA_;
  private Battle01CommandNode.CommandFlag? wFlags_;
  private bool isInitializeResetDelegate_ = true;
  private Battle01CommandNode.ResetDelegate onReset_;
  public float delayTime = 0.83f;

  public Battle01CommandNode.CommandFlag ActiveCommands
  {
    get
    {
      return !this.wFlags_.HasValue ? (this.wFlags_ = new Battle01CommandNode.CommandFlag?((Battle01CommandNode.CommandFlag) ((Object.op_Inequality((Object) this.cmdWait_, (Object) null) ? 1 : 0) | (Object.op_Inequality((Object) this.cmdSkill_, (Object) null) ? 2 : 0) | (Object.op_Inequality((Object) this.cmdOugi_, (Object) null) ? 4 : 0)))).Value : this.wFlags_.Value;
    }
  }

  public void resetCurrentUnitPosition(bool bClear = false)
  {
    if (this.isInitializeResetDelegate_)
    {
      this.isInitializeResetDelegate_ = false;
      if (Object.op_Inequality((Object) this.cmdSkill_, (Object) null))
        this.onReset_ += new Battle01CommandNode.ResetDelegate(this.cmdSkill_.resetCurrentUnitPosition);
      if (Object.op_Inequality((Object) this.cmdOugi_, (Object) null))
        this.onReset_ += new Battle01CommandNode.ResetDelegate(this.cmdOugi_.resetCurrentUnitPosition);
      if (Object.op_Inequality((Object) this.cmdSEA_, (Object) null))
        this.onReset_ += new Battle01CommandNode.ResetDelegate(this.cmdSEA_.resetCurrentUnitPosition);
    }
    Battle01CommandNode.ResetDelegate onReset = this.onReset_;
    if (onReset == null)
      return;
    onReset(bClear);
  }

  public void setSEAButtonActive(bool value, bool isDelay = false)
  {
    if (!Object.op_Inequality((Object) this.objSEA_, (Object) null) || this.objSEA_.activeSelf == value)
      return;
    if (isDelay && ((Component) this).gameObject.activeSelf)
      this.StartCoroutine(this.setSEAButtonActiveDelay(value));
    else
      this.objSEA_.SetActive(value);
  }

  private IEnumerator setSEAButtonActiveDelay(bool value)
  {
    yield return (object) new WaitForSeconds(this.delayTime);
    this.objSEA_.SetActive(value);
  }

  public enum CommandNo
  {
    Wait,
    Skill,
    Ougi,
    SEA,
  }

  [Flags]
  public enum CommandFlag
  {
    Nil = 0,
    Wait = 1,
    Skill = 2,
    Ougi = 4,
    SEA = 8,
  }

  private delegate void ResetDelegate(bool bClear);
}
