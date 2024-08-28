import os
import json

from lib.PotkPaths import PotkPaths
from lib.story_viewer.ScriptFileActions import ScriptFileActions
import lib.MasterData as MasterData

class ParsedScriptFile:
    actions: list
    itr: int

    def __init__(self, scriptId: str):
        self.actions = []
        self.itr = 0
        jsdt = MasterData.getScript(scriptId)
        scriptTxt = jsdt[0]['script']
        scriptLst = scriptTxt.split('\n')
        for line in scriptLst:
            self.parse(line)
        print('Loaded script with ' + str(len(self.actions)) + ' actions')

    def parse(self, line):
        if not line:
            #break for user action
            self.actions.extend([ScriptFileActions.WaitForUser()])
        elif line[:2] == ';;':
            #Comment - do nothing
            pass
        elif line[:1] == '#':
            #lisp action - handle
            lispAction = line[1:].strip().split(' ')
            match lispAction[0]:
                case 'background':
                    self.actions.extend([ScriptFileActions.SetBackground(lispAction[1])])
                case 'script':
                    #just notes the format - do nothing so long as the format is lisp
                    if lispAction[1] != '"lisp"':
                        print('UNKNOWN SCRIPT FORMAT: ' + lispAction[1])
                case 'waitandnext':
                    # wait, then automatically proceed
                    self.actions.extend([ScriptFileActions.SetLockoutTime(float(lispAction[1]))])
                    
                    
                case 'textboxarrow':
                    # clear the textbox and set text scroll time
                    self.actions.extend([ScriptFileActions.SetTextboxArrow(int(lispAction[1]), int(lispAction[2]))])
                case 'textflame':
                    # clear the textbox and set text scroll time
                    self.actions.extend([ScriptFileActions.SetTextboxBackground(int(lispAction[1]), int(lispAction[2]))])
                case 'textwindow':
                    # clear the textbox and set text scroll time
                    self.actions.extend([ScriptFileActions.SetTextboxStyle(lispAction[1].replace('"',''))])
                case 'textsize':
                    # clear the textbox and set text scroll time
                    self.actions.extend([ScriptFileActions.SetTextSize(int(lispAction[1]))])
                    
                    
                case 'body':
                    # Add a new character to the scene (id)
                    self.actions.extend([ScriptFileActions.AddCharacter(int(lispAction[1]))])      
                case 'entry':
                    # Add a duplicated character to the scene (clone id, id)
                    self.actions.extend([ScriptFileActions.AddCharacterCopy(int(lispAction[1]), int(lispAction[2]))])
                case 'mask':
                    # Set the transparency mask on or off (id, on/off)
                    self.actions.extend([ScriptFileActions.SetCharacterMask(int(lispAction[1]), lispAction[2])])
                case 'pos':
                    # Set the character position (id, position)
                    self.actions.extend([ScriptFileActions.SetCharacterPos(int(lispAction[1]), int(lispAction[2]))])
                case 'face':
                    # Set the character face (id, face name)
                    self.actions.extend([ScriptFileActions.SetCharacterFace(int(lispAction[1]), lispAction[2].replace('"',''))])
                case 'scale':
                    # Set the character size (id, scale%, duration)
                    self.actions.extend([ScriptFileActions.SetCharacterScale(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]))])
                case 'alpha':
                    # Set the character transparency (id, alpha%, duration)
                    self.actions.extend([ScriptFileActions.SetCharacterAlpha(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]))])
                case 'chara':
                    # ??????? Old filler action or something??
                    pass#self.actions.extend([ScriptFileActions.SetCharacterSpeaking(int(lispAction[1]))])
                case 'move':
                    # Set the character transparency (id, alpha%, duration)
                    self.actions.extend([ScriptFileActions.MoveCharacter(int(lispAction[1]), int(lispAction[2]), float(lispAction[3]))])
                    
                    
                case 'fadeout':
                    # Fade the screen out to a color (image name, duration)
                    self.actions.extend([ScriptFileActions.FadeOut(lispAction[1].replace('"',''), float(lispAction[2]))])
                case 'fadein':
                    # Fade the screen out to a color (image name, duration)
                    self.actions.extend([ScriptFileActions.FadeIn(lispAction[1].replace('"',''), float(lispAction[2]))])  
                case 'imageset':
                    # Add a special effect art asset (se id, image art name)
                    self.actions.extend([ScriptFileActions.AddSpecialEffect(int(lispAction[1]), lispAction[2].replace('"',''))])
                case 'imagealpha':
                    # Set the art of a special effect (se id, alpha, duration)
                    self.actions.extend([ScriptFileActions.SetSpecialEffectAlpha(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]))])
                case 'imagescale':
                    # Set the scale of a special effect (se id, scale, duration)
                    self.actions.extend([ScriptFileActions.SetSpecialEffectScale(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]))])
                case 'imagepos':
                    # Set the position of a special effect (se id, x, y)
                    self.actions.extend([ScriptFileActions.SetSpecialEffectPos(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]))])
                case 'imagemoveto':
                    # Set the position of a special effect over time (se id, x, y, duration)
                    self.actions.extend([ScriptFileActions.MoveSpecialEffect(int(lispAction[1]), float(lispAction[2]), float(lispAction[3]), float(lispAction[4]))])
                        
                    
                case 'bgm':
                    # Set the background music (bgm name, [optional] volume???)
                    if len(lispAction) == 2:
                        self.actions.extend([ScriptFileActions.SetBackgroundMusic(lispAction[1].replace('"',''), 0)])
                    else:
                        self.actions.extend([ScriptFileActions.SetBackgroundMusic(lispAction[1].replace('"',''), float(lispAction[2]))])
                case 'bgmfile':
                    # Set the background music (bgm name, file name, [optional] volume???)
                    self.actions.extend([ScriptFileActions.SetBackgroundMusic(lispAction[1].replace('"',''), float(lispAction[3]), file=lispAction[2].replace('"',''))])
                case 'voice':
                    # Play a voice line (character id, cue name)
                    self.actions.extend([ScriptFileActions.PlayVoiceLine(int(lispAction[1]), lispAction[2].replace('"',''))])
                case 'se':
                    # Play a sound effect (cue name)
                    self.actions.extend([ScriptFileActions.PlaySoundEffect(lispAction[1].replace('"',''))])
                case 'sedelay':
                    # Play a sound effect after a delay (cue name, delay)
                    self.actions.extend([ScriptFileActions.DelayedAction(ScriptFileActions.PlaySoundEffect(lispAction[1].replace('"','')), float(lispAction[2]))])
                case 'select':
                    # create selection buttons
                    #print('Select has ' + str(len(lispAction)) + ' arguments')
                    btncnt = int((len(lispAction) - 1) / 2)
                    btns = []
                    for i in range(btncnt):
                        #print('Iteration at step ' + str(i) + ' with text ' + lispAction[i * 2 + 1].replace('"','') + ' and label ' + lispAction[i * 2 + 2].replace('"',''))
                        btns += [(lispAction[i * 2 + 1].replace('"',''), lispAction[i * 2 + 2].replace('"',''))]
                    self.actions.extend([ScriptFileActions.CreateUserChoice(btns)])
                case 'label':
                    # Set the current label name
                    self.actions.extend([ScriptFileActions.SetLabel(lispAction[1].replace('"',''))])
                case 'labeljump':
                    # Set the target label name to skip to
                    self.actions.extend([ScriptFileActions.LabelJump(lispAction[1].replace('"',''))])
                case _:
                    #Unknown Action
                    print('Unknown lisp action found: ' + lispAction[0])
        elif line[:1] == '@':
            #Change speaker
            self.actions.extend([ScriptFileActions.SetSpeaker(line[1:])])
            pass
        else:
            #dialog line
            self.actions.extend([ScriptFileActions.AddDialog(line)])
            pass

    def runNextAction(self, env):
        if self.itr < len(self.actions):
            self.actions[self.itr].run(env)
            self.itr+=1
        else:
            env.waitForInput()