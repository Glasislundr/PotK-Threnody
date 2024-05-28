// Decompiled with JetBrains decompiler
// Type: SceneButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public class SceneButton : NGMenuBase
{
  public string scene = "";
  [SerializeField]
  private string argument = "";
  [SerializeField]
  private bool reName;
  private const string typesSeparate = ":";
  private const string typeInt = "(int|i)";
  private const string patternInt = "[\\+-]?\\d+";
  private const string typeFloat = "(float|f)";
  private const string patternFloat = "[\\+-]?\\d?\\.\\d+";
  private const string typeString = "(string|s)";
  private const string patternString = "\"[^\"]*\"";
  private const string pattern = "((int|i):[\\+-]?\\d+|(float|f):[\\+-]?\\d?\\.\\d+|(string|s):\"[^\"]*\")";

  private void Start()
  {
    if (!this.reName)
      return;
    UILabel componentInChildren = ((Component) this).GetComponentInChildren<UILabel>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    if (string.IsNullOrEmpty(this.scene))
    {
      componentInChildren.SetText("BackScene");
    }
    else
    {
      string text = this.scene.Replace("_", " ");
      if (text.Length > 20)
        componentInChildren.SetText(text.Substring(0, 18) + "..");
      else
        componentInChildren.SetText(text);
    }
  }

  private object[] Args
  {
    get
    {
      if (string.IsNullOrEmpty(this.argument))
        return (object[]) null;
      MatchCollection matchCollection = new Regex("((int|i):[\\+-]?\\d+|(float|f):[\\+-]?\\d?\\.\\d+|(string|s):\"[^\"]*\")", RegexOptions.Singleline).Matches(this.argument);
      List<object> objectList = new List<object>();
      if (matchCollection.Count > 0)
      {
        string pattern1 = "(int|i):";
        string pattern2 = "(float|f):";
        string pattern3 = "(string|s):";
        foreach (object obj1 in matchCollection)
        {
          string input = obj1.ToString().Trim();
          object obj2;
          if (Regex.IsMatch(input, pattern1))
            obj2 = (object) int.Parse(Regex.Replace(input, pattern1, string.Empty));
          else if (Regex.IsMatch(input, pattern2))
          {
            obj2 = (object) float.Parse(Regex.Replace(input, pattern2, string.Empty));
          }
          else
          {
            string str = Regex.Replace(input, pattern3, string.Empty).Substring(1);
            obj2 = (object) str.Substring(0, str.Length - 1);
          }
          objectList.Add(obj2);
        }
      }
      return objectList.Count <= 0 ? (object[]) null : objectList.ToArray();
    }
  }

  public void onButtonScene()
  {
    if (string.IsNullOrEmpty(this.scene))
    {
      this.backScene();
    }
    else
    {
      object[] args = this.Args;
      if (args != null)
        Singleton<NGSceneManager>.GetInstance().changeScene(this.scene, true, args);
      else
        this.changeScene(this.scene);
    }
  }
}
