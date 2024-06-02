import os
import lib.downloader.UnityPy as UnityPy
from lib.downloader.paths import Paths, PATH
from lib.downloader.api import Enviroment
from lib.downloader import AssetBatchConverter
from multiprocessing import Pool, cpu_count
import shutil
import logging
logger = logging.getLogger(__name__)
logging.basicConfig(filename='converter.log', level=logging.INFO)

SRC = os.path.join(PATH, *["data", "cache"])
DST = os.path.join(PATH, "extracted")


def update_assetbundle(args):
    env, fpath, item = args
    try:
        sfp = os.path.join(SRC, item["FileName"])

        data = open(sfp, "rb").read()

        dfp = os.path.join(DST, *fpath.split("/"))
        uenv = UnityPy.load(data)
        objects = uenv.objects

        logger.debug('Attempting to extract '+fpath +' with ' + str(len(objects))+' objects found')
        extracted = []
        for obj in objects:
            if obj.path_id not in extracted:
                extracted.extend(AssetBatchConverter.export_obj(obj, dfp, len(objects) > 2))
        return fpath, len(extracted)
    except Exception as e:
        print(e)
        return fpath, 0


def update_streamingasset(args):
    env, fpath, item = args
    
    sfp = os.path.join(SRC, item["FileName"])
    dfp = os.path.join(DST, "StreamingAssets", *fpath.split("/")) + item["Extension"]
    os.makedirs(os.path.dirname(dfp), exist_ok=True)
    shutil.copyfile(sfp,dfp)
    
    return fpath, 1


def main():
    AssetBatchConverter.DST = DST

    paths = Paths()
    env = Enviroment(True)

    os.makedirs(SRC, exist_ok=True)
    os.makedirs(DST, exist_ok=True)
    # AssetBundle
    TODO = []
    for fpath, item in paths["AssetBundle"].items():
        sfp = os.path.join(SRC, item["FileName"])
        if not os.path.exists(sfp):
            logger.error('ERROR: Source file not found - ' + sfp + '\n')
        elif os.path.getsize(sfp) != item["FileSize"]:
            logger.warning('ERROR: Source file not expected size - ' + sfp + ' (expected: ' + str(item["FileSize"]) + ', found:' + str(os.path.getsize(sfp)) + ')\n')
            TODO.append((fpath, item))
        else:
            TODO.append((fpath, item))

    pool = Pool(processes=cpu_count())
    for i, (fpath, ext) in enumerate(
        pool.imap_unordered(
            update_assetbundle, ((env, fpath, item) for fpath, item in TODO)
        )
    ):
        print(f"{i}/{len(TODO)} - {fpath} - extracted {ext} assets")

    # StreamingAssets
    TODO = []
    for fpath, item in paths["StreamingAssets"].items():
        sfp = os.path.join(SRC, item["FileName"])
        if not os.path.exists(sfp):
            logger.error('ERROR: Source file not found - ' + sfp + '\n')
        elif os.path.getsize(sfp) != item["FileSize"]:
            logger.warning('ERROR: Source file not expected size - ' + sfp + ' (expected: ' + str(item["FileSize"]) + ', found:' + str(os.path.getsize(sfp)) + ')\n')
            TODO.append((fpath, item))
        else:
            TODO.append((fpath, item))

    for i, (fpath, ext) in enumerate(
        pool.imap_unordered(
            update_streamingasset, ((env, fpath, item) for fpath, item in TODO)
        )
    ):
        print(f"{i}/{len(TODO)} - {fpath}")
if __name__ == "__main__":
    main()
