// Decompiled with JetBrains decompiler
// Type: GameCore.TypeInfoParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  internal class TypeInfoParser
  {
    private int name_space = -1;
    private List<int> _nested;
    private int name = -1;
    private List<int> _modifiers;
    private List<TypeInfoParser> _type_arguments;
    private AssemblyInfoParser assembly;

    private List<int> nested
    {
      get
      {
        if (this._nested == null)
          this._nested = new List<int>();
        return this._nested;
      }
    }

    private List<int> modifiers
    {
      get
      {
        if (this._modifiers == null)
          this._modifiers = new List<int>();
        return this._modifiers;
      }
    }

    private List<TypeInfoParser> type_arguments
    {
      get
      {
        if (this._type_arguments == null)
          this._type_arguments = new List<TypeInfoParser>();
        return this._type_arguments;
      }
    }

    public static TypeInfo Parse(string type)
    {
      int p = 0;
      char[] type1 = new char[type.Length + 1];
      for (int index = 0; index < type.Length; ++index)
        type1[index] = type[index];
      type1[type.Length] = char.MinValue;
      TypeInfoParser info = TypeInfoParser.Parse(type1, ref p, false);
      return info == null ? (TypeInfo) null : TypeInfoParser.ToTypeInfo(info, type1);
    }

    private static string ToStr(char[] type, int s)
    {
      if (s == -1)
        return (string) null;
      int index = s;
      while (type[index] != char.MinValue)
        ++index;
      return new string(type, s, index - s);
    }

    private static TypeInfo ToTypeInfo(TypeInfoParser info, char[] type)
    {
      TypeInfo typeInfo = new TypeInfo();
      typeInfo.name_space = info.name_space == -1 ? "" : TypeInfoParser.ToStr(type, info.name_space);
      typeInfo.nested = info._nested == null ? new string[0] : info.nested.Select<int, string>((Func<int, string>) (x => TypeInfoParser.ToStr(type, x))).ToArray<string>();
      typeInfo.name = TypeInfoParser.ToStr(type, info.name);
      typeInfo.modifiers = info._modifiers == null ? new int[0] : info.modifiers.ToArray();
      typeInfo.type_arguments = info._type_arguments == null ? new TypeInfo[0] : info._type_arguments.Select<TypeInfoParser, TypeInfo>((Func<TypeInfoParser, TypeInfo>) (x => TypeInfoParser.ToTypeInfo(x, type))).ToArray<TypeInfo>();
      typeInfo.assembly = info.assembly == null ? (AssemblyInfo) null : AssemblyInfoParser.ToAssemblyInfo(info.assembly, type);
      TypeInfoCache.Register(typeInfo.AssemblyQualifiedName, typeInfo);
      return typeInfo;
    }

    private static TypeInfoParser Parse(char[] type, ref int p, bool is_recursed)
    {
      int num1 = p;
      bool flag1 = false;
      bool flag2 = false;
      int index = -1;
      int num2 = 0;
      while (type[p] == ' ')
      {
        ++num1;
        ++p;
      }
      TypeInfoParser typeInfoParser1 = new TypeInfoParser();
      while (type[p] != char.MinValue)
      {
        switch (type[p])
        {
          case '&':
          case '*':
          case ',':
          case '[':
          case ']':
            flag2 = true;
            break;
          case '+':
            type[p] = char.MinValue;
            typeInfoParser1.nested.Add(p + 1);
            if (typeInfoParser1.name == -1)
            {
              if (index != -1)
              {
                typeInfoParser1.name_space = num1;
                type[index] = char.MinValue;
                typeInfoParser1.name = index + 1;
                break;
              }
              typeInfoParser1.name_space = type.Length - 1;
              typeInfoParser1.name = num1;
              break;
            }
            break;
          case '.':
            index = p;
            break;
          case '\\':
            ++p;
            break;
          case '`':
            type[p] = char.MinValue;
            break;
        }
        if (!flag2)
          ++p;
        else
          break;
      }
      if (typeInfoParser1.name == -1)
      {
        if (index != -1)
        {
          typeInfoParser1.name_space = num1;
          type[index] = char.MinValue;
          typeInfoParser1.name = index + 1;
        }
        else
        {
          typeInfoParser1.name_space = type.Length - 1;
          typeInfoParser1.name = num1;
        }
      }
      while (type[p] != char.MinValue)
      {
        switch (type[p])
        {
          case '&':
            if (flag1)
              return (TypeInfoParser) null;
            flag1 = true;
            typeInfoParser1.modifiers.Add(0);
            type[p++] = char.MinValue;
            break;
          case '*':
            typeInfoParser1.modifiers.Add(-1);
            type[p++] = char.MinValue;
            break;
          case ',':
            if (!is_recursed)
            {
              type[p++] = char.MinValue;
              while (type[p] != char.MinValue && util.isspace(type[p]))
                ++p;
              if (type[p] == char.MinValue)
                return (TypeInfoParser) null;
              typeInfoParser1.assembly = AssemblyInfoParser.Parse(type, p);
              if (typeInfoParser1.assembly == null)
                return (TypeInfoParser) null;
              break;
            }
            goto label_81;
          case '[':
            type[p++] = char.MinValue;
            if (type[p] == char.MinValue)
              return (TypeInfoParser) null;
            if (type[p] == ',' || type[p] == '*' || type[p] == ']')
            {
              num2 = 1;
              while (type[p] != char.MinValue && type[p] != ']')
              {
                if (type[p] == ',')
                {
                  ++num2;
                }
                else
                {
                  if (type[p] != '*')
                    return (TypeInfoParser) null;
                  typeInfoParser1.modifiers.Add(-2);
                }
                ++p;
              }
              if (type[p++] != ']')
                return (TypeInfoParser) null;
              typeInfoParser1.modifiers.Add(num2);
              break;
            }
            if (num2 != 0)
              return (TypeInfoParser) null;
            while (type[p] != char.MinValue)
            {
              bool flag3 = false;
              if (type[p] == '[')
              {
                ++p;
                flag3 = true;
              }
              TypeInfoParser typeInfoParser2 = TypeInfoParser.Parse(type, ref p, true);
              if (typeInfoParser2 == null)
                return (TypeInfoParser) null;
              typeInfoParser1.type_arguments.Add(typeInfoParser2);
              if (flag3 && type[p] != ']')
              {
                if (type[p] != ',')
                  return (TypeInfoParser) null;
                type[p++] = char.MinValue;
                int p1 = p;
                while (type[p] != char.MinValue && type[p] != ']')
                  ++p;
                if (type[p] != ']')
                  return (TypeInfoParser) null;
                type[p++] = char.MinValue;
                while (type[p1] != char.MinValue && util.isspace(type[p1]))
                  ++p1;
                if (type[p1] == char.MinValue)
                  return (TypeInfoParser) null;
                typeInfoParser2.assembly = AssemblyInfoParser.Parse(type, p1);
                if (typeInfoParser2.assembly == null)
                  return (TypeInfoParser) null;
              }
              else if (flag3 && type[p] == ']')
                type[p++] = char.MinValue;
              if (type[p] == ']')
              {
                type[p++] = char.MinValue;
                break;
              }
              if (type[p] == char.MinValue)
                return (TypeInfoParser) null;
              type[p++] = char.MinValue;
            }
            break;
          case ']':
            if (!is_recursed)
              return (TypeInfoParser) null;
            goto label_81;
          default:
            return (TypeInfoParser) null;
        }
        if (typeInfoParser1.assembly != null)
          break;
      }
label_81:
      return typeInfoParser1.name == -1 ? (TypeInfoParser) null : typeInfoParser1;
    }
  }
}
