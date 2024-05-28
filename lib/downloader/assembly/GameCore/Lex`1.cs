// Decompiled with JetBrains decompiler
// Type: GameCore.Lex`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class Lex<T> where T : class
  {
    private static readonly Regex tokenPattern = new Regex("\\d+(\\.\\d+)?|[+-/*^%\\(\\)]|[a-zA-Z][a-zA-Z_.0-9]+|IF|,|(==|!=|<=|>=|<>|=|<|>)|USESKILL|USEITEM");
    public readonly string Text;
    private readonly string[] tokens;
    private int pos;

    public Lex(string text)
    {
      this.Text = text.Replace(" ", "");
      this.tokens = Lex<T>.tokenPattern.Matches(this.Text).Cast<Match>().Select<Match, string>((Func<Match, string>) (x => x.Value)).ToArray<string>();
      this.pos = 0;
    }

    public void Next() => ++this.pos;

    public void Skip(string expect)
    {
      if (expect != this.Token())
        throw new FormatException("expect=" + expect + " but token=" + this.Token() + " : " + this.Show());
      this.Next();
    }

    public string Token()
    {
      if (this.Eof())
        throw new IndexOutOfRangeException("EOF " + this.Show());
      return this.tokens[this.pos];
    }

    public string TokenAndNext()
    {
      string str = this.Token();
      this.Next();
      return str;
    }

    public T Try(string expect, Func<T> callback)
    {
      if (this.Eof() || !(this.Token() == expect))
        return default (T);
      this.Next();
      return callback();
    }

    public T Try(string expect1, string expect2, Func<T> callback)
    {
      return this.Try(expect1, (Func<T>) (() =>
      {
        T obj = callback();
        this.Skip(expect2);
        return obj;
      }));
    }

    public bool Eof() => this.pos >= this.tokens.Length;

    public string Show()
    {
      return string.Format("tokens='{0}' pos={1} text={2}", (object) string.Join(" ", this.tokens), (object) this.pos, (object) this.Text);
    }
  }
}
