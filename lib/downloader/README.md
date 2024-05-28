# Phantom of the Kill Data-Mine

This git contains tools to download and extract assets of the game [Phantom of the Kill](https://dg-pk.fg-games.co.jp/).

The ownership of the assets belongs to [gumi](https://gu3.co.jp).

## Scripts

### 1_update_files.py

downloads and extracts new and missing files

### 2_convert_masterdata.py

converts the raw masterdata into a readable json

### 3_edit_n_copy_images.py

merges the RBA and Alpha images and saves them in texture

### Requirements

Python 3.6+

and the module UnityPy

```cmd
pip install UnityPy==1.7.10
```