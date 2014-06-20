using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Permissions;
using System.Web.Services.Protocols;
using Castle.DynamicProxy.Generators;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using com.quark.qpp;
using com.quark.qpp.common.constants;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.asset.service.remote;
using com.quark.qpp.core.attribute.service.remote;
using com.quark.qpp.core.collection.service.remote;
using com.quark.qpp.core.content.service.remote;
using com.quark.qpp.core.publishing.service.remote;
using com.quark.qpp.core.query.service.remote;
using com.quark.qpp.core.relation.service.remote;
using com.quark.qpp.core.security.service.remote;
using com.quark.qpp.core.workflow.service.remote;
using com.quark.qpp.FileTransferGateway;
using QppFacade;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace IHS.Phoenix.QPP.Facade.SoapFacade
{
    public static class QppHelper
    {
        private static readonly Dictionary<Type, string> ServicesTypesToNamesMap = new Dictionary<Type, string>
        {
            {typeof (SessionService), ServiceNames.SESSION_SERVICE},
            {typeof (QueryService), ServiceNames.QUERY_SERVICE},
            {typeof (PublishingService), ServiceNames.PUBLISHING_SERVICE},
            {typeof (AssetService), ServiceNames.ASSET_SERVICE},
            {typeof (AttributeService), ServiceNames.ATTRIBUTE_SERVICE},
            {typeof (AttributeDomainService), ServiceNames.ATTRIBUTE_DOMAIN_SERVICE},
            {typeof (ContentStructureService), ServiceNames.CONTENT_STRUCTURE_SERVICE},
            {typeof (TrusteeService), ServiceNames.TRUSTEE_SERVICE},
            {typeof (CollectionService), ServiceNames.COLLECTION_SERVICE},
            {typeof (RelationService), ServiceNames.RELATION_SERVICE},
            {typeof (WorkflowService), ServiceNames.WORKFLOW_SERVICE}
        };

        public static TService GetService<TService>(this ServiceFactory serviceFactory) where TService : QppSOAPClientProtocol
        {
            var type = typeof (TService);
            string serviceName;
            if (ServicesTypesToNamesMap.TryGetValue(type, out serviceName) == false)
            {
                throw new InvalidOperationException(
                    String.Format("No service mapped to type {0}", type.Name));
            }

            return serviceFactory.GetService(serviceName) as TService;
        }
    }

    public class QppFacadeWindsorInstaller : IWindsorInstaller
    {
        private string _qppHost;


        public QppFacadeWindsorInstaller(string qppHost)
        {
            _qppHost = qppHost;
        }

        public QppFacadeWindsorInstaller()
        {
            _qppHost = "localhost";
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            AttributesToAvoidReplicating.Add(typeof (PermissionSetAttribute));
            var serviceFactory = new ServiceFactory(_qppHost, 61400, false, new CookieContainer());

            Func<int, IEnumerable<DomainValue>> resolveDomain = domainId => container.Resolve<AttributeDomainService>().getDomainValues(domainId);
            Func<string, long> resolveCollection = collectionId => container.Resolve<Qpp>().GetCollectionIdByPath(collectionId);
            Func<IEnumerable<Attribute>> getAttributes = () => container.Resolve<AttributeService>().getAllAttributes();


            container.Register(
                Component.For<SessionService>().UsingFactoryMethod(serviceFactory.GetService<SessionService>),
                Component.For<PublishingService>().UsingFactoryMethod(serviceFactory.GetService<PublishingService>),
                Component.For<AssetService>().UsingFactoryMethod(serviceFactory.GetService<AssetService>),
                Component.For<QueryService>().UsingFactoryMethod(serviceFactory.GetService<QueryService>),
                Component.For<AttributeService>().UsingFactoryMethod(serviceFactory.GetService<AttributeService>),
                Component.For<AttributeDomainService>().UsingFactoryMethod(serviceFactory.GetService<AttributeDomainService>),
                Component.For<ContentStructureService>().UsingFactoryMethod(serviceFactory.GetService<ContentStructureService>),
                Component.For<TrusteeService>().UsingFactoryMethod(serviceFactory.GetService<TrusteeService>),
                Component.For<CollectionService>().UsingFactoryMethod(serviceFactory.GetService<CollectionService>),
                Component.For<RelationService>().UsingFactoryMethod(serviceFactory.GetService<RelationService>),
                Component.For<WorkflowService>().UsingFactoryMethod(serviceFactory.GetService<WorkflowService>),
                Component.For<Func<IEnumerable<Attribute>>>().Instance(getAttributes),
                Component.For<Func<int, IEnumerable<DomainValue>>>().Instance(resolveDomain),
                Component.For<Func<string, long>>().Instance(resolveCollection),
                Component.For<FileTransferGatewayConnector>().LifestyleSingleton().DependsOn(Dependency.OnValue<string>(_qppHost)),
                Component.For<Qpp>().ImplementedBy<Qpp>()
                );
            container.Resolve<Qpp>().LogIn();
            PhoenixAttributes.Init(getAttributes, resolveDomain, resolveCollection);
            PhoenixValuesInitializer.Initialize(s => (SoapHttpClientProtocol) serviceFactory.GetService(s));
        }
    }
}