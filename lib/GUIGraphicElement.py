import pygame

from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.conf.conf import conf
import lib.StreamingAsset as StreamingAsset

def createScreenPos(pos, size, parentRect, centerVertical=True, centerHorizonal=True):
    return (((parentRect[0] + (parentRect[2] / 2) - (size[0] / 2 )) if centerHorizonal else parentRect[0]) + pos[0], ((parentRect[1] + (parentRect[3] / 2) - (size[1] / 2 )) if centerVertical else parentRect[1]) + pos[1])

class GUIGraphicElement:
    image: pygame.Surface
    pos: list
    centerVertical: bool
    centerHorizonal: bool
    parentRect: list
    screenPos: list
    children: list
    visible: bool

    def __init__(self, image, pos=[0,0], centerVertical=False, centerHorizonal=True, parentRect=[0,0,conf.display_width,conf.display_height]):
        self.pos = pos
        self.centerVertical = centerVertical
        self.centerHorizonal = centerHorizonal
        self.parentRect = parentRect
        self.visible = True
        self.image = image
        self.screenPos = createScreenPos(self.pos, (self.image.get_width(), self.image.get_height()), parentRect, centerVertical, centerHorizonal)

        self.children = []
        
    def getCenter(self):
        return (self.screenPos[0] + self.image.get_width()/2, self.screenPos[1] + self.image.get_height()/2)

    def getRect(self):
        return (self.screenPos[0], self.screenPos[1], self.image.get_width(), self.image.get_height())

    def setImage(self, image):
        self.image = image
        self.screenPos = createScreenPos(self.pos, (self.image.get_width(), self.image.get_height()), self.parentRect, self.centerVertical, self.centerHorizonal)

    def setVisible(self, visible):
        self.visible = visible
    
    def setPos(self, pos):
        self.pos = pos
        self.screenPos = createScreenPos(self.pos, (self.image.get_width(), self.image.get_height()), self.parentRect, self.centerVertical, self.centerHorizonal)

    def addChild(self, child):
        self.children += [child]

    def removeChild(self, child):
        self.children.remove(child)
        
    def draw(self, display):
        if self.visible:
            display.blit(self.image, self.screenPos)
            for child in self.children:
                child.draw(display)