using System;
using UnityEngine;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class YCDataManager : ADataManager {

            private static string ADVERTISING_ID = "ADVERTISING_ID";
            private static string ADSSHOW = "ADSSHOW";
            private static string LANGUAGE = "LANGUAGE";

            private static string GDPR_ADS = "GDPR_ADS";
            private static string GDPR_ANALYTICS = "GDPR_ANALYTICS";
            private static string INTERSTITIALS_NB = "INTERSTITIALS_NB";
            private static string REWARDEDS_NB = "REWARDEDS_NB";
            private static string TIMESTAMP = "TIMESTAMP";
            private static string LEVEL_PLAYED = "LEVEL_PLAYED";

            private int[] interstitialsWatchedMilestones = new int[] { 10, 15, 20 };
            private int[] rewardedsWatchedMilestones = new int[] { 2, 5, 7 };
            private int[] levelPlayedMilestones = new int[] { 1, 5, 10, 20, 50 };

            private void Awake() {
                if (this.HasKey(TIMESTAMP) == false) {
                    this.SetInt(TIMESTAMP, this.GetTimestamp());
                }
            }

            public int GetTimestamp() {
                return (int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000);
            }

            public int GetDiffTimestamp() {
                return this.GetTimestamp() - this.GetInt(TIMESTAMP);
            }

            private void CheckMmpEvent(int value, int[] milestones, string endString) {
                for (int i = 0; i < milestones.Length; i++) {
                    if (value == milestones[i]) {
                        YCManager.instance.mmpManager.SendEvent(value.ToString() + endString);
                        return;
                    }
                }
            }

            //ADVERTISING ID
            public string GetAdvertisingId() {
                return this.GetString(ADVERTISING_ID, "");
            }
            public void SetAdvertisingId(string id) {
                this.SetString(ADVERTISING_ID, id);
            }

            // ADS
            public bool GetAdsShow() {
                return this.GetInt(ADSSHOW, 1) == 1;
            }
            public void BuyAdsShow() {
                this.SetInt(ADSSHOW, 0);
            }

            // LANGUAGE
            public void SetLanguage(string lang) {
                this.SetString(LANGUAGE, lang);
            }
            public string GetLanguage() {
                return this.GetString(LANGUAGE, "EN");
            }
            public bool HasLanguage() {
                return this.HasKey(LANGUAGE);
            }

            // GDPR
            public void SetGdprAds(bool consent) {
                this.SetBool(GDPR_ADS, consent);
            }
            public bool GetGdprAds() {
                return this.GetBool(GDPR_ADS, true);
            }

            public void SetGdprAnalytics(bool analytics) {
                this.SetBool(GDPR_ANALYTICS, analytics);
            }
            public bool GetGdprAnalytics() {
                return this.GetBool(GDPR_ANALYTICS, true);
            }

            // NB INTERSTITIALS
            public int GetInterstitialsNb() {
                return this.GetInt(INTERSTITIALS_NB, 0);
            }
            public void IncrementInterstitialsNb() {
                int newValue = this.GetInterstitialsNb() + 1;
                this.SetInt(INTERSTITIALS_NB, newValue);
                this.CheckMmpEvent(newValue, this.interstitialsWatchedMilestones, "_interstitials_watched");
            }

            // NB REWARDEDS
            public int GetRewardedsNb() {
                return this.GetInt(REWARDEDS_NB, 0);
            }
            public void IncrementRewardedsNb() {
                int newValue = this.GetRewardedsNb() + 1;
                this.SetInt(REWARDEDS_NB, newValue);
                this.CheckMmpEvent(newValue, this.rewardedsWatchedMilestones, "_rewardeds_watched");
            }

            // LEVEL PLAYED
            public int GetLevelPlayed() {
                return this.GetInt(LEVEL_PLAYED, 0);
            }
            public void IncrementLevelPlayed() {
                int newValue = this.GetLevelPlayed() + 1;
                this.SetInt(LEVEL_PLAYED, newValue);
                this.CheckMmpEvent(newValue, this.levelPlayedMilestones, "_level_played");
            }

        }
    }
}
