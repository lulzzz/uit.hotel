using System.Linq;
using GraphQL.Types;
using uit.hotel.Models;
using uit.hotel.Queries.Base;

namespace uit.hotel.ObjectTypes
{
    public class BookingStatusEnumType : EnumerationGraphType<BookingStatusEnum>
    {
        public BookingStatusEnumType()
        {
            Name = nameof(BookingStatusEnum);
            Description = "Trạng thái của đơn đặt phòng";
        }
    }

    public class BookingType : ObjectGraphType<Booking>
    {
        public BookingType()
        {
            Name = nameof(Booking);
            Description = "Một thông tin thuê phòng của khách hàng";

            Field(x => x.Id).Description("Id của thông tin thuê phòng");
            Field(x => x.BookCheckInTime).Description("Thời điểm nhận phòng dự kiến của khách hàng");
            Field(x => x.BookCheckOutTime).Description("Thời điểm trả phòng dự kiến của khách hàng");
            Field(x => x.RealCheckInTime).Description("Thời điểm nhận phòng của khách hàng");
            Field(x => x.RealCheckOutTime).Description("Thời điểm trả phòng của khách hàng");
            Field(x => x.CreateTime).Description("Thời điểm tạo thông tin thuê phòng");
            Field<NonNullGraphType<BookingStatusEnumType>>(
                nameof(Booking.Status),
                "Trạng thái của thông tin thuê phòng"
            );
            Field(x => x.Total).Description("Tổng tiền");
            Field(x => x.TotalPrice).Description("Tổng tiền thuê");
            Field(x => x.TotalServicesDetails).Description("Tổng tiền dịch vụ");

            Field(x => x.BaseNightCheckInTime).Description("Thời gian nhận phòng, tính giá theo đêm");
            Field(x => x.BaseDayCheckInTime).Description("Thời gian nhận phòng, tính giá theo ngày");
            Field(x => x.BaseDayCheckOutTime).Description("Thời gian trả phòng");

            Field(x => x.EarlyCheckInFee).Description("Phí nhận phòng sớm");
            Field(x => x.LateCheckOutFee).Description("Phí trả phòng trễ");

            Field<EmployeeType>(
                nameof(Booking.EmployeeBooking),
                "Nhân viên thực hiện giao dịch nhận đặt phòng từ khách hàng",
                resolve: context => context.Source.EmployeeBooking
            );
            Field<EmployeeType>(
                nameof(Booking.EmployeeCheckIn),
                "Nhân viên thực hiện check-in cho khách hàng",
                resolve: context => context.Source.EmployeeCheckIn
            );
            Field<EmployeeType>(
                nameof(Booking.EmployeeCheckOut),
                "Nhân viên thực hiện check-out cho khách hàng",
                resolve: context => context.Source.EmployeeCheckOut
            );
            Field<NonNullGraphType<BillType>>(
                nameof(Booking.Bill),
                "Thông tin hóa đơn của thông tin thuê phòng",
                resolve: context => context.Source.Bill
            );
            Field<NonNullGraphType<RoomType>>(
                nameof(Booking.Room),
                "Phòng khách hàng chọn đặt trước",
                resolve: context => context.Source.Room
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<PatronType>>>>(
                nameof(Booking.Patrons),
                "Danh sách khách hàng yêu cầu đặt phòng",
                resolve: context => context.Source.Patrons.ToList()
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<ServicesDetailType>>>>(
                nameof(Booking.ServicesDetails),
                "Danh sách chi tiết sử dụng dịch vụ của khách hàng",
                resolve: context => context.Source.ServicesDetails.ToList()
            );
            Field<NonNullGraphType<PriceType>>(
                nameof(Booking.Price),
                "Công thức giá"
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<PriceItemType>>>>(
                nameof(Booking.PriceItems),
                "Danh sách đơn vị giá",
                resolve: context => context.Source.PriceItems.ToList()
            );
            Field<NonNullGraphType<ListGraphType<NonNullGraphType<PriceVolatilityItemType>>>>(
               nameof(Booking.PriceVolatilityItems),
               "Danh sách đơn vị giá biến động",
               resolve: context => context.Source.PriceVolatilityItems.ToList()
           );
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
