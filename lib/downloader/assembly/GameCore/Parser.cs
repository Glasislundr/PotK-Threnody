// Decompiled with JetBrains decompiler
// Type: GameCore.Parser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class Parser
  {
    public readonly INode Root;
    private readonly Lex<INode> lex;
    private string[] battle_var = new string[6]
    {
      "sum_own",
      "sum_enemy",
      "is_own_team_can_attacked",
      "turn",
      "sum_enemy_range_1",
      "sum_enemy_range_2"
    };

    public Parser(string text)
    {
      this.lex = new Lex<INode>(text);
      this.Root = this.exp();
      if (!this.lex.Eof())
        throw new FormatException("parse error token=" + this.lex.Token() + " " + this.lex.Show());
    }

    public string ToLisp() => this.toLisp(this.Root);

    private string toLisp(INode obj)
    {
      switch (obj)
      {
        case Node node:
          if (!double.TryParse(node.Text, out double _))
          {
            if (((IEnumerable<string>) this.battle_var).Contains<string>(node.Text))
              return "battle@" + node.Text;
            int length = node.Text.IndexOf("_");
            if (length > 0)
              return node.Text.Substring(0, length) + "@" + node.Text.Substring(length + 1);
          }
          return node.Text;
        case Tree tree:
          return string.Format("({0} {1} {2})", (object) tree.Operand, (object) this.toLisp(tree.Left), (object) this.toLisp(tree.Right));
        case If @if:
          Cond condition = @if.Condition;
          string str = condition.Operand;
          if (condition.Operand == "==")
            str = "=";
          if (condition.Operand == "!=")
            str = "not";
          return @if.Left is UseItem || @if.Left is UseSkill ? string.Format("(if ({1} {0} {2}) {3} nil)", (object) this.toLisp(condition.Left), (object) str, (object) this.toLisp(condition.Right), (object) this.toLisp(@if.Left)) : (@if.Right is UseItem || @if.Right is UseSkill ? string.Format("(if ({1} {0} {2}) nil {3})", (object) this.toLisp(condition.Left), (object) str, (object) this.toLisp(condition.Right), (object) this.toLisp(@if.Left)) : string.Format("(if ({1} {0} {2}) {3} {4})", (object) this.toLisp(condition.Left), (object) str, (object) this.toLisp(condition.Right), (object) this.toLisp(@if.Left), (object) this.toLisp(@if.Right)));
        case UseSkill useSkill:
          return string.Format("{0}", (object) useSkill.getParam((Func<string, float>) null));
        case UseItem useItem:
          return string.Format("{0}", (object) useItem.getParam((Func<string, float>) null));
        default:
          return "BUG";
      }
    }

    public string Show() => this.show(this.Root) + " <" + this.lex.Show() + ">";

    public string show(INode obj)
    {
      switch (obj)
      {
        case Node node:
          return "Node(" + node.Text + ")";
        case Tree tree:
          return string.Format("Tree({0}, {1}, {2})", (object) tree.Operand, (object) this.show(tree.Left), (object) this.show(tree.Right));
        case If @if:
          Cond condition = @if.Condition;
          return string.Format("If({0} {1} {2}, {3}, {4})", (object) this.show(condition.Left), (object) condition.Operand, (object) this.show(condition.Right), (object) this.show(@if.Left), (object) this.show(@if.Right));
        case UseSkill useSkill:
          return string.Format("UseSkill id;{0}", (object) useSkill.getParam((Func<string, float>) null));
        case UseItem useItem:
          return string.Format("UseItem id;{0}", (object) useItem.getParam((Func<string, float>) null));
        default:
          return "BUG";
      }
    }

    private INode exp()
    {
      INode val = this.term();
      return this.lex.Try("+", (Func<INode>) (() => (INode) new Tree("+", val, this.exp()))) ?? this.lex.Try("-", (Func<INode>) (() => (INode) new Tree("-", val, this.exp()))) ?? val;
    }

    private INode term()
    {
      INode val = this.factor();
      return this.lex.Try("*", (Func<INode>) (() => (INode) new Tree("*", val, this.term()))) ?? this.lex.Try("/", (Func<INode>) (() => (INode) new Tree("/", val, this.term()))) ?? this.lex.Try("^", (Func<INode>) (() => (INode) new Tree("^", val, this.term()))) ?? this.lex.Try("%", (Func<INode>) (() => (INode) new Tree("%", val, this.term()))) ?? val;
    }

    private INode factor()
    {
      return this.lex.Try("IF", ")", (Func<INode>) (() => this.cond())) ?? this.lex.Try("USESKILL", ")", (Func<INode>) (() => this.useskill())) ?? this.lex.Try("USEITEM", ")", (Func<INode>) (() => this.useitem())) ?? this.lex.Try("(", ")", (Func<INode>) (() => this.exp())) ?? (INode) new Node(this.lex.TokenAndNext());
    }

    private INode cond()
    {
      this.lex.Skip("(");
      INode node1 = this.exp();
      string operand = this.lex.TokenAndNext();
      INode node2 = this.exp();
      this.lex.Skip(",");
      INode left1 = this.exp();
      this.lex.Skip(",");
      INode right1 = this.exp();
      INode left2 = node1;
      INode right2 = node2;
      return (INode) new If(new Cond(operand, left2, right2), left1, right1);
    }

    private INode useskill()
    {
      this.lex.Skip("(");
      return (INode) new UseSkill(this.exp() as Node);
    }

    private INode useitem()
    {
      this.lex.Skip("(");
      return (INode) new UseItem(this.exp() as Node);
    }
  }
}
