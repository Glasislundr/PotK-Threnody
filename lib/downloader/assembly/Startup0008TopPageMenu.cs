// Decompiled with JetBrains decompiler
// Type: Startup0008TopPageMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Startup0008TopPageMenu : MonoBehaviour
{
  [SerializeField]
  private UITextList agreementTextList;
  [SerializeField]
  private UILabel agreementTitle;

  public void Initialize(string title, string hedder, string text, string privacy_policy = null)
  {
    this.agreementTitle.SetText(title);
    this.agreementTextList.Clear();
    this.agreementTextList.Add(hedder);
    this.agreementTextList.Add(text);
    if (string.IsNullOrEmpty(privacy_policy))
      return;
    this.agreementTextList.Add(privacy_policy);
  }

  public IEnumerator ScrollValue()
  {
    yield return (object) null;
    this.agreementTextList.scrollValue = 0.0f;
    if ((double) this.agreementTextList.scrollValue != 0.0)
      this.agreementTextList.scrollValue = 0.0f;
  }
}
