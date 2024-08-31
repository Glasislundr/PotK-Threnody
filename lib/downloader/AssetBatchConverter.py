import os
import lib.downloader.UnityPy as UnityPy
from collections import Counter
import zipfile
import json
import logging
logger = logging.getLogger(__name__)
logging.basicConfig(filename='converter.log', level=logging.INFO)


TYPES = [
    # Images
    'ClassIDType.Sprite',
    'ClassIDType.Texture2D',
    # Text (filish)
    'ClassIDType.TextAsset',
    'ClassIDType.Shader',
    'ClassIDType.MonoBehaviour',
    'ClassIDType.Mesh',
    # Font
    'ClassIDType.Font',
    # Audio
    'ClassIDType.AudioClip',
    # Renderers
    'ClassIDType.Material',
    'ClassIDType.Renderer',
    'ClassIDType.MeshRenderer',
    'ClassIDType.SkinnedMeshRenderer',
]

ROOT = os.path.dirname(os.path.realpath(__file__))

# destination folder
DST = os.path.join(ROOT, 'extracted')

def extract_assets(src):
    # load source
    env = UnityPy.load(src)

    # iterate over assets
    for asset in env.assets:
        # assets without container / internal path will be ignored for now
        if not asset.container:
            continue
        # filter objects and put Texture2Ds at the end of the list
        objs = sorted((obj for obj in asset.get_objects(
        ) if obj.type.name in TYPES), key=lambda x: 1 if str(x.type) == "ClassIDType.Texture2D" else 0)
        cobjs = sorted(((key, obj) for key, obj in asset.container.items(
        ) if obj.type.name in TYPES), key=lambda x: 1 if str(x[1].type) == "ClassIDType.Texture2D" else 0)
        # check which mode we will have to use
        num_cont = len(cobjs)
        num_objs = len(objs)

        # check if container contains all important assets, if yes, just ignore the container
        if num_objs <= num_cont * 2:
            for asset_path, obj in cobjs:
                fp = os.path.join(DST, *asset_path.split('/'))
                export_obj(obj, fp)

        # otherwise use the container to generate a path for the normal objects
        else:
            extracted = []
            # find the most common path
            occurence_count = Counter(os.path.splitext(asset_path)[
                                      0] for asset_path in asset.container.keys())
            local_path = os.path.join(
                DST, *occurence_count.most_common(1)[0][0].split('/'))

            for obj in objs:
                if obj.path_id not in extracted:
                    extracted.extend(export_obj(
                        obj, local_path, append_name=True))


def export_obj(obj, fp: str, append_name: bool = False) -> list:
    if str(obj.type) not in TYPES:
        logger.error('!!   Invalid object type: ' + str(obj.type) + ' for ' + fp)
        return []

    data = obj.read()
    if append_name:
        if hasattr(data, 'name') and data.name:
            fp = os.path.join(fp, data.name)
        else:
            theType = str(obj.type)[12:]
            fp = os.path.join(fp, theType + '_' + str(obj.path_id))

    fp, extension = os.path.splitext(fp)
    os.makedirs(os.path.dirname(fp), exist_ok=True)
    #print('!!   Exporting object of type: ' + str(obj.type) + ' to ' + fp)

    # streamlineable types
    export = None
    if str(obj.type) == 'ClassIDType.TextAsset':
        if not extension:
            extension = '.txt'
        export = data.script

    elif str(obj.type) == "ClassIDType.Font":
        if data.m_FontData:
            extension = ".ttf"
            if data.m_FontData[0:4] == b"OTTO":
                extension = ".otf"
            export = data.m_FontData
        else:
            return [obj.path_id]

    elif str(obj.type) == "ClassIDType.Mesh":
        extension = ".obj"
        export = data.export().encode("utf8")

    elif str(obj.type) == "ClassIDType.Shader":
        extension = ".txt"
        export = data.export().encode("utf8")

    elif str(obj.type) == "ClassIDType.MonoBehaviour":
        # The data structure of MonoBehaviours is custom
        # and is stored as nodes
        # If this structure doesn't exist,
        # it might still help to at least save the binary data,
        # which can then be inspected in detail.
        if obj.serialized_type.nodes:
            extension = ".json"
            export = json.dumps(
                obj.read_typetree(),
                indent=4,
                ensure_ascii=False
            ).encode("utf8")
        else:
            extension = ".bin"
            export = data.raw_data
            
    elif str(obj.type) == "ClassIDType.Material":
        extension = ".mtl"
        export = UnityPy.export.MeshRendererExporter.export_material(data).encode("utf8")

    if export:
        with open(f"{fp}{extension}", "wb") as f:
            f.write(export)

    # non-streamlineable types
    if str(obj.type) == "ClassIDType.Sprite":
        data.image.save(f"{fp}.png")

        return [obj.path_id, data.m_RD.texture.path_id, getattr(data.m_RD.alphaTexture, 'path_id', None)]

    elif str(obj.type) == "ClassIDType.Texture2D":
        #print('!!!  Attempting to save Texture2D object')
        if not os.path.exists(fp) and data.m_Width:
            # textures can have size 0.....
            data.image.save(f"{fp}.png")
            #print('!!!  Wrote? ' + f"{fp}.png")

    elif str(obj.type) == "ClassIDType.AudioClip":
        samples = data.samples
        if len(samples) == 0:
            pass
        elif len(samples) == 1:
            with open(f"{fp}.wav", "wb") as f:
                f.write(list(data.samples.values())[0])
        else:
            os.makedirs(fp, exist_ok=True)
            for name, clip_data in samples.items():
                with open(os.path.join(fp, f"{name}.wav"), "wb") as f:
                    f.write(clip_data)

    elif str(obj.type) == "ClassIDType.Renderer" or str(obj.type) == "ClassIDType.MeshRenderer" or str(obj.type) == "ClassIDType.SkinnedMeshRenderer":
        data.export(fp)

    return [obj.path_id]
