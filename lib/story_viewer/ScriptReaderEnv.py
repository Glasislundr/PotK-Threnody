import pygame

from lib.story_viewer.ParsedScriptFile import ParsedScriptFile
from lib.story_viewer.TextBox import TextBox
from lib.Character import Character
from lib.SpecialEffect import SpecialEffect
from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.colors import colors
from lib.story_viewer.StoryButton import StoryButton
from lib.conf.conf import conf

class ScriptReaderEnv:
    script: ParsedScriptFile
    timeLockout: int
    timedActions: list
    waiting: bool
    characters: dict
    characterLayers: list
    effects: dict
    effectLayers: list
    textboxes: list
    activeTextBox: TextBox
    bgm: str
    buttons: list
    label: str
    curTargetLabel: str
    nextTargetLabel: str
    updated: bool
    
    lastVoice: pygame.mixer.Sound
    def __init__(self, script):
        self.waiting = False
        self.background = None
        self.characters = {}
        self.characterLayers = [[]]
        self.effects = {}
        self.effectLayers = [[]]
        self.activeTextBox = TextBox()
        self.textboxes = [TextBox(),self.activeTextBox]
        self.timeLockout = 0
        self.timedActions = []
        self.bgm = None
        self.script = script
        self.buttons = []
        self.label = None
        self.curTargetLabel = None
        self.nextTargetLabel = None
        self.updated = True
        self.lastVoice = None
        
        PotkRes.preloadStandardStoryGuiElements()
        PotkRes.preloadUnitData()
    def setLockoutTime(self, time):
        self.timeLockout = time
    def waitForInput(self):
        #print('Comparing label: ' + str(self.label) + ', curTar: ' + str(self.curTargetLabel) + ', and nextTar: ' + str(self.nextTargetLabel))
        if self.curTargetLabel and self.curTargetLabel != self.label:
            self.label = None
            self.nextTargetLabel = None
        else:
            self.waiting = True
            self.curTargetLabel = self.nextTargetLabel
            self.nextTargetLabel = None
            self.label = None
    def proceedAfterWait(self):
        self.waiting = False
        if self.lastVoice is not None and self.lastVoice.get_num_channels():
            self.lastVoice.stop()
    def registerTimedAction(self, action):
        self.timedActions += [action]
    def unregisterTimedAction(self, action):
        self.timedActions.remove(action)
    def setLabel(self, label):
        self.label = label
    def setCurrentTargetLabel(self, label):
        self.curTargetLabel = label
    def setNextTargetLabel(self, label):
        self.nextTargetLabel = label
    def userMouseDown(self, pos):
        for button in self.buttons:
            button.checkClick(self,pos)
    def userMouseUp(self, pos):
        if len(self.buttons) > 0:
            for button in self.buttons:
                button.checkUnclick(self,pos)
        else:
            self.proceedAfterWait()
            
    def getCharacter(self,cid):
        return self.characters[cid]
    def getSpecialEffect(self,seId):
        return self.effects[seId]
            
    def setBackgroundImage(self,img):
        self.background = img
        self.backgroundX = (conf.display_width / 2) - (img.get_width() / 2)
        self.backgroundY = (conf.display_height / 2) - (img.get_height() / 2)
        self.updated = True
    def setTextboxBackground(self, frameType, boxId):
        #TODO what is second arg??
        self.activeTextBox.background = frameType
        self.activeTextBox.style = 'normal'
        self.activeTextBox.visible = True
        self.updated = True
    def setTextboxStyle(self, style):
        if style == 'top_close':
            self.textboxes[0].visible = False
        if style == 'bottom_close':
            self.textboxes[1].visible = False
        if style == 'close':
            self.activeTextBox.visible = False
        else:
            self.activeTextBox.style = style
            self.activeTextBox.visible = True
        self.updated = True
    def setTextboxArrow(self, boxId, time):
        #TODO time?
        txtboxId = abs(boxId - 1)
        if self.textboxes[txtboxId]:
            self.activeTextBox = self.textboxes[txtboxId]
        else:
            self.textboxes[txtboxId] = TextBox()
            self.activeTextBox = self.textboxes[txtboxId]
        self.updated = True
    def setSpeaker(self,name):
        if self.activeTextBox:
            self.activeTextBox.speaker = name
            self.activeTextBox.text = ''
        self.updated = True
    def addDialog(self,line):
        if self.activeTextBox:
            self.activeTextBox.text += line + '\n'
        self.updated = True
    def setTextSize(self,size):
        if self.activeTextBox:
            self.activeTextBox.setFontSize(size)
        self.updated = True
    def addCharacter(self,cid):
        self.characters[cid] = Character(cid)
        self.characterLayers[0] += [self.characters[cid]]
        self.updated = True
    def setCharacterLayer(self,cid,layer):
        c = self.characters[cid]
        self.characterLayers[c.layer].remove(c)
        c.setLayer(layer)
        while layer >= len(self.characterLayers):
            self.characterLayers += [[]]
        self.characterLayers[c.layer] += [c]
        self.updated = True
    def setCharacterMask(self,cid,mask):
        self.characters[cid].maskOn = (mask == 'on')
        self.updated = True
    def setCharacterFace(self,cid,face):
        self.characters[cid].changeFace(face)
        self.updated = True
    def setCharacterStoryPos(self,cid,pos):
        self.characters[cid].setStoryPos(pos)
        self.updated = True
    def startMovingCharacterToStoryPos(self,cid,pos):
        self.characters[cid].startMoveToStoryPos(pos)
        self.updated = True
    def finishMovingCharacter(self, cid):
        self.characters[cid].finishMoving()
        self.updated = True
    def setCharacterPos(self,cid,x,y):
        self.characters[cid].setPos((x,y))
        self.updated = True
    def setCharacterAlpha(self,cid,alpha):
        self.characters[cid].setAlpha(alpha)
        self.updated = True
    def setCharacterScale(self,cid,scale):
        self.characters[cid].setScale(scale)
        self.updated = True
    def setCharacterSpeaking(self, cid):
        for icid, char in self.characters.items():
            if icid == cid:
                char.speaking = True
            else:
                char.speaking = False
        self.updated = True
    def addSpecialEffect(self,seId,seArtName,initialAlpha=1.0):
        self.effects[seId] = SpecialEffect(seArtName, initialAlpha=initialAlpha)
        self.effectLayers[0] += [self.effects[seId]]
        self.updated = True
    def setSpecialEffectLayer(self,seId,layer):
        se = self.effects[seId]
        self.effectLayers[se.layer].remove(se)
        se.setLayer(layer)
        while layer >= len(self.effectLayers):
            self.effectLayers += [[]]
        self.effectLayers[se.layer] += [se]
        self.updated = True
    def setSpecialEffectAlpha(self,seId,alpha):
        self.effects[seId].setAlpha(alpha)
        self.updated = True
    def setSpecialEffectScale(self,seId,scale):
        self.effects[seId].setScale(scale)
        self.updated = True
    def setSpecialEffectPos(self,seId,x,y):
        self.effects[seId].setPos((x,y))
        self.updated = True
    def setBackgroundMusic(self,path,name,num):
        if name == 'stop':
            pygame.mixer.music.stop()
            return
        self.bgm = PotkRes.findMusicByCue(path,name)
        if self.bgm:
            pygame.mixer.music.load(self.bgm)
            pygame.mixer.music.play(loops=-1)
            if num:
                pygame.mixer.music.set_volume(num * conf.music_volume)
            else:
                pygame.mixer.music.set_volume(conf.music_volume)
        else:
            pygame.mixer.music.stop()
    def playVoiceLine(self,cid,cue):
        vo = PotkRes.getVoiceLine(cid,cue)
        if vo:
            vo.set_volume(conf.voice_volume)
            vo.play()
            self.lastVoice = vo
    def playSoundEffect(self,cue):
        se = PotkRes.getSoundEffect(cue)
        if se:
            se.set_volume(conf.sfx_volume)
            se.play()
    def createButton(self, position, text, onClick):
        ypos = 444 + position * 135
        self.buttons.extend((StoryButton((52,ypos,52+PotkRes.guiQuestionAnswerButton.get_width(),ypos+PotkRes.guiQuestionAnswerButton.get_height()), StoryButton.createTextButtonImage(PotkRes.guiQuestionAnswerButton, text), StoryButton.createTextButtonImage(PotkRes.guiQuestionAnswerButtonClicked, text), onClick),))
        self.updated = True
    def clearButtons(self):
        self.buttons = []
        self.updated = True
        
        
        
    def drawScene(self, display):
        # Draw background elements
        if self.background:
            display.blit(self.background, (self.backgroundX,self.backgroundY))
        # Draw characters
        for cid, char in self.characters.items():
            char.draw(display)
            if char.speaking and self.textboxes[1].visible and self.textboxes[1].background:
                arimg = PotkRes.guiTextBoxSpeaking if char.storyPos < 4 else PotkRes.guiTextBoxSpeakingFlip
                tarPosX = (display.get_width() / 6) * (char.storyPos)
                tarPosY = 850 - arimg.get_height()
                display.blit(arimg, (tarPosX,tarPosY))

        # Draw Special Effects
        for name, effect in self.effects.items():
            effect.draw(display)
        
        # Draw GUI elements
        display.blit(PotkRes.guiDiagBorderTop, ((conf.display_width / 2) - (PotkRes.guiDiagBorderTop.get_width() / 2),0))
        display.blit(PotkRes.guiDiagBorderBot, ((conf.display_width / 2) - (PotkRes.guiDiagBorderBot.get_width() / 2),conf.display_height - PotkRes.guiDiagBorderBot.get_height()))

        if self.textboxes[0].visible:
            if self.textboxes[0].background:
                tbackground = PotkRes.guiTextBoxBackgroundR
                tname = PotkRes.guiTextBoxNameP
                if self.textboxes[0].style == 'toge':
                    tframe = PotkRes.guiTextBoxSpikeFrameO
                elif self.textboxes[0].style == 'moya':
                    tframe = PotkRes.guiTextBoxThoughtFrameP
                else:
                    tframe = PotkRes.guiTextBoxStandardFrameY
            else:
                tbackground = PotkRes.guiTextBoxBackgroundG
                tname = PotkRes.guiTextBoxNameG
                if self.textboxes[0].style == 'toge':
                    tframe = PotkRes.guiTextBoxSpikeFrameY
                elif self.textboxes[0].style == 'moya':
                    tframe = PotkRes.guiTextBoxThoughtFrameG
                else:
                    tframe = PotkRes.guiTextBoxStandardFrameB
            display.blit(tbackground, (17,40))
            display.blit(tframe, (49,55))
            if self.textboxes[0].speaker:
                display.blit(tname, (31,30))
                theName = PotkRes.name_font.render(self.textboxes[0].speaker, True, colors.white)
                display.blit(theName, (95,25))
            textList = self.textboxes[0].text.split('\n')
            offset = 0
            for line in textList:
                theText = self.textboxes[0].myfont.render(line, True, colors.black)
                display.blit(theText, (95,67 + offset))
                offset += self.textboxes[0].textspacing
            if self.activeTextBox == self.textboxes[0]:
                display.blit(PotkRes.guiTextBoxNextArrow, (611,143))
        if self.textboxes[1].visible:
            if self.textboxes[1].background:
                tbackground = PotkRes.guiTextBoxBackgroundR
                tname = PotkRes.guiTextBoxNameP
                if self.textboxes[1].style == 'toge':
                    tframe = PotkRes.guiTextBoxSpikeFrameO
                elif self.textboxes[1].style == 'moya':
                    tframe = PotkRes.guiTextBoxThoughtFrameP
                else:
                    tframe = PotkRes.guiTextBoxStandardFrameY
            else:
                tbackground = PotkRes.guiTextBoxBackgroundG
                tname = PotkRes.guiTextBoxNameG
                if self.textboxes[1].style == 'toge':
                    tframe = PotkRes.guiTextBoxSpikeFrameY
                elif self.textboxes[1].style == 'moya':
                    tframe = PotkRes.guiTextBoxThoughtFrameG
                else:
                    tframe = PotkRes.guiTextBoxStandardFrameB
                
            display.blit(tbackground, (17,843))
            display.blit(tframe, (49,858))
            if self.textboxes[1].speaker:
                display.blit(tname, (31,833))
                theName = PotkRes.name_font.render(self.textboxes[1].speaker, True, colors.white)
                display.blit(theName, (95,828))
            textList = self.textboxes[1].text.split('\n')
            offset = 0
            for line in textList:
                theText = self.textboxes[1].myfont.render(line, True, colors.black)
                display.blit(theText, (95,870 + offset))
                offset += self.textboxes[1].textspacing
            if self.activeTextBox == self.textboxes[1]:
                display.blit(PotkRes.guiTextBoxNextArrow, (611,946))
        
        for button in self.buttons:
            button.draw(display)
    def update(self,display):
        # Update Timed actions
        for action in tuple(self.timedActions):
            #print('running action!')
            action.run(self)
        
        if self.timeLockout > 0:
            self.timeLockout -= 1
            if self.timeLockout <= 0:
                self.waiting = False
        else:
            while not self.waiting:
                self.script.runNextAction(self)

        if self.updated:
            display.fill(colors.white)
            self.drawScene(display)
            self.updated = False
        return self