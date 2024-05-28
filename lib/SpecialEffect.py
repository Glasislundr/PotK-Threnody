import pygame

from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.conf.conf import conf

halfScreenWidth = conf.display_width / 2
halfScreenHeight = conf.display_height / 2

def createScreenPos(pos, size):
    return (halfScreenWidth - (size[0] / 2 ) - pos[0], halfScreenHeight - (size[1] / 2 ) - pos[1])

class SpecialEffect:
    seName: str
    image: pygame.Surface
    curImg: pygame.Surface
    pos: list
    screenPos: list
    alpha: float
    layer: int

    def __init__(self, seName, initialAlpha=1.0):
        self.seName = seName
        self.pos = [0,0]
        self.alpha = initialAlpha
        self.scale = 1.0
        self.layer = 0

        self.image = PotkRes.getGameImage(PotkPaths.getSpecialEffectArtPath(self.seName)).convert_alpha()
        self.rebuildCurImage()
        self.screenPos = createScreenPos(self.pos, (self.curImg.get_width(), self.curImg.get_height()))

    def setPos(self, pos):
        self.pos = pos
        self.screenPos = createScreenPos(self.pos, (self.curImg.get_width(), self.curImg.get_height()))
    def setAlpha(self, alpha):
        self.alpha = alpha
        self.rebuildCurImage()
    def setScale(self, scale):
        self.scale = scale
        modscale = scale / self.image.get_height() * self.curImg.get_height()
        self.curImg = pygame.transform.smoothscale_by(self.curImg, modscale)
        self.screenPos = createScreenPos(self.pos, (self.curImg.get_width(), self.curImg.get_height()))
    def rebuildCurImage(self):
        self.curImg = self.image.copy()
        self.curImg = pygame.transform.smoothscale_by(self.curImg, self.scale)
        self.curImg.set_alpha(int(self.alpha * 255))
        
    def draw(self, display):
        display.blit(self.curImg, self.screenPos)