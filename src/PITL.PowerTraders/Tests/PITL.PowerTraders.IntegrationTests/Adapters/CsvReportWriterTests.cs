using Microsoft.Extensions.Logging;
using PITL.PowerTraders.CsvGeneratorService;
using PITL.PowerTraders.IntegrationTests.BaseClasses;
using PITL.PowerTraders.Interfaces;
using PITL.PowerTraders.Models.DomainModels;

namespace PITL.PowerTraders.IntegrationTests.Adapters
{
    public class CsvReportWriterTests : BaseIntegrationTest
    {
        private ICsvReportWriter _sut;

        public CsvReportWriterTests(): base()
        {
            ILogger<CsvReportWriter> _sutLogger = LoggerFactory.CreateLogger<CsvReportWriter>();
            _sut = new CsvReportWriter(_sutLogger);
        }

        [Fact]
        public async Task CsvReportWriterTests_ServiceInstancetiatedProperly()
        {
            Assert.NotNull(_sut);
            Assert.IsType<CsvReportWriter>(_sut);
        }

        [Fact]
        public async Task CsvReportWriterTests_WriteSampleFileWithSampleData_Positive()
        {
            Assert.NotNull(_sut);
            Assert.IsType<CsvReportWriter>(_sut);
            List<PeriodVolumeDTO> periodVolumeDTOs = Enumerable.Range(1, 10)
                .Select(id => new PeriodVolumeDTO
                {
                    LocalTime = new TimeOnly(23, 00),
                    Volume = new Random().NextDouble()
                })
                .ToList();

            string sampleGenFileName = "dummy.csv";
            
            var returnVal = _sut.GeneratePeriodVolumeCsvFile(sampleGenFileName, periodVolumeDTOs);

            Assert.True(returnVal.IsSuccess);
            Assert.False(string.IsNullOrWhiteSpace(returnVal.FilePath));
            Assert.True(Directory.Exists(returnVal.FilePath));
            Assert.False(string.IsNullOrWhiteSpace(returnVal.FileName));
            Assert.True(string.Equals(returnVal.FileName, sampleGenFileName, StringComparison.InvariantCultureIgnoreCase));
            string fullFilePath = Path.Combine(returnVal.FilePath, returnVal.FileName);
            Assert.True(File.Exists(fullFilePath));
        }

        [Fact]
        public async Task CsvReportWriterTests_DoesNOT_GenerateCsvFile_If_EmptyDataList_Sent()
        {
            Assert.NotNull(_sut);
            Assert.IsType<CsvReportWriter>(_sut);
            List<PeriodVolumeDTO> periodVolumeDTOs = new List<PeriodVolumeDTO>();

            string sampleGenFileName = "not_generated_file.csv";

            var returnVal = _sut.GeneratePeriodVolumeCsvFile(sampleGenFileName, periodVolumeDTOs);

            Assert.False(returnVal.IsSuccess);
            Assert.True(string.IsNullOrWhiteSpace(returnVal.FilePath));
            Assert.True(string.IsNullOrWhiteSpace(returnVal.FileName));
            Assert.True(string.Equals(returnVal.FileName, string.Empty, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task CsvReportWriterTests_DoesNOT_GenerateCsvFile_If_NullDataList_Sent()
        {
            Assert.NotNull(_sut);
            Assert.IsType<CsvReportWriter>(_sut);
            List<PeriodVolumeDTO> periodVolumeDTOs = null;

            string sampleGenFileName = "not_generated_file.csv";

            var returnVal = _sut.GeneratePeriodVolumeCsvFile(sampleGenFileName, periodVolumeDTOs);

            Assert.False(returnVal.IsSuccess);
            Assert.True(string.IsNullOrWhiteSpace(returnVal.FilePath));
            Assert.True(string.IsNullOrWhiteSpace(returnVal.FileName));
            Assert.True(string.Equals(returnVal.FileName, string.Empty, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
