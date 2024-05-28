import urllib.request

GAME = 'punk.gu3.jp'
ASSETS = 'production-punk.nativebase.gu3.jp'

class Enviroment:
    @property
    def DlcUrlBase(self):
        return "/{0}".format("2018")  # Application.unityVersion.Split('.')[0])

    def __init__(self, review_app_connect=False):
        self.label = "review" if review_app_connect else "production"

        self.ServerUrl = "https://{}.gu3.jp/".format(
            "review-game.punk" if review_app_connect else "punk"),
        self.NativeBaseUrl = "https://production-punk.nativebase.gu3.jp",
        self.LogCollectionUrl = "https://punk-logcollection-production.gu3.jp/punk.production.client",
        self.ClientErrorApi = "/api/v2/client/error",
        self.AuthApiPrefix = "/auth",
        self.PurchaseApiPrefix = "/api/v2/charge",
        self.DlcPath = "https://{0}.gu3.jp/dlc/production{1}/{2}/".format(
            "punk-dlc-review" if review_app_connect else "punk-dlc",
            self.DlcUrlBase,
            "android"
        )

    def download_asset(self, atype, id):
        url = f"{self.DlcPath}{atype}/{id}"
        # print(url)
        try:
            return urllib.request.urlopen(url, timeout=10).read()
        except:
            print(f"TimeOut: {url}")
            return self.download_asset(atype, id)