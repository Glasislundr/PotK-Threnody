// Decompiled with JetBrains decompiler
// Type: QuestStageEntryInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuestStageEntryInfo : MonoBehaviour
{
  [SerializeField]
  private GameObject normal;
  [SerializeField]
  private GameObject hurd;
  [SerializeField]
  private UILabel textNormal;
  [SerializeField]
  private UILabel textHurd;

  public void Awake() => this.IsDisplay = false;

  public string TextNormal
  {
    set => this.textNormal.SetTextLocalize(value);
  }

  public string TextHurd
  {
    set => this.textHurd.SetTextLocalize(value);
  }

  public bool Normal
  {
    get => this.normal.activeSelf;
    set
    {
      this.normal.SetActive(value);
      if (!value)
        return;
      this.IsDisplay = value;
    }
  }

  public bool Hurd
  {
    get => this.hurd.activeSelf;
    set
    {
      this.hurd.SetActive(value);
      if (!value)
        return;
      this.IsDisplay = value;
    }
  }

  public bool IsDisplay
  {
    set => ((Component) this).gameObject.SetActive(value);
  }
}
