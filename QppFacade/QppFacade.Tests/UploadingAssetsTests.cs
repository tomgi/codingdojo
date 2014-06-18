using System;
using System.IO;
using System.Text;
using AutoMapper;
using Castle.Windsor;
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
            _assetId = _sut.Upload(
                new FileAsset("asset.txt")
                    .With(PhoenixAttributes.CONTENT_TYPE.WithValue("Chemical Report"))
                    .With(PhoenixAttributes.NAME.WithValue("acetic acid2"))
                    .With(PhoenixAttributes.WORKFLOW.WithValue("Default Workflow"))
                    .With(PhoenixAttributes.STATUS.WithValue("Default"))
                    .With(PhoenixAttributes.COLLECTION.WithValue("Home/Test"))
                    .With(PhoenixAttributes.FILE_EXTENSION.WithValue("txt"))
                    .With(PhoenixAttributes.ORIGINAL_FILENAME.WithValue("acetic acid2"))
                    .With(PhoenixAttributes.DITA_TITLE.WithValue("acetic acid2"))
                );

        private It should_upload_asset_properly = () =>
        {
            var file = _sut.GetFile<FileAsset>(_assetId);
            file.With(PhoenixAttributes.DITA_TITLE.WithValue("dupa"));
            _sut.UpdateFile(file);
            _fileUpdated = _sut.GetFile<FileAsset>(_assetId);
            _fileUpdated[PhoenixAttributes.DITA_TITLE].ShouldEqual("dupa");
        };

        private It should_map_things_nicely = () =>
        {
            var model = new DatabaseModel()
            {
                Id = _fileUpdated.Id,
                DitaTitle = (string) _fileUpdated[PhoenixAttributes.DITA_TITLE],
                Name = (string) _fileUpdated[PhoenixAttributes.NAME]
            };
            Mapper.CreateMap<FileAsset, DatabaseModel>()
                  .ForMember(dest => dest.DitaTitle, opts => opts.MapFrom(fileAsset => fileAsset[PhoenixAttributes.DITA_TITLE]))
                  .ForMember(dest => dest.Name, opts => opts.MapFrom(fileAsset => fileAsset[PhoenixAttributes.NAME]));

            var modelAutoMapper = Mapper.Map<FileAsset,DatabaseModel>(_fileUpdated);
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
