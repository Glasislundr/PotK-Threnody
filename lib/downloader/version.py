import http.client
import urllib.request
import json
import io
import re
from zipfile import ZipFile
import os

HOST = "punk.gu3.jp"
HEADERS = {
    "Content-Type": "application/json; charset=UTF-8",
    "X-GUMI-DEVICE-OS": "android",
}
APK_NAME = "jp.co.gu3.punk"


def update_app_ver():
    # 1. download apk via qooapp
    buf = download_QooApp_apk(APK_NAME)

    zf = ZipFile(buf)
    data = zf.open("assets/bin/Data/Managed/Metadata/global-metadata.dat", "r").read()
    zf.close()
    buf.close()

    app_ver = re.findall(b"\d{4}\.\d{2}\.\d{2}-\d{2}\.\d{2}\.\d{2}", data)[0].decode()

    return app_ver


def update_dlc_ver(app_ver):
    # 2. fetch dlc_version from the server
    # 1. get access token
    def request(host, path, body={}, headers=HEADERS):
        req = urllib.request.Request(
            f"https://{host}/{path.lstrip('/')}",
            headers=headers,
            data=json.dumps(body).encode("utf8"),
        )
        # con = http.client.HTTPSConnection(host)
        # con.connect()
        # con.request("POST", path, json.dumps(body), headers)
        # res_body = con.getresponse().read()
        # con.close()
        res_body = urllib.request.urlopen(req).read()
        return json.loads(res_body)

    # 1. get access token
    token = request(
        HOST,
        "/auth/v2/accesstoken",
        {
            "secret_key": "06ff26bc-c3c7-4579-a7a5-b0632e58f23b",
            "device_id": "f66bb6ff-1e6d-4702-9737-1f7d237eec5a",
            "idfv": "03df982c-7fdb-3614-a64d-b928655f0ba3",
            "idfa": "94b8acde-697e-4631-82ce-262a724f9095",
        },
    )["access_token"]
    HEADERS["Authorization"] = f"gauth {token}"
    # 2. get dlc version
    ret = request(HOST, "/api/v2/player/boot/release", {"application_version": app_ver})
    dlc_version = ret["dlc_latest_version"]

    return dlc_version


def download_QooApp_apk(apk):
    con = http.client.HTTPSConnection("api.qoo-app.com")
    con.connect()
    add = "?" + "&".join(
        [
            "supported_abis=x86%2Carmeabi-v7a%2Carmeabi",
            "device=aosp",
            "device_model=SM-A805N",
            "sdk_version=22",
            "version_code=297",
            "version_name=7.10.12",
            "os=android+5.1.1",
        ]
    )
    con.request("GET", f"/v6/apps/{apk}/download{add}")
    res = con.getresponse()
    con.close()

    download_url = res.headers["Location"]
    return download_with_bar(download_url)


def download_with_bar(url):
    resp = urllib.request.urlopen(url)
    length = resp.getheader("content-length")
    if length:
        length = int(length)
        blocksize = max(4096, length // 100)
    else:
        blocksize = 1000000  # just made something up

    buf = io.BytesIO()
    size = 0
    while True:
        buf1 = resp.read(blocksize)
        if not buf1:
            break
        buf.write(buf1)
        size += len(buf1)
        if length:
            print(" {:.2f}\r{}".format(size / length, url), end="")
    print()
    buf.seek(0)
    return buf
