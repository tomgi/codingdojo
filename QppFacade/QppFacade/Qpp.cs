using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using com.quark.qpp.common.utility;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.asset.service.remote;
using com.quark.qpp.core.security.service.remote;
using com.quark.qpp.FileTransferGateway;

namespace QppFacade
{
    public class Qpp
    {
        private readonly SessionService _sessionService;
        private readonly AssetService _assetService;
        private readonly FileTransferGatewayConnector _fileTransferGatewayConnector;

        public Qpp(
            SessionService sessionService,
            AssetService assetService,
            FileTransferGatewayConnector fileTransferGatewayConnector)
        {
            _sessionService = sessionService;
            _assetService = assetService;
            _fileTransferGatewayConnector = fileTransferGatewayConnector;
        }

        public long UploadAssetModelFromDirectory(AssetModel asset, DirectoryInfo directory)
        {
            var assetRelations = new List<AssetRelation>();

            foreach (var relation in asset.Relations)
            {
                relation.AssetModel.Id = 
                    UploadAssetModelFromDirectory(relation.AssetModel, directory);
                assetRelations.Add(relation.ToAssetRelation(asset.Id));
            }

            var assetPath = Path.Combine(directory.FullName, asset.Get(PhoenixAttributes.ORIGINAL_FILENAME));

            Func<Stream> readStreamFromFile = 
                () => File.Open(assetPath, FileMode.Open, FileAccess.Read);

            return CheckInNewAsset(asset.ToAsset(), assetRelations.ToArray(), readStreamFromFile);
        }

        private long CheckInNewAsset(Asset asset, AssetRelation[] relations, Func<Stream> openStream)
        {
            var contextId = _assetService.createNewCheckInContextWithRelations(
               asset,
               false,
               relations);

            long assetId;
            try
            {
                var properties = new PropertyCollection
                {
                    {streamingProperties.HTTPStreamingPort, 61400},
                    {streamingProperties.HTTPCookieContainer, new CookieContainer()}
                };

                _fileTransferGatewayConnector.setStreamingTransportType(
                    StreamingTransportType.http);
                _fileTransferGatewayConnector.setProperties(
                    StreamingTransportType.http,
                    properties);
                using (var stream = openStream())
                {
                    _fileTransferGatewayConnector.Upload(contextId, stream);
                }
            }
            finally
            {
                assetId = _assetService.closeContext(contextId).assetId;
                _assetService.unlockAsset(assetId);
            }

            return assetId;
        }

        public AssetModel GetAssetModel(long assetId)
        {
            var compositeAsset = _assetService.getAssetWithRelations(assetId);

            var assetModel = new AssetModel { Id = compositeAsset.asset.assetId };

            foreach (var relation in compositeAsset.relatedAssets ?? Enumerable.Empty<RelatedAsset>())
            {
                var childAsset = GetAssetModel(relation.assetRelation.childAssetId);

                assetModel.WithRelation(
                    Relation
                    .To(childAsset)
                    .OfType(relation.assetRelation.relationTypeId));
            }

            foreach (var attributeValue in compositeAsset.asset.attributeValues)
            {
                assetModel.Set(attributeValue);
            }

            return assetModel;
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

        public void UpdateAssetModel(AssetModel assetModel)
        {
            foreach (var relation in assetModel.Relations)
            {
                UpdateAssetModel(relation.AssetModel);
            }

            _assetService.lockAsset(assetModel.Id);
            _assetService.setAttributeValues(assetModel.Id, assetModel.GimmeModifiableAttributeValues());
            _assetService.unlockAsset(assetModel.Id);
        }

        public void DeleteAssetModel(AssetModel assetModel)
        {
            foreach (var relation in assetModel.Relations)
            {
                DeleteAssetModel(relation.AssetModel);
            }

            _assetService.lockAsset(assetModel.Id);
            _assetService.deleteAsset(assetModel.Id);
        }
    }
}