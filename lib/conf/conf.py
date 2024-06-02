import os
import json
import traceback

CONF_FILE_PATH = os.path.join(os.path.dirname(os.path.realpath(__file__)), 'conf.json')

class conf:
    #Default values, generally overwritten by the conf.json file
    display_width = 720
    display_height = 1136
    music_volume = 0.25
    voice_volume = 0.5
    sfx_volume = 0.5
    fps = 60
    
    home_unit = 100111
    home_bg = 3
    
def loadFromFile():
    with open(CONF_FILE_PATH, 'r') as f:
        loaded = json.load(f)
        for key, item in loaded.items():
            setattr(conf, key, item)
    
def saveToFile():
    toSave = {}
    for key, item in dict(conf.__dict__).items():
        if key[0] != '_':
            toSave[key] = item
    with open(CONF_FILE_PATH, 'w') as f:
        json.dump(toSave, f)

if os.path.exists(CONF_FILE_PATH):
    try:
        loadFromFile()
        saveToFile()
    except Exception as e:
        #print(traceback.format_exc())
        saveToFile()
else:
    saveToFile()