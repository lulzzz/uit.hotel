﻿using System;
using System.Linq;
using Realms;
using uit.hotel.Businesses;

namespace uit.hotel.Models
{
    public class Bill : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTimeOffset Time { get; set; }
        public Patron Patron { get; set; }
        public Employee Employee { get; set; }

        [Backlink(nameof(Receipt.Bill))]
        public IQueryable<Receipt> Receipts { get; }

        [Backlink(nameof(Booking.Bill))]
        public IQueryable<Booking> Bookings { get; }

        public long Total { get; private set; }
        public long TotalReceipts { get; private set; }

        public void Calculate()
        {
            CalculateTotal();
            CalculateTotalReceipts();
        }

        public void CalculateTotal()
        {
            long total = 0;
            foreach (var b in Bookings) total += b.Total;
            Total = total;
        }

        public void CalculateTotalReceipts()
        {
            long total = 0;
            foreach (var r in Receipts) total += r.Money;
            TotalReceipts = total;
        }

        public Bill GetManaged()
        {
            var bill = BillBusiness.Get(Id);
            if (bill == null)
                throw new Exception("Mã hóa đơn không tồn tại");
            return bill;
        }
    }
}
