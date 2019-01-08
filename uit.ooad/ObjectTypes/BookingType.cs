using System.Linq;
using GraphQL.Types;
using uit.ooad.Models;
using uit.ooad.Queries.Base;

namespace uit.ooad.ObjectTypes
{
    public class BookingType : ObjectGraphType<Booking>
    {
        public BookingType()
        {
            Name = nameof(Booking);
            Description = "Một thông tin thuê phòng của khách hàng";

            Field(x => x.Id).Description("Id của thông tin thuê phòng");
            Field(x => x.BookCheckInTime, true).Description("Thời điểm nhận phòng dự kiến của khách hàng");
            Field(x => x.BookCheckOutTime, true).Description("Thời điểm trả phòng dự kiến của khách hàng");
            Field(x => x.RealCheckInTime, true).Description("Thời điểm nhận phòng của khách hàng");
            Field(x => x.RealCheckOutTime, true).Description("Thời điểm trả phòng của khách hàng");
            Field(x => x.CreateTime).Description("Thời điểm tạo thông tin thuê phòng");
            Field(x => x.Status).Description("Trạng thái của thông tin thuê phòng");
            Field(x => x.Total).Description("Tổng tiền của booking");
            Field(x => x.TotalRates).Description("Tổng tiền của booking");
            Field(x => x.TotalVolatilityRate).Description("Tổng tiền của booking");
            Field(x => x.TotalServicesDetails).Description("Tổng tiền của booking");

            Field<EmployeeType>(
                nameof(Booking.EmployeeBooking),
                "Nhân viên thực hiện giao dịch nhận đặt phòng từ khách hàng",
                resolve: context => context.Source.EmployeeBooking);
            Field<EmployeeType>(
                nameof(Booking.EmployeeCheckIn),
                "Nhân viên thực hiện check-in cho khách hàng",
                resolve: context => context.Source.EmployeeCheckIn);
            Field<EmployeeType>(
                nameof(Booking.EmployeeCheckOut),
                "Nhân viên thực hiện check-out cho khách hàng",
                resolve: context => context.Source.EmployeeCheckOut);

            Field<NonNullGraphType<BillType>>(
                nameof(Booking.Bill),
                "Thông tin hóa đơn của thông tin thuê phòng",
                resolve: context => context.Source.Bill);
            Field<NonNullGraphType<RoomType>>(
                nameof(Booking.Room),
                "Phòng khách hàng chọn đặt trước",
                resolve: context => context.Source.Room);

            Field<NonNullGraphType<ListGraphType<PatronType>>>(
                nameof(Booking.Patrons),
                "Danh sách khách hàng yêu cầu đặt phòng",
                resolve: context => context.Source.Patrons.ToList());

            Field<ListGraphType<HouseKeepingType>>(
                nameof(Booking.HouseKeepings),
                "Danh sách nhân viên dọn phòng cho phòng đã đặt này",
                resolve: context => context.Source.HouseKeepings.ToList());

            Field<ListGraphType<ServicesDetailType>>(
                nameof(Booking.ServicesDetails),
                "Danh sách chi tiết sử dụng dịch vụ của khách hàng",
                resolve: context => context.Source.ServicesDetails.ToList());
        }
    }

    public class BookingIdInput : InputType<Booking>
    {
        public BookingIdInput()
        {
            Name = _Id;
            Description = "Input cho một thông tin một đơn đặt phòng";
            Field(x => x.Id).Description("Id của một đơn đặt phòng");
        }
    }

    public class BookingCreateInput : InputType<Booking>
    {
        public BookingCreateInput()
        {
            Name = _Creation;

            Field(x => x.BookCheckInTime).Description("Thời điểm nhận phòng dự kiến của khách hàng");
            Field(x => x.BookCheckOutTime).Description("Thời điểm trả phòng dự kiến của khách hàng");

            Field<NonNullGraphType<RoomIdInput>>(
                nameof(Booking.Room),
                "Phòng khách hàng chọn đặt trước"
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<PatronIdInput>>>>(
                nameof(Booking.ListOfPatrons),
                "Danh sách khách hàng"
            );
        }
    }

    public class BookAndCheckInCreateInput : InputType<Booking>
    {
        public BookAndCheckInCreateInput()
        {
            Name = "BookAndCheckInCreateInput";

            Field(x => x.BookCheckOutTime).Description("Thời điểm trả phòng dự kiến của khách hàng");

            Field<NonNullGraphType<RoomIdInput>>(
                nameof(Booking.Room),
                "Phòng khách hàng chọn đặt trước"
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<PatronIdInput>>>>(
                nameof(Booking.ListOfPatrons),
                "Danh sách khách hàng"
            );
        }
    }
}