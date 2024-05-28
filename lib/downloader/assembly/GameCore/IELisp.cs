// Decompiled with JetBrains decompiler
// Type: GameCore.IELisp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.LispCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace GameCore
{
  public class IELisp
  {
    protected Stack<Dictionary<string, object>> envStack;
    protected Dictionary<string, object> globalEnv;
    protected readonly Dictionary<int, Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>> specialForms;
    public SExpNumber numberDic;
    public object trueObject = new object();
    public int applyCount;
    protected Stopwatch timer;
    protected long thresholdMS;
    protected Stack<object> evalStack;
    public string error;
    public List<object> errorObjects;

    public IELisp(SExpNumber ndic)
    {
      this.envStack = new Stack<Dictionary<string, object>>();
      this.globalEnv = new Dictionary<string, object>();
      this.evalStack = new Stack<object>();
      this.envStack.Push(this.globalEnv);
      this.defineIEPrimitives();
      this.defunIEPrimitives();
      this.numberDic = ndic;
      this.specialForms = new Dictionary<int, Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>>()
      {
        {
          "quote".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_quote)
        },
        {
          "and".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_and)
        },
        {
          "or".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_or)
        },
        {
          "if".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_if)
        },
        {
          "while".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_while)
        },
        {
          "foreach".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_foreach)
        },
        {
          "progn".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_progn)
        },
        {
          "setq".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_setq)
        },
        {
          "defun".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_defun)
        },
        {
          "let".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_let)
        },
        {
          "mapcar".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_mapcar)
        },
        {
          "eval".GetHashCode(),
          new Func<object, Stack<Dictionary<string, object>>, IELisp.ReturnObject, IEnumerator>(this.sf_eval)
        }
      };
    }

    protected bool checkArgLen(string func, Cons args, int len)
    {
      if (SExp.length((object) args) >= len)
        return true;
      this.setArgumentError(func, len);
      return false;
    }

    public static Cons nthCons(int idx, Cons l)
    {
      switch (idx)
      {
        case 0:
          return l;
        case 1:
          return l.cdr as Cons;
        case 2:
          return l.cdr is Cons cdr1 ? cdr1.cdr as Cons : (Cons) null;
        case 3:
          return l.cdr is Cons cdr2 && cdr2.cdr is Cons cdr3 ? cdr3.cdr as Cons : (Cons) null;
        case 4:
          return l.cdr is Cons cdr4 && cdr4.cdr is Cons cdr5 && cdr5.cdr is Cons cdr6 ? cdr6.cdr as Cons : (Cons) null;
        default:
          return IELisp.nthCons(idx - 4, IELisp.nthCons(4, l));
      }
    }

    public object nth(int idx, Cons l)
    {
      if (l == null)
        return (object) null;
      return IELisp.nthCons(idx, l)?.car;
    }

    protected T checkType<T>(string func, object v)
    {
      if (v is T obj)
        return obj;
      this.setError(func + ":引数 (" + v + ") 型エラー (" + (object) typeof (T) + ")", (object) func, v);
      throw new Exception(this.error);
    }

    protected T checkType<T>(string func, Cons args, int idx)
    {
      if (SExp.length((object) args) <= idx)
        return (T) this.setArgumentError(func, idx);
      return this.nth(idx, args) is T obj ? obj : (T) this.setTypeError<T>(func, args, idx);
    }

    protected Cons checkTypeCons(string func, Cons args, int idx)
    {
      if (SExp.length((object) args) <= idx)
        return (Cons) this.setArgumentError(func, idx);
      object obj = this.nth(idx, args);
      switch (obj)
      {
        case null:
        case Cons _:
          return (Cons) obj;
        default:
          return (Cons) this.setTypeError<Cons>(func, args, idx);
      }
    }

    protected object setTypeError<T>(string func, Cons args, int idx)
    {
      this.setError(func + ":引数[" + (object) idx + "] 型エラー (" + (object) typeof (T) + ")", (object) func, this.nth(idx, args));
      throw new Exception(this.error);
    }

    protected object setArgumentError(string func, int idx)
    {
      this.setError(func + ":引数の数が合っていない", (object) func, (object) idx);
      throw new Exception(this.error);
    }

    public static bool symbolp_(object obj) => obj is string;

    public static bool lambdap_(object lam) => SExp.car(lam) as string == "lambda";

    public object defun(string name, Func<Cons, object> func)
    {
      this.globalEnv[name] = (object) func;
      return (object) func;
    }

    public object setq(string sym, object val, Stack<Dictionary<string, object>> es = null)
    {
      if (es != null)
      {
        foreach (Dictionary<string, object> e in es)
        {
          if (e.ContainsKey(sym))
            return e[sym] = val;
        }
      }
      return this.globalEnv[sym] = val;
    }

    private void defineIEPrimitives()
    {
      this.setq("t", this.trueObject);
      this.setq("nil", (object) null);
    }

    private void defunIEFourArithmeticOperations(string exp)
    {
      switch (exp)
      {
        case "+":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            float f = (float) this.checkType<Decimal?>(exp, args, 0).Value;
            for (Cons cdr = args.cdr as Cons; cdr != null; cdr = cdr.cdr as Cons)
            {
              float num = (float) this.checkType<Decimal?>(exp, cdr.car).Value;
              f += num;
            }
            return (object) this.numberDic.numberObject(f);
          }));
          break;
        case "-":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            float f = (float) this.checkType<Decimal?>(exp, args, 0).Value;
            for (Cons cdr = args.cdr as Cons; cdr != null; cdr = cdr.cdr as Cons)
            {
              float num = (float) this.checkType<Decimal?>(exp, cdr.car).Value;
              f -= num;
            }
            return (object) this.numberDic.numberObject(f);
          }));
          break;
        case "*":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            float f = (float) this.checkType<Decimal?>(exp, args, 0).Value;
            for (Cons cdr = args.cdr as Cons; cdr != null; cdr = cdr.cdr as Cons)
            {
              float num = (float) this.checkType<Decimal?>(exp, cdr.car).Value;
              f *= num;
            }
            return (object) this.numberDic.numberObject(f);
          }));
          break;
        case "/":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            float f = (float) this.checkType<Decimal?>(exp, args, 0).Value;
            for (Cons cdr = args.cdr as Cons; cdr != null; cdr = cdr.cdr as Cons)
            {
              float num = (float) this.checkType<Decimal?>(exp, cdr.car).Value;
              f /= num;
            }
            return (object) this.numberDic.numberObject(f);
          }));
          break;
        case "%":
          this.defun(exp, (Func<Cons, object>) (args => (object) this.numberDic.numberObject((float) this.checkType<Decimal?>(exp, args, 0).Value % (float) this.checkType<Decimal?>(exp, args, 1).Value)));
          break;
      }
    }

    private void defunIELogicalOperations(string exp)
    {
      switch (exp)
      {
        case "<":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 2))
              return (object) null;
            return (double) (float) this.checkType<Decimal?>(exp, args, 0).Value >= (double) (float) this.checkType<Decimal?>(exp, args, 1).Value ? (object) null : this.trueObject;
          }));
          break;
        case ">":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 2))
              return (object) null;
            return (double) (float) this.checkType<Decimal?>(exp, args, 0).Value <= (double) (float) this.checkType<Decimal?>(exp, args, 1).Value ? (object) null : this.trueObject;
          }));
          break;
        case "<=":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 2))
              return (object) null;
            return (double) (float) this.checkType<Decimal?>(exp, args, 0).Value > (double) (float) this.checkType<Decimal?>(exp, args, 1).Value ? (object) null : this.trueObject;
          }));
          break;
        case ">=":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 2))
              return (object) null;
            return (double) (float) this.checkType<Decimal?>(exp, args, 0).Value < (double) (float) this.checkType<Decimal?>(exp, args, 1).Value ? (object) null : this.trueObject;
          }));
          break;
        case "=":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 2))
              return (object) null;
            return !(this.checkType<Decimal?>(exp, args, 0).Value == this.checkType<Decimal?>(exp, args, 1).Value) ? (object) null : this.trueObject;
          }));
          break;
        case "not":
          this.defun(exp, (Func<Cons, object>) (args =>
          {
            if (!this.checkArgLen(exp, args, 1))
              return (object) null;
            return args.car != null ? (object) null : this.trueObject;
          }));
          break;
      }
    }

    private void defunIEPrimitives()
    {
      this.defun("car", (Func<Cons, object>) (args => this.checkArgLen("car", args, 1) ? SExp.car(args.car) : (object) null));
      this.defun("cdr", (Func<Cons, object>) (args => this.checkArgLen("cdr", args, 1) ? SExp.cdr(args.car) : (object) null));
      this.defun("cons", (Func<Cons, object>) (args => this.checkArgLen("cons", args, 2) ? (object) SExp.cons(args.car, this.nth(1, args)) : (object) null));
      this.defun("consp", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("consp", args, 1))
          return (object) null;
        object car = args.car;
        return car != null && !SExp.consp_(car) ? (object) null : this.trueObject;
      }));
      this.defun("atom", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("atom", args, 1))
          return (object) null;
        return !SExp.atom_(args.car) ? (object) null : this.trueObject;
      }));
      this.defun("eq", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("eq", args, 2))
          return (object) null;
        return !args.car.Equals(this.nth(1, args)) ? (object) null : this.trueObject;
      }));
      this.defun("list", (Func<Cons, object>) (args => (object) args));
      this.defun("length", (Func<Cons, object>) (args => this.checkArgLen("length", args, 1) ? (object) this.numberDic.numberObject(SExp.length(args.car)) : (object) null));
      this.defunIEFourArithmeticOperations("+");
      this.defunIEFourArithmeticOperations("-");
      this.defunIEFourArithmeticOperations("*");
      this.defunIEFourArithmeticOperations("/");
      this.defunIEFourArithmeticOperations("%");
      this.defunIELogicalOperations("<");
      this.defunIELogicalOperations(">");
      this.defunIELogicalOperations("=");
      this.defunIELogicalOperations("not");
      this.defunIELogicalOperations(">=");
      this.defunIELogicalOperations("<=");
      this.defun("nth", (Func<Cons, object>) (args => this.checkArgLen("nth", args, 2) ? this.nth((int) this.checkType<Decimal?>("nth", args, 0).Value, this.nth(1, args) as Cons) : (object) null));
      this.defun("tail", (Func<Cons, object>) (args => this.checkArgLen("tail", args, 1) ? SExp.car((object) SExp.lastCons(args.car)) : (object) null));
      this.defun("union", (Func<Cons, object>) (args => this.checkArgLen("union", args, 2) ? SExp.union(args.car, this.nth(1, args)) : (object) null));
      this.defun("intersection", (Func<Cons, object>) (args => this.checkArgLen("intersection", args, 2) ? SExp.intersection(args.car, this.nth(1, args)) : (object) null));
      this.defun("set-difference", (Func<Cons, object>) (args => this.checkArgLen("set-difference", args, 2) ? SExp.setDifference(args.car, this.nth(1, args)) : (object) null));
      this.defun("member", (Func<Cons, object>) (args => this.checkArgLen("member", args, 2) ? SExp.member(args.car, this.nth(1, args)) : (object) null));
      this.defun("remove", (Func<Cons, object>) (args => this.checkArgLen("remove", args, 2) ? (object) SExp.remove_(args.car, this.nth(1, args)) : (object) null));
      this.defun("append", (Func<Cons, object>) (args => SExp.append(args)));
      this.defun("assoc", (Func<Cons, object>) (args => this.checkArgLen("assoc", args, 2) ? SExp.assoc(args.car, this.nth(1, args)) : (object) null));
      this.defun("numberp", (Func<Cons, object>) (args =>
      {
        if (!this.checkArgLen("numberp", args, 1))
          return (object) null;
        return !(args.car is Decimal?) ? (object) null : this.trueObject;
      }));
      this.defun("print", (Func<Cons, object>) (args =>
      {
        SExpString sexpString1 = this.checkType<SExpString>("print", args, 0);
        string str = "";
        for (Cons cdr = args.cdr as Cons; cdr != null; cdr = cdr.cdr as Cons)
        {
          object car = cdr.car;
          if (car is SExpString)
          {
            SExpString sexpString2 = car as SExpString;
            str += sexpString2.strValue;
          }
          else
            str = car != null ? (car != this.trueObject ? str + car : str + "t") : str + "nil";
        }
        return (object) (sexpString1.strValue + str);
      }));
    }

    protected virtual void setError(string e, params object[] args)
    {
      this.error = e + "\nLast S-Expression:" + this.evalStack.Peek();
      if (this.errorObjects == null)
        this.errorObjects = new List<object>();
      foreach (object obj in args)
        this.errorObjects.Add(obj);
    }

    protected virtual object symbolValE(string sym, Stack<Dictionary<string, object>> es)
    {
      foreach (Dictionary<string, object> e in es)
      {
        if (e.ContainsKey(sym))
          return e[sym];
      }
      this.setError("未定義シンボルを参照", (object) sym);
      return (object) null;
    }

    protected object applyPrimitiveE(Func<Cons, object> func, object args) => func(args as Cons);

    private IEnumerator applyE(
      object func,
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IELisp ieLisp = this;
      if (ieLisp.timer == null)
      {
        ++ieLisp.applyCount;
        if (ieLisp.applyCount > 10 || ieLisp.error != null)
        {
          ieLisp.applyCount = 0;
          yield return (object) null;
        }
      }
      else if (ieLisp.timer.ElapsedMilliseconds > ieLisp.thresholdMS)
      {
        ieLisp.timer.Reset();
        yield return (object) null;
        ieLisp.timer.Start();
      }
      if (func is Func<Cons, object>)
      {
        try
        {
          ret.value = ieLisp.applyPrimitiveE(func as Func<Cons, object>, args);
        }
        catch (Exception ex)
        {
          if (ieLisp.error != null)
            ieLisp.setError(ex.ToString(), (object) ex);
        }
      }
      else
      {
        IEnumerator ee = ieLisp.applyLambdaE(func, args, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
    }

    private IEnumerator evalArgsE(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      Cons r = SExp.cons((object) null, (object) null);
      Cons c = r;
      Cons al;
      for (al = args as Cons; al != null; al = al.cdr as Cons)
      {
        IEnumerator ee = this.evalE(al.car, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        c.cdr = (object) SExp.cons(ret.value, (object) null);
        c = c.cdr as Cons;
      }
      al = (Cons) null;
      ret.value = r.cdr;
    }

    private IEnumerator sf_quote(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      ret.value = SExp.car(args);
      yield break;
    }

    private IEnumerator sf_and(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      Cons l;
      for (l = args as Cons; l != null; l = l.cdr as Cons)
      {
        IEnumerator ee = this.evalE(l.car, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        if (ret.value == null)
          yield break;
      }
      l = (Cons) null;
    }

    private IEnumerator sf_or(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      Cons l;
      for (l = args as Cons; l != null; l = l.cdr as Cons)
      {
        IEnumerator ee = this.evalE(l.car, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        if (ret.value != null)
          yield break;
      }
      l = (Cons) null;
    }

    private IEnumerator sf_if(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IEnumerator ee = this.evalE(SExp.car(args), es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
      if (ret.value != null)
      {
        ee = this.evalE(this.nth(1, args as Cons), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
      else
      {
        ee = this.evalBodyE((object) IELisp.nthCons(2, args as Cons), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
    }

    private IEnumerator sf_while(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      ret.value = (object) null;
      while (true)
      {
        IEnumerator ee = this.evalE(SExp.car(args), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        if (ret.value != null)
        {
          ee = this.evalBodyE(SExp.cdr(args), es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
        }
        else
          break;
      }
    }

    private IEnumerator sf_foreach(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IELisp ieLisp = this;
      if (!(SExp.car(args) is string x))
      {
        ieLisp.setError("foreach", args);
      }
      else
      {
        Dictionary<string, object> e = new Dictionary<string, object>();
        es.Push(e);
        e[x] = (object) null;
        IEnumerator ee = ieLisp.evalE(ieLisp.nth(1, args as Cons), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        Cons l = ret.value as Cons;
        ret.value = (object) null;
        for (; l != null; l = l.cdr as Cons)
        {
          e[x] = l.car;
          ee = ieLisp.evalBodyE((object) IELisp.nthCons(2, args as Cons), es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
        }
        es.Pop();
      }
    }

    private IEnumerator sf_progn(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IEnumerator ee = this.evalBodyE(args, es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
    }

    private IEnumerator sf_setq(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IELisp ieLisp = this;
      if (!(SExp.car(args) is string s))
      {
        ieLisp.setError("setq", args);
      }
      else
      {
        IEnumerator ee = ieLisp.evalE(SExp.car(SExp.cdr(args)), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        ret.value = ieLisp.setq(s, ret.value, es);
      }
    }

    private IEnumerator sf_defun(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      ret.value = this.setq(SExp.car(args) as string, (object) SExp.cons((object) "lambda", (object) SExp.cons(SExp.car(SExp.cdr(args)), SExp.cdr(SExp.cdr(args)))));
      yield break;
    }

    private IEnumerator sf_let(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      Dictionary<string, object> e = new Dictionary<string, object>();
      Cons l;
      IEnumerator ee;
      for (l = SExp.car(args) as Cons; l != null; l = l.cdr as Cons)
      {
        object a = l.car;
        if (a is Cons)
        {
          ee = this.evalE(SExp.car(SExp.cdr(a)), es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
          e[SExp.car(a) as string] = ret.value;
        }
        else
          e[a as string] = (object) null;
        a = (object) null;
      }
      l = (Cons) null;
      es.Push(e);
      ee = this.evalBodyE(SExp.cdr(args), es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
      es.Pop();
    }

    private IEnumerator sf_mapcar(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      Cons mapret = new Cons();
      Cons w = mapret;
      object f = SExp.car(args);
      IEnumerator ee = this.evalE(SExp.car(SExp.cdr(args)), es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
      Cons l;
      for (l = ret.value as Cons; l != null; l = l.cdr as Cons)
      {
        ee = this.applyE(f, (object) l, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        w = SExp.addCdr(w, ret.value);
      }
      l = (Cons) null;
      ret.value = mapret.cdr;
    }

    private IEnumerator sf_eval(
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IEnumerator ee = this.evalE(SExp.car(args), es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
      ee = this.evalE(ret.value, es, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
    }

    private IEnumerator evalCallE(
      object func,
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      IELisp ieLisp = this;
      IEnumerator ee;
      if (func is string str)
      {
        int hashCode = str.GetHashCode();
        if (ieLisp.specialForms.ContainsKey(hashCode))
        {
          ee = ieLisp.specialForms[hashCode](args, es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
        }
        else
        {
          ee = ieLisp.evalE(func, es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
          object fe = ret.value;
          ee = ieLisp.evalArgsE(args, es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
          object args1 = ret.value;
          ee = ieLisp.applyE(fe, args1, es, ret);
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
          fe = (object) null;
        }
      }
      else if (SExp.car(func) as string == "lambda")
      {
        ee = ieLisp.evalArgsE(args, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        ee = ieLisp.applyLambdaE(func, ret.value, es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
      else
      {
        ieLisp.setError(func.ToString(), args);
        ret.value = (object) null;
      }
    }

    private IEnumerator applyLambdaE(
      object lam,
      object args,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      if (SExp.car(lam) as string != "lambda")
      {
        ret.value = (object) null;
      }
      else
      {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        Cons cons1 = args as Cons;
        for (Cons cons2 = SExp.car(SExp.cdr(lam)) as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
        {
          dictionary[cons2.car as string] = cons1?.car;
          cons1 = cons1.cdr as Cons;
        }
        es.Push(dictionary);
        IEnumerator ee = this.evalBodyE(SExp.cdr(SExp.cdr(lam)), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
        es.Pop();
      }
    }

    private IEnumerator evalE(
      object sexp,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      this.evalStack.Push(sexp);
      if (sexp is string)
        ret.value = this.symbolValE(sexp as string, es);
      else if (SExp.atom_(sexp))
        ret.value = sexp;
      else if (SExp.car(sexp) as string == "lambda")
      {
        ret.value = sexp;
      }
      else
      {
        IEnumerator ee = this.evalCallE(SExp.car(sexp), SExp.cdr(sexp), es, ret);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
      this.evalStack.Pop();
    }

    private IEnumerator evalBodyE(
      object sexp,
      Stack<Dictionary<string, object>> es,
      IELisp.ReturnObject ret)
    {
      ret.value = (object) null;
      Cons l;
      for (l = sexp as Cons; l != null; l = l.cdr as Cons)
      {
        IEnumerator ee = this.evalE(SExp.car((object) l), es, ret);
        while (ee.MoveNext())
        {
          yield return ee.Current;
          if (this.timer != null && this.timer.ElapsedMilliseconds > this.thresholdMS)
          {
            this.timer.Reset();
            yield return (object) null;
            this.timer.Start();
          }
        }
        ee = (IEnumerator) null;
      }
      l = (Cons) null;
    }

    public IEnumerator topLevelCallE(object func, object args, IELisp.ReturnObject ret)
    {
      this.evalStack.Clear();
      if (this.timer != null)
        this.timer.Start();
      IEnumerator ee = this.evalCallE(func, args, this.envStack, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
    }

    public IEnumerator evalTopLevelE(object sexp, IELisp.ReturnObject ret)
    {
      this.evalStack.Clear();
      if (this.timer != null)
        this.timer.Start();
      IEnumerator ee = this.evalE(sexp, this.envStack, ret);
      while (ee.MoveNext())
        yield return ee.Current;
      ee = (IEnumerator) null;
    }

    public class ReturnObject
    {
      public object value;
    }
  }
}
