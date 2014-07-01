using System;
using System.Collections.Generic;
using System.Reflection;
using com.quark.qpp.common.dto;
using com.quark.qpp.core.attribute.service.constants;
using IHS.Phoenix.QPP;
using IHS.Phoenix.QPP.Facade.SoapFacade.QppAttributes;
using Attribute = com.quark.qpp.core.attribute.service.dto.Attribute;

namespace QppFacade
{
     public class AttributePlaceholder<T> : IAttribute<T>
    {
        public long Id { get; set; }
        public string Name { get; set; }

         public int Type { get; private set; }
    }


    public static partial class PhoenixAttributes
    {
        public static Dictionary<string, object> ByName { get; private set; }

        public static Dictionary<long, object> ById { get; private set; }

        private static bool Initialized = false;

        public static void Init(
            Func<IEnumerable<Attribute>> getQppAttributes,
            Func<int, IEnumerable<DomainValue>> getDomainValues,
            Func<string, long> getCollectionValues)
        {
            if (Initialized)
                return;
            Initialized = true;
            ByName = new Dictionary<string, object>();
            ById = new Dictionary<long, object>();

            foreach (var qppAttribute in getQppAttributes())
            {
                object attribute = null;
                if (qppAttribute.valueType == AttributeValueTypes.TEXT)
                {
                    attribute = new TextAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.NUMERIC)
                {
                    attribute = new NumAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.BOOLEAN)
                {
                    attribute = new BoolAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DATETIME)
                {
                    attribute = new DateTimeAttr(qppAttribute);
                }
                else if (qppAttribute.valueType == AttributeValueTypes.DOMAIN)
                {
                    attribute = new DomainAttr(qppAttribute);
                }
                if (attribute != null)
                {
                    ByName[qppAttribute.name] = attribute;
                    ById[qppAttribute.id] = attribute;
                }
            }

            //var builder = new StringBuilder();

            foreach (var fieldInfo in typeof(PhoenixAttributes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attributePlaceholder = (IAttribute) fieldInfo.GetValue(null);
                object attribute = null;
                if (attributePlaceholder.Id != 0)
                {
                    if (ById.ContainsKey(attributePlaceholder.Id))
                        attribute = ById[attributePlaceholder.Id];
                }
                else
                {
                    if (ByName.ContainsKey(attributePlaceholder.Name))
                        attribute = ByName[attributePlaceholder.Name];
                }
                if (attribute != null)
                    fieldInfo.SetValue(null, attribute);

                //if (attribute != null)
                //    builder.AppendLine(string.Format("public static IAttribute<{0}> {1} = new AttributePlaceholder<{0}> {{Id={2},Name=\"{3}\"}}",
                //        GetAttributeType(attribute),
                //        fieldInfo.Name,
                //        attribute.Id,
                //        attribute.Name));
            }
            //Debug.Write(builder.ToString());
        }
    }

    public static partial class PhoenixAttributes
    {
        public static IAttribute<string> APP_STUDIO_ISSUE_URN = new AttributePlaceholder<string> { Id = 180, Name = "App Studio Issue URN" };
        public static IAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_ID = new AttributePlaceholder<long> { Id = 414, Name = "Article Component Reference Asset Id" };
        public static IAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_MAJ_VERSION = new AttributePlaceholder<long> { Id = 415, Name = "Article Component Reference Asset Major Version" };
        public static IAttribute<long> ARTICLE_COMP_REFRENCE_ASSET_MIN_VERSION = new AttributePlaceholder<long> { Id = 416, Name = "Article Component Reference Asset Minor version" };
        public static IAttribute<long> ARTICLE_COMPONENT_ID = new AttributePlaceholder<long> { Id = 305, Name = "Article Component id" };
        public static IAttribute<long> ARTICLE_ID = new AttributePlaceholder<long> { Id = 413, Name = "Article Id" };
        public static IAttribute<string> ASSET_CHECKSUM = new AttributePlaceholder<string> { Id = 407, Name = "Asset Checksum" };
        public static IAttribute<long> ATTACHED_COMPONENT_ID = new AttributePlaceholder<long> { Id = 412, Name = "Attached Component Id" };
        public static IAttribute<long> ATTACHED_LAYOUT_ID = new AttributePlaceholder<long> { Id = 401, Name = "Attached Layout Uid" };
        public static IAttribute<string> ATTACHMENT_CHECKSUM = new AttributePlaceholder<string> { Id = 408, Name = "Attachment Checksum" };
        public static IAttribute<string> AUXILIARY_DATA = new AttributePlaceholder<string> { Id = 409, Name = "Auxillary Data" };
        public static IAttribute<long> BOX_ID = new AttributePlaceholder<long> { Id = 402, Name = "Box Uid" };
        public static IAttribute<long> CHARACTER_COUNT = new AttributePlaceholder<long> { Id = 356, Name = "Character count" };
        public static IAttribute<DateTime> CHECK_OUT_DATE_TIME = new AttributePlaceholder<DateTime> { Id = 17, Name = "Check Out Date and Time" };
        public static IAttribute<string> CHECKED_OUT_APPLICATION = new AttributePlaceholder<string> { Id = 34, Name = "Checkedout Application" };
        public static IAttribute<PhoenixValue> CHECKED_OUT_BY = new AttributePlaceholder<PhoenixValue> { Id = 14, Name = "Checked out by" };
        public static IAttribute<long> CHECKED_OUT_DURATION = new AttributePlaceholder<long> { Id = 35, Name = "Checked out duration" };
        public static IAttribute<string> CHECKED_OUT_FILE_PATH = new AttributePlaceholder<string> { Id = 16, Name = "Checked out file path" };
        public static IAttribute<string> CHECKED_OUT_MACHINE_NAME = new AttributePlaceholder<string> { Id = 15, Name = "Checked out machine name" };
        public static IAttribute<PhoenixValue> COLLECTION = new AttributePlaceholder<PhoenixValue> { Id = 55, Name = "Collection" };
        public static IAttribute<string> COLLECTION_PATH = new AttributePlaceholder<string> { Id = 57, Name = "Collection Path" };
        public static IAttribute<PhoenixValue> COLLECTION_TEMPLATE = new AttributePlaceholder<PhoenixValue> { Id = 58, Name = "Collection Template" };
        public static IAttribute<long> COLOR_DEPTH = new AttributePlaceholder<long> { Id = 204, Name = "Color depth" };
        public static IAttribute<PhoenixValue> COLOR_SPACE = new AttributePlaceholder<PhoenixValue> { Id = 205, Name = "Color space" };
        public static IAttribute<string> COMPONENT_NAME = new AttributePlaceholder<string> { Id = 303, Name = "Component name" };
        public static IAttribute<long> COMPONENT_POSITION = new AttributePlaceholder<long> { Id = 302, Name = "Component position" };
        public static IAttribute<PhoenixValue> CONTENT_CREATOR = new AttributePlaceholder<PhoenixValue> { Id = 32, Name = "Content creator" };
        public static IAttribute<PhoenixValue> CONTENT_TYPE = new AttributePlaceholder<PhoenixValue> { Id = 62, Name = "Content Type" };
        public static IAttribute<string> CONTENT_TYPE_HIERARCHY = new AttributePlaceholder<string> { Id = 63, Name = "Content Type Hierarchy" };
        public static IAttribute<DateTime> CREATED = new AttributePlaceholder<DateTime> { Id = 3, Name = "Created" };
        public static IAttribute<PhoenixValue> CREATOR = new AttributePlaceholder<PhoenixValue> { Id = 5, Name = "Creator" };
        public static IAttribute<bool> DEPENDENT_ON_COLLECTION_RESOURCES = new AttributePlaceholder<bool> { Id = 418, Name = "Dependent On Collection Resources" };
        public static IAttribute<string> DEVICE_NAME = new AttributePlaceholder<string> { Id = 158, Name = "Device Name" };
        public static IAttribute<string> DITA_AUDIENCE = new AttributePlaceholder<string> { Id = 452, Name = "Audience (DITA)" };
        public static IAttribute<string> DITA_AUTHOR = new AttributePlaceholder<string> { Id = 453, Name = "Author (DITA)" };
        public static IAttribute<string> DITA_BRAND = new AttributePlaceholder<string> { Id = 454, Name = "Brand (DITA)" };
        public static IAttribute<string> DITA_CATEGORY = new AttributePlaceholder<string> { Id = 455, Name = "Category (DITA)" };
        public static IAttribute<string> DITA_ID = new AttributePlaceholder<string> { Id = 456, Name = "Id (DITA)" };
        public static IAttribute<string> DITA_IMPORTANCE = new AttributePlaceholder<string> { Id = 466, Name = "Importance (DITA)" };
        public static IAttribute<string> DITA_KEYWORDS = new AttributePlaceholder<string> { Id = 457, Name = "Keywords (DITA)" };
        public static IAttribute<string> DITA_LANGUAGE = new AttributePlaceholder<string> { Id = 464, Name = "Language (DITA)" };
        public static IAttribute<string> DITA_NAVIGATION_TITLE = new AttributePlaceholder<string> { Id = 458, Name = "Navigation Title (DITA)" };
        public static IAttribute<string> DITA_OTHER_PROPERTIES = new AttributePlaceholder<string> { Id = 465, Name = "Other Properties (DITA)" };
        public static IAttribute<string> DITA_PLATFORM = new AttributePlaceholder<string> { Id = 459, Name = "Platform (DITA)" };
        public static IAttribute<string> DITA_PRODUCT_NAME = new AttributePlaceholder<string> { Id = 460, Name = "Product Name (DITA)" };
        public static IAttribute<string> DITA_PUBLISHING_CONTENT = new AttributePlaceholder<string> { Id = 463, Name = "Publishing Intent (DITA)" };
        public static IAttribute<string> DITA_SEARCH_TITLE = new AttributePlaceholder<string> { Id = 461, Name = "Search Title (DITA)" };
        public static IAttribute<string> DITA_TITLE = new AttributePlaceholder<string> { Id = 462, Name = "Title (DITA)" };
        public static IAttribute<string> FILE_EXTENSION = new AttributePlaceholder<string> { Id = 11, Name = "File extension" };
        public static IAttribute<string> FILE_PATH = new AttributePlaceholder<string> { Id = 8, Name = "File path" };
        public static IAttribute<long> FILE_SIZE = new AttributePlaceholder<long> { Id = 12, Name = "File size" };
        public static IAttribute<string> FIRST_PAGE = new AttributePlaceholder<string> { Id = 51, Name = "First page" };
        public static IAttribute<string> GLOBAL_ID = new AttributePlaceholder<string> { Id = 66, Name = "Global ID" };
        public static IAttribute<bool> HAS_CHILDREN = new AttributePlaceholder<bool> { Id = 65, Name = "Has Children" };
        public static IAttribute<long> ID = new AttributePlaceholder<long> { Id = 1, Name = "Id" };
        public static IAttribute<long> INDESIGN_OBJECT_UID = new AttributePlaceholder<long> { Id = 419, Name = "InDesign Object UID" };
        public static IAttribute<PhoenixValue> INDEXING_STATUS = new AttributePlaceholder<PhoenixValue> { Id = 29, Name = "Indexing status" };
        public static IAttribute<string> IPTC_BY_LINE = new AttributePlaceholder<string> { Id = 213, Name = "By-line (IPTC)" };
        public static IAttribute<string> IPTC_BY_LINE_TITLE = new AttributePlaceholder<string> { Id = 214, Name = "By-line Title (IPTC)" };
        public static IAttribute<string> IPTC_CAPTION = new AttributePlaceholder<string> { Id = 223, Name = "Caption (IPTC)" };
        public static IAttribute<string> IPTC_CATEGORY = new AttributePlaceholder<string> { Id = 208, Name = "Category (IPTC)" };
        public static IAttribute<string> IPTC_CITY = new AttributePlaceholder<string> { Id = 215, Name = "City (IPTC)" };
        public static IAttribute<string> IPTC_COPYRIGHT_NOTICE = new AttributePlaceholder<string> { Id = 222, Name = "Copyright Notice (IPTC)" };
        public static IAttribute<string> IPTC_COUNTRY = new AttributePlaceholder<string> { Id = 217, Name = "Country (IPTC)" };
        public static IAttribute<string> IPTC_CREDIT = new AttributePlaceholder<string> { Id = 220, Name = "Credit (IPTC)" };
        public static IAttribute<string> IPTC_HEADLINE = new AttributePlaceholder<string> { Id = 219, Name = "Headline (IPTC)" };
        public static IAttribute<string> IPTC_KEYWORDS = new AttributePlaceholder<string> { Id = 210, Name = "Keywords (IPTC)" };
        public static IAttribute<string> IPTC_OBJECT_NAME = new AttributePlaceholder<string> { Id = 206, Name = "Object Name (IPTC)" };
        public static IAttribute<string> IPTC_ORIGINAL_TRANSMISSION_REFERENCE = new AttributePlaceholder<string> { Id = 218, Name = "Original Transmission Reference (IPTC)" };
        public static IAttribute<string> IPTC_PROVINCE = new AttributePlaceholder<string> { Id = 216, Name = "Province (IPTC)" };
        public static IAttribute<string> IPTC_SOURCE = new AttributePlaceholder<string> { Id = 221, Name = "Source (IPTC)" };
        public static IAttribute<string> IPTC_SPECIAL_INSTRUCTIONS = new AttributePlaceholder<string> { Id = 211, Name = "Special Instructions (IPTC)" };
        public static IAttribute<string> IPTC_SUPPLEMENTAL_CATEGORIES = new AttributePlaceholder<string> { Id = 209, Name = "Supplemental Categories (IPTC)" };
        public static IAttribute<string> IPTC_URGENCY = new AttributePlaceholder<string> { Id = 207, Name = "Urgency (IPTC)" };
        public static IAttribute<string> IPTC_WRITER = new AttributePlaceholder<string> { Id = 224, Name = "Writer (IPTC)" };
        public static IAttribute<bool> IS_CHECKED_OUT = new AttributePlaceholder<bool> { Id = 13, Name = "Is checked out" };
        public static IAttribute<bool> IS_COLLECTED = new AttributePlaceholder<bool> { Id = 18, Name = "Is collected" };
        public static IAttribute<bool> IS_COLLECTION_TEMPLATE = new AttributePlaceholder<bool> { Id = 59, Name = "Is Template" };
        public static IAttribute<bool> IS_PLACEHOLDER = new AttributePlaceholder<bool> { Id = 30, Name = "Is placeholder" };
        public static IAttribute<PhoenixValue> ISSUE = new AttributePlaceholder<PhoenixValue> { Id = 26, Name = "Issue" };
        public static IAttribute<string> ITEM_NAME = new AttributePlaceholder<string> { Id = 410, Name = "Item Name" };
        public static IAttribute<string> ITEM_TYPE = new AttributePlaceholder<string> { Id = 411, Name = "Item Type" };
        public static IAttribute<PhoenixValue> JOB_JACKET = new AttributePlaceholder<PhoenixValue> { Id = 44, Name = "Job jacket" };
        public static IAttribute<string> JOB_TICKET_TEMPLATE_NAME = new AttributePlaceholder<string> { Id = 45, Name = "Job ticket template name" };
        public static IAttribute<DateTime> LAST_CONTENT_UPDATE = new AttributePlaceholder<DateTime> { Id = 19, Name = "Last content update" };
        public static IAttribute<PhoenixValue> LAST_CONTENT_UPDATED_BY = new AttributePlaceholder<PhoenixValue> { Id = 20, Name = "Last content updated by" };
        public static IAttribute<DateTime> LAST_GEOMETRY_UPDATE = new AttributePlaceholder<DateTime> { Id = 21, Name = "Last geometry update" };
        public static IAttribute<DateTime> LAST_JOB_JACKET_MODIFIED = new AttributePlaceholder<DateTime> { Id = 10, Name = "Job jacket last modified" };
        public static IAttribute<DateTime> LAST_MODIFIED = new AttributePlaceholder<DateTime> { Id = 4, Name = "Last modified" };
        public static IAttribute<PhoenixValue> LAST_MODIFIER = new AttributePlaceholder<PhoenixValue> { Id = 6, Name = "Last modifier" };
        public static IAttribute<string> LAST_PAGE = new AttributePlaceholder<string> { Id = 52, Name = "Last page" };
        public static IAttribute<bool> LAYOUT_GEOMETRY_DIFFERS = new AttributePlaceholder<bool> { Id = 151, Name = "Layout Geometry Differs" };
        public static IAttribute<string> LAYOUT_NAME = new AttributePlaceholder<string> { Id = 157, Name = "Layout Name" };
        public static IAttribute<long> LINE_COUNT = new AttributePlaceholder<long> { Id = 354, Name = "Line count" };
        public static IAttribute<string> MAC_CREATOR_TYPE = new AttributePlaceholder<string> { Id = 47, Name = "Mac creator type" };
        public static IAttribute<string> MAC_OS_TYPE = new AttributePlaceholder<string> { Id = 46, Name = "Mac OS type" };
        public static IAttribute<long> MAJOR_VERSION = new AttributePlaceholder<long> { Id = 7, Name = "Major version" };
        public static IAttribute<string> MIME_TYPE = new AttributePlaceholder<string> { Id = 48, Name = "Mime type" };
        public static IAttribute<long> MINOR_VERSION = new AttributePlaceholder<long> { Id = 61, Name = "Minor version" };
        public static IAttribute<string> NAME = new AttributePlaceholder<string> { Id = 2, Name = "Name" };
        public static IAttribute<long> NUMBER_OF_LAYOUTS = new AttributePlaceholder<long> { Id = 60, Name = "Number of Layouts" };
        public static IAttribute<long> NUMBER_OF_PAGES = new AttributePlaceholder<long> { Id = 43, Name = "Number of pages" };
        public static IAttribute<PhoenixValue> ORIENTATION = new AttributePlaceholder<PhoenixValue> { Id = 153, Name = "Orientation" };
        public static IAttribute<string> ORIGINAL_FILENAME = new AttributePlaceholder<string> { Id = 33, Name = "Original filename" };
        public static IAttribute<long> PAGE_INDEX = new AttributePlaceholder<long> { Id = 404, Name = "Page Index" };
        public static IAttribute<string> PAGE_NAME = new AttributePlaceholder<string> { Id = 403, Name = "Page Name" };
        public static IAttribute<long> PIXEL_HEIGHT = new AttributePlaceholder<long> { Id = 202, Name = "Pixel height" };
        public static IAttribute<long> PIXEL_WIDTH = new AttributePlaceholder<long> { Id = 201, Name = "Pixel width" };
        public static IAttribute<string> RELATION_STATUS = new AttributePlaceholder<string> { Id = 64, Name = "Relationship Status" };
        public static IAttribute<PhoenixValue> REPOSITORY = new AttributePlaceholder<PhoenixValue> { Id = 28, Name = "Repository" };
        public static IAttribute<PhoenixValue> REPOSITORY_TYPE = new AttributePlaceholder<PhoenixValue> { Id = 27, Name = "Repository type" };
        public static IAttribute<long> RESOLUTION = new AttributePlaceholder<long> { Id = 203, Name = "Resolution" };
        public static IAttribute<string> REVISION_COMMENTS = new AttributePlaceholder<string> { Id = 31, Name = "Revision comments" };
        public static IAttribute<PhoenixValue> ROUTED_TO = new AttributePlaceholder<PhoenixValue> { Id = 25, Name = "Routed to" };
        public static IAttribute<PhoenixValue> STATUS = new AttributePlaceholder<PhoenixValue> { Id = 24, Name = "Status" };
        public static IAttribute<PhoenixValue> STORY_DIRECTION = new AttributePlaceholder<PhoenixValue> { Id = 154, Name = "Story Direction" };
        public static IAttribute<PhoenixValue> TEXT_INDEXING_STATUS = new AttributePlaceholder<PhoenixValue> { Id = 53, Name = "Text Indexing status" };
        public static IAttribute<string> TEXT_PREVIEW = new AttributePlaceholder<string> { Id = 352, Name = "Text preview" };
        public static IAttribute<long> WORD_COUNT = new AttributePlaceholder<long> { Id = 353, Name = "Word count" };
        public static IAttribute<PhoenixValue> WORKFLOW = new AttributePlaceholder<PhoenixValue> { Id = 54, Name = "Workflow" };
        public static IAttribute<string> XPATH = new AttributePlaceholder<string> { Id = 417, Name = "XPath" };
        public static IAttribute<long> SourceDocumentId = new AttributePlaceholder<long> { Id = 504, Name = "Source Document Id" };
        public static IAttribute<DateTime> PublishedDate = new AttributePlaceholder<DateTime> { Id = 505, Name = "Published Date" };
        public static IAttribute<bool> UserSetPublishedDate = new AttributePlaceholder<bool> { Id = 506, Name = "User-Set Published Date" };
        public static IAttribute<DateTime> UserProposedPublishedDate = new AttributePlaceholder<DateTime> { Id = 509, Name = "User-Proposed Published Date" };
        public static IAttribute<bool> DontUpdatePublishedDate = new AttributePlaceholder<bool> { Id = 507, Name = "Don't Update Published Date" };
        public static IAttribute<bool> HiddenUpdate = new AttributePlaceholder<bool> { Id = 508, Name = "Hidden Update" };
        public static IAttribute<string> InRangeName = new AttributePlaceholder<string> { Id = 515, Name = "InRangeName" };
        public static IAttribute<string> InRangeValue = new AttributePlaceholder<string> { Id = 516, Name = "InRangeValue" };
        public static IAttribute<string> OutSheet = new AttributePlaceholder<string> { Id = 517, Name = "OutSheet" };
        public static IAttribute<string> OutRange = new AttributePlaceholder<string> { Id = 518, Name = "OutRange" };
        public static IAttribute<string> OutChartName = new AttributePlaceholder<string> { Id = 519, Name = "OutChartName" };
        public static IAttribute<string> OutImageName = new AttributePlaceholder<string> { Id = 520, Name = "OutImageName" };
        public static IAttribute<string> PdfBusinessLine = new AttributePlaceholder<string> { Id = 620, Name = "PDF Business Line" };
        public static IAttribute<string> PdfReportType = new AttributePlaceholder<string> { Id = 621, Name = "PDF Report Type" };
        public static IAttribute<string> PdfReportTitle = new AttributePlaceholder<string> { Id = 622, Name = "PDF Report Title" };
        public static IAttribute<string> PdfSubheading = new AttributePlaceholder<string> { Id = 623, Name = "PDF Subheading" };
        public static IAttribute<string> PdfDate = new AttributePlaceholder<string> { Id = 624, Name = "PDF Date" };
        public static IAttribute<string> PdfAddressBar = new AttributePlaceholder<string> { Id = 625, Name = "PDF Address Bar" };
        public static IAttribute<string> UploaderVersion = new AttributePlaceholder<string> { Id = 501, Name = "Uploader Version" };
        public static IAttribute<long> AnalystWhoSentForReviewId = new AttributePlaceholder<long> { Id = 502, Name = "Analyst who sent for review id" };
        public static IAttribute<long> EditorWhoSentBackToAnalystId = new AttributePlaceholder<long> { Id = 503, Name = "Editor who sent back to Analyst id" };
        public static IAttribute<string> PublishFailReason = new AttributePlaceholder<string> { Id = 510, Name = "Publishing fail reason" };
        public static IAttribute<DateTime> DataRefreshStart = new AttributePlaceholder<DateTime> { Id = 511, Name = "Data refresh started" };
        public static IAttribute<DateTime> DataRefreshEnd = new AttributePlaceholder<DateTime> { Id = 512, Name = "Data refresh ended" };
        public static IAttribute<DateTime> DataRefreshExcelPull = new AttributePlaceholder<DateTime> { Id = 513, Name = "Data refresh excels pulled" };
        public static IAttribute<string> DataRefreshFailReason = new AttributePlaceholder<string> { Id = 514, Name = "Data refresh fail reason" };
        public static IAttribute<string> ChemicalProductAndMarket = new AttributePlaceholder<string> { Id = 635, Name = "Chemical Product and Market(Taxonomy)" };
        public static IAttribute<long> BrandTaxonomy = new AttributePlaceholder<long> { Id = 628, Name = "Brand(Taxonomy)" };
        public static IAttribute<string> DomainsTaxonomy = new AttributePlaceholder<string> { Id = 627, Name = "Domain(Taxonomy)" };
        public static IAttribute<long> ContentTypeTaxonomy = new AttributePlaceholder<long> { Id = 629, Name = "Content Type(Taxonomy)" };
        public static IAttribute<long> FileClassificationTaxonomy = new AttributePlaceholder<long> { Id = 630, Name = "File Classification(Taxonomy)" };
        public static IAttribute<long> PublishFrequencyTaxonomy = new AttributePlaceholder<long> { Id = 631, Name = "Publish Frequency(Taxonomy)" };
        public static IAttribute<string> AuthorizationCode = new AttributePlaceholder<string> { Id = 634, Name = "Authorization Code" };
        public static IAttribute<string> Authors = new AttributePlaceholder<string> { Id = 636, Name = "Authors" };
        public static IAttribute<string> DomainUrl = new AttributePlaceholder<string> { Id = 638, Name = "Domain Url" };
        public static IAttribute<string> HardCopyPublicationDate = new AttributePlaceholder<string> { Id = 639, Name = "Hardcopy Publication Date" };
        public static IAttribute<string> BrandDisplayName = new AttributePlaceholder<string> { Id = 632, Name = "Brand" };
        public static IAttribute<string> DomainsDisplayName = new AttributePlaceholder<string> { Id = 633, Name = "Domains" };

    }

    public class CollectionValue
    {
        private readonly string _collection;

        public CollectionValue(string collection)
        {
            _collection = collection;
        }

        public static implicit operator string(CollectionValue value)
        {
            return value._collection;
        }
    }
}