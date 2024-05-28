import pygame
from lib.Button import Button

class StoryButton(Button):
    
    def checkUnclick(self,env,pos):
        inside = self.checkInside(pos)
        if inside:
            self.onClickAction.run(env)
            if self.clickSound:
                self.clickSound.play()
        else:
            self.curImage = self.imageUnclicked
        self.clicking = False
        return inside