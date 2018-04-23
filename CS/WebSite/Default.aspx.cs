using System;
using System.Data;
using System.Threading;
using DevExpress.Web.ASPxGridView;

public partial class _Default : System.Web.UI.Page {
    private DataTable table;

    private DataTable DataTable {
        get {
            if(Session["DataTable"] == null) {
                InitializeDataTable();
                Session["DataTable"] = table;
            }
            table = (DataTable)Session["DataTable"];
            return table;
        }
    }

    protected void Page_Init(object sender, EventArgs e) {
        gv.DataSource = DataTable;
    }

    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack)
            gv.DataBind();
    }

    private void InitializeDataTable() {
        table = new DataTable("Table");
        DataColumn column;

        column = new DataColumn();
        column.DataType = typeof(Int32);
        column.ColumnName = "ID";
        table.Columns.Add(column);

        table.PrimaryKey = new DataColumn[] { column };

        column = new DataColumn();
        column.DataType = typeof(String);
        column.ColumnName = "Item";
        table.Columns.Add(column);

        PopulateDataTable();
    }

    private void PopulateDataTable() {
        DataRow row;
        for(int i = 0; i < 20; i++) {
            row = table.NewRow();
            row["ID"] = i;
            row["Item"] = "Item " + i;
            table.Rows.Add(row);
        }
    }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) {
        ASPxGridView gridView = (ASPxGridView)sender;
        int index = gridView.FocusedRowIndex;
        if((e.Parameters == "up" && index <= 0) || (e.Parameters == "down" && index >= gv.VisibleRowCount - 1)) return;
        int id = (int)gridView.GetRowValues(index, "ID");

        int nextIndex = e.Parameters == "up" ? --index : ++index;

        int nextId = (int)gridView.GetRowValues(nextIndex, "ID");

        DataRow dataRow = table.Rows.Find(id);
        dataRow["ID"] = int.MaxValue;

        DataRow nextDataRow = table.Rows.Find(nextId);
        nextDataRow["ID"] = id;

        dataRow["ID"] = nextId;

        gridView.DataBind();
        gridView.FocusedRowIndex = nextIndex;
    }
}