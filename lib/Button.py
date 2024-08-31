import pygame

from lib.PotkRes import PotkRes
from lib.colors import colors

class Button:
    rect: tuple
    clicking: bool
    imageUnclicked: pygame.Surface
    imageClicked: pygame.Surface
    curImage: pygame.Surface
    clickSound: pygame.mixer.Sound

    def __init__(self, rect, imageUnclicked, imageClicked, onClick):
        self.clicking = False
        self.rect = rect
        self.imageUnclicked = imageUnclicked
        self.curImage = imageUnclicked
        self.imageClicked = imageClicked
        self.onClickAction = onClick
        self.clickSound = None
    def checkInside(self,pos):
        #print('Checking for click at: ' str(pos[0]) + ',' + str(pos[1]) + ' with bounding rect: ' + str(self.rect[0]) + ',' + str(self.rect[1]) + str(self.rect[2]) + ',' + str(self.rect[3]))
        return pos[0] >= self.rect[0] and pos[0] <= self.rect[2] and pos[1] >= self.rect[1] and pos[1] <= self.rect[3]
    def checkClick(self,env,pos):
        inside = self.checkInside(pos)
        if inside:
            self.curImage = self.imageClicked
            self.clicking = True
        return inside
    def checkUnclick(self,env,pos):
        inside = self.checkInside(pos)
        if inside and self.clicking:
            self.onClickAction.run(env)
            if self.clickSound:
                self.clickSound.play()
        self.curImage = self.imageUnclicked
        self.clicking = False
        return inside
        
    def draw(self,display):
        display.blit(self.curImage, (self.rect[0], self.rect[1]))
        
    @staticmethod
    def createTextButtonImage(surface, text):
        btnText = PotkRes.name_font.render(text, True, colors.white)
        newSur = surface.copy()
        newSur.blit(btnText, (surface.get_width() / 2 - btnText.get_width() / 2,surface.get_height() / 2 - btnText.get_height() / 2))
        return newSur