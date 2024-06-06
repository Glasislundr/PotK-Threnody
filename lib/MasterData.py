import os
import json

from lib.PotkPaths import PotkPaths
from lib.downloader.paths import PATH
import lib.downloader.UnityPy as UnityPy
from lib.downloader.master_data_reader import MasterDataReader

cache = os.path.join(PATH,*['data','cache'])

master_data_parser = json.load(open(os.path.join(PATH,*['lib','downloader','master_data_parser.json']), 'r', encoding='utf8'))

def extractMasterData(fpath):
    name = fpath.split('/')[-1]
    item = PotkPaths.paths['AssetBundle'][fpath]
    env = UnityPy.load(os.path.join(cache, item["FileName"]))
    data = env.container[fpath.lower()].read().script
    reader = MasterDataReader(data)
    pname = name.split('_',1)[0]
    parser = master_data_parser[pname]
    
    # parse data
    values = []
    while reader.buf.tell() < len(data):
        try:
            values.append({
                key: getattr(reader, func)(
                    arg) if arg else getattr(reader, func)()
                for key, func, arg in parser
            })
        except Exception as e:
            print(e)
    return values

def getMasterData(name):
    ext_path = PotkPaths.getExtractedMasterDataFilePath(name)
    if os.path.exists(ext_path):
        with open(ext_path, 'r', encoding="utf8") as file:
            return json.load(file)
    else:
        fpath = PotkPaths.getInternalMasterDataPath(name)
        return extractMasterData(fpath)

def getScript(name):
    ext_path = PotkPaths.getExtractedScriptFilePath(name)
    if os.path.exists(ext_path):
        with open(ext_path, 'r', encoding="utf8") as file:
            return json.load(file)
    else:
        fpath = PotkPaths.getInternalScriptPath(name)
        return extractMasterData(fpath)
        