// Decompiled with JetBrains decompiler
// Type: Startup00016Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Startup00016Menu : MonoBehaviour
{
  [SerializeField]
  private UITextList textList;
  private bool flag;

  public void InitScene()
  {
    string privacyPolicy = TermsOfService.GetData().privacyPolicy;
    this.textList.Clear();
    this.textList.Add(privacyPolicy);
    this.textList.scrollValue = 0.0f;
    this.flag = true;
  }

  private void Update()
  {
    if (!this.flag || (double) this.textList.scrollValue == 0.0)
      return;
    this.flag = false;
    this.textList.scrollValue = 0.0f;
  }
}
