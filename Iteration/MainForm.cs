using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Gauss {
    public partial class MainForm : Form {

        public MainForm() {
            InitializeComponent();
        }

        private string DefaultText = String.Format("{0:f}", 0.0);

        // ������� ��������� � ��������� ��� ��� ����� ��������
        private TextBox InitTextBox(bool readOnly) {
            TextBox textBox = new TextBox();
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox.Text = DefaultText;
            textBox.ReadOnly = readOnly;
            if (!readOnly) {
                textBox.CausesValidation = true;
                textBox.Validating += ValidateTextBox;
            }
            return textBox;
        }

        // �������� ���������� ������ � ���������
        private void ValidateTextBox(object sender, CancelEventArgs e) {
            TextBox textBox = (TextBox)sender;
            double result;
            e.Cancel = !double.TryParse(textBox.Text, out result);
        }

        // ������ ��������� ������ ���������, �������� ������ � ������� ������������
        private TextBox[,] InitTextBoxMatrix(TableLayoutPanel layoutPanel, int count, bool readOnly) {
            layoutPanel.SuspendLayout();

            layoutPanel.Controls.Clear();

            layoutPanel.ColumnStyles.Clear();
            layoutPanel.ColumnCount = count;

            layoutPanel.RowStyles.Clear();
            layoutPanel.RowCount = count;

            TextBox[,] result = new TextBox[count, count];
            float cellSize = 1f / count * 100f;

            for (int col = 0; col < count; ++col) {
                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellSize));
                for (int row = 0; row < count; ++row) {
                    layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, cellSize));

                    TextBox textBox = InitTextBox(readOnly);

                    layoutPanel.Controls.Add(textBox, col, row);
                    result[col, row] = textBox;
                }
            }

            layoutPanel.ResumeLayout(true);

            return result;
        }

        // ������ ���������� ������ ���������, �������� ������ ������� ������������
        private TextBox[] InitTextBoxArray(TableLayoutPanel layoutPanel, int count, bool readOnly) {
            layoutPanel.SuspendLayout();

            layoutPanel.Controls.Clear();

            layoutPanel.ColumnStyles.Clear();
            layoutPanel.ColumnCount = 1;

            layoutPanel.RowStyles.Clear();
            layoutPanel.RowCount = count;

            TextBox[] result = new TextBox[count];
            float cellSize = 1f / count * 100f;

            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            for (int row = 0; row < count; ++row) {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, cellSize));

                TextBox textBox = InitTextBox(readOnly);

                layoutPanel.Controls.Add(textBox, 0, row);
                result[row] = textBox;
            }

            layoutPanel.ResumeLayout(true);

            return result;
        }

        private int n;
        private TextBox[,] matrixA;
        private TextBox[] vectorB;
        private TextBox[] vectorX;

        private void InitMatrixA() {
            matrixA = InitTextBoxMatrix(layoutMatrixA, n, false);
        }

        private void InitVectorX() {
            vectorX = InitTextBoxArray(layoutVectorX, n, true);
        }

        private void InitVectorB() {
            vectorB = InitTextBoxArray(layoutVectorB, n, false);
        }

        public int N {
            get { return n; }
            set {
                if (value != n && value > 0) {
                    n = value;
                    InitMatrixA();
                    InitVectorX();
                    InitVectorB();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            N = (int)numericUpDown1.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            N = (int)numericUpDown1.Value;
        }

        private void button1_Click(object sender, EventArgs e) {
            if (Validate()) {
                try {
                    var system = new LinearSystem(MatrixA, VectorB);
                    VectorX = system.XVector;
                } catch (Exception error) {
                    MessageBox.Show(error.Message);
                }
            }
        }

        public double[,] MatrixA {
            get {
                // �������� �������� ������������� ������� A
                double[,] matrix_a = new double[n, n];
                for (int i = 0; i < n; ++i)
                    for (int j = 0; j < n; ++j)
                        matrix_a[i, j] = double.Parse(matrixA[j, i].Text);
                return matrix_a;
            }
            set {
                // ���������� � ���������� ������� A
                for (int i = 0; i < n; ++i)
                    for (int j = 0; j < n; ++j)
                        matrixA[j, i].Text = value[i, j].ToString("f");
            }
        }

        public double[] VectorB {
            get {
                // �������� �������� ������������� ������ B
                double[] vector_b = new double[n];
                for (int j = 0; j < n; ++j)
                    vector_b[j] = double.Parse(vectorB[j].Text);
                return vector_b;
            }
            set {
                // ���������� � ���������� ������ B
                for (int j = 0; j < n; ++j)
                    vectorB[j].Text = value[j].ToString("f");
            }
        }

        public double[] VectorX {
            set {
                // ���������� ����������� ��������� X
                for (int j = 0; j < n; ++j)
                    vectorX[j].Text = value[j].ToString("f");
            }
        }

    }
}