using Google.Analytics.SDK.Core;
using Google.Analytics.SDK.Core.Extensions;
using Google.Analytics.SDK.Core.Hits;
using Google.Analytics.SDK.Core.Hits.MobileHits;
using Google.Analytics.SDK.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace szzminer.Tools
{
    public class GoogleAnalyiticsSDK
    {
        public const string DataApplicationWebProplerty = "UA-175572898-2";
        public const string Applicationname = "松之宅矿工1.2.3";
        public const string ApplicationVersion = "1.2.3";
        public const string ApplicationId = "1.2.3";
        public static ITracker tracker = TrackerBuilder.BuildMobileTracker(DataApplicationWebProplerty, Applicationname, ApplicationVersion, ApplicationId);


        public void ScreenViewAsync(string info)
        {
            Task.Run(() => ScreenViewHitHelper.SendAsync(tracker, info));
        }

        public void EventAsync(string act, string things)
        {
            Task.Run(() => EventHitHelper.SendAsync(tracker, Applicationname, act, things));
        }

        public void ExceptionAsync(string info)
        {
            Task.Run(() => ExceptionHitHelper.SendAsync(tracker, info, true));
        }

        public void SocialHitAsync(string eventcatagory, string action, string label)
        {
            Task.Run(() => EventHitHelper.SendAsync(tracker, eventcatagory, action, label));
        }
    }
    class TimingHitHelper
    {
        public static async Task<bool> SendAsync(ITracker tracker, string userTimeingCatagory, string userTimeingVariableName, int userTimingTime, string ip)
        {
            var hit = new TimingHit(userTimeingCatagory, userTimeingVariableName, userTimingTime)
            {
                DataSource = "app",
                UserAgentOverride = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 Ip/" + ip
            };

            // create the hit request.
            var request = (HitRequestBase)tracker.CreateHitRequest(hit);

            // Run a debug check to ensure its valid.
            var debugResponse = await request.ExecuteDebugAsync();
            if (!((DebugResult)debugResponse).IsValid()) return false;

            // Send hit.
            var collectRequest = await request.ExecuteCollectAsync();
            Console.Write(collectRequest.RawResponse);

            return true;
        }
    }
    class SocialHitHelper
    {
        public static async Task<bool> SendAsync(ITracker tracker, string socialNetwork, string socialAction, string socialActionTarget, string ip)
        {
            SocialHit hit = new SocialHit(socialNetwork, socialAction, socialActionTarget)
            {
                DataSource = "app",
                UserAgentOverride = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 Ip/" + ip
            };

            // create the hit request.
            var request = (HitRequestBase)tracker.CreateHitRequest(hit);

            // Run a debug check to ensure its valid.
            var debugResponse = await request.ExecuteDebugAsync();
            if (!((DebugResult)debugResponse).IsValid())
                return false;

            // Send hit.
            var collectRequest = await request.ExecuteCollectAsync();
            Console.Write(collectRequest.RawResponse);

            return true;
        }
    }
    class ScreenViewHitHelper
    {

        public static async Task<bool> SendAsync(ITracker tracker, string screenName)
        {
            var hit = new ScreenViewHit(screenName)
            {
                DataSource = "app",
                UserAgentOverride = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36"
            };

            // create the hit request.
            var request = (HitRequestBase)tracker.CreateHitRequest(hit);

            // Run a debug check to ensure its valid.
            var debugResponse = await request.ExecuteDebugAsync();
            if (!((DebugResult)debugResponse).IsValid()) return false;

            // Send hit.
            var collectRequest = await request.ExecuteCollectAsync();
            Console.Write(collectRequest.RawResponse);

            return true;
        }
    }
    class ExceptionHitHelper
    {
        public static async Task<bool> SendAsync(ITracker tracker, string exceptionDescription, bool fatal)
        {
            var hit = new ExceptionHit(exceptionDescription, fatal)
            {
                DataSource = "app",
                UserAgentOverride = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36"
            };

            // create the hit request.
            var request = (HitRequestBase)tracker.CreateHitRequest(hit);

            // Run a debug check to ensure its valid.
            var debugResponse = await request.ExecuteDebugAsync();
            if (!((DebugResult)debugResponse).IsValid()) return false;

            // Send hit.
            var collectRequest = await request.ExecuteCollectAsync();
            Console.Write(collectRequest.RawResponse);

            return true;
        }
    }
    class EventHitHelper
    {
        public static async Task<bool> SendAsync(ITracker tracker, string eventCatagory, string eventAction, string eventLabel)
        {
            var hit = new EventHit(eventCatagory, eventAction, eventLabel)
            {
                DataSource = "app",
                UserAgentOverride = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36"
            };

            // create the hit request.
            var request = (HitRequestBase)tracker.CreateHitRequest(hit);

            // Run a debug check to ensure its valid.
            var debugResponse = await request.ExecuteDebugAsync();
            if (!((DebugResult)debugResponse).IsValid())
                return false;

            // Send hit.
            var collectRequest = await request.ExecuteCollectAsync();
            Console.Write(collectRequest.RawResponse);

            return true;
        }
    }
}
