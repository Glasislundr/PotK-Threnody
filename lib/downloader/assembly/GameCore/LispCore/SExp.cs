// Decompiled with JetBrains decompiler
// Type: GameCore.LispCore.SExp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore.LispCore
{
  public static class SExp
  {
    public static Cons cons(object a, object d)
    {
      return new Cons() { car = a, cdr = d };
    }

    public static object car(object o) => o is Cons cons ? cons.car : (object) null;

    public static object cdr(object o) => o is Cons cons ? cons.cdr : (object) null;

    public static Cons lastCons(object o)
    {
      if (!(o is Cons cons))
        return (Cons) null;
      while (cons.cdr is Cons)
        cons = cons.cdr as Cons;
      return cons;
    }

    public static bool consp_(object c) => c is Cons;

    public static bool atom_(object c) => !SExp.consp_(c);

    public static object assoc(object key, object o)
    {
      for (Cons cons = o as Cons; cons != null; cons = cons.cdr as Cons)
      {
        if (SExp.equals_(SExp.car(cons.car), key))
          return cons.car;
      }
      return (object) null;
    }

    public static object mapcar(Func<object, object> func, object o)
    {
      Cons cons1 = SExp.cons((object) null, (object) null);
      Cons cons2 = cons1;
      for (Cons cons3 = o as Cons; cons3 != null; cons3 = cons3.cdr as Cons)
      {
        cons2.cdr = (object) SExp.cons(func(cons3.car), (object) null);
        cons2 = cons2.cdr as Cons;
      }
      return cons1.cdr;
    }

    public static Cons addCdr(Cons l, object o)
    {
      Cons cons = new Cons();
      cons.car = o;
      l.cdr = (object) cons;
      return cons;
    }

    public static int length(object o)
    {
      int num = 0;
      for (Cons cons = o as Cons; cons != null; cons = cons.cdr as Cons)
        ++num;
      return num;
    }

    public static object copyList(object o)
    {
      Cons cons1 = new Cons();
      Cons l = cons1;
      for (Cons cons2 = o as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
        l = SExp.addCdr(l, cons2.car);
      return cons1.cdr;
    }

    public static bool equals_(object o1, object o2)
    {
      if (o1 == o2)
        return true;
      if (o2 == null)
        return false;
      if (SExp.consp_(o1))
      {
        Cons cons1 = o2 as Cons;
        for (Cons cons2 = o1 as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
        {
          if (cons1 == null || !SExp.equals_(cons2.car, cons1.car))
            return false;
          cons1 = cons1.cdr as Cons;
        }
        return cons1 == null;
      }
      return !SExp.consp_(o2) && o1.Equals(o2);
    }

    public static Cons remove_(object o, object a)
    {
      Cons cons1 = new Cons();
      cons1.cdr = a;
      Cons cons2 = cons1;
      while (cons2.cdr != null)
      {
        if (SExp.equals_(o, SExp.car(cons2.cdr)))
          cons2.cdr = SExp.cdr(cons2.cdr);
        else
          cons2 = cons2.cdr as Cons;
      }
      return cons1.cdr as Cons;
    }

    public static object member(object item, object list)
    {
      for (Cons cons = list as Cons; cons != null; cons = cons.cdr as Cons)
      {
        if (SExp.equals_(item, cons.car))
          return (object) cons;
      }
      return (object) null;
    }

    public static object intersection(object a1, object a2)
    {
      Cons cons1 = new Cons();
      Cons l = cons1;
      Cons cons2 = SExp.copyList(a2) as Cons;
      for (Cons cons3 = a1 as Cons; cons3 != null && SExp.consp_((object) cons2); cons3 = cons3.cdr as Cons)
      {
        object car = cons3.car;
        if (SExp.member(car, (object) cons2) != null)
        {
          l = SExp.addCdr(l, car);
          cons2 = SExp.remove_(car, (object) cons2);
        }
      }
      return cons1.cdr;
    }

    public static object union(object a1, object a2)
    {
      Cons cons1 = new Cons();
      Cons l = cons1;
      for (Cons cons2 = a1 as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
      {
        object car = cons2.car;
        if (SExp.member(car, cons1.cdr) == null)
          l = SExp.addCdr(l, car);
      }
      for (Cons cons3 = a2 as Cons; cons3 != null; cons3 = cons3.cdr as Cons)
      {
        object car = cons3.car;
        if (SExp.member(car, cons1.cdr) == null)
          l = SExp.addCdr(l, car);
      }
      return cons1.cdr;
    }

    public static object setDifference(object a1, object a2)
    {
      Cons cons1 = new Cons();
      Cons l = cons1;
      for (Cons cons2 = a1 as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
      {
        object car = cons2.car;
        if (SExp.member(car, a2) == null)
          l = SExp.addCdr(l, car);
      }
      return cons1.cdr;
    }

    public static object append(Cons args)
    {
      Cons cons1 = new Cons();
      Cons l = cons1;
      for (; args != null; args = args.cdr as Cons)
      {
        for (Cons cons2 = args.car as Cons; cons2 != null; cons2 = cons2.cdr as Cons)
          l = SExp.addCdr(l, cons2.car);
      }
      return cons1.cdr;
    }

    public static List<T> toCSList<T>(object arg) where T : class
    {
      List<T> csList = new List<T>();
      for (Cons cons = arg as Cons; cons != null; cons = cons.cdr as Cons)
        csList.Add((T) cons.car);
      return csList;
    }

    public static object toLispList<T>(IEnumerable<T> args) where T : class
    {
      Cons cons1 = SExp.cons((object) null, (object) null);
      Cons cons2 = cons1;
      foreach (T obj in args)
      {
        object a = (object) obj;
        cons2.cdr = (object) SExp.cons(a, (object) null);
        cons2 = cons2.cdr as Cons;
      }
      return cons1.cdr;
    }
  }
}
