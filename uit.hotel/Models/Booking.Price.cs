using System;
using Realms;
using uit.hotel.Queries.Helper;

namespace uit.hotel.Models
{
    public partial class Booking : RealmObject
    {
        //? Calculated fields
        public DateTimeOffset BaseCheckInTime { get; private set; }
        public DateTimeOffset BaseCheckOutTime { get; private set; }
        public Price Price { get; private set; }
        public long HourPrice { get; private set; }
        public long NightPrice { get; private set; }
        public long DayPrice { get; private set; }
        public long EarlyCheckInFee { get; private set; }
        public long LateCheckOutFee { get; private set; }

        public void CalculatePrice()
        {
            BaseCheckInTime = (RealCheckInTime != DateTimeOffset.MinValue ? RealCheckInTime : BookCheckInTime).Round();
            BaseCheckOutTime = (RealCheckOutTime != DateTimeOffset.MinValue ? RealCheckOutTime : BookCheckOutTime).Round();

            Price = Room.RoomKind.GetPrice(BaseCheckInTime);
            HourPrice = 0;
            EarlyCheckInFee = 0;
            LateCheckOutFee = 0;
            NightPrice = 0;
            DayPrice = 0;

            CalculateHour();
            if (HourPrice == 0)
            {
                CalculateCheckIn();
                CalculateCheckOut();
                CalculateNight();
                CalculateDay();
            }

            TotalPrice = HourPrice + EarlyCheckInFee + LateCheckOutFee + NightPrice + DayPrice;
        }

        private void CalculateHour()
        {
            var timeSpan = BaseCheckOutTime - BaseCheckInTime;
            if (timeSpan.FloatHour() <= Price.HourTimeSpan)
            {
                HourPrice = (long)(Price.HourPrice * timeSpan.FloatHour());
            }
        }

        private void CalculateCheckIn()
        {
            var checkInHour = BaseCheckInTime.FloatHour();
            if (checkInHour <= Price.MaxCheckInNightTime)
            {
                BaseCheckInTime = BaseCheckInTime.AtHour(Price.CheckInNightTime).AddDays(-1);
            }
            else if (checkInHour <= Price.CheckInDayTime)
            {
                EarlyCheckInFee = (long)(Price.EarlyCheckInFee * (Price.CheckInDayTime - checkInHour));
                BaseCheckInTime = BaseCheckInTime.AtHour(Price.CheckInDayTime);
            }
            else if (checkInHour <= Price.CheckInNightTime - Price.ToleranceTimeSpan)
            {
                BaseCheckInTime.AtHour(Price.CheckInDayTime);
            }
            else if (checkInHour <= Price.CheckInNightTime)
            {
                EarlyCheckInFee = (long)(Price.EarlyCheckInFee * (Price.CheckInNightTime - checkInHour));
                BaseCheckInTime = BaseCheckInTime.AtHour(Price.CheckInNightTime);
            }
            else
            {
                BaseCheckInTime = BaseCheckInTime.AtHour(Price.CheckInNightTime);
            }
        }

        private void CalculateCheckOut()
        {
            var checkOutTime = BaseCheckOutTime.FloatHour();
            if (checkOutTime <= Price.CheckOutNightTime)
            {
                BaseCheckOutTime = BaseCheckOutTime.AtHour(Price.CheckOutNightTime);
            }
            else if (checkOutTime <= Price.CheckInDayTime + Price.ToleranceTimeSpan)
            {
                BaseCheckOutTime = BaseCheckOutTime.AtHour(Price.CheckInNightTime);
                LateCheckOutFee = (long)(Price.LateCheckOutFee * (checkOutTime - Price.CheckOutNightTime));
            }
            else
            {
                BaseCheckOutTime = BaseCheckOutTime.AtHour(Price.CheckOutDayTime).AddDays(1);
            }
        }

        private void CalculateNight()
        {
            if (BaseCheckInTime.FloatHour() == Price.CheckInNightTime) // isNight
            {
                NightPrice = Price.NightPrice;
                BaseCheckInTime = BaseCheckInTime.AtHour(Price.CheckInDayTime).AddDays(1);
            }
        }

        private void CalculateDay()
        {
            var itepriceTime = BaseCheckInTime;
            while (itepriceTime <= BaseCheckOutTime)
            {
                var remain = (BaseCheckOutTime - itepriceTime).Days;
                if (remain >= 30 && Price.MonthPrice != 0)
                {
                    DayPrice += Price.MonthPrice;
                    itepriceTime = itepriceTime.AddDays(30);
                }
                else if (remain >= 7 && Price.WeekPrice != 0)
                {
                    DayPrice += Price.WeekPrice;
                    itepriceTime = itepriceTime.AddDays(7);
                }
                else
                {
                    DayPrice += Price.DayPrice;
                    itepriceTime = itepriceTime.AddDays(1);
                }
            }
        }
    }
}
