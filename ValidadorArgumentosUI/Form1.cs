using System;
using System.Drawing;
using System.Windows.Forms;

namespace ValidadorArgumentosUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Validador de Argumentos | Lógica Proposicional";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1150, 700);
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.White;

            Controls.Clear();

            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                ColumnCount = 2,
                RowCount = 2
            };
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 370));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(main);

            // Header
            var header = new Panel { Dock = DockStyle.Fill };
            var title = new Label
            {
                Text = "Validador de Argumentos (Lógica Proposicional)",
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 25)
            };
            var subtitle = new Label
            {
                Text = "Ingresa premisas y conclusión. Genera la tabla de verdad y revisa renglones críticos.",
                Dock = DockStyle.Top,
                Height = 24,
                ForeColor = Color.FromArgb(90, 90, 90)
            };
            header.Controls.Add(subtitle);
            header.Controls.Add(title);

            main.Controls.Add(header, 0, 0);
            main.SetColumnSpan(header, 2);

            // LEFT
            var left = new Panel { Dock = DockStyle.Fill };
            main.Controls.Add(left, 0, 1);

            var grpEntrada = new GroupBox { Text = "Entrada", Dock = DockStyle.Top, Height = 360, Padding = new Padding(12) };
            left.Controls.Add(grpEntrada);

            var lblPrem = new Label { Text = "Premisas (una por línea):", Dock = DockStyle.Top, Height = 22 };
            var txtPrem = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Top, Height = 190, BorderStyle = BorderStyle.FixedSingle };

            var spacer1 = new Panel { Dock = DockStyle.Top, Height = 10 };

            var lblConcl = new Label { Text = "Conclusión:", Dock = DockStyle.Top, Height = 22 };
            var txtConcl = new TextBox { Dock = DockStyle.Top, Height = 28, BorderStyle = BorderStyle.FixedSingle };

            grpEntrada.Controls.Add(txtConcl);
            grpEntrada.Controls.Add(lblConcl);
            grpEntrada.Controls.Add(spacer1);
            grpEntrada.Controls.Add(txtPrem);
            grpEntrada.Controls.Add(lblPrem);

            var grpAcc = new GroupBox { Text = "Acciones", Dock = DockStyle.Top, Height = 160, Padding = new Padding(12) };
            left.Controls.Add(grpAcc);

            var rowBtns = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, WrapContents = false };
            var btnGen = new Button { Text = "Generar tabla", Width = 150, Height = 36, BackColor = Color.FromArgb(30, 90, 200), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnClr = new Button { Text = "Limpiar", Width = 150, Height = 36, BackColor = Color.White, ForeColor = Color.FromArgb(40, 40, 40), FlatStyle = FlatStyle.Flat };
            rowBtns.Controls.Add(btnGen);
            rowBtns.Controls.Add(btnClr);

            var chk1 = new CheckBox { Text = "Resaltar renglones críticos", Dock = DockStyle.Top, Checked = true };
            var chk2 = new CheckBox { Text = "Mostrar solo renglones críticos", Dock = DockStyle.Top, Checked = false };

            grpAcc.Controls.Add(chk2);
            grpAcc.Controls.Add(chk1);
            grpAcc.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 10 });
            grpAcc.Controls.Add(rowBtns);

            // RIGHT
            var right = new Panel { Dock = DockStyle.Fill };
            main.Controls.Add(right, 1, 1);

            var banner = new Panel { Dock = DockStyle.Top, Height = 42, Padding = new Padding(8), BackColor = Color.FromArgb(245, 247, 250) };
            var lblEstado = new Label { Text = "Estado: —", Dock = DockStyle.Left, Width = 260, Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 80, 80), TextAlign = ContentAlignment.MiddleLeft };
            var lblCounts = new Label { Text = "Filas: —    Críticos: —", Dock = DockStyle.Fill, ForeColor = Color.FromArgb(90, 90, 90), TextAlign = ContentAlignment.MiddleLeft };
            banner.Controls.Add(lblCounts);
            banner.Controls.Add(lblEstado);
            right.Controls.Add(banner);

            var grpTabla = new GroupBox { Text = "Tabla de verdad", Dock = DockStyle.Fill, Padding = new Padding(10) };
            right.Controls.Add(grpTabla);

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                GridColor = Color.FromArgb(220, 220, 220)
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            grid.EnableHeadersVisualStyles = false;

            // placeholder columnas/filas
            grid.Columns.Add("p", "p");
            grid.Columns.Add("q", "q");
            grid.Columns.Add("Prem1", "Premisa1");
            grid.Columns.Add("Conc", "Conclusión");
            for (int i = 0; i < 6; i++)
                grid.Rows.Add(i % 2 == 0 ? "V" : "F", i % 3 == 0 ? "V" : "F", "V", "F");

            grpTabla.Controls.Add(grid);

            var grpLog = new GroupBox { Text = "Mensajes / Errores", Dock = DockStyle.Bottom, Height = 170, Padding = new Padding(10) };
            right.Controls.Add(grpLog);

            var log = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BorderStyle = BorderStyle.FixedSingle };
            log.Text = "✅ Interfaz lista. Falta conectar lógica.";
            grpLog.Controls.Add(log);
        }
    }
}