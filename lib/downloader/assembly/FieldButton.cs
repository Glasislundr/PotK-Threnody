// Decompiled with JetBrains decompiler
// Type: FieldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FieldButton : MonoBehaviour
{
  [SerializeField]
  private GameObject on;
  [SerializeField]
  private GameObject off;
  private FieldButton.State state;

  private void Awake()
  {
    this.on.SetActive(false);
    this.off.SetActive(false);
    this.state = FieldButton.State.none;
  }

  public bool isDown
  {
    get => this.state == FieldButton.State.on;
    set
    {
      if (value)
      {
        this.state = FieldButton.State.on;
        this.onDown();
      }
      else
      {
        this.state = FieldButton.State.off;
        this.onUp();
      }
    }
  }

  public bool isActive
  {
    get => ((Component) this).gameObject.activeSelf;
    set
    {
      ((Component) this).gameObject.SetActive(value);
      if (!value)
        return;
      this.state = FieldButton.State.off;
      this.onUp();
    }
  }

  private void onDown()
  {
    this.on.SetActive(true);
    this.off.SetActive(false);
  }

  private void onUp()
  {
    this.on.SetActive(false);
    this.off.SetActive(true);
  }

  private enum State
  {
    none,
    on,
    off,
  }
}
