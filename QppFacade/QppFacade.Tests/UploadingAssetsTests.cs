using System;
using System.IO;
using System.Text;
using AutoMapper;
using Castle.Windsor;
using IHS.Phoenix.QPP;
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
            _assetId = _sut.Upload(
                new FileAsset("asset.txt")
                    .With(PhoenixAttributes.CONTENT_TYPE, CustomContentTypes.Report)
                    .With(PhoenixAttributes.NAME, "acetic acid2")
                    .With(PhoenixAttributes.WORKFLOW, CustomWorkflows.Default)
                    .With(PhoenixAttributes.STATUS, CustomStatuses.Default)
                    .With(PhoenixAttributes.COLLECTION, CustomCollections.HomeTest)
                    .With(PhoenixAttributes.FILE_EXTENSION, "txt")
                    .With(PhoenixAttributes.ORIGINAL_FILENAME, "acetic acid2")
                    .With(PhoenixAttributes.DITA_TITLE, "acetic acid2")
                );

        private It should_upload_asset_properly = () =>
        {
            var file = _sut.GetFile<FileAsset>(_assetId);
            file.With(PhoenixAttributes.DITA_TITLE, "dupa");
            _sut.UpdateFile(file);
            _fileUpdated = _sut.GetFile<FileAsset>(_assetId);
            _fileUpdated.Get(PhoenixAttributes.DITA_TITLE).ShouldEqual("dupa");
        };

        private It should_map_things_nicely = () =>
        {
            var model = new DatabaseModel()
            {
                Id = _fileUpdated.Id,
                DitaTitle = (string) _fileUpdated.Get(PhoenixAttributes.DITA_TITLE),
                Name = (string) _fileUpdated.Get(PhoenixAttributes.NAME)
            };
            Mapper.CreateMap<FileAsset, DatabaseModel>()
                  .ForMember(dest => dest.DitaTitle, opts => opts.MapFrom(fileAsset => fileAsset.Get(PhoenixAttributes.DITA_TITLE)))
                  .ForMember(dest => dest.Name, opts => opts.MapFrom(fileAsset => fileAsset.Get(PhoenixAttributes.NAME)));

            var modelAutoMapper = Mapper.Map<FileAsset, DatabaseModel>(_fileUpdated);
            modelAutoMapper.Id.ShouldEqual(_fileUpdated.Id);
        };

        private Cleanup after = () => _sut.Delete(_assetId);

        private static Qpp _sut;
        private static long _assetId;
        private static WindsorContainer _container;
        private static FileAsset _fileUpdated;

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }

    public class DatabaseModel
    {
        public long Id { get; set; }
        public string DitaTitle { get; set; }
        public string Name { get; set; }
    }
}