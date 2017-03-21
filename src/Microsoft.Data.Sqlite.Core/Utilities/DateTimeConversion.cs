// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    using System;

    internal static class DateTimeConversion
    {
        // Computes DateTime from julian date. This function is a port of the
        // computeYMD and computeHMS functions from the original Sqlite core
        // source code in 'date.c'.
        public static DateTime FromJulianDate(double julianDate)
        {
            // computeYMD
            long iJD = (long)(julianDate * 86400000.0 + 0.5);
            int Z = (int)((iJD + 43200000) / 86400000);
            int A = (int)((Z - 1867216.25) / 36524.25);
            A = Z + 1 + A - (A / 4);
            int B = A + 1524;
            int C = (int)((B - 122.1) / 365.25);
            int D = (36525 * (C & 32767)) / 100;
            int E = (int)((B - D) / 30.6001);
            int X1 = (int)(30.6001 * E);
            int day = B - D - X1;
            int month = E < 14 ? E - 1 : E - 13;
            int year = month > 2 ? C - 4716 : C - 4715;

            // computeHMS
            int s = (int)((iJD + 43200000) % 86400000);
            double fracSecond = s / 1000.0;
            s = (int)fracSecond;
            fracSecond -= s;
            int hour = s / 3600;
            s -= hour * 3600;
            int minute = s / 60;
            fracSecond += s - minute * 60;

            int second = (int)fracSecond;
            int millisecond = (int)Math.Round((fracSecond - second) * 1000.0);

            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
    }
}
