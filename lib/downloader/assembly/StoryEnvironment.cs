// Decompiled with JetBrains decompiler
// Type: StoryEnvironment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.LispCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniLinq;
using UnityEngine;

#nullable disable
public class StoryEnvironment
{
  private float r;
  private float g;
  private float b;
  private float fromAlpha;
  private float toAlpha;
  private float standPos;
  private GameObject obj;
  public static bool IsNoWaitOnChoise;
  private Dictionary<string, object> scriptEnv = new Dictionary<string, object>();
  private Lisp engine;
  private List<StoryBlock> storyBlocks;
  private StoryExecuter executer;
  private int currentIdx;
  private string nextLabel;

  private int? lispNumberToInt(object a)
  {
    Decimal? nullable = a as Decimal?;
    return !nullable.HasValue ? new int?() : new int?((int) nullable.Value);
  }

  private float? lispNumberToFloat(object a)
  {
    Decimal? nullable = a as Decimal?;
    return !nullable.HasValue ? new float?() : new float?((float) nullable.Value);
  }

  private string lispStringToString(object a)
  {
    return !(a is SExpString sexpString) ? (string) null : sexpString.strValue;
  }

  private void defineVariables()
  {
    this.engine.setq("on", (object) true);
    this.engine.setq("off", (object) false);
  }

  private static void colorFromName(string name, out float r, out float g, out float b)
  {
    switch (name)
    {
      case "black":
        r = 0.0f;
        g = 0.0f;
        b = 0.0f;
        break;
      case "white":
        r = 1f;
        g = 1f;
        b = 1f;
        break;
      case "red":
        r = 1f;
        g = 0.0f;
        b = 0.0f;
        break;
      case "green":
        r = 0.0f;
        g = 1f;
        b = 0.0f;
        break;
      case "blue":
        r = 0.0f;
        g = 0.0f;
        b = 1f;
        break;
      case "pink":
        r = 1f;
        g = 0.75f;
        b = 0.8f;
        break;
      default:
        r = 0.2f;
        g = 0.0f;
        b = 0.0f;
        break;
    }
  }

  private void defunCommands()
  {
    this.engine.defun("serif", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      if (nullable.Value == 0)
      {
        this.current.text.pos = TextBlock.Position.BOTTOM;
      }
      else
      {
        this.current.text.pos = TextBlock.Position.TOP;
        this.executer.openTopLabelObject();
      }
      return args[0];
    }));
    this.engine.defun("setname", (Func<List<object>, object>) (args =>
    {
      this.engine.setq("name", args[0]);
      string text = this.lispStringToString(args[0]);
      if (text == null)
        return (object) null;
      int? nullable = new int?();
      if (args.Count >= 2)
        nullable = this.lispNumberToInt(args[1]);
      string s = TextBlock.decorateText(text);
      if (this.current.text.pos == TextBlock.Position.BOTTOM)
        this.executer.setBottomName(s, nullable.HasValue ? nullable.Value : -1);
      else
        this.executer.setTopName(s, nullable.HasValue ? nullable.Value : -1);
      return args[0];
    }));
    this.engine.defun("gotolabel", (Func<List<object>, object>) (args =>
    {
      this.setNextLabel(this.lispStringToString(args[0]));
      return args[0];
    }));
    this.engine.defun("debug-addtext", (Func<List<object>, object>) (args =>
    {
      this.current.addText(this.lispStringToString(args[0]) ?? args[0].ToString());
      return args[0];
    }));
    this.engine.defun("unity-debug-log", (Func<List<object>, object>) (args =>
    {
      Debug.Log((object) (this.lispStringToString(args[0]) ?? args[0].ToString()));
      return args[0];
    }));
    this.engine.defun("body", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      int? job_id = args.Count > 1 ? this.lispNumberToInt(args[1]) : new int?();
      this.executer.setPerson(nullable.Value, nullable.Value, job_id);
      return args[0];
    }));
    this.engine.defun("entry", (Func<List<object>, object>) (args =>
    {
      int? nullable1 = this.lispNumberToInt(args[0]);
      if (!nullable1.HasValue)
        return (object) null;
      int? nullable2 = this.lispNumberToInt(args[1]);
      if (!nullable2.HasValue)
        return (object) null;
      int? job_id = args.Count > 2 ? this.lispNumberToInt(args[2]) : new int?();
      this.executer.setPerson(nullable1.Value, nullable2.Value, job_id);
      return args[0];
    }));
    this.engine.defun("pos", (Func<List<object>, object>) (args =>
    {
      int? nullable3 = this.lispNumberToInt(args[0]);
      if (!nullable3.HasValue)
        return (object) null;
      int? nullable4 = this.lispNumberToInt(args[1]);
      if (!nullable4.HasValue)
        return (object) null;
      this.executer.setCharaPosition(nullable3.Value, nullable4.Value);
      return args[0];
    }));
    this.engine.defun("chara", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      int? jobid = args.Count > 1 ? this.lispNumberToInt(args[1]) : new int?();
      this.executer.getCharaPosition(nullable.Value, jobid);
      return args[0];
    }));
    this.engine.defun("face", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      string s = this.lispStringToString(args[1]);
      if (s == null)
        return (object) null;
      this.executer.setFace(nullable.Value, s);
      return args[0];
    }));
    this.engine.defun("eye", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      string s = this.lispStringToString(args[1]);
      if (s == null)
        return (object) null;
      this.executer.setEye(nullable.Value, s);
      return args[0];
    }));
    this.engine.defun("leftin", (Func<List<object>, object>) (args =>
    {
      int? nullable5 = this.lispNumberToInt(args[0]);
      if (!nullable5.HasValue)
        return (object) null;
      float? nullable6 = this.lispNumberToFloat(args[1]);
      if (!nullable6.HasValue)
        return (object) null;
      this.executer.setCharaMoveIn(nullable5.Value, nullable6.Value, -1000f);
      return args[0];
    }));
    this.engine.defun("rightin", (Func<List<object>, object>) (args =>
    {
      int? nullable7 = this.lispNumberToInt(args[0]);
      if (!nullable7.HasValue)
        return (object) null;
      float? nullable8 = this.lispNumberToFloat(args[1]);
      if (!nullable8.HasValue)
        return (object) null;
      this.executer.setCharaMoveIn(nullable7.Value, nullable8.Value, 1000f);
      return args[0];
    }));
    this.engine.defun("leftout", (Func<List<object>, object>) (args =>
    {
      int? nullable9 = this.lispNumberToInt(args[0]);
      if (!nullable9.HasValue)
        return (object) null;
      float? nullable10 = this.lispNumberToFloat(args[1]);
      if (!nullable10.HasValue)
        return (object) null;
      this.executer.setCharaMoveOut(nullable9.Value, nullable10.Value, -1000f);
      return args[0];
    }));
    this.engine.defun("rightout", (Func<List<object>, object>) (args =>
    {
      int? nullable11 = this.lispNumberToInt(args[0]);
      if (!nullable11.HasValue)
        return (object) null;
      float? nullable12 = this.lispNumberToFloat(args[1]);
      if (!nullable12.HasValue)
        return (object) null;
      this.executer.setCharaMoveOut(nullable11.Value, nullable12.Value, 1000f);
      return args[0];
    }));
    this.engine.defun("scale", (Func<List<object>, object>) (args =>
    {
      int? nullable13 = this.lispNumberToInt(args[0]);
      if (!nullable13.HasValue)
        return (object) null;
      float? nullable14 = this.lispNumberToFloat(args[1]);
      if (!nullable14.HasValue)
        return (object) null;
      float? nullable15 = this.lispNumberToFloat(args[2]);
      if (!nullable15.HasValue)
        return (object) null;
      this.executer.setCharaScale(nullable13.Value, nullable14.Value, nullable15.Value);
      return args[0];
    }));
    this.engine.defun("henshinbody", (Func<List<object>, object>) (args =>
    {
      int count = args.Count;
      switch (count)
      {
        case 3:
        case 4:
          int? nullable16 = this.lispNumberToInt(args[0]);
          if (!nullable16.HasValue)
            return (object) null;
          int num1 = count == 3 ? 0 : 1;
          List<object> objectList1 = args;
          int index1 = num1;
          int num2 = index1 + 1;
          int? nullable17 = this.lispNumberToInt(objectList1[index1]);
          if (!nullable17.HasValue)
            return (object) null;
          List<object> objectList2 = args;
          int index2 = num2;
          int index3 = index2 + 1;
          int? nullable18 = this.lispNumberToInt(objectList2[index2]);
          if (!nullable18.HasValue)
            return (object) null;
          int? nullable19 = this.lispNumberToInt(args[index3]);
          if (!nullable19.HasValue)
            return (object) null;
          this.executer.setHenshin(nullable16.Value, nullable17.Value, nullable18.Value, nullable19.Value);
          return args[0];
        default:
          return (object) null;
      }
    }));
    this.engine.defun("henshin", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.startHenshin(nullable.Value);
      return args[0];
    }));
    this.engine.defun("henshinskip", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.skipHenshin(nullable.Value);
      return args[0];
    }));
    this.engine.defun("emotionbody", (Func<List<object>, object>) (args =>
    {
      int? nullable20 = this.lispNumberToInt(args[0]);
      if (!nullable20.HasValue)
        return (object) null;
      int? nullable21 = this.lispNumberToInt(args[1]);
      if (!nullable21.HasValue)
        return (object) null;
      int? nullable22 = this.lispNumberToInt(args[2]);
      if (!nullable22.HasValue)
        return (object) null;
      int? nullable23 = this.lispNumberToInt(args[3]);
      if (!nullable23.HasValue)
        return (object) null;
      int? nullable24 = this.lispNumberToInt(args[4]);
      if (!nullable24.HasValue)
        return (object) null;
      int? nullable25 = args.Count == 6 ? this.lispNumberToInt(args[5]) : new int?(0);
      if (!nullable25.HasValue)
        return (object) null;
      this.executer.setEmotion(nullable20.Value, nullable21.Value, nullable25.Value, nullable22.Value, nullable23.Value, nullable24.Value);
      return args[0];
    }));
    this.engine.defun("envbody", (Func<List<object>, object>) (args =>
    {
      int? nullable26 = this.lispNumberToInt(args[0]);
      if (!nullable26.HasValue)
        return (object) null;
      int? nullable27 = this.lispNumberToInt(args[1]);
      if (!nullable27.HasValue)
        return (object) null;
      int? nullable28 = args.Count == 3 ? this.lispNumberToInt(args[2]) : new int?(0);
      if (!nullable28.HasValue)
        return (object) null;
      this.executer.setEnvEffect(nullable26.Value, nullable27.Value, nullable28.Value);
      return args[0];
    }));
    this.engine.defun("effectbody", (Func<List<object>, object>) (args =>
    {
      int? nullable29 = this.lispNumberToInt(args[0]);
      if (!nullable29.HasValue)
        return (object) null;
      int? nullable30 = this.lispNumberToInt(args[1]);
      if (!nullable30.HasValue)
        return (object) null;
      int? nullable31 = this.lispNumberToInt(args[2]);
      if (!nullable31.HasValue)
        return (object) null;
      int? nullable32 = this.lispNumberToInt(args[3]);
      if (!nullable32.HasValue)
        return (object) null;
      int? nullable33 = args.Count == 5 ? this.lispNumberToInt(args[4]) : new int?(0);
      if (!nullable33.HasValue)
        return (object) null;
      this.executer.setEffect(nullable29.Value, nullable30.Value, nullable33.Value, nullable31.Value, nullable32.Value);
      return args[0];
    }));
    this.engine.defun("effectpattern", (Func<List<object>, object>) (args =>
    {
      int? nullable34 = this.lispNumberToInt(args[0]);
      if (!nullable34.HasValue)
        return (object) null;
      int? nullable35 = this.lispNumberToInt(args[1]);
      if (!nullable35.HasValue)
        return (object) null;
      int? nullable36 = args.Count == 3 ? this.lispNumberToInt(args[2]) : new int?(0);
      if (!nullable36.HasValue)
        return (object) null;
      this.executer.changeEffect(nullable34.Value, nullable35.Value, nullable36.Value);
      return args[0];
    }));
    this.engine.defun("effectstart", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.startEffect(nullable.Value);
      return args[0];
    }));
    this.engine.defun("effectskip", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.skipEffect(nullable.Value);
      return args[0];
    }));
    this.engine.defun("jump", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.setJump(nullable.Value);
      return args[0];
    }));
    this.engine.defun("clash", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.setClash(nullable.Value);
      return args[0];
    }));
    this.engine.defun("move", (Func<List<object>, object>) (args =>
    {
      int? nullable37 = this.lispNumberToInt(args[0]);
      if (!nullable37.HasValue)
        return (object) null;
      int? nullable38 = this.lispNumberToInt(args[1]);
      if (!nullable38.HasValue)
        return (object) null;
      float? nullable39 = this.lispNumberToFloat(args[2]);
      if (!nullable39.HasValue)
        return (object) null;
      this.executer.setMoveChara(nullable37.Value, nullable38.Value, nullable39.Value);
      return args[0];
    }));
    this.engine.defun("brightness", (Func<List<object>, object>) (args =>
    {
      int? nullable40 = this.lispNumberToInt(args[0]);
      if (!nullable40.HasValue)
        return (object) null;
      float? nullable41 = this.lispNumberToFloat(args[1]);
      if (!nullable41.HasValue)
        return (object) null;
      float? nullable42 = this.lispNumberToFloat(args[2]);
      if (!nullable42.HasValue)
        return (object) null;
      this.executer.setCharaBrightness(nullable40.Value, nullable41.Value, nullable42.Value);
      return args[0];
    }));
    this.engine.defun("alpha", (Func<List<object>, object>) (args =>
    {
      int? nullable43 = this.lispNumberToInt(args[0]);
      if (!nullable43.HasValue)
        return (object) null;
      float? nullable44 = this.lispNumberToFloat(args[1]);
      if (!nullable44.HasValue)
        return (object) null;
      float? nullable45 = this.lispNumberToFloat(args[2]);
      if (!nullable45.HasValue)
        return (object) null;
      this.executer.setCharaAlpha(nullable43.Value, nullable44.Value, nullable45.Value);
      return args[0];
    }));
    this.engine.defun("reversal", (Func<List<object>, object>) (args =>
    {
      int? nullable46 = this.lispNumberToInt(args[0]);
      if (!nullable46.HasValue)
        return (object) null;
      int? nullable47 = this.lispNumberToInt(args[1]);
      if (!nullable47.HasValue)
        return (object) null;
      StoryExecuter executer = this.executer;
      int id = nullable46.Value;
      int? nullable48 = nullable47;
      int num3 = 0;
      int num4 = nullable48.GetValueOrDefault() == num3 & nullable48.HasValue ? 1 : 0;
      executer.setCharaReversal(id, num4 != 0);
      return args[0];
    }));
    this.engine.defun("distinction", (Func<List<object>, object>) (args =>
    {
      int? nullable49 = this.lispNumberToInt(args[0]);
      if (!nullable49.HasValue)
        return (object) null;
      int? nullable50 = this.lispNumberToInt(args[1]);
      if (!nullable50.HasValue)
        return (object) null;
      this.executer.setUnitDistinction(nullable49.Value, nullable50.Value);
      return args[0];
    }));
    this.engine.defun("distinctionstop", (Func<List<object>, object>) (args =>
    {
      this.executer.stopDistinction();
      return args[0];
    }));
    this.engine.defun("clone", (Func<List<object>, object>) (args =>
    {
      int? nullable51 = this.lispNumberToInt(args[0]);
      if (!nullable51.HasValue)
        return (object) null;
      int? nullable52 = this.lispNumberToInt(args[1]);
      if (!nullable52.HasValue)
        return (object) null;
      this.executer.setPerson(nullable51.Value, nullable52.Value);
      return args[0];
    }));
    this.engine.defun("cutinname", (Func<List<object>, object>) (args =>
    {
      float? nullable53 = this.lispNumberToFloat(args[0]);
      if (!nullable53.HasValue)
        return (object) null;
      int? nullable54 = this.lispNumberToInt(args[1]);
      if (!nullable54.HasValue)
        return (object) null;
      this.executer.setCutinName(nullable53.Value, nullable54.Value);
      return args[0];
    }));
    this.engine.defun("textflame", (Func<List<object>, object>) (args =>
    {
      int? nullable55 = this.lispNumberToInt(args[0]);
      if (!nullable55.HasValue)
        return (object) null;
      if (nullable55.Value == 0)
      {
        int? nullable56 = this.lispNumberToInt(args[1]);
        if (!nullable56.HasValue)
          return (object) null;
        this.executer.setTextFlame(nullable55.Value, nullable56.Value);
      }
      else
        this.executer.setTextFlame(nullable55.Value);
      return args[0];
    }));
    this.engine.defun("textboxarrow", (Func<List<object>, object>) (args =>
    {
      int? nullable57 = this.lispNumberToInt(args[0]);
      if (!nullable57.HasValue)
        return (object) null;
      int? nullable58 = this.lispNumberToInt(args[1]);
      if (!nullable58.HasValue)
        return (object) null;
      if (nullable57.Value == 0)
        this.executer.setBottomTextArrow(nullable58.Value);
      else
        this.executer.setTopTextArrow(nullable58.Value);
      return (object) null;
    }));
    this.engine.defun("layer", (Func<List<object>, object>) (args =>
    {
      int? nullable59 = this.lispNumberToInt(args[0]);
      if (!nullable59.HasValue)
        return (object) null;
      int? nullable60 = this.lispNumberToInt(args[1]);
      if (!nullable60.HasValue)
        return (object) null;
      this.executer.setCharaLayer(nullable59.Value, nullable60.Value);
      return args[0];
    }));
    this.engine.defun("delete", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.deleteUnit(nullable.Value);
      return args[0];
    }));
    this.engine.defun("mask", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      bool enable = (bool) args[1];
      this.executer.SetMaskEnable(nullable.Value, enable);
      return args[0];
    }));
    this.engine.defun("background", (Func<List<object>, object>) (args =>
    {
      string s = this.lispStringToString(args[0]);
      if (s == null)
        return (object) null;
      this.executer.setBackGround(s);
      return args[0];
    }));
    this.engine.defun("wait", (Func<List<object>, object>) (args =>
    {
      float? nullable = this.lispNumberToFloat(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.setWait(nullable.Value);
      return args[0];
    }));
    this.engine.defun("waitandnext", (Func<List<object>, object>) (args =>
    {
      float? nullable = this.lispNumberToFloat(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.setWait(nullable.Value, true);
      return args[0];
    }));
    this.engine.defun("fillrect", (Func<List<object>, object>) (args =>
    {
      string name = this.lispStringToString(args[0]);
      if (name == null)
        return (object) null;
      float? nullable61 = this.lispNumberToFloat(args[1]);
      if (!nullable61.HasValue)
        return (object) null;
      float? nullable62 = this.lispNumberToFloat(args[2]);
      if (!nullable62.HasValue)
        return (object) null;
      float? nullable63 = this.lispNumberToFloat(args[3]);
      if (!nullable63.HasValue)
        return (object) null;
      StoryEnvironment.colorFromName(name, out this.r, out this.g, out this.b);
      this.executer.setColorAndTime(this.r, this.g, this.b, nullable62.Value, nullable63.Value, nullable61.Value);
      this.executer.startFillrect();
      return args[0];
    }));
    this.engine.defun("subfillrect", (Func<List<object>, object>) (args =>
    {
      int? nullable64 = this.lispNumberToInt(args[0]);
      if (!nullable64.HasValue)
        return (object) null;
      string name = this.lispStringToString(args[1]);
      if (name == null)
        return (object) null;
      float? nullable65 = this.lispNumberToFloat(args[2]);
      if (!nullable65.HasValue)
        return (object) null;
      float? nullable66 = this.lispNumberToFloat(args[3]);
      if (!nullable66.HasValue)
        return (object) null;
      float? nullable67 = this.lispNumberToFloat(args[4]);
      if (!nullable67.HasValue)
        return (object) null;
      StoryEnvironment.colorFromName(name, out this.r, out this.g, out this.b);
      this.executer.setSubDepth(nullable64.Value);
      this.executer.setSubColorAndTime(this.r, this.g, this.b, nullable66.Value, nullable67.Value, nullable65.Value);
      this.executer.startSubFillrect();
      return args[0];
    }));
    this.engine.defun("framein", (Func<List<object>, object>) (args =>
    {
      int? nullable68 = this.lispNumberToInt(args[0]);
      if (!nullable68.HasValue)
        return (object) null;
      float? nullable69 = this.lispNumberToFloat(args[1]);
      if (!nullable69.HasValue)
        return (object) null;
      this.executer.startMoveFrame(nullable68.Value, false, nullable69.Value);
      return args[0];
    }));
    this.engine.defun("frameout", (Func<List<object>, object>) (args =>
    {
      int? nullable70 = this.lispNumberToInt(args[0]);
      if (!nullable70.HasValue)
        return (object) null;
      float? nullable71 = this.lispNumberToFloat(args[1]);
      if (!nullable71.HasValue)
        return (object) null;
      this.executer.startMoveFrame(nullable70.Value, true, nullable71.Value);
      return args[0];
    }));
    this.engine.defun("buttonsin", (Func<List<object>, object>) (args =>
    {
      int? nullable72 = this.lispNumberToInt(args[0]);
      if (!nullable72.HasValue)
        return (object) null;
      float? nullable73 = this.lispNumberToFloat(args[1]);
      if (!nullable73.HasValue)
        return (object) null;
      this.executer.startMoveButtons(nullable72.Value, false, nullable73.Value);
      return args[0];
    }));
    this.engine.defun("buttonsout", (Func<List<object>, object>) (args =>
    {
      int? nullable74 = this.lispNumberToInt(args[0]);
      if (!nullable74.HasValue)
        return (object) null;
      float? nullable75 = this.lispNumberToFloat(args[1]);
      if (!nullable75.HasValue)
        return (object) null;
      this.executer.startMoveButtons(nullable74.Value, true, nullable75.Value);
      return args[0];
    }));
    this.engine.defun("fadein", (Func<List<object>, object>) (args =>
    {
      string name = this.lispStringToString(args[0]);
      if (name == null)
        return (object) null;
      float? nullable = this.lispNumberToFloat(args[1]);
      if (!nullable.HasValue)
        return (object) null;
      StoryEnvironment.colorFromName(name, out this.r, out this.g, out this.b);
      this.fromAlpha = 1f;
      this.toAlpha = 0.0f;
      this.executer.setColorAndTime(this.r, this.g, this.b, this.fromAlpha, this.toAlpha, nullable.Value);
      this.executer.startFade();
      return args[0];
    }));
    this.engine.defun("fadeout", (Func<List<object>, object>) (args =>
    {
      string name = this.lispStringToString(args[0]);
      if (name == null)
        return (object) null;
      float? nullable = this.lispNumberToFloat(args[1]);
      if (!nullable.HasValue)
        return (object) null;
      StoryEnvironment.colorFromName(name, out this.r, out this.g, out this.b);
      this.fromAlpha = 0.0f;
      this.toAlpha = 1f;
      this.executer.setColorAndTime(this.r, this.g, this.b, this.fromAlpha, this.toAlpha, nullable.Value);
      this.executer.startFade();
      return args[0];
    }));
    this.engine.defun("flush", (Func<List<object>, object>) (args =>
    {
      string name = this.lispStringToString(args[0]);
      if (name == null)
        return (object) null;
      float? nullable76 = this.lispNumberToFloat(args[1]);
      if (!nullable76.HasValue)
        return (object) null;
      int? nullable77 = this.lispNumberToInt(args[2]);
      if (!nullable77.HasValue)
        return (object) null;
      StoryEnvironment.colorFromName(name, out this.r, out this.g, out this.b);
      return this.executer.startFlush(new Color(this.r, this.g, this.b), nullable76.Value, nullable77.Value);
    }));
    this.engine.defun("textwindow", (Func<List<object>, object>) (args =>
    {
      string s = this.lispStringToString(args[0]);
      switch (s)
      {
        case null:
          return (object) null;
        case "close":
        case "":
          this.executer.setTextClose(this.current.text.pos);
          break;
        case "top_close":
          this.executer.setTextClose(TextBlock.Position.TOP);
          break;
        case "bottom_close":
          this.executer.setTextClose(TextBlock.Position.BOTTOM);
          break;
        default:
          if (this.current.text.pos == TextBlock.Position.TOP)
          {
            this.executer.setTextTopWindow(s);
            break;
          }
          this.executer.setTextBottomWindow(s);
          break;
      }
      return args[0];
    }));
    this.engine.defun("textsize", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      if (this.current.text.pos == TextBlock.Position.TOP)
        this.executer.setTextSize(nullable.Value, true);
      else
        this.executer.setTextSize(nullable.Value, false);
      return args[0];
    }));
    this.engine.defun("textshake", (Func<List<object>, object>) (args =>
    {
      float? nullable = this.lispNumberToFloat(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      if (this.current.text.pos == TextBlock.Position.TOP)
        this.executer.setTextShake(nullable.Value, true);
      else
        this.executer.setTextShake(nullable.Value, false);
      return args[0];
    }));
    this.engine.defun("textcolor", (Func<List<object>, object>) (args =>
    {
      string color = this.lispStringToString(args[0]);
      if (color == null)
        return (object) null;
      this.executer.SetColorText(this.current.text.pos, color);
      return args[0];
    }));
    this.engine.defun("textalign", (Func<List<object>, object>) (args =>
    {
      string align = this.lispStringToString(args[0]);
      if (align == null)
        return (object) null;
      this.executer.SetTextAlgin(this.current.text.pos, align);
      return args[0];
    }));
    this.engine.defun("textstop", (Func<List<object>, object>) (args =>
    {
      this.executer.SetColorText(this.current.text.pos, "normal");
      if (this.current.text.pos == TextBlock.Position.TOP)
      {
        this.executer.setTextSize(24, true);
        this.executer.setTextTopWindow("normal");
      }
      else
      {
        this.executer.setTextSize(24, false);
        this.executer.setTextBottomWindow("normal");
      }
      if (this.current.text.pos == TextBlock.Position.TOP)
        this.executer.stopTextShake(true);
      else
        this.executer.stopTextShake(false);
      return args[0];
    }));
    this.engine.defun("shake", (Func<List<object>, object>) (args =>
    {
      int? nullable78 = this.lispNumberToInt(args[0]);
      if (!nullable78.HasValue)
        return (object) null;
      float w = 3f;
      if (nullable78.Value != 0)
        w = 7f;
      float? nullable79 = this.lispNumberToFloat(args[1]);
      this.executer.setShake(w, nullable79.Value);
      return args[0];
    }));
    this.engine.defun("shakeloop", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      float w = 3f;
      if (nullable.Value != 0)
        w = 7f;
      this.executer.setShake(w, 0.0f);
      return (object) null;
    }));
    this.engine.defun("shakestop", (Func<List<object>, object>) (args =>
    {
      this.executer.stopShake();
      return (object) null;
    }));
    this.engine.defun("imageset", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) false;
      string name = this.lispStringToString(args[1]);
      if (name == null)
        return (object) null;
      this.executer.setImageName(nullable.Value, name);
      return args[0];
    }));
    this.engine.defun("imagelayer", (Func<List<object>, object>) (args =>
    {
      int? nullable80 = this.lispNumberToInt(args[0]);
      if (!nullable80.HasValue)
        return (object) false;
      int? nullable81 = this.lispNumberToInt(args[1]);
      if (!nullable81.HasValue)
        return (object) false;
      this.executer.setImagePriority(nullable80.Value, nullable81.Value);
      return args[0];
    }));
    this.engine.defun("imagepos", (Func<List<object>, object>) (args =>
    {
      int? nullable82 = this.lispNumberToInt(args[0]);
      if (!nullable82.HasValue)
        return (object) null;
      float? nullable83 = this.lispNumberToFloat(args[1]);
      if (!nullable83.HasValue)
        return (object) null;
      float? nullable84 = this.lispNumberToFloat(args[2]);
      if (!nullable84.HasValue)
        return (object) null;
      this.executer.setImagePosition(nullable82.Value, nullable83.Value, nullable84.Value);
      return args[0];
    }));
    this.engine.defun("imagealpha", (Func<List<object>, object>) (args =>
    {
      int? nullable85 = this.lispNumberToInt(args[0]);
      if (!nullable85.HasValue)
        return (object) null;
      float? nullable86 = this.lispNumberToFloat(args[1]);
      if (!nullable86.HasValue)
        return (object) null;
      float? nullable87 = this.lispNumberToFloat(args[2]);
      if (!nullable87.HasValue)
        return (object) null;
      this.executer.setImageAlpha(nullable85.Value, nullable86.Value, nullable87.Value);
      return args[0];
    }));
    this.engine.defun("imagescale", (Func<List<object>, object>) (args =>
    {
      int? nullable88 = this.lispNumberToInt(args[0]);
      if (!nullable88.HasValue)
        return (object) null;
      float? nullable89 = this.lispNumberToFloat(args[1]);
      if (!nullable89.HasValue)
        return (object) null;
      float? nullable90 = this.lispNumberToFloat(args[2]);
      if (!nullable90.HasValue)
        return (object) null;
      this.executer.setImageScale(nullable88.Value, nullable89.Value, nullable90.Value);
      return args[0];
    }));
    this.engine.defun("imageleftin", (Func<List<object>, object>) (args =>
    {
      int? nullable91 = this.lispNumberToInt(args[0]);
      if (!nullable91.HasValue)
        return (object) null;
      float? nullable92 = this.lispNumberToFloat(args[1]);
      if (!nullable92.HasValue)
        return (object) null;
      this.executer.setImageMoveIn(nullable91.Value, nullable92.Value, -2500f, 0.0f);
      return args[0];
    }));
    this.engine.defun("imagerightin", (Func<List<object>, object>) (args =>
    {
      int? nullable93 = this.lispNumberToInt(args[0]);
      if (!nullable93.HasValue)
        return (object) null;
      float? nullable94 = this.lispNumberToFloat(args[1]);
      if (!nullable94.HasValue)
        return (object) null;
      this.executer.setImageMoveIn(nullable93.Value, nullable94.Value, 2500f, 0.0f);
      return args[0];
    }));
    this.engine.defun("imageleftout", (Func<List<object>, object>) (args =>
    {
      int? nullable95 = this.lispNumberToInt(args[0]);
      if (!nullable95.HasValue)
        return (object) null;
      float? nullable96 = this.lispNumberToFloat(args[1]);
      if (!nullable96.HasValue)
        return (object) null;
      this.executer.setImageMoveOut(nullable95.Value, nullable96.Value, -2500f, 0.0f);
      return args[0];
    }));
    this.engine.defun("imagerightout", (Func<List<object>, object>) (args =>
    {
      int? nullable97 = this.lispNumberToInt(args[0]);
      if (!nullable97.HasValue)
        return (object) null;
      float? nullable98 = this.lispNumberToFloat(args[1]);
      if (!nullable98.HasValue)
        return (object) null;
      this.executer.setImageMoveOut(nullable97.Value, nullable98.Value, 2500f, 0.0f);
      return args[0];
    }));
    this.engine.defun("imagemoveto", (Func<List<object>, object>) (args =>
    {
      int? nullable99 = this.lispNumberToInt(args[0]);
      if (!nullable99.HasValue)
        return (object) null;
      float? nullable100 = this.lispNumberToFloat(args[1]);
      if (!nullable100.HasValue)
        return (object) null;
      float? nullable101 = this.lispNumberToFloat(args[2]);
      if (!nullable101.HasValue)
        return (object) null;
      float? nullable102 = this.lispNumberToFloat(args[3]);
      if (!nullable102.HasValue)
        return (object) null;
      this.executer.setImageMoveOut(nullable99.Value, nullable102.Value, nullable100.Value, nullable101.Value);
      return args[0];
    }));
    this.engine.defun("emotion", (Func<List<object>, object>) (args =>
    {
      if (!this.lispNumberToInt(args[0]).HasValue)
        return (object) false;
      string str = this.lispStringToString(args[1]);
      if (str == null)
        return (object) null;
      if (!this.lispNumberToFloat(args[2]).HasValue)
        return (object) null;
      if (!this.lispNumberToFloat(args[3]).HasValue)
        return (object) null;
      switch (str)
      {
        case "del":
          this.executer.deleteEmotion();
          break;
        case "brightness":
          this.executer.setEmotionBright();
          break;
        default:
          this.executer.setEmotion();
          break;
      }
      return args[0];
    }));
    this.engine.defun("voice", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) false;
      string name = this.lispStringToString(args[1]);
      if (name == null)
        return (object) null;
      this.executer.setVoice(nullable.Value.ToString(), name);
      return args[0];
    }));
    this.engine.defun("voicedelay", (Func<List<object>, object>) (args =>
    {
      int? nullable103 = this.lispNumberToInt(args[0]);
      if (!nullable103.HasValue)
        return (object) false;
      string name = this.lispStringToString(args[1]);
      if (name == null)
        return (object) null;
      float? nullable104 = this.lispNumberToFloat(args[2]);
      if (!nullable104.HasValue)
        return (object) false;
      this.executer.setVoice(nullable103.Value.ToString(), name, nullable104.Value);
      return args[0];
    }));
    this.engine.defun("se", (Func<List<object>, object>) (args =>
    {
      string clip = this.lispStringToString(args[0]);
      if (clip == null)
        return (object) null;
      this.executer.setSe(clip);
      return args[0];
    }));
    this.engine.defun("sedelay", (Func<List<object>, object>) (args =>
    {
      string clip = this.lispStringToString(args[0]);
      if (clip == null)
        return (object) null;
      float? nullable = this.lispNumberToFloat(args[1]);
      if (!nullable.HasValue)
        return (object) false;
      this.executer.setSe(clip, nullable.Value);
      return args[0];
    }));
    this.engine.defun("sestop", (Func<List<object>, object>) (args =>
    {
      string clip = this.lispStringToString(args[0]);
      if (clip == null)
        return (object) null;
      this.executer.stopSe(clip);
      return args[0];
    }));
    this.engine.defun("sestopdelay", (Func<List<object>, object>) (args =>
    {
      string clip = this.lispStringToString(args[0]);
      if (clip == null)
        return (object) null;
      float? nullable = this.lispNumberToFloat(args[1]);
      if (!nullable.HasValue)
        return (object) false;
      this.executer.stopSe(clip, nullable.Value);
      return args[0];
    }));
    this.engine.defun("bgm", (Func<List<object>, object>) (args =>
    {
      string s = this.lispStringToString(args[0]);
      switch (s)
      {
        case null:
          return (object) null;
        case "stop":
          this.executer.stopBgm();
          break;
        default:
          float? nullable = this.lispNumberToFloat(args[1]);
          if ((double) nullable.Value < 0.699999988079071)
          {
            this.executer.setBgm(s);
            break;
          }
          this.executer.setBgm(s, nullable.Value);
          break;
      }
      return args[0];
    }));
    this.engine.defun("bgmfile", (Func<List<object>, object>) (args =>
    {
      string s = this.lispStringToString(args[0]);
      switch (s)
      {
        case null:
          return (object) null;
        case "stop":
          this.executer.stopBgm();
          break;
        default:
          string file = this.lispStringToString(args[1]);
          float? nullable = this.lispNumberToFloat(args[2]);
          if ((double) nullable.Value < 0.699999988079071)
          {
            this.executer.setBgmFile(file, s);
            break;
          }
          this.executer.setBgmFile(file, s, nullable.Value);
          break;
      }
      return args[0];
    }));
    this.engine.defun("setnextbgmblock", (Func<List<object>, object>) (args =>
    {
      int? nullable = this.lispNumberToInt(args[0]);
      if (!nullable.HasValue)
        return (object) null;
      this.executer.setNextBgmBlock(nullable.Value);
      return args[0];
    }));
    this.engine.defun("movieplay", (Func<List<object>, object>) (args =>
    {
      string fileName = this.lispStringToString(args[0]);
      if (fileName == null)
        return (object) null;
      bool enabledSkip = (args.Count<object>() < 2 ? "skip" : this.lispStringToString(args[1])) == "skip";
      this.executer.PlayMovie(fileName, enabledSkip);
      return args[0];
    }));
    this.engine.defun("flag", (Func<List<object>, object>) (args =>
    {
      if (args.Count < 2)
        return (object) null;
      return this.lispStringToString(args[0]) == null ? (object) null : args[0];
    }));
    this.engine.defun("flagjump", (Func<List<object>, object>) (args =>
    {
      if (args.Count < 2)
        return (object) null;
      string v = this.lispStringToString(args[0]);
      if (v == null)
        return (object) null;
      bool? nullable = (bool?) this.getVariable(v);
      if (!nullable.HasValue)
      {
        this.setVariable(v, (object) false);
        nullable = new bool?(false);
      }
      this.setNextLabel(this.lispStringToString(args[1]));
      return args[0];
    }));
    this.engine.defun("labeljump", (Func<List<object>, object>) (args =>
    {
      if (args.Count < 1)
        return (object) null;
      this.setNextLabel(this.lispStringToString(args[0]));
      return args[0];
    }));
    this.engine.defun("select", (Func<List<object>, object>) (args =>
    {
      if (args.Count < 2 || args.Count % 2 != 0)
        return (object) null;
      SelectBlock sb = new SelectBlock();
      int num = args.Count / 2;
      for (int index = 0; index < num; ++index)
      {
        SelectBlock.Data data = new SelectBlock.Data();
        data.msg = this.lispStringToString(args[index * 2]);
        data.label = this.lispStringToString(args[index * 2 + 1]);
        if (data.msg == null || data.label == null)
          return (object) null;
        sb.data.Add(data);
      }
      this.current.setSelect(sb);
      this.executer.StartSelect(sb);
      this.current.next_enable = false;
      return args[0];
    }));
    this.engine.defun("popupstoryeffect", (Func<List<object>, object>) (args =>
    {
      if (args.Count < 1)
        return (object) null;
      this.executer.PopupStoryEffect(this.lispStringToString(args[0]));
      return args[0];
    }));
  }

  public bool SkipReady()
  {
    if (StoryEnvironment.IsNoWaitOnChoise)
      return true;
    return this.storyBlocks != null && this.storyBlocks.Count > 0 && this.current.select == null;
  }

  public StoryBlock current => this.storyBlocks[this.currentIdx];

  public ReadOnlyCollection<StoryBlock> all()
  {
    return new ReadOnlyCollection<StoryBlock>((IList<StoryBlock>) this.storyBlocks);
  }

  public void initialize(string program, StoryExecuter exec)
  {
    this.engine = new Lisp(this.scriptEnv);
    this.executer = exec;
    this.defineVariables();
    this.defunCommands();
    this.storyBlocks = new StoryParser().parse(program, this.scriptEnv);
    this.currentIdx = 0;
  }

  public void setVariable(string v, object o) => this.scriptEnv[v] = o;

  public object getVariable(string v)
  {
    try
    {
      return this.scriptEnv[v];
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("getVariable not found name=" + v));
      return (object) null;
    }
  }

  public object nextBlock()
  {
    if (this.nextBlockp())
    {
      if (this.nextLabel == null)
      {
        ++this.currentIdx;
      }
      else
      {
        this.setCurrent(this.nextLabel);
        this.nextLabel = (string) null;
      }
    }
    return (object) this.storyBlocks[this.currentIdx];
  }

  public object resetBlock()
  {
    this.currentIdx = 0;
    return (object) this.storyBlocks[this.currentIdx];
  }

  public object backBlock() => (object) this.storyBlocks[this.currentIdx - 1];

  public bool nextBlockp()
  {
    return this.nextLabel != null || this.currentIdx < this.storyBlocks.Count - 1;
  }

  public object setCurrent(string label)
  {
    int num = 0;
    foreach (StoryBlock storyBlock in this.storyBlocks)
    {
      if (storyBlock.label == label)
      {
        this.currentIdx = num;
        return (object) label;
      }
      ++num;
    }
    return (object) null;
  }

  public void evalCurrent() => this.current.eval(this.engine);

  public void setNextLabel(string label) => this.nextLabel = label;
}
