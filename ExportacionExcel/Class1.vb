Imports System.Windows.Forms

Public Class Class1

    Function llenarActividadesExcel(ByVal ElGrid As DataGridView) As Boolean

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        Try
            'Añadimos el Libro al programa, y la hoja al libro
            exLibro = exApp.Workbooks.Add
            exHoja = exLibro.Worksheets.Add()

            ' ¿Cuantas columnas y cuantas filas?
            Dim NCol As Integer = ElGrid.ColumnCount
            Dim NRow As Integer = ElGrid.RowCount
            'Aqui recorremos todas las filas, y por cada fila todas las columnas
            'y vamos escribiendo.

            For i As Integer = 1 To NCol
                exHoja.Cells.Item(1, i) = ElGrid.Columns(i - 1).Name.ToString
            Next

            For Fila As Integer = 0 To NRow - 1
                For Col As Integer = 0 To NCol - 1
                    exHoja.Cells.Item(Fila + 2, Col + 1) = ElGrid.Item(Col, Fila).Value
                Next
            Next
            'Titulo en negrita, Alineado al centro y que el tamaño de la columna
            'se ajuste al texto
            exHoja.Rows.Item(1).Font.Bold = 2
            exHoja.Rows.Item(1).Interior.Color = RGB(153, 204, 255)
            exHoja.Rows.Item(1).HorizontalAlignment = 3
            exHoja.Columns.AutoFit()
            exHoja.Columns.Range("A1", "M13").Borders.LineStyle = 1
            exHoja.Columns("A").NumberFormat = "0"
            exHoja.Columns("F").NumberFormat = "0"
            exHoja.Columns("C").HorizontalAlignment = 3
            'exHoja.Columns.Range("E2", "E13").Interior.Color = RGB(192, 192, 192)
            exHoja.Columns("E").HorizontalAlignment = 3
            exHoja.Columns("H").HorizontalAlignment = 3
            exHoja.Columns("I").HorizontalAlignment = 3
            exHoja.Columns("J").HorizontalAlignment = 3
            exHoja.Columns("K").HorizontalAlignment = 3
            exHoja.Columns("L").HorizontalAlignment = 3
            exHoja.Columns("N") = Nothing
            'Aplicación visible
            exApp.Application.Visible = True
            exHoja = Nothing
            exLibro = Nothing
            exApp = Nothing

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al exportar a Excel")
            Return False
        End Try
        Return True
    End Function
End Class
