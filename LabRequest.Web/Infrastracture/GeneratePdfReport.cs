using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using LabRequest.DomainModel.Repository;
using LabRequest.DomainModel.Entities;
using PdfRpt.Core.Helper;

namespace LabRequest.Web.Infrastracture
{
    public class GeneratePdfReport
    {
        private readonly ReportRepository _reportrepository = new ReportRepository();
        private readonly UserRepository _userRepository = new UserRepository();
        public IPdfReportData CreatePdfReport(Report model)
        {
            var report = new PdfReport();
            report.DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.RightToLeft);
                doc.Orientation(model.ReportType == "r2"
                    ? PageOrientation.Landscape
                    : PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
            });
            report.DefaultFonts(font =>
            {
                font.Path(AppPath.ApplicationPath
                        + @"\Content\fonts\mirza-font\Mirza.ttf",
                        Environment.GetEnvironmentVariable("SystemRoot")
                        + @"\fonts\tahoma.ttf");
                font.Size(10);
            });
            report.PagesFooter(footer =>
                footer.DefaultFooter(string.Format("تاریخ تهیه گزارش:{0} - تهیه کننده:{1}",
                PersianDateTime.Now.ToString
                    (PersianDateTimeFormat.FullDateFullTime),
                    _userRepository.GetUser(HttpContext.Current.User.Identity.Name,
                    int.Parse(CookieEncryptor.Decrypt(HttpContext.Current.Request.Cookies["LabRequest-Cookie"].Value)))
                    .PersonFamily)));
            report.MainTableTemplate(template =>
                template.BasicTemplate(BasicTemplate.SilverTemplate));
            switch (model.ReportType)
            {
                case "r1":
                    {
                        report.PagesHeader(header =>
                        {
                            header.CacheHeader(true);
                            header.XHtmlHeader(rptHeader =>
                            {
                                rptHeader.PageHeaderProperties(new XHeaderBasicProperties
                                {
                                    RunDirection = PdfRunDirection.RightToLeft,
                                    ShowBorder = true
                                });
                                rptHeader.AddPageHeader(pageHeader =>
                                {
                                    const string message = "گزارش نتایج درخواستها";
                                    var photo = AppPath.ApplicationPath + @"\Content\images\bipc.png";
                                    var image = string.Format("<img src='{0}' height='100px' width='150px'/>", photo);
                                    return string.Format(@"<table style='font-family:Mirza;width:100%;font-size:14pt;text-align:center'>
										            <tr>
											            <td>{0}</td>
										            </tr>
										            <tr>
											            <td>{1}</td>
										            </tr>
								                </table>", image, message);
                                });

                                rptHeader.GroupHeaderProperties(new XHeaderBasicProperties
                                {
                                    RunDirection = PdfRunDirection.RightToLeft,
                                    ShowBorder = true,
                                    SpacingBeforeTable = 10f
                                });
                                rptHeader.AddGroupHeader(groupHeader =>
                                {
                                    var data = groupHeader.NewGroupInfo;
                                    var pcode = data.GetSafeStringValueOf<TestResultReport>(x => x.Pcode);
                                    var unit = data.GetSafeStringValueOf<TestResultReport>(x => x.ApplicantName);
                                    var request = data.GetSafeStringValueOf<TestResultReport>(x => x.RequestNameDes);
                                    var sample = data.GetSafeStringValueOf<TestResultReport>(x => x.SampleName);
                                    var date = data.GetSafeStringValueOf<TestResultReport>(x => x.ExeDate);
                                    return string.Format(@"<table style='width:100%;font-size:10pt;font-family:Mirza'>
                                    												            <tr>
                                                                                                    <td style='font-family:tahoma'>{0}</td>                                                    
                                    													            <td>شماره پیگیری:</td>													            
                                                                                                    <td style='font-family:tahoma'>{1}</td>                                                    
                                    													            <td>عنوان درخواست:</td>													            
                                    												            </tr>
                                    												            <tr>
                                                                                                    <td style='font-family:tahoma'>{2}</td>                                                    
                                    													            <td>درخواست کننده:</td>													            
                                                                                                    <td style='font-family:tahoma'>{3}'</td>                                                    
                                    													            <td>نام نمونه:</td>													            
                                    												            </tr>
                                    												            <tr>
                                    													            <td style='font-family:tahoma'>{4}</td>
                                    													            <td>تاریخ انجام درخواست:</td>
                                    												            </tr>
                                    								                </table>",
                                                !string.IsNullOrEmpty(pcode) ? pcode : string.Empty,
                                                !string.IsNullOrEmpty(request) ? request : string.Empty,
                                                !string.IsNullOrEmpty(unit) ? unit : string.Empty,
                                                !string.IsNullOrEmpty(sample) ? sample : string.Empty,
                                                !string.IsNullOrEmpty(date) ? date : string.Empty);
                                });
                            });
                        });
                        report.MainTablePreferences(table =>
                        {
                            table.ColumnsWidthsType(TableColumnWidthType.Relative);
                            table.NumberOfDataRowsPerPage(0);
                            table.GroupsPreferences(new GroupsPreferences
                            {
                                GroupType = GroupType.HideGroupingColumns,
                                RepeatHeaderRowPerGroup = true,
                                ShowOneGroupPerPage = true,
                                SpacingBeforeAllGroupsSummary = 5f,
                                NewGroupAvailableSpacingThreshold = 300,
                                SpacingAfterAllGroupsSummary = 5f
                            });
                            table.SpacingAfter(4f);
                        });
                        report.MainTableDataSource(datasource =>
                             {
                                 datasource.StronglyTypedList
                                     (_reportrepository.TestResult
                                     (model, PersianDateTime.Now.Year.ToString()));
                             });
                        report.MainTableColumns(columns =>
                        {
                            columns.AddColumn(column =>
                            {
                                column.PropertyName("rowNo");
                                column.IsRowNumber(true);
                                column.IsVisible(true);
                                column.Order(0);
                                column.Width(1);
                                column.HeaderCell("#");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<TestResultReport>(x => x.Pcode);
                                column.IsVisible(true);
                                column.Order(1);
                                column.Width(3);
                                column.HeaderCell("شماره پیگیری");
                                column.Group((g1, g2) => g1.ToString() == g2.ToString());
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<TestResultReport>(x => x.OutputName);
                                column.IsVisible(true);
                                column.Order(2);
                                column.Width(3);
                                column.HeaderCell("نام خروجی");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<TestResultReport>(x => x.OutputValue);
                                column.IsVisible(true);
                                column.Order(3);
                                column.Width(3);
                                column.HeaderCell("نتیجه آزمایش");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<TestResultReport>(x => x.MeasurDes);
                                column.IsVisible(true);
                                column.Order(4);
                                column.Width(3);
                                column.HeaderCell("واحد اندازه گیری");
                            });
                        });
                        break;
                    }
                case "r2":
                    {
                        report.PagesHeader(header => header.DefaultHeader(defaultheader =>
                        {
                            //header.CacheHeader(false);
                            defaultheader.Message("لیست نتایج آزمایشهای انجام شده");
                            defaultheader.ImagePath(AppPath.ApplicationPath + @"Content\images\bipc.png");
                            defaultheader.RunDirection(PdfRunDirection.RightToLeft);
                        }));
                        report.MainTablePreferences(table =>
                        {
                            table.ColumnsWidthsType(TableColumnWidthType.Relative);
                            table.NumberOfDataRowsPerPage(0);
                            table.GroupsPreferences(new GroupsPreferences
                            {
                                GroupType = GroupType.HideGroupingColumns,
                                RepeatHeaderRowPerGroup = false,
                                ShowOneGroupPerPage = false,
                                SpacingBeforeAllGroupsSummary = 1f,
                                NewGroupAvailableSpacingThreshold = 150,
                                SpacingAfterAllGroupsSummary = 1f
                            });
                            table.SpacingAfter(1f);
                        });
                        report.MainTableDataSource(datasource =>
                        {
                            datasource.StronglyTypedList
                                (_reportrepository.TestResultlist(model,
                                PersianDateTime.Now.Year.ToString()));
                        });
                        report.MainTableColumns(columns =>
                        {
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.RowNumber);
                                //column.PropertyName("rowNo");
                                //column.IsRowNumber(true);
                                column.IsVisible(true);
                                column.Order(0);
                                column.Width(1);
                                column.HeaderCell("#");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.OutputName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(1);
                                column.Width(3);
                                column.HeaderCell("نام خروجی");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.Spec);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(2);
                                column.Width(2);
                                column.HeaderCell("SPEC");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.OutputValue);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(3);
                                column.Width(2);
                                column.HeaderCell("نتیجه آزمایش");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.Des);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(4);
                                column.Width(2);
                                column.HeaderCell("واحد اندازه گیری");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.SampleEName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(5);
                                column.Width(2);
                                column.HeaderCell("نام نمونه");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.ExeDate);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(6);
                                column.Width(2);
                                column.HeaderCell("تاریخ انجام");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.ConfirmDate);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(7);
                                column.Width(2);
                                column.HeaderCell("تاریخ تایید تست");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.ConfirmTime);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(8);
                                column.Width(2);
                                column.HeaderCell("ساعت تایید تست");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.Pcode);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(9);
                                column.Width(3);
                                column.HeaderCell("کد پیگیری");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestResultListReport>(p => p.TestOutputId);
                                column.IsRowNumber(false);
                                column.IsVisible(false);
                                column.Group((g1, g2) => Convert.ToInt32(g1) == Convert.ToInt32(g2));
                            });
                        });
                        break;
                    }
                case "r3":
                    {
                        report.PagesHeader(header => header.DefaultHeader(defaultheader =>
                        {
                            header.CacheHeader(false);
                            defaultheader.Message("لیست وضعیت تستهای درخواست شده");
                            defaultheader.ImagePath(AppPath.ApplicationPath + @"Content\images\bipc.png");
                            defaultheader.RunDirection(PdfRunDirection.RightToLeft);
                        }));
                        report.MainTablePreferences(table =>
                        {
                            table.ColumnsWidthsType(TableColumnWidthType.Relative);
                            table.NumberOfDataRowsPerPage(0);
                            table.GroupsPreferences(new GroupsPreferences
                            {
                                GroupType = GroupType.HideGroupingColumns,
                                RepeatHeaderRowPerGroup = false,
                                ShowOneGroupPerPage = false,
                                //SpacingBeforeAllGroupsSummary = 1f,
                                //NewGroupAvailableSpacingThreshold = 150,
                                //SpacingAfterAllGroupsSummary = 1f
                            });
                            table.SpacingAfter(1f);
                        });
                        report.MainTableDataSource(datasource =>
                        {
                            datasource.StronglyTypedList
                                (_reportrepository.TestStatusList(model,
                                PersianDateTime.Now.Year.ToString()));
                        });
                        report.MainTableColumns(columns =>
                        {
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.RowNumber);
                                //column.PropertyName("rowNo");
                                column.IsRowNumber(true);
                                column.IsVisible(false);
                                column.Order(0);
                                column.Width(1);
                                column.HeaderCell("#");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.ApplicantName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(1);
                                column.Width(2);
                                column.HeaderCell("واحد درخواست کننده");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.SampleName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(2);
                                column.Width(3);
                                column.HeaderCell("نام نمونه");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.RequestDate);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(3);
                                column.Width(2);
                                column.HeaderCell("تاریخ درخواست");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.TestState);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(4);
                                column.Width(1);
                                column.HeaderCell("وضعیت");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.Pcode);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(5);
                                column.Width(3);
                                column.HeaderCell("کد پیگیری");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.LaboratoryId);
                                column.IsRowNumber(false);
                                column.IsVisible(false);
                                column.Order(6);
                                column.Width(2);
                                column.HeaderCell("شماره لابراتوار");
                                column.Group(true,
                                 (g1, g2) => g1.ToString() == g2.ToString());
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserTestStatuslist>(p => p.UnitId);
                                column.IsRowNumber(false);
                                column.IsVisible(false);
                                column.Order(5);
                                column.Width(2);
                                column.HeaderCell("کد واحد");
                                column.Group(true, (h1, h2) => h1.ToString() == h2.ToString());
                            });
                        });
                        break;
                    }
                case "r4":
                    {
                        report.PagesHeader(header =>
                        {
                            header.CacheHeader(true);
                            header.XHtmlHeader(rptHeader =>
                            {
                                rptHeader.PageHeaderProperties(new XHeaderBasicProperties
                                {
                                    RunDirection = PdfRunDirection.RightToLeft,
                                    ShowBorder = true
                                });
                                rptHeader.AddPageHeader(pageHeader =>
                                {
                                    const string message = "درخواستهای انجام شده شرکتها";
                                    var photo = AppPath.ApplicationPath + @"\Content\images\bipc.png";
                                    var image = string.Format("<img src='{0}' height='100px' width='150px'/>", photo);
                                    return string.Format(@"<table style='font-family:Mirza;width:100%;font-size:14pt;text-align:center'>
										            <tr>
											            <td>{0}</td>
										            </tr>
										            <tr>
											            <td>{1}</td>
										            </tr>
								                </table>", image, message);
                                });

                                rptHeader.GroupHeaderProperties(new XHeaderBasicProperties
                                {
                                    RunDirection = PdfRunDirection.RightToLeft,
                                    ShowBorder = true,
                                    SpacingBeforeTable = 20f
                                });
                                rptHeader.AddGroupHeader(groupHeader =>
                                {
                                    var data = groupHeader.NewGroupInfo;
                                    var pcode = data.GetSafeStringValueOf<UserCompanyOperation>(x => x.Des);
                                    var unit = data.GetSafeStringValueOf<UserCompanyOperation>(x => x.ApplicantName);
                                    return string.Format(@"<table style='width:30%;font-size:10pt;font-family:Mirza'>
												            <tr>
                                                                <td style='font-family:tahoma'>{0}</td>                                                    
													            <td>واحد:</td>													            
                                                                <td style='font-family:tahoma'>{1}</td>                                                    
													            <td>شرکت:</td>													            
												            </tr>
								                </table>",
                                        !string.IsNullOrEmpty(unit) ? unit : string.Empty,
                                        !string.IsNullOrEmpty(pcode) ? pcode : string.Empty);
                                });
                            });
                        });

                        report.MainTablePreferences(table =>
                        {
                            table.ColumnsWidthsType(TableColumnWidthType.Relative);
                            table.NumberOfDataRowsPerPage(0);
                            table.GroupsPreferences(new GroupsPreferences
                            {
                                GroupType = GroupType.HideGroupingColumns,
                                RepeatHeaderRowPerGroup = false,
                                ShowOneGroupPerPage = false,
                                SpacingBeforeAllGroupsSummary = 1f,
                                NewGroupAvailableSpacingThreshold = 150,
                                SpacingAfterAllGroupsSummary = 1f
                            });
                            table.SpacingAfter(1f);
                        });
                        report.MainTableDataSource(datasource =>
                        {
                            datasource.StronglyTypedList
                                (_reportrepository.CompanyOperationList(model,
                                PersianDateTime.Now.Year.ToString()));
                        });
                        report.MainTableSummarySettings(summarySettings =>
                        {
                            summarySettings.OverallSummarySettings("مجموع:");
                            //summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                            //summarySettings.PageSummarySettings("Page Summary");
                        });
                        report.MainTableColumns(columns =>
                        {
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.RowNumber);
                                //column.PropertyName("rowNo");
                                column.IsRowNumber(true);
                                column.IsVisible(true);
                                column.Order(0);
                                column.Width(1);
                                column.HeaderCell("#");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.Pcode);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(1);
                                column.Width(3);
                                column.HeaderCell("کد پیگیری");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.TestName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(2);
                                column.Width(2);
                                column.HeaderCell("نام تست");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.SampleName);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(3);
                                column.Width(2);
                                column.HeaderCell("نام نمونه");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.Description);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(4);
                                column.Width(1);
                                column.HeaderCell("واحد اندازه گیری");
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.Price);
                                column.IsRowNumber(false);
                                column.IsVisible(true);
                                column.Order(5);
                                column.Width(2);
                                column.HeaderCell("قیمت");
                                //column.ColumnItemsTemplate(template =>
                                //{
                                //    template.TextBlock();
                                //    template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                                //});
                                column.AggregateFunction(aggregateFunction =>
                                {
                                    aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                                    aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
                                });
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.CompanyId);
                                column.IsRowNumber(false);
                                column.IsVisible(false);
                                column.Order(6);
                                column.Width(2);
                                //column.HeaderCell("قیمت");
                                column.Group((g1, g2) => g1.ToString() == g2.ToString());
                            });
                            columns.AddColumn(column =>
                            {
                                column.PropertyName<UserCompanyOperation>(p => p.ApplicantName);
                                column.IsRowNumber(false);
                                column.IsVisible(false);
                                column.Order(7);
                                column.Width(2);
                                //column.HeaderCell("قیمت");
                                column.Group((g1, g2) => g1.ToString() == g2.ToString());
                            });
                        });
                        break;
                    }
            }
            report.MainTableEvents(
            events =>
                events.DataSourceIsEmpty("اطلاعات جهت نمایش یافت نشد دوباره امتحان کنید"));
            report.Export(export => export.ToExcel());
            //var fileName = string.Format("گزارش-{0}.pdf", Guid.NewGuid().ToString("N"));
            report.Encrypt(encrypt =>
             {
                 encrypt.WithPassword("secret");
                 encrypt.WithPermissions(new DocumentPermissions
                 {
                     AllowAssembly = false,
                     AllowCopy = false,
                     AllowDegradedPrinting = false,
                     AllowFillIn = false,
                     AllowModifyAnnotations = false,
                     AllowModifyContents = false,
                     AllowPrinting = false,
                     AllowScreenReaders = false
                 });
             });
            var fileName = string.Format("Report-{0}.pdf", Guid.NewGuid());
            return report.Generate(data =>
                {
                    fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                    data.FlushInBrowser(fileName, FlushType.Inline);
                });
            #region code comment
            //events.MainTableCreated(args =>
            //   {
            //       var infoTable = new PdfGrid(numColumns: 1)
            //       {
            //           WidthPercentage = 100
            //       };
            //       infoTable.AddSimpleRow(
            //            (cellData, properties) =>
            //            {
            //                cellData.Value = "Show data before the main table ...";
            //                properties.PdfFont = events.PdfFont;
            //                properties.RunDirection = PdfRunDirection.LeftToRight;
            //            });
            //       var table = infoTable.AddBorderToTable(borderColor: BaseColor.LIGHT_GRAY, spacingBefore: 10f);
            //       table.SpacingAfter = 10f;
            //       args.PdfDoc.Add(table);
            //   });
            //events.ShouldSkipRow(args =>
            //{
            //    var rowData = args.TableRowData;
            //    //var previousTableRowData = args.PreviousTableRowData;

            //    var description = rowData.FirstOrDefault(x => x.PropertyName == "Description");
            //    if (description != null &&
            //        description.PropertyValue.ToSafeString() == "Description Description ... 1")
            //    {
            //        return true; // don't render this row.
            //    }

            //    return false;
            //});

            //var pageNumber = 0;
            //events.ShouldSkipHeader(args =>
            //{
            //    pageNumber++;
            //    if (pageNumber == 2)
            //    {
            //        return true; // don't render this header row.
            //    }

            //    return false;
            //});

            //events.ShouldSkipFooter(args =>
            //{
            //    if (pageNumber == 2)
            //    {
            //        return true; // don't render this footer row.
            //    }

            //    return false;
            //});

            //events.MainTableAdded(maintable =>
            //{
            //    var infoTable = new PdfGrid(numColumns: 2) { WidthPercentage = 80, SpacingAfter = 50, SpacingBefore = 50 };
            //    infoTable.AddSimpleRow(
            //         (cellData, properties) =>
            //         {
            //             cellData.Value = "میثم";
            //             properties.ShowBorder = false;
            //             properties.BorderWidth = 0;
            //             properties.BorderWidth = 000;
            //             properties.PdfFont = events.PdfFont;
            //             properties.RunDirection = PdfRunDirection.RightToLeft;
            //         },
            //         (cellData, properties) =>
            //         {
            //             cellData.Value = "سامورایی";
            //             properties.ShowBorder = false;
            //             properties.BorderWidth = 0;
            //             properties.BorderWidth = 000;
            //             properties.PdfFont = events.PdfFont;
            //             properties.RunDirection = PdfRunDirection.RightToLeft;
            //         }
            //         );
            //    maintable.PdfDoc.Add(infoTable.AddBorderToTable());
            //});
            #endregion
        }
    }
}