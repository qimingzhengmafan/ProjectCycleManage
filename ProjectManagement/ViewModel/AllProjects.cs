using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Page_Navigation_App.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModel
{
    public class AllProjects:ViewModelBase
    {

        private IEnumerable<ISeries> _series;
        public IEnumerable<ISeries> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "My chart title",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15)
            };


        public AllProjects()
        {
            SolidColorPaint TextPaint = new SolidColorPaint()
            {
                Color = SKColors.DarkSlateGray,
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

            SolidColorPaint TextPaint2 = new SolidColorPaint()
            {
                Color = SKColors.DarkSlateGray,
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

        }

        public SolidColorPaint LegendTextPaint { get; set; } =
            new SolidColorPaint
            {
                Color = new SKColor(25, 26, 27),
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

        /// <summary>
        /// 提示文本样式
        /// </summary>
        public SolidColorPaint TipTextPaint { get; set; }
         = new SolidColorPaint()
         {
             Color = SKColors.DarkSlateGray,
             SKTypeface = SKFontManager.Default.MatchCharacter('汉')
         };

    }
}
