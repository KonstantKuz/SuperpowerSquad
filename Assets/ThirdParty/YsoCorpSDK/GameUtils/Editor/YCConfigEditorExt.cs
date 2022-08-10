using System;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace YsoCorp.GameUtils
{
    public static class YCConfigEditorExt
    {
        public static void InitMax(this YCConfig ycConfig)
        {
            AppLovinSettings.Instance.AdMobIosAppId = ycConfig.AdMobIosAppId;
            AppLovinSettings.Instance.AdMobAndroidAppId = ycConfig.AdMobAndroidAppId;
            EditorUtility.SetDirty(AppLovinSettings.Instance);
            AssetDatabase.SaveAssets();
        }

        public static void EditorImportConfig(this YCConfig ycConfig)
        {
            if (ycConfig.gameYcId != "")
            {
                string url =
                    RequestManager.GetUrlEmptyStatic(
                        "games/setting/" + ycConfig.gameYcId + "/" + Application.identifier, true);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "Get";
                request.ContentType = "application/json";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            YCConfig.InfosData infos = Newtonsoft.Json.JsonConvert
                                .DeserializeObject<YCConfig.DataData>(reader.ReadToEnd()).data;
                            if (infos.name != "")
                            {
                                ycConfig.Name = infos.name;
                                ycConfig.FbAppId = infos.facebook_app_id;
                                ycConfig.appleId = infos.ios_key;

                                ycConfig.AdMobAndroidAppId = infos.admob_android_app_id;
                                ycConfig.AdMobIosAppId = infos.admob_ios_app_id;

                                ycConfig.IosInterstitial = infos.applovin.adunits.ios.interstitial;
                                ycConfig.IosRewarded = infos.applovin.adunits.ios.rewarded;
                                ycConfig.IosBanner = infos.applovin.adunits.ios.banner;
                                ycConfig.AndroidInterstitial = infos.applovin.adunits.android.interstitial;
                                ycConfig.AndroidRewarded = infos.applovin.adunits.android.rewarded;
                                ycConfig.AndroidBanner = infos.applovin.adunits.android.banner;
                                // MMPs
                                ycConfig.MmpAdjust = infos.mmps.adjust.active;
                                ycConfig.MmpAdjustAppToken =
                                    infos.mmps.adjust.active ? infos.mmps.adjust.app_token : "";
                                ycConfig.MmpTenjin = infos.mmps.tenjin.active;
                                ycConfig.DisplayImportConfigDialog(true, infos);
                                ycConfig.InitFacebook();
                                ycConfig.InitMax();
                                ycConfig.InitFirebase(infos);
                            }
                            else
                            {
                                ycConfig.DisplayImportConfigDialog(false,
                                    "Impossible to import config. Check your Game Yc Id or your connection.");
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    ycConfig.DisplayImportConfigDialog(false,
                        "Impossible to import config. Check your Game Yc Id or your connection.");
                }
            }
            else
            {
                ycConfig.DisplayImportConfigDialog(false, "Please enter Game Yc Id.");
            }
        }
    }
}