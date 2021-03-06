using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uit.hotel.Models;

namespace uit.hotel.DataAccesses
{
    public class RoomKindDataAccess : RealmDatabase
    {
        public static int NextId => Get().Count() == 0 ? 1 : Get().Max(i => i.Id) + 1;

        public static async Task<RoomKind> Add(RoomKind roomKind)
        {
            await Database.WriteAsync(realm =>
            {
                roomKind.Id = NextId;
                roomKind.IsActive = true;

                var price = new Price
                {
                    HourPrice = 0,
                    DayPrice = 0,
                    NightPrice = 0,
                    WeekPrice = 0,
                    MonthPrice = 0,
                    LateCheckOutFee = 0,
                    EarlyCheckInFee = 0,
                    EffectiveStartDate = DateTimeOffset.MinValue,
                    Employee = null,
                    RoomKind = roomKind
                };
                PriceDataAccess.Add(realm, price);

                roomKind = realm.Add(roomKind);
            });
            return roomKind;
        }

        public static async Task<RoomKind> Update(RoomKind roomKindInDatabase, RoomKind roomKind)
        {
            await Database.WriteAsync(realm =>
            {
                roomKindInDatabase.Name = roomKind.Name;
                roomKindInDatabase.NumberOfBeds = roomKind.NumberOfBeds;
                roomKindInDatabase.AmountOfPeople = roomKind.AmountOfPeople;
            });
            return roomKindInDatabase;
        }

        internal static async void SetIsActive(RoomKind roomKind, bool isActive)
        {
            await Database.WriteAsync(realm => roomKind.IsActive = isActive);
        }

        public static async void Delete(RoomKind roomKind)
        {
            await Database.WriteAsync(realm =>
            {
                realm.RemoveRange(roomKind.PriceVolatilities);
                realm.RemoveRange(roomKind.Prices);
                realm.Remove(roomKind);
            });
        }

        public static RoomKind Get(int roomKindId) => Database.Find<RoomKind>(roomKindId);

        public static IEnumerable<RoomKind> Get() => Database.All<RoomKind>();
    }
}
