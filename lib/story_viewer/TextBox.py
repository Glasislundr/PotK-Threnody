import pygame

from lib.PotkPaths import PotkPaths

class TextBox:
    visible: bool
    speaker: str
    text: str
    background: int
    style: str
    myfont: pygame.font.Font
    textspacing: int
    def __init__(self):
        self.visible = False
        self.speaker = ''
        self.text = ''
        self.background = 1
        self.style = 'normal'
        self.textspacing = 32
        self.myfont = pygame.font.Font(PotkPaths.font_path, 24)
    def setFontSize(self, size):
        self.textspacing = size * 4 / 3
        self.myfont = pygame.font.Font(PotkPaths.font_path, size)
