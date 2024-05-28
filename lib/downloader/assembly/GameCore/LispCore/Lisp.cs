// Decompiled with JetBrains decompiler
// Type: GameCore.LispCore.Lisp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore.LispCore
{
  public class Lisp
  {
    protected Dictionary<string, object> env;

    public Lisp()
    {
      this.env = new Dictionary<string, object>();
      this.definePrimitives();
      this.defunPrimitives();
    }

    public Lisp(Dictionary<string, object> e)
    {
      this.env = e;
      this.definePrimitives();
      this.defunPrimitives();
    }

    public object defun(string name, Func<List<object>, object> func, Dictionary<string, object> e = null)
    {
      if (e == null)
        this.env[name] = (object) func;
      else
        e[name] = (object) func;
      return (object) func;
    }

    private void defunFourArithmeticOperations(string exp)
    {
      switch (exp)
      {
        case "+":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable1 = args[0] as Decimal?;
            Decimal? nullable2 = args[1] as Decimal?;
            if (!nullable1.HasValue || !nullable2.HasValue)
              return (object) null;
            Decimal? nullable3 = nullable1;
            Decimal? nullable4 = nullable2;
            return nullable3.HasValue & nullable4.HasValue ? (object) new Decimal?(nullable3.GetValueOrDefault() + nullable4.GetValueOrDefault()) : (object) new Decimal?();
          }));
          break;
        case "-":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable5 = args[0] as Decimal?;
            Decimal? nullable6 = args[1] as Decimal?;
            if (!nullable5.HasValue || !nullable6.HasValue)
              return (object) null;
            Decimal? nullable7 = nullable5;
            Decimal? nullable8 = nullable6;
            return nullable7.HasValue & nullable8.HasValue ? (object) new Decimal?(nullable7.GetValueOrDefault() - nullable8.GetValueOrDefault()) : (object) new Decimal?();
          }));
          break;
        case "*":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable9 = args[0] as Decimal?;
            Decimal? nullable10 = args[1] as Decimal?;
            if (!nullable9.HasValue || !nullable10.HasValue)
              return (object) null;
            Decimal? nullable11 = nullable9;
            Decimal? nullable12 = nullable10;
            return nullable11.HasValue & nullable12.HasValue ? (object) new Decimal?(nullable11.GetValueOrDefault() * nullable12.GetValueOrDefault()) : (object) new Decimal?();
          }));
          break;
        case "/":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable13 = args[0] as Decimal?;
            Decimal? nullable14 = args[1] as Decimal?;
            if (!nullable13.HasValue || !nullable14.HasValue)
              return (object) null;
            Decimal? nullable15 = nullable13;
            Decimal? nullable16 = nullable14;
            return nullable15.HasValue & nullable16.HasValue ? (object) new Decimal?(nullable15.GetValueOrDefault() / nullable16.GetValueOrDefault()) : (object) new Decimal?();
          }));
          break;
        case "%":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable17 = args[0] as Decimal?;
            Decimal? nullable18 = args[1] as Decimal?;
            if (!nullable17.HasValue || !nullable18.HasValue)
              return (object) null;
            Decimal? nullable19 = nullable17;
            Decimal? nullable20 = nullable18;
            return nullable19.HasValue & nullable20.HasValue ? (object) new Decimal?(nullable19.GetValueOrDefault() % nullable20.GetValueOrDefault()) : (object) new Decimal?();
          }));
          break;
      }
    }

    private void defunLogicalOperations(string exp)
    {
      switch (exp)
      {
        case "and":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            bool? nullable1 = args[0] as bool?;
            bool? nullable2 = args[1] as bool?;
            return nullable1.HasValue && nullable2.HasValue ? (!nullable1.Value ? (object) false : (nullable2.Value ? (object) true : (object) false)) : (object) false;
          }));
          break;
        case "or":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            bool? nullable3 = args[0] as bool?;
            bool? nullable4 = args[1] as bool?;
            return nullable3.HasValue && nullable4.HasValue ? (nullable3.Value ? (object) true : (nullable4.Value ? (object) true : (object) false)) : (object) false;
          }));
          break;
        case "<":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable5 = args[0] as Decimal?;
            Decimal? nullable6 = args[1] as Decimal?;
            if (!nullable5.HasValue || !nullable6.HasValue)
              return (object) false;
            Decimal? nullable7 = nullable5;
            Decimal? nullable8 = nullable6;
            return (object) (nullable7.GetValueOrDefault() < nullable8.GetValueOrDefault() & nullable7.HasValue & nullable8.HasValue);
          }));
          break;
        case ">":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable9 = args[0] as Decimal?;
            Decimal? nullable10 = args[1] as Decimal?;
            if (!nullable9.HasValue || !nullable10.HasValue)
              return (object) false;
            Decimal? nullable11 = nullable9;
            Decimal? nullable12 = nullable10;
            return (object) (nullable11.GetValueOrDefault() > nullable12.GetValueOrDefault() & nullable11.HasValue & nullable12.HasValue);
          }));
          break;
        case "=":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            Decimal? nullable13 = args[0] as Decimal?;
            Decimal? nullable14 = args[1] as Decimal?;
            if (!nullable13.HasValue || !nullable14.HasValue)
              return (object) false;
            Decimal? nullable15 = nullable13;
            Decimal? nullable16 = nullable14;
            return (object) (nullable15.GetValueOrDefault() == nullable16.GetValueOrDefault() & nullable15.HasValue == nullable16.HasValue);
          }));
          break;
        case "not":
          this.defun(exp, (Func<List<object>, object>) (args =>
          {
            bool? nullable17 = args[0] as bool?;
            if (!nullable17.HasValue)
              return (object) false;
            bool? nullable18 = nullable17;
            return nullable18.HasValue ? (object) new bool?(!nullable18.GetValueOrDefault()) : (object) new bool?();
          }));
          break;
      }
    }

    private void definePrimitives()
    {
      this.setq("true", (object) true);
      this.setq("false", (object) false);
    }

    private void defunPrimitives()
    {
      this.defun("car", (Func<List<object>, object>) (args => SExp.car(args[0])));
      this.defun("cdr", (Func<List<object>, object>) (args => SExp.cdr(args[0])));
      this.defun("cons", (Func<List<object>, object>) (args => (object) SExp.cons(args[0], args[1])));
      this.defun("atom", (Func<List<object>, object>) (args => (object) SExp.atom_(args[0])));
      this.defun("eq", (Func<List<object>, object>) (args => (object) args[0].Equals(args[1])));
      this.defun("list", (Func<List<object>, object>) (args => SExp.toLispList<object>((IEnumerable<object>) args)));
      this.defunFourArithmeticOperations("+");
      this.defunFourArithmeticOperations("-");
      this.defunFourArithmeticOperations("*");
      this.defunFourArithmeticOperations("/");
      this.defunFourArithmeticOperations("%");
      this.defunLogicalOperations("and");
      this.defunLogicalOperations("or");
      this.defunLogicalOperations("<");
      this.defunLogicalOperations(">");
      this.defunLogicalOperations("=");
      this.defunLogicalOperations("not");
    }

    public static bool symbolp_(object obj) => obj is string;

    public static bool? symbolp(object obj) => new bool?(obj is string);

    public static bool? functionp(object o)
    {
      return new bool?(o is Func<List<object>, object> || Lisp.lambdap_(o));
    }

    public static bool lambdap_(object lam) => SExp.car(lam) as string == "lambda";

    public static bool? lambdap(object lam) => Lisp.symbolEqual(SExp.car(lam), "lambda");

    private static bool? symbolEqual(object obj, string v) => new bool?(obj as string == v);

    public object symbolVal(string sym, Dictionary<string, object> e = null)
    {
      if (e != null && e.ContainsKey(sym))
        return e[sym];
      return this.env.ContainsKey(sym) ? this.env[sym] : (object) sym;
    }

    public object setq(string sym, object val, Dictionary<string, object> e = null)
    {
      if (e == null)
        this.env[sym] = val;
      else
        e[sym] = val;
      return val;
    }

    protected object applyPrimitive(Func<List<object>, object> func, object args)
    {
      return func(SExp.toCSList<object>(args));
    }

    protected object applyLambda(object lam, object args)
    {
      if (!Lisp.lambdap_(lam))
        return (object) null;
      Dictionary<string, object> e = new Dictionary<string, object>();
      for (object obj = SExp.car(SExp.cdr(lam)); SExp.consp_(obj); obj = SExp.cdr(obj))
      {
        e[SExp.car(obj) as string] = SExp.car(args);
        args = SExp.cdr(args);
      }
      return this.evalBody(SExp.cdr(SExp.cdr(lam)), e);
    }

    protected virtual object apply(object func, object args)
    {
      return func is Func<List<object>, object> ? this.applyPrimitive(func as Func<List<object>, object>, args) : this.applyLambda(func, args);
    }

    protected object evalCall(object func, object args, Dictionary<string, object> e)
    {
      if (func is string str)
      {
        switch (str)
        {
          case "quote":
            return SExp.car(args);
          case "if":
            bool? nullable1 = this.eval(SExp.car(args), e) as bool?;
            return (!nullable1.HasValue ? 0 : (nullable1.Value ? 1 : 0)) != 0 ? this.eval(SExp.car(SExp.cdr(args)), e) : this.evalBody(SExp.cdr(SExp.cdr(args)), e);
          case "while":
            object obj = (object) null;
            while (true)
            {
              bool? nullable2 = this.eval(SExp.car(args), e) as bool?;
              if ((!nullable2.HasValue ? 0 : (nullable2.Value ? 1 : 0)) != 0)
                obj = this.evalBody(SExp.cdr(args), e);
              else
                break;
            }
            return obj;
          case "setq":
            return this.setq(SExp.car(args) as string, this.eval(SExp.car(SExp.cdr(args)), e));
          case "defun":
            return this.setq(SExp.car(args) as string, (object) SExp.cons((object) "lambda", (object) SExp.cons(SExp.car(SExp.cdr(args)), SExp.cdr(SExp.cdr(args)))));
          default:
            return this.apply(this.eval(func, e), SExp.mapcar((Func<object, object>) (x => this.eval(x, e)), args));
        }
      }
      else
        return Lisp.lambdap_(func) ? this.applyLambda(func, SExp.mapcar((Func<object, object>) (x => this.eval(x, e)), args)) : (object) null;
    }

    public object eval(object sexp, Dictionary<string, object> e)
    {
      if (Lisp.symbolp_(sexp))
        return this.symbolVal(sexp as string, e);
      return SExp.atom_(sexp) ? sexp : this.evalCall(SExp.car(sexp), SExp.cdr(sexp), e);
    }

    public object evalBody(object sexp, Dictionary<string, object> e)
    {
      object obj1 = sexp;
      object obj2 = (object) null;
      for (; SExp.consp_(obj1); obj1 = SExp.cdr(obj1))
        obj2 = this.eval(SExp.car(obj1), e);
      return obj2;
    }

    public object evalTopLevel(object sexp) => this.eval(sexp, (Dictionary<string, object>) null);
  }
}
