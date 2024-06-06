import json
import os
import re
import lib.downloader.UnityPy as UnityPy
from lib.downloader.master_data_reader import MasterDataReader
from lib.downloader.paths import Paths, PATH

paths = Paths().__dict__
master_data_parser_json = os.path.join(PATH,*['lib','downloader','master_data_parser.json'])
cache = os.path.join(PATH,*['data','cache'])
masterdata = os.path.join(PATH,'masterdata')

if not os.path.exists(master_data_parser_json):
    input('couldn\'t find master_data_parser_json')
os.makedirs(masterdata, exist_ok=True)
# 1 - name, 2 - args
rePARSE = re.compile(r'return new ([^\n]+?)\(\)\s*\{\s*(.+?)\s*\};', re.S)
# 1 - name, 2 - func, 3 - arg
reARG = re.compile(r'\s*(.+?) = reader.(.+?)\((.*?)?\),')

master_data_parser = json.load(open(master_data_parser_json, 'r', encoding='utf8'))

def create_parser(correct_name):
    return master_data_parser[correct_name]


for fpath, item in paths['AssetBundle'].items():
    am = UnityPy.AssetsManager()
    if 'MasterData' in fpath:
        try:
            name = fpath.split('/')[-1]
            #print(name)
            # fetch raw data from unity asset
            #am.load(os.path.join(cache, item['FileName']))
            env = UnityPy.load(os.path.join(cache, item["FileName"]))
            data = env.container[fpath.lower()].read().script
            reader = MasterDataReader(data)

            # create parser
            pname = name.split('_',1)[0]
            parser = create_parser(pname)
            if not parser:
                raise EnvironmentError

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

            # save data
            name = name.split("_")
            if len(name) > 1:
                df = os.path.join(masterdata, *name[:-2])
                os.makedirs(df, exist_ok=True)
                dfp = os.path.join(df, f"{name[-1]}.json")
            else:
                dfp = os.path.join(masterdata, f"{name[0]}.json")

            open(dfp, 'wb').write(json.dumps(
                values, ensure_ascii=False, indent='\t').encode('utf8'))
            print("finished", fpath)
        except UnicodeError:
            print('unicode error', fpath)
        except FileNotFoundError:
            print('not found', fpath)
        except EnvironmentError:
            print('parse problem', fpath)
        except NotImplementedError:
            print('unity error', fpath)
