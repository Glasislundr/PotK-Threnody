// Decompiled with JetBrains decompiler
// Type: StoryUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
public class StoryUtility
{
  private static Regex lineRegexp = new Regex("(?<prefix>(#|@))\\s*((?<command>\\S+)\\s+(?<args>.*)|(?<command>\\S+)|)");

  private static string lineParser(string line, out string command, out string args)
  {
    Match match = StoryUtility.lineRegexp.Match(line);
    if (match.Success)
    {
      command = match.Groups[nameof (command)].Value;
      args = match.Groups[nameof (args)].Value;
      return match.Groups["prefix"].Value;
    }
    command = (string) null;
    args = (string) null;
    return (string) null;
  }

  public static Tuple<IEnumerable<UnitUnit>, IEnumerable<string>> GetResource(int scriptId)
  {
    return StoryUtility.GetResource(MasterData.ScriptScript.ContainsKey(scriptId) ? MasterData.ScriptScript[scriptId].script : string.Empty);
  }

  public static Tuple<IEnumerable<UnitUnit>, IEnumerable<string>> GetResource(string txtScript)
  {
    if (string.IsNullOrEmpty(txtScript))
      return (Tuple<IEnumerable<UnitUnit>, IEnumerable<string>>) null;
    string[] strArray1 = txtScript.Split('\n');
    HashSet<UnitUnit> unitUnitSet = new HashSet<UnitUnit>();
    HashSet<string> stringSet = new HashSet<string>();
    foreach (string line in strArray1)
    {
      if (!line.StartsWith(";") && line.Length != 0)
      {
        string args = (string) null;
        string command = (string) null;
        if (StoryUtility.lineParser(line, out command, out args) == "#")
        {
          string[] strArray2 = args.Split(' ', '\t');
          if (strArray2.Length != 0)
          {
            switch (command)
            {
              case "background":
                string str1 = strArray2[0].Replace("\"", "");
                if (!string.IsNullOrEmpty(str1))
                {
                  stringSet.Add("Prefabs/BackGround/" + str1);
                  continue;
                }
                continue;
              case "bgmfile":
                string bgmPath = strArray2[1].Replace("\"", "");
                if (!string.IsNullOrEmpty(bgmPath))
                {
                  foreach (string str2 in Singleton<ResourceManager>.GetInstance().PathsFromBgm(bgmPath))
                    stringSet.Add(str2);
                  continue;
                }
                continue;
              case "body":
              case "chara":
                int result1;
                if (int.TryParse(strArray2[0], out result1))
                {
                  if (MasterData.UnitUnit.ContainsKey(result1))
                  {
                    unitUnitSet.Add(MasterData.UnitUnit[result1]);
                    continue;
                  }
                  foreach (string str3 in Singleton<ResourceManager>.GetInstance().PathsFromMobUnit(result1))
                    stringSet.Add(str3);
                  continue;
                }
                continue;
              case "entry":
                int result2;
                if (strArray2.Length > 1 && int.TryParse(strArray2[1], out result2))
                {
                  if (MasterData.UnitUnit.ContainsKey(result2))
                  {
                    unitUnitSet.Add(MasterData.UnitUnit[result2]);
                    continue;
                  }
                  foreach (string str4 in Singleton<ResourceManager>.GetInstance().PathsFromMobUnit(result2))
                    stringSet.Add(str4);
                  continue;
                }
                continue;
              case "imageset":
                string str5 = strArray2[1].Replace("\"", "");
                if (!string.IsNullOrEmpty(str5))
                {
                  stringSet.Add("AssetBundle/Resources/EventImages/" + str5);
                  continue;
                }
                continue;
              case "movieplay":
                string moviePath = Singleton<NGSoundManager>.GetInstance().platform + "/" + strArray2[0].Replace("\"", "");
                if (!string.IsNullOrEmpty(moviePath))
                {
                  foreach (string str6 in Singleton<ResourceManager>.GetInstance().PathsFromMovie(moviePath))
                    stringSet.Add(str6);
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    return new Tuple<IEnumerable<UnitUnit>, IEnumerable<string>>((IEnumerable<UnitUnit>) unitUnitSet, (IEnumerable<string>) stringSet);
  }
}
