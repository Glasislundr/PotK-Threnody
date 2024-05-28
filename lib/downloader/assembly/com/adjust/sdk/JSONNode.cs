﻿// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.JSONNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace com.adjust.sdk
{
  public class JSONNode
  {
    public virtual void Add(string aKey, JSONNode aItem)
    {
    }

    public virtual JSONNode this[int aIndex]
    {
      get => (JSONNode) null;
      set
      {
      }
    }

    public virtual JSONNode this[string aKey]
    {
      get => (JSONNode) null;
      set
      {
      }
    }

    public virtual string Value
    {
      get => "";
      set
      {
      }
    }

    public virtual int Count => 0;

    public virtual void Add(JSONNode aItem) => this.Add("", aItem);

    public virtual JSONNode Remove(string aKey) => (JSONNode) null;

    public virtual JSONNode Remove(int aIndex) => (JSONNode) null;

    public virtual JSONNode Remove(JSONNode aNode) => aNode;

    public virtual IEnumerable<JSONNode> Childs
    {
      get
      {
        yield break;
      }
    }

    public IEnumerable<JSONNode> DeepChilds
    {
      get
      {
        foreach (JSONNode child in this.Childs)
        {
          foreach (JSONNode deepChild in child.DeepChilds)
            yield return deepChild;
        }
      }
    }

    public override string ToString() => nameof (JSONNode);

    public virtual string ToString(string aPrefix) => nameof (JSONNode);

    public virtual int AsInt
    {
      get
      {
        int result = 0;
        return int.TryParse(this.Value, out result) ? result : 0;
      }
      set => this.Value = value.ToString();
    }

    public virtual float AsFloat
    {
      get
      {
        float result = 0.0f;
        return float.TryParse(this.Value, out result) ? result : 0.0f;
      }
      set => this.Value = value.ToString();
    }

    public virtual double AsDouble
    {
      get
      {
        double result = 0.0;
        return double.TryParse(this.Value, out result) ? result : 0.0;
      }
      set => this.Value = value.ToString();
    }

    public virtual bool AsBool
    {
      get
      {
        bool result = false;
        return bool.TryParse(this.Value, out result) ? result : !string.IsNullOrEmpty(this.Value);
      }
      set => this.Value = value ? "true" : "false";
    }

    public virtual JSONArray AsArray => this as JSONArray;

    public virtual JSONClass AsObject => this as JSONClass;

    public static implicit operator JSONNode(string s) => (JSONNode) new JSONData(s);

    public static implicit operator string(JSONNode d)
    {
      return !(d == (object) null) ? d.Value : (string) null;
    }

    public static bool operator ==(JSONNode a, object b)
    {
      return b == null && (object) (a as JSONLazyCreator) != null || (object) a == b;
    }

    public static bool operator !=(JSONNode a, object b) => !(a == b);

    public override bool Equals(object obj) => (object) this == obj;

    public override int GetHashCode() => base.GetHashCode();

    internal static string Escape(string aText)
    {
      string str = "";
      foreach (char ch in aText)
      {
        switch (ch)
        {
          case '\b':
            str += "\\b";
            break;
          case '\t':
            str += "\\t";
            break;
          case '\n':
            str += "\\n";
            break;
          case '\f':
            str += "\\f";
            break;
          case '\r':
            str += "\\r";
            break;
          case '"':
            str += "\\\"";
            break;
          case '\\':
            str += "\\\\";
            break;
          default:
            str += ch.ToString();
            break;
        }
      }
      return str;
    }

    public static JSONNode Parse(string aJSON)
    {
      Stack<JSONNode> jsonNodeStack = new Stack<JSONNode>();
      JSONNode jsonNode = (JSONNode) null;
      int index = 0;
      string aItem = "";
      string aKey1 = "";
      bool flag = false;
      for (; index < aJSON.Length; ++index)
      {
        char ch1 = aJSON[index];
        switch (ch1)
        {
          case '\t':
          case ' ':
            if (flag)
            {
              string str1 = aItem;
              ch1 = aJSON[index];
              string str2 = ch1.ToString();
              aItem = str1 + str2;
              continue;
            }
            continue;
          case '\n':
          case '\r':
            continue;
          case '"':
            flag = !flag;
            continue;
          case ',':
            if (flag)
            {
              string str3 = aItem;
              ch1 = aJSON[index];
              string str4 = ch1.ToString();
              aItem = str3 + str4;
              continue;
            }
            if (aItem != "")
            {
              if (jsonNode is JSONArray)
                jsonNode.Add((JSONNode) aItem);
              else if (aKey1 != "")
                jsonNode.Add(aKey1, (JSONNode) aItem);
            }
            aKey1 = "";
            aItem = "";
            continue;
          case ':':
            if (flag)
            {
              string str5 = aItem;
              ch1 = aJSON[index];
              string str6 = ch1.ToString();
              aItem = str5 + str6;
              continue;
            }
            aKey1 = aItem;
            aItem = "";
            continue;
          case '[':
            if (flag)
            {
              string str7 = aItem;
              ch1 = aJSON[index];
              string str8 = ch1.ToString();
              aItem = str7 + str8;
              continue;
            }
            jsonNodeStack.Push((JSONNode) new JSONArray());
            if (jsonNode != (object) null)
            {
              string aKey2 = aKey1.Trim();
              if (jsonNode is JSONArray)
                jsonNode.Add(jsonNodeStack.Peek());
              else if (aKey2 != "")
                jsonNode.Add(aKey2, jsonNodeStack.Peek());
            }
            aKey1 = "";
            aItem = "";
            jsonNode = jsonNodeStack.Peek();
            continue;
          case '\\':
            ++index;
            if (flag)
            {
              char ch2 = aJSON[index];
              switch (ch2)
              {
                case 'b':
                  aItem += "\b";
                  continue;
                case 'f':
                  aItem += "\f";
                  continue;
                case 'n':
                  aItem += "\n";
                  continue;
                case 'r':
                  aItem += "\r";
                  continue;
                case 't':
                  aItem += "\t";
                  continue;
                case 'u':
                  string s = aJSON.Substring(index + 1, 4);
                  string str9 = aItem;
                  ch1 = (char) int.Parse(s, NumberStyles.AllowHexSpecifier);
                  string str10 = ch1.ToString();
                  aItem = str9 + str10;
                  index += 4;
                  continue;
                default:
                  aItem += ch2.ToString();
                  continue;
              }
            }
            else
              continue;
          case ']':
          case '}':
            if (flag)
            {
              string str11 = aItem;
              ch1 = aJSON[index];
              string str12 = ch1.ToString();
              aItem = str11 + str12;
              continue;
            }
            if (jsonNodeStack.Count == 0)
              throw new Exception("JSON Parse: Too many closing brackets");
            jsonNodeStack.Pop();
            if (aItem != "")
            {
              string aKey3 = aKey1.Trim();
              if (jsonNode is JSONArray)
                jsonNode.Add((JSONNode) aItem);
              else if (aKey3 != "")
                jsonNode.Add(aKey3, (JSONNode) aItem);
            }
            aKey1 = "";
            aItem = "";
            if (jsonNodeStack.Count > 0)
            {
              jsonNode = jsonNodeStack.Peek();
              continue;
            }
            continue;
          case '{':
            if (flag)
            {
              string str13 = aItem;
              ch1 = aJSON[index];
              string str14 = ch1.ToString();
              aItem = str13 + str14;
              continue;
            }
            jsonNodeStack.Push((JSONNode) new JSONClass());
            if (jsonNode != (object) null)
            {
              string aKey4 = aKey1.Trim();
              if (jsonNode is JSONArray)
                jsonNode.Add(jsonNodeStack.Peek());
              else if (aKey4 != "")
                jsonNode.Add(aKey4, jsonNodeStack.Peek());
            }
            aKey1 = "";
            aItem = "";
            jsonNode = jsonNodeStack.Peek();
            continue;
          default:
            string str15 = aItem;
            ch1 = aJSON[index];
            string str16 = ch1.ToString();
            aItem = str15 + str16;
            continue;
        }
      }
      if (flag)
        throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
      return jsonNode;
    }

    public virtual void Serialize(BinaryWriter aWriter)
    {
    }

    public void SaveToStream(Stream aData) => this.Serialize(new BinaryWriter(aData));

    public void SaveToCompressedStream(Stream aData)
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public void SaveToCompressedFile(string aFileName)
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public string SaveToCompressedBase64()
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public static JSONNode Deserialize(BinaryReader aReader)
    {
      JSONBinaryTag jsonBinaryTag = (JSONBinaryTag) aReader.ReadByte();
      switch (jsonBinaryTag)
      {
        case JSONBinaryTag.Array:
          int num1 = aReader.ReadInt32();
          JSONArray jsonArray = new JSONArray();
          for (int index = 0; index < num1; ++index)
            jsonArray.Add(JSONNode.Deserialize(aReader));
          return (JSONNode) jsonArray;
        case JSONBinaryTag.Class:
          int num2 = aReader.ReadInt32();
          JSONClass jsonClass = new JSONClass();
          for (int index = 0; index < num2; ++index)
          {
            string aKey = aReader.ReadString();
            JSONNode aItem = JSONNode.Deserialize(aReader);
            jsonClass.Add(aKey, aItem);
          }
          return (JSONNode) jsonClass;
        case JSONBinaryTag.Value:
          return (JSONNode) new JSONData(aReader.ReadString());
        case JSONBinaryTag.IntValue:
          return (JSONNode) new JSONData(aReader.ReadInt32());
        case JSONBinaryTag.DoubleValue:
          return (JSONNode) new JSONData(aReader.ReadDouble());
        case JSONBinaryTag.BoolValue:
          return (JSONNode) new JSONData(aReader.ReadBoolean());
        case JSONBinaryTag.FloatValue:
          return (JSONNode) new JSONData(aReader.ReadSingle());
        default:
          throw new Exception("Error deserializing JSON. Unknown tag: " + (object) jsonBinaryTag);
      }
    }

    public static JSONNode LoadFromCompressedFile(string aFileName)
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public static JSONNode LoadFromCompressedStream(Stream aData)
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public static JSONNode LoadFromCompressedBase64(string aBase64)
    {
      throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
    }

    public static JSONNode LoadFromStream(Stream aData)
    {
      using (BinaryReader aReader = new BinaryReader(aData))
        return JSONNode.Deserialize(aReader);
    }

    public static JSONNode LoadFromBase64(string aBase64)
    {
      MemoryStream aData = new MemoryStream(Convert.FromBase64String(aBase64));
      aData.Position = 0L;
      return JSONNode.LoadFromStream((Stream) aData);
    }
  }
}
