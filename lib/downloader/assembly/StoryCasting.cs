// Decompiled with JetBrains decompiler
// Type: StoryCasting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;

#nullable disable
public class StoryCasting
{
  private Dictionary<string, StoryCasting.CastUnit> dicCast_ = new Dictionary<string, StoryCasting.CastUnit>();
  private const char FIRST_CAST = 'a';

  public void setCasting(int unitAID, params int[] otherids)
  {
    this.dicCast_.Clear();
    char ch = 'a';
    this.dicCast_.Add(ch.ToString(), new StoryCasting.CastUnit(unitAID));
    for (int index = 0; index < otherids.Length; ++index)
    {
      ++ch;
      this.dicCast_.Add(ch.ToString(), new StoryCasting.CastUnit(otherids[index]));
    }
  }

  public void setQuestion(int id, params int[] otherids)
  {
    char ch = 'a';
    string key1 = ch.ToString();
    if (this.dicCast_.ContainsKey(key1))
      this.dicCast_[key1].setQuestion(id);
    for (int index = 0; index < otherids.Length; ++index)
    {
      ++ch;
      string key2 = ch.ToString();
      if (this.dicCast_.ContainsKey(key2))
        this.dicCast_[key2].setQuestion(otherids[index]);
    }
  }

  public void setPatchScript(
    Dictionary<string, string> scripts,
    params Dictionary<string, string>[] otherscripts)
  {
    char ch = 'a';
    if (this.dicCast_.ContainsKey(ch.ToString()))
      this.dicCast_[ch.ToString()].setPatch(scripts);
    for (int index = 0; index < otherscripts.Length; ++index)
    {
      ++ch;
      if (this.dicCast_.ContainsKey(ch.ToString()))
        this.dicCast_[ch.ToString()].setPatch(otherscripts[index]);
    }
  }

  public string buildScript(string baseScript, Hashtable replaseExtension)
  {
    return this.replaceText(this.overwriteText(this.removeComment(baseScript)), replaseExtension);
  }

  private string removeComment(string txtScript)
  {
    string[] separator = new string[2]{ "\r\n", "\n" };
    string[] strArray = txtScript.Split(separator, StringSplitOptions.None);
    List<string> stringList = new List<string>();
    foreach (string str in strArray)
    {
      if (!str.StartsWith(";"))
        stringList.Add(str);
    }
    return string.Join("\n", stringList.ToArray());
  }

  private string overwriteText(string text)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    string G_UNIT = "UNIT";
    string G_ID = "ID";
    string G_TEXTBLOCK = "TBLOCK";
    string str1 = "[\\t 　]";
    string str2 = "(" + str1 + "*)";
    string str3 = "(" + str1 + "*\\n?)";
    StoryCasting.CastUnit castUnit;
    string str4;
    return new Regex("overwrite_(?<" + G_UNIT + ">\\w{1})" + str2 + "\\(" + str2 + "(?<" + G_ID + ">\\w+)" + str2 + "\\)" + str2 + "\\{" + str3 + "(?<" + G_TEXTBLOCK + ">[^}]*)\\}" + str3).Replace(text, (MatchEvaluator) (match => this.dicCast_.TryGetValue(match.Groups[G_UNIT].Value, out castUnit) && castUnit.dicScript_.TryGetValue(match.Groups[G_ID].Value, out str4) ? str4 : match.Groups[G_TEXTBLOCK].Value));
  }

  private string replaceText(string text, Hashtable replaseExtension)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    string G_UNIT = "UNIT";
    string G_FUNC = "FUNC";
    Regex regex = new Regex("%\\(([^)]+)\\)s");
    Regex replaceValue = new Regex("^unit_(?<" + G_UNIT + ">\\w{1}$)");
    Regex funcPattern = new Regex("(?<" + G_FUNC + ">^\\w+):unit_(?<" + G_UNIT + ">\\w{1}$)");
    return regex.Replace(text, (MatchEvaluator) (match =>
    {
      string str1 = match.Groups[1].Value;
      Match match1 = replaceValue.Match(str1);
      StoryCasting.CastUnit castUnit1;
      if (match1.Success && this.dicCast_.TryGetValue(match1.Groups[G_UNIT].Value, out castUnit1))
        return castUnit1.characterId_;
      if (replaseExtension != null && replaseExtension.ContainsKey((object) str1))
        return (string) replaseExtension[(object) str1];
      Match match2 = funcPattern.Match(str1);
      StoryCasting.CastUnit castUnit2;
      if (match2.Success && this.dicCast_.TryGetValue(match2.Groups[G_UNIT].Value, out castUnit2))
      {
        string str2 = castUnit2.execute(match2.Groups[G_FUNC].Value);
        if (!string.IsNullOrEmpty(str2))
          return str2;
      }
      return match.Groups[0].Value;
    }));
  }

  private class CastUnit
  {
    private static string KEY_QUESTION = "uq";

    public int id_ { get; private set; }

    public int refId_ { get; private set; }

    public UnitUnit unit_ { get; private set; }

    public string characterId_ { get; private set; }

    public Dictionary<string, string> dicScript_ { get; private set; }

    public CastUnit(int id)
    {
      this.id_ = id;
      this.unit_ = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).FirstOrDefault<UnitUnit>((Func<UnitUnit, bool>) (u => u.ID == id));
      int refId = this.unit_ != null ? this.unit_.resource_reference_unit_id_UnitUnit : 0;
      this.refId_ = refId;
      this.characterId_ = refId.ToString();
      this.dicScript_ = MasterData.DateScriptPartsList != null ? ((IEnumerable<DateScriptParts>) MasterData.DateScriptPartsList).Where<DateScriptParts>((Func<DateScriptParts, bool>) (sc => sc.unitID == refId)).ToDictionary<DateScriptParts, string, string>((Func<DateScriptParts, string>) (sc => sc.scriptID), (Func<DateScriptParts, string>) (sc => sc.script)) : new Dictionary<string, string>();
      this.updatePatch();
    }

    public void setQuestion(int id)
    {
      DateScriptQuestion dateScriptQuestion = MasterData.DateScriptQuestionList != null ? ((IEnumerable<DateScriptQuestion>) MasterData.DateScriptQuestionList).FirstOrDefault<DateScriptQuestion>((Func<DateScriptQuestion, bool>) (q => q.unitID == this.refId_ && q.questionID == id)) : (DateScriptQuestion) null;
      if (dateScriptQuestion == null || string.IsNullOrEmpty(dateScriptQuestion.script))
        return;
      this.dicScript_.Add(StoryCasting.CastUnit.KEY_QUESTION, dateScriptQuestion.script);
    }

    public void setPatch(Dictionary<string, string> patches)
    {
      if (patches == null || !patches.Any<KeyValuePair<string, string>>())
        return;
      foreach (KeyValuePair<string, string> patch in patches)
      {
        if (this.dicScript_.ContainsKey(patch.Key))
          this.dicScript_[patch.Key] = patch.Value;
        else
          this.dicScript_.Add(patch.Key, patch.Value);
      }
      this.updatePatch();
    }

    private void updatePatch()
    {
      string str;
      if (!this.dicScript_.TryGetValue("chara", out str))
        return;
      this.characterId_ = str;
    }

    public string execute(string func)
    {
      return !string.IsNullOrEmpty(func) && func == "unitName" && this.unit_ != null ? this.unit_.name : string.Empty;
    }
  }
}
