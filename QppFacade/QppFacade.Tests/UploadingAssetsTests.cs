using System;
using System.Collections.Generic;
using Castle.Windsor;
using com.quark.qpp.core.asset.service.dto;
using com.quark.qpp.core.attribute.service.constants;
using IHS.Phoenix.QPP.Facade.SoapFacade;
using Machine.Specifications;

namespace QppFacade.Tests
{
    [Subject(typeof (Type), "Category")]
    public class when_uploading_asset_on_the_platform
    {
        private Establish context = () =>
        {
            _container = new WindsorContainer();
            _container.Install(new QppFacadeWindsorInstaller());

            _sut = _container.Resolve<Qpp>();
        };


        private Because of = () =>
            _assetId = _sut.Upload(new Dictionary<object,object>
            {
                {DefaultAttributes.CONTENT_TYPE,"Chemical Report"},
                {DefaultAttributes.NAME,"acetic acid"},
                {"Brand (Taxonomy)","5"},
                {"Domains (Taxonomy)","20"},
                {"Chemical Product and Market (Taxonomy)","1,2,3"},
            });

        private It should_upload_asset_properly = () =>
        {
            Asset asset = _sut.Get(_assetId);
        };

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
    }
}
