﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using Castle.Core.Internal;
using com.quark.qpp.common.utility;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.asset.service.remote;
using com.quark.qpp.core.attribute.service.dto;
using com.quark.qpp.core.collection.service.dto;
using com.quark.qpp.core.collection.service.remote;
using com.quark.qpp.core.relation.service.constants;
using com.quark.qpp.core.security.service.remote;
using com.quark.qpp.FileTransferGateway;
using QppFacade.Models;

namespace QppFacade
{
    public class Qpp
    {
        private readonly SessionService _sessionService;
        private readonly AssetService _assetService;
        private readonly FileTransferGatewayConnector _fileTransferGatewayConnector;
        private readonly CollectionService _collectionService;

        public Qpp(
            SessionService sessionService,
            AssetService assetService,
            FileTransferGatewayConnector fileTransferGatewayConnector,
            CollectionService collectionService)
        {
            _sessionService = sessionService;
            _assetService = assetService;
            _fileTransferGatewayConnector = fileTransferGatewayConnector;
            _collectionService = collectionService;
        }

        public long Upload(FileAsset assetModel)
        {
            return CheckInNewAsset(assetModel, null);
        }

        public long Upload(Picture assetModel)
        {
            if (assetModel.IsChart)
            {
                var spreadsheetId = Upload(assetModel.Chart.AssetModel);

                return CheckInNewAsset(
                    assetModel,
                    new[]
                    {
                        new AssetRelation
                        {
                            childAssetId = spreadsheetId,
                            relationTypeId = assetModel.Chart.RelationType,
                            relationAttributes = assetModel.Chart.GimmeAttributeValues()
                        }
                    });
            }
            return Upload(assetModel as FileAsset);
        }

        public long UploadTopic(Topic assetModel)
        {
            var assetRelations = new List<AssetRelation>();
            foreach (var xmlReference in assetModel.Pictures)
            {
                var pictureId = Upload(xmlReference.AssetModel);
                assetRelations.Add(
                    new AssetRelation
                    {
                        childAssetId = pictureId,
                        relationTypeId = xmlReference.RelationType,
                        relationAttributes = xmlReference.GimmeAttributeValues()
                    });
            }

            foreach (var tableReference in assetModel.Tables)
            {
                var pictureId = Upload(tableReference.AssetModel);
                assetRelations.Add(
                    new AssetRelation
                    {
                        childAssetId = pictureId,
                        relationTypeId = tableReference.RelationType,
                        relationAttributes = tableReference.GimmeAttributeValues()
                    });
            }
            return CheckInNewAsset(assetModel, assetRelations.ToArray());
        }


        public long CheckInNewAsset(FileAsset assetModel, AssetRelation[] relations)
        {
            var asset = assetModel.ToAsset();

            long assetId = 0;

            var contextId = _assetService.createNewCheckInContextWithRelations(
                asset,
                false,
                relations);
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
                assetModel.WithContentDo(stream => _fileTransferGatewayConnector.Upload(contextId, stream));
            }
            finally
            {
                assetId = _assetService.closeContext(contextId).assetId;
                _assetService.unlockAsset(assetId);
            }

            return assetId;
        }

        public TAssetModel GetFile<TAssetModel>(long assetId) where TAssetModel : FileAsset
        {
            var asset = _assetService.getAsset(assetId);
            var assetModel = Activator.CreateInstance<TAssetModel>();
            assetModel.Id = asset.assetId;
            PushAttributes(asset.attributeValues, assetModel);
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

            asset.attributeValues = assetModel.GimmeAttributeValuesForUpdate();
            _assetService.updateAsset(asset);
            _assetService.unlockAsset(assetModel.Id);
        }

        public Topic GetTopicWithReferencedItems(long assetId)
        {
            var topic = GetFile<Topic>(assetId);
            var pictureRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {DefaultRelationTypes.XML_COMP_REFERENCE});
            foreach (var assetRelation in pictureRelations)
            {
                var picture = GetFile<Picture>(assetRelation.childAssetId);
                var chartRelations = _assetService.getChildAssetRelationsOfType(picture.Id, new long[] {1001});
                if (false == chartRelations.IsNullOrEmpty())
                {
                    var chartSpreadsheet = GetFile<FileAsset>(chartRelations.First().childAssetId);
                    picture.WithChartReference(new ChartSourceReference(chartSpreadsheet));
                }
                var pictureReference = new XmlReference<Picture>(picture);
                PushAttributes(assetRelation.relationAttributes, pictureReference);
                topic.AddPictureReference(pictureReference);
            }

            var tableRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {1000});
            foreach (var tableRelation in tableRelations)
            {
                var tableSpreadsheet = GetFile<FileAsset>(tableRelation.childAssetId);
                var tableReference = new TableSourceReference(tableSpreadsheet);
                PushAttributes(tableRelation.relationAttributes, tableReference);
                topic.AddTableReference(tableReference);
            }
            return topic;
        }

        private void PushAttributes(IEnumerable<AttributeValue> attributes, AttributeBag model)
        {
            foreach (var attributeValue in attributes)
            {
                var attrInfo = PhoenixAttributes.ById[attributeValue.attributeId];
                if (attrInfo == null)
                    continue;
                //attrInfo => IAttribute<string>, "FromAttributeValue"
//                model[attrInfo] = attrInfo.FromAttributeValue(attributeValue);
            }
        }

        public void Delete(Topic topic)
        {
            foreach (var pictureReference in topic.Pictures)
            {
                if (pictureReference.AssetModel.IsChart)
                    Delete(pictureReference.AssetModel.Chart.AssetModel.Id);
                Delete(pictureReference.AssetModel.Id);
            }
            foreach (var tableRerference in topic.Tables)
            {
                Delete(tableRerference.AssetModel.Id);
            }
            Delete(topic.Id);
        }

        public long UploadDitaMap(DitaMap ditaMap)
        {
            var assetRelations = new List<AssetRelation>();
            foreach (var topicReference in ditaMap.Topics)
            {
                var topicId = Upload(topicReference.AssetModel);
                assetRelations.Add(
                    new AssetRelation
                    {
                        childAssetId = topicId,
                        relationTypeId = topicReference.RelationType,
                        relationAttributes = topicReference.GimmeAttributeValues()
                    });
            }


            return CheckInNewAsset(ditaMap, assetRelations.ToArray());
        }

        public DitaMap GetDitaMapWithReferencedItems(long assetId)
        {
            var ditaMap = GetFile<DitaMap>(assetId);
            var topicRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {DefaultRelationTypes.XML_COMP_REFERENCE});
            foreach (var assetRelation in topicRelations)
            {
                var topic = GetFile<Topic>(assetRelation.childAssetId);

                var topicReference = new XmlReference<Topic>(topic);
                PushAttributes(assetRelation.relationAttributes, topicReference);
                ditaMap.AddTopicReference(topicReference);
            }

            return ditaMap;
        }

        public void Delete(DitaMap ditaMap)
        {
            foreach (var topicReference in ditaMap.Topics)
            {
                Delete(topicReference.AssetModel.Id);
            }

            Delete(ditaMap.Id);
        }
    }
}