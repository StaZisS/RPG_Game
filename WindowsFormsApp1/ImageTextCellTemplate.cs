using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class ImageTextCellTemplate : DataGridViewCell
        {
            public ImageTextCellTemplate()
            {
                Style.BackColor = Color.Black;
                Style.ForeColor = Color.White;
                Style.SelectionBackColor = Color.Black;
                Style.SelectionForeColor = Color.White;
            }

            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);

                if (value is ImageTextData data)
                {
                    graphics.FillRectangle(Brushes.Gray, cellBounds);
                    if (data.Image != null)
                    {
                        graphics.DrawImage(data.Image, cellBounds);
                    }

                    if (!string.IsNullOrEmpty(data.Text))
                    {
                        SizeF textSize = graphics.MeasureString(data.Text, cellStyle.Font);

                        float textX = cellBounds.Right - textSize.Width - 15;
                        float textY = cellBounds.Bottom - textSize.Height - 10;

                        Font largerFont = new Font(cellStyle.Font.FontFamily, 16);
                        graphics.DrawString(data.Text, largerFont, new SolidBrush(cellStyle.ForeColor), textX, textY);

                    }
                }

                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                {

                    Color borderColor = Color.Black;
                    int borderWidth = 4;
                    DashStyle borderStyle = DashStyle.Solid;

                    if (cellState.HasFlag(DataGridViewElementStates.Selected))
                    {
                        borderColor = Color.Red;
                        borderWidth = 6;
                    }

                    using (Pen borderPen = new Pen(borderColor, borderWidth))
                    {
                        borderPen.DashStyle = borderStyle;
                        graphics.DrawLine(borderPen, cellBounds.Left, cellBounds.Top, cellBounds.Right, cellBounds.Top);
                        graphics.DrawLine(borderPen, cellBounds.Left, cellBounds.Bottom, cellBounds.Right,
                            cellBounds.Bottom);
                        graphics.DrawLine(borderPen, cellBounds.Left, cellBounds.Top, cellBounds.Left,
                            cellBounds.Bottom);
                        graphics.DrawLine(borderPen, cellBounds.Right, cellBounds.Top, cellBounds.Right,
                            cellBounds.Bottom);
                    }
                }

            }

            public override object Clone()
            {
                var clone = (ImageTextCellTemplate)base.Clone();
                return clone;
            }

            public override Type ValueType => typeof(ImageTextData);

            public override object DefaultNewRowValue => new ImageTextData();

            public override bool ReadOnly => true;
            
            protected override void OnMouseEnter(int rowIndex)
            {
                base.OnMouseEnter(rowIndex);
                var cellValue = DataGridView.Rows[rowIndex].Cells[ColumnIndex].Value;
                if (cellValue is ImageTextData imageTextData && !string.IsNullOrEmpty(imageTextData.Description))
                {
                    ToolTipText = imageTextData.Description;
                }
            }

            protected override void OnMouseLeave(int rowIndex)
            {
                base.OnMouseLeave(rowIndex);
                ToolTipText = null;
            }
        }
}