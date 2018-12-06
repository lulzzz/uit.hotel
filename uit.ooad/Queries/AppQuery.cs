﻿using uit.ooad.Queries.Query;

namespace uit.ooad.Queries
{
    public class AppQuery : RootObjectGraphType
    {
        public AppQuery()
        {
            AddFields(
                new RoomQuery(),
                new FloorQuery(),
                new EmployeeQuery()
            );
        }
    }
}
