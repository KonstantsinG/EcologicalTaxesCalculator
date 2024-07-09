using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CalculatorTools.Utilities
{
    public static class TableInstancesFactory
    {
        public static void ReplaceParagraphToTextBox(ref TableRow row)
        {
            TableCell cell;
            
            for (int i = 0; i < row.Cells.Count; i++)
            {
                cell = row.Cells[i];
                ReplaceParagraphToTextBox(ref cell);
            }
        }

        public static void ReplaceParagraphToTextBox(ref TableCell cell)
        {
            Paragraph p;
            Run run;
            BlockUIContainer ui;
            TextBox box;

            if (cell.Blocks.ElementAt(0).GetType() == typeof(Paragraph))
            {
                p = (Paragraph)cell.Blocks.ElementAt(0);
                run = (Run)p.Inlines.ElementAt(0);
                cell.Blocks.Remove(p);

                box = new TextBox();
                ui = new BlockUIContainer();
                box.Text = run.Text;
                box.FontWeight = run.FontWeight;
                ui.Child = box;
                cell.Blocks.Add(ui);
            }
        }

        public static void ReplaceTextBoxToParagraph(ref TableRow row)
        {
            TableCell cell;

            for (int i = 0; i < row.Cells.Count; i++)
            {
                cell = row.Cells[i];
                ReplaceTextBoxToParagraph(ref cell);
            }
        }

        public static void ReplaceTextBoxToParagraph(ref TableCell cell)
        {
            BlockUIContainer ui;
            TextBox tBox;
            Paragraph p;
            Run run;

            ui = cell.Blocks.ElementAt(0) as BlockUIContainer;
            if (ui.Child.GetType() == typeof(TextBox))
            {
                tBox = (TextBox)ui.Child;
                cell.Blocks.Remove(ui);

                p = new Paragraph();
                run = new Run();
                run.Text = (ui.Child as TextBox).Text;
                run.FontWeight = tBox.FontWeight;
                p.Inlines.Add(run);
                cell.Blocks.Add(p);
            }
        }

        public static TableRow ConstructSaveRow(int skipCount, int continueCount, RoutedEventHandler doneClickHandler, RoutedEventHandler cancelClickHandler)
        {
            TableRow row = new TableRow();
            TableCell cell;
            BlockUIContainer ui = new BlockUIContainer();
            Button btn = new Button();

            for (int i = 0; i < skipCount; i++)
            {
                cell = new TableCell();
                cell.Background = new SolidColorBrush(Colors.LightGray);
                row.Cells.Add(cell);
            }

            cell = new TableCell();
            btn.Content = "Сохранить";
            btn.Background = new SolidColorBrush(Colors.LightGreen);
            btn.Click += doneClickHandler;
            ui.Child = btn;
            cell.Blocks.Add(ui);
            row.Cells.Add(cell);

            cell = new TableCell();
            btn = new Button();
            ui = new BlockUIContainer();
            btn.Content = "Отмена";
            btn.Background = new SolidColorBrush(Colors.PaleVioletRed);
            btn.Click += cancelClickHandler;
            ui.Child = btn;
            cell.Blocks.Add(ui);
            row.Cells.Add(cell);

            for (int i = 0; i < continueCount; i++)
            {
                cell = new TableCell();
                cell.Background = new SolidColorBrush(Colors.LightGray);
                row.Cells.Add(cell);
            }

            return row;
        }

        public static TableCell ConstructCell(string value, string[] extraProps = null, bool editable = false)
        {
            TableCell cell = new TableCell();

            if (!editable)
            {
                Paragraph p = new Paragraph();
                Run run = new Run();
                run.Text = value;

                if (extraProps != null)
                {
                    if (extraProps.Contains("bold")) run.FontWeight = FontWeights.Bold;
                }

                p.Inlines.Add(run);
                cell.Blocks.Add(p);
            }
            else
            {
                BlockUIContainer ui = new BlockUIContainer();
                TextBox tBox = new TextBox();
                tBox.Text = value;

                if (extraProps != null)
                {
                    if (extraProps.Contains("bold")) tBox.FontWeight = FontWeights.Bold;
                }

                ui.Child = tBox;
                cell.Blocks.Add(ui);
            }
           
            if (extraProps != null)
            {
                if (extraProps.Contains("padding")) cell.Padding = new Thickness(5);
                if (extraProps.Contains("borders")) cell.BorderBrush = new SolidColorBrush(Colors.Black); cell.BorderThickness = new Thickness(0, 1, 1, 0);
                if (extraProps.Contains("bgHeader")) cell.Background = new SolidColorBrush(Colors.LightGray);
                if (extraProps.Contains("bgAttention")) cell.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
                if (extraProps.Contains("center")) cell.TextAlignment = TextAlignment.Center;
            }

            return cell;
        }

        public static string[] CheckForDots(params string[] strs)
        {
            for (int i = 0; i < strs.Count(); i++)
            {
                if (strs[i].Contains("."))
                    strs[i] = strs[i].Replace(".", ",");
            }

            return strs;
        }

        public static TableRow ConstructEmptyRow(int cellsCount)
        {
            TableRow row = new TableRow();

            for (int i = 0; i < cellsCount; i++)
                row.Cells.Add(ConstructCell("", new string[] { "borders", "padding" }));

            row.Cells[cellsCount - 1].BorderThickness = new Thickness(0,1,0,0);

            return row;
        }

        public static string FormatNumber(double number, int decimalPlaces)
        {
            if (Math.Abs(number) < Math.Pow(10, -decimalPlaces) || Math.Abs(number) >= Math.Pow(10, decimalPlaces))
            {
                string format = "0." + new string('#', decimalPlaces) + "E+00";
                return number.ToString(format);
            }
            else
            {
                return Math.Round(number, decimalPlaces).ToString();
            }
        }
    }
}
