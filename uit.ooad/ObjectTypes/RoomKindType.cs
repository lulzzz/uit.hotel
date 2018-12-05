using System.Linq;
using GraphQL.Types;
using uit.ooad.Models;

namespace uit.ooad.ObjectTypes
{
    public class RoomKindType : ObjectGraphType<RoomKind>
    {
        public RoomKindType()
        {
            Name = "RoomKind";
            Description = "Một loại phòng hiện có trong khách sạn";

            Field(x => x.Id).Description("Id của loại phòng");
            Field(x => x.Name).Description("Tên loại phòng");
            Field(x => x.NumberOfBeds).Description("Số giường");
            Field(x => x.AmountOfPeople).Description("Số người trong một phòng");
            Field(x => x.PriceByDate).Description("Giá theo ngày");

            Field<ListGraphType<RoomType>>("rooms", resolve: context => context.Source.Rooms.ToList(), description: "Danh sách các phòng thuộc loại phòng này");
            Field<ListGraphType<RateType>>("rates", resolve: context => context.Source.Rates.ToList(), description: "Danh sách giá cố định của loại phòng");
            Field<ListGraphType<VolatilityRateType>>("volatilityRates", resolve: context => context.Source.VolatilityRates.ToList(), description: "Danh sách giá biến động của loại phòng");
        }
    }
}
