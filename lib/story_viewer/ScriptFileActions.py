import os

from lib.PotkPaths import PotkPaths
from lib.PotkRes import PotkRes
from lib.conf.conf import conf
import lib.AssetBundle as AssetBundle

class ScriptFileActions:
    class WaitForUser:
        def run(self, env):
            env.waitForInput()
    class SetLockoutTime:
        def __init__(self, time):
            self.time = time * 60
        def run(self, env):
            env.setLockoutTime(self.time)
    class DelayedAction:
        def __init__(self, action, time):
            self.action = action
            self.time = time
        def run(self, env):
            if self.time > 0:
                env.registerTimedAction(ScriptFileActions.DelayedActionWaiting(env, self.action, self.time))
            else:
                self.action.run(env)
    class DelayedActionWaiting:
        def __init__(self, env, action, time):
            self.action = action
            self.timesRemaining = int(time * conf.fps)
            #print('Set Alpha Periodic action: ' + str(self.cid) + ' to ' + str(self.alphaTarget) + ' by changing ' + str(self.alphaStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            self.timesRemaining -= 1
            if self.timesRemaining <= 0:
                env.unregisterTimedAction(self)
                self.action.run(env)
            
            
    class SetBackground:
        def __init__(self, name):
            self.img = AssetBundle.getBackground(name.replace('"',''))
        def run(self, env):
            env.setBackgroundImage(self.img)
    class SetSpeaker:
        def __init__(self, name):
            self.name = name
        def run(self, env):
            env.setSpeaker(self.name)
    class SetTextboxBackground:
        def __init__(self, frameType, time):
            self.frameType = frameType
            self.time = time
        def run(self, env):
            env.setTextboxBackground(self.frameType, self.time)
    class SetTextboxStyle:
        def __init__(self, style):
            self.style = style
        def run(self, env):
            env.setTextboxStyle(self.style)
    class SetTextboxArrow:
        def __init__(self, boxId, time):
            self.boxId = boxId
            self.time = time
        def run(self, env):
            env.setTextboxArrow(self.boxId, self.time)
    class SetTextSize:
        def __init__(self, size):
            self.size = size
        def run(self, env):
            env.setTextSize(self.size)
    class AddDialog:
        def __init__(self, line):
            self.line = line
        def run(self, env):
            env.addDialog(self.line)
            
            
    class AddCharacter:
        def __init__(self, cid):
            self.cid = cid
        def run(self, env):
            env.addCharacter(self.cid)
    class AddCharacterCopy:
        def __init__(self, eid, cid):
            self.eid = eid
            self.cid = cid
        def run(self, env):
            env.addCharacterCopy(self.eid, self.cid)
    class SetCharacterMask:
        def __init__(self, cid, mask):
            self.cid = cid
            self.mask = mask
        def run(self, env):
            env.setCharacterMask(self.cid, self.mask)
    class SetCharacterFace:
        def __init__(self, cid, face):
            self.cid = cid
            self.face = face
        def run(self, env):
            env.setCharacterFace(self.cid, self.face)
    class SetCharacterPos:
        def __init__(self, cid, pos):
            self.cid = cid
            self.pos = pos
        def run(self, env):
            env.setCharacterStoryPos(self.cid, self.pos)
    class SetCharacterAlpha:
        def __init__(self, cid, alpha, time):
            self.cid = cid
            self.alpha = alpha
            self.time = time
        def run(self, env):
            if self.time > 0:
                #print('Set Alpha Duration: ' + str(self.cid) + ' to ' + str(self.alpha) + ' over ' + str(self.time))
                env.registerTimedAction(ScriptFileActions.SetCharacterAlphaPeriodic(env, self.cid, self.alpha, self.time))
            else:
                #print('Set Alpha Instant: ' + str(self.cid) + ' to ' + str(self.alpha))
                env.setCharacterAlpha(self.cid, self.alpha)
    class SetCharacterAlphaPeriodic:
        def __init__(self, env, cid, alpha, time):
            self.cid = cid
            self.alphaTarget = alpha
            self.alphaStep = (alpha - env.getCharacter(cid).alpha) / (time * conf.fps)
            self.timesRemaining = int(time * conf.fps + 2)
            #print('Set Alpha Periodic action: ' + str(self.cid) + ' to ' + str(self.alphaTarget) + ' by changing ' + str(self.alphaStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            char = env.getCharacter(self.cid)
            newAlpha = char.alpha + self.alphaStep
            if self.alphaStep > 0:
                newAlpha = min(newAlpha,self.alphaTarget)
            else:
                newAlpha = max(newAlpha,self.alphaTarget)
            env.setCharacterAlpha(self.cid, newAlpha)
            #print('Periodic Setting Alpha for: ' + str(self.cid) + ' to ' + str(newAlpha) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            if self.timesRemaining <= 0 or char.alpha == self.alphaTarget:
                env.unregisterTimedAction(self)
    class SetCharacterScale:
        def __init__(self, cid, scale, time):
            self.cid = cid
            self.scale = scale
            self.time = time
        def run(self, env):
            if self.time > 0:
                #print('Set Scale Duration: ' + str(self.cid) + ' to ' + str(self.scale) + ' over ' + str(self.time))
                env.registerTimedAction(ScriptFileActions.SetCharacterScalePeriodic(env, self.cid, self.scale, self.time))
            else:
                #print('Set Scale Instant: ' + str(self.cid) + ' to ' + str(self.scale))
                env.setCharacterScale(self.cid, self.scale)
    class SetCharacterScalePeriodic:
        def __init__(self, env, cid, scale, time):
            self.cid = cid
            self.scaleTarget = scale
            self.scaleStep = (scale - env.getCharacter(cid).scale) / (time * conf.fps)
            self.timesRemaining = int(time * conf.fps + 2)
            #print('Set Scale Periodic action: ' + str(self.cid) + ' to ' + str(self.scaleTarget) + ' by changing ' + str(self.scaleStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            char = env.getCharacter(self.cid)
            newScale = char.scale + self.scaleStep
            if self.scaleStep > 0:
                newScale = min(newScale,self.scaleTarget)
            else:
                newScale = max(newScale,self.scaleTarget)
            env.setCharacterScale(self.cid, newScale)
            #print('Periodic Setting Scale for: ' + str(self.cid) + ' to ' + str(newScale) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            if self.timesRemaining <= 0 or char.scale == self.scaleTarget:
                env.unregisterTimedAction(self)
    class MoveCharacter:
        def __init__(self, cid, pos, time):
            self.cid = cid
            self.pos = pos
            self.time = time
        def run(self, env):
            if self.time > 0:
                char = env.getCharacter(self.cid)
                env.startMovingCharacterToStoryPos(self.cid, self.pos)
                tarPos = char.storyPosToUnitPos(self.pos)
                print(f'Moving unit over time: from {char.pos} to {tarPos} (based on {self.pos})')
                env.registerTimedAction(ScriptFileActions.MoveCharacterPeriodic(env, self.cid, tarPos[0], tarPos[1], self.time))
            else:
                #print('Set Alpha Instant: ' + str(self.cid) + ' to ' + str(self.alpha))
                env.setCharacterStoryPos(self.cid, self.pos)
    class MoveCharacterPeriodic:
        def __init__(self, env, cid, x, y, time):
            char = env.getCharacter(cid)
            self.cid = cid
            self.xTarget = x
            self.yTarget = char.pos[1]
            self.xStep = (x - char.pos[0]) / (time * conf.fps)
            self.yStep = 0#(y - char.pos[1]) / (time * conf.fps)
            print(f'Creating movement from x:{char.pos[0]} to x:{x} and y:{char.pos[1]} to y:{y}')
            self.timesRemaining = int(time * conf.fps + 2)
            print(f'Move Character Periodic action: {self.cid} to {self.xTarget},{self.yTarget}  by changing {self.xStep},{self.yStep} {self.timesRemaining} times')
        def run(self, env):
            char = env.getCharacter(self.cid)
            newX = char.pos[0] + self.xStep
            #print(f'From {char.pos[0]} to {newX} pre-check')
            if self.xStep > 0:
                newX = min(newX,self.xTarget)
            else:
                newX = max(newX,self.xTarget)
            #print(f'From {char.pos[0]} to {newX} post-check')
            
            newY = char.pos[1] + self.yStep
            if self.yStep > 0:
                newY = min(newY,self.yTarget)
            else:
                newY = max(newY,self.yTarget)
                
            env.setCharacterPos(self.cid, newX, newY)
            #print('Periodic Setting Position for: ' + str(self.cid) + ' to ' + str(newX) + ',' + str(newY) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            #print('Checking unregister: ' + str(char.pos[0]) + ',' + str(char.pos[1]) + ' vs ' + str(self.xTarget) + ',' + str(self.yTarget) + ' (' + str(self.timesRemaining) + ' times remaining)')
            if self.timesRemaining <= 0 or (char.pos[0] == self.xTarget and char.pos[1] == self.yTarget):
                env.finishMovingCharacter(self.cid)
                env.unregisterTimedAction(self)
    class SetCharacterSpeaking:
        def __init__(self, cid):
            self.cid = cid
        def run(self, env):
            env.setCharacterSpeaking(self.cid)
            
            
    class FadeOut:
        def __init__(self, name, time):
            self.name = name
            self.time = time
        def run(self, env):
            if self.time > 0:
                env.addSpecialEffect('screen', 'bg_' + self.name, initialAlpha=0.0)
                env.registerTimedAction(ScriptFileActions.SetSpecialEffectAlphaPeriodic(env, 'screen', 1.0, self.time))
            else:
                env.addSpecialEffect('screen', 'bg_' + self.name)
    class FadeIn:
        def __init__(self, name, time):
            self.name = name
            self.time = time
        def run(self, env):
            if self.time > 0:
                env.addSpecialEffect('screen', 'bg_' + self.name)
                env.registerTimedAction(ScriptFileActions.SetSpecialEffectAlphaPeriodic(env, 'screen', 0.0, self.time))
            else:
                env.addSpecialEffect('screen', 'bg_' + self.name, initialAlpha=0.0)
    class AddSpecialEffect:
        def __init__(self, seId, name):
            self.seId = seId
            self.name = name
        def run(self, env):
            env.addSpecialEffect(self.seId, self.name)
    class SetSpecialEffectPos:
        def __init__(self, seId, x, y):
            self.seId = seId
            self.x = x
            self.y = y
        def run(self, env):
            env.setSpecialEffectPos(self.seId, self.x, self.y)
    class MoveSpecialEffect:
        def __init__(self, seId, x, y, time):
            self.seId = seId
            self.x = x
            self.y = y
            self.time = time
        def run(self, env):
            if self.time > 0:
                #print('Set Alpha Duration: ' + str(self.cid) + ' to ' + str(self.alpha) + ' over ' + str(self.time))
                env.registerTimedAction(ScriptFileActions.MoveSpecialEffectPeriodic(env, self.seId, self.x, self.y, self.time))
            else:
                #print('Set Alpha Instant: ' + str(self.cid) + ' to ' + str(self.alpha))
                env.setSpecialEffectPos(self.seId, self.x, self.y)
    class MoveSpecialEffectPeriodic:
        def __init__(self, env, seId, x, y, time):
            self.seId = seId
            self.xTarget = x
            self.yTarget = y
            self.xStep = (x - env.getSpecialEffect(seId).pos[0]) / (time * conf.fps)
            self.yStep = (y - env.getSpecialEffect(seId).pos[1]) / (time * conf.fps)
            self.timesRemaining = int(time * conf.fps + 2)
            #print('Set Alpha Periodic action: ' + str(self.cid) + ' to ' + str(self.alphaTarget) + ' by changing ' + str(self.alphaStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            se = env.getSpecialEffect(self.seId)
            newX = se.pos[0] + self.xStep
            if self.xStep > 0:
                newX = min(newX,self.xTarget)
            else:
                newX = max(newX,self.xTarget)
            
            newY = se.pos[1] + self.yStep
            if self.yStep > 0:
                newY = min(newY,self.yTarget)
            else:
                newY = max(newY,self.yTarget)
                
            env.setSpecialEffectPos(self.seId, newX, newY)
            #print('Periodic Setting Position for: ' + str(self.seId) + ' to ' + str(newX) + ',' + str(newY) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            #print('Checking unregister: ' + str(se.pos[0]) + ',' + str(se.pos[1]) + ' vs ' + str(self.xTarget) + ',' + str(self.yTarget) + ' (' + str(self.timesRemaining) + ' times remaining)')
            if self.timesRemaining <= 0 or (se.pos[0] == self.xTarget and se.pos[1] == self.yTarget):
                print('UNREGISTERED')
                env.unregisterTimedAction(self)
    class SetSpecialEffectScale:
        def __init__(self, seId, scale, time):
            self.seId = seId
            self.scale = scale
            self.time = time
        def run(self, env):
            if self.time > 0:
                #print('Set Scale Duration: ' + str(self.seId) + ' to ' + str(self.scale) + ' over ' + str(self.time))
                env.registerTimedAction(ScriptFileActions.SetSpecialEffectScalePeriodic(env, self.seId, self.scale, self.time))
            else:
                #print('Set Scale Instant: ' + str(self.seId) + ' to ' + str(self.scale))
                env.setSpecialEffectScale(self.seId, self.scale)
    class SetSpecialEffectScalePeriodic:
        def __init__(self, env, seId, scale, time):
            self.seId = seId
            self.scaleTarget = scale
            self.scaleStep = (scale - env.getSpecialEffect(seId).scale) / (time * conf.fps)
            self.timesRemaining = int(time * conf.fps + 2)
            #print('Set Scale Periodic action: ' + str(self.seId) + ' to ' + str(self.scaleTarget) + ' by changing ' + str(self.scaleStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            se = env.getSpecialEffect(self.seId)
            newScale = se.scale + self.scaleStep
            if self.scaleStep > 0:
                newScale = min(newScale,self.scaleTarget)
            else:
                newScale = max(newScale,self.scaleTarget)
            env.setSpecialEffectScale(self.seId, newScale)
            #print('Periodic Setting Scale for: ' + str(self.seId) + ' to ' + str(newScale) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            if self.timesRemaining <= 0 or se.scale == self.scaleTarget:
                env.unregisterTimedAction(self)
    class SetSpecialEffectAlpha:
        def __init__(self, seId, alpha, time):
            self.seId = seId
            self.alpha = alpha
            self.time = time
        def run(self, env):
            if self.time > 0:
                #print('Set Alpha Duration: ' + str(self.cid) + ' to ' + str(self.alpha) + ' over ' + str(self.time))
                env.registerTimedAction(ScriptFileActions.SetSpecialEffectAlphaPeriodic(env, self.seId, self.alpha, self.time))
            else:
                #print('Set Alpha Instant: ' + str(self.cid) + ' to ' + str(self.alpha))
                env.setSpecialEffectAlpha(self.seId, self.alpha)
    class SetSpecialEffectAlphaPeriodic:
        def __init__(self, env, seId, alpha, time):
            self.seId = seId
            self.alphaTarget = alpha
            self.alphaStep = (alpha - env.getSpecialEffect(seId).alpha) / (time * conf.fps)
            self.timesRemaining = time * conf.fps + 2
            #print('Set Alpha Periodic action: ' + str(self.cid) + ' to ' + str(self.alphaTarget) + ' by changing ' + str(self.alphaStep) + ' ' + str(self.timesRemaining) + ' times')
        def run(self, env):
            se = env.getSpecialEffect(self.seId)
            newAlpha = se.alpha + self.alphaStep
            if self.alphaStep > 0:
                newAlpha = min(newAlpha,self.alphaTarget)
            else:
                newAlpha = max(newAlpha,self.alphaTarget)
            env.setSpecialEffectAlpha(self.seId, newAlpha)
            #print('Periodic Setting Alpha for: ' + str(self.cid) + ' to ' + str(newAlpha) + ' (' + str(self.timesRemaining) + ' times remaining)')
            self.timesRemaining -= 1
            if self.timesRemaining <= 0 or se.alpha == self.alphaTarget:
                env.unregisterTimedAction(self)
            
            
    class SetBackgroundMusic:
        def __init__(self, name, num, file=None):
            self.name = name
            self.num = num
            if file:
                self.path = PotkPaths.getMusicFilePath(file)
            elif name != 'stop':
                self.path = PotkPaths.getMusicPath('BgmCueSheet')
            else:
                self.num = 0
                self.path = ''
        def run(self, env):
            env.setBackgroundMusic(self.path, self.name, self.num)
    class PlayVoiceLine:
        def __init__(self, cid, cue):
            self.cid = cid
            self.cue = cue
        def run(self, env):
            env.playVoiceLine(self.cid, self.cue)
    class PlaySoundEffect:
        def __init__(self, cue):
            self.cue = cue
        def run(self, env):
            env.playSoundEffect(self.cue)
            
            
    class CreateUserChoice:
        def __init__(self, buttons):
            self.buttons = buttons
        def run(self, env):
            for i in range(len(self.buttons)):
                env.createButton(i, self.buttons[i][0], ScriptFileActions.UserChoice(self.buttons[i][1]))
    class UserChoice:
        def __init__(self, label):
            self.label = label
        def run(self, env):
            env.setCurrentTargetLabel(self.label)
            env.clearButtons()
            env.proceedAfterWait()
    class SetLabel:
        def __init__(self, label):
            self.label = label
        def run(self, env):
            env.setLabel(self.label)
    class LabelJump:
        def __init__(self, label):
            self.label = label
        def run(self, env):
            env.setNextTargetLabel(self.label)
    