// Decompiled with JetBrains decompiler
// Type: NGInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class NGInput : Singleton<NGInput>
{
  [SerializeField]
  private bool disable;
  private float timer;

  public bool Disable
  {
    get => this.disable;
    set
    {
      if (value)
      {
        this.timer = 0.5f;
        if (!this.disable)
          this.OpenDisable();
        this.disable = true;
      }
      else
      {
        if ((double) this.timer >= 0.0)
          return;
        this.disable = false;
      }
    }
  }

  private void OpenDisable()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlockAutoClose = true;
    this.StartCoroutine(this.CloseDisable());
  }

  private IEnumerator CloseDisable()
  {
    do
    {
      this.timer -= Time.deltaTime;
      yield return (object) null;
    }
    while ((double) this.timer >= 0.0);
    this.Disable = false;
  }

  protected override void Initialize() => this.disable = false;

  public bool GetKeyUp(string code) => !this.Disable && Input.GetKeyUp(code);

  public bool GetKeyUp(KeyCode code) => !this.Disable && Input.GetKeyUp(code);
}
