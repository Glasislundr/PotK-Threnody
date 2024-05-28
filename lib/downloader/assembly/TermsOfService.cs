// Decompiled with JetBrains decompiler
// Type: TermsOfService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Agreement/TermsOfService")]
public class TermsOfService : MonoBehaviour
{
  private static int? update_date_;
  [SerializeField]
  [Tooltip("更新年月日(YYYYMMDD)")]
  private string UpdateDate_ = "20200401";
  [SerializeField]
  [Tooltip("rule.txt")]
  public TermsOfService.Content content_;
  [SerializeField]
  [Tooltip("privacy_policy.txt")]
  private TextAsset privacyPolicy_;

  public static int update_date
  {
    get
    {
      if (TermsOfService.update_date_.HasValue)
        return TermsOfService.update_date_.Value;
      TermsOfService.initialize();
      return TermsOfService.update_date_.Value;
    }
  }

  private static void initialize()
  {
    TermsOfService.update_date_ = new int?(int.Parse(TermsOfService.GetData().UpdateDate_, NumberStyles.AllowHexSpecifier));
  }

  public static TermsOfService GetData()
  {
    return Resources.Load<GameObject>("Agreement/TermsOfService").GetComponent<TermsOfService>();
  }

  public string UpdateDate
  {
    get => this.UpdateDate_;
    set
    {
    }
  }

  public TermsOfService.Content content
  {
    get
    {
      if (!this.content_.isLoaded)
        this.content_.loadText(this.content_.asset_.text);
      return this.content_;
    }
  }

  public string privacyPolicy => this.privacyPolicy_.text;

  private void Awake() => this.content_.loadText(this.content_.asset_.text);

  [Serializable]
  public class Content
  {
    public TextAsset asset_;

    public bool isLoaded { get; private set; }

    public string title { get; set; }

    public string header { get; set; }

    public string text { get; set; }

    public string titleDissent { get; set; }

    public string textDissent { get; set; }

    public void setTextAsset(TextAsset txtAsset)
    {
      this.asset_ = txtAsset;
      this.isLoaded = false;
      if (!Object.op_Inequality((Object) txtAsset, (Object) null))
        return;
      this.loadText(txtAsset.text);
    }

    public void loadText(string data)
    {
      if (this.isLoaded || string.IsNullOrEmpty(data))
        return;
      int num = 0;
      using (StringReader stringReader = new StringReader(data))
      {
        List<string> values1 = new List<string>();
        List<string> values2 = new List<string>();
        List<string> values3 = new List<string>();
        while (stringReader.Peek() != -1)
        {
          string str = stringReader.ReadLine();
          if (str.StartsWith("#"))
          {
            switch (str)
            {
              case "#title":
                num = 1;
                continue;
              case "#header":
                num = 2;
                continue;
              case "#main":
                num = 3;
                continue;
              case "#titleDissent":
                num = 4;
                continue;
              case "#dissent":
                num = 5;
                continue;
            }
          }
          switch (num)
          {
            case 1:
              this.title = str;
              continue;
            case 2:
              values1.Add(str);
              continue;
            case 3:
              values2.Add(str);
              continue;
            case 4:
              this.titleDissent = str;
              continue;
            case 5:
              values3.Add(str);
              continue;
            default:
              continue;
          }
        }
        this.header = string.Join("\n", (IEnumerable<string>) values1);
        this.text = string.Join("\n", (IEnumerable<string>) values2);
        this.textDissent = string.Join("\n", (IEnumerable<string>) values3);
      }
      this.isLoaded = true;
    }
  }
}
