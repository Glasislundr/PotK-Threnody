# PotK - Threnody
This application is an attempt to recreate the story portions of the game [Phantom of the Kill](https://dg-pk.fg-games.co.jp/) now that the game has ended service. No game assets are distributed as part of this repository, but tools are provided to download and extract the game assets (so long as the servers remain up).

This git contains tools to download and extract assets of the game [Phantom of the Kill](https://dg-pk.fg-games.co.jp/).

The ownership of the assets belongs to [gumi](https://gu3.co.jp).

## Version Support
This project was developed on Python 3.10.11 with pygame 2.5.2. It may function on other versions.

If you need to install pygame, you can do so with a simple pip command:
```cmd
pip install pygame
```
NOTE: Pygame is only used for the Story Viewer (script 05), and is not needed for the extraction tools.

## Installation and Usage
The numbered python scripts in the root folder outline the steps.

Do note that in total the downloaded and extracted data will take roughly 72 GB of disk space.

- 01_raw_download.py - Downloads all of the raw game files into the cache folder.
  - Note that the cache data takes roughly 10GB of disk space.
  - This may stop working soon, when the servers are shut off.
  - This can be skipped if the cache folder can be provided from another source, such as DMM.
- 02_convert_masterdata.py - Extracts the MasterData json files.
- 03_extract_all_assets.py - Extracts all the game files from the raw Unity data files.
  - The extracted data will take roughly 30GB of disk space.
  - This is limited to the types of data that UnityPy can extract. Animations, for example, are limited or missing.
- 04_convert_all_sound_files.bat - Uses vgmstream to extract the sound files to .wav format.
  - The converted sound files will add another 30GB of disk space used.
  - In order to run this step, you will need to download Vgmstream-cli from https://github.com/vgmstream/vgmstream/releases/ and extract that package into the extracted\StreamingAssets\android folder that will be created by step 3.
  - You can skip this step and still proceed to step 5.
- 05_read_story.py - A story emulator for the PotK story files.
  - Currently only loads files by ID, set by a variable in the script itself.
  - This will load without the converted sound files, it will simply be silent.

## License
MIT

## Included Projects
This project includes code from other projects.

https://gitlab.com/K0lb3/phantom-of-the-kill - As the basis for the file download and extraction process. Major modifications have been made to extract more files

https://github.com/K0lb3/UnityPy - For parsing Unity data files; included in the repo as a fix was needed to export Renderer objects

https://github.com/kohos/CriTools - For parsing ACB/AWB audio files. The originally Javascript files have been translated into Python.

## Additional thanks to:
https://github.com/vgmstream/vgmstream - For providing a greater understanding of the ACB/AWB files through the helpful comments in the related parts of their code. Also thanks for providing the HCA encryption key for PotK.

https://github.com/Youjose/PyCriCodecs - For providing another reference for the ACB/AWB files.
