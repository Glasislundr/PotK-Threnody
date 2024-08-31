import pygame

from lib.colors import colors
from lib.PotkPaths import PotkPaths
from lib.PotkRes import PotkRes
from lib.conf.conf import conf
from lib.GUIGraphicElement import GUIGraphicElement, createScreenPos

class NamePlate(GUIGraphicElement):
    speaker: str
    speakerImg: pygame.Surface
    def __init__(self, pos=None, centerVertical=False, centerHorizonal=False, parentRect=[0,0,conf.display_width,conf.display_height]):
        if pos is None:
            pos = [PotkRes.guiTextBoxNameP.get_width() * 0.05,0 - PotkRes.guiTextBoxNameP.get_height() / 4]
        super(NamePlate, self).__init__(PotkRes.guiTextBoxNameP, pos, centerVertical, centerHorizonal, parentRect)
        self.speaker = ''
        self.speakerPos = self.screenPos
    def setStyle(self, backgroundStyle):
        if backgroundStyle:
            self.setImage(PotkRes.guiTextBoxNameP)
        else:
            self.setImage(PotkRes.guiTextBoxNameG)
    def setSpeaker(self, speaker):
        self.speaker = speaker
        self.speakerImg = PotkRes.name_font.render(self.speaker, True, colors.white)
        self.speakerPos = createScreenPos((0,0), (self.speakerImg.get_width(), self.speakerImg.get_height()), self.getRect(), True, True)
    def draw(self, display):
        if self.visible and self.speaker:
            display.blit(self.image, self.screenPos)
            display.blit(self.speakerImg, self.speakerPos)
            for child in self.children:
                child.draw(display)

class TextFrame(GUIGraphicElement):
    text: str
    textspacing: int
    style: str
    textLines: list
    myfont: pygame.font.Font
    active: bool
    arrowPos: tuple
    def __init__(self, pos=[0,0], centerVertical=True, centerHorizonal=True, parentRect=[0,0,conf.display_width,conf.display_height]):
        super(TextFrame, self).__init__(PotkRes.guiTextBoxStandardFrameY, pos, centerVertical, centerHorizonal, parentRect)
        self.text = ''
        self.textspacing = 32
        self.style = 'normal'
        self.textLines = []
        self.myfont = pygame.font.Font(PotkPaths.font_path, 24)
        self.active = False
        self.arrowPos = (self.screenPos[0] + self.image.get_width() * 0.9, self.screenPos[1] + self.image.get_width() * 0.7)
    def setActive(self, active):
        self.active = active
    def setText(self, text):
        self.text = text
        self.renderText()
    def addTextLine(self, newText):
        self.text += '\n' + newText
        self.renderText()
    def setFontSize(self, size):
        self.textspacing = size * 4 / 3
        self.myfont = pygame.font.Font(PotkPaths.font_path, size)
        self.renderText()
    def setStyle(self, style, backgroundStyle):
        self.style = style
        if backgroundStyle:
            if self.style == 'toge':
                self.setImage(PotkRes.guiTextBoxSpikeFrameO)
            elif self.style == 'moya':
                self.setImage(PotkRes.guiTextBoxThoughtFrameP)
            else:
                self.setImage(PotkRes.guiTextBoxStandardFrameY)
        else:
            if self.style == 'toge':
                self.setImage(PotkRes.guiTextBoxSpikeFrameY)
            elif self.style == 'moya':
                self.setImage(PotkRes.guiTextBoxThoughtFrameG)
            else:
                self.setImage(PotkRes.guiTextBoxStandardFrameB)
    def renderText(self):
        textList = self.text.split('\n')
        self.textLines = []
        offset = 0 - (self.textspacing * 2)
        for line in textList:
            theText = self.myfont.render(line, True, colors.black)
            rect = self.getRect()
            tpos = (rect[2] * 0.11, offset)
            self.textLines += [(createScreenPos(tpos, (theText.get_width(), theText.get_height()), rect, self.centerVertical, False), theText)]
            offset += self.textspacing
    def draw(self, display):
        if self.visible:
            display.blit(self.image, self.screenPos)
            for line in self.textLines:
                display.blit(line[1], line[0])
            for child in self.children:
                child.draw(display)
            if self.active:
                display.blit(PotkRes.guiTextBoxNextArrow, self.arrowPos)
            

class TextBox(GUIGraphicElement):
    background: int
    style: str
    
    namePlate: NamePlate
    textFrame: TextFrame
    def __init__(self, pos=[0,0], centerVertical=False, centerHorizonal=True, parentRect=[0,0,conf.display_width,conf.display_height]):
        super(TextBox, self).__init__(PotkRes.guiTextBoxBackgroundR, pos, centerVertical, centerHorizonal, parentRect)
        self.setVisible(False)
        self.background = 1
        self.style = 'normal'
        
        ownRect = self.getRect()
        self.textFrame = TextFrame(parentRect=ownRect)
        self.addChild(self.textFrame)
        self.namePlate = NamePlate(parentRect=ownRect)
        self.addChild(self.namePlate)
        
    def setFontSize(self, size):
        self.textFrame.setFontSize(size)
    def setText(self, text):
        self.textFrame.setText(text)
    def addTextLine(self, text):
        self.textFrame.addTextLine(text)
    def setActive(self, active):
        self.textFrame.setActive(active)
    def setSpeaker(self, speaker):
        self.namePlate.setSpeaker(speaker)
    def setBackground(self, background):
        self.background = background
        if background:
            self.setImage(PotkRes.guiTextBoxBackgroundR)
        else:
            self.setImage(PotkRes.guiTextBoxBackgroundG)
        self.namePlate.setStyle(background)
        self.textFrame.setStyle(self.style, background)
    def setStyle(self, style):
        self.style = style
        self.textFrame.setStyle(style, self.background)
        