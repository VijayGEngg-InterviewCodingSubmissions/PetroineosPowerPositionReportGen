using System.Collections.Frozen;

namespace PITL.PowerTraders.Models.DomainModels
{
    public static class StaticMappings
    {
        public static FrozenDictionary<int, TimeOnly> PeriodToHourMapping =
            new Dictionary<int, TimeOnly>
            {
                [1] = new TimeOnly(23, 0),
                [2] = new TimeOnly(0, 0),
                [3] = new TimeOnly(1, 0),
                [4] = new TimeOnly(2, 0),
                [5] = new TimeOnly(3, 0),
                [6] = new TimeOnly(4, 0),
                [7] = new TimeOnly(5, 0),
                [8] = new TimeOnly(6, 0),
                [9] = new TimeOnly(7, 0),
                [10] = new TimeOnly(8, 0),
                [11] = new TimeOnly(9, 0),
                [12] = new TimeOnly(10, 0),
                [13] = new TimeOnly(11, 0),
                [14] = new TimeOnly(12, 0),
                [15] = new TimeOnly(13,00),
                [16] = new TimeOnly(14, 0),
                [17] = new TimeOnly(15, 0),
                [18] = new TimeOnly(16, 0),
                [19] = new TimeOnly(17, 0),
                [20] = new TimeOnly(18, 0),
                [21] = new TimeOnly(19, 0),
                [22] = new TimeOnly(20, 0),
                [23] = new TimeOnly(21, 0),
                [24] = new TimeOnly(22, 0)
            }.ToFrozenDictionary();

        public static FrozenDictionary<TimeOnly, int> HourToPeriodMapping = 
            new Dictionary<TimeOnly, int>
            {
                [new TimeOnly(23, 0)] = 1,
                [new TimeOnly(0, 0)] = 2,
                [new TimeOnly(1, 0)] = 3,
                [new TimeOnly(2, 0)] = 4,
                [new TimeOnly(3, 0)] = 5,
                [new TimeOnly(4, 0)] = 6,
                [new TimeOnly(5, 0)] = 7,
                [new TimeOnly(6, 0)] = 8,
                [new TimeOnly(7, 0)] = 9,
                [new TimeOnly(8, 0)] = 10,
                [new TimeOnly(9, 0)] = 11,
                [new TimeOnly(10, 0)] = 12,
                [new TimeOnly(11, 0)] = 13,
                [new TimeOnly(12, 0)] = 14,
                [new TimeOnly(13, 0)] = 15,
                [new TimeOnly(14, 0)] = 16,
                [new TimeOnly(15, 0)] = 17,
                [new TimeOnly(16, 0)] = 18,
                [new TimeOnly(17, 0)] = 19,
                [new TimeOnly(18, 0)] = 20,
                [new TimeOnly(19, 0)] = 21,
                [new TimeOnly(20, 0)] = 22,
                [new TimeOnly(21, 0)] = 23,
                [new TimeOnly(22, 0)] = 24,
            }.ToFrozenDictionary();
    }
}