// Decompiled with JetBrains decompiler
// Type: TextBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
public class TextBlock
{
  private string text_ = "";
  public TextBlock.Position pos = TextBlock.Position.BOTTOM;
  private Hashtable replaceTable_ = TextBlock.createReplaceTable();
  private Dictionary<string, Func<string, string>> replaceFuncTable_ = TextBlock.createFuncReplaceTable();

  public string text => this.text_;

  public void setText(string text) => this.text_ = this.decorateTextL(text);

  public void addText(string text) => this.text_ += this.decorateTextL(text);

  private string decorateTextL(string text)
  {
    return TextBlock.replaceText(text, (IDictionary) this.replaceTable_, this.replaceFuncTable_);
  }

  public static string decorateText(string text)
  {
    return TextBlock.replaceText(text, (IDictionary) TextBlock.createReplaceTable());
  }

  private static string replaceText(
    string text,
    IDictionary args,
    Dictionary<string, Func<string, string>> argfuncs = null)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    Regex regex = new Regex("%\\(([^)]+)\\)s");
    Regex regIntReplace = new Regex("(^\\w+):(.+$)");
    bool disableArgFuncs = argfuncs == null || argfuncs.Count == 0;
    string input = text;
    MatchEvaluator evaluator = (MatchEvaluator) (match =>
    {
      string str1 = match.Groups[1].Value;
      if (args.Contains((object) str1))
        return args[(object) str1].ToString();
      return disableArgFuncs ? "" : regIntReplace.Replace(str1, (MatchEvaluator) (im =>
      {
        string key = im.Groups[1].Value;
        string str2 = im.Groups[2].Value;
        return argfuncs.ContainsKey(key) ? argfuncs[key](str2) : "";
      }));
    });
    return regex.Replace(input, evaluator);
  }

  private static string correctText(string text)
  {
    return string.IsNullOrEmpty(text) || text.Length <= 3 ? text : new TextBlock.BBCodeAnalyzer().correctText(text);
  }

  private static Hashtable createReplaceTable()
  {
    return new Hashtable()
    {
      {
        (object) "userName",
        (object) TextBlock.getUserName()
      },
      {
        (object) "USERNAME",
        (object) TextBlock.getUSERNAME()
      }
    };
  }

  private static string getUSERNAME() => string.Format("\"{0}\"", (object) TextBlock.getUserName());

  private static string getUserName() => TextBlock.correctText(SMManager.Get<Player>().name);

  private static Dictionary<string, Func<string, string>> createFuncReplaceTable()
  {
    return new Dictionary<string, Func<string, string>>();
  }

  private static string toNowDate(string form) => ServerTime.NowAppTimeAddDelta().ToString(form);

  private static string toBeforeDay(string baseDate)
  {
    string beforeDay = baseDate;
    DateTime result;
    if (DateTime.TryParse(baseDate, out result))
    {
      TimeSpan timeSpan = result - ServerTime.NowAppTimeAddDelta();
      beforeDay = (timeSpan.TotalDays <= 0.0 ? timeSpan - timeSpan : timeSpan + new TimeSpan(863999999999L)).Days.ToString();
    }
    return beforeDay;
  }

  private static string toAfterDay(string baseDate)
  {
    string afterDay = baseDate;
    DateTime result;
    if (DateTime.TryParse(baseDate, out result))
    {
      TimeSpan timeSpan = ServerTime.NowAppTimeAddDelta() - result;
      afterDay = (timeSpan.TotalDays <= 0.0 ? timeSpan - timeSpan : timeSpan + new TimeSpan(863999999999L)).Days.ToString();
    }
    return afterDay;
  }

  public enum Position
  {
    TOP,
    BOTTOM,
  }

  private class BBCode
  {
    public int countNest_;
    public const int MIN_LENGTH = 3;

    public string patternBegin_ { get; private set; }

    public string patternEnd_ { get; private set; }

    public string codeEnd_ { get; private set; }

    public BBCode(string patternBegin, string patternEnd, string codeEnd)
    {
      this.patternBegin_ = patternBegin;
      this.patternEnd_ = patternEnd;
      this.codeEnd_ = codeEnd;
    }

    public void addNest() => ++this.countNest_;

    public void subNest()
    {
      if (this.countNest_ <= 0)
        return;
      --this.countNest_;
    }
  }

  private class BBCodeAnalyzer
  {
    private TextBlock.BBCode[] bbcodes_ = new TextBlock.BBCode[8]
    {
      new TextBlock.BBCode("\\[[0-9a-f]{6}\\]", "\\[\\-\\]", "[-]"),
      new TextBlock.BBCode("\\[b\\]", "\\[\\/b\\]", "[/b]"),
      new TextBlock.BBCode("\\[i\\]", "\\[\\/i\\]", "[/i]"),
      new TextBlock.BBCode("\\[u\\]", "\\[\\/u\\]", "[/u]"),
      new TextBlock.BBCode("\\[s\\]", "\\[\\/s\\]", "[/s]"),
      new TextBlock.BBCode("\\[sub\\]", "\\[\\/sub\\]", "[/sub]"),
      new TextBlock.BBCode("\\[sup\\]", "\\[\\/sup\\]", "[/sup]"),
      new TextBlock.BBCode("\\[url=.*?\\]", "\\[\\/url\\]", "[/url]")
    };

    private string beginPatterns
    {
      get
      {
        string beginPatterns = "";
        for (int index = 0; index < this.bbcodes_.Length; ++index)
          beginPatterns = beginPatterns + (index == 0 ? "(" : "|(") + this.bbcodes_[index].patternBegin_ + ")";
        return beginPatterns;
      }
    }

    private string endPatterns
    {
      get
      {
        string endPatterns = "";
        for (int index = 0; index < this.bbcodes_.Length; ++index)
          endPatterns = endPatterns + (index == 0 ? "(" : "|(") + this.bbcodes_[index].patternEnd_ + ")";
        return endPatterns;
      }
    }

    public string correctText(string text)
    {
      if (string.IsNullOrEmpty(text) || text.Length <= 3)
        return text;
      this.resetNest();
      MatchCollection matchCollection = Regex.Matches(text, this.beginPatterns + "|" + this.endPatterns);
      List<TextBlock.BBCode> bbCodeList = new List<TextBlock.BBCode>();
      foreach (Match match in matchCollection)
      {
        int groupnum;
        for (groupnum = 1; groupnum <= this.bbcodes_.Length; ++groupnum)
        {
          if (match.Groups[groupnum].Success)
          {
            TextBlock.BBCode bbcode = this.bbcodes_[groupnum - 1];
            bbcode.addNest();
            bbCodeList.Add(bbcode);
            break;
          }
        }
        if (groupnum > this.bbcodes_.Length)
        {
          for (int count = match.Groups.Count; groupnum < count; ++groupnum)
          {
            if (match.Groups[groupnum].Success)
            {
              this.bbcodes_[groupnum - 1 - this.bbcodes_.Length].subNest();
              break;
            }
          }
        }
      }
      string str = text;
      bbCodeList.Reverse();
      foreach (TextBlock.BBCode bbCode in bbCodeList)
      {
        if (bbCode.countNest_ > 0)
        {
          str += bbCode.codeEnd_;
          --bbCode.countNest_;
        }
      }
      return str;
    }

    private void resetNest()
    {
      for (int index = 0; index < this.bbcodes_.Length; ++index)
        this.bbcodes_[index].countNest_ = 0;
    }

    private TextBlock.BBCode beginToBBCode(string text)
    {
      for (int index = 0; index < this.bbcodes_.Length; ++index)
      {
        if (Regex.IsMatch(text, this.bbcodes_[index].patternBegin_))
          return this.bbcodes_[index];
      }
      return (TextBlock.BBCode) null;
    }

    private TextBlock.BBCode endToBBCode(string text)
    {
      for (int index = 0; index < this.bbcodes_.Length; ++index)
      {
        if (Regex.IsMatch(text, this.bbcodes_[index].patternEnd_))
          return this.bbcodes_[index];
      }
      return (TextBlock.BBCode) null;
    }
  }
}
