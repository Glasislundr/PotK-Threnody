import pygame
import os

from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.conf.conf import conf

class Character:
    cid: int
    utype: str
    resId: int
    image: pygame.Surface
    curImg: pygame.Surface
    maskOn: bool
    storyPos: int
    pos: []
    face: str
    faceImgs: dict
    speaking: bool
    alpha: float
    moving: bool
    layer: int

    def __init__(self, cid):
        self.cid = cid
        self.maskOn = True
        self.face = 'normal'
        self.speaking = False
        self.storyPos = -1
        self.alpha = 1.0
        self.baseScale = 1.0
        self.scale = 1.0
        self.moving = False
        self.layer = 0

        self.utype = PotkRes.udict[self.cid]['type']
        if self.utype == 'unit':
            self.resId = PotkRes.udict[self.cid]['resource_reference_unit_id_UnitUnit']
            self.image = PotkRes.getGameImage(PotkPaths.getUnitArtPath(self.resId))
            self.baseScale = PotkRes.udict[self.cid]['story_texture_scale']
            self.curImg = pygame.transform.smoothscale_by(self.image.copy(), self.baseScale)
            self.faceImgs = {}
            thepth = PotkPaths.getUnitFaceFolder(self.resId)
            if os.path.exists(thepth):
                for fp in os.listdir(thepth):
                    #print('Loading face: ' + fp)
                    self.faceImgs[fp.replace('.png', '')] = PotkRes.getGameImage(PotkPaths.getUnitFacePath(self.resId, fp))
        else:
            self.resId = PotkRes.udict[self.cid]['resource_reference_unit_id_MobUnit']
            self.image = PotkRes.getGameImage(PotkPaths.getMobArtPath(self.resId))
            self.baseScale = PotkRes.udict[self.cid]['story_texture_scale']
            self.curImg = pygame.transform.smoothscale_by(self.image.copy(), self.baseScale)
            self.faceImgs = {}
            thepth = PotkPaths.getMobFaceFolder(self.resId)
            if os.path.exists(thepth):
                for fp in os.listdir(thepth):
                    #print('Loading face: ' + fp)
                    self.faceImgs[fp.replace('.png', '')] = PotkRes.getGameImage(PotkPaths.getMobFacePath(self.resId, fp))
        self.pos = self.storyPosToUnitPos(self.storyPos)

    def storyPosToUnitPos(self, pos):
        tarPosX = (conf.display_width / 6) * (pos) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x']
        tarPosY = conf.display_height - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y']
        
        #tarPosX = (display.get_width() / 6) * (self.pos) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x']
        #tarPosY = display.get_height() - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y']
        return (tarPosX, tarPosY)
    def offsetToUnitPos(self, pos):
        tarPosX = (conf.display_width / 2) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x'] + pos[0]
        tarPosY = conf.display_height - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y'] - pos[1]
        return (tarPosX, tarPosY)
    
    def setLayer(self, layer):
        self.layer = layer
    def changeFace(self, face):
        if face:
            self.face = face
        self.rebuildCurImage()
    def setStoryPos(self, pos):
        self.storyPos = pos
        self.pos = self.storyPosToUnitPos(pos)
        #Update mask
    def startMoveToStoryPos(self, pos):
        self.storyPos = pos
        self.moving = True
        #Update mask
    def finishMoving(self):
        self.moving = False
    def setPos(self, pos):
        self.pos = pos
    def setAlpha(self, alpha):
        self.alpha = alpha
        self.rebuildCurImage()
    def setScale(self, scale):
        self.scale = scale
        modscale = scale / self.image.get_height() * self.curImg.get_height()
        self.curImg = pygame.transform.smoothscale_by(self.curImg, modscale)
        if not self.moving:
            self.pos = self.storyPosToUnitPos(self.storyPos)
    def rebuildCurImage(self):
        self.curImg = self.image.copy()
        if self.face != 'normal':
            self.curImg.blit(self.faceImgs[self.face], (PotkRes.udict[self.cid]['face_x'],self.curImg.get_height() - self.faceImgs[self.face].get_height() - PotkRes.udict[self.cid]['face_y']))
        self.curImg = pygame.transform.smoothscale_by(self.curImg, self.scale * self.baseScale)
        self.curImg.set_alpha(int(self.alpha * 255))
        
    def draw(self, display):
        
        #tarPosX = (display.get_width() / 6) * (self.storyPos) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x']
        #tarPosY = display.get_height() - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y']
        display.blit(self.curImg, self.pos)#(tarPosX, tarPosY))