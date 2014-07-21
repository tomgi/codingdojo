using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using IHS.Phoenix.QPP;

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

        public long Upload(AssetModel assetModel)
        {
            return CheckInNewAsset(assetModel, null);
        }

        public long UploadPicture(AssetModel assetModel)
        {
            var chartRelation = assetModel.RelationsOfType(CustomRelations.ChartSource).SingleOrDefault();
            if (chartRelation != null)
            {
                var spreadsheetId = Upload(chartRelation.AssetModel);

                return CheckInNewAsset(
                    assetModel,
                    new[]
                    {
                        new AssetRelation
                        {
                            childAssetId = spreadsheetId,
                            relationTypeId = chartRelation.RelationType,
                            relationAttributes = chartRelation.GimmeAttributeValues()
                        }
                    });
            }
            return Upload(assetModel as AssetModel);
        }

        public long UploadTopic(AssetModel assetModel)
        {
            var assetRelations = new List<AssetRelation>();
            foreach (var xmlReference in assetModel.RelationsOfType(DefaultRelationTypes.XML_COMP_REFERENCE))
            {
                var pictureId = UploadPicture(xmlReference.AssetModel);
                assetRelations.Add(
                    new AssetRelation
                    {
                        childAssetId = pictureId,
                        relationTypeId = xmlReference.RelationType,
                        relationAttributes = xmlReference.GimmeAttributeValues()
                    });
            }

            foreach (var tableReference in assetModel.RelationsOfType(CustomRelations.TableSource))
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

        public long UploadAssetFromFile(AssetModel topic, FileInfo file)
        {
            var assetRelations = new List<AssetRelation>();
            foreach (var xmlReference in topic.RelationsOfType(DefaultRelationTypes.XML_COMP_REFERENCE))
            {
                var pictureId = UploadPicture(xmlReference.AssetModel);
                assetRelations.Add(
                    new AssetRelation
                    {
                        childAssetId = pictureId,
                        relationTypeId = xmlReference.RelationType,
                        relationAttributes = xmlReference.GimmeAttributeValues()
                    });
            }

            foreach (var tableReference in topic.RelationsOfType(CustomRelations.TableSource))
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

            Func<Stream> readStreamFromFile = 
                () => File.Open(file.FullName, FileMode.Open, FileAccess.Read);

            return CheckInNewAsset(topic.ToAsset(), assetRelations.ToArray(), readStreamFromFile);
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


        public long CheckInNewAsset(AssetModel assetModel, AssetRelation[] relations)
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

        public TAssetModel GetFile<TAssetModel>(long assetId) where TAssetModel : AssetModel
        {
            var asset = _assetService.getAsset(assetId);
            var assetModel = Activator.CreateInstance<TAssetModel>();
            assetModel.Id = asset.assetId;
            PushAttributes(asset.attributeValues, assetModel);
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

        public void UpdateFile(AssetModel assetModel)
        {
            _assetService.lockAsset(assetModel.Id);
            var asset = new Asset
            {
                assetId = assetModel.Id
            };

            asset.attributeValues = assetModel.GimmeModifiableAttributeValues();
            _assetService.updateAsset(asset);
            _assetService.unlockAsset(assetModel.Id);
        }

        public AssetModel GetTopicWithReferencedItems(long assetId)
        {
            var topic = GetFile<AssetModel>(assetId);
            var pictureRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {DefaultRelationTypes.XML_COMP_REFERENCE});
            foreach (var assetRelation in pictureRelations)
            {
                var picture = GetFile<AssetModel>(assetRelation.childAssetId);
                var chartRelations = _assetService.getChildAssetRelationsOfType(picture.Id, new long[] {1001});
                if (false == chartRelations.IsNullOrEmpty())
                {
                    var chartSpreadsheet = GetFile<AssetModel>(chartRelations.First().childAssetId);
                    picture.WithRelation(Relation.To(chartSpreadsheet).OfType(CustomRelations.ChartSource));
                }
                var pictureReference = Relation.To(picture).OfType(DefaultRelationTypes.XML_COMP_REFERENCE)
                                                .With(assetRelation.relationAttributes);
                topic.WithRelation(pictureReference);
                AssetModel temp = topic;
            }

            var tableRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {1000});
            foreach (var tableRelation in tableRelations)
            {
                var tableSpreadsheet = GetFile<AssetModel>(tableRelation.childAssetId);
                var tableReference = Relation.To(tableSpreadsheet).OfType(CustomRelations.TableSource);
                PushAttributes(tableRelation.relationAttributes, tableReference);
                topic.WithRelation(tableReference);
                AssetModel temp = topic;
            }
            return topic;
        }

        private void PushAttributes(IEnumerable<AttributeValue> attributes, AttributeBag model)
        {
            foreach (var attributeValue in attributes)
            {
                model.Set(attributeValue);
            }
        }

        public void Delete(AssetModel topic)
        {
            foreach (var pictureReference in topic.RelationsOfType(DefaultRelationTypes.XML_COMP_REFERENCE))
            {
                var chartRelation =
                    pictureReference.AssetModel.RelationsOfType(CustomRelations.ChartSource)
                                    .SingleOrDefault();
                if (chartRelation != null)
                    Delete(chartRelation.AssetModel.Id);
                Delete(pictureReference.AssetModel.Id);
            }
            foreach (var tableRerference in topic.RelationsOfType(CustomRelations.TableSource))
            {
                Delete(tableRerference.AssetModel.Id);
            }
            Delete(topic.Id);
        }

        public long UploadDitaMap(AssetModel ditaMap)
        {
            var assetRelations = new List<AssetRelation>();
            foreach (var topicReference in (IEnumerable<Relation>) ditaMap.Relations)
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

        public AssetModel GetDitaMapWithReferencedItems(long assetId)
        {
            var ditaMap = GetFile<AssetModel>(assetId);
            var topicRelations = _assetService.getChildAssetRelationsOfType(assetId, new long[] {DefaultRelationTypes.XML_COMP_REFERENCE});
            foreach (var assetRelation in topicRelations)
            {
                var topic = GetFile<AssetModel>(assetRelation.childAssetId);

                var topicReference = Relation.To(topic).OfType(DefaultRelationTypes.XML_COMP_REFERENCE)
                                              .With(assetRelation.relationAttributes);
                ditaMap.WithRelation(topicReference);
                AssetModel temp = ditaMap;
            }

            return ditaMap;
        }

        public void DeleteDitaMap(AssetModel ditaMap)
        {
            foreach (var topicReference in (IEnumerable<Relation>) ditaMap.Relations)
            {
                Delete(topicReference.AssetModel.Id);
            }

            Delete(ditaMap.Id);
        }
    }
}