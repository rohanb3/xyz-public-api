using System;
using System.Collections.Generic;
using System.IO;
using Xyzies.TWC.OptymyzeClient.Models;
using OfficeOpenXml;

namespace Xyzies.TWC.OptymyzeClient.Utilities
{
    public class ExcelParser
    {
        public IEnumerable<RetailerCommissionEarningsRow> ParseRetailerCommissionEarnings(string path, string sheetName)
        {
            var fileInfo = new FileInfo(path);

            List<RetailerCommissionEarningsRow> retailerCommissionEarningsList = new List<RetailerCommissionEarningsRow>();

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                var sheet = excelPackage.Workbook.Worksheets[sheetName];

                int totalRow = sheet.Dimension.Rows;
                int totalColumns = sheet.Dimension.Columns;

                var dict = new Dictionary<RetailerCommissionEarningsColumnType, int>();

                for (int column = 1; column <= totalColumns; column++)
                {
                    switch (sheet.Cells[1, column].Value)
                    {
                        case "Account":
                            dict.Add(RetailerCommissionEarningsColumnType.Account, column);
                            break;
                        case "Bounty Rate":
                            dict.Add(RetailerCommissionEarningsColumnType.BountyRate, column);
                            break;
                        case "Bundle Name":
                            dict.Add(RetailerCommissionEarningsColumnType.BundleName, column);
                            break;
                        case "Closed Date":
                            dict.Add(RetailerCommissionEarningsColumnType.ClosedDate, column);
                            break;
                        case "Internet PSU":
                            dict.Add(RetailerCommissionEarningsColumnType.InternetPSU, column);
                            break;
                        case "Phone PSU":
                            dict.Add(RetailerCommissionEarningsColumnType.PhonePSU, column);
                            break;
                        case "Video PSU":
                            dict.Add(RetailerCommissionEarningsColumnType.VideoPSU, column);
                            break;
                        case "WO Number":
                            dict.Add(RetailerCommissionEarningsColumnType.WONumber, column);
                            break;
                        case "Direction":
                            dict.Add(RetailerCommissionEarningsColumnType.Direction, column);
                            break;
                        default:
                            continue;
                    }
                }

                for (int row = 2; row <= totalRow; row++)
                {
                    var retailerCommissionEarningsRow = new RetailerCommissionEarningsRow();

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.Account, out int column))
                    {
                        retailerCommissionEarningsRow.Account = sheet.Cells[row, column].Value?.ToString();
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.BountyRate, out column) && sheet.Cells[row, column].Value != null && decimal.TryParse(sheet.Cells[row, column].Value.ToString(), out decimal numberFromDecimalParse))
                    {
                        retailerCommissionEarningsRow.BountyRate = numberFromDecimalParse;
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.BundleName, out column))
                    {
                        retailerCommissionEarningsRow.BundleName = sheet.Cells[row, column].Value?.ToString();
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.ClosedDate, out column) && !string.IsNullOrWhiteSpace(sheet.Cells[row, column].Text) && DateTime.TryParse(sheet.Cells[row, column].Text, out DateTime date))
                    {
                        retailerCommissionEarningsRow.ClosedDate = date;
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.InternetPSU, out column) && sheet.Cells[row, column].Value != null && int.TryParse(sheet.Cells[row, column].Value.ToString(), out int numberFromIntParse))
                    {
                        retailerCommissionEarningsRow.InternetPSU = numberFromIntParse;
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.PhonePSU, out column) && sheet.Cells[row, column].Value != null && int.TryParse(sheet.Cells[row, column].Value.ToString(), out numberFromIntParse))
                    {
                        retailerCommissionEarningsRow.PhonePSU = numberFromIntParse;
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.VideoPSU, out column) && sheet.Cells[row, column].Value != null && int.TryParse(sheet.Cells[row, column].Value.ToString(), out numberFromIntParse))
                    {
                        retailerCommissionEarningsRow.VideoPSU = numberFromIntParse;
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.WONumber, out column))
                    {
                        retailerCommissionEarningsRow.WONumber = sheet.Cells[row, column].Value?.ToString();
                    }

                    if (dict.TryGetValue(RetailerCommissionEarningsColumnType.Direction, out column))
                    {
                        retailerCommissionEarningsRow.Direction = sheet.Cells[row, column].Value?.ToString();
                    }

                    retailerCommissionEarningsList.Add(retailerCommissionEarningsRow);
                };
            }
            return retailerCommissionEarningsList;
        }
    }
}
