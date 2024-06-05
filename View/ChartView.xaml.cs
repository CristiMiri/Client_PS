using LiveCharts.Wpf;
using LiveCharts;
using System.Windows;
using Client.Services;
using Newtonsoft.Json;
using LiveCharts.Definitions.Charts;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for ChartView.xaml
    /// </summary>
    public partial class ChartView : Window
    {
        
        public ChartView()
        {
            InitializeComponent();
        }

        private async void ShowParticipantsByConferenceChartButton_Click(object sender, RoutedEventArgs e)
        {            
            var data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfParticipantsByConference",new { });
            Dictionary<string, int> parsedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.Data.ToString());
            var series = new ColumnSeries
            {
                Title = "Participants",
                Values = new ChartValues<int>(parsedData.Values)
            };
            this.CartesianChart.Visibility = Visibility.Visible;
            this.LineChart.Visibility = Visibility.Hidden;
            this.PieChart.Visibility = Visibility.Hidden;
            var chart = this.CartesianChart;
            chart.Series = new SeriesCollection { series };
            chart.AxisX[0].Labels = parsedData.Keys.ToArray();
            chart.AxisY[0].LabelFormatter = value => value.ToString("N");
        }

        private async void ShowParticipantsBySectionChartButton_Click(object sender, RoutedEventArgs e)
        {
            var data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfParticipantsBySection", new { });
            Dictionary<string, int> parsedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.Data.ToString());
            var series = new ColumnSeries
            {
                Title = "Presentations by Author",
                Values = new ChartValues<int>(parsedData.Values)
            };
            this.CartesianChart.Visibility = Visibility.Visible;
            this.LineChart.Visibility = Visibility.Hidden;
            this.PieChart.Visibility = Visibility.Hidden;
            var chart = this.CartesianChart;
            chart.Series = new SeriesCollection { series };
            chart.AxisX[0].Labels = parsedData.Keys.ToArray();
            chart.AxisY[0].LabelFormatter = value => value.ToString("N");
        }

        private async void ShowPresentationsPerDayChartButton_Click(object sender, RoutedEventArgs e)
        {
            var  data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfPresentationsPerDay", new { });
            Dictionary<DateTime, int> parsedData = JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(data.Data.ToString());
            var series = new LineSeries
            {
                Title = "Presentations per Day",
                Values = new ChartValues<int>(parsedData.Values)
            };

            var chart = this.CartesianChart;
            this.CartesianChart.Visibility = Visibility.Visible;
            this.LineChart.Visibility = Visibility.Hidden;
            this.PieChart.Visibility = Visibility.Hidden;
            chart.Series = new SeriesCollection { series };
            chart.AxisX[0].Labels = parsedData.Keys.Select(date => date.ToString("d")).ToArray();
            chart.AxisY[0].LabelFormatter = value => value.ToString("N");
        }

        private async void ShowPresentationsByAuthorChartButton_Click(object sender, RoutedEventArgs e)
        {
            var data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfPresentationsByAuthor", new { });
            Dictionary<string, int> parsedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.Data.ToString());

            var series = new LineSeries
            {
                Title = "Presentations per Day",
                Values = new ChartValues<int>(parsedData.Values)
            };

            var chart = this.CartesianChart;
            this.CartesianChart.Visibility = Visibility.Visible;
            this.LineChart.Visibility = Visibility.Hidden;
            this.PieChart.Visibility = Visibility.Hidden;

            chart.Series = new SeriesCollection { series };
            chart.AxisX[0].Labels = parsedData.Keys.Select(dateStr => DateTime.Parse(dateStr))
                                       .Select(date => date.ToString("d"))
                                       .ToArray();
            chart.AxisY[0].LabelFormatter = value => value.ToString("N");
        }

        private async void ShowLineChartDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfParticipantsBySection", new { });
            Dictionary<string, int> parsedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.Data.ToString());
            SeriesCollection seriesCollection = [];
            foreach (var item in parsedData)
            {
                seriesCollection.Add(new PieSeries
                {
                    Title = item.Key,
                    Values = new ChartValues<int> { item.Value },
                    DataLabels = true
                });
            }

            var pieChart = this.PieChart;
            this.CartesianChart.Visibility = Visibility.Hidden;
            this.LineChart.Visibility = Visibility.Hidden;
            this.PieChart.Visibility = Visibility.Visible;

            // Clear existing series to avoid potential issues

            pieChart.Series = new SeriesCollection();
            pieChart.Series.AddRange(seriesCollection);
        }

        private async void ShowRingChartButton_Click(object sender, RoutedEventArgs e)
        {
            var data = await ClientHost.Instance.RequestStatisticsService("GetNumberOfParticipantsBySection", new { });
            var parsedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data.Data.ToString());
            var series = new LineSeries
            {
                Title = "Presentations per Day",
                Values = new ChartValues<int>(parsedData.Values)
            };

            var lineChart = this.LineChart;
            this.CartesianChart.Visibility = Visibility.Hidden;
            this.LineChart.Visibility = Visibility.Visible;
            this.PieChart.Visibility = Visibility.Hidden;
            lineChart.Series = new SeriesCollection { series };
            lineChart.AxisX[0].Labels = parsedData.Keys.Select(dateStr => DateTime.Parse(dateStr))
                                       .Select(date => date.ToString("D"))
                                       .ToArray();
            lineChart.AxisY[0].LabelFormatter = value => value.ToString("N");
        }
    }
}
