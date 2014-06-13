﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using com.quark.qpp.common.dto;
using com.quark.qpp.common.utility;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.asset.service.remote;
using com.quark.qpp.core.attribute.service.dto;
using com.quark.qpp.core.collection.service.dto;
using com.quark.qpp.core.collection.service.remote;
using com.quark.qpp.core.security.service.remote;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;
using System.Data;
using com.quark.qpp.FileTransferGateway;

namespace QppFacade
{
    public class Qpp
    {
        private readonly SessionService _sessionService;
        private readonly AssetService _assetService;
        private readonly QppAttributes _qppAttributes;
        private readonly FileTransferGatewayConnector _fileTransferGatewayConnector;
        private readonly CollectionService _collectionService;

        public Qpp(SessionService sessionService, AssetService assetService, QppAttributes qppAttributes, FileTransferGatewayConnector fileTransferGatewayConnector, CollectionService collectionService)
        {
            _sessionService = sessionService;
            _assetService = assetService;
            _qppAttributes = qppAttributes;
            _fileTransferGatewayConnector = fileTransferGatewayConnector;
            _collectionService = collectionService;
        }

        public long Upload(FileAsset assetModel)
        {
            var asset = new Asset
            {
                attributeValues = assetModel.Attributes.Select(item => _qppAttributes.Find(item.Key).CreateValue(item.Value)).ToArray()
            };
            return CheckInNewAsset(asset, assetModel.Content);
        }


        public long CheckInNewAsset(Asset asset, Stream fileStream)
        {
            long assetId = 0;

            var contextId = _assetService.createNewCheckInContextWithRelations(
                asset,
                false,
                new AssetRelation[]{});
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
                _fileTransferGatewayConnector.Upload(contextId, fileStream);
            }
            finally
            {
                assetId = _assetService.closeContext(contextId).assetId;
                _assetService.unlockAsset(assetId);
            }
            return assetId;
        }

        public FileAsset GetFile(long assetId)
        {
            var asset = _assetService.getAsset(assetId);
            var assetModel = new FileAsset()
            {
                Id = asset.assetId
            };

            foreach (var attributeValue in asset.attributeValues)
            {
                var attrInfo = _qppAttributes.Find(attributeValue.attributeId);
                if (attrInfo == null)
                    continue;

                assetModel.With(attrInfo.Id, attrInfo.GetValue(attributeValue));
            }


            return assetModel;
        }

        public void UpdateDitaMap(DitaMap ditaMap)
        {
            //_assetService.getAsset(assetId);
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

        public long GetCollectionIdByPath(string collectionPath)
        {
            var collections = collectionPath.Split('/');
            return FindCollectionRecursive(_collectionService, null, collections, 0);
        }

        private static long FindCollectionRecursive(CollectionService collectionService, CollectionInfo collectionInfo, string[] collections, int i)
        {
            var childCollections = i == 0
                ? collectionService.getAccessibleTopLevelCollections()
                : collectionService.getAccessibleImmediateChildCollections(collectionInfo.id);

            var collection = childCollections == null ? null : childCollections.SingleOrDefault(c => c.name == collections[i]);

            if (collection == null)
                throw new InvalidOperationException(String.Format("Collection {0} not found", String.Join("//", collections.Take(i + 1))));

            if (i == collections.Length - 1)
                return collection.id;

            return FindCollectionRecursive(collectionService, collection, collections, ++i);
        }

        public void Delete(long assetId)
        {
            _assetService.lockAsset(assetId);
            _assetService.deleteAsset(assetId);
        }

        public void UpdateFile(FileAsset assetModel)
        {
            _assetService.lockAsset(assetModel.Id);
            var asset = new Asset
            {
                assetId = assetModel.Id
            };
            var attributeValues = new List<AttributeValue>();
            foreach (var attribute in assetModel.Attributes)
            {
                var attrInfo = _qppAttributes.Find(attribute.Key);
                if (attrInfo == null)
                    continue;
                if (false == attrInfo.CanBeUpdated())
                    continue;
                var attributeValue = attrInfo.CreateValue(attribute.Value);
                if (attributeValue != null)
                    attributeValues.Add(attributeValue);
            }
            asset.attributeValues = attributeValues.ToArray();
            _assetService.updateAsset(asset);
            _assetService.unlockAsset(assetModel.Id);
        }
    }
}