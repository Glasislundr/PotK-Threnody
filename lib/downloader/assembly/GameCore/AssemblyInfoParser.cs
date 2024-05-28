// Decompiled with JetBrains decompiler
// Type: GameCore.AssemblyInfoParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace GameCore
{
  internal class AssemblyInfoParser
  {
    private int name = -1;
    private int culture = -1;
    private int version = -1;
    private int public_key_token = -1;

    public static AssemblyInfoParser Parse(char[] type, int p)
    {
      AssemblyInfoParser assemblyInfoParser = new AssemblyInfoParser();
      bool flag1 = false;
      if (type[p] == '"')
      {
        flag1 = true;
        ++p;
      }
      assemblyInfoParser.name = p;
      while (type[p] != char.MinValue && (util.isalnum(type[p]) || type[p] == '.' || type[p] == '-' || type[p] == '_' || type[p] == '$' || type[p] == '@' || type[p] == ' '))
        ++p;
      if (flag1)
      {
        if (type[p] != '"')
          return assemblyInfoParser;
        type[p++] = char.MinValue;
      }
      if (type[p] != ',')
        return assemblyInfoParser;
      type[p] = char.MinValue;
      int index = p - 1;
      while (type[index] != char.MinValue && util.isspace(type[index]))
        type[index--] = char.MinValue;
      ++p;
      while (util.isspace(type[p]))
        ++p;
      while (type[p] != char.MinValue)
      {
        if (type[p] == 'V' && util.strncasecmp(type, p, "Version=", 0, 8) == 0)
        {
          p += 8;
          assemblyInfoParser.version = p;
          while (type[p] != char.MinValue && type[p] != ',')
            ++p;
        }
        else if (type[p] == 'C' && util.strncasecmp(type, p, "Culture=", 0, 8) == 0)
        {
          p += 8;
          if (util.strncasecmp(type, p, "neutral", 0, 7) == 0)
          {
            assemblyInfoParser.culture = type.Length - 1;
            p += 7;
          }
          else
          {
            assemblyInfoParser.culture = p;
            while (type[p] != char.MinValue && type[p] != ',')
              ++p;
          }
        }
        else if (type[p] == 'P' && util.strncasecmp(type, p, "PublicKeyToken=", 0, 15) == 0)
        {
          p += 15;
          if (util.strncmp(type, p, "null", 0, 4) == 0)
          {
            p += 4;
          }
          else
          {
            assemblyInfoParser.public_key_token = p;
            while (type[p] != char.MinValue && type[p] != ',')
              ++p;
          }
        }
        else
        {
          while (type[p] != char.MinValue && type[p] != ',')
            ++p;
        }
        bool flag2 = false;
        while (util.isspace(type[p]) || type[p] == ',')
        {
          type[p++] = char.MinValue;
          flag2 = true;
        }
        if (!flag2)
          return assemblyInfoParser;
      }
      return (AssemblyInfoParser) null;
    }

    public static AssemblyInfo ToAssemblyInfo(AssemblyInfoParser info, char[] type)
    {
      Func<int, string> func = (Func<int, string>) (s =>
      {
        if (s == -1)
          return (string) null;
        int index = s;
        while (type[index] != char.MinValue)
          ++index;
        return new string(type, s, index - s);
      });
      return new AssemblyInfo()
      {
        name = func(info.name),
        culture = func(info.culture),
        version = func(info.version),
        public_key_token = func(info.public_key_token)
      };
    }
  }
}
