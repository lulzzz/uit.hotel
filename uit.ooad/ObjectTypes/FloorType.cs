﻿using System.Linq;
using GraphQL.Types;
using uit.ooad.Models;
using uit.ooad.Queries.Base;

namespace uit.ooad.ObjectTypes
{
    public class FloorType : ObjectGraphType<Floor>
    {
        public FloorType()
        {
            Name = nameof(Floor);
            Description = "Một tầng trong khách sạn";

            Field(x => x.Id).Description("Id của tầng");
            Field(x => x.Name).Description("Tên tầng");
            Field<ListGraphType<RoomType>>(
                nameof(Floor.Rooms),
                resolve: context => context.Source.Rooms.ToList(),
                description: "Danh sách các phòng có trong tầng"
            );
        }
    }

    public class FloorCreateInput : InputType<Floor>
    {
        public FloorCreateInput()
        {
            Name = _Creation;
            Field<NonNullGraphType<IntGraphType>>(nameof(Floor.Id));
            Field<NonNullGraphType<StringGraphType>>(nameof(Floor.Name));
        }
    }
}
