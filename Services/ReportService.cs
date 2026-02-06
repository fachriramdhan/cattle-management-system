using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SimSapi.Models;

namespace SimSapi.Services
{
    public class ReportService
    {
        public byte[] GenerateProduksiReport(
            List<ProduksiSusu> data,
            DateTime startDate,
            DateTime endDate)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // =========================
                    // PAGE SETUP
                    // =========================
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x =>
                        x.FontSize(11)
                         .FontFamily("Arial"));

                    // =========================
                    // HEADER
                    // =========================
                    page.Header()
                        .Text("LAPORAN PRODUKSI SUSU")
                        .SemiBold()
                        .FontSize(18)
                        .FontColor(Colors.Blue.Darken2);

                    // =========================
                    // CONTENT
                    // =========================
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            // ===== INFO PERIODE =====
                            column.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text($"Periode: {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}");

                                row.RelativeItem()
                                    .AlignRight() // ✅ FIX: AlignRight di container
                                    .Text($"Total Produksi: {data.Sum(x => x.VolumeLiter):N2} Liter");
                            });

                            // ===== TABEL DATA =====
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(40);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                });

                                // ----- HEADER TABLE -----
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyleHeader).Text("No");
                                    header.Cell().Element(CellStyleHeader).Text("Kode Sapi");
                                    header.Cell().Element(CellStyleHeader).Text("Nama Sapi");
                                    header.Cell().Element(CellStyleHeader).Text("Volume (L)");
                                    header.Cell().Element(CellStyleHeader).Text("Tanggal");
                                });

                                // ----- BODY TABLE -----
                                int no = 1;
                                foreach (var item in data)
                                {
                                    table.Cell().Element(CellStyleBody)
                                        .Text(no++.ToString());

                                    table.Cell().Element(CellStyleBody)
                                        .Text(item.Sapi?.KodeSapi ?? "-");

                                    table.Cell().Element(CellStyleBody)
                                        .Text(item.Sapi?.NamaSapi ?? "-");

                                    table.Cell().Element(CellStyleBody)
                                        .Text(item.VolumeLiter.ToString("N2"));

                                    table.Cell().Element(CellStyleBody)
                                        .Text(item.Tanggal.ToString("dd/MM/yyyy"));
                                }
                            });
                        });

                    // =========================
                    // FOOTER
                    // =========================
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Halaman ");
                            x.CurrentPageNumber();
                            x.Span(" dari ");
                            x.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }

        // =========================
        // TABLE STYLES
        // =========================
        static IContainer CellStyleHeader(IContainer container)
        {
            return container
                .DefaultTextStyle(x => x.SemiBold())
                .PaddingVertical(5)
                .BorderBottom(1)
                .BorderColor(Colors.Black);
        }

        static IContainer CellStyleBody(IContainer container)
        {
            return container
                .PaddingVertical(5)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2);
        }
    }
}
