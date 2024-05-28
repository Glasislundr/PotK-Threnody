import json
import os
import re
import lib.downloader.UnityPy as UnityPy
from lib.downloader.master_data_reader import MasterDataReader
from lib.downloader.paths import Paths, PATH

paths = Paths().__dict__
assembly = os.path.join(PATH,*['lib','downloader','assembly','MasterDataTable'])
cache = os.path.join(PATH,*['data','cache'])
masterdata = os.path.join(PATH,'masterdata')

if not os.path.isdir(assembly):
    input('couldn\'t find assembly')
os.makedirs(masterdata, exist_ok=True)
# 1 - name, 2 - args
rePARSE = re.compile(r'return new ([^\n]+?)\(\)\s*\{\s*(.+?)\s*\};', re.S)
# 1 - name, 2 - func, 3 - arg
reARG = re.compile(r'\s*(.+?) = reader.(.+?)\((.*?)?\),')


def create_parser(correct_name, text):
    args = None
    for match in rePARSE.finditer(text):
        if match[1] == correct_name:
            args = match[2]
            break
    if not args:
        raise EnvironmentError
    parser = []  # key, func, arg
    for match in reARG.finditer(args+","):
        parser.append([
            match[1], match[2], (True if match[3] ==
                                 'true' else False) if match[3] else None
        ])
    return parser


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
            apath = os.path.join(assembly, f"{pname}.cs")
            adata = open(apath, 'rt', encoding='utf8').read()
            parser = create_parser(pname, adata)
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
