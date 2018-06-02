using GQService.com.gq.dto;
using GQService.com.gq.excel;
using GQService.com.gq.service;
using GQ.com.gq.graficos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

public class Main
{
    private IList<GraficoEjemploDto> GetDatos(string baseDatos, string filtro)
    {
        var sql = Services.session.CreateSQLQuery(string.Format(@"
        SELECT 
            Filtro, 
            ID_Fecha, 
            Concepto, 
            Valor
        FROM `{0}`.gq_grafico_valores
        WHERE Filtro = :Filtro 
        ORDER BY Concepto, ID_Fecha
        ", baseDatos));
        sql.SetParameter("Filtro", filtro);

        sql.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(GraficoEjemploDto)));

        return sql.List<GraficoEjemploDto>();
    }

    #region Excel
    public object GetExcel(string baseDatos, string filtro)
    {
        var list = GetDatos(baseDatos, filtro);

        var workbook = new Workbook();
        workbook.Worksheets.Add(new Worksheet("Datos"));
        var worksheet = workbook.Worksheets[0];

        var ws = worksheet;

        ws.GenerateByIEnumerable(list);

        byte[] bytes;
        using (MemoryStream oStream = new MemoryStream())
        {
            workbook.Save(oStream);
            oStream.Position = 0;
            bytes = oStream.ToArray();
        }
        var file = new FileContentResult(bytes, "application/octet-stream");
        file.FileDownloadName = baseDatos + "_" + filtro + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
        return file;
    }
    #endregion

    #region PDF
    public FileResult GetPDF(string baseDatos, string filtro)
    {
        MemoryStream workStream = new MemoryStream();
        StringBuilder status = new StringBuilder("");
        DateTime dTime = DateTime.Now;
        //file name to be created   
        Document doc = new Document();
        doc.SetMargins(0, 0, 0, 0);
        //Create PDF Table with 4 columns  
        PdfPTable tableLayout = new PdfPTable(4);
        doc.SetMargins(10, 10, 10, 10);

        PdfWriter.GetInstance(doc, workStream).CloseStream = false;
        doc.Open();

        //Add Content to PDF   
        doc.Add(Add_Content_To_PDF(tableLayout, baseDatos, filtro));

        // Closing the document  
        doc.Close();

        byte[] byteInfo = workStream.ToArray();
        workStream.Write(byteInfo, 0, byteInfo.Length);
        workStream.Position = 0;

        var file = new FileContentResult(byteInfo, "application/octet-stream");
        file.FileDownloadName = baseDatos + "_" + filtro + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
        return file;
    }

    protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, string baseDatos, string filtro)
    {
        var list = GetDatos(baseDatos, filtro);

        float[] headers = { 50, 50, 50, 50 }; //Header Widths  
        tableLayout.SetWidths(headers); //Set the pdf headers  
        tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
        tableLayout.HeaderRows = 1;

        tableLayout.AddCell(new PdfPCell(new Phrase("Titulo Pdf", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
        {
            Colspan = 12,
            Border = 0,
            PaddingBottom = 5,
            HorizontalAlignment = Element.ALIGN_CENTER
        });

        ////Add header  
        AddCellToHeader(tableLayout, "Filtro");
        AddCellToHeader(tableLayout, "ID_Fecha");
        AddCellToHeader(tableLayout, "Concepto");
        AddCellToHeader(tableLayout, "Valor");

        ////Add body  
        for (int i = 0; i < list.Count; i++)
        {
            AddCellToBody(tableLayout, list[i].Filtro);
            AddCellToBody(tableLayout, list[i].ID_Fecha.ToString());
            AddCellToBody(tableLayout, list[i].Concepto);
            AddCellToBody(tableLayout, list[i].Valor.ToString());
        }

        return tableLayout;
    }

    private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
    {
        tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
        {
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 5,
            BackgroundColor = iTextSharp.text.BaseColor.GRAY
        });
    }

    // Method to add single cell to the body  
    private static void AddCellToBody(PdfPTable tableLayout, string cellText)
    {
        tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
        {
            HorizontalAlignment = Element.ALIGN_LEFT,
            Padding = 5,
            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
        });
    }

    #endregion

    #region Grafico
    public object ObtenerGraficos(string baseDatos, string filtro)
    {
        ChartCollectionDto cc = new ChartCollectionDto();
        List<ChartDto> charts = cc.Charts;

        var result = GetDatos(baseDatos, filtro);

        var key = "";

        foreach (var item in result)
        {
            if (!key.Equals(item.Concepto))
            {
                key = item.Concepto;
                charts.Add(new ChartDto { key = key, yAxis = 1, type = ChartDto.TYPE_AREA, color = GetColor(item), order = GetOrder(item) });

            }
            charts[charts.Count - 1].values.Add(new ChartValuesDto { x = item.ID_Fecha, y = item.Valor });
        }


        cc.Charts = cc.Charts.OrderBy(x => x.order).ToList();

        return cc;
    }

    private int GetOrder(GraficoEjemploDto item)
    {
        switch (item.Concepto)
        {
            case "ConceptoX":
                return 0;
            case "ConceptoY":
                return 1;
        }
        return 0;
    }

    public string GetColor(GraficoEjemploDto item)
    {
        switch (item.Concepto)
        {
            case "ConceptoX":
                return "#4f81bd";
            case "ConceptoY":
                return "#ff0000";
        }
        return null;
    }
    #endregion

    public class GraficoEjemploDto
    {
        public string Filtro { get; set; }
        public long ID_Fecha { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
    }
}
