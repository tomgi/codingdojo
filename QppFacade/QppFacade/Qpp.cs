using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using com.quark.qpp.common.utility;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.asset.service.remote;
using com.quark.qpp.core.security.service.remote;

namespace QppFacade
{
    public class Qpp
    {
        private readonly SessionService _sessionService;
        private readonly AssetService _assetService;

        public Qpp(SessionService sessionService, AssetService assetService)
        {
            _sessionService = sessionService;
            _assetService = assetService;
        }

        public long Upload(Dictionary<object, object> dictionary)
        {
            return 0;
        }

        public Asset Get(long assetId)
        {
            return _assetService.getAsset(assetId);
        }

        public void LogIn()
        {
                _sessionService.logOn(
                    "Admin",
                    Encryptor.encrypt("Admin"),
                    Environment.MachineName,
                    Assembly.GetExecutingAssembly().FullName,
                    GetCurrentTimeZoneUtcOffset().ToString(@"\+h\:mm"));
        }
        private TimeSpan GetCurrentTimeZoneUtcOffset()
        {
            var zone = TimeZone.CurrentTimeZone;
            var offset = zone.GetUtcOffset(DateTime.Now);
            return offset;
        }
    }
}