using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.ViewModel
{
    public partial class DataChartVM: ObservableObject
    {
        public SolidColorPaint LegendTextPaint { get; set; } =
            new SolidColorPaint
            {
                Color = new SKColor(51, 51, 51),
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

        public SolidColorPaint TipTextPaint { get; set; }
         = new SolidColorPaint()
         {
             Color = SKColors.DarkSlateGray,
             SKTypeface = SKFontManager.Default.MatchCharacter('汉')
         };
        public ISeries[] Series { get; set; } =
        [
            new ColumnSeries<double>
            {
                Name = "预算金额",
                Values = [2000, 5000, 4000, 5000, 5000, 5000],
                MaxBarWidth = 30,
                Fill = new SolidColorPaint(new SKColor(76, 172, 250)),
            },
            new ColumnSeries<double>
            {
                Name = "实际支出",
                Values = [3000, 1000, 6000, 5000, 5000, 5000],
                MaxBarWidth = 30,
                Fill = new SolidColorPaint(new SKColor(109, 203, 112)),
            }
        ];

        public Axis[] XAxes { get; set; } =
        [
            new Axis
            {
                Labels = ["测试 1", "Category 2", "Category 3","Category 4", "Category 5", "Category 6","Category 7", "Category 8", "Category 9"],
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                TicksAtCenter = true,
                // By default the axis tries to optimize the number of 
                // labels to fit the available space, 
                // when you need to force the axis to show all the labels then you must: 
                ForceStepToMin = true,
                MinStep = 1,
                LabelsPaint = new SolidColorPaint(SKColors.Black){SKTypeface = SKFontManager.Default.MatchCharacter('汉')}, // 标签颜色
            }
        ];
    }
}
