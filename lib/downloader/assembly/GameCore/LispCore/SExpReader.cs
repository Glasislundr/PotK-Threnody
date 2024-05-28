// Decompiled with JetBrains decompiler
// Type: GameCore.LispCore.SExpReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore.LispCore
{
  public class SExpReader
  {
    private object data;
    private string str;
    private SExpNumber numberDic;

    public SExpReader()
    {
    }

    public SExpReader(SExpNumber ndic) => this.numberDic = ndic;

    private Decimal? numberObject(Decimal d)
    {
      return this.numberDic != null ? this.numberDic.numberObject(d) : new Decimal?(d);
    }

    public object parse(string s, bool retList = false)
    {
      if (string.IsNullOrEmpty(s))
        return (object) null;
      try
      {
        this.str = s;
        if (retList)
        {
          Cons cons1 = SExp.cons((object) null, (object) null);
          Cons cons2 = cons1;
          int p1 = 0;
          do
          {
            cons2.cdr = (object) SExp.cons((object) null, (object) null);
            int p2 = this.readObj(p1);
            (cons2.cdr as Cons).car = this.data;
            cons2 = cons2.cdr as Cons;
            p1 = this.removeWhiteSpace(p2);
          }
          while (p1 != this.str.Length);
          return cons1.cdr;
        }
        this.readObj(0);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.str = (string) null;
      }
      return this.data;
    }

    private string substring(string s, int start, int end)
    {
      if (start >= s.Length)
        return "";
      return end >= s.Length ? s.Substring(start) : s.Substring(start, end - start);
    }

    private string readCharAt(string s, int i) => this.substring(s, i, i + 1);

    private int readObj(int p)
    {
      p = this.removeWhiteSpace(p);
      switch (this.readCharAt(this.str, p))
      {
        case "(":
          return this.readList(p + 1);
        case ")":
          throw new Exception("readObj : ')'");
        default:
          return this.readPrimitive(p);
      }
    }

    private int makeSExpString(int p)
    {
      int start = p;
      for (; p != this.str.Length; ++p)
      {
        switch (this.readCharAt(this.str, p))
        {
          case "\\":
            ++p;
            break;
          case "\"":
            goto label_5;
        }
      }
label_5:
      this.data = (object) new SExpString(this.substring(this.str, start, p));
      return p;
    }

    private int makeToken(int p)
    {
      int start = p;
      string c = this.readCharAt(this.str, p);
      if (c == "\"")
        return this.makeSExpString(p + 1) + 1;
      for (; p != this.str.Length && !this.whiteSpacep(c) && !this.punctp(c); c = this.readCharAt(this.str, p))
        ++p;
      this.data = (object) this.substring(this.str, start, p);
      return p;
    }

    private int readPrimitive(int p)
    {
      p = this.makeToken(this.removeWhiteSpace(p));
      if (this.data is SExpString)
        return p;
      object obj = this.readNumber(this.data as string);
      if (obj == null)
        return p;
      this.data = obj;
      return p;
    }

    private object readNumber(string s)
    {
      Decimal result;
      return Decimal.TryParse(s, out result) ? (object) this.numberObject(result) : (object) null;
    }

    private int readList(int p)
    {
      List<object> args = new List<object>();
      p = this.removeWhiteSpace(p);
      while (this.readCharAt(this.str, p) != ")")
      {
        p = this.readObj(p);
        args.Add(this.data);
        p = this.removeWhiteSpace(p);
        if (this.readCharAt(this.str, p) == ".")
        {
          Cons lispList = SExp.toLispList<object>((IEnumerable<object>) args) as Cons;
          p = this.readObj(p + 1);
          SExp.lastCons((object) lispList).cdr = this.data;
          this.data = (object) lispList;
          p = this.removeWhiteSpace(p);
          if (this.readCharAt(this.str, p) != ")")
            throw new Exception("readList : '.' ')'");
          return p + 1;
        }
        if (p == this.str.Length)
          throw new Exception("readList : EOF.");
      }
      this.data = args.Count != 0 ? SExp.toLispList<object>((IEnumerable<object>) args) : (object) null;
      return p + 1;
    }

    private bool numberp(string c, bool is_first)
    {
      switch (c)
      {
        case "-":
          return is_first;
        case ".":
        case "0":
        case "1":
        case "2":
        case "3":
        case "4":
        case "5":
        case "6":
        case "7":
        case "8":
        case "9":
          return true;
        default:
          return false;
      }
    }

    private bool punctp(string c) => c == "(" || c == ")";

    private bool whiteSpacep(string c)
    {
      return c == " " || c == "\b" || c == "\f" || c == "\r" || c == "\n" || c == "\t";
    }

    private int removeWhiteSpace(int p)
    {
      while (this.whiteSpacep(this.readCharAt(this.str, p)))
        ++p;
      return p;
    }
  }
}
