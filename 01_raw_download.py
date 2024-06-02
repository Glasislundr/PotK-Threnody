import os
import UnityPy
from lib.downloader.paths import Paths, PATH
from lib.downloader.api import Enviroment
from lib.downloader import AssetBatchConverter
from multiprocessing import Pool, cpu_count
import traceback

import logging
logger = logging.getLogger(__name__)
logging.basicConfig(filename='missingDownloads.log', level=logging.INFO)

SRC = os.path.join(PATH, *["Data", "cache"])
DST = os.path.join(PATH, "extracted")


def update_assetbundle(args):
    env, fpath, item = args
    try:
        sfp = os.path.join(SRC, item["FileName"])
        os.makedirs(os.path.dirname(sfp), exist_ok=True)

        data = env.download_asset("ab", item["FileName"])
        open(sfp, "wb").write(data)

        return fpath, 1
    except Exception as e:
        logger.error(traceback.format_exc())
        return fpath, 0

def update_streamasset(args):
    env, fpath, item = args
    try:
        sfp = os.path.join(SRC, item["FileName"])
        os.makedirs(os.path.dirname(sfp), exist_ok=True)

        data = env.download_asset("sa", item["FileName"])
        open(sfp, "wb").write(data)

        return fpath, 1
    except Exception as e:
        logger.error(traceback.format_exc())
        return fpath, 0

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
        if not os.path.exists(sfp):# or os.path.getsize(sfp) != item["FileSize"]:
            TODO.append((fpath, item))
            logger.error(f'AB Missing File: path: {fpath} fileName: {item["FileName"]}')

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
        # cut the crap, no need to save as "raw" file, since no extraction is required
        dfp = os.path.join(SRC, item["FileName"])
        if not os.path.exists(dfp):# or os.path.getsize(dfp) != item["FileSize"]:
            TODO.append((fpath, item))
            logger.error(f'SA Missing File: path: {fpath} fileName: {item["FileName"]}')

    for i, (fpath, ext) in enumerate(
        pool.imap_unordered(
            update_streamasset, ((env, fpath, item) for fpath, item in TODO)
        )
    ):
        print(f"{i}/{len(TODO)} - {fpath}" if ext else f"{i}/{len(TODO)} - Failed to download {fpath}")


if __name__ == "__main__":
    main()
