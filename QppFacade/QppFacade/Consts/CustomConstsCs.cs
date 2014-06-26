namespace IHS.Phoenix.QPP
{
    public static class CustomAttributes
    {
        public static readonly PhoenixValue SourceDocumentId = new PhoenixValue(-1, "Source Document Id");
        public static readonly PhoenixValue PublishedDate = new PhoenixValue(-2, "Published Date");
        public static readonly PhoenixValue UserSetPublishedDate = new PhoenixValue(-3, "User-Set Published Date");
        public static readonly PhoenixValue UserProposedPublishedDate = new PhoenixValue(-4, "User-Proposed Published Date");
        public static readonly PhoenixValue DontUpdatePublishedDate = new PhoenixValue(-5, "Don't Update Published Date");
        public static readonly PhoenixValue HiddenUpdate = new PhoenixValue(-6, "Hidden Update");
        public static readonly PhoenixValue InRangeName = new PhoenixValue(-7, "InRangeName");
        public static readonly PhoenixValue InRangeValue = new PhoenixValue(-8, "InRangeValue");
        public static readonly PhoenixValue OutSheet = new PhoenixValue(-9, "OutSheet");
        public static readonly PhoenixValue OutRange = new PhoenixValue(-10, "OutRange");
        public static readonly PhoenixValue OutChartName = new PhoenixValue(-11, "OutChartName");
        public static readonly PhoenixValue OutImageName = new PhoenixValue(-12, "OutImageName");
        public static readonly PhoenixValue PdfBusinessLine = new PhoenixValue(-13, "PDF Business Line");
        public static readonly PhoenixValue PdfReportType = new PhoenixValue(-14, "PDF Report Type");
        public static readonly PhoenixValue PdfReportTitle = new PhoenixValue(-15, "PDF Report Title");
        public static readonly PhoenixValue PdfSubheading = new PhoenixValue(-16, "PDF Subheading");
        public static readonly PhoenixValue PdfDate = new PhoenixValue(-17, "PDF Date");
        public static readonly PhoenixValue PdfAddressBar = new PhoenixValue(-18, "PDF Address Bar");
        public static readonly PhoenixValue UploaderVersion = new PhoenixValue(-19, "Uploader Version");
        public static readonly PhoenixValue AnalystWhoSentForReviewId = new PhoenixValue(-20, "Analyst who sent for review id");
        public static readonly PhoenixValue EditorWhoSentBackToAnalystId = new PhoenixValue(-21, "Editor who sent back to Analyst id");
        public static readonly PhoenixValue PublishFailReason = new PhoenixValue(-22, "Publishing fail reason");
        public static readonly PhoenixValue DataRefreshStart = new PhoenixValue(-23, "Data refresh started");
        public static readonly PhoenixValue DataRefreshEnd = new PhoenixValue(-24, "Data refresh ended");
        public static readonly PhoenixValue DataRefreshExcelPull = new PhoenixValue(-25, "Data refresh excels pulled");
        public static readonly PhoenixValue DataRefreshFailReason = new PhoenixValue(-26, "Data refresh fail reason");
    }

    public static class CustomContentTypes
    {
        public static readonly PhoenixValue ObjectSourceSpreadsheet = new PhoenixValue(-27, "Object Source Spreadsheet");
        public static readonly PhoenixValue LogFile = new PhoenixValue(-28, "Log File");
        public static readonly PhoenixValue IHSDocument = new PhoenixValue(-29, "IHS Document");
        public static readonly PhoenixValue IHSDocumentMap = new PhoenixValue(-30, "IHS Document Map");
        public static readonly PhoenixValue ReportSection = new PhoenixValue(-31, "Report Section");
        public static readonly PhoenixValue Report = new PhoenixValue(-32, "Report");
        public static readonly PhoenixValue MyInsightReportSection = new PhoenixValue(-33, "My Insight Report Section");
        public static readonly PhoenixValue PDFPublicationComponent = new PhoenixValue(-34, "PDF Publication Component");
        public static readonly PhoenixValue ContactsAndAnalysts = new PhoenixValue(-35, "ContactsAndAnalysts");
        public static readonly PhoenixValue PdfPublication = new PhoenixValue(-36, "PDF Publication");
        public static readonly PhoenixValue PdfPublicationTemplate = new PhoenixValue(-37, "PDF Publication Template");
        public static readonly PhoenixValue SummaryTables = new PhoenixValue(-38, "Summary Tables");
    }

    public static class CustomPrivilegeGroups
    {
        public static readonly PhoenixValue Phoenix = new PhoenixValue(-39, "Phoenix");
    }

    public static class CustomPrivileges
    {
        public static readonly PhoenixValue RefreshData = new PhoenixValue(-40, "Refresh Data");
    }

    public static class CustomRelations
    {
        public static readonly PhoenixValue TableSource = new PhoenixValue(-41, "Table Source Reference");
        public static readonly PhoenixValue ChartSource = new PhoenixValue(-42, "Chart Source Reference");
        public static readonly PhoenixValue ImageSource = new PhoenixValue(-43, "Image Source Reference");
    }

    public static class CustomRoles
    {
        public static readonly PhoenixValue Administrator = new PhoenixValue(-44, "Administrator");
        public static readonly PhoenixValue ContentCreator = new PhoenixValue(-45, "Content Creator");
        public static readonly PhoenixValue ContentEditor = new PhoenixValue(-46, "Content Editor");
        public static readonly PhoenixValue DataAdmin = new PhoenixValue(-47, "Data Admin");
        public static readonly PhoenixValue ContentCreatorAdmin = new PhoenixValue(-48, "Content Creator Admin");
        public static readonly PhoenixValue ContentEditorAdmin = new PhoenixValue(-49, "Content Editor Admin");
        public static readonly PhoenixValue DataAndContentCreatorAdmin = new PhoenixValue(-50, "Data And Content Creator Admin");
    }

    public static class CustomStatuses
    {
        public static readonly PhoenixValue Default = new PhoenixValue(-51, "Default");
        public static readonly PhoenixValue PublishingInQueue = new PhoenixValue(-52, "Publishing - In Queue");
        public static readonly PhoenixValue Published = new PhoenixValue(-53, "Published");
        public static readonly PhoenixValue ContentCreationAuthoring = new PhoenixValue(-54, "Content Creation - Authoring");
        public static readonly PhoenixValue ContentEditingQuickAmends = new PhoenixValue(-55, "Content Editing - Quick Amends");
        public static readonly PhoenixValue ContentEditingInQueue = new PhoenixValue(-56, "Content Editing - In Queue");
        public static readonly PhoenixValue ContentEditing = new PhoenixValue(-57, "Content Editing");
        public static readonly PhoenixValue ContentCreationCorrecting = new PhoenixValue(-58, "Content Creation - Correcting");
        public static readonly PhoenixValue Publishing = new PhoenixValue(-59, "Publishing");
        public static readonly PhoenixValue PublishingFailed = new PhoenixValue(-60, "Publishing - Failed");
        public static readonly PhoenixValue DataAdminUpdating = new PhoenixValue(-61, "Data Admin Updating");
        public static readonly PhoenixValue ReadyForDataAdminUpdate = new PhoenixValue(-62, "Ready For Data Admin Update");
        public static readonly PhoenixValue DataRefreshInQueue = new PhoenixValue(-63, "Data Refresh - In Queue");
        public static readonly PhoenixValue DataRefresh = new PhoenixValue(-64, "Data Refresh");
        public static readonly PhoenixValue DataRefreshFailed = new PhoenixValue(-65, "Data Refresh - Failed");
        public static readonly PhoenixValue DataRefreshCompleted = new PhoenixValue(-66, "Data Refresh - Completed");
    }

    public static class CustomWorkflows
    {
        public static readonly PhoenixValue Default = new PhoenixValue(-67, "Default Workflow");
        public static readonly PhoenixValue Document = new PhoenixValue(-68, "Document Workflow");
        public static readonly PhoenixValue ObjectSource = new PhoenixValue(-69, "Object Source Workflow");
        public static readonly PhoenixValue DataRefresh = new PhoenixValue(-70, "Data Refresh Workflow");
    }

    public static class CustomCollections
    {
        public static readonly PhoenixValue HomeTest = new PhoenixValue(-71, "Home/Test");
    }
}