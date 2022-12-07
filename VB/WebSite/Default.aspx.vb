Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Threading
Imports DevExpress.Web

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private table As DataTable

	Private ReadOnly Property DataTable() As DataTable
		Get
			If Session("DataTable") Is Nothing Then
				InitializeDataTable()
				Session("DataTable") = table
			End If
			table = CType(Session("DataTable"), DataTable)
			Return table
		End Get
	End Property

	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		gv.DataSource = DataTable
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) Then
			gv.DataBind()
		End If
	End Sub

	Private Sub InitializeDataTable()
		table = New DataTable("Table")
		Dim column As DataColumn

		column = New DataColumn()
		column.DataType = GetType(Int32)
		column.ColumnName = "ID"
		table.Columns.Add(column)

		table.PrimaryKey = New DataColumn() { column }

		column = New DataColumn()
		column.DataType = GetType(String)
		column.ColumnName = "Item"
		table.Columns.Add(column)

		PopulateDataTable()
	End Sub

	Private Sub PopulateDataTable()
		Dim row As DataRow
		For i As Integer = 0 To 19
			row = table.NewRow()
			row("ID") = i
			row("Item") = "Item " & i
			table.Rows.Add(row)
		Next i
	End Sub

	Protected Sub gv_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
		Dim gridView As ASPxGridView = CType(sender, ASPxGridView)
		Dim index As Integer = gridView.FocusedRowIndex
		If (e.Parameters = "up" AndAlso index <= 0) OrElse (e.Parameters = "down" AndAlso index >= gv.VisibleRowCount - 1) Then
			Return
		End If
		Dim id As Integer = CInt(Fix(gridView.GetRowValues(index, "ID")))

        Dim nextIndex As Integer = If(e.Parameters = "up", index - 1, index + 1)

		Dim nextId As Integer = CInt(Fix(gridView.GetRowValues(nextIndex, "ID")))

		Dim dataRow As DataRow = table.Rows.Find(id)
		dataRow("ID") = Integer.MaxValue

		Dim nextDataRow As DataRow = table.Rows.Find(nextId)
		nextDataRow("ID") = id

		dataRow("ID") = nextId

		gridView.DataBind()
		gridView.FocusedRowIndex = nextIndex
	End Sub
End Class