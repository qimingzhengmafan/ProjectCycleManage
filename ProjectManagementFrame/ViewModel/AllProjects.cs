using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFrame.ViewModel
{
    public class AllProjects
    {
        // you can convert any array, list or IEnumerable<T> to a pie series collection:
        public IEnumerable<ISeries> Series { get; set; } =
            new[] { 2, 4, 1, 4, 3 }.AsPieSeries();

        public IEnumerable<ISeries> Series2 { get; set; } = new List<ISeries>
        {
            new PieSeries<int> { Values = new List<int> { 2 } },
            new PieSeries<int> { Values = new List<int> { 4 } },
            new PieSeries<int> { Values = new List<int> { 1 } },
            new PieSeries<int> { Values = new List<int> { 4 } },
            new PieSeries<int> { Values = new List<int> { 3 } },
        };

        public LabelVisual Title { get; set; } =
            new LabelVisual
            {
                Text = "My chart title",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15)
            };
    }
}
